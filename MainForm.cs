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
        InitializeComponent();
        InitializePdfRendering();
        RefreshRecentProjectsMenu();
        BindProject();
        LoadPdfFromProject();
        UpdateTitle();
        UpdateStatus("Ready.");
    }

    private void OpenProjectMenuItem_Click(object? sender, EventArgs e)
    {
        if (!ConfirmSaveChanges())
        {
            return;
        }

        using var dialog = new OpenFileDialog
        {
            Title = "Open PDF Page Studio Project",
            Filter = "PDF Page Studio Project (*.ppsproj)|*.ppsproj|All files (*.*)|*.*",
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

    private void AddPdfFileMenuItem_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Add PDF File",
            Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
            CheckFileExists = true,
            InitialDirectory = GetInitialPdfFolder(),
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _project.PdfFilePath = dialog.FileName;
        MarkDirty();
        LoadPdfFromProject();
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
        UpdateStatus("Action deleted.");
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
        UpdateStatus("Action moved up.");
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
        UpdateStatus("Action moved down.");
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
        if (_isBinding || !Enum.TryParse<UnitType>(unitTypeComboBox.Text, out var newUnit) || newUnit == _project.UnitType)
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

        if (Enum.TryParse<PdfActionType>(actionTypeComboBox.Text, out var type))
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

        if (Enum.TryParse<PageFilterType>(pageFilterTypeComboBox.Text, out var type))
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
        UpdateStatus("Page filter updated.");
        RefreshCurrentPagePreview();
    }

    private void ActionNumberTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_isBinding || GetSelectedAction() is not { } action || sender is not TextBox textBox)
        {
            return;
        }

        if (!TryReadNullableFloat(textBox.Text, out var value))
        {
            UpdateStatus("Use a decimal number or leave the field empty.");
            return;
        }

        value = Round(value);
        if (textBox == leftTextBox) action.Left = value;
        else if (textBox == topTextBox) action.Top = value;
        else if (textBox == rightTextBox) action.Right = value;
        else if (textBox == bottomTextBox) action.Bottom = value;
        else if (textBox == targetedWidthTextBox) action.TargetedWidth = value;
        else if (textBox == targetedHeightTextBox) action.TargetedHeight = value;
        else if (textBox == rulerPositionTextBox) action.Position = value;
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

        if (Enum.TryParse<RulerStyle>(rulerStyleComboBox.Text, out var style))
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

        if (Enum.TryParse<RulerValueMode>(rulerValueModeComboBox.Text, out var mode))
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

        if (Enum.TryParse<RulerOrientation>(rulerOrientationComboBox.Text, out var orientation))
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
            UpdateStatus("Opened: " + path);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Open project failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus("Open project failed.");
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
            Title = "Save PDF Page Studio Project",
            Filter = "PDF Page Studio Project (*.ppsproj)|*.ppsproj|All files (*.*)|*.*",
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
            UpdateStatus("Saved: " + path);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, "Save project failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus("Save project failed.");
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
            MessageBox.Show(this, ex.Message, "PDF preview initialization failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            UpdateStatus("PDF file not found: " + _project.PdfFilePath);
            return;
        }

        try
        {
            _pdfDocument = PdfDocument.Load(_project.PdfFilePath);
            ShowPdfWorkspace();
            SetPdfToolbarEnabled(true);
            FitPageButton_Click(this, EventArgs.Empty);
            RenderCurrentPage();
            UpdateStatus("PDF loaded: " + _project.PdfFilePath);
        }
        catch (Exception ex)
        {
            ShowProjectInfo();
            MessageBox.Show(this, ex.Message, "PDF load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateStatus("PDF load failed.");
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
        var image = _pdfDocument.Render(_pageIndex, width, height, renderDpi, renderDpi, PdfRenderFlags.Annotations);
        pdfPageViewer.SetPage(image);
        UpdatePdfNavigation();
        UpdatePageSizeLabel(pageSize);
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
        pageCountLabel.Text = hasDocument ? $"of {_pdfDocument!.PageCount}" : "of 0";
        firstPageButton.Enabled = hasDocument && _pageIndex > 0;
        previousPageButton.Enabled = hasDocument && _pageIndex > 0;
        nextPageButton.Enabled = hasDocument && _pageIndex < _pdfDocument!.PageCount - 1;
        lastPageButton.Enabled = hasDocument && _pageIndex < _pdfDocument!.PageCount - 1;
        pageSizeLabel.Text = hasDocument ? pageSizeLabel.Text : "Page: -- x -- in";
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
        pageCountLabel.Text = enabled && _pdfDocument != null ? $"of {_pdfDocument.PageCount}" : "of 0";
        if (!enabled)
        {
            pageSizeLabel.Text = "Page: -- x -- in";
        }
    }

    private void UpdatePageSizeLabel(SizeF pageSizePoints)
    {
        var widthInches = pageSizePoints.Width / 72f;
        var heightInches = pageSizePoints.Height / 72f;
        var width = _project.UnitType == UnitType.Cm ? widthInches * 2.54f : widthInches;
        var height = _project.UnitType == UnitType.Cm ? heightInches * 2.54f : heightInches;
        var unit = _project.UnitType == UnitType.Cm ? "cm" : "in";
        pageSizeLabel.Text = string.Format(
            CultureInfo.InvariantCulture,
            "Page: {0:0.###} x {1:0.###} {2}",
            width,
            height,
            unit);
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
        UpdateStatus("Action added.");
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
        actionPropertiesPanel.Visible = hasAction;
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
        actionTypeComboBox.SelectedItem = action.Type.ToString();
        actionNameTextBox.Text = action.Name;
        pageFilterTypeComboBox.SelectedItem = action.PageFilter.Type.ToString();
        pageFilterRangeTextBox.Text = FormatPageRanges(action.PageFilter.Range);
        leftTextBox.Text = FormatNullableFloat(action.Left);
        topTextBox.Text = FormatNullableFloat(action.Top);
        rightTextBox.Text = FormatNullableFloat(action.Right);
        bottomTextBox.Text = FormatNullableFloat(action.Bottom);
        targetedWidthTextBox.Text = FormatNullableFloat(action.TargetedWidth);
        targetedHeightTextBox.Text = FormatNullableFloat(action.TargetedHeight);
        proportionalCheckBox.Checked = action.Proportional ?? true;
        rulerColorTextBox.Text = action.Color;
        rulerStyleComboBox.SelectedItem = action.Style.ToString();
        rulerValueModeComboBox.SelectedItem = action.RulerValueMode.ToString();
        rulerOrientationComboBox.SelectedItem = action.Orientation.ToString();
        rulerPositionTextBox.Text = FormatNullableFloat(action.Position);
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
            ? $"Preview: actions through selected ({count})"
            : $"Preview: all actions ({count})";
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

    private ProjectAction? GetSelectedAction()
    {
        var index = actionsListBox.SelectedIndex;
        return index >= 0 && index < _project.Actions.Count ? _project.Actions[index] : null;
    }

    private static string FormatActionListItem(int index, ProjectAction action)
    {
        return $"{index + 1}. {action.Name} [{action.Type}] ({FormatPageFilter(action.PageFilter)})";
    }

    private static string FormatPageFilter(PageFilter? filter)
    {
        if (filter == null)
        {
            return "All pages";
        }

        var range = filter.Range.Count == 0 ? "All pages" : FormatPageRanges(filter.Range);
        return filter.Type == PageFilterType.Any ? range : $"{range}, {filter.Type}";
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
                    error = "Page range must use positive page numbers.";
                    return false;
                }

                ranges.Add(new PageFilterRange { Start = page, End = page });
                continue;
            }

            var startText = part[..dashIndex].Trim();
            var endText = part[(dashIndex + 1)..].Trim();
            if (!int.TryParse(startText, out var start) || start < 1)
            {
                error = "Page range start must be a positive page number.";
                return false;
            }

            int? end = null;
            if (!string.IsNullOrWhiteSpace(endText))
            {
                if (!int.TryParse(endText, out var parsedEnd) || parsedEnd < start)
                {
                    error = "Page range end must be empty or greater than/equal to start.";
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
                PdfActionType.AddRuler => "Add Ruler",
                _ => action.Type.ToString(),
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
        return type == PdfActionType.AddRuler ? "Add Ruler" : type.ToString();
    }

    private void UpdateActionEditorVisibility(ProjectAction? action)
    {
        var type = action?.Type;
        var hasMargins = type is PdfActionType.Trim or PdfActionType.Expand;
        var hasTargetSize = type is PdfActionType.Resize or PdfActionType.Zoom;
        var hasProportional = type is PdfActionType.Resize or PdfActionType.Zoom;
        var hasAnchor = type == PdfActionType.Zoom;
        var hasRuler = type == PdfActionType.AddRuler;

        SetEditorRowVisible(leftLabel, leftTextBox, hasMargins);
        SetEditorRowVisible(topLabel, topTextBox, hasMargins);
        SetEditorRowVisible(rightLabel, rightTextBox, hasMargins);
        SetEditorRowVisible(bottomLabel, bottomTextBox, hasMargins);
        SetEditorRowVisible(targetedWidthLabel, targetedWidthTextBox, hasTargetSize);
        SetEditorRowVisible(targetedHeightLabel, targetedHeightTextBox, hasTargetSize);
        SetEditorRowVisible(proportionalLabel, proportionalCheckBox, hasProportional);
        SetEditorRowVisible(anchorLabel, anchorPanel, hasAnchor);
        SetEditorRowVisible(rulerColorLabel, rulerColorTextBox, hasRuler);
        SetEditorRowVisible(rulerStyleLabel, rulerStyleComboBox, hasRuler);
        SetEditorRowVisible(rulerValueModeLabel, rulerValueModeComboBox, hasRuler);
        SetEditorRowVisible(rulerOrientationLabel, rulerOrientationComboBox, hasRuler);
        SetEditorRowVisible(rulerPositionLabel, rulerPositionTextBox, hasRuler);
    }

    private static void SetEditorRowVisible(Control label, Control editor, bool visible)
    {
        label.Visible = visible;
        editor.Visible = visible;
    }

    private void ClearActionParameterInputs()
    {
        leftTextBox.Text = "";
        topTextBox.Text = "";
        rightTextBox.Text = "";
        bottomTextBox.Text = "";
        targetedWidthTextBox.Text = "";
        targetedHeightTextBox.Text = "";
        proportionalCheckBox.Checked = true;
        rulerColorTextBox.Text = "";
        rulerStyleComboBox.SelectedIndex = -1;
        rulerValueModeComboBox.SelectedIndex = -1;
        rulerOrientationComboBox.SelectedIndex = -1;
        rulerPositionTextBox.Text = "";
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
        projectUnitTypeComboBox.SelectedItem = _project.UnitType.ToString();
        unitTypeComboBox.SelectedItem = _project.UnitType.ToString();
    }

    private static float? ConvertUnitValue(float? value, float factor)
    {
        return value == null ? null : Round(value.Value * factor);
    }

    private static bool TryReadNullableFloat(string text, out float? value)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            value = null;
            return true;
        }

        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) ||
            float.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out parsed))
        {
            value = parsed;
            return true;
        }

        value = null;
        return false;
    }

    private static float? Round(float? value)
    {
        return value == null ? null : Round(value.Value);
    }

    private static float Round(float value)
    {
        return (float)Math.Round(value, 3, MidpointRounding.AwayFromZero);
    }

    private static string FormatNullableFloat(float? value)
    {
        return value == null ? "" : value.Value.ToString("0.###", CultureInfo.InvariantCulture);
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
            var emptyItem = new ToolStripMenuItem("(No recent projects)")
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
            MessageBox.Show(this, "Project file was not found and has been removed from recent projects.", "Recent project not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        var name = string.IsNullOrWhiteSpace(_project.Name) ? "Untitled" : _project.Name.Trim();
        Text = $"PDF Page Studio - {name}{(_isDirty ? " *" : "")}";
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
            "Save changes to the current project?",
            "PDF Page Studio",
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
        var name = string.IsNullOrWhiteSpace(_project.Name) ? "Untitled" : _project.Name.Trim();
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
