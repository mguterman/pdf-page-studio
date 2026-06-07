using System.Text.Json;
using System.Text.Json.Serialization;
using PdfiumViewer;
using System.Globalization;

namespace PdfPageStudio;

public sealed partial class MainForm : Form
{
    private const string ProjectExtension = ".ppsproj";
    private readonly JsonSerializerOptions _jsonOptions = CreateJsonOptions();
    private readonly AppSettings _settings;
    private PdfPageStudioProject _project = new();
    private PdfDocument? _pdfDocument;
    private ContextMenuStrip? _actionMenu;
    private string? _projectPath;
    private int _pageIndex;
    private float _zoomFactor = 1f;
    private bool _isDirty;
    private bool _isBinding;

    public MainForm()
    {
        _settings = AppSettings.Load();
        TranslationService.Initialize(_settings.Language);
        InitializeComponent();
        ApplyTranslations();
        InitializePdfRendering();
        RefreshRecentProjectsMenu();
        BindProject();
        LoadPdfFromProject();
        UpdateTitle();
        UpdateStatus(TranslationService.T("status.ready"));
    }

    private void OpenProjectMenuItem_Click(object? sender, EventArgs e)
    {
        if (!ConfirmSaveChanges())
        {
            return;
        }

        using var dialog = new OpenFileDialog
        {
            Title = TranslationService.T("dialog.openProject.title"),
            Filter = TranslationService.T("dialog.project.filter"),
            DefaultExt = "ppsproj",
            CheckFileExists = true,
            InitialDirectory = GetInitialProjectFolder(),
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        OpenProject(dialog.FileName);
    }

    private void SaveProjectMenuItem_Click(object? sender, EventArgs e)
    {
        SaveProject();
    }

    private void SaveProjectAsMenuItem_Click(object? sender, EventArgs e)
    {
        SaveProjectAs();
    }

    private void EnglishLanguageMenuItem_Click(object? sender, EventArgs e)
    {
        ChangeLanguage("en");
    }

    private void RussianLanguageMenuItem_Click(object? sender, EventArgs e)
    {
        ChangeLanguage("ru");
    }

    private void HebrewLanguageMenuItem_Click(object? sender, EventArgs e)
    {
        ChangeLanguage("he");
    }

    private void AddPdfFileMenuItem_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = TranslationService.T("dialog.addPdf.title"),
            Filter = TranslationService.T("dialog.pdf.filter"),
            CheckFileExists = true,
            InitialDirectory = GetInitialPdfFolder(),
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _project.PdfFilePath = dialog.FileName;
        _settings.LastPdfFolder = Path.GetDirectoryName(dialog.FileName) ?? "";
        _settings.Save();
        MarkDirty();
        LoadPdfFromProject();
    }

    private void ChangeLanguage(string language)
    {
        if (string.Equals(TranslationService.CurrentLanguage, language, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        TranslationService.SetLanguage(language);
        _settings.Language = TranslationService.CurrentLanguage;
        _settings.Save();
        ApplyTranslations();
        RefreshRecentProjectsMenu();
        RefreshActions(actionsListBox.SelectedIndex);
        UpdateTitle();
        if (_pdfDocument != null)
        {
            UpdatePdfNavigation();
            UpdatePageSizeLabel(_pdfDocument.PageSizes[_pageIndex]);
            UpdatePreviewStatus();
        }
        else
        {
            SetPdfToolbarEnabled(false);
        }
    }

    private void ApplyTranslations()
    {
        _isBinding = true;

        fileMenuItem.Text = TranslationService.T("menu.file");
        openProjectMenuItem.Text = TranslationService.T("menu.openProject");
        saveProjectMenuItem.Text = TranslationService.T("menu.save");
        saveProjectAsMenuItem.Text = TranslationService.T("menu.saveAs");
        addPdfFileMenuItem.Text = TranslationService.T("menu.addPdf");
        openRecentProjectMenuItem.Text = TranslationService.T("menu.openRecent");
        languageMenuItem.Text = TranslationService.T("menu.language");
        englishLanguageMenuItem.Text = TranslationService.T("language.english");
        russianLanguageMenuItem.Text = TranslationService.T("language.russian");
        hebrewLanguageMenuItem.Text = TranslationService.T("language.hebrew");
        englishLanguageMenuItem.Checked = TranslationService.CurrentLanguage == "en";
        russianLanguageMenuItem.Checked = TranslationService.CurrentLanguage == "ru";
        hebrewLanguageMenuItem.Checked = TranslationService.CurrentLanguage == "he";

        projectNameLabel.Text = TranslationService.T("project.name");
        projectUnitTypeLabel.Text = TranslationService.T("project.unitType");
        projectDescriptionLabel.Text = TranslationService.T("project.description");

        firstPageButton.Text = TranslationService.T("nav.firstPage");
        previousPageButton.Text = TranslationService.T("nav.previousPage");
        nextPageButton.Text = TranslationService.T("nav.nextPage");
        lastPageButton.Text = TranslationService.T("nav.lastPage");
        firstPageButton.ToolTipText = TranslationService.T("nav.firstPage.tooltip");
        previousPageButton.ToolTipText = TranslationService.T("nav.previousPage.tooltip");
        nextPageButton.ToolTipText = TranslationService.T("nav.nextPage.tooltip");
        lastPageButton.ToolTipText = TranslationService.T("nav.lastPage.tooltip");
        zoomOutButton.Text = TranslationService.T("nav.zoomOut");
        zoomInButton.Text = TranslationService.T("nav.zoomIn");
        zoomOutButton.ToolTipText = TranslationService.T("nav.zoomOut.tooltip");
        zoomInButton.ToolTipText = TranslationService.T("nav.zoomIn.tooltip");
        fitWidthButton.Text = TranslationService.T("nav.fitWidth");
        fitPageButton.Text = TranslationService.T("nav.fitPage");

        actionsTitleLabel.Text = TranslationService.T("actions.title");
        applyAllRadioButton.Text = TranslationService.T("actions.applyAll");
        untilCurrentRadioButton.Text = TranslationService.T("actions.untilCurrent");
        addActionButton.Text = TranslationService.T("actions.add");
        insertBeforeActionButton.Text = TranslationService.T("actions.insertBefore");
        insertAfterActionButton.Text = TranslationService.T("actions.insertAfter");
        deleteActionButton.Text = TranslationService.T("actions.delete");
        moveActionUpButton.Text = TranslationService.T("actions.up");
        moveActionDownButton.Text = TranslationService.T("actions.down");
        propertiesTitleLabel.Text = TranslationService.T("properties.title");

        actionTypeLabel.Text = TranslationService.T("field.type");
        actionNameLabel.Text = TranslationService.T("field.name");
        pageFilterTypeLabel.Text = TranslationService.T("field.pageType");
        pageFilterRangeLabel.Text = TranslationService.T("field.range");
        pageFilterRangeTextBox.PlaceholderText = TranslationService.T("field.range.placeholder");
        leftLabel.Text = TranslationService.T("field.left");
        topLabel.Text = TranslationService.T("field.top");
        rightLabel.Text = TranslationService.T("field.right");
        bottomLabel.Text = TranslationService.T("field.bottom");
        targetedWidthLabel.Text = TranslationService.T("field.width");
        targetedHeightLabel.Text = TranslationService.T("field.height");
        proportionalLabel.Text = TranslationService.T("field.proportional");
        anchorLabel.Text = TranslationService.T("field.anchor");
        rulerColorLabel.Text = TranslationService.T("field.color");
        rulerStyleLabel.Text = TranslationService.T("field.style");
        rulerValueModeLabel.Text = TranslationService.T("field.mode");
        rulerOrientationLabel.Text = TranslationService.T("field.line");
        rulerPositionLabel.Text = TranslationService.T("field.position");

        ConfigureEnumCombo(projectUnitTypeComboBox, "enum.unit.", _project.UnitType);
        ConfigureEnumCombo(actionTypeComboBox, "enum.action.", GetSelectedAction()?.Type ?? PdfActionType.Trim);
        ConfigureEnumCombo(pageFilterTypeComboBox, "enum.pageFilter.", GetSelectedAction()?.PageFilter?.Type ?? PageFilterType.Any);
        ConfigureEnumCombo(rulerStyleComboBox, "enum.rulerStyle.", GetSelectedAction()?.Style ?? RulerStyle.Solid);
        ConfigureEnumCombo(rulerValueModeComboBox, "enum.rulerMode.", GetSelectedAction()?.RulerValueMode ?? RulerValueMode.Percent);
        ConfigureEnumCombo(rulerOrientationComboBox, "enum.rulerOrientation.", GetSelectedAction()?.Orientation ?? RulerOrientation.Vertical);

        _isBinding = false;
        BindSelectedAction();
    }

    private void PreviousPageButton_Click(object? sender, EventArgs e)
    {
        if (_pdfDocument == null || _pageIndex == 0)
        {
            return;
        }

        _pageIndex--;
        RenderCurrentPage();
    }

    private void FirstPageButton_Click(object? sender, EventArgs e)
    {
        if (_pdfDocument == null || _pageIndex == 0)
        {
            return;
        }

        _pageIndex = 0;
        RenderCurrentPage();
    }

    private void NextPageButton_Click(object? sender, EventArgs e)
    {
        if (_pdfDocument == null || _pageIndex >= _pdfDocument.PageCount - 1)
        {
            return;
        }

        _pageIndex++;
        RenderCurrentPage();
    }

    private void LastPageButton_Click(object? sender, EventArgs e)
    {
        if (_pdfDocument == null || _pageIndex >= _pdfDocument.PageCount - 1)
        {
            return;
        }

        _pageIndex = _pdfDocument.PageCount - 1;
        RenderCurrentPage();
    }

    private void PageNumberTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
        {
            return;
        }

        e.SuppressKeyPress = true;
        GoToPageFromTextBox();
    }

    private void PageNumberTextBox_Leave(object? sender, EventArgs e)
    {
        GoToPageFromTextBox();
    }

    private void ZoomOutButton_Click(object? sender, EventArgs e)
    {
        _zoomFactor = Math.Max(0.1f, _zoomFactor / 1.25f);
        pdfPageViewer.CustomZoom = _zoomFactor;
        fitPageButton.Checked = false;
        fitWidthButton.Checked = false;
    }

    private void ZoomInButton_Click(object? sender, EventArgs e)
    {
        _zoomFactor = Math.Min(6f, _zoomFactor * 1.25f);
        pdfPageViewer.CustomZoom = _zoomFactor;
        fitPageButton.Checked = false;
        fitWidthButton.Checked = false;
    }

    private void FitWidthButton_Click(object? sender, EventArgs e)
    {
        pdfPageViewer.ZoomMode = PdfZoomMode.FitWidth;
        fitWidthButton.Checked = true;
        fitPageButton.Checked = false;
    }

    private void FitPageButton_Click(object? sender, EventArgs e)
    {
        pdfPageViewer.ZoomMode = PdfZoomMode.FitPage;
        fitPageButton.Checked = true;
        fitWidthButton.Checked = false;
    }

    private void ProjectNameTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding)
        {
            return;
        }

