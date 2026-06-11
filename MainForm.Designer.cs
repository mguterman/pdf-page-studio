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
    private ToolStripMenuItem convertMenuItem;
    private ToolStripMenuItem convertCurrentPdfFileMenuItem;
    private ToolStripMenuItem convertMultiplePdfFilesMenuItem;
    private ToolStripSeparator recentMenuSeparator;
    private ToolStripMenuItem openRecentProjectMenuItem;
    private ToolStripMenuItem languageMenuItem;
    private ToolStripMenuItem englishLanguageMenuItem;
    private ToolStripMenuItem russianLanguageMenuItem;
    private ToolStripMenuItem hebrewLanguageMenuItem;
    private ToolStripMenuItem settingsMenuItem;
    private ToolStripMenuItem inchUnitMenuItem;
    private ToolStripMenuItem centimeterUnitMenuItem;
    private Panel mainContentPanel;
    private TableLayoutPanel projectInfoPanel;
    private Label projectNameLabel;
    private TextBox projectNameTextBox;
    private Label projectUnitTypeLabel;
    private ComboBox projectUnitTypeComboBox;
    private Label projectDescriptionLabel;
    private TextBox projectDescriptionTextBox;
    private TableLayoutPanel pdfWorkspacePanel;
    private ToolStrip pdfNavigationToolStrip;
    private ToolStripButton firstPageButton;
    private ToolStripButton previousPageButton;
    private ToolStripTextBox pageNumberTextBox;
    private ToolStripLabel pageCountLabel;
    private ToolStripButton nextPageButton;
    private ToolStripButton lastPageButton;
    private ToolStripSeparator navigationSeparator;
    private ToolStripButton zoomOutButton;
    private ToolStripButton zoomInButton;
    private ToolStripButton fitWidthButton;
    private ToolStripButton fitPageButton;
    private ToolStripSeparator pageSizeSeparator;
    private ToolStripLabel pageSizeLabel;
    private ToolStripSeparator outputFolderSeparator;
    private ToolStripLabel outputFolderLabel;
    private ToolStripTextBox outputFolderTextBox;
    private ToolStripButton browseOutputFolderButton;
    private ToolStripButton openOutputFolderButton;
    private ToolStripDropDownButton convertDropDownButton;
    private ToolStripMenuItem convertCurrentPdfMenuItem;
    private ToolStripMenuItem convertMultiplePdfMenuItem;
    private PdfPageViewer pdfPageViewer;
    private TableLayoutPanel actionsPanel;
    private Label actionsTitleLabel;
    private FlowLayoutPanel actionButtonsPanel;
    private Button addActionButton;
    private Button addLineButton;
    private Button addFrameButton;
    private DataGridView actionsGridView;
    private DataGridViewCheckBoxColumn actionEnabledColumn;
    private DataGridViewTextBoxColumn actionIndexColumn;
    private DataGridViewTextBoxColumn actionNameColumn;
    private DataGridViewTextBoxColumn actionTypeColumn;
    private DataGridViewTextBoxColumn actionPagesColumn;
    private DataGridViewButtonColumn actionMoveUpColumn;
    private DataGridViewButtonColumn actionMoveDownColumn;
    private DataGridViewButtonColumn actionDuplicateColumn;
    private DataGridViewButtonColumn actionDeleteColumn;
    private ToolTip actionToolTip;
    private Label propertiesTitleLabel;
    private Panel actionPropertiesScrollPanel;
    private TableLayoutPanel actionPropertiesPanel;
    private Label actionTypeLabel;
    private ComboBox actionTypeComboBox;
    private Label actionNameLabel;
    private TextBox actionNameTextBox;
    private Label pageFilterTypeLabel;
    private ComboBox pageFilterTypeComboBox;
    private Label pageFilterRangeLabel;
    private TextBox pageFilterRangeTextBox;
    private Label leftLabel;
    private NumericUpDown leftNumericBox;
    private Label topLabel;
    private NumericUpDown topNumericBox;
    private Label rightLabel;
    private NumericUpDown rightNumericBox;
    private Label bottomLabel;
    private NumericUpDown bottomNumericBox;
    private Label targetedWidthLabel;
    private FlowLayoutPanel targetedWidthPanel;
    private NumericUpDown targetedWidthNumericBox;
    private CheckBox targetedWidthEnabledCheckBox;
    private Label targetedHeightLabel;
    private FlowLayoutPanel targetedHeightPanel;
    private NumericUpDown targetedHeightNumericBox;
    private CheckBox targetedHeightEnabledCheckBox;
    private Label proportionalLabel;
    private CheckBox proportionalCheckBox;
    private Label anchorLabel;
    private TableLayoutPanel anchorPanel;
    private RadioButton anchorTopLeftRadioButton;
    private RadioButton anchorTopCenterRadioButton;
    private RadioButton anchorTopRightRadioButton;
    private RadioButton anchorMiddleLeftRadioButton;
    private RadioButton anchorMiddleCenterRadioButton;
    private RadioButton anchorMiddleRightRadioButton;
    private RadioButton anchorBottomLeftRadioButton;
    private RadioButton anchorBottomCenterRadioButton;
    private RadioButton anchorBottomRightRadioButton;
    private Label rulerColorLabel;
    private FlowLayoutPanel rulerColorPanel;
    private TextBox rulerColorTextBox;
    private Button rulerColorButton;
    private Label rulerStyleLabel;
    private ComboBox rulerStyleComboBox;
    private Label rulerValueModeLabel;
    private ComboBox rulerValueModeComboBox;
    private Label rulerOrientationLabel;
    private ComboBox rulerOrientationComboBox;
    private Label rulerPositionLabel;
    private NumericUpDown rulerPositionNumericBox;
    private Label rulerEndLabel;
    private NumericUpDown rulerEndNumericBox;
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
        convertMenuItem = new ToolStripMenuItem();
        convertCurrentPdfFileMenuItem = new ToolStripMenuItem();
        convertMultiplePdfFilesMenuItem = new ToolStripMenuItem();
        recentMenuSeparator = new ToolStripSeparator();
        openRecentProjectMenuItem = new ToolStripMenuItem();
        languageMenuItem = new ToolStripMenuItem();
        englishLanguageMenuItem = new ToolStripMenuItem();
        russianLanguageMenuItem = new ToolStripMenuItem();
        hebrewLanguageMenuItem = new ToolStripMenuItem();
        settingsMenuItem = new ToolStripMenuItem();
        inchUnitMenuItem = new ToolStripMenuItem();
        centimeterUnitMenuItem = new ToolStripMenuItem();
        mainContentPanel = new Panel();
        projectInfoPanel = new TableLayoutPanel();
        projectNameLabel = new Label();
        projectNameTextBox = new TextBox();
        projectUnitTypeLabel = new Label();
        projectUnitTypeComboBox = new ComboBox();
        projectDescriptionLabel = new Label();
        projectDescriptionTextBox = new TextBox();
        pdfWorkspacePanel = new TableLayoutPanel();
        pdfNavigationToolStrip = new ToolStrip();
        firstPageButton = new ToolStripButton();
        previousPageButton = new ToolStripButton();
        pageNumberTextBox = new ToolStripTextBox();
        pageCountLabel = new ToolStripLabel();
        nextPageButton = new ToolStripButton();
        lastPageButton = new ToolStripButton();
        navigationSeparator = new ToolStripSeparator();
        zoomOutButton = new ToolStripButton();
        zoomInButton = new ToolStripButton();
        fitWidthButton = new ToolStripButton();
        fitPageButton = new ToolStripButton();
        pageSizeSeparator = new ToolStripSeparator();
        pageSizeLabel = new ToolStripLabel();
        outputFolderSeparator = new ToolStripSeparator();
        outputFolderLabel = new ToolStripLabel();
        outputFolderTextBox = new ToolStripTextBox();
        browseOutputFolderButton = new ToolStripButton();
        openOutputFolderButton = new ToolStripButton();
        convertDropDownButton = new ToolStripDropDownButton();
        convertCurrentPdfMenuItem = new ToolStripMenuItem();
        convertMultiplePdfMenuItem = new ToolStripMenuItem();
        pdfPageViewer = new PdfPageViewer();
        actionsPanel = new TableLayoutPanel();
        actionsTitleLabel = new Label();
        actionButtonsPanel = new FlowLayoutPanel();
        addActionButton = new Button();
        addLineButton = new Button();
        addFrameButton = new Button();
        actionsGridView = new DataGridView();
        actionEnabledColumn = new DataGridViewCheckBoxColumn();
        actionIndexColumn = new DataGridViewTextBoxColumn();
        actionNameColumn = new DataGridViewTextBoxColumn();
        actionTypeColumn = new DataGridViewTextBoxColumn();
        actionPagesColumn = new DataGridViewTextBoxColumn();
        actionMoveUpColumn = new DataGridViewButtonColumn();
        actionMoveDownColumn = new DataGridViewButtonColumn();
        actionDuplicateColumn = new DataGridViewButtonColumn();
        actionDeleteColumn = new DataGridViewButtonColumn();
        actionToolTip = new ToolTip(components);
        propertiesTitleLabel = new Label();
        actionPropertiesScrollPanel = new Panel();
        actionPropertiesPanel = new TableLayoutPanel();
        actionTypeLabel = new Label();
        actionTypeComboBox = new ComboBox();
        actionNameLabel = new Label();
        actionNameTextBox = new TextBox();
        pageFilterTypeLabel = new Label();
        pageFilterTypeComboBox = new ComboBox();
        pageFilterRangeLabel = new Label();
        pageFilterRangeTextBox = new TextBox();
        leftLabel = new Label();
        leftNumericBox = new NumericUpDown();
        topLabel = new Label();
        topNumericBox = new NumericUpDown();
        rightLabel = new Label();
        rightNumericBox = new NumericUpDown();
        bottomLabel = new Label();
        bottomNumericBox = new NumericUpDown();
        targetedWidthLabel = new Label();
        targetedWidthPanel = new FlowLayoutPanel();
        targetedWidthNumericBox = new NumericUpDown();
        targetedWidthEnabledCheckBox = new CheckBox();
        targetedHeightLabel = new Label();
        targetedHeightPanel = new FlowLayoutPanel();
        targetedHeightNumericBox = new NumericUpDown();
        targetedHeightEnabledCheckBox = new CheckBox();
        proportionalLabel = new Label();
        proportionalCheckBox = new CheckBox();
        anchorLabel = new Label();
        anchorPanel = new TableLayoutPanel();
        anchorTopLeftRadioButton = new RadioButton();
        anchorTopCenterRadioButton = new RadioButton();
        anchorTopRightRadioButton = new RadioButton();
        anchorMiddleLeftRadioButton = new RadioButton();
        anchorMiddleCenterRadioButton = new RadioButton();
        anchorMiddleRightRadioButton = new RadioButton();
        anchorBottomLeftRadioButton = new RadioButton();
        anchorBottomCenterRadioButton = new RadioButton();
        anchorBottomRightRadioButton = new RadioButton();
        rulerColorLabel = new Label();
        rulerColorPanel = new FlowLayoutPanel();
        rulerColorTextBox = new TextBox();
        rulerColorButton = new Button();
        rulerStyleLabel = new Label();
        rulerStyleComboBox = new ComboBox();
        rulerValueModeLabel = new Label();
        rulerValueModeComboBox = new ComboBox();
        rulerOrientationLabel = new Label();
        rulerOrientationComboBox = new ComboBox();
        rulerPositionLabel = new Label();
        rulerPositionNumericBox = new NumericUpDown();
        rulerEndLabel = new Label();
        rulerEndNumericBox = new NumericUpDown();
        mainStatusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        mainMenuStrip.SuspendLayout();
        mainContentPanel.SuspendLayout();
        projectInfoPanel.SuspendLayout();
        pdfWorkspacePanel.SuspendLayout();
        pdfNavigationToolStrip.SuspendLayout();
        actionsPanel.SuspendLayout();
        actionButtonsPanel.SuspendLayout();
        actionPropertiesScrollPanel.SuspendLayout();
        actionPropertiesPanel.SuspendLayout();
        anchorPanel.SuspendLayout();
        mainStatusStrip.SuspendLayout();
        SuspendLayout();

        mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem, settingsMenuItem, languageMenuItem });
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
            convertMenuItem,
            recentMenuSeparator,
            openRecentProjectMenuItem,
        });
        fileMenuItem.Name = "fileMenuItem";
        fileMenuItem.Size = new Size(37, 20);
        fileMenuItem.Text = TranslationService.T("menu.file");

        openProjectMenuItem.Name = "openProjectMenuItem";
        openProjectMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        openProjectMenuItem.Size = new Size(211, 22);
        openProjectMenuItem.Text = TranslationService.T("menu.openProject");
        openProjectMenuItem.Click += OpenProjectMenuItem_Click;

        saveProjectMenuItem.Name = "saveProjectMenuItem";
        saveProjectMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        saveProjectMenuItem.Size = new Size(211, 22);
        saveProjectMenuItem.Text = TranslationService.T("menu.save");
        saveProjectMenuItem.Click += SaveProjectMenuItem_Click;

        saveProjectAsMenuItem.Name = "saveProjectAsMenuItem";
        saveProjectAsMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        saveProjectAsMenuItem.Size = new Size(211, 22);
        saveProjectAsMenuItem.Text = TranslationService.T("menu.saveAs");
        saveProjectAsMenuItem.Click += SaveProjectAsMenuItem_Click;

        saveMenuSeparator.Name = "saveMenuSeparator";
        saveMenuSeparator.Size = new Size(208, 6);

        addPdfFileMenuItem.Name = "addPdfFileMenuItem";
        addPdfFileMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
        addPdfFileMenuItem.Size = new Size(211, 22);
        addPdfFileMenuItem.Text = TranslationService.T("menu.addPdf");
        addPdfFileMenuItem.Click += AddPdfFileMenuItem_Click;

        convertMenuItem.DropDownItems.AddRange(new ToolStripItem[] { convertCurrentPdfFileMenuItem, convertMultiplePdfFilesMenuItem });
        convertMenuItem.Name = "convertMenuItem";
        convertMenuItem.Size = new Size(211, 22);
        convertMenuItem.Text = TranslationService.T("convert.button");

        convertCurrentPdfFileMenuItem.Name = "convertCurrentPdfFileMenuItem";
        convertCurrentPdfFileMenuItem.Size = new Size(210, 22);
        convertCurrentPdfFileMenuItem.Text = TranslationService.T("convert.menu.current");
        convertCurrentPdfFileMenuItem.Click += ConvertCurrentPdfMenuItem_Click;

        convertMultiplePdfFilesMenuItem.Name = "convertMultiplePdfFilesMenuItem";
        convertMultiplePdfFilesMenuItem.Size = new Size(210, 22);
        convertMultiplePdfFilesMenuItem.Text = TranslationService.T("convert.menu.multiple");
        convertMultiplePdfFilesMenuItem.Click += ConvertMultiplePdfMenuItem_Click;

        recentMenuSeparator.Name = "recentMenuSeparator";
        recentMenuSeparator.Size = new Size(208, 6);

        openRecentProjectMenuItem.Name = "openRecentProjectMenuItem";
        openRecentProjectMenuItem.Size = new Size(211, 22);
        openRecentProjectMenuItem.Text = TranslationService.T("menu.openRecent");

        languageMenuItem.DropDownItems.AddRange(new ToolStripItem[] { englishLanguageMenuItem, russianLanguageMenuItem, hebrewLanguageMenuItem });
        languageMenuItem.Name = "languageMenuItem";
        languageMenuItem.Size = new Size(71, 20);
        languageMenuItem.Text = TranslationService.T("menu.language");

        englishLanguageMenuItem.Name = "englishLanguageMenuItem";
        englishLanguageMenuItem.Size = new Size(180, 22);
        englishLanguageMenuItem.Text = TranslationService.T("language.english");
        englishLanguageMenuItem.Click += EnglishLanguageMenuItem_Click;

        russianLanguageMenuItem.Name = "russianLanguageMenuItem";
        russianLanguageMenuItem.Size = new Size(180, 22);
        russianLanguageMenuItem.Text = TranslationService.T("language.russian");
        russianLanguageMenuItem.Click += RussianLanguageMenuItem_Click;

        hebrewLanguageMenuItem.Name = "hebrewLanguageMenuItem";
        hebrewLanguageMenuItem.Size = new Size(180, 22);
        hebrewLanguageMenuItem.Text = TranslationService.T("language.hebrew");
        hebrewLanguageMenuItem.Click += HebrewLanguageMenuItem_Click;

        settingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { inchUnitMenuItem, centimeterUnitMenuItem });
        settingsMenuItem.Name = "settingsMenuItem";
        settingsMenuItem.Size = new Size(61, 20);
        settingsMenuItem.Text = TranslationService.T("menu.settings");

        inchUnitMenuItem.Name = "inchUnitMenuItem";
        inchUnitMenuItem.Size = new Size(180, 22);
        inchUnitMenuItem.Text = TranslationService.T("enum.unit.Inch");
        inchUnitMenuItem.Click += InchUnitMenuItem_Click;

        centimeterUnitMenuItem.Name = "centimeterUnitMenuItem";
        centimeterUnitMenuItem.Size = new Size(180, 22);
        centimeterUnitMenuItem.Text = TranslationService.T("enum.unit.Cm");
        centimeterUnitMenuItem.Click += CentimeterUnitMenuItem_Click;

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
        projectInfoPanel.Controls.Add(projectUnitTypeLabel, 0, 1);
        projectInfoPanel.Controls.Add(projectUnitTypeComboBox, 1, 1);
        projectInfoPanel.Controls.Add(projectDescriptionLabel, 0, 2);
        projectInfoPanel.Controls.Add(projectDescriptionTextBox, 1, 2);
        projectInfoPanel.Dock = DockStyle.Fill;
        projectInfoPanel.Name = "projectInfoPanel";
        projectInfoPanel.Padding = new Padding(16);
        projectInfoPanel.RowCount = 4;
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
        projectInfoPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        projectInfoPanel.Size = new Size(1100, 676);
        projectInfoPanel.TabIndex = 0;

        projectNameLabel.AutoSize = true;
        projectNameLabel.Dock = DockStyle.Fill;
        projectNameLabel.Name = "projectNameLabel";
        projectNameLabel.TabIndex = 0;
        projectNameLabel.Text = TranslationService.T("project.name");
        projectNameLabel.TextAlign = ContentAlignment.MiddleLeft;

        projectNameTextBox.Dock = DockStyle.Fill;
        projectNameTextBox.Name = "projectNameTextBox";
        projectNameTextBox.TabIndex = 1;
        projectNameTextBox.TextChanged += ProjectNameTextBox_TextChanged;

        projectUnitTypeLabel.AutoSize = true;
        projectUnitTypeLabel.Dock = DockStyle.Fill;
        projectUnitTypeLabel.Name = "projectUnitTypeLabel";
        projectUnitTypeLabel.TabIndex = 2;
        projectUnitTypeLabel.Text = TranslationService.T("project.unitType");
        projectUnitTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

        projectUnitTypeComboBox.Dock = DockStyle.Left;
        projectUnitTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        projectUnitTypeComboBox.Name = "projectUnitTypeComboBox";
        projectUnitTypeComboBox.Size = new Size(120, 23);
        projectUnitTypeComboBox.TabIndex = 3;
        projectUnitTypeComboBox.SelectedIndexChanged += UnitTypeComboBox_SelectedIndexChanged;

        projectDescriptionLabel.AutoSize = true;
        projectDescriptionLabel.Dock = DockStyle.Fill;
        projectDescriptionLabel.Name = "projectDescriptionLabel";
        projectDescriptionLabel.TabIndex = 4;
        projectDescriptionLabel.Text = TranslationService.T("project.description");
        projectDescriptionLabel.TextAlign = ContentAlignment.TopLeft;

        projectDescriptionTextBox.AcceptsReturn = true;
        projectDescriptionTextBox.AcceptsTab = true;
        projectDescriptionTextBox.Dock = DockStyle.Fill;
        projectDescriptionTextBox.Multiline = true;
        projectDescriptionTextBox.Name = "projectDescriptionTextBox";
        projectDescriptionTextBox.ScrollBars = ScrollBars.Vertical;
        projectDescriptionTextBox.TabIndex = 5;
        projectDescriptionTextBox.TextChanged += ProjectDescriptionTextBox_TextChanged;

        pdfWorkspacePanel.ColumnCount = 2;
        pdfWorkspacePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pdfWorkspacePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 480F));
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
            firstPageButton,
            previousPageButton,
            pageNumberTextBox,
            pageCountLabel,
            nextPageButton,
            lastPageButton,
            navigationSeparator,
            zoomOutButton,
            zoomInButton,
            fitWidthButton,
            fitPageButton,
            pageSizeSeparator,
            pageSizeLabel,
            outputFolderSeparator,
            outputFolderLabel,
            outputFolderTextBox,
            browseOutputFolderButton,
            openOutputFolderButton,
            convertDropDownButton,
        });
        pdfNavigationToolStrip.Location = new Point(0, 0);
        pdfNavigationToolStrip.Name = "pdfNavigationToolStrip";
        pdfNavigationToolStrip.Padding = new Padding(8, 3, 8, 3);
        pdfNavigationToolStrip.Size = new Size(1100, 31);
        pdfNavigationToolStrip.TabIndex = 0;

        firstPageButton.Enabled = false;
        firstPageButton.Name = "firstPageButton";
        firstPageButton.Size = new Size(32, 20);
        firstPageButton.Text = TranslationService.T("nav.firstPage");
        firstPageButton.ToolTipText = TranslationService.T("nav.firstPage.tooltip");
        firstPageButton.Click += FirstPageButton_Click;

        previousPageButton.Enabled = false;
        previousPageButton.Name = "previousPageButton";
        previousPageButton.Size = new Size(26, 20);
        previousPageButton.Text = TranslationService.T("nav.previousPage");
        previousPageButton.ToolTipText = TranslationService.T("nav.previousPage.tooltip");
        previousPageButton.Click += PreviousPageButton_Click;

        pageNumberTextBox.Enabled = false;
        pageNumberTextBox.Name = "pageNumberTextBox";
        pageNumberTextBox.Size = new Size(48, 25);
        pageNumberTextBox.KeyDown += PageNumberTextBox_KeyDown;
        pageNumberTextBox.Leave += PageNumberTextBox_Leave;

        pageCountLabel.Name = "pageCountLabel";
        pageCountLabel.Size = new Size(29, 20);
        pageCountLabel.Text = TranslationService.T("nav.pageCount.empty");

        nextPageButton.Enabled = false;
        nextPageButton.Name = "nextPageButton";
        nextPageButton.Size = new Size(26, 20);
        nextPageButton.Text = TranslationService.T("nav.nextPage");
        nextPageButton.ToolTipText = TranslationService.T("nav.nextPage.tooltip");
        nextPageButton.Click += NextPageButton_Click;

        lastPageButton.Enabled = false;
        lastPageButton.Name = "lastPageButton";
        lastPageButton.Size = new Size(32, 20);
        lastPageButton.Text = TranslationService.T("nav.lastPage");
        lastPageButton.ToolTipText = TranslationService.T("nav.lastPage.tooltip");
        lastPageButton.Click += LastPageButton_Click;

        navigationSeparator.Name = "navigationSeparator";
        navigationSeparator.Size = new Size(6, 25);

        zoomOutButton.Enabled = false;
        zoomOutButton.Name = "zoomOutButton";
        zoomOutButton.Size = new Size(23, 20);
        zoomOutButton.Text = TranslationService.T("nav.zoomOut");
        zoomOutButton.ToolTipText = TranslationService.T("nav.zoomOut.tooltip");
        zoomOutButton.Click += ZoomOutButton_Click;

        zoomInButton.Enabled = false;
        zoomInButton.Name = "zoomInButton";
        zoomInButton.Size = new Size(23, 20);
        zoomInButton.Text = TranslationService.T("nav.zoomIn");
        zoomInButton.ToolTipText = TranslationService.T("nav.zoomIn.tooltip");
        zoomInButton.Click += ZoomInButton_Click;

        fitWidthButton.Enabled = false;
        fitWidthButton.Name = "fitWidthButton";
        fitWidthButton.Size = new Size(60, 20);
        fitWidthButton.Text = TranslationService.T("nav.fitWidth");
        fitWidthButton.Click += FitWidthButton_Click;

        fitPageButton.Enabled = false;
        fitPageButton.Name = "fitPageButton";
        fitPageButton.Size = new Size(54, 20);
        fitPageButton.Text = TranslationService.T("nav.fitPage");
        fitPageButton.Click += FitPageButton_Click;

        pageSizeSeparator.Name = "pageSizeSeparator";
        pageSizeSeparator.Size = new Size(6, 25);

        pageSizeLabel.Name = "pageSizeLabel";
        pageSizeLabel.Size = new Size(74, 20);
        pageSizeLabel.Text = TranslationService.T("page.size.empty");

        outputFolderSeparator.Name = "outputFolderSeparator";
        outputFolderSeparator.Size = new Size(6, 25);

        outputFolderLabel.Name = "outputFolderLabel";
        outputFolderLabel.Size = new Size(82, 20);
        outputFolderLabel.Text = TranslationService.T("output.folder");

        outputFolderTextBox.Name = "outputFolderTextBox";
        outputFolderTextBox.Size = new Size(220, 25);
        outputFolderTextBox.Leave += OutputFolderTextBox_Leave;
        outputFolderTextBox.KeyDown += OutputFolderTextBox_KeyDown;

        browseOutputFolderButton.Name = "browseOutputFolderButton";
        browseOutputFolderButton.Size = new Size(62, 20);
        browseOutputFolderButton.Text = TranslationService.T("common.browse");
        browseOutputFolderButton.Click += BrowseOutputFolderButton_Click;

        openOutputFolderButton.Name = "openOutputFolderButton";
        openOutputFolderButton.Size = new Size(45, 20);
        openOutputFolderButton.Text = TranslationService.T("common.open");
        openOutputFolderButton.Click += OpenOutputFolderButton_Click;

        convertDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { convertCurrentPdfMenuItem, convertMultiplePdfMenuItem });
        convertDropDownButton.Name = "convertDropDownButton";
        convertDropDownButton.Size = new Size(71, 20);
        convertDropDownButton.Text = TranslationService.T("convert.button");

        convertCurrentPdfMenuItem.Name = "convertCurrentPdfMenuItem";
        convertCurrentPdfMenuItem.Size = new Size(210, 22);
        convertCurrentPdfMenuItem.Text = TranslationService.T("convert.menu.current");
        convertCurrentPdfMenuItem.Click += ConvertCurrentPdfMenuItem_Click;

        convertMultiplePdfMenuItem.Name = "convertMultiplePdfMenuItem";
        convertMultiplePdfMenuItem.Size = new Size(210, 22);
        convertMultiplePdfMenuItem.Text = TranslationService.T("convert.menu.multiple");
        convertMultiplePdfMenuItem.Click += ConvertMultiplePdfMenuItem_Click;

        pdfPageViewer.Dock = DockStyle.Fill;
        pdfPageViewer.Name = "pdfPageViewer";
        pdfPageViewer.TabIndex = 1;
        pdfPageViewer.ZoomMode = PdfZoomMode.FitPage;

        actionsPanel.ColumnCount = 1;
        actionsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionsPanel.Controls.Add(actionsTitleLabel, 0, 0);
        actionsPanel.Controls.Add(actionButtonsPanel, 0, 1);
        actionsPanel.Controls.Add(actionsGridView, 0, 2);
        actionsPanel.Controls.Add(propertiesTitleLabel, 0, 3);
        actionsPanel.Controls.Add(actionPropertiesScrollPanel, 0, 4);
        actionsPanel.Dock = DockStyle.Fill;
        actionsPanel.Name = "actionsPanel";
        actionsPanel.Padding = new Padding(12);
        actionsPanel.RowCount = 5;
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        actionsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
        actionsPanel.TabIndex = 2;

        actionsTitleLabel.AutoSize = true;
        actionsTitleLabel.Dock = DockStyle.Fill;
        actionsTitleLabel.Font = new Font(actionsTitleLabel.Font, FontStyle.Bold);
        actionsTitleLabel.Name = "actionsTitleLabel";
        actionsTitleLabel.TabIndex = 0;
        actionsTitleLabel.Text = TranslationService.T("actions.title");
        actionsTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionButtonsPanel.Controls.Add(addActionButton);
        actionButtonsPanel.Controls.Add(addLineButton);
        actionButtonsPanel.Controls.Add(addFrameButton);
        actionButtonsPanel.Dock = DockStyle.Fill;
        actionButtonsPanel.Name = "actionButtonsPanel";
        actionButtonsPanel.TabIndex = 1;

        addActionButton.Name = "addActionButton";
        addActionButton.Size = new Size(86, 28);
        addActionButton.TabIndex = 0;
        addActionButton.Text = TranslationService.T("actions.add");
        addActionButton.UseVisualStyleBackColor = true;
        addActionButton.Click += AddActionButton_Click;

        addLineButton.Name = "addLineButton";
        addLineButton.Size = new Size(76, 28);
        addLineButton.TabIndex = 1;
        addLineButton.Text = TranslationService.T("actions.addLine");
        addLineButton.UseVisualStyleBackColor = true;

        addFrameButton.Name = "addFrameButton";
        addFrameButton.Size = new Size(82, 28);
        addFrameButton.TabIndex = 2;
        addFrameButton.Text = TranslationService.T("actions.addFrame");
        addFrameButton.UseVisualStyleBackColor = true;

        actionsGridView.AllowUserToAddRows = false;
        actionsGridView.AllowUserToDeleteRows = false;
        actionsGridView.AllowUserToResizeRows = false;
        actionsGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        actionsGridView.BackgroundColor = SystemColors.Window;
        actionsGridView.BorderStyle = BorderStyle.FixedSingle;
        actionsGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        actionsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        actionsGridView.ColumnHeadersVisible = true;
        actionsGridView.Columns.AddRange(new DataGridViewColumn[] { actionEnabledColumn, actionIndexColumn, actionNameColumn, actionTypeColumn, actionPagesColumn, actionMoveUpColumn, actionMoveDownColumn, actionDuplicateColumn, actionDeleteColumn });
        actionsGridView.DefaultCellStyle.BackColor = Color.White;
        actionsGridView.DefaultCellStyle.ForeColor = SystemColors.ControlText;
        actionsGridView.DefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
        actionsGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
        actionsGridView.DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
        actionsGridView.Dock = DockStyle.Fill;
        actionsGridView.GridColor = Color.FromArgb(230, 230, 230);
        actionsGridView.MultiSelect = false;
        actionsGridView.Name = "actionsGridView";
        actionsGridView.ReadOnly = false;
        actionsGridView.RowHeadersVisible = false;
        actionsGridView.RowTemplate.Height = 36;
        actionsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        actionsGridView.ShowCellToolTips = true;
        actionsGridView.TabIndex = 2;
        actionsGridView.CellClick += ActionsGridView_CellClick;
        actionsGridView.CellContentClick += ActionsGridView_CellContentClick;
        actionsGridView.CellValueChanged += ActionsGridView_CellValueChanged;
        actionsGridView.CurrentCellDirtyStateChanged += ActionsGridView_CurrentCellDirtyStateChanged;
        actionsGridView.SelectionChanged += ActionsGridView_SelectionChanged;

        actionEnabledColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionEnabledColumn.HeaderText = "";
        actionEnabledColumn.Name = "actionEnabledColumn";
        actionEnabledColumn.Resizable = DataGridViewTriState.False;
        actionEnabledColumn.Width = 36;

        actionIndexColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionIndexColumn.HeaderText = "#";
        actionIndexColumn.Name = "actionIndexColumn";
        actionIndexColumn.ReadOnly = true;
        actionIndexColumn.Resizable = DataGridViewTriState.False;
        actionIndexColumn.Width = 34;

        actionNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        actionNameColumn.HeaderText = "Name";
        actionNameColumn.Name = "actionNameColumn";
        actionNameColumn.ReadOnly = true;
        actionNameColumn.MinimumWidth = 120;

        actionTypeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionTypeColumn.HeaderText = "Type";
        actionTypeColumn.Name = "actionTypeColumn";
        actionTypeColumn.ReadOnly = true;
        actionTypeColumn.Width = 90;

        actionPagesColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionPagesColumn.HeaderText = "Pages";
        actionPagesColumn.Name = "actionPagesColumn";
        actionPagesColumn.ReadOnly = true;
        actionPagesColumn.Width = 82;

        actionMoveUpColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionMoveUpColumn.HeaderText = "";
        actionMoveUpColumn.Name = "actionMoveUpColumn";
        actionMoveUpColumn.ReadOnly = true;
        actionMoveUpColumn.Text = "^";
        actionMoveUpColumn.UseColumnTextForButtonValue = false;
        actionMoveUpColumn.Width = 34;

        actionMoveDownColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionMoveDownColumn.HeaderText = "";
        actionMoveDownColumn.Name = "actionMoveDownColumn";
        actionMoveDownColumn.ReadOnly = true;
        actionMoveDownColumn.Text = "v";
        actionMoveDownColumn.UseColumnTextForButtonValue = false;
        actionMoveDownColumn.Width = 34;

        actionDuplicateColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionDuplicateColumn.HeaderText = "";
        actionDuplicateColumn.Name = "actionDuplicateColumn";
        actionDuplicateColumn.ReadOnly = true;
        actionDuplicateColumn.Text = "+";
        actionDuplicateColumn.UseColumnTextForButtonValue = true;
        actionDuplicateColumn.Width = 34;

        actionDeleteColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        actionDeleteColumn.HeaderText = "";
        actionDeleteColumn.Name = "actionDeleteColumn";
        actionDeleteColumn.ReadOnly = true;
        actionDeleteColumn.Text = "X";
        actionDeleteColumn.UseColumnTextForButtonValue = true;
        actionDeleteColumn.Width = 34;

        propertiesTitleLabel.AutoSize = true;
        propertiesTitleLabel.Dock = DockStyle.Fill;
        propertiesTitleLabel.Font = new Font(propertiesTitleLabel.Font, FontStyle.Bold);
        propertiesTitleLabel.Name = "propertiesTitleLabel";
        propertiesTitleLabel.TabIndex = 3;
        propertiesTitleLabel.Text = TranslationService.T("properties.title");
        propertiesTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionPropertiesScrollPanel.AutoScroll = true;
        actionPropertiesScrollPanel.Controls.Add(actionPropertiesPanel);
        actionPropertiesScrollPanel.Dock = DockStyle.Fill;
        actionPropertiesScrollPanel.Name = "actionPropertiesScrollPanel";
        actionPropertiesScrollPanel.TabIndex = 4;

        actionPropertiesPanel.AutoSize = true;
        actionPropertiesPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        actionPropertiesPanel.ColumnCount = 2;
        actionPropertiesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
        actionPropertiesPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        actionPropertiesPanel.Controls.Add(actionTypeLabel, 0, 0);
        actionPropertiesPanel.Controls.Add(actionTypeComboBox, 1, 0);
        actionPropertiesPanel.Controls.Add(actionNameLabel, 0, 1);
        actionPropertiesPanel.Controls.Add(actionNameTextBox, 1, 1);
        actionPropertiesPanel.Controls.Add(pageFilterTypeLabel, 0, 2);
        actionPropertiesPanel.Controls.Add(pageFilterTypeComboBox, 1, 2);
        actionPropertiesPanel.Controls.Add(pageFilterRangeLabel, 0, 3);
        actionPropertiesPanel.Controls.Add(pageFilterRangeTextBox, 1, 3);
        actionPropertiesPanel.Controls.Add(leftLabel, 0, 4);
        actionPropertiesPanel.Controls.Add(leftNumericBox, 1, 4);
        actionPropertiesPanel.Controls.Add(topLabel, 0, 5);
        actionPropertiesPanel.Controls.Add(topNumericBox, 1, 5);
        actionPropertiesPanel.Controls.Add(rightLabel, 0, 6);
        actionPropertiesPanel.Controls.Add(rightNumericBox, 1, 6);
        actionPropertiesPanel.Controls.Add(bottomLabel, 0, 7);
        actionPropertiesPanel.Controls.Add(bottomNumericBox, 1, 7);
        actionPropertiesPanel.Controls.Add(targetedWidthLabel, 0, 8);
        actionPropertiesPanel.Controls.Add(targetedWidthPanel, 1, 8);
        actionPropertiesPanel.Controls.Add(targetedHeightLabel, 0, 9);
        actionPropertiesPanel.Controls.Add(targetedHeightPanel, 1, 9);
        actionPropertiesPanel.Controls.Add(proportionalLabel, 0, 10);
        actionPropertiesPanel.Controls.Add(proportionalCheckBox, 1, 10);
        actionPropertiesPanel.Controls.Add(anchorLabel, 0, 11);
        actionPropertiesPanel.Controls.Add(anchorPanel, 1, 11);
        actionPropertiesPanel.Controls.Add(rulerColorLabel, 0, 12);
        actionPropertiesPanel.Controls.Add(rulerColorPanel, 1, 12);
        actionPropertiesPanel.Controls.Add(rulerStyleLabel, 0, 13);
        actionPropertiesPanel.Controls.Add(rulerStyleComboBox, 1, 13);
        actionPropertiesPanel.Controls.Add(rulerValueModeLabel, 0, 14);
        actionPropertiesPanel.Controls.Add(rulerValueModeComboBox, 1, 14);
        actionPropertiesPanel.Controls.Add(rulerOrientationLabel, 0, 15);
        actionPropertiesPanel.Controls.Add(rulerOrientationComboBox, 1, 15);
        actionPropertiesPanel.Controls.Add(rulerPositionLabel, 0, 16);
        actionPropertiesPanel.Controls.Add(rulerPositionNumericBox, 1, 16);
        actionPropertiesPanel.Controls.Add(rulerEndLabel, 0, 17);
        actionPropertiesPanel.Controls.Add(rulerEndNumericBox, 1, 17);
        actionPropertiesPanel.Dock = DockStyle.Top;
        actionPropertiesPanel.Name = "actionPropertiesPanel";
        actionPropertiesPanel.RowCount = 18;
        for (var rowIndex = 0; rowIndex < 18; rowIndex++)
        {
            actionPropertiesPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowIndex == 11 ? 78F : 34F));
        }
        actionPropertiesPanel.TabIndex = 4;

        actionTypeLabel.AutoSize = true;
        actionTypeLabel.Dock = DockStyle.Fill;
        actionTypeLabel.Name = "actionTypeLabel";
        actionTypeLabel.Text = TranslationService.T("field.type");
        actionTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionTypeComboBox.Dock = DockStyle.Fill;
        actionTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        actionTypeComboBox.Name = "actionTypeComboBox";
        actionTypeComboBox.SelectedIndexChanged += ActionTypeComboBox_SelectedIndexChanged;

        actionNameLabel.AutoSize = true;
        actionNameLabel.Dock = DockStyle.Fill;
        actionNameLabel.Name = "actionNameLabel";
        actionNameLabel.TabIndex = 0;
        actionNameLabel.Text = TranslationService.T("field.name");
        actionNameLabel.TextAlign = ContentAlignment.MiddleLeft;

        actionNameTextBox.Dock = DockStyle.Fill;
        actionNameTextBox.Name = "actionNameTextBox";
        actionNameTextBox.TabIndex = 1;
        actionNameTextBox.TextChanged += ActionNameTextBox_TextChanged;

        pageFilterTypeLabel.AutoSize = true;
        pageFilterTypeLabel.Dock = DockStyle.Fill;
        pageFilterTypeLabel.Name = "pageFilterTypeLabel";
        pageFilterTypeLabel.TabIndex = 2;
        pageFilterTypeLabel.Text = TranslationService.T("field.pageType");
        pageFilterTypeLabel.TextAlign = ContentAlignment.MiddleLeft;

        pageFilterTypeComboBox.Dock = DockStyle.Fill;
        pageFilterTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        pageFilterTypeComboBox.FormattingEnabled = true;
        pageFilterTypeComboBox.Name = "pageFilterTypeComboBox";
        pageFilterTypeComboBox.TabIndex = 3;
        pageFilterTypeComboBox.SelectedIndexChanged += PageFilterTypeComboBox_SelectedIndexChanged;

        pageFilterRangeLabel.AutoSize = true;
        pageFilterRangeLabel.Dock = DockStyle.Fill;
        pageFilterRangeLabel.Name = "pageFilterRangeLabel";
        pageFilterRangeLabel.TabIndex = 4;
        pageFilterRangeLabel.Text = TranslationService.T("field.range");
        pageFilterRangeLabel.TextAlign = ContentAlignment.MiddleLeft;

        pageFilterRangeTextBox.Dock = DockStyle.Fill;
        pageFilterRangeTextBox.Name = "pageFilterRangeTextBox";
        pageFilterRangeTextBox.PlaceholderText = TranslationService.T("field.range.placeholder");
        pageFilterRangeTextBox.TabIndex = 5;
        pageFilterRangeTextBox.TextChanged += PageFilterRangeTextBox_TextChanged;

        ConfigureEditorLabel(leftLabel, TranslationService.T("field.left"));
        ConfigureEditorNumericBox(leftNumericBox, ActionNumberNumericBox_ValueChanged);
        ConfigureEditorLabel(topLabel, TranslationService.T("field.top"));
        ConfigureEditorNumericBox(topNumericBox, ActionNumberNumericBox_ValueChanged);
        ConfigureEditorLabel(rightLabel, TranslationService.T("field.right"));
        ConfigureEditorNumericBox(rightNumericBox, ActionNumberNumericBox_ValueChanged);
        ConfigureEditorLabel(bottomLabel, TranslationService.T("field.bottom"));
        ConfigureEditorNumericBox(bottomNumericBox, ActionNumberNumericBox_ValueChanged);
        ConfigureEditorLabel(targetedWidthLabel, TranslationService.T("field.width"));
        ConfigureTargetParameterPanel(targetedWidthPanel, targetedWidthNumericBox, targetedWidthEnabledCheckBox);
        ConfigureEditorNumericBox(targetedWidthNumericBox, ActionNumberNumericBox_ValueChanged);
        targetedWidthEnabledCheckBox.CheckedChanged += TargetedWidthEnabledCheckBox_CheckedChanged;
        ConfigureEditorLabel(targetedHeightLabel, TranslationService.T("field.height"));
        ConfigureTargetParameterPanel(targetedHeightPanel, targetedHeightNumericBox, targetedHeightEnabledCheckBox);
        ConfigureEditorNumericBox(targetedHeightNumericBox, ActionNumberNumericBox_ValueChanged);
        targetedHeightEnabledCheckBox.CheckedChanged += TargetedHeightEnabledCheckBox_CheckedChanged;

        ConfigureEditorLabel(proportionalLabel, TranslationService.T("field.proportional"));
        proportionalCheckBox.AutoSize = true;
        proportionalCheckBox.Dock = DockStyle.Fill;
        proportionalCheckBox.Name = "proportionalCheckBox";
        proportionalCheckBox.CheckedChanged += ProportionalCheckBox_CheckedChanged;

        ConfigureEditorLabel(anchorLabel, TranslationService.T("field.anchor"));
        anchorPanel.ColumnCount = 3;
        anchorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        anchorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        anchorPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333F));
        anchorPanel.Controls.Add(anchorTopLeftRadioButton, 0, 0);
        anchorPanel.Controls.Add(anchorTopCenterRadioButton, 1, 0);
        anchorPanel.Controls.Add(anchorTopRightRadioButton, 2, 0);
        anchorPanel.Controls.Add(anchorMiddleLeftRadioButton, 0, 1);
        anchorPanel.Controls.Add(anchorMiddleCenterRadioButton, 1, 1);
        anchorPanel.Controls.Add(anchorMiddleRightRadioButton, 2, 1);
        anchorPanel.Controls.Add(anchorBottomLeftRadioButton, 0, 2);
        anchorPanel.Controls.Add(anchorBottomCenterRadioButton, 1, 2);
        anchorPanel.Controls.Add(anchorBottomRightRadioButton, 2, 2);
        anchorPanel.Dock = DockStyle.Fill;
        anchorPanel.Name = "anchorPanel";
        anchorPanel.RowCount = 3;
        anchorPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        anchorPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        anchorPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
        ConfigureAnchorButton(anchorTopLeftRadioButton);
        ConfigureAnchorButton(anchorTopCenterRadioButton);
        ConfigureAnchorButton(anchorTopRightRadioButton);
        ConfigureAnchorButton(anchorMiddleLeftRadioButton);
        ConfigureAnchorButton(anchorMiddleCenterRadioButton);
        ConfigureAnchorButton(anchorMiddleRightRadioButton);
        ConfigureAnchorButton(anchorBottomLeftRadioButton);
        ConfigureAnchorButton(anchorBottomCenterRadioButton);
        ConfigureAnchorButton(anchorBottomRightRadioButton);
        anchorMiddleCenterRadioButton.Checked = true;

        ConfigureEditorLabel(rulerColorLabel, TranslationService.T("field.color"));
        rulerColorPanel.Controls.Add(rulerColorTextBox);
        rulerColorPanel.Controls.Add(rulerColorButton);
        rulerColorPanel.Dock = DockStyle.Fill;
        rulerColorPanel.Margin = new Padding(0);
        rulerColorPanel.WrapContents = false;
        ConfigureEditorTextBox(rulerColorTextBox, RulerColorTextBox_TextChanged);
        rulerColorTextBox.Width = 112;
        rulerColorButton.Name = "rulerColorButton";
        rulerColorButton.Size = new Size(32, 23);
        rulerColorButton.Text = "...";
        rulerColorButton.UseVisualStyleBackColor = true;
        rulerColorButton.Click += RulerColorButton_Click;
        ConfigureEditorLabel(rulerStyleLabel, TranslationService.T("field.style"));
        rulerStyleComboBox.Dock = DockStyle.Fill;
        rulerStyleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        rulerStyleComboBox.SelectedIndexChanged += RulerStyleComboBox_SelectedIndexChanged;
        ConfigureEditorLabel(rulerValueModeLabel, TranslationService.T("field.mode"));
        rulerValueModeComboBox.Dock = DockStyle.Fill;
        rulerValueModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        rulerValueModeComboBox.SelectedIndexChanged += RulerValueModeComboBox_SelectedIndexChanged;
        ConfigureEditorLabel(rulerOrientationLabel, TranslationService.T("field.line"));
        rulerOrientationComboBox.Dock = DockStyle.Fill;
        rulerOrientationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        rulerOrientationComboBox.SelectedIndexChanged += RulerOrientationComboBox_SelectedIndexChanged;
        ConfigureEditorLabel(rulerPositionLabel, TranslationService.T("field.left"));
        ConfigureEditorNumericBox(rulerPositionNumericBox, ActionNumberNumericBox_ValueChanged);
        ConfigureEditorLabel(rulerEndLabel, TranslationService.T("field.right"));
        ConfigureEditorNumericBox(rulerEndNumericBox, ActionNumberNumericBox_ValueChanged);

        mainStatusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        mainStatusStrip.Location = new Point(0, 700);
        mainStatusStrip.Name = "mainStatusStrip";
        mainStatusStrip.Size = new Size(1100, 22);
        mainStatusStrip.TabIndex = 2;

        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(39, 17);
        statusLabel.Text = TranslationService.T("status.ready");

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
        Text = TranslationService.T("app.title");
        WindowState = FormWindowState.Maximized;

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
        actionPropertiesScrollPanel.ResumeLayout(false);
        actionPropertiesPanel.ResumeLayout(false);
        actionPropertiesPanel.PerformLayout();
        anchorPanel.ResumeLayout(false);
        anchorPanel.PerformLayout();
        mainStatusStrip.ResumeLayout(false);
        mainStatusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private static void ConfigureEditorLabel(Label label, string text)
    {
        label.AutoSize = true;
        label.Dock = DockStyle.Fill;
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleLeft;
    }

    private static void ConfigureEditorTextBox(TextBox textBox, EventHandler handler)
    {
        textBox.Dock = DockStyle.Fill;
        textBox.TextChanged += handler;
    }

    private static void ConfigureEditorNumericBox(NumericUpDown numericBox, EventHandler handler)
    {
        numericBox.DecimalPlaces = 3;
        numericBox.Dock = DockStyle.Fill;
        numericBox.Increment = 0.001M;
        numericBox.Maximum = 100000M;
        numericBox.Minimum = 0M;
        numericBox.ThousandsSeparator = true;
        numericBox.KeyDown += NumericBox_KeyDown;
        numericBox.MouseDown += NumericBox_MouseDown;
        numericBox.ValueChanged += handler;
    }

    private static void ConfigureTargetParameterPanel(FlowLayoutPanel panel, NumericUpDown numericBox, CheckBox checkBox)
    {
        panel.Controls.Add(numericBox);
        panel.Controls.Add(checkBox);
        panel.Dock = DockStyle.Fill;
        panel.Margin = new Padding(0);
        panel.WrapContents = false;
        numericBox.Width = 128;
        checkBox.AutoSize = true;
        checkBox.Margin = new Padding(8, 4, 0, 0);
    }

    private void ConfigureAnchorButton(RadioButton radioButton)
    {
        radioButton.Appearance = Appearance.Button;
        radioButton.Dock = DockStyle.Fill;
        radioButton.Margin = new Padding(1);
        radioButton.TextAlign = ContentAlignment.MiddleCenter;
        radioButton.Text = "";
        radioButton.CheckedChanged += AnchorRadioButton_CheckedChanged;
    }
}
