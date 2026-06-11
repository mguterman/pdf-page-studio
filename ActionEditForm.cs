using System.Globalization;

namespace PdfPageStudio;

public sealed class ActionEditForm : Form
{
    private readonly UnitType _unitType;
    private readonly bool _allowTypeChange;
    private readonly Label _descriptionLabel = new();
    private readonly TableLayoutPanel _parametersPanel = new();
    private readonly TextBox _nameTextBox = new();
    private readonly ComboBox _typeComboBox = new();
    private readonly ComboBox _pageFilterTypeComboBox = new();
    private readonly TextBox _pageFilterRangeTextBox = new();
    private readonly NumericUpDown _leftNumericBox = CreateNumericBox();
    private readonly NumericUpDown _topNumericBox = CreateNumericBox();
    private readonly NumericUpDown _rightNumericBox = CreateNumericBox();
    private readonly NumericUpDown _bottomNumericBox = CreateNumericBox();
    private readonly CheckBox _enableWidthCheckBox = new();
    private readonly NumericUpDown _targetedWidthNumericBox = CreateNumericBox();
    private readonly CheckBox _enableHeightCheckBox = new();
    private readonly NumericUpDown _targetedHeightNumericBox = CreateNumericBox();
    private readonly CheckBox _proportionalCheckBox = new();
    private readonly TableLayoutPanel _anchorPanel = new();
    private readonly ComboBox _rulerOrientationComboBox = new();
    private readonly NumericUpDown _rulerPositionNumericBox = CreateNumericBox();
    private readonly ComboBox _rulerValueModeComboBox = new();
    private readonly ComboBox _rulerStyleComboBox = new();
    private readonly TextBox _rulerColorTextBox = new();
    private readonly Button _rulerColorButton = new();
    private readonly CheckBox _previewCheckBox = new();
    private readonly ErrorProvider _errorProvider = new();
    private readonly Dictionary<(AnchorHorizontal Horizontal, AnchorVertical Vertical), RadioButton> _anchorButtons = [];
    private bool _isBinding;

    public ActionEditForm(ProjectAction action, bool previewEnabled, UnitType unitType, bool allowTypeChange)
    {
        Action = CloneAction(action);
        _unitType = unitType;
        _allowTypeChange = allowTypeChange;
        PreviewEnabled = previewEnabled;
        InitializeComponent();
        BindAction();
    }

    public ProjectAction Action { get; private set; }
    public bool PreviewEnabled { get; private set; }
    public event EventHandler? ActionChanged;
    public event EventHandler? PreviewEnabledChanged;

