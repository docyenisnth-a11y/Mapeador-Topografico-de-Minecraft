using System;
using System.Windows.Forms;

namespace MinecraftMapApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Inicializa as configurações visuais do Windows Forms
            ApplicationConfiguration.Initialize();
            
            // Abre o formulário principal que você programou
            Application.Run(new FormMapa());
        }
    }
}
