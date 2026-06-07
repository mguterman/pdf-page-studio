using System.Globalization;
using System.Text.Json;

namespace PdfResizer;

public sealed partial class MainForm : Form
{
    private readonly AppSettings _settings;
    private SizeF _trimPageSizeInches;

    public MainForm()
    {
        _settings = AppSettings.Load();
        InitializeComponent();
        ApplyTrimSettingsToInputs();
        UpdateTrimFrameSize();
        InitializeNativeDependencies();
    }

    private void InitializeNativeDependencies()
    {
        try
        {
            PdfiumNativeLoader.EnsureLoaded();
        }
        catch (Exception ex)
        {
            trimBrowsePdfButton.Enabled = false;
            trimButton.Enabled = false;
            trimStatusLabel.Text = "Preview initialization failed: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Preview initialization failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TrimBrowsePdfButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreatePdfDialog(_settings.LastTrimFolder);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            PdfBookResizer.GetPageCount(dialog.FileName);
            trimInputTextBox.Text = dialog.FileName;
            trimDestinationTextBox.Text = Path.GetDirectoryName(dialog.FileName) ?? "";
            _settings.LastTrimFolder = trimDestinationTextBox.Text;
            _settings.Save();

            UpdateTrimOutputPath();
            UpdateTrimButtons();
            LoadTrimPreview();
            trimStatusLabel.Text = "Ready.";
        }
        catch (Exception ex)
        {
            trimButton.Enabled = false;
            trimStatusLabel.Text = "Could not read PDF: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Could not read PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TrimDestinationButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreateFolderDialog(GetTrimDefaultFolder());
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        trimDestinationTextBox.Text = dialog.SelectedPath;
        UpdateTrimOutputPath();
        UpdateTrimButtons();
    }

    private void TrimOpenDestinationButton_Click(object? sender, EventArgs e)
    {
        OpenFolder(trimDestinationTextBox.Text, trimStatusLabel);
    }

    private void TrimSetting_ValueChanged(object? sender, EventArgs e)
    {
        _settings.Trim = ReadTrimSettingsFromInputs();
        _settings.Save();
        trimPreviewBox.SetTrim(_settings.Trim);
        UpdateTrimOutputPath();
        UpdateTrimFrameSize();
    }

    private void TrimButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateTrimInputs())
        {
            return;
        }

        var outputPath = BuildTrimOutputPath();

        try
        {
            SetTrimBusy(true, "Trimming...");
            var settings = ReadTrimSettingsFromInputs();

            PdfBookResizer.TrimPdf(
                trimInputTextBox.Text,
                outputPath,
                settings.Left / 100f,
                settings.Top / 100f,
                settings.Right / 100f,
                settings.Bottom / 100f);

            trimStatusLabel.Text = "Done: " + outputPath;
            MessageBox.Show(this, "PDF trim completed.", "PDF Resizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            trimStatusLabel.Text = "Error: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Trim failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetTrimBusy(false, "");
        }
    }

    private void AdjustBrowsePdfButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreatePdfDialog(_settings.LastAdjustFolder);
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            var pageCount = PdfBookResizer.GetPageCount(dialog.FileName);
            adjustInputTextBox.Text = dialog.FileName;
            adjustDestinationTextBox.Text = Path.GetDirectoryName(dialog.FileName) ?? "";
            startPageInput.Maximum = pageCount;
            endPageInput.Maximum = pageCount;
            startPageInput.Value = 1;
            endPageInput.Value = pageCount;
            _settings.LastAdjustFolder = adjustDestinationTextBox.Text;
            _settings.Save();

