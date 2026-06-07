namespace PdfPageStudio;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileMenuItem;
    private ToolStripMenuItem openProjectMenuItem;
    private ToolStripMenuItem saveProjectMenuItem;
    private ToolStripMenuItem saveProjectAsMenuItem;
    private ToolStripSeparator saveMenuSeparator;
    private ToolStripMenuItem addPdfFileMenuItem;
    private ToolStripSeparator recentMenuSeparator;
    private ToolStripMenuItem openRecentProjectMenuItem;
    private Panel mainContentPanel;
    private TableLayoutPanel projectInfoPanel;
    private Label projectNameLabel;
    private TextBox projectNameTextBox;
    private Label projectDescriptionLabel;
    private TextBox projectDescriptionTextBox;
    private TableLayoutPanel pdfWorkspacePanel;
    private ToolStrip pdfNavigationToolStrip;
    private ToolStripButton previousPageButton;
    private ToolStripTextBox pageNumberTextBox;
    private ToolStripLabel pageCountLabel;
    private ToolStripButton nextPageButton;
    private ToolStripSeparator navigationSeparator;
    private ToolStripButton zoomOutButton;
    private ToolStripButton zoomInButton;
    private ToolStripButton fitWidthButton;
    private ToolStripButton fitPageButton;
    private PdfPageViewer pdfPageViewer;
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
        saveMenuSeparator = new ToolStripSeparator();
        addPdfFileMenuItem = new ToolStripMenuItem();
        recentMenuSeparator = new ToolStripSeparator();
        openRecentProjectMenuItem = new ToolStripMenuItem();
        mainContentPanel = new Panel();
        projectInfoPanel = new TableLayoutPanel();
        projectNameLabel = new Label();
        projectNameTextBox = new TextBox();
        projectDescriptionLabel = new Label();
        projectDescriptionTextBox = new TextBox();
        pdfWorkspacePanel = new TableLayoutPanel();
        pdfNavigationToolStrip = new ToolStrip();
        previousPageButton = new ToolStripButton();
        pageNumberTextBox = new ToolStripTextBox();
        pageCountLabel = new ToolStripLabel();
        nextPageButton = new ToolStripButton();
        navigationSeparator = new ToolStripSeparator();
        zoomOutButton = new ToolStripButton();
        zoomInButton = new ToolStripButton();
        fitWidthButton = new ToolStripButton();
        fitPageButton = new ToolStripButton();
        pdfPageViewer = new PdfPageViewer();
        mainStatusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        mainMenuStrip.SuspendLayout();
        mainContentPanel.SuspendLayout();
        projectInfoPanel.SuspendLayout();
        pdfWorkspacePanel.SuspendLayout();
        pdfNavigationToolStrip.SuspendLayout();
        mainStatusStrip.SuspendLayout();
        SuspendLayout();

        mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem });
        mainMenuStrip.Location = new Point(0, 0);
        mainMenuStrip.Name = "mainMenuStrip";
        mainMenuStrip.Size = new Size(1100, 24);
        mainMenuStrip.TabIndex = 0;

        fileMenuItem.DropDownItems.AddRange(new ToolStripItem[]
        {
            openProjectMenuItem,
            saveProjectMenuItem,
            saveProjectAsMenuItem,
            saveMenuSeparator,
            addPdfFileMenuItem,
            recentMenuSeparator,
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

        saveMenuSeparator.Name = "saveMenuSeparator";
        saveMenuSeparator.Size = new Size(208, 6);

        addPdfFileMenuItem.Name = "addPdfFileMenuItem";
        addPdfFileMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
        addPdfFileMenuItem.Size = new Size(211, 22);
        addPdfFileMenuItem.Text = "Add &PDF File...";
        addPdfFileMenuItem.Click += AddPdfFileMenuItem_Click;

        recentMenuSeparator.Name = "recentMenuSeparator";
        recentMenuSeparator.Size = new Size(208, 6);

        openRecentProjectMenuItem.Name = "openRecentProjectMenuItem";
        openRecentProjectMenuItem.Size = new Size(211, 22);
        openRecentProjectMenuItem.Text = "Open &Recent Project";

        mainContentPanel.Controls.Add(pdfWorkspacePanel);
        mainContentPanel.Controls.Add(projectInfoPanel);
        mainContentPanel.Dock = DockStyle.Fill;
        mainContentPanel.Location = new Point(0, 24);
        mainContentPanel.Name = "mainContentPanel";
        mainContentPanel.Size = new Size(1100, 676);
        mainContentPanel.TabIndex = 1;

        projectInfoPanel.ColumnCount = 2;
        projectInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        projectInfoPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        projectInfoPanel.Controls.Add(projectNameLabel, 0, 0);
        projectInfoPanel.Controls.Add(projectNameTextBox, 1, 0);
        projectInfoPanel.Controls.Add(projectDescriptionLabel, 0, 1);
        projectInfoPanel.Controls.Add(projectDescriptionTextBox, 1, 1);
        projectInfoPanel.Dock = DockStyle.Fill;
        projectInfoPanel.Name = "projectInfoPanel";
        projectInfoPanel.Padding = new Padding(16);
        projectInfoPanel.RowCount = 3;
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        projectInfoPanel.Size = new Size(1100, 676);
        projectInfoPanel.TabIndex = 0;

        projectNameLabel.AutoSize = true;
        projectNameLabel.Dock = DockStyle.Fill;
        projectNameLabel.Name = "projectNameLabel";
        projectNameLabel.TabIndex = 0;
        projectNameLabel.Text = "Name";
        projectNameLabel.TextAlign = ContentAlignment.MiddleLeft;

        projectNameTextBox.Dock = DockStyle.Fill;
        projectNameTextBox.Name = "projectNameTextBox";
        projectNameTextBox.TabIndex = 1;
        projectNameTextBox.TextChanged += ProjectNameTextBox_TextChanged;

        projectDescriptionLabel.AutoSize = true;
        projectDescriptionLabel.Dock = DockStyle.Fill;
        projectDescriptionLabel.Name = "projectDescriptionLabel";
        projectDescriptionLabel.TabIndex = 2;
        projectDescriptionLabel.Text = "Description";
        projectDescriptionLabel.TextAlign = ContentAlignment.TopLeft;

        projectDescriptionTextBox.AcceptsReturn = true;
        projectDescriptionTextBox.AcceptsTab = true;
        projectDescriptionTextBox.Dock = DockStyle.Fill;
        projectDescriptionTextBox.Multiline = true;
        projectDescriptionTextBox.Name = "projectDescriptionTextBox";
        projectDescriptionTextBox.ScrollBars = ScrollBars.Vertical;
        projectDescriptionTextBox.TabIndex = 3;
        projectDescriptionTextBox.TextChanged += ProjectDescriptionTextBox_TextChanged;

        pdfWorkspacePanel.ColumnCount = 1;
        pdfWorkspacePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pdfWorkspacePanel.Controls.Add(pdfNavigationToolStrip, 0, 0);
        pdfWorkspacePanel.Controls.Add(pdfPageViewer, 0, 1);
        pdfWorkspacePanel.Dock = DockStyle.Fill;
        pdfWorkspacePanel.Name = "pdfWorkspacePanel";
        pdfWorkspacePanel.RowCount = 2;
        pdfWorkspacePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
        pdfWorkspacePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pdfWorkspacePanel.Size = new Size(1100, 676);
        pdfWorkspacePanel.TabIndex = 1;
        pdfWorkspacePanel.Visible = false;

        pdfNavigationToolStrip.GripStyle = ToolStripGripStyle.Hidden;
        pdfNavigationToolStrip.Items.AddRange(new ToolStripItem[]
        {
            previousPageButton,
            pageNumberTextBox,
            pageCountLabel,
            nextPageButton,
            navigationSeparator,
            zoomOutButton,
            zoomInButton,
            fitWidthButton,
            fitPageButton,
        });
        pdfNavigationToolStrip.Location = new Point(0, 0);
        pdfNavigationToolStrip.Name = "pdfNavigationToolStrip";
        pdfNavigationToolStrip.Padding = new Padding(8, 3, 8, 3);
        pdfNavigationToolStrip.Size = new Size(1100, 31);
        pdfNavigationToolStrip.TabIndex = 0;

        previousPageButton.Enabled = false;
        previousPageButton.Name = "previousPageButton";
        previousPageButton.Size = new Size(64, 20);
        previousPageButton.Text = "Previous";
        previousPageButton.Click += PreviousPageButton_Click;

        pageNumberTextBox.Enabled = false;
        pageNumberTextBox.Name = "pageNumberTextBox";
        pageNumberTextBox.Size = new Size(48, 25);
        pageNumberTextBox.KeyDown += PageNumberTextBox_KeyDown;
        pageNumberTextBox.Leave += PageNumberTextBox_Leave;

        pageCountLabel.Name = "pageCountLabel";
        pageCountLabel.Size = new Size(29, 20);
        pageCountLabel.Text = "of 0";

        nextPageButton.Enabled = false;
        nextPageButton.Name = "nextPageButton";
        nextPageButton.Size = new Size(35, 20);
        nextPageButton.Text = "Next";
        nextPageButton.Click += NextPageButton_Click;

        navigationSeparator.Name = "navigationSeparator";
        navigationSeparator.Size = new Size(6, 25);

        zoomOutButton.Enabled = false;
        zoomOutButton.Name = "zoomOutButton";
        zoomOutButton.Size = new Size(23, 20);
        zoomOutButton.Text = "-";
        zoomOutButton.ToolTipText = "Zoom out";
        zoomOutButton.Click += ZoomOutButton_Click;

        zoomInButton.Enabled = false;
        zoomInButton.Name = "zoomInButton";
        zoomInButton.Size = new Size(23, 20);
        zoomInButton.Text = "+";
        zoomInButton.ToolTipText = "Zoom in";
        zoomInButton.Click += ZoomInButton_Click;

        fitWidthButton.Enabled = false;
        fitWidthButton.Name = "fitWidthButton";
        fitWidthButton.Size = new Size(60, 20);
        fitWidthButton.Text = "Fit Width";
        fitWidthButton.Click += FitWidthButton_Click;

        fitPageButton.Enabled = false;
        fitPageButton.Name = "fitPageButton";
        fitPageButton.Size = new Size(54, 20);
        fitPageButton.Text = "Fit Page";
        fitPageButton.Click += FitPageButton_Click;

        pdfPageViewer.Dock = DockStyle.Fill;
        pdfPageViewer.Name = "pdfPageViewer";
        pdfPageViewer.TabIndex = 1;
        pdfPageViewer.ZoomMode = PdfZoomMode.FitPage;

        mainStatusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        mainStatusStrip.Location = new Point(0, 700);
        mainStatusStrip.Name = "mainStatusStrip";
        mainStatusStrip.Size = new Size(1100, 22);
        mainStatusStrip.TabIndex = 2;

        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(39, 17);
        statusLabel.Text = "Ready.";

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 722);
        Controls.Add(mainContentPanel);
        Controls.Add(mainStatusStrip);
        Controls.Add(mainMenuStrip);
        MainMenuStrip = mainMenuStrip;
        MinimumSize = new Size(860, 560);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PDF Page Studio";

        mainMenuStrip.ResumeLayout(false);
        mainMenuStrip.PerformLayout();
        mainContentPanel.ResumeLayout(false);
        projectInfoPanel.ResumeLayout(false);
        projectInfoPanel.PerformLayout();
        pdfWorkspacePanel.ResumeLayout(false);
        pdfWorkspacePanel.PerformLayout();
        pdfNavigationToolStrip.ResumeLayout(false);
        pdfNavigationToolStrip.PerformLayout();
        mainStatusStrip.ResumeLayout(false);
        mainStatusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
