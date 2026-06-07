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
    private ToolStripSeparator pageSizeSeparator;
    private ToolStripLabel pageSizeLabel;
    private PdfPageViewer pdfPageViewer;
    private TableLayoutPanel actionsPanel;
    private Label actionsTitleLabel;
    private FlowLayoutPanel actionButtonsPanel;
    private Button addActionButton;
    private Button insertBeforeActionButton;
    private Button insertAfterActionButton;
    private Button deleteActionButton;
    private Button moveActionUpButton;
    private Button moveActionDownButton;
    private ListBox actionsListBox;
    private Label propertiesTitleLabel;
    private TableLayoutPanel actionPropertiesPanel;
    private Label actionNameLabel;
    private TextBox actionNameTextBox;
    private Label pageFilterTypeLabel;
    private ComboBox pageFilterTypeComboBox;
    private Label pageFilterRangeLabel;
    private TextBox pageFilterRangeTextBox;
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
        pageSizeSeparator = new ToolStripSeparator();
        pageSizeLabel = new ToolStripLabel();
        pdfPageViewer = new PdfPageViewer();
        actionsPanel = new TableLayoutPanel();
        actionsTitleLabel = new Label();
        actionButtonsPanel = new FlowLayoutPanel();
        addActionButton = new Button();
        insertBeforeActionButton = new Button();
        insertAfterActionButton = new Button();
        deleteActionButton = new Button();
        moveActionUpButton = new Button();
        moveActionDownButton = new Button();
        actionsListBox = new ListBox();
        propertiesTitleLabel = new Label();
        actionPropertiesPanel = new TableLayoutPanel();
        actionNameLabel = new Label();
        actionNameTextBox = new TextBox();
        pageFilterTypeLabel = new Label();
        pageFilterTypeComboBox = new ComboBox();
        pageFilterRangeLabel = new Label();
        pageFilterRangeTextBox = new TextBox();
        mainStatusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        mainMenuStrip.SuspendLayout();
        mainContentPanel.SuspendLayout();
        projectInfoPanel.SuspendLayout();
        pdfWorkspacePanel.SuspendLayout();
        pdfNavigationToolStrip.SuspendLayout();
        actionsPanel.SuspendLayout();
        actionButtonsPanel.SuspendLayout();
        actionPropertiesPanel.SuspendLayout();
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

        pdfWorkspacePanel.ColumnCount = 2;
        pdfWorkspacePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pdfWorkspacePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
        pdfWorkspacePanel.Controls.Add(pdfNavigationToolStrip, 0, 0);
        pdfWorkspacePanel.Controls.Add(pdfPageViewer, 0, 1);
        pdfWorkspacePanel.Controls.Add(actionsPanel, 1, 0);
        pdfWorkspacePanel.Dock = DockStyle.Fill;
        pdfWorkspacePanel.Name = "pdfWorkspacePanel";
        pdfWorkspacePanel.RowCount = 2;
        pdfWorkspacePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
        pdfWorkspacePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pdfWorkspacePanel.SetRowSpan(actionsPanel, 2);
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
            pageSizeSeparator,
            pageSizeLabel,
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

        pageSizeSeparator.Name = "pageSizeSeparator";
        pageSizeSeparator.Size = new Size(6, 25);

        pageSizeLabel.Name = "pageSizeLabel";
        pageSizeLabel.Size = new Size(74, 20);
        pageSizeLabel.Text = "Page: -- x -- in";

        pdfPageViewer.Dock = DockStyle.Fill;
        pdfPageViewer.Name = "pdfPageViewer";
        pdfPageViewer.TabIndex = 1;
        pdfPageViewer.ZoomMode = PdfZoomMode.FitPage;

        actionsPanel.ColumnCount = 1;
        actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionsPanel.Controls.Add(actionsTitleLabel, 0, 0);
        actionsPanel.Controls.Add(actionButtonsPanel, 0, 1);
        actionsPanel.Controls.Add(actionsListBox, 0, 2);
        actionsPanel.Controls.Add(propertiesTitleLabel, 0, 3);
        actionsPanel.Controls.Add(actionPropertiesPanel, 0, 4);
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.Name = "actionsPanel";
        actionsPanel.Padding = new Padding(12);
        actionsPanel.RowCount = 5;
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 74F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
        actionsPanel.TabIndex = 2;

        actionsTitleLabel.AutoSize = true;
        actionsTitleLabel.Dock = DockStyle.Fill;
        actionsTitleLabel.Font = new Font(actionsTitleLabel.Font, FontStyle.Bold);
        actionsTitleLabel.Name = "actionsTitleLabel";
        actionsTitleLabel.TabIndex = 0;
        actionsTitleLabel.Text = "Actions";
        actionsTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionButtonsPanel.Controls.Add(addActionButton);
        actionButtonsPanel.Controls.Add(insertBeforeActionButton);
        actionButtonsPanel.Controls.Add(insertAfterActionButton);
        actionButtonsPanel.Controls.Add(deleteActionButton);
        actionButtonsPanel.Controls.Add(moveActionUpButton);
        actionButtonsPanel.Controls.Add(moveActionDownButton);
        actionButtonsPanel.Dock = DockStyle.Fill;
        actionButtonsPanel.Name = "actionButtonsPanel";
        actionButtonsPanel.TabIndex = 1;

        addActionButton.Name = "addActionButton";
        addActionButton.Size = new Size(68, 28);
        addActionButton.TabIndex = 0;
        addActionButton.Text = "Add";
        addActionButton.UseVisualStyleBackColor = true;
        addActionButton.Click += AddActionButton_Click;

        insertBeforeActionButton.Name = "insertBeforeActionButton";
        insertBeforeActionButton.Size = new Size(102, 28);
        insertBeforeActionButton.TabIndex = 1;
        insertBeforeActionButton.Text = "Insert Before";
        insertBeforeActionButton.UseVisualStyleBackColor = true;
        insertBeforeActionButton.Click += InsertBeforeActionButton_Click;

        insertAfterActionButton.Name = "insertAfterActionButton";
        insertAfterActionButton.Size = new Size(94, 28);
        insertAfterActionButton.TabIndex = 2;
        insertAfterActionButton.Text = "Insert After";
        insertAfterActionButton.UseVisualStyleBackColor = true;
        insertAfterActionButton.Click += InsertAfterActionButton_Click;

        deleteActionButton.Name = "deleteActionButton";
        deleteActionButton.Size = new Size(68, 28);
        deleteActionButton.TabIndex = 3;
        deleteActionButton.Text = "Delete";
        deleteActionButton.UseVisualStyleBackColor = true;
        deleteActionButton.Click += DeleteActionButton_Click;

        moveActionUpButton.Name = "moveActionUpButton";
        moveActionUpButton.Size = new Size(68, 28);
        moveActionUpButton.TabIndex = 4;
        moveActionUpButton.Text = "Up";
        moveActionUpButton.UseVisualStyleBackColor = true;
        moveActionUpButton.Click += MoveActionUpButton_Click;

        moveActionDownButton.Name = "moveActionDownButton";
        moveActionDownButton.Size = new Size(68, 28);
        moveActionDownButton.TabIndex = 5;
        moveActionDownButton.Text = "Down";
        moveActionDownButton.UseVisualStyleBackColor = true;
        moveActionDownButton.Click += MoveActionDownButton_Click;

        actionsListBox.Dock = DockStyle.Fill;
        actionsListBox.FormattingEnabled = true;
        actionsListBox.IntegralHeight = false;
        actionsListBox.Name = "actionsListBox";
        actionsListBox.TabIndex = 2;
        actionsListBox.SelectedIndexChanged += ActionsListBox_SelectedIndexChanged;

        propertiesTitleLabel.AutoSize = true;
        propertiesTitleLabel.Dock = DockStyle.Fill;
        propertiesTitleLabel.Font = new Font(propertiesTitleLabel.Font, FontStyle.Bold);
        propertiesTitleLabel.Name = "propertiesTitleLabel";
        propertiesTitleLabel.TabIndex = 3;
        propertiesTitleLabel.Text = "Properties";
        propertiesTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionPropertiesPanel.ColumnCount = 2;
        actionPropertiesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        actionPropertiesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionPropertiesPanel.Controls.Add(actionNameLabel, 0, 0);
        actionPropertiesPanel.Controls.Add(actionNameTextBox, 1, 0);
        actionPropertiesPanel.Controls.Add(pageFilterTypeLabel, 0, 1);
        actionPropertiesPanel.Controls.Add(pageFilterTypeComboBox, 1, 1);
        actionPropertiesPanel.Controls.Add(pageFilterRangeLabel, 0, 2);
        actionPropertiesPanel.Controls.Add(pageFilterRangeTextBox, 1, 2);
        actionPropertiesPanel.Dock = DockStyle.Top;
        actionPropertiesPanel.Name = "actionPropertiesPanel";
        actionPropertiesPanel.RowCount = 4;
        actionPropertiesPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        actionPropertiesPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        actionPropertiesPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        actionPropertiesPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        actionPropertiesPanel.TabIndex = 4;

        actionNameLabel.AutoSize = true;
        actionNameLabel.Dock = DockStyle.Fill;
        actionNameLabel.Name = "actionNameLabel";
        actionNameLabel.TabIndex = 0;
        actionNameLabel.Text = "Name";
        actionNameLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionNameTextBox.Dock = DockStyle.Fill;
        actionNameTextBox.Name = "actionNameTextBox";
        actionNameTextBox.TabIndex = 1;
        actionNameTextBox.TextChanged += ActionNameTextBox_TextChanged;

        pageFilterTypeLabel.AutoSize = true;
        pageFilterTypeLabel.Dock = DockStyle.Fill;
        pageFilterTypeLabel.Name = "pageFilterTypeLabel";
        pageFilterTypeLabel.TabIndex = 2;
        pageFilterTypeLabel.Text = "Page Type";
        pageFilterTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

        pageFilterTypeComboBox.Dock = DockStyle.Fill;
        pageFilterTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        pageFilterTypeComboBox.FormattingEnabled = true;
        pageFilterTypeComboBox.Items.AddRange(new object[] { "Any", "Odd", "Even" });
        pageFilterTypeComboBox.Name = "pageFilterTypeComboBox";
        pageFilterTypeComboBox.TabIndex = 3;
        pageFilterTypeComboBox.SelectedIndexChanged += PageFilterTypeComboBox_SelectedIndexChanged;

        pageFilterRangeLabel.AutoSize = true;
        pageFilterRangeLabel.Dock = DockStyle.Fill;
        pageFilterRangeLabel.Name = "pageFilterRangeLabel";
        pageFilterRangeLabel.TabIndex = 4;
        pageFilterRangeLabel.Text = "Range";
        pageFilterRangeLabel.TextAlign = ContentAlignment.MiddleLeft;

        pageFilterRangeTextBox.Dock = DockStyle.Fill;
        pageFilterRangeTextBox.Name = "pageFilterRangeTextBox";
        pageFilterRangeTextBox.PlaceholderText = "All pages or 1-3, 5, 7-";
        pageFilterRangeTextBox.TabIndex = 5;
        pageFilterRangeTextBox.TextChanged += PageFilterRangeTextBox_TextChanged;

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
        actionsPanel.ResumeLayout(false);
        actionsPanel.PerformLayout();
        actionButtonsPanel.ResumeLayout(false);
        actionPropertiesPanel.ResumeLayout(false);
        actionPropertiesPanel.PerformLayout();
        mainStatusStrip.ResumeLayout(false);
        mainStatusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