    private void InitializeComponent()
    {
        Text = TranslationService.T("dialog.action.title", GetActionDisplayName(Action.Type));
        StartPosition = FormStartPosition.CenterParent;
        MinimizeBox = false;
        MaximizeBox = false;
        ShowInTaskbar = false;
        Size = new Size(520, 650);
        MinimumSize = new Size(500, 520);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(14),
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _descriptionLabel.Dock = DockStyle.Top;
        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(470, 0);
        _descriptionLabel.Margin = new Padding(0, 0, 0, 12);

        _parametersPanel.Dock = DockStyle.Fill;
        _parametersPanel.AutoScroll = true;
        _parametersPanel.ColumnCount = 2;
        _parametersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        _parametersPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

        var footerPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 8, 0, 0),
        };
        _previewCheckBox.AutoSize = true;
        _previewCheckBox.Text = TranslationService.T("dialog.action.preview");
        _previewCheckBox.CheckedChanged += PreviewCheckBox_CheckedChanged;
        footerPanel.Controls.Add(_previewCheckBox);

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 12, 0, 0),
        };
        var applyButton = new Button
        {
            Text = TranslationService.T("common.apply"),
            DialogResult = DialogResult.OK,
            Size = new Size(90, 30),
        };
        applyButton.Click += ApplyButton_Click;
        var cancelButton = new Button
        {
            Text = TranslationService.T("common.cancel"),
            DialogResult = DialogResult.Cancel,
            Size = new Size(90, 30),
        };
        buttonPanel.Controls.Add(applyButton);
        buttonPanel.Controls.Add(cancelButton);
        AcceptButton = applyButton;
        CancelButton = cancelButton;

        root.Controls.Add(_descriptionLabel, 0, 0);
        root.Controls.Add(_parametersPanel, 0, 1);
        root.Controls.Add(footerPanel, 0, 2);
        root.Controls.Add(buttonPanel, 0, 3);
        Controls.Add(root);

        ConfigureEnumCombo(_typeComboBox, "enum.action.", Action.Type);
        _typeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeComboBox.Enabled = _allowTypeChange;
        _typeComboBox.SelectedIndexChanged += TypeComboBox_SelectedIndexChanged;
        ConfigureEnumCombo(_pageFilterTypeComboBox, "enum.pageFilter.", Action.PageFilter.Type);
        _pageFilterTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _pageFilterTypeComboBox.SelectedIndexChanged += (_, _) => UpdateActionFromInputs();
        _pageFilterRangeTextBox.PlaceholderText = TranslationService.T("field.range.placeholder");
        _pageFilterRangeTextBox.TextChanged += (_, _) => UpdateActionFromInputs();
        _nameTextBox.TextChanged += (_, _) => UpdateActionFromInputs();

        ConfigureNumeric(_leftNumericBox);
        ConfigureNumeric(_topNumericBox);
        ConfigureNumeric(_rightNumericBox);
        ConfigureNumeric(_bottomNumericBox);
        ConfigureNumeric(_targetedWidthNumericBox);
        ConfigureNumeric(_targetedHeightNumericBox);
        ConfigureNumeric(_rulerPositionNumericBox);

        _enableWidthCheckBox.CheckedChanged += (_, _) =>
        {
            _targetedWidthNumericBox.Enabled = _enableWidthCheckBox.Checked;
            UpdateActionFromInputs();
        };
        _enableHeightCheckBox.CheckedChanged += (_, _) =>
        {
            _targetedHeightNumericBox.Enabled = _enableHeightCheckBox.Checked;
            UpdateActionFromInputs();
        };
        _proportionalCheckBox.AutoSize = true;
        _proportionalCheckBox.CheckedChanged += (_, _) => UpdateActionFromInputs();

        ConfigureAnchorPanel();
        ConfigureEnumCombo(_rulerOrientationComboBox, "enum.rulerOrientation.", Action.Orientation);
        _rulerOrientationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _rulerOrientationComboBox.SelectedIndexChanged += (_, _) => UpdateActionFromInputs();
        ConfigureRulerValueModeCombo();
        _rulerValueModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _rulerValueModeComboBox.SelectedIndexChanged += (_, _) => UpdateActionFromInputs();
        ConfigureEnumCombo(_rulerStyleComboBox, "enum.rulerStyle.", Action.Style);
        _rulerStyleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _rulerStyleComboBox.SelectedIndexChanged += (_, _) => UpdateActionFromInputs();
        _rulerColorTextBox.TextChanged += (_, _) => UpdateActionFromInputs();
        _rulerColorButton.Text = TranslationService.T("common.browse");
        _rulerColorButton.Click += RulerColorButton_Click;
        _errorProvider.ContainerControl = this;
    }

    private void BindAction()
    {
        _isBinding = true;
        Text = TranslationService.T("dialog.action.title", GetActionDisplayName(Action.Type));
        _descriptionLabel.Text = GetActionDescription(Action.Type);
        _parametersPanel.SuspendLayout();
        _parametersPanel.Controls.Clear();
        _parametersPanel.RowStyles.Clear();
        _parametersPanel.RowCount = 0;

        AddRow(TranslationService.T("field.type"), _typeComboBox);
        AddRow(TranslationService.T("field.name"), _nameTextBox);
        AddRow(TranslationService.T("field.pageType"), _pageFilterTypeComboBox);
        AddRow(TranslationService.T("field.range"), _pageFilterRangeTextBox);

        var type = Action.Type;
        if (type is PdfActionType.Trim or PdfActionType.Expand or PdfActionType.AddFrame)
        {
            AddRow(TranslationService.T("field.left"), _leftNumericBox);
            AddRow(TranslationService.T("field.top"), _topNumericBox);
            AddRow(TranslationService.T("field.right"), _rightNumericBox);
            AddRow(TranslationService.T("field.bottom"), _bottomNumericBox);
        }

        if (type is PdfActionType.Resize or PdfActionType.Zoom or PdfActionType.AdjustSize)
        {
            AddRow(TranslationService.T("field.width"), CreateOptionalPanel(_enableWidthCheckBox, _targetedWidthNumericBox, type == PdfActionType.AdjustSize));
            AddRow(TranslationService.T("field.height"), CreateOptionalPanel(_enableHeightCheckBox, _targetedHeightNumericBox, type == PdfActionType.AdjustSize));
        }

        if (type is PdfActionType.Resize or PdfActionType.Zoom)
        {
            AddRow(TranslationService.T("field.proportional"), _proportionalCheckBox);
        }

        if (type == PdfActionType.Zoom)
        {
            AddRow(TranslationService.T("field.anchor"), _anchorPanel);
        }

        if (type == PdfActionType.AddRuler)
        {
            AddRow(TranslationService.T("field.line"), _rulerOrientationComboBox);
            AddRow(TranslationService.T("field.position"), _rulerPositionNumericBox);
            AddRow(TranslationService.T("field.mode"), _rulerValueModeComboBox);
        }

        if (type is PdfActionType.AddRuler or PdfActionType.AddFrame)
        {
            AddRow(TranslationService.T("field.style"), _rulerStyleComboBox);
            AddRow(TranslationService.T("field.color"), CreateColorPanel());
        }

        SelectEnum(_typeComboBox, Action.Type);
        _nameTextBox.Text = Action.Name;
        SelectEnum(_pageFilterTypeComboBox, Action.PageFilter.Type);
        _pageFilterRangeTextBox.Text = FormatPageRanges(Action.PageFilter.Range);
        _leftNumericBox.Value = FloatToDecimal(Action.Left);
        _topNumericBox.Value = FloatToDecimal(Action.Top);
        _rightNumericBox.Value = FloatToDecimal(Action.Right);
        _bottomNumericBox.Value = FloatToDecimal(Action.Bottom);
        _enableWidthCheckBox.Checked = type != PdfActionType.AdjustSize || Action.EnableWidth;
        _enableHeightCheckBox.Checked = type != PdfActionType.AdjustSize || Action.EnableHeight;
        _targetedWidthNumericBox.Enabled = type != PdfActionType.AdjustSize || Action.EnableWidth;
        _targetedHeightNumericBox.Enabled = type != PdfActionType.AdjustSize || Action.EnableHeight;
        _targetedWidthNumericBox.Value = FloatToDecimal(Action.TargetedWidth);
        _targetedHeightNumericBox.Value = FloatToDecimal(Action.TargetedHeight);
        _proportionalCheckBox.Checked = Action.Proportional ?? true;
        SelectAnchorButton(Action.AnchorHorizontal, Action.AnchorVertical);
        SelectEnum(_rulerOrientationComboBox, Action.Orientation);
        SelectEnum(_rulerValueModeComboBox, Action.RulerValueMode);
        SelectEnum(_rulerStyleComboBox, Action.Style);
        _rulerPositionNumericBox.Value = FloatToDecimal(Action.Position);
        _rulerColorTextBox.Text = Action.Color;
        _previewCheckBox.Checked = PreviewEnabled;
        _parametersPanel.ResumeLayout(true);
        _isBinding = false;
    }

    private void AddRow(string labelText, Control editor)
    {
        var rowIndex = _parametersPanel.RowCount++;
        _parametersPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var label = new Label
        {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Text = labelText,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 4, 8, 4),
        };
        editor.Dock = DockStyle.Fill;
        editor.Margin = new Padding(0, 4, 0, 4);
        _parametersPanel.Controls.Add(label, 0, rowIndex);
        _parametersPanel.Controls.Add(editor, 1, rowIndex);
    }

    private static Panel CreateOptionalPanel(CheckBox checkBox, NumericUpDown numericBox, bool useCheckBox)
    {
        var panel = new Panel { Height = 28 };
        checkBox.AutoSize = true;
        checkBox.Visible = useCheckBox;
        checkBox.Location = new Point(0, 5);
        numericBox.Location = new Point(useCheckBox ? 24 : 0, 0);
        numericBox.Width = 180;
        panel.Controls.Add(checkBox);
        panel.Controls.Add(numericBox);
        return panel;
    }

    private Panel CreateColorPanel()
    {
        var panel = new Panel { Height = 28 };
        _rulerColorTextBox.Location = new Point(0, 0);
        _rulerColorTextBox.Width = 130;
        _rulerColorButton.Location = new Point(138, 0);
        _rulerColorButton.Width = 90;
        panel.Controls.Add(_rulerColorTextBox);
        panel.Controls.Add(_rulerColorButton);
        return panel;
    }

    private void ConfigureAnchorPanel()
    {
        _anchorPanel.ColumnCount = 3;
        _anchorPanel.RowCount = 3;
        _anchorPanel.Width = 90;
        _anchorPanel.Height = 78;
        for (var index = 0; index < 3; index++)
        {
            _anchorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            _anchorPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
        }

        AddAnchorButton(AnchorHorizontal.Left, AnchorVertical.Top, 0, 0);
        AddAnchorButton(AnchorHorizontal.Center, AnchorVertical.Top, 1, 0);
        AddAnchorButton(AnchorHorizontal.Right, AnchorVertical.Top, 2, 0);
        AddAnchorButton(AnchorHorizontal.Left, AnchorVertical.Center, 0, 1);
        AddAnchorButton(AnchorHorizontal.Center, AnchorVertical.Center, 1, 1);
        AddAnchorButton(AnchorHorizontal.Right, AnchorVertical.Center, 2, 1);
        AddAnchorButton(AnchorHorizontal.Left, AnchorVertical.Bottom, 0, 2);
        AddAnchorButton(AnchorHorizontal.Center, AnchorVertical.Bottom, 1, 2);
        AddAnchorButton(AnchorHorizontal.Right, AnchorVertical.Bottom, 2, 2);
    }

    private void AddAnchorButton(AnchorHorizontal horizontal, AnchorVertical vertical, int column, int row)
    {
        var button = new RadioButton
        {
            Appearance = Appearance.Button,
            Dock = DockStyle.Fill,
            Margin = new Padding(1),
            Text = "",
            Tag = (horizontal, vertical),
        };
        button.CheckedChanged += (_, _) =>
        {
            if (button.Checked)
            {
                UpdateActionFromInputs();
            }
        };
        _anchorButtons[(horizontal, vertical)] = button;
        _anchorPanel.Controls.Add(button, column, row);
    }

    private void UpdateActionFromInputs()
    {
        if (_isBinding)
        {
            return;
        }

        if (TryGetSelectedEnum(_typeComboBox, out PdfActionType type) && type != Action.Type)
        {
            Action.Type = type;
            ApplyActionDefaults(Action, overwriteName: true);
            BindAction();
            RaiseActionChanged();
            return;
        }

        Action.Name = _nameTextBox.Text;
        Action.UseDefaultName = false;
        if (TryGetSelectedEnum(_pageFilterTypeComboBox, out PageFilterType pageFilterType))
        {
            Action.PageFilter.Type = pageFilterType;
        }

        if (TryParsePageRanges(_pageFilterRangeTextBox.Text, out var ranges, out _))
        {
            _errorProvider.SetError(_pageFilterRangeTextBox, "");
            Action.PageFilter.Range = ranges;
        }

        Action.Left = (float)_leftNumericBox.Value;
        Action.Top = (float)_topNumericBox.Value;
        Action.Right = (float)_rightNumericBox.Value;
        Action.Bottom = (float)_bottomNumericBox.Value;
        Action.EnableWidth = _enableWidthCheckBox.Checked;
        Action.EnableHeight = _enableHeightCheckBox.Checked;
        Action.TargetedWidth = Action.Type == PdfActionType.AdjustSize && !Action.EnableWidth ? null : ZeroToNull((float)_targetedWidthNumericBox.Value);
        Action.TargetedHeight = Action.Type == PdfActionType.AdjustSize && !Action.EnableHeight ? null : ZeroToNull((float)_targetedHeightNumericBox.Value);
        Action.Proportional = _proportionalCheckBox.Checked;
        if (_anchorButtons.FirstOrDefault(pair => pair.Value.Checked).Key is var anchor)
        {
            Action.AnchorHorizontal = anchor.Horizontal;
            Action.AnchorVertical = anchor.Vertical;
        }

        if (TryGetSelectedEnum(_rulerOrientationComboBox, out RulerOrientation orientation))
        {
            Action.Orientation = orientation;
        }

        if (TryGetSelectedEnum(_rulerValueModeComboBox, out RulerValueMode mode))
        {
            Action.RulerValueMode = mode;
        }

        if (TryGetSelectedEnum(_rulerStyleComboBox, out RulerStyle style))
        {
            Action.Style = style;
        }

        Action.Position = (float)_rulerPositionNumericBox.Value;
        Action.Color = _rulerColorTextBox.Text;
        RaiseActionChanged();
    }

    private void RaiseActionChanged()
    {
        if (PreviewEnabled)
        {
            ActionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void PreviewCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (_isBinding)
        {
            return;
        }

        PreviewEnabled = _previewCheckBox.Checked;
        PreviewEnabledChanged?.Invoke(this, EventArgs.Empty);
        if (PreviewEnabled)
        {
            ActionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void TypeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        UpdateActionFromInputs();
    }

    private void RulerColorButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new ColorDialog
        {
            Color = ParseColor(_rulerColorTextBox.Text),
            FullOpen = true,
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _rulerColorTextBox.Text = ColorTranslator.ToHtml(dialog.Color);
    }

    private void ApplyButton_Click(object? sender, EventArgs e)
    {
        if (!TryParsePageRanges(_pageFilterRangeTextBox.Text, out var ranges, out var error))
        {
            _errorProvider.SetError(_pageFilterRangeTextBox, error);
            DialogResult = DialogResult.None;
            return;
        }

        Action.PageFilter.Range = ranges;
        UpdateActionFromInputs();
    }

    private static NumericUpDown CreateNumericBox()
    {
        return new NumericUpDown
        {
            DecimalPlaces = 3,
            Minimum = -1000,
            Maximum = 1000,
            Increment = 0.001M,
            Width = 180,
        };
    }

    private static void ConfigureNumeric(NumericUpDown numericBox)
    {
        numericBox.ValueChanged += (_, _) =>
        {
            if (numericBox.FindForm() is ActionEditForm form)
            {
                form.UpdateActionFromInputs();
            }
        };
    }

    private void ConfigureRulerValueModeCombo()
    {
        _rulerValueModeComboBox.DisplayMember = nameof(EnumComboItem<RulerValueMode>.Text);
        _rulerValueModeComboBox.ValueMember = nameof(EnumComboItem<RulerValueMode>.Value);
        _rulerValueModeComboBox.Items.Clear();
        _rulerValueModeComboBox.Items.Add(new EnumComboItem<RulerValueMode>(RulerValueMode.Percent, TranslationService.T("enum.rulerMode.Percent")));
        _rulerValueModeComboBox.Items.Add(new EnumComboItem<RulerValueMode>(RulerValueMode.Unit, TranslationService.T("enum.unit." + _unitType)));
        SelectEnum(_rulerValueModeComboBox, Action.RulerValueMode);
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

    private void SelectAnchorButton(AnchorHorizontal horizontal, AnchorVertical vertical)
    {
        if (_anchorButtons.TryGetValue((horizontal, vertical), out var button))
        {
            button.Checked = true;
        }
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

    private static decimal FloatToDecimal(float? value)
    {
        return (decimal)Round(value ?? 0);
    }

    private static float? ZeroToNull(float value)
    {
        return Math.Abs(value) < 0.0001f ? null : Round(value);
    }

    private static float Round(float value)
    {
        return (float)Math.Round(value, 3, MidpointRounding.AwayFromZero);
    }

    private static string GetActionDisplayName(PdfActionType type)
    {
        return TranslationService.T("enum.action." + type);
    }

    private static string GetActionDescription(PdfActionType type)
    {
        return TranslationService.T("enum.action." + type + ".description");
    }

    private static void ApplyActionDefaults(ProjectAction action, bool overwriteName)
    {
        if (overwriteName)
        {
            action.Name = GetActionDisplayName(action.Type);
            action.UseDefaultName = true;
        }

        action.PageFilter ??= new PageFilter();
        action.PageFilter.Range ??= [];
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
            case PdfActionType.Zoom:
                action.Proportional ??= true;
                break;
            case PdfActionType.AdjustSize:
                action.EnableWidth = action.EnableWidth || action.TargetedWidth is > 0;
                action.EnableHeight = action.EnableHeight || action.TargetedHeight is > 0;
                break;
            case PdfActionType.AddRuler:
                action.Color = string.IsNullOrWhiteSpace(action.Color) ? "#FF0000" : action.Color;
                action.RulerValueMode = action.Position == null ? RulerValueMode.Unit : action.RulerValueMode;
                action.Position ??= action.RulerValueMode == RulerValueMode.Percent ? 50 : 0;
                break;
            case PdfActionType.AddFrame:
                action.Left ??= 0.25f;
                action.Top ??= 0.25f;
                action.Right ??= 0.25f;
                action.Bottom ??= 0.25f;
                action.Color = string.IsNullOrWhiteSpace(action.Color) ? "#FF0000" : action.Color;
                break;
        }
    }

    private static ProjectAction CloneAction(ProjectAction source)
    {
        return new ProjectAction
        {
            Id = source.Id,
            Enabled = source.Enabled,
            Type = source.Type,
            Name = source.Name,
            UseDefaultName = source.UseDefaultName,
            PageFilter = new PageFilter
            {
                Type = source.PageFilter.Type,
                Range = source.PageFilter.Range
                    .Select(range => new PageFilterRange { Start = range.Start, End = range.End })
                    .ToList(),
            },
            Left = source.Left,
            Top = source.Top,
            Right = source.Right,
            Bottom = source.Bottom,
            TargetedWidth = source.TargetedWidth,
            TargetedHeight = source.TargetedHeight,
            EnableWidth = source.EnableWidth,
            EnableHeight = source.EnableHeight,
            Proportional = source.Proportional,
            AnchorHorizontal = source.AnchorHorizontal,
            AnchorVertical = source.AnchorVertical,
            Color = source.Color,
            Style = source.Style,
            RulerValueMode = source.RulerValueMode,
            Orientation = source.Orientation,
            Position = source.Position,
        };
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
                    ? range.Start.ToString(CultureInfo.InvariantCulture)
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
}
