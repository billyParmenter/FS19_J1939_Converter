using FAST_Converter.Navigation;
using System.Windows;

namespace FAST_Converter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Closing += ViewModel.MainPageViewModel.OnWindowClosing;
            var provider = NavigationService.Instance.Provider;
            DataContext = provider;
        }
    }
}
