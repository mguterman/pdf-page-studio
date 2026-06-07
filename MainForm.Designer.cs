namespace PdfResizer;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private TabControl mainTabControl;
    private TabPage trimTabPage;
    private TabPage adjustTabPage;

    private TableLayoutPanel trimRootLayout;
    private Label trimChoosePdfLabel;
    private TextBox trimInputTextBox;
    private Button trimBrowsePdfButton;
    private PdfPreviewBox trimPreviewBox;
    private TableLayoutPanel trimSettingsLayout;
    private Label trimLeftLabel;
    private NumericUpDown trimLeftInput;
    private Label trimTopLabel;
    private NumericUpDown trimTopInput;
    private Label trimRightLabel;
    private NumericUpDown trimRightInput;
    private Label trimBottomLabel;
    private NumericUpDown trimBottomInput;
    private Label trimUnitsLabel;
    private Label trimFrameSizeTitleLabel;
    private TextBox trimFrameSizeTextBox;
    private Label trimDestinationLabel;
    private TextBox trimDestinationTextBox;
    private FlowLayoutPanel trimDestinationButtonsPanel;
    private Button trimDestinationButton;
    private Button trimOpenDestinationButton;
    private Label trimOutputLabel;
    private TextBox trimOutputTextBox;
    private Button trimButton;
    private Label trimStatusLabel;

    private TableLayoutPanel adjustRootLayout;
    private Label adjustChoosePdfLabel;
    private TextBox adjustInputTextBox;
    private Button adjustBrowsePdfButton;
    private GroupBox adjustSettingsGroupBox;
    private TableLayoutPanel adjustSettingsLayout;
    private Label targetedPageSizeLabel;
    private FlowLayoutPanel pageSizePanel;
    private NumericUpDown widthInput;
    private Label pageSizeSeparatorLabel;
    private NumericUpDown heightInput;
    private Label inchesLabel;
    private Label shiftLabel;
    private FlowLayoutPanel shiftPanel;
    private NumericUpDown shiftInput;
    private Label shiftUnitsLabel;
    private Label adjustDestinationLabel;
    private TextBox adjustDestinationTextBox;
    private FlowLayoutPanel adjustDestinationButtonsPanel;
    private Button adjustDestinationButton;
    private Button adjustOpenDestinationButton;
    private Label adjustOutputLabel;
    private TextBox adjustOutputTextBox;
    private Button adjustConvertButton;
    private Label adjustStatusLabel;
    private NumericUpDown startPageInput;
    private NumericUpDown endPageInput;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        mainTabControl = new TabControl();
        trimTabPage = new TabPage();
        adjustTabPage = new TabPage();
        trimRootLayout = new TableLayoutPanel();
        trimChoosePdfLabel = new Label();
        trimInputTextBox = new TextBox();
        trimBrowsePdfButton = new Button();
        trimPreviewBox = new PdfPreviewBox();
        trimSettingsLayout = new TableLayoutPanel();
        trimLeftLabel = new Label();
        trimLeftInput = new NumericUpDown();
        trimTopLabel = new Label();
        trimTopInput = new NumericUpDown();
        trimRightLabel = new Label();
        trimRightInput = new NumericUpDown();
        trimBottomLabel = new Label();
        trimBottomInput = new NumericUpDown();
        trimUnitsLabel = new Label();
        trimFrameSizeTitleLabel = new Label();
        trimFrameSizeTextBox = new TextBox();
        trimDestinationLabel = new Label();
        trimDestinationTextBox = new TextBox();
        trimDestinationButtonsPanel = new FlowLayoutPanel();
        trimDestinationButton = new Button();
        trimOpenDestinationButton = new Button();
        trimOutputLabel = new Label();
        trimOutputTextBox = new TextBox();
        trimButton = new Button();
        trimStatusLabel = new Label();
        adjustRootLayout = new TableLayoutPanel();
        adjustChoosePdfLabel = new Label();
        adjustInputTextBox = new TextBox();
        adjustBrowsePdfButton = new Button();
        adjustSettingsGroupBox = new GroupBox();
        adjustSettingsLayout = new TableLayoutPanel();
        targetedPageSizeLabel = new Label();
        pageSizePanel = new FlowLayoutPanel();
        widthInput = new NumericUpDown();
        pageSizeSeparatorLabel = new Label();
        heightInput = new NumericUpDown();
        inchesLabel = new Label();
        shiftLabel = new Label();
        shiftPanel = new FlowLayoutPanel();
        shiftInput = new NumericUpDown();
        shiftUnitsLabel = new Label();
        adjustDestinationLabel = new Label();
        adjustDestinationTextBox = new TextBox();
        adjustDestinationButtonsPanel = new FlowLayoutPanel();
        adjustDestinationButton = new Button();
        adjustOpenDestinationButton = new Button();
        adjustOutputLabel = new Label();
        adjustOutputTextBox = new TextBox();
        adjustConvertButton = new Button();
        adjustStatusLabel = new Label();
        startPageInput = new NumericUpDown();
        endPageInput = new NumericUpDown();
        mainTabControl.SuspendLayout();
        trimTabPage.SuspendLayout();
        adjustTabPage.SuspendLayout();
        trimRootLayout.SuspendLayout();
        trimSettingsLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)trimLeftInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trimTopInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trimRightInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trimBottomInput).BeginInit();
        trimDestinationButtonsPanel.SuspendLayout();
        adjustRootLayout.SuspendLayout();
        adjustSettingsGroupBox.SuspendLayout();
        adjustSettingsLayout.SuspendLayout();
        pageSizePanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)widthInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)heightInput).BeginInit();
        shiftPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)shiftInput).BeginInit();
        adjustDestinationButtonsPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)startPageInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)endPageInput).BeginInit();
        SuspendLayout();

        mainTabControl.Controls.Add(trimTabPage);
        mainTabControl.Controls.Add(adjustTabPage);
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Name = "mainTabControl";
        mainTabControl.SelectedIndex = 0;
        mainTabControl.TabIndex = 0;

        trimTabPage.Controls.Add(trimRootLayout);
        trimTabPage.Name = "trimTabPage";
        trimTabPage.Padding = new Padding(12);
        trimTabPage.TabIndex = 0;
        trimTabPage.Text = "Trim Page";
        trimTabPage.UseVisualStyleBackColor = true;

        trimRootLayout.ColumnCount = 3;
        trimRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        trimRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        trimRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260F));
        trimRootLayout.Controls.Add(trimChoosePdfLabel, 0, 0);
        trimRootLayout.Controls.Add(trimInputTextBox, 1, 0);
        trimRootLayout.Controls.Add(trimBrowsePdfButton, 2, 0);
        trimRootLayout.Controls.Add(trimPreviewBox, 0, 1);
        trimRootLayout.Controls.Add(trimSettingsLayout, 2, 1);
        trimRootLayout.Controls.Add(trimDestinationLabel, 0, 2);
        trimRootLayout.Controls.Add(trimDestinationTextBox, 1, 2);
        trimRootLayout.Controls.Add(trimDestinationButtonsPanel, 2, 2);
        trimRootLayout.Controls.Add(trimFrameSizeTitleLabel, 0, 3);
        trimRootLayout.Controls.Add(trimFrameSizeTextBox, 1, 3);
        trimRootLayout.Controls.Add(trimOutputLabel, 0, 4);
        trimRootLayout.Controls.Add(trimOutputTextBox, 1, 4);
        trimRootLayout.Controls.Add(trimButton, 2, 4);
        trimRootLayout.Controls.Add(trimStatusLabel, 1, 5);
        trimRootLayout.Dock = DockStyle.Fill;
        trimRootLayout.Name = "trimRootLayout";
        trimRootLayout.RowCount = 6;
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        trimRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        trimRootLayout.SetColumnSpan(trimPreviewBox, 2);
        trimRootLayout.TabIndex = 0;

        trimChoosePdfLabel.AutoSize = true;
        trimChoosePdfLabel.Dock = DockStyle.Fill;
        trimChoosePdfLabel.Name = "trimChoosePdfLabel";
        trimChoosePdfLabel.TabIndex = 0;
        trimChoosePdfLabel.Text = "Choose PDF file";
        trimChoosePdfLabel.TextAlign = ContentAlignment.MiddleLeft;

        trimInputTextBox.Dock = DockStyle.Fill;
        trimInputTextBox.Name = "trimInputTextBox";
        trimInputTextBox.ReadOnly = true;
        trimInputTextBox.TabIndex = 1;

        trimBrowsePdfButton.Dock = DockStyle.Fill;
        trimBrowsePdfButton.Name = "trimBrowsePdfButton";
        trimBrowsePdfButton.TabIndex = 2;
        trimBrowsePdfButton.Text = "Browse...";
        trimBrowsePdfButton.UseVisualStyleBackColor = true;
        trimBrowsePdfButton.Click += TrimBrowsePdfButton_Click;

        trimPreviewBox.Dock = DockStyle.Fill;
        trimPreviewBox.Name = "trimPreviewBox";
        trimPreviewBox.TabIndex = 3;

        trimSettingsLayout.ColumnCount = 2;
        trimSettingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        trimSettingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        trimSettingsLayout.Controls.Add(trimLeftLabel, 0, 0);
        trimSettingsLayout.Controls.Add(trimLeftInput, 0, 1);
        trimSettingsLayout.Controls.Add(trimTopLabel, 1, 0);
        trimSettingsLayout.Controls.Add(trimTopInput, 1, 1);
        trimSettingsLayout.Controls.Add(trimRightLabel, 0, 2);
        trimSettingsLayout.Controls.Add(trimRightInput, 0, 3);
        trimSettingsLayout.Controls.Add(trimBottomLabel, 1, 2);
        trimSettingsLayout.Controls.Add(trimBottomInput, 1, 3);
        trimSettingsLayout.Controls.Add(trimUnitsLabel, 0, 4);
        trimSettingsLayout.Dock = DockStyle.Top;
        trimSettingsLayout.Name = "trimSettingsLayout";
        trimSettingsLayout.RowCount = 6;
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        trimSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        trimSettingsLayout.SetColumnSpan(trimUnitsLabel, 2);
        trimSettingsLayout.TabIndex = 4;

        trimLeftLabel.AutoSize = true;
        trimLeftLabel.Dock = DockStyle.Fill;
        trimLeftLabel.Name = "trimLeftLabel";
        trimLeftLabel.Text = "Left";
        trimLeftLabel.TextAlign = ContentAlignment.BottomLeft;

        trimLeftInput.Dock = DockStyle.Fill;
        trimLeftInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        trimLeftInput.Name = "trimLeftInput";
        trimLeftInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
        trimLeftInput.ValueChanged += TrimSetting_ValueChanged;

        trimTopLabel.AutoSize = true;
        trimTopLabel.Dock = DockStyle.Fill;
        trimTopLabel.Name = "trimTopLabel";
        trimTopLabel.Text = "Top";
        trimTopLabel.TextAlign = ContentAlignment.BottomLeft;

        trimTopInput.Dock = DockStyle.Fill;
        trimTopInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        trimTopInput.Name = "trimTopInput";
        trimTopInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
        trimTopInput.ValueChanged += TrimSetting_ValueChanged;

        trimRightLabel.AutoSize = true;
        trimRightLabel.Dock = DockStyle.Fill;
        trimRightLabel.Name = "trimRightLabel";
        trimRightLabel.Text = "Right";
        trimRightLabel.TextAlign = ContentAlignment.BottomLeft;

        trimRightInput.Dock = DockStyle.Fill;
        trimRightInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        trimRightInput.Name = "trimRightInput";
        trimRightInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
        trimRightInput.ValueChanged += TrimSetting_ValueChanged;

        trimBottomLabel.AutoSize = true;
        trimBottomLabel.Dock = DockStyle.Fill;
        trimBottomLabel.Name = "trimBottomLabel";
        trimBottomLabel.Text = "Bottom";
        trimBottomLabel.TextAlign = ContentAlignment.BottomLeft;

        trimBottomInput.Dock = DockStyle.Fill;
        trimBottomInput.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        trimBottomInput.Name = "trimBottomInput";
        trimBottomInput.Value = new decimal(new int[] { 100, 0, 0, 0 });
        trimBottomInput.ValueChanged += TrimSetting_ValueChanged;

        trimUnitsLabel.AutoSize = true;
        trimUnitsLabel.Dock = DockStyle.Fill;
        trimUnitsLabel.Name = "trimUnitsLabel";
        trimUnitsLabel.Text = "Values are hundredths of an inch.";
        trimUnitsLabel.TextAlign = ContentAlignment.MiddleLeft;

        trimFrameSizeTitleLabel.AutoSize = true;
        trimFrameSizeTitleLabel.Dock = DockStyle.Fill;
        trimFrameSizeTitleLabel.Name = "trimFrameSizeTitleLabel";
        trimFrameSizeTitleLabel.Text = "Frame size";
        trimFrameSizeTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        trimFrameSizeTextBox.Dock = DockStyle.Fill;
        trimFrameSizeTextBox.Font = new Font(trimFrameSizeTextBox.Font, FontStyle.Bold);
        trimFrameSizeTextBox.Name = "trimFrameSizeTextBox";
        trimFrameSizeTextBox.ReadOnly = true;
        trimFrameSizeTextBox.Text = "0 x 0 inches";

        trimDestinationLabel.AutoSize = true;
        trimDestinationLabel.Dock = DockStyle.Fill;
        trimDestinationLabel.Name = "trimDestinationLabel";
        trimDestinationLabel.Text = "Destination folder";
        trimDestinationLabel.TextAlign = ContentAlignment.MiddleLeft;

        trimDestinationTextBox.Dock = DockStyle.Fill;
        trimDestinationTextBox.Name = "trimDestinationTextBox";
        trimDestinationTextBox.PlaceholderText = "Optional. Default: same folder as selected PDF file.";
        trimDestinationTextBox.ReadOnly = true;

        trimDestinationButtonsPanel.Controls.Add(trimDestinationButton);
        trimDestinationButtonsPanel.Controls.Add(trimOpenDestinationButton);
        trimDestinationButtonsPanel.Dock = DockStyle.Fill;
        trimDestinationButtonsPanel.Margin = Padding.Empty;
        trimDestinationButtonsPanel.Name = "trimDestinationButtonsPanel";
        trimDestinationButtonsPanel.WrapContents = false;

        trimDestinationButton.Margin = new Padding(0, 0, 6, 0);
        trimDestinationButton.Name = "trimDestinationButton";
        trimDestinationButton.Size = new Size(82, 28);
        trimDestinationButton.Text = "Browse...";
        trimDestinationButton.UseVisualStyleBackColor = true;
        trimDestinationButton.Click += TrimDestinationButton_Click;

        trimOpenDestinationButton.Enabled = false;
        trimOpenDestinationButton.Margin = Padding.Empty;
        trimOpenDestinationButton.Name = "trimOpenDestinationButton";
        trimOpenDestinationButton.Size = new Size(58, 28);
        trimOpenDestinationButton.Text = "Open";
        trimOpenDestinationButton.UseVisualStyleBackColor = true;
        trimOpenDestinationButton.Click += TrimOpenDestinationButton_Click;

        trimOutputLabel.AutoSize = true;
        trimOutputLabel.Dock = DockStyle.Fill;
        trimOutputLabel.Name = "trimOutputLabel";
        trimOutputLabel.Text = "Output file";
        trimOutputLabel.TextAlign = ContentAlignment.MiddleLeft;

        trimOutputTextBox.Dock = DockStyle.Fill;
        trimOutputTextBox.Name = "trimOutputTextBox";
        trimOutputTextBox.ReadOnly = true;

        trimButton.Dock = DockStyle.Fill;
        trimButton.Enabled = false;
        trimButton.Name = "trimButton";
        trimButton.Text = "Trim";
        trimButton.UseVisualStyleBackColor = true;
        trimButton.Click += TrimButton_Click;

        trimStatusLabel.AutoSize = true;
        trimStatusLabel.Dock = DockStyle.Fill;
        trimStatusLabel.Name = "trimStatusLabel";
        trimStatusLabel.TextAlign = ContentAlignment.MiddleLeft;

        adjustTabPage.Controls.Add(adjustRootLayout);
        adjustTabPage.Name = "adjustTabPage";
        adjustTabPage.Padding = new Padding(12);
        adjustTabPage.TabIndex = 1;
        adjustTabPage.Text = "Adjust Page Size";
        adjustTabPage.UseVisualStyleBackColor = true;

        adjustRootLayout.ColumnCount = 3;
        adjustRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        adjustRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        adjustRootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        adjustRootLayout.Controls.Add(adjustChoosePdfLabel, 0, 0);
        adjustRootLayout.Controls.Add(adjustInputTextBox, 1, 0);
        adjustRootLayout.Controls.Add(adjustBrowsePdfButton, 2, 0);
        adjustRootLayout.Controls.Add(adjustSettingsGroupBox, 0, 1);
        adjustRootLayout.Controls.Add(adjustDestinationLabel, 0, 2);
        adjustRootLayout.Controls.Add(adjustDestinationTextBox, 1, 2);
        adjustRootLayout.Controls.Add(adjustDestinationButtonsPanel, 2, 2);
        adjustRootLayout.Controls.Add(adjustOutputLabel, 0, 3);
        adjustRootLayout.Controls.Add(adjustOutputTextBox, 1, 3);
        adjustRootLayout.Controls.Add(adjustConvertButton, 2, 3);
        adjustRootLayout.Controls.Add(adjustStatusLabel, 1, 4);
        adjustRootLayout.Dock = DockStyle.Fill;
        adjustRootLayout.Name = "adjustRootLayout";
        adjustRootLayout.RowCount = 5;
        adjustRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        adjustRootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        adjustRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        adjustRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        adjustRootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        adjustRootLayout.SetColumnSpan(adjustSettingsGroupBox, 3);

        adjustChoosePdfLabel.AutoSize = true;
        adjustChoosePdfLabel.Dock = DockStyle.Fill;
        adjustChoosePdfLabel.Text = "Choose PDF file";
        adjustChoosePdfLabel.TextAlign = ContentAlignment.MiddleLeft;

        adjustInputTextBox.Dock = DockStyle.Fill;
        adjustInputTextBox.ReadOnly = true;

        adjustBrowsePdfButton.Dock = DockStyle.Fill;
        adjustBrowsePdfButton.Text = "Browse...";
        adjustBrowsePdfButton.UseVisualStyleBackColor = true;
        adjustBrowsePdfButton.Click += AdjustBrowsePdfButton_Click;

        adjustSettingsGroupBox.Controls.Add(adjustSettingsLayout);
        adjustSettingsGroupBox.Dock = DockStyle.Fill;
        adjustSettingsGroupBox.Text = "Adjust Page Size";

        adjustSettingsLayout.ColumnCount = 2;
        adjustSettingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        adjustSettingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        adjustSettingsLayout.Controls.Add(targetedPageSizeLabel, 0, 0);
        adjustSettingsLayout.Controls.Add(pageSizePanel, 1, 0);
        adjustSettingsLayout.Controls.Add(shiftLabel, 0, 1);
        adjustSettingsLayout.Controls.Add(shiftPanel, 1, 1);
        adjustSettingsLayout.Dock = DockStyle.Top;
        adjustSettingsLayout.Name = "adjustSettingsLayout";
        adjustSettingsLayout.Padding = new Padding(12);
        adjustSettingsLayout.RowCount = 4;
        adjustSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        adjustSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        adjustSettingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        targetedPageSizeLabel.AutoSize = true;
        targetedPageSizeLabel.Dock = DockStyle.Fill;
        targetedPageSizeLabel.Text = "Targeted Page Size";
        targetedPageSizeLabel.TextAlign = ContentAlignment.MiddleLeft;

        pageSizePanel.Controls.Add(widthInput);
        pageSizePanel.Controls.Add(pageSizeSeparatorLabel);
        pageSizePanel.Controls.Add(heightInput);
        pageSizePanel.Controls.Add(inchesLabel);
        pageSizePanel.Dock = DockStyle.Fill;
        pageSizePanel.Margin = Padding.Empty;
        pageSizePanel.WrapContents = false;

        widthInput.DecimalPlaces = 2;
        widthInput.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
        widthInput.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        widthInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        widthInput.Size = new Size(90, 23);
        widthInput.Value = new decimal(new int[] { 6, 0, 0, 0 });
        widthInput.ValueChanged += AdjustSetting_ValueChanged;

        pageSizeSeparatorLabel.AutoSize = true;
        pageSizeSeparatorLabel.Margin = new Padding(8, 7, 4, 0);
        pageSizeSeparatorLabel.Text = "x";

        heightInput.DecimalPlaces = 2;
        heightInput.Increment = new decimal(new int[] { 25, 0, 0, 131072 });
        heightInput.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
        heightInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        heightInput.Size = new Size(90, 23);
        heightInput.Value = new decimal(new int[] { 9, 0, 0, 0 });
        heightInput.ValueChanged += AdjustSetting_ValueChanged;

        inchesLabel.AutoSize = true;
        inchesLabel.Margin = new Padding(8, 7, 4, 0);
        inchesLabel.Text = "inches";

        shiftLabel.AutoSize = true;
        shiftLabel.Dock = DockStyle.Fill;
        shiftLabel.Text = "Shift";
        shiftLabel.TextAlign = ContentAlignment.MiddleLeft;

        shiftPanel.Controls.Add(shiftInput);
        shiftPanel.Controls.Add(shiftUnitsLabel);
        shiftPanel.Dock = DockStyle.Fill;
        shiftPanel.Margin = Padding.Empty;
        shiftPanel.WrapContents = false;

        shiftInput.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
        shiftInput.Minimum = new decimal(new int[] { 200, 0, 0, int.MinValue });
        shiftInput.Size = new Size(72, 23);
        shiftInput.Value = new decimal(new int[] { 22, 0, 0, 0 });
        shiftInput.ValueChanged += AdjustSetting_ValueChanged;

        shiftUnitsLabel.AutoSize = true;
        shiftUnitsLabel.Margin = new Padding(8, 7, 4, 0);
        shiftUnitsLabel.Text = "hundredths of an inch";

        adjustDestinationLabel.AutoSize = true;
        adjustDestinationLabel.Dock = DockStyle.Fill;
        adjustDestinationLabel.Text = "Destination folder";
        adjustDestinationLabel.TextAlign = ContentAlignment.MiddleLeft;

        adjustDestinationTextBox.Dock = DockStyle.Fill;
        adjustDestinationTextBox.PlaceholderText = "Optional. Default: same folder as selected PDF file.";
        adjustDestinationTextBox.ReadOnly = true;

        adjustDestinationButtonsPanel.Controls.Add(adjustDestinationButton);
        adjustDestinationButtonsPanel.Controls.Add(adjustOpenDestinationButton);
        adjustDestinationButtonsPanel.Dock = DockStyle.Fill;
        adjustDestinationButtonsPanel.Margin = Padding.Empty;
        adjustDestinationButtonsPanel.WrapContents = false;

        adjustDestinationButton.Margin = new Padding(0, 0, 6, 0);
        adjustDestinationButton.Size = new Size(82, 28);
        adjustDestinationButton.Text = "Browse...";
        adjustDestinationButton.UseVisualStyleBackColor = true;
        adjustDestinationButton.Click += AdjustDestinationButton_Click;

        adjustOpenDestinationButton.Enabled = false;
        adjustOpenDestinationButton.Margin = Padding.Empty;
        adjustOpenDestinationButton.Size = new Size(58, 28);
        adjustOpenDestinationButton.Text = "Open";
        adjustOpenDestinationButton.UseVisualStyleBackColor = true;
        adjustOpenDestinationButton.Click += AdjustOpenDestinationButton_Click;

        adjustOutputLabel.AutoSize = true;
        adjustOutputLabel.Dock = DockStyle.Fill;
        adjustOutputLabel.Text = "Output file";
        adjustOutputLabel.TextAlign = ContentAlignment.MiddleLeft;

        adjustOutputTextBox.Dock = DockStyle.Fill;
        adjustOutputTextBox.ReadOnly = true;

        adjustConvertButton.Dock = DockStyle.Fill;
        adjustConvertButton.Enabled = false;
        adjustConvertButton.Text = "Convert";
        adjustConvertButton.UseVisualStyleBackColor = true;
        adjustConvertButton.Click += AdjustConvertButton_Click;

        adjustStatusLabel.AutoSize = true;
        adjustStatusLabel.Dock = DockStyle.Fill;
        adjustStatusLabel.TextAlign = ContentAlignment.MiddleLeft;

        startPageInput.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        startPageInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        startPageInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
        startPageInput.Visible = false;

        endPageInput.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        endPageInput.Visible = false;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1220, 760);
        Controls.Add(mainTabControl);
        MinimumSize = new Size(1080, 680);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PDF Resizer";

        mainTabControl.ResumeLayout(false);
        trimTabPage.ResumeLayout(false);
        adjustTabPage.ResumeLayout(false);
        trimRootLayout.ResumeLayout(false);
        trimRootLayout.PerformLayout();
        trimSettingsLayout.ResumeLayout(false);
        trimSettingsLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)trimLeftInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)trimTopInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)trimRightInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)trimBottomInput).EndInit();
        trimDestinationButtonsPanel.ResumeLayout(false);
        adjustRootLayout.ResumeLayout(false);
        adjustRootLayout.PerformLayout();
        adjustSettingsGroupBox.ResumeLayout(false);
        adjustSettingsLayout.ResumeLayout(false);
        adjustSettingsLayout.PerformLayout();
        pageSizePanel.ResumeLayout(false);
        pageSizePanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)widthInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)heightInput).EndInit();
        shiftPanel.ResumeLayout(false);
        shiftPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)shiftInput).EndInit();
        adjustDestinationButtonsPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)startPageInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)endPageInput).EndInit();
        ResumeLayout(false);
    }
}
