/**
 * @file    CoreNavigation.cs
 * @author  Drew Hoffer
 */

using FAST_Converter.ViewModel;

namespace FAST_Converter.Navigation.Startup
{
    /**
     * @brief A View Model for the AdminCarrierDataView. Much of the navigation related aspects can be found here : https://github.com/Tosker/WpfViewChanger
     */
    public class CoreNavigation
    {
        private NavigationService service;

        public INavigationProvider Handler { get; set;}


        /**
         *  Instantiates a new CoreNavigation object with reference to the Navservice instance
         */
        public CoreNavigation() 
        {
            service = NavigationService.Instance;
        }

        /**
         *  Links a navigation handler and instance of the service to the corenavigationviewmodel.
         *  Inheriting classes will now be registerd with a window
         *  
         *  @param  INavigationProvider navigationHandler
         *  @param  WrapperViewModel defaultNavigation
         *  @param forceDefaultNavigation
         *  @return void
         */
        public void Startup(INavigationProvider navigationHandler, WrapperViewModel defaultNavigation = null,
                bool forceDefaultNavigation = false) 
        {
            service.RegisterProvider(navigationHandler);
            Handler = navigationHandler;
            if (defaultNavigation != null) 
            {
                service.SetDefaultNavigation(defaultNavigation,forceDefaultNavigation);
            }
        }
    }
}
