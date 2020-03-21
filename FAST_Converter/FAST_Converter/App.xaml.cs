using FAST_Converter.Navigation.Startup;
using FAST_Converter.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FAST_Converter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            CoreNavigation core = new CoreNavigation();

            core.Startup(new AppViewModel(), new SettingsViewModel(), true);
            DataTemplateManager manager = new DataTemplateManager();
            manager.LoadDataTemplateByConvention();
        }
    }
}
