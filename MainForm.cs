using System.Text.Json;

namespace PdfPageStudio;

public sealed partial class MainForm : Form
{
    private const string ProjectExtension = ".ppsproj";
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly AppSettings _settings;
    private PdfPageStudioProject _project = new();
    private string? _projectPath;
    private bool _isDirty;
    private bool _isBinding;

    public MainForm()
    {
        _settings = AppSettings.Load();
        InitializeComponent();
        RefreshRecentProjectsMenu();
        BindProject();
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

    private void OpenProject(string path)
    {
        try
        {
            var json = File.ReadAllText(path);
            _project = JsonSerializer.Deserialize<PdfPageStudioProject>(json, _jsonOptions) ?? new PdfPageStudioProject();
            _projectPath = path;
            _isDirty = false;
            _settings.AddRecentProject(path);
            _settings.Save();
            BindProject();
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
        _isBinding = true;
        projectNameTextBox.Text = _project.Name;
        projectDescriptionTextBox.Text = _project.Description;
        _isBinding = false;
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
