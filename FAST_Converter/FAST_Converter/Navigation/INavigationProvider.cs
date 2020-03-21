/**
 * @file    INavigationProvider.cs
 * @author  Drew Hoffer
 */

using FAST_Converter.ViewModel;

namespace FAST_Converter.Navigation
{
    /**
     * @brief An interface allowing navigation to be utilized by inheriting viewmodels.
     * The app viewmodel that inherits this will need an instance of the current window we are looking at and a reference to the overalll window application
     */
    public interface INavigationProvider
    {

        WrapperViewModel Current { get; set; }

        IWindow Window { get; set; }
    }
}
