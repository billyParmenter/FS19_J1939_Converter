/**
 * @file    NavigationEventArgs.cs
 * @author  Drew Hoffer
 */

using FAST_Converter.ViewModel;
using System;


namespace FAST_Converter.Navigation
{
    public delegate void BeforeNavigationEventHandler(object sender, NavigationEventArgs e);

    public delegate void AfterNavigationEventHandler(object sender, NavigationEventArgs e);

    public delegate void BeforeClosingEventHandler(object sender, WindowEventArgs e);

    public delegate void AfterClosingEventHandler(object sender, WindowEventArgs e);



    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// ViewModel to be navigated to
        /// </summary>
        public WrapperViewModel ViewModelTo { get; private set; }

        /// <summary>
        /// ViewModel to be navigated from
        /// </summary>
        public WrapperViewModel ViewModelFrom { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NavigationEventArgs() {

        }
        /// <summary>
        /// Constructor with ViewModels that indicate where navigation will go and where it came from
        /// </summary>
        public NavigationEventArgs(WrapperViewModel navigatingTo, WrapperViewModel navigatingFrom = null)
        {
            ViewModelTo = navigatingTo;
            ViewModelFrom = navigatingFrom;
        }


    }




    public class WindowEventArgs : EventArgs
    {
        /// <summary>
        /// Window to be navigated to
        /// </summary>
        public IWindow WindowTo { get; private set; }

        /// <summary>
        /// Window being navigated from
        /// </summary>
        public IWindow WindowFrom { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowEventArgs()
        {
        }

        /// <summary>
        /// Constructor with of parameters which indicate where navigation will go and where it came from
        /// </summary>
        public WindowEventArgs(IWindow windowTo, IWindow windowFrom = null)
        {
            WindowTo = windowTo;
            WindowFrom = windowFrom;
        }
    }
}
