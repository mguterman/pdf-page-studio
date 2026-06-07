using System.Windows.Forms;

namespace PdfResizer
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
