namespace PdfPageStudio;

partial class ProjectSetupForm
{
    private TableLayoutPanel rootPanel;
    private Label nameLabel;
    private TextBox nameTextBox;
    private Label unitTypeLabel;
    private ComboBox unitTypeComboBox;
    private Label descriptionLabel;
    private TextBox descriptionTextBox;
    private FlowLayoutPanel buttonPanel;
    private Button createButton;
    private Button cancelButton;

    private void InitializeComponent()
    {
        rootPanel = new TableLayoutPanel();
        nameLabel = new Label();
        nameTextBox = new TextBox();
        unitTypeLabel = new Label();
        unitTypeComboBox = new ComboBox();
        descriptionLabel = new Label();
        descriptionTextBox = new TextBox();
        buttonPanel = new FlowLayoutPanel();
        createButton = new Button();
        cancelButton = new Button();
        rootPanel.SuspendLayout();
        buttonPanel.SuspendLayout();
        SuspendLayout();

        rootPanel.ColumnCount = 2;
        rootPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        rootPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rootPanel.Controls.Add(nameLabel, 0, 0);
        rootPanel.Controls.Add(nameTextBox, 1, 0);
        rootPanel.Controls.Add(unitTypeLabel, 0, 1);
        rootPanel.Controls.Add(unitTypeComboBox, 1, 1);
        rootPanel.Controls.Add(descriptionLabel, 0, 2);
        rootPanel.Controls.Add(descriptionTextBox, 1, 2);
        rootPanel.Controls.Add(buttonPanel, 0, 3);
        rootPanel.Dock = DockStyle.Fill;
        rootPanel.Name = "rootPanel";
        rootPanel.Padding = new Padding(14);
        rootPanel.RowCount = 4;
        rootPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        rootPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        rootPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        rootPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        rootPanel.SetColumnSpan(buttonPanel, 2);
        rootPanel.TabIndex = 0;

        nameLabel.AutoSize = true;
        nameLabel.Dock = DockStyle.Fill;
        nameLabel.Margin = new Padding(0, 4, 8, 4);
        nameLabel.Name = "nameLabel";
        nameLabel.Text = TranslationService.T("project.name");
        nameLabel.TextAlign = ContentAlignment.MiddleLeft;

        nameTextBox.Dock = DockStyle.Fill;
        nameTextBox.Name = "nameTextBox";
        nameTextBox.TabIndex = 0;

        unitTypeLabel.AutoSize = true;
        unitTypeLabel.Dock = DockStyle.Fill;
        unitTypeLabel.Margin = new Padding(0, 4, 8, 4);
        unitTypeLabel.Name = "unitTypeLabel";
        unitTypeLabel.Text = TranslationService.T("project.unitType");
        unitTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

        unitTypeComboBox.Dock = DockStyle.Left;
        unitTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        unitTypeComboBox.Name = "unitTypeComboBox";
        unitTypeComboBox.Size = new Size(140, 23);
        unitTypeComboBox.TabIndex = 1;

        descriptionLabel.AutoSize = true;
        descriptionLabel.Dock = DockStyle.Fill;
        descriptionLabel.Margin = new Padding(0, 4, 8, 4);
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Text = TranslationService.T("project.description");
        descriptionLabel.TextAlign = ContentAlignment.TopLeft;

        descriptionTextBox.AcceptsReturn = true;
        descriptionTextBox.AcceptsTab = true;
        descriptionTextBox.Dock = DockStyle.Fill;
        descriptionTextBox.Multiline = true;
        descriptionTextBox.Name = "descriptionTextBox";
        descriptionTextBox.ScrollBars = ScrollBars.Vertical;
        descriptionTextBox.TabIndex = 2;

        buttonPanel.AutoSize = true;
        buttonPanel.Controls.Add(createButton);
        buttonPanel.Controls.Add(cancelButton);
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.FlowDirection = FlowDirection.RightToLeft;
        buttonPanel.Name = "buttonPanel";
        buttonPanel.Padding = new Padding(0, 10, 0, 0);
        buttonPanel.TabIndex = 3;

        createButton.DialogResult = DialogResult.OK;
        createButton.Name = "createButton";
        createButton.Size = new Size(100, 30);
        createButton.Text = TranslationService.T("common.create");
        createButton.UseVisualStyleBackColor = true;
        createButton.Click += CreateButton_Click;

        cancelButton.DialogResult = DialogResult.Cancel;
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(90, 30);
        cancelButton.Text = TranslationService.T("common.cancel");
        cancelButton.UseVisualStyleBackColor = true;

        AcceptButton = createButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(460, 320);
        Controls.Add(rootPanel);
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(420, 280);
        Name = "ProjectSetupForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = TranslationService.T("dialog.newProject.title");

        rootPanel.ResumeLayout(false);
        rootPanel.PerformLayout();
        buttonPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
