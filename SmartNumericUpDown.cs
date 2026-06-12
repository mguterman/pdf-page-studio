using System.Globalization;

namespace PdfPageStudio;

internal sealed class SmartNumericUpDown : NumericUpDown
{
    private const int DebounceMilliseconds = 1000;
    private readonly System.Windows.Forms.Timer _timer;
    
    private TextBox _textBox = null!;

    private static int GetDecimalPlaces(string text)
    {
        var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        var index = text.IndexOf(separator, StringComparison.Ordinal);
        if (index < 0)
        {
            index = text.IndexOf('.', StringComparison.Ordinal);
        }

        return index < 0 ? 0 : text.Length - index - 1;
    }

    private static decimal GetNumericStep(string text, int digitIndex)
    {
        if (digitIndex < 0 || digitIndex >= text.Length || !char.IsDigit(text[digitIndex]))
        {
            return 0;
        }

        var decimalIndex = text.IndexOf(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
        if (decimalIndex < 0)
        {
            decimalIndex = text.IndexOf('.', StringComparison.Ordinal);
        }

        int exponent;
        if (decimalIndex < 0 || digitIndex < decimalIndex)
        {
            var integerDigitsBeforeTarget = text.Take(digitIndex).Count(char.IsDigit);
            var integerDigits = text.Take(decimalIndex < 0 ? text.Length : decimalIndex).Count(char.IsDigit);
            exponent = integerDigits - integerDigitsBeforeTarget - 1;
        }
        else
        {
            exponent = -text.Skip(decimalIndex + 1)
                .Take(digitIndex - decimalIndex)
                .Count(char.IsDigit);
        }

        return (decimal)Math.Pow(10, exponent);
    }

    private void ApplyStep(int shift)
    {
        var text = _textBox.Text;
        var rightCaret = _textBox.TextLength - _textBox.SelectionStart;

        var cursor = text.Length - rightCaret;
        var digitIndex = FindNumericStepDigit(text, cursor);
        var step = GetNumericStep(text, digitIndex);
        var value = decimal.Parse(text, NumberStyles.Number, CultureInfo.CurrentCulture);
        value += shift * step;

        var decimalPlaces = GetDecimalPlaces(text);
        _textBox.Text = value.ToString("F" + decimalPlaces, CultureInfo.CurrentCulture);

        BeginInvoke(() =>
        {
            var position = Math.Clamp(_textBox.TextLength - rightCaret, 0, _textBox.TextLength);
            _textBox.SelectionStart = position;
            _textBox.SelectionLength = 0;
        });
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Up or Keys.Down)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            return;
        }

        base.OnKeyDown(e);
    }

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode is not (Keys.Up or Keys.Down))
        {
            return;
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
    }
   
    public override void UpButton()
    {
        ApplyStep(1);
    }

    public override void DownButton()
    {
        ApplyStep(-1);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if (e.Delta == 0)
        {
            return;
        }
        ApplyStep(Math.Sign(e.Delta));
    }

    public SmartNumericUpDown()
    {
        _timer = new System.Windows.Forms.Timer { Interval = DebounceMilliseconds };
        _timer.Tick += (_, _) =>
        {
            _timer.Stop();
            SmartValueChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        _textBox = Controls.OfType<TextBox>().First();
        _textBox.KeyDown += TextBox_KeyDown;
    }

    public event EventHandler? SmartValueChanged;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _timer.Dispose();
        }

        base.Dispose(disposing);
    }
           
    protected override void OnValueChanged(EventArgs e)
    {
        base.OnValueChanged(e);
        ResetTimer();
    }   

    private void ResetTimer()
    {
        _timer.Stop();
        _timer.Start();
    }     

    private static int FindNumericStepDigit(string text, int cursorPosition)
    {
        if (cursorPosition < text.Length)
        {
            for (var index = cursorPosition; index < text.Length; index++)
            {
                if (char.IsDigit(text[index]))
                {
                    return index;
                }
            }
        }

        for (var index = cursorPosition - 1; index >= 0; index--)
        {
            if (char.IsDigit(text[index]))
            {
                return index;
            }
        }

        return -1;
    }
}
