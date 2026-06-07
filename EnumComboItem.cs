namespace PdfPageStudio;

public sealed class EnumComboItem<TEnum>
    where TEnum : struct, Enum
{
    public EnumComboItem(TEnum value, string text)
    {
        Value = value;
        Text = text;
    }

    public TEnum Value { get; }
    public string Text { get; }

    public override string ToString()
    {
        return Text;
    }
}
