namespace PdfPageStudio;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileMenuItem;
    private ToolStripMenuItem openProjectMenuItem;
    private ToolStripMenuItem saveProjectMenuItem;
    private ToolStripMenuItem saveProjectAsMenuItem;
    private ToolStripMenuItem openRecentProjectMenuItem;
    private ToolStripSeparator fileMenuSeparator;
    private TableLayoutPanel mainLayoutPanel;
    private Label projectNameLabel;
    private TextBox projectNameTextBox;
    private Label projectDescriptionLabel;
    private TextBox projectDescriptionTextBox;
    private StatusStrip mainStatusStrip;
    private ToolStripStatusLabel statusLabel;

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
        mainMenuStrip = new MenuStrip();
        fileMenuItem = new ToolStripMenuItem();
        openProjectMenuItem = new ToolStripMenuItem();
        saveProjectMenuItem = new ToolStripMenuItem();
        saveProjectAsMenuItem = new ToolStripMenuItem();
        fileMenuSeparator = new ToolStripSeparator();
        openRecentProjectMenuItem = new ToolStripMenuItem();
        mainLayoutPanel = new TableLayoutPanel();
        projectNameLabel = new Label();
        projectNameTextBox = new TextBox();
        projectDescriptionLabel = new Label();
        projectDescriptionTextBox = new TextBox();
        mainStatusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        mainMenuStrip.SuspendLayout();
        mainLayoutPanel.SuspendLayout();
        mainStatusStrip.SuspendLayout();
        SuspendLayout();

        mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem });
        mainMenuStrip.Location = new Point(0, 0);
        mainMenuStrip.Name = "mainMenuStrip";
        mainMenuStrip.Size = new Size(920, 24);
        mainMenuStrip.TabIndex = 0;
        mainMenuStrip.Text = "mainMenuStrip";

        fileMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            openProjectMenuItem,
            saveProjectMenuItem,
            saveProjectAsMenuItem,
            fileMenuSeparator,
            openRecentProjectMenuItem,
        });
        fileMenuItem.Name = "fileMenuItem";
        fileMenuItem.Size = new Size(37, 20);
        fileMenuItem.Text = "&File";

        openProjectMenuItem.Name = "openProjectMenuItem";
        openProjectMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        openProjectMenuItem.Size = new Size(211, 22);
        openProjectMenuItem.Text = "&Open Project...";
        openProjectMenuItem.Click += OpenProjectMenuItem_Click;

        saveProjectMenuItem.Name = "saveProjectMenuItem";
        saveProjectMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        saveProjectMenuItem.Size = new Size(211, 22);
        saveProjectMenuItem.Text = "&Save";
        saveProjectMenuItem.Click += SaveProjectMenuItem_Click;

        saveProjectAsMenuItem.Name = "saveProjectAsMenuItem";
        saveProjectAsMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        saveProjectAsMenuItem.Size = new Size(211, 22);
        saveProjectAsMenuItem.Text = "Save &As...";
        saveProjectAsMenuItem.Click += SaveProjectAsMenuItem_Click;

        fileMenuSeparator.Name = "fileMenuSeparator";
        fileMenuSeparator.Size = new Size(208, 6);

        openRecentProjectMenuItem.Name = "openRecentProjectMenuItem";
        openRecentProjectMenuItem.Size = new Size(211, 22);
        openRecentProjectMenuItem.Text = "Open &Recent Project";

        mainLayoutPanel.ColumnCount = 2;
        mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        mainLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        mainLayoutPanel.Controls.Add(projectNameLabel, 0, 0);
        mainLayoutPanel.Controls.Add(projectNameTextBox, 1, 0);
        mainLayoutPanel.Controls.Add(projectDescriptionLabel, 0, 1);
        mainLayoutPanel.Controls.Add(projectDescriptionTextBox, 1, 1);
        mainLayoutPanel.Dock = DockStyle.Fill;
        mainLayoutPanel.Location = new Point(0, 24);
        mainLayoutPanel.Name = "mainLayoutPanel";
        mainLayoutPanel.Padding = new Padding(16);
        mainLayoutPanel.RowCount = 3;
        mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        mainLayoutPanel.Size = new Size(920, 514);
        mainLayoutPanel.TabIndex = 1;

        projectNameLabel.AutoSize = true;
        projectNameLabel.Dock = DockStyle.Fill;
        projectNameLabel.Name = "projectNameLabel";
        projectNameLabel.Size = new Size(124, 38);
        projectNameLabel.TabIndex = 0;
        projectNameLabel.Text = "Name";
        projectNameLabel.TextAlign = ContentAlignment.MiddleLeft;

        projectNameTextBox.Dock = DockStyle.Fill;
        projectNameTextBox.Location = new Point(149, 19);
        projectNameTextBox.Name = "projectNameTextBox";
        projectNameTextBox.Size = new Size(752, 23);
        projectNameTextBox.TabIndex = 1;
        projectNameTextBox.TextChanged += ProjectNameTextBox_TextChanged;

        projectDescriptionLabel.AutoSize = true;
        projectDescriptionLabel.Dock = DockStyle.Fill;
        projectDescriptionLabel.Name = "projectDescriptionLabel";
        projectDescriptionLabel.Size = new Size(124, 160);
        projectDescriptionLabel.TabIndex = 2;
        projectDescriptionLabel.Text = "Description";
        projectDescriptionLabel.TextAlign = ContentAlignment.TopLeft;

        projectDescriptionTextBox.AcceptsReturn = true;
        projectDescriptionTextBox.AcceptsTab = true;
        projectDescriptionTextBox.Dock = DockStyle.Fill;
        projectDescriptionTextBox.Location = new Point(149, 57);
        projectDescriptionTextBox.Multiline = true;
        projectDescriptionTextBox.Name = "projectDescriptionTextBox";
        projectDescriptionTextBox.ScrollBars = ScrollBars.Vertical;
        projectDescriptionTextBox.Size = new Size(752, 154);
        projectDescriptionTextBox.TabIndex = 3;
        projectDescriptionTextBox.TextChanged += ProjectDescriptionTextBox_TextChanged;

        mainStatusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        mainStatusStrip.Location = new Point(0, 538);
        mainStatusStrip.Name = "mainStatusStrip";
        mainStatusStrip.Size = new Size(920, 22);
        mainStatusStrip.TabIndex = 2;
        mainStatusStrip.Text = "mainStatusStrip";

        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(39, 17);
        statusLabel.Text = "Ready.";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(920, 560);
        Controls.Add(mainLayoutPanel);
        Controls.Add(mainStatusStrip);
        Controls.Add(mainMenuStrip);
        MainMenuStrip = mainMenuStrip;
        MinimumSize = new Size(720, 420);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PDF Page Studio";

        mainMenuStrip.ResumeLayout(false);
        mainMenuStrip.PerformLayout();
        mainLayoutPanel.ResumeLayout(false);
        mainLayoutPanel.PerformLayout();
        mainStatusStrip.ResumeLayout(false);
        mainStatusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
