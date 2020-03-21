/**
 * @file    AppViewModel.cs
 * @author  Drew Hoffer & Trent Thompson
 */


using FAST_Converter.Navigation;


namespace FAST_Converter.ViewModel
{
    class AppViewModel : WrapperViewModel, INavigationProvider
    {
        private WrapperViewModel _current;

        public WrapperViewModel Current 
        {
            get 
            {
                return _current;
            }
            set 
            {
                OnPropertyChanged(ref _current, value);
            }
        }


        private IWindow _window;


        public IWindow Window {
            get 
            {
                return _window;
            }
            set 
            {
                OnPropertyChanged(ref _window, value);
            }
        }

        public AppViewModel() {
           
        }

    }
}
