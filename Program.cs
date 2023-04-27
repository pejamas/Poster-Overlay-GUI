using PosterOverlay;
using System;
using System.Windows.Forms;

namespace PosterOverlay
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // Create and show the form
            PosterOverlay form = new PosterOverlay();
            Application.Run(form);
        }
    }
}