        _project.Name = projectNameTextBox.Text;
        MarkDirty();
    }

    private void ProjectDescriptionTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding)
        {
            return;
        }

        _project.Description = projectDescriptionTextBox.Text;
        MarkDirty();
    }

    private void AddActionButton_Click(object? sender, EventArgs e)
    {
        ShowAddActionMenu(addActionButton, _project.Actions.Count);
    }

    private void InsertBeforeActionButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex >= 0 ? actionsListBox.SelectedIndex : 0;
        ShowAddActionMenu(insertBeforeActionButton, index);
    }

    private void InsertAfterActionButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex >= 0 ? actionsListBox.SelectedIndex + 1 : _project.Actions.Count;
        ShowAddActionMenu(insertAfterActionButton, index);
    }

    private void DeleteActionButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex;
        if (index < 0 || index >= _project.Actions.Count)
        {
            return;
        }

        _project.Actions.RemoveAt(index);
        MarkDirty();
        RefreshActions(Math.Min(index, _project.Actions.Count - 1));
        UpdateStatus(TranslationService.T("status.actionDeleted"));
        RefreshCurrentPagePreview();
    }

    private void MoveActionUpButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex;
        if (index <= 0 || index >= _project.Actions.Count)
        {
            return;
        }

        (_project.Actions[index - 1], _project.Actions[index]) = (_project.Actions[index], _project.Actions[index - 1]);
        MarkDirty();
        RefreshActions(index - 1);
        UpdateStatus(TranslationService.T("status.actionMovedUp"));
        RefreshCurrentPagePreview();
    }

    private void MoveActionDownButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex;
        if (index < 0 || index >= _project.Actions.Count - 1)
        {
            return;
        }

        (_project.Actions[index + 1], _project.Actions[index]) = (_project.Actions[index], _project.Actions[index + 1]);
        MarkDirty();
        RefreshActions(index + 1);
        UpdateStatus(TranslationService.T("status.actionMovedDown"));
        RefreshCurrentPagePreview();
    }

    private void ActionsListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        BindSelectedAction();
        UpdateActionButtons();
        if (!_isBinding && _project.PreviewApplyMode == PreviewApplyMode.UntilCurrent)
        {
            RefreshCurrentPagePreview();
        }
    }

    private void PreviewModeRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isBinding)
        {
            return;
        }

        var mode = untilCurrentRadioButton.Checked ? PreviewApplyMode.UntilCurrent : PreviewApplyMode.ApplyAll;
        if (_project.PreviewApplyMode == mode)
        {
            return;
        }

        _project.PreviewApplyMode = mode;
        MarkDirty();
        RefreshCurrentPagePreview();
    }

    private void ActionNameTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        action.Name = actionNameTextBox.Text;
        MarkDirty();
        RefreshActions(actionsListBox.SelectedIndex);
        RefreshCurrentPagePreview();
    }

    private void UnitTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || !TryGetSelectedEnum(projectUnitTypeComboBox, out UnitType newUnit) || newUnit == _project.UnitType)
        {
            return;
        }

        var oldUnit = _project.UnitType;
        _project.UnitType = newUnit;
        ConvertProjectUnits(oldUnit, newUnit);
        MarkDirty();
        SyncUnitCombos();
        BindSelectedAction();
        if (_pdfDocument != null)
        {
            UpdatePageSizeLabel(_pdfDocument.PageSizes[_pageIndex]);
        }
    }

    private void ActionTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (TryGetSelectedEnum(actionTypeComboBox, out PdfActionType type))
        {
            action.Type = type;
            ApplyActionDefaults(action, overwriteName: true);
            MarkDirty();
            RefreshActions(actionsListBox.SelectedIndex);
            RefreshCurrentPagePreview();
        }
    }

    private void PageFilterTypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (TryGetSelectedEnum(pageFilterTypeComboBox, out PageFilterType type))
        {
            action.PageFilter.Type = type;
            MarkDirty();
            RefreshCurrentPagePreview();
        }
    }

    private void PageFilterRangeTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (!TryParsePageRanges(pageFilterRangeTextBox.Text, out var ranges, out var error))
        {
            UpdateStatus(error);
            return;
        }

        action.PageFilter.Range = ranges;
        MarkDirty();
        UpdateStatus(TranslationService.T("status.pageFilterUpdated"));
        RefreshCurrentPagePreview();
    }

    private void ActionNumberNumericBox_ValueChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action || sender is not NumericUpDown numericBox)
        {
            return;
        }

        var value = (float)numericBox.Value;
        if (numericBox == leftNumericBox) action.Left = value;
        else if (numericBox == topNumericBox) action.Top = value;
        else if (numericBox == rightNumericBox) action.Right = value;
        else if (numericBox == bottomNumericBox) action.Bottom = value;
        else if (numericBox == targetedWidthNumericBox) action.TargetedWidth = ZeroToNull(value);
        else if (numericBox == targetedHeightNumericBox) action.TargetedHeight = ZeroToNull(value);
        else if (numericBox == rulerPositionNumericBox) action.Position = value;
        else return;

        MarkDirty();
        RefreshCurrentPagePreview();
    }

    private void ProportionalCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        action.Proportional = proportionalCheckBox.Checked;
        MarkDirty();
        RefreshCurrentPagePreview();
    }

    private void AnchorRadioButton_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action || sender is not RadioButton { Checked: true } radioButton)
        {
            return;
        }

        (action.AnchorHorizontal, action.AnchorVertical) = radioButton switch
        {
            var button when button == anchorTopLeftRadioButton => (AnchorHorizontal.Left, AnchorVertical.Top),
            var button when button == anchorTopCenterRadioButton => (AnchorHorizontal.Center, AnchorVertical.Top),
            var button when button == anchorTopRightRadioButton => (AnchorHorizontal.Right, AnchorVertical.Top),
            var button when button == anchorMiddleLeftRadioButton => (AnchorHorizontal.Left, AnchorVertical.Center),
            var button when button == anchorMiddleRightRadioButton => (AnchorHorizontal.Right, AnchorVertical.Center),
            var button when button == anchorBottomLeftRadioButton => (AnchorHorizontal.Left, AnchorVertical.Bottom),
            var button when button == anchorBottomCenterRadioButton => (AnchorHorizontal.Center, AnchorVertical.Bottom),
            var button when button == anchorBottomRightRadioButton => (AnchorHorizontal.Right, AnchorVertical.Bottom),
            _ => (AnchorHorizontal.Center, AnchorVertical.Center),
        };
        MarkDirty();
        RefreshCurrentPagePreview();
    }

    private void RulerColorTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        action.Color = rulerColorTextBox.Text;
        MarkDirty();
        RefreshCurrentPagePreview();
    }

    private void RulerStyleComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (TryGetSelectedEnum(rulerStyleComboBox, out RulerStyle style))
        {
            action.Style = style;
            MarkDirty();
            RefreshCurrentPagePreview();
        }
    }

    private void RulerValueModeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (TryGetSelectedEnum(rulerValueModeComboBox, out RulerValueMode mode))
        {
            action.RulerValueMode = mode;
            MarkDirty();
            RefreshCurrentPagePreview();
        }
    }

    private void RulerOrientationComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action)
        {
            return;
        }

        if (TryGetSelectedEnum(rulerOrientationComboBox, out RulerOrientation orientation))
        {
            action.Orientation = orientation;
            MarkDirty();
            RefreshCurrentPagePreview();
        }
    }

    private void OpenProject(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            _project = JsonSerializer.Deserialize<PdfPageStudioProject>(json, _jsonOptions) ?? new PdfPageStudioProject();
            NormalizeProject();
            _projectPath = path;
            _isDirty = false;
            _settings.AddRecentProject(path);
            _settings.Save();
            BindProject();
            LoadPdfFromProject();
            RefreshRecentProjectsMenu();
            UpdateTitle();
            UpdateStatus(TranslationService.T("status.opened", path));
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, TranslationService.T("message.openProjectFailed.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus(TranslationService.T("status.openProjectFailed"));
        }
    }

    private bool SaveProject()
    {
        if (string.IsNullOrWhiteSpace(_projectPath))
        {
            return SaveProjectAs();
        }

        return SaveProjectTo(_projectPath);
    }

    private bool SaveProjectAs()
    {
        using var dialog = new SaveFileDialog
        {
            Title = TranslationService.T("dialog.saveProject.title"),
            Filter = TranslationService.T("dialog.project.filter"),
            DefaultExt = "ppsproj",
            AddExtension = true,
            OverwritePrompt = true,
            InitialDirectory = GetInitialProjectFolder(),
            FileName = GetDefaultProjectFileName(),
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return false;
        }

        var path = EnsureProjectExtension(dialog.FileName);
        return SaveProjectTo(path);
    }

    private bool SaveProjectTo(string path)
    {
        try
        {
            var folder = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var json = JsonSerializer.Serialize(_project, _jsonOptions);
            File.WriteAllText(path, json);
            _projectPath = path;
            _isDirty = false;
            _settings.AddRecentProject(path);
            _settings.Save();
            RefreshRecentProjectsMenu();
            UpdateTitle();
            UpdateStatus(TranslationService.T("status.saved", path));
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, TranslationService.T("message.saveProjectFailed.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus(TranslationService.T("status.saveProjectFailed"));
            return false;
        }
    }

    private void BindProject()
    {
        NormalizeProject();
        _isBinding = true;
        projectNameTextBox.Text = _project.Name;
        projectDescriptionTextBox.Text = _project.Description;
        SyncUnitCombos();
        applyAllRadioButton.Checked = _project.PreviewApplyMode == PreviewApplyMode.ApplyAll;
        untilCurrentRadioButton.Checked = _project.PreviewApplyMode == PreviewApplyMode.UntilCurrent;
        _isBinding = false;
        RefreshActions(actionsListBox.SelectedIndex);
    }

    private void InitializePdfRendering()
    {
        try
        {
            PdfiumNativeLoader.EnsureLoaded();
        }
        catch (Exception ex)
        {
            addPdfFileMenuItem.Enabled = false;
            SetPdfToolbarEnabled(false);
            MessageBox.Show(this, ex.Message, TranslationService.T("message.previewInitFailed.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadPdfFromProject()
    {
        _pdfDocument?.Dispose();
        _pdfDocument = null;
        _pageIndex = 0;
        pdfPageViewer.SetPage(null);
        SetPdfToolbarEnabled(false);

        if (string.IsNullOrWhiteSpace(_project.PdfFilePath))
        {
            ShowProjectInfo();
            return;
        }

        if (!File.Exists(_project.PdfFilePath))
        {
            ShowProjectInfo();
            UpdateStatus(TranslationService.T("status.pdfNotFound", _project.PdfFilePath));
            return;
        }

        try
        {
            _pdfDocument = PdfDocument.Load(_project.PdfFilePath);
            ShowPdfWorkspace();
            SetPdfToolbarEnabled(true);
            FitPageButton_Click(this, EventArgs.Empty);
            RenderCurrentPage();
            UpdateStatus(TranslationService.T("status.pdfLoaded", _project.PdfFilePath));
        }
        catch (Exception ex)
        {
            ShowProjectInfo();
            MessageBox.Show(this, ex.Message, TranslationService.T("message.pdfLoadFailed.title"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus(TranslationService.T("status.pdfLoadFailed"));
        }
    }

    private void RenderCurrentPage()
    {
        if (_pdfDocument == null)
        {
            return;
        }

        var pageSize = _pdfDocument.PageSizes[_pageIndex];
        const float renderDpi = 144f;
        var width = Math.Max(1, (int)Math.Round(pageSize.Width / 72f * renderDpi));
        var height = Math.Max(1, (int)Math.Round(pageSize.Height / 72f * renderDpi));
        using var image = _pdfDocument.Render(_pageIndex, width, height, renderDpi, renderDpi, PdfRenderFlags.Annotations);
        var previewImage = ApplyPreviewActions(image, pageSize, renderDpi, out var previewPageSize);
        pdfPageViewer.SetPage(previewImage);
        UpdatePdfNavigation();
        UpdatePageSizeLabel(previewPageSize);
        UpdatePreviewStatus();
    }

    private void RefreshCurrentPagePreview()
    {
        if (_pdfDocument == null)
        {
            return;
        }

        RenderCurrentPage();
    }

    private void GoToPageFromTextBox()
    {
        if (_pdfDocument == null)
        {
            return;
        }

        if (!int.TryParse(pageNumberTextBox.Text, out var pageNumber))
        {
            UpdatePdfNavigation();
            return;
        }

        pageNumber = Math.Clamp(pageNumber, 1, _pdfDocument.PageCount);
        if (pageNumber - 1 == _pageIndex)
        {
            UpdatePdfNavigation();
            return;
        }

        _pageIndex = pageNumber - 1;
        RenderCurrentPage();
    }

    private void UpdatePdfNavigation()
    {
        var hasDocument = _pdfDocument != null;
        pageNumberTextBox.Text = hasDocument ? (_pageIndex + 1).ToString() : "";
        pageCountLabel.Text = hasDocument ? TranslationService.T("nav.pageCount", _pdfDocument!.PageCount) : TranslationService.T("nav.pageCount.empty");
        firstPageButton.Enabled = hasDocument && _pageIndex > 0;
        previousPageButton.Enabled = hasDocument && _pageIndex > 0;
        nextPageButton.Enabled = hasDocument && _pageIndex < _pdfDocument!.PageCount - 1;
        lastPageButton.Enabled = hasDocument && _pageIndex < _pdfDocument!.PageCount - 1;
        pageSizeLabel.Text = hasDocument ? pageSizeLabel.Text : TranslationService.T("page.size.empty");
    }

    private void SetPdfToolbarEnabled(bool enabled)
    {
        pageNumberTextBox.Enabled = enabled;
        zoomOutButton.Enabled = enabled;
        zoomInButton.Enabled = enabled;
        fitWidthButton.Enabled = enabled;
        fitPageButton.Enabled = enabled;
        firstPageButton.Enabled = false;
        previousPageButton.Enabled = false;
        nextPageButton.Enabled = false;
        lastPageButton.Enabled = false;
        pageCountLabel.Text = enabled && _pdfDocument != null ? TranslationService.T("nav.pageCount", _pdfDocument.PageCount) : TranslationService.T("nav.pageCount.empty");
        if (!enabled)
        {
            pageSizeLabel.Text = TranslationService.T("page.size.empty");
        }
    }

    private void UpdatePageSizeLabel(SizeF pageSizePoints)
    {
        var widthInches = pageSizePoints.Width / 72f;
        var heightInches = pageSizePoints.Height / 72f;
        var width = _project.UnitType == UnitType.Cm ? widthInches * 2.54f : widthInches;
        var height = _project.UnitType == UnitType.Cm ? heightInches * 2.54f : heightInches;
        var unit = _project.UnitType == UnitType.Cm ? "cm" : "in";
        pageSizeLabel.Text = TranslationService.T("page.size", width, height, unit);
    }

    private void ShowProjectInfo()
    {
        pdfWorkspacePanel.Visible = false;
        projectInfoPanel.Visible = true;
        projectInfoPanel.BringToFront();
    }

    private void ShowPdfWorkspace()
    {
        projectInfoPanel.Visible = false;
        pdfWorkspacePanel.Visible = true;
        pdfWorkspacePanel.BringToFront();
    }

    private void InsertAction(int index, PdfActionType type)
    {
        var action = new ProjectAction { Type = type };
        ApplyActionDefaults(action, overwriteName: true);
        index = Math.Clamp(index, 0, _project.Actions.Count);
        _project.Actions.Insert(index, action);
        MarkDirty();
        RefreshActions(index);
        UpdateStatus(TranslationService.T("status.actionAdded"));
        RefreshCurrentPagePreview();
    }

    private void ShowAddActionMenu(Control owner, int insertIndex)
    {
        _actionMenu?.Dispose();
        _actionMenu = new ContextMenuStrip();
        foreach (var actionType in Enum.GetValues<PdfActionType>())
        {
            var item = new ToolStripMenuItem(GetActionDisplayName(actionType))
            {
                Tag = actionType,
            };
            item.Click += (_, _) => InsertAction(insertIndex, actionType);
            _actionMenu.Items.Add(item);
        }

        _actionMenu.Show(owner, new Point(0, owner.Height));
    }

    private void NormalizeProject()
    {
        _project.Actions ??= [];
        foreach (var action in _project.Actions)
        {
            if (string.IsNullOrWhiteSpace(action.Id))
            {
                action.Id = Guid.NewGuid().ToString("N");
            }

            if (string.IsNullOrWhiteSpace(action.Name))
            {
                ApplyActionDefaults(action, overwriteName: true);
            }

            action.PageFilter ??= new PageFilter();
            action.PageFilter.Range ??= [];
            ApplyActionDefaults(action, overwriteName: false);
        }
    }

    private void RefreshActions(int selectedIndex)
    {
        _isBinding = true;
        actionsListBox.Items.Clear();
        for (var index = 0; index < _project.Actions.Count; index++)
        {
            actionsListBox.Items.Add(FormatActionListItem(index, _project.Actions[index]));
        }

        if (_project.Actions.Count > 0)
        {
            actionsListBox.SelectedIndex = Math.Clamp(selectedIndex, 0, _project.Actions.Count - 1);
        }

        _isBinding = false;
        BindSelectedAction();
        UpdateActionButtons();
    }

    private void BindSelectedAction()
    {
        _isBinding = true;
        var action = GetSelectedAction();
        var hasAction = action != null;
        propertiesTitleLabel.Visible = hasAction;
        actionPropertiesScrollPanel.Visible = hasAction;
        actionNameTextBox.Enabled = hasAction;
        actionTypeComboBox.Enabled = hasAction;
        pageFilterTypeComboBox.Enabled = hasAction;
        pageFilterRangeTextBox.Enabled = hasAction;

        if (action == null)
        {
            actionNameTextBox.Text = "";
            actionTypeComboBox.SelectedIndex = -1;
            pageFilterTypeComboBox.SelectedIndex = -1;
            pageFilterRangeTextBox.Text = "";
            ClearActionParameterInputs();
            UpdateActionEditorVisibility(null);
            _isBinding = false;
            return;
        }

        action.PageFilter ??= new PageFilter();
        SelectEnum(actionTypeComboBox, action.Type);
        actionNameTextBox.Text = action.Name;
        SelectEnum(pageFilterTypeComboBox, action.PageFilter.Type);
        pageFilterRangeTextBox.Text = FormatPageRanges(action.PageFilter.Range);
        leftNumericBox.Value = FloatToDecimal(action.Left);
        topNumericBox.Value = FloatToDecimal(action.Top);
        rightNumericBox.Value = FloatToDecimal(action.Right);
        bottomNumericBox.Value = FloatToDecimal(action.Bottom);
        targetedWidthNumericBox.Value = FloatToDecimal(action.TargetedWidth);
        targetedHeightNumericBox.Value = FloatToDecimal(action.TargetedHeight);
        proportionalCheckBox.Checked = action.Proportional ?? true;
        rulerColorTextBox.Text = action.Color;
        SelectEnum(rulerStyleComboBox, action.Style);
        SelectEnum(rulerValueModeComboBox, action.RulerValueMode);
        SelectEnum(rulerOrientationComboBox, action.Orientation);
        rulerPositionNumericBox.Value = FloatToDecimal(action.Position);
        SelectAnchorButton(action.AnchorHorizontal, action.AnchorVertical);
        UpdateActionEditorVisibility(action);
        _isBinding = false;
    }

    private void UpdateActionButtons()
    {
        var selectedIndex = actionsListBox.SelectedIndex;
        var hasSelection = selectedIndex >= 0 && selectedIndex < _project.Actions.Count;
        insertBeforeActionButton.Enabled = hasSelection || _project.Actions.Count == 0;
        insertAfterActionButton.Enabled = hasSelection || _project.Actions.Count == 0;
        deleteActionButton.Enabled = hasSelection;
        moveActionUpButton.Enabled = hasSelection && selectedIndex > 0;
        moveActionDownButton.Enabled = hasSelection && selectedIndex < _project.Actions.Count - 1;
    }

    private void UpdatePreviewStatus()
    {
        if (_pdfDocument == null)
        {
            return;
        }

        var count = GetPreviewActionCount();
        var label = _project.PreviewApplyMode == PreviewApplyMode.UntilCurrent
            ? TranslationService.T("status.previewUntilCurrent", count)
            : TranslationService.T("status.previewApplyAll", count);
        UpdateStatus(label);
    }

    private int GetPreviewActionCount()
    {
        if (_project.PreviewApplyMode == PreviewApplyMode.ApplyAll)
        {
            return _project.Actions.Count;
        }

        var selectedIndex = actionsListBox.SelectedIndex;
        return selectedIndex < 0 ? 0 : Math.Min(selectedIndex + 1, _project.Actions.Count);
    }

    private Bitmap ApplyPreviewActions(Image originalImage, SizeF originalPageSizePoints, float dpi, out SizeF previewPageSizePoints)
    {
        var current = new Bitmap(originalImage);
        previewPageSizePoints = originalPageSizePoints;
        var actionCount = GetPreviewActionCount();
        var pageNumber = _pageIndex + 1;

        for (var index = 0; index < actionCount; index++)
        {
            var action = _project.Actions[index];
            if (!PageFilterEvaluator.AppliesToPage(action.PageFilter, pageNumber))
            {
                continue;
            }

            var next = ApplyPreviewAction(current, action, dpi, ref previewPageSizePoints);
            if (!ReferenceEquals(next, current))
            {
                current.Dispose();
                current = next;
            }
        }

        return current;
    }

    private Bitmap ApplyPreviewAction(Bitmap source, ProjectAction action, float dpi, ref SizeF pageSizePoints)
    {
        return action.Type switch
        {
            PdfActionType.Trim => ApplyTrimPreview(source, action, dpi, ref pageSizePoints),
            PdfActionType.Expand => ApplyExpandPreview(source, action, dpi, ref pageSizePoints),
            PdfActionType.Resize => ApplyResizePreview(source, action, dpi, ref pageSizePoints),
            PdfActionType.Zoom => ApplyZoomPreview(source, action),
            PdfActionType.AddRuler => ApplyRulerPreview(source, action, dpi),
            _ => new Bitmap(source),
        };
    }

    private Bitmap ApplyTrimPreview(Bitmap source, ProjectAction action, float dpi, ref SizeF pageSizePoints)
    {
        var left = ToPixels(action.Left, dpi);
        var top = ToPixels(action.Top, dpi);
        var right = ToPixels(action.Right, dpi);
        var bottom = ToPixels(action.Bottom, dpi);
        left = Math.Clamp(left, 0, Math.Max(0, source.Width - 1));
        top = Math.Clamp(top, 0, Math.Max(0, source.Height - 1));
        right = Math.Clamp(right, 0, Math.Max(0, source.Width - left - 1));
        bottom = Math.Clamp(bottom, 0, Math.Max(0, source.Height - top - 1));

        var width = Math.Max(1, source.Width - left - right);
        var height = Math.Max(1, source.Height - top - bottom);
        var result = CreateCanvas(width, height);
        using var graphics = Graphics.FromImage(result);
        ConfigureHighQuality(graphics);
        graphics.DrawImage(source, new Rectangle(-left, -top, source.Width, source.Height));

        pageSizePoints = new SizeF(
            Math.Max(1, pageSizePoints.Width - UnitValueToPoints(action.Left) - UnitValueToPoints(action.Right)),
            Math.Max(1, pageSizePoints.Height - UnitValueToPoints(action.Top) - UnitValueToPoints(action.Bottom)));
        return result;
    }

    private Bitmap ApplyExpandPreview(Bitmap source, ProjectAction action, float dpi, ref SizeF pageSizePoints)
    {
        var left = ToPixels(action.Left, dpi);
        var top = ToPixels(action.Top, dpi);
        var right = ToPixels(action.Right, dpi);
        var bottom = ToPixels(action.Bottom, dpi);
        var width = Math.Max(1, source.Width + left + right);
        var height = Math.Max(1, source.Height + top + bottom);
        var result = CreateCanvas(width, height);
        using var graphics = Graphics.FromImage(result);
        ConfigureHighQuality(graphics);
        graphics.DrawImage(source, new Rectangle(left, top, source.Width, source.Height));

        pageSizePoints = new SizeF(
            pageSizePoints.Width + UnitValueToPoints(action.Left) + UnitValueToPoints(action.Right),
            pageSizePoints.Height + UnitValueToPoints(action.Top) + UnitValueToPoints(action.Bottom));
        return result;
    }

    private Bitmap ApplyResizePreview(Bitmap source, ProjectAction action, float dpi, ref SizeF pageSizePoints)
    {
        var currentWidthInches = pageSizePoints.Width / 72f;
        var currentHeightInches = pageSizePoints.Height / 72f;
        float? targetWidthInches = action.TargetedWidth is > 0 ? UnitValueToInches(action.TargetedWidth.Value) : null;
        float? targetHeightInches = action.TargetedHeight is > 0 ? UnitValueToInches(action.TargetedHeight.Value) : null;

        if ((targetWidthInches == null || targetWidthInches <= 0) && (targetHeightInches == null || targetHeightInches <= 0))
        {
            return new Bitmap(source);
        }

        if (action.Proportional != false)
        {
            if ((targetWidthInches == null || targetWidthInches <= 0) && targetHeightInches > 0)
            {
                targetWidthInches = currentWidthInches * targetHeightInches.Value / currentHeightInches;
            }
            else if ((targetHeightInches == null || targetHeightInches <= 0) && targetWidthInches > 0)
            {
                targetHeightInches = currentHeightInches * targetWidthInches.Value / currentWidthInches;
            }
        }

        targetWidthInches = targetWidthInches is > 0 ? targetWidthInches : currentWidthInches;
        targetHeightInches = targetHeightInches is > 0 ? targetHeightInches : currentHeightInches;
        var width = Math.Max(1, (int)Math.Round(targetWidthInches.Value * dpi));
        var height = Math.Max(1, (int)Math.Round(targetHeightInches.Value * dpi));
        var result = CreateCanvas(width, height);
        using var graphics = Graphics.FromImage(result);
        ConfigureHighQuality(graphics);
        graphics.DrawImage(source, new Rectangle(0, 0, width, height));
        pageSizePoints = new SizeF(targetWidthInches.Value * 72f, targetHeightInches.Value * 72f);
        return result;
    }

    private Bitmap ApplyZoomPreview(Bitmap source, ProjectAction action)
    {
        var widthPercent = action.TargetedWidth;
        var heightPercent = action.TargetedHeight;
        if ((widthPercent == null || widthPercent <= 0) && (heightPercent == null || heightPercent <= 0))
        {
            return new Bitmap(source);
        }

        if (action.Proportional != false)
        {
            if ((widthPercent == null || widthPercent <= 0) && heightPercent > 0)
            {
                widthPercent = heightPercent;
            }
            else if ((heightPercent == null || heightPercent <= 0) && widthPercent > 0)
            {
                heightPercent = widthPercent;
            }
        }

        var scaleX = Math.Max(0.001f, (widthPercent ?? 100f) / 100f);
        var scaleY = Math.Max(0.001f, (heightPercent ?? 100f) / 100f);
        var contentWidth = Math.Max(1, (int)Math.Round(source.Width * scaleX));
        var contentHeight = Math.Max(1, (int)Math.Round(source.Height * scaleY));
        var x = GetAnchorOffset(source.Width, contentWidth, action.AnchorHorizontal);
        var y = GetAnchorOffset(source.Height, contentHeight, action.AnchorVertical);
        var result = CreateCanvas(source.Width, source.Height);
        using var graphics = Graphics.FromImage(result);
        ConfigureHighQuality(graphics);
        graphics.DrawImage(source, new Rectangle(x, y, contentWidth, contentHeight));
        return result;
    }

    private Bitmap ApplyRulerPreview(Bitmap source, ProjectAction action, float dpi)
    {
        var result = new Bitmap(source);
        var color = ParseColor(action.Color);
        using var graphics = Graphics.FromImage(result);
        using var pen = new Pen(color, 2f);
        if (action.Style == RulerStyle.Dotted)
        {
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }

        var position = action.Position ?? 0;
        if (action.Orientation == RulerOrientation.Vertical)
        {
            var x = action.RulerValueMode == RulerValueMode.Percent
                ? (int)Math.Round(source.Width * position / 100f)
                : ToPixels(position, dpi);
            x = Math.Clamp(x, 0, source.Width - 1);
            graphics.DrawLine(pen, x, 0, x, source.Height);
        }
        else
        {
            var y = action.RulerValueMode == RulerValueMode.Percent
                ? (int)Math.Round(source.Height * position / 100f)
                : ToPixels(position, dpi);
            y = Math.Clamp(y, 0, source.Height - 1);
            graphics.DrawLine(pen, 0, y, source.Width, y);
        }

        return result;
    }

    private static int GetAnchorOffset(int canvasSize, int contentSize, AnchorHorizontal anchor)
    {
        return anchor switch
        {
            AnchorHorizontal.Left => 0,
            AnchorHorizontal.Right => canvasSize - contentSize,
            _ => (canvasSize - contentSize) / 2,
        };
    }

    private static int GetAnchorOffset(int canvasSize, int contentSize, AnchorVertical anchor)
    {
        return anchor switch
        {
            AnchorVertical.Top => 0,
            AnchorVertical.Bottom => canvasSize - contentSize,
            _ => (canvasSize - contentSize) / 2,
        };
    }

    private static Bitmap CreateCanvas(int width, int height)
    {
        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        return bitmap;
    }

    private static void ConfigureHighQuality(Graphics graphics)
    {
        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    }

    private static Color ParseColor(string color)
    {
        try
        {
            return ColorTranslator.FromHtml(string.IsNullOrWhiteSpace(color) ? "#FF0000" : color);
        }
        catch
        {
            return Color.Red;
        }
    }

    private ProjectAction? GetSelectedAction()
    {
        var index = actionsListBox.SelectedIndex;
        return index >= 0 && index < _project.Actions.Count ? _project.Actions[index] : null;
    }

    private static string FormatActionListItem(int index, ProjectAction action)
    {
        return $"{index + 1}. {action.Name} [{GetActionDisplayName(action.Type)}] ({FormatPageFilter(action.PageFilter)})";
    }

    private static string FormatPageFilter(PageFilter? filter)
    {
        if (filter == null)
        {
            return TranslationService.T("pageFilter.all");
        }

        var range = filter.Range.Count == 0 ? TranslationService.T("pageFilter.all") : FormatPageRanges(filter.Range);
        return filter.Type == PageFilterType.Any ? range : $"{range}, {TranslationService.T("enum.pageFilter." + filter.Type)}";
    }

    private static string FormatPageRanges(IReadOnlyList<PageFilterRange> ranges)
    {
        if (ranges.Count == 0)
        {
            return "";
        }

        return string.Join(", ", ranges.Select(range =>
            range.End == null
                ? $"{range.Start}-"
                : range.Start == range.End.Value
                    ? range.Start.ToString()
                    : $"{range.Start}-{range.End.Value}"));
    }

    private static bool TryParsePageRanges(string text, out List<PageFilterRange> ranges, out string error)
    {
        ranges = [];
        error = "";

        if (string.IsNullOrWhiteSpace(text))
        {
            return true;
        }

        var parts = text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var part in parts)
        {
            var dashIndex = part.IndexOf('-', StringComparison.Ordinal);
            if (dashIndex < 0)
            {
                if (!int.TryParse(part, out var page) || page < 1)
                {
                    error = TranslationService.T("error.pageRangePositive");
                    return false;
                }

                ranges.Add(new PageFilterRange { Start = page, End = page });
                continue;
            }

            var startText = part[..dashIndex].Trim();
            var endText = part[(dashIndex + 1)..].Trim();
            if (!int.TryParse(startText, out var start) || start < 1)
            {
                error = TranslationService.T("error.pageRangeStart");
                return false;
            }

            int? end = null;
            if (!string.IsNullOrWhiteSpace(endText))
            {
                if (!int.TryParse(endText, out var parsedEnd) || parsedEnd < start)
                {
                    error = TranslationService.T("error.pageRangeEnd");
                    return false;
                }

                end = parsedEnd;
            }

            ranges.Add(new PageFilterRange { Start = start, End = end });
        }

        return true;
    }

    private void ApplyActionDefaults(ProjectAction action, bool overwriteName)
    {
        if (overwriteName)
        {
            action.Name = action.Type switch
            {
                _ => GetActionDisplayName(action.Type),
            };
        }

        switch (action.Type)
        {
            case PdfActionType.Trim:
            case PdfActionType.Expand:
                action.Left ??= 0;
                action.Top ??= 0;
                action.Right ??= 0;
                action.Bottom ??= 0;
                break;
            case PdfActionType.Resize:
                action.Proportional ??= true;
                break;
            case PdfActionType.Zoom:
                action.Proportional ??= true;
                action.AnchorHorizontal = action.AnchorHorizontal;
                action.AnchorVertical = action.AnchorVertical;
                break;
            case PdfActionType.AddRuler:
                action.Color = string.IsNullOrWhiteSpace(action.Color) ? "#FF0000" : action.Color;
                action.Style = action.Style;
                action.RulerValueMode = action.RulerValueMode;
                action.Orientation = action.Orientation;
                action.Position ??= action.RulerValueMode == RulerValueMode.Percent ? 50 : 0;
                break;
        }
    }

    private static string GetActionDisplayName(PdfActionType type)
    {
        return TranslationService.T("enum.action." + type);
    }

    private void UpdateActionEditorVisibility(ProjectAction? action)
    {
        var type = action?.Type;
        var hasMargins = type is PdfActionType.Trim or PdfActionType.Expand;
        var hasTargetSize = type is PdfActionType.Resize or PdfActionType.Zoom;
        var hasProportional = type is PdfActionType.Resize or PdfActionType.Zoom;
        var hasAnchor = type == PdfActionType.Zoom;
        var hasRuler = type == PdfActionType.AddRuler;

        SetEditorRowVisible(leftLabel, leftNumericBox, hasMargins);
        SetEditorRowVisible(topLabel, topNumericBox, hasMargins);
        SetEditorRowVisible(rightLabel, rightNumericBox, hasMargins);
        SetEditorRowVisible(bottomLabel, bottomNumericBox, hasMargins);
        SetEditorRowVisible(targetedWidthLabel, targetedWidthNumericBox, hasTargetSize);
        SetEditorRowVisible(targetedHeightLabel, targetedHeightNumericBox, hasTargetSize);
        SetEditorRowVisible(proportionalLabel, proportionalCheckBox, hasProportional);
        SetEditorRowVisible(anchorLabel, anchorPanel, hasAnchor);
        SetEditorRowVisible(rulerColorLabel, rulerColorTextBox, hasRuler);
        SetEditorRowVisible(rulerStyleLabel, rulerStyleComboBox, hasRuler);
        SetEditorRowVisible(rulerValueModeLabel, rulerValueModeComboBox, hasRuler);
        SetEditorRowVisible(rulerOrientationLabel, rulerOrientationComboBox, hasRuler);
        SetEditorRowVisible(rulerPositionLabel, rulerPositionNumericBox, hasRuler);
        actionPropertiesPanel.PerformLayout();
        actionPropertiesScrollPanel.PerformLayout();
    }

    private static void SetEditorRowVisible(Control label, Control editor, bool visible)
    {
        label.Visible = visible;
        editor.Visible = visible;
    }

    private void ClearActionParameterInputs()
    {
        leftNumericBox.Value = 0;
        topNumericBox.Value = 0;
        rightNumericBox.Value = 0;
        bottomNumericBox.Value = 0;
        targetedWidthNumericBox.Value = 0;
        targetedHeightNumericBox.Value = 0;
        proportionalCheckBox.Checked = true;
        rulerColorTextBox.Text = "";
        rulerStyleComboBox.SelectedIndex = -1;
        rulerValueModeComboBox.SelectedIndex = -1;
        rulerOrientationComboBox.SelectedIndex = -1;
        rulerPositionNumericBox.Value = 0;
    }

    private void SelectAnchorButton(AnchorHorizontal horizontal, AnchorVertical vertical)
    {
        var button = (horizontal, vertical) switch
        {
            (AnchorHorizontal.Left, AnchorVertical.Top) => anchorTopLeftRadioButton,
            (AnchorHorizontal.Center, AnchorVertical.Top) => anchorTopCenterRadioButton,
            (AnchorHorizontal.Right, AnchorVertical.Top) => anchorTopRightRadioButton,
            (AnchorHorizontal.Left, AnchorVertical.Center) => anchorMiddleLeftRadioButton,
            (AnchorHorizontal.Right, AnchorVertical.Center) => anchorMiddleRightRadioButton,
            (AnchorHorizontal.Left, AnchorVertical.Bottom) => anchorBottomLeftRadioButton,
            (AnchorHorizontal.Center, AnchorVertical.Bottom) => anchorBottomCenterRadioButton,
            (AnchorHorizontal.Right, AnchorVertical.Bottom) => anchorBottomRightRadioButton,
            _ => anchorMiddleCenterRadioButton,
        };
        button.Checked = true;
    }

    private void ConvertProjectUnits(UnitType oldUnit, UnitType newUnit)
    {
        var factor = oldUnit == UnitType.Inch && newUnit == UnitType.Cm
            ? 2.54f
            : oldUnit == UnitType.Cm && newUnit == UnitType.Inch
                ? 1f / 2.54f
                : 1f;

        if (Math.Abs(factor - 1f) < 0.0001f)
        {
            return;
        }

        foreach (var action in _project.Actions)
        {
            action.Left = ConvertUnitValue(action.Left, factor);
            action.Top = ConvertUnitValue(action.Top, factor);
            action.Right = ConvertUnitValue(action.Right, factor);
            action.Bottom = ConvertUnitValue(action.Bottom, factor);
            action.TargetedWidth = ConvertUnitValue(action.TargetedWidth, factor);
            action.TargetedHeight = ConvertUnitValue(action.TargetedHeight, factor);
            if (action.Type == PdfActionType.AddRuler && action.RulerValueMode == RulerValueMode.Unit)
            {
                action.Position = ConvertUnitValue(action.Position, factor);
            }
        }
    }

    private void SyncUnitCombos()
    {
        SelectEnum(projectUnitTypeComboBox, _project.UnitType);
    }

    private static void ConfigureEnumCombo<TEnum>(ComboBox comboBox, string keyPrefix, TEnum selectedValue)
        where TEnum : struct, Enum
    {
        comboBox.DisplayMember = nameof(EnumComboItem<TEnum>.Text);
        comboBox.ValueMember = nameof(EnumComboItem<TEnum>.Value);
        comboBox.Items.Clear();
        foreach (var value in Enum.GetValues<TEnum>())
        {
            comboBox.Items.Add(new EnumComboItem<TEnum>(value, TranslationService.T(keyPrefix + value)));
        }

        SelectEnum(comboBox, selectedValue);
    }

    private static bool TryGetSelectedEnum<TEnum>(ComboBox comboBox, out TEnum value)
        where TEnum : struct, Enum
    {
        if (comboBox.SelectedItem is EnumComboItem<TEnum> item)
        {
            value = item.Value;
            return true;
        }

        value = default;
        return false;
    }

    private static void SelectEnum<TEnum>(ComboBox comboBox, TEnum value)
        where TEnum : struct, Enum
    {
        for (var index = 0; index < comboBox.Items.Count; index++)
        {
            if (comboBox.Items[index] is EnumComboItem<TEnum> item && EqualityComparer<TEnum>.Default.Equals(item.Value, value))
            {
                comboBox.SelectedIndex = index;
                return;
            }
        }

        comboBox.SelectedIndex = -1;
    }

    private static float? ConvertUnitValue(float? value, float factor)
    {
        return value == null ? null : Round(value.Value * factor);
    }

    private int ToPixels(float? value, float dpi)
    {
        return ToPixels(value ?? 0, dpi);
    }

    private int ToPixels(float value, float dpi)
    {
        return Math.Max(0, (int)Math.Round(UnitValueToInches(value) * dpi));
    }

    private float UnitValueToPoints(float? value)
    {
        return UnitValueToInches(value) * 72f;
    }

    private float UnitValueToInches(float? value)
    {
        return UnitValueToInches(value ?? 0);
    }

    private float UnitValueToInches(float value)
    {
        return _project.UnitType == UnitType.Cm ? value / 2.54f : value;
    }

    private static float? ZeroToNull(float value)
    {
        return Math.Abs(value) < 0.0001f ? null : Round(value);
    }

    private static decimal FloatToDecimal(float? value)
    {
        return (decimal)Round(value ?? 0);
    }

    private static float Round(float value)
    {
        return (float)Math.Round(value, 3, MidpointRounding.AwayFromZero);
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }

    private void RefreshRecentProjectsMenu()
    {
        openRecentProjectMenuItem.DropDownItems.Clear();

        if (_settings.RecentProjects.Count == 0)
        {
            var emptyItem = new ToolStripMenuItem(TranslationService.T("recent.empty"))
            {
                Enabled = false,
            };
            openRecentProjectMenuItem.DropDownItems.Add(emptyItem);
            openRecentProjectMenuItem.Enabled = false;
            return;
        }

        openRecentProjectMenuItem.Enabled = true;
        foreach (var recentPath in _settings.RecentProjects)
        {
            var item = new ToolStripMenuItem(recentPath)
            {
                Tag = recentPath,
            };
            item.Click += RecentProjectMenuItem_Click;
            openRecentProjectMenuItem.DropDownItems.Add(item);
        }
    }

    private void RecentProjectMenuItem_Click(object? sender, EventArgs e)
    {
        if (sender is not ToolStripMenuItem { Tag: string path })
        {
            return;
        }

        if (!ConfirmSaveChanges())
        {
            return;
        }

        if (!File.Exists(path))
        {
            _settings.RemoveRecentProject(path);
            _settings.Save();
            RefreshRecentProjectsMenu();
            MessageBox.Show(this, TranslationService.T("message.recentProjectNotFound.body"), TranslationService.T("message.recentProjectNotFound.title"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        OpenProject(path);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!ConfirmSaveChanges())
        {
            e.Cancel = true;
            return;
        }

        _pdfDocument?.Dispose();
        _actionMenu?.Dispose();
        base.OnFormClosing(e);
    }

    private void MarkDirty()
    {
        if (_isDirty)
        {
            return;
        }

        _isDirty = true;
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        var name = string.IsNullOrWhiteSpace(_project.Name) ? TranslationService.T("common.untitled") : _project.Name.Trim();
        Text = TranslationService.T("app.titleWithProject", name, _isDirty ? TranslationService.T("app.dirtyMark") : "");
    }

    private void UpdateStatus(string text)
    {
        statusLabel.Text = text;
    }

    private bool ConfirmSaveChanges()
    {
        if (!_isDirty)
        {
            return true;
        }

        var result = MessageBox.Show(
            this,
            TranslationService.T("message.saveChanges.body"),
            TranslationService.T("app.title"),
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);

        return result switch
        {
            DialogResult.Yes => SaveProject(),
            DialogResult.No => true,
            _ => false,
        };
    }

    private string GetInitialProjectFolder()
    {
        if (!string.IsNullOrWhiteSpace(_projectPath))
        {
            var currentFolder = Path.GetDirectoryName(_projectPath);
            if (Directory.Exists(currentFolder))
            {
                return currentFolder;
            }
        }

        foreach (var recentPath in _settings.RecentProjects)
        {
            var recentFolder = Path.GetDirectoryName(recentPath);
            if (Directory.Exists(recentFolder))
            {
                return recentFolder;
            }
        }

        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    private string GetInitialPdfFolder()
    {
        if (Directory.Exists(_settings.LastPdfFolder))
        {
            return _settings.LastPdfFolder;
        }

        if (!string.IsNullOrWhiteSpace(_project.PdfFilePath))
        {
            var pdfFolder = Path.GetDirectoryName(_project.PdfFilePath);
            if (Directory.Exists(pdfFolder))
            {
                return pdfFolder;
            }
        }

        return GetInitialProjectFolder();
    }

    private string GetDefaultProjectFileName()
    {
        var name = string.IsNullOrWhiteSpace(_project.Name) ? TranslationService.T("common.untitled") : _project.Name.Trim();
        var invalid = Path.GetInvalidFileNameChars();
        var cleaned = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
        return cleaned + ProjectExtension;
    }

    private static string EnsureProjectExtension(string path)
    {
        return string.Equals(Path.GetExtension(path), ProjectExtension, StringComparison.OrdinalIgnoreCase)
            ? path
            : path + ProjectExtension;
    }
}
