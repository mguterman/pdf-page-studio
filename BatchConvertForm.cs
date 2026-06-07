namespace PdfPageStudio;

public sealed class BatchConvertForm : Form
{
    private readonly ListBox _filesListBox = new();
    private readonly Button _addButton = new();
    private readonly Button _removeButton = new();
    private readonly Button _convertButton = new();
    private readonly Button _cancelButton = new();

    public BatchConvertForm()
    {
        Text = TranslationService.T("batch.title");
        Width = 720;
        Height = 460;
        MinimumSize = new Size(520, 340);
        StartPosition = FormStartPosition.CenterParent;
        AllowDrop = true;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12),
        };
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

        var label = new Label
        {
            Dock = DockStyle.Fill,
            Text = TranslationService.T("batch.dropFiles"),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        _filesListBox.Dock = DockStyle.Fill;
        _filesListBox.SelectionMode = SelectionMode.MultiExtended;
        _filesListBox.SelectedIndexChanged += (_, _) => UpdateButtons();

        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
        };

        ConfigureButton(_convertButton, TranslationService.T("convert.menu.multiple"), ConvertButton_Click);
        ConfigureButton(_cancelButton, TranslationService.T("common.cancel"), (_, _) => DialogResult = DialogResult.Cancel);
        ConfigureButton(_addButton, TranslationService.T("common.browse"), AddButton_Click);
        ConfigureButton(_removeButton, TranslationService.T("common.remove"), RemoveButton_Click);

        buttonsPanel.Controls.Add(_convertButton);
        buttonsPanel.Controls.Add(_cancelButton);
        buttonsPanel.Controls.Add(_removeButton);
        buttonsPanel.Controls.Add(_addButton);
        panel.Controls.Add(label, 0, 0);
        panel.Controls.Add(_filesListBox, 0, 1);
        panel.Controls.Add(buttonsPanel, 0, 2);
        Controls.Add(panel);

        DragEnter += BatchConvertForm_DragEnter;
        DragDrop += BatchConvertForm_DragDrop;
        UpdateButtons();
    }

    public IReadOnlyList<string> PdfFiles => _filesListBox.Items.Cast<string>().ToList();

    private static void ConfigureButton(Button button, string text, EventHandler handler)
    {
        button.Text = text;
        button.Width = 112;
        button.Height = 30;
        button.Click += handler;
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = TranslationService.T("dialog.addPdf.title"),
            Filter = TranslationService.T("dialog.pdf.filter"),
            CheckFileExists = true,
            Multiselect = true,
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            AddFiles(dialog.FileNames);
        }
    }

    private void RemoveButton_Click(object? sender, EventArgs e)
    {
        var selected = _filesListBox.SelectedItems.Cast<string>().ToList();
        foreach (var file in selected)
        {
            _filesListBox.Items.Remove(file);
        }

        UpdateButtons();
    }

    private void ConvertButton_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    private void BatchConvertForm_DragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void BatchConvertForm_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files)
        {
            AddFiles(files);
        }
    }

    private void AddFiles(IEnumerable<string> files)
    {
        foreach (var file in files.Where(file => string.Equals(Path.GetExtension(file), ".pdf", StringComparison.OrdinalIgnoreCase)))
        {
            if (!_filesListBox.Items.Cast<string>().Any(item => string.Equals(item, file, StringComparison.OrdinalIgnoreCase)))
            {
                _filesListBox.Items.Add(file);
            }
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        _convertButton.Enabled = _filesListBox.Items.Count > 0;
        _removeButton.Enabled = _filesListBox.SelectedItems.Count > 0 || _filesListBox.Items.Count > 0;
    }
}
