namespace PdfPageStudio;

public sealed partial class ProjectSetupForm : Form
{
    private readonly bool _isNewProject;

    public ProjectSetupForm(PdfPageStudioProject project, bool isNewProject)
    {
        _isNewProject = isNewProject;
        ProjectName = project.Name;
        ProjectDescription = project.Description;
        UnitType = project.UnitType;
        InitializeComponent();
        BindProject();
    }

    public string ProjectName { get; private set; }
    public string ProjectDescription { get; private set; }
    public UnitType UnitType { get; private set; }

    private void BindProject()
    {
        Text = TranslationService.T(_isNewProject ? "dialog.newProject.title" : "dialog.projectSettings.title");
        createButton.Text = TranslationService.T(_isNewProject ? "common.create" : "common.apply");
        nameTextBox.Text = ProjectName;
        descriptionTextBox.Text = ProjectDescription;
        ConfigureEnumCombo(unitTypeComboBox, UnitType);
    }

    private static void ConfigureEnumCombo(ComboBox comboBox, UnitType selectedValue)
    {
        comboBox.DisplayMember = nameof(EnumComboItem<UnitType>.Text);
        comboBox.ValueMember = nameof(EnumComboItem<UnitType>.Value);
        comboBox.Items.Clear();
        foreach (var value in Enum.GetValues<UnitType>())
        {
            comboBox.Items.Add(new EnumComboItem<UnitType>(value, TranslationService.T("enum.unit." + value)));
        }

        foreach (var item in comboBox.Items.OfType<EnumComboItem<UnitType>>())
        {
            if (item.Value == selectedValue)
            {
                comboBox.SelectedItem = item;
                return;
            }
        }
    }

    private void CreateButton_Click(object? sender, EventArgs e)
    {
        ProjectName = nameTextBox.Text.Trim();
        ProjectDescription = descriptionTextBox.Text;
        if (unitTypeComboBox.SelectedItem is EnumComboItem<UnitType> item)
        {
            UnitType = item.Value;
        }
    }
}