            UpdateAdjustOutputPath();
            UpdateAdjustButtons();
            adjustStatusLabel.Text = "Ready.";
        }
        catch (Exception ex)
        {
            adjustConvertButton.Enabled = false;
            adjustStatusLabel.Text = "Could not read PDF: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Could not read PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AdjustDestinationButton_Click(object? sender, EventArgs e)
    {
        using var dialog = CreateFolderDialog(GetAdjustDefaultFolder());
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        adjustDestinationTextBox.Text = dialog.SelectedPath;
        UpdateAdjustOutputPath();
        UpdateAdjustButtons();
    }

    private void AdjustOpenDestinationButton_Click(object? sender, EventArgs e)
    {
        OpenFolder(adjustDestinationTextBox.Text, adjustStatusLabel);
    }

    private void AdjustSetting_ValueChanged(object? sender, EventArgs e)
    {
        UpdateAdjustOutputPath();
    }

    private void AdjustConvertButton_Click(object? sender, EventArgs e)
    {
        if (!ValidateAdjustInputs())
        {
            return;
        }

        var outputPath = BuildAdjustOutputPath();

        try
        {
            SetAdjustBusy(true, "Converting...");
            var endPage = endPageInput.Value == 0 ? (int?)null : (int)endPageInput.Value;
            var shiftInches = (float)shiftInput.Value / 100f;

            PdfBookResizer.ConvertToPageSize(
                adjustInputTextBox.Text,
                outputPath,
                (float)widthInput.Value,
                (float)heightInput.Value,
                shiftInches,
                (int)startPageInput.Value,
                endPage);

            adjustStatusLabel.Text = "Done: " + outputPath;
            MessageBox.Show(this, "PDF conversion completed.", "PDF Resizer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            adjustStatusLabel.Text = "Error: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Conversion failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            SetAdjustBusy(false, "");
        }
    }

    private static OpenFileDialog CreatePdfDialog(string? lastFolder)
    {
        return new OpenFileDialog
        {
            Title = "Choose PDF file",
            Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
            CheckFileExists = true,
            InitialDirectory = Directory.Exists(lastFolder)
                ? lastFolder
                : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        };
    }

    private static FolderBrowserDialog CreateFolderDialog(string selectedPath)
    {
        return new FolderBrowserDialog
        {
            Description = "Choose destination folder",
            UseDescriptionForTitle = true,
            SelectedPath = selectedPath,
        };
    }

    private void LoadTrimPreview()
    {
        try
        {
            var preview = PdfPreviewRenderer.RenderFirstPage(trimInputTextBox.Text);
            _trimPageSizeInches = preview.PageSizeInches;
            trimPreviewBox.SetPreview(preview.Image, preview.PageSizeInches);
            trimPreviewBox.SetTrim(ReadTrimSettingsFromInputs());
            UpdateTrimFrameSize();
        }
        catch (Exception ex)
        {
            trimStatusLabel.Text = "Could not render preview: " + ex.Message;
            MessageBox.Show(this, ex.Message, "Could not render preview", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ApplyTrimSettingsToInputs()
    {
        trimLeftInput.Value = _settings.Trim.Left;
        trimTopInput.Value = _settings.Trim.Top;
        trimRightInput.Value = _settings.Trim.Right;
        trimBottomInput.Value = _settings.Trim.Bottom;
    }

    private TrimSettings ReadTrimSettingsFromInputs()
    {
        return new TrimSettings
        {
            Left = (int)trimLeftInput.Value,
            Top = (int)trimTopInput.Value,
            Right = (int)trimRightInput.Value,
            Bottom = (int)trimBottomInput.Value,
        };
    }

    private void UpdateTrimFrameSize()
    {
        var settings = ReadTrimSettingsFromInputs();
        var width = Math.Max(0, _trimPageSizeInches.Width - settings.Left / 100f - settings.Right / 100f);
        var height = Math.Max(0, _trimPageSizeInches.Height - settings.Top / 100f - settings.Bottom / 100f);
        trimFrameSizeTextBox.Text = $"{width:0.##} x {height:0.##} inches";
    }

    private void UpdateTrimOutputPath()
    {
        trimOutputTextBox.Text = CanBuildTrimOutputPath() ? BuildTrimOutputPath() : "";
        UpdateTrimButtons();
    }

    private void UpdateAdjustOutputPath()
    {
        adjustOutputTextBox.Text = CanBuildAdjustOutputPath() ? BuildAdjustOutputPath() : "";
        UpdateAdjustButtons();
    }

    private void UpdateTrimButtons(bool enabled = true)
    {
        var canAct = enabled && HasTrimInput() && HasTrimDestination();
        trimButton.Enabled = canAct;
        trimOpenDestinationButton.Enabled = canAct;
    }

    private void UpdateAdjustButtons(bool enabled = true)
    {
        var canAct = enabled && HasAdjustInput() && HasAdjustDestination();
        adjustConvertButton.Enabled = canAct;
        adjustOpenDestinationButton.Enabled = canAct;
    }

    private bool ValidateTrimInputs()
    {
        if (!HasTrimInput())
        {
            trimStatusLabel.Text = "Choose a PDF file first.";
            return false;
        }

        if (!HasTrimDestination())
        {
            trimStatusLabel.Text = "Choose an existing destination folder.";
            return false;
        }

        return true;
    }

    private bool ValidateAdjustInputs()
    {
        if (!HasAdjustInput())
        {
            adjustStatusLabel.Text = "Choose a PDF file first.";
            return false;
        }

        if (!HasAdjustDestination())
        {
            adjustStatusLabel.Text = "Choose an existing destination folder.";
            return false;
        }

        return true;
    }

    private bool CanBuildTrimOutputPath()
    {
        return !string.IsNullOrWhiteSpace(trimInputTextBox.Text)
            && !string.IsNullOrWhiteSpace(trimDestinationTextBox.Text);
    }

    private bool CanBuildAdjustOutputPath()
    {
        return !string.IsNullOrWhiteSpace(adjustInputTextBox.Text)
            && !string.IsNullOrWhiteSpace(adjustDestinationTextBox.Text);
    }

    private bool HasTrimInput()
    {
        return !string.IsNullOrWhiteSpace(trimInputTextBox.Text);
    }

    private bool HasAdjustInput()
    {
        return !string.IsNullOrWhiteSpace(adjustInputTextBox.Text);
    }

    private bool HasTrimDestination()
    {
        return !string.IsNullOrWhiteSpace(trimDestinationTextBox.Text)
            && Directory.Exists(trimDestinationTextBox.Text);
    }

    private bool HasAdjustDestination()
    {
        return !string.IsNullOrWhiteSpace(adjustDestinationTextBox.Text)
            && Directory.Exists(adjustDestinationTextBox.Text);
    }

    private string BuildTrimOutputPath()
    {
        var sourceFileName = Path.GetFileName(trimInputTextBox.Text);
        var settings = ReadTrimSettingsFromInputs();
        var prefix = $"Trim_{settings.Left}_{settings.Top}_{settings.Right}_{settings.Bottom}_";
        return Path.Combine(trimDestinationTextBox.Text, prefix + sourceFileName);
    }

    private string BuildAdjustOutputPath()
    {
        var sourceFileName = Path.GetFileName(adjustInputTextBox.Text);
        var prefix = $"Converted_{FormatToken(widthInput.Value)}_{FormatToken(heightInput.Value)}_{(int)shiftInput.Value}_";
        return Path.Combine(adjustDestinationTextBox.Text, prefix + sourceFileName);
    }

    private static string FormatToken(decimal value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture).Replace(".", "p");
    }

    private string GetTrimDefaultFolder()
    {
        if (Directory.Exists(trimDestinationTextBox.Text))
        {
            return trimDestinationTextBox.Text;
        }

        if (!string.IsNullOrWhiteSpace(trimInputTextBox.Text))
        {
            return Path.GetDirectoryName(trimInputTextBox.Text) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        return Directory.Exists(_settings.LastTrimFolder)
            ? _settings.LastTrimFolder
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    private string GetAdjustDefaultFolder()
    {
        if (Directory.Exists(adjustDestinationTextBox.Text))
        {
            return adjustDestinationTextBox.Text;
        }

        if (!string.IsNullOrWhiteSpace(adjustInputTextBox.Text))
        {
            return Path.GetDirectoryName(adjustInputTextBox.Text) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        return Directory.Exists(_settings.LastAdjustFolder)
            ? _settings.LastAdjustFolder
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    private static void OpenFolder(string folder, Label statusLabel)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
        {
            statusLabel.Text = "Choose an existing destination folder first.";
            return;
        }

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = folder,
            UseShellExecute = true,
        });
    }

    private void SetTrimBusy(bool isBusy, string status)
    {
        UseWaitCursor = isBusy;
        UpdateTrimButtons(!isBusy);
        if (isBusy)
        {
            trimStatusLabel.Text = status;
        }
    }

    private void SetAdjustBusy(bool isBusy, string status)
    {
        UseWaitCursor = isBusy;
        UpdateAdjustButtons(!isBusy);
        if (isBusy)
        {
            adjustStatusLabel.Text = status;
        }
    }

    private sealed class AppSettings
    {
        public string? LastTrimFolder { get; set; }
        public string? LastAdjustFolder { get; set; }
        public TrimSettings Trim { get; set; } = new();

        public static AppSettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath))
                {
                    return new AppSettings();
                }

                var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsPath)) ?? new AppSettings();
                settings.Trim ??= new TrimSettings();
                return settings;
            }
            catch
            {
                return new AppSettings();
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true }));
        }

        private static string SettingsPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PdfResizer",
            "settings.json");
    }
}
