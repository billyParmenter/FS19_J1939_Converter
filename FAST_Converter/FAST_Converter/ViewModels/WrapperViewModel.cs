/**
* @file    WrapperViewmodel.cs
* @author  Drew Hoffer & Trent Thompson
*/

using FAST_Converter.Navigation;


namespace FAST_Converter.ViewModel
{ 
    public class WrapperViewModel : BaseViewModel
    {
        protected NavigationService Service { get; private set; }

        public WrapperViewModel() {
            if (Service == null) { 
                Service = NavigationService.Instance;
            }
        }

        /**
        *  Navigates to the specified navigation object
        *  
        *  @param  WrapperViewModel navigationObject : Navigation object representing 
        *                                              the place to navigate to.
        *          
        *  @return Void
        */
        protected virtual void Navigate(WrapperViewModel navigationObject)
        {
            Service.Navigate(navigationObject);
        }

        /**
        *  Navigates to previous window/view
        *  
        *  @param  Void
        *          
        *  @return Void
        */
        protected virtual void NavigateBack()
        {
            Service.NavigateToPrevious();
        }
    }
}
