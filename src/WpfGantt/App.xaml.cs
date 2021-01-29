using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfGantt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var win = new MainWindow();
            win.Show();

            win.wpfPlot1.Reset();
            var recipe = (IRecipe)Activator.CreateInstance(typeof(Gantt));
            recipe.ExecuteRecipe(win.wpfPlot1.Plot);
            win.wpfPlot1.Render();
        }
    }
}
