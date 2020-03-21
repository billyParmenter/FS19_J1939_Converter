/**
 * @file    NavigationService.cs
 * @author  Drew Hoffer
 */

using FAST_Converter.ViewModel;
using System;
using System.Collections.ObjectModel;


namespace FAST_Converter.Navigation
{
    public sealed class NavigationService
    {
        public static readonly NavigationService Instance = new NavigationService();
    
        public ObservableCollection<WrapperViewModel> NavigationHistory { get; private set; } = new ObservableCollection<WrapperViewModel>();

        public INavigationProvider Provider { get; private set; }
        
        public WrapperViewModel DefaultNavigation { get; private set;}

        public int MaxHistoryObjects { get; set; } = 5;

        public event BeforeNavigationEventHandler BeforeNavigation;

        public event AfterNavigationEventHandler AfterNavigation;

        public event BeforeClosingEventHandler BeforeClosing;

        public event AfterClosingEventHandler AfterClosing;

        public NavigationService() {

        }

        /**
        *  Navigates to a view specified by the parameter
        *  
        *  @param  WrapperViewModel navObject : An object representing the view to be navigated to
        *          
        *  @return Void
        */
        public void Navigate(WrapperViewModel navObject)
        {
            if (Provider == null)
                throw new NullReferenceException("The navigation service does not have a registered 'INavigationProvider'");

            var args = new NavigationEventArgs(navObject, Provider.Current);

            OnBeforeNavigate(this, args);

            if (Provider.Current != null && Provider.Current != navObject)
                AddToHistory(Provider.Current);

            Provider.Current = navObject;

            OnAfterNavigate(this, args);
        }

        /**
        *  Navigates to the previous window/element.  If there is no element to navigate to, 
        *  and exception is thrown.
        *  
        *  @param  Void
        *          
        *  @return Void
        */
        public void NavigateToPrevious()
        {
            if (NavigationHistory.Count <= 0)
                throw new Exception("There is no previous element to navigate");

            var previousNavigation = NavigationHistory[NavigationHistory.Count - 1];
            Navigate(previousNavigation);
        }

        /**
        *  Assigns an INavigationProvider to the current object's provider.
        *  
        *  @param  INavigationProvider provider : The navigation provider to be assigned
        *          
        *  @return Void
        */
        public void RegisterProvider(INavigationProvider provider)
        {
            this.Provider = provider;
        }

        /**
        *  Sets default navigation based on parameters.  Can force setting if the provider 
        *  is empty.
        *  
        *  @param  WrapperViewModel navigationObject : The navigation object to be set as default   
        *  @param  bool forceIfProviderEmpty : A bool value indicating if the default navigation 
        *                                      should be forced.  False by default.
        *  @return Void
        */
        public void SetDefaultNavigation(WrapperViewModel navigationObject, bool forceIfProviderEmpty = false)
        {
            DefaultNavigation = navigationObject;

            if (forceIfProviderEmpty)
            {
                if (Provider != null)
                    Provider.Current = navigationObject;
            }
        }

        /**
        *  Event handler for when the current context has not yet navigated.
        *  
        *  @param  Standard event handler parameters
        *          
        *  @return Void
        */
        private void OnBeforeNavigate(object sender, NavigationEventArgs e)
        {
            BeforeNavigation?.Invoke(this, e);
        }

        /**
        *  Event handler for when the current context has just navigated.
        *  
        *  @param  Standard event handler parameters
        *          
        *  @return Void
        */
        private void OnAfterNavigate(object sender, NavigationEventArgs e)
        {
            AfterNavigation?.Invoke(this, e);
        }

        /**
        *  Event handler for when the current context has not yet closed.
        *  
        *  @param  Standard event handler parameters
        *          
        *  @return Void
        */
        private void OnBeforeClosing(object sender, WindowEventArgs e)
        {
            BeforeClosing?.Invoke(this, e);
        }

        /**
        *  Event handler for when the current context has just closed.
        *  
        *  @param  Standard event handler parameters
        *          
        *  @return Void
        */
        private void OnAfterClosing(object sender, WindowEventArgs e)
        {
            AfterClosing?.Invoke(this, e);
        }

        /**
        *  Updates navigation history whenever the user navigates.
        *  
        *  @param  WrapperViewModel navObj : Navigation object representing the current navigation 
        *                                    point.
        *          
        *  @return Void
        */
        private void AddToHistory(WrapperViewModel navObj)
        {
            if (NavigationHistory.Count >= MaxHistoryObjects)
                NavigationHistory.RemoveAt(0);

            NavigationHistory.Add(navObj);
        }
    }
}
