using System.Text.Json;
using PdfiumViewer;

namespace PdfPageStudio;

public sealed partial class MainForm : Form
{
    private const string ProjectExtension = ".ppsproj";
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly AppSettings _settings;
    private PdfPageStudioProject _project = new();
    private PdfDocument? _pdfDocument;
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

    private void NextPageButton_Click(object? sender, EventArgs e)
    {
        if (_pdfDocument == null || _pageIndex >= _pdfDocument.PageCount - 1)
        {
            return;
        }

        _pageIndex++;
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
        InsertAction(_project.Actions.Count);
    }

    private void InsertBeforeActionButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex >= 0 ? actionsListBox.SelectedIndex : 0;
        InsertAction(index);
    }

    private void InsertAfterActionButton_Click(object? sender, EventArgs e)
    {
        var index = actionsListBox.SelectedIndex >= 0 ? actionsListBox.SelectedIndex + 1 : _project.Actions.Count;
        InsertAction(index);
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
    }

    private void ActionsListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        BindSelectedAction();
        UpdateActionButtons();
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
        previousPageButton.Enabled = hasDocument && _pageIndex > 0;
        nextPageButton.Enabled = hasDocument && _pageIndex < _pdfDocument!.PageCount - 1;
    }

    private void SetPdfToolbarEnabled(bool enabled)
    {
        pageNumberTextBox.Enabled = enabled;
        zoomOutButton.Enabled = enabled;
        zoomInButton.Enabled = enabled;
        fitWidthButton.Enabled = enabled;
        fitPageButton.Enabled = enabled;
        previousPageButton.Enabled = false;
        nextPageButton.Enabled = false;
        pageCountLabel.Text = enabled && _pdfDocument != null ? $"of {_pdfDocument.PageCount}" : "of 0";
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

    private void InsertAction(int index)
    {
        var action = new ProjectAction();
        index = Math.Clamp(index, 0, _project.Actions.Count);
        _project.Actions.Insert(index, action);
        MarkDirty();
        RefreshActions(index);
        UpdateStatus("Action added.");
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

            if (string.IsNullOrWhiteSpace(action.Type))
            {
                action.Type = "NewAction";
            }

            if (string.IsNullOrWhiteSpace(action.Name))
            {
                action.Name = "New Action";
            }

            action.PageFilter ??= new PageFilter();
            action.PageFilter.Range ??= [];
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
        actionNameTextBox.Enabled = hasAction;
        pageFilterTypeComboBox.Enabled = hasAction;
        pageFilterRangeTextBox.Enabled = hasAction;

        if (action == null)
        {
            actionNameTextBox.Text = "";
            pageFilterTypeComboBox.SelectedIndex = -1;
            pageFilterRangeTextBox.Text = "";
            _isBinding = false;
            return;
        }

        action.PageFilter ??= new PageFilter();
        actionNameTextBox.Text = action.Name;
        pageFilterTypeComboBox.SelectedItem = action.PageFilter.Type.ToString();
        pageFilterRangeTextBox.Text = FormatPageRanges(action.PageFilter.Range);
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

    private ProjectAction? GetSelectedAction()
    {
        var index = actionsListBox.SelectedIndex;
        return index >= 0 && index < _project.Actions.Count ? _project.Actions[index] : null;
    }

    private static string FormatActionListItem(int index, ProjectAction action)
    {
        return $"{index + 1}. {action.Name} ({FormatPageFilter(action.PageFilter)})";
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
