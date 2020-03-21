/**
 * @file    IWindow.cs
 * @author  Drew Hoffer
 */

namespace FAST_Converter.Navigation
{
    /**
     * @brief An interface allowing window viewmodels to use for general window functionality 
     */
    public interface IWindow
    {
        /**
         *  Displays window that current viewmodel is associated with
         *  
         *  @param  void
         *  @return void
         */
        void ShowWindow();



        /**
         *  Closes window that current viewmodel is associated with
         *  
         *  @param  void
         *  @return void
         */
        void CloseWindow();



        /**
         *  Sets all the datacontext links for the curent window and its instantiated objects
         *  
         *  @param  void
         *  @return void
         */
        void SetDataContext(object dataContext);


        /**
         *  Loads another window from this one
         *  
         *  @param  IWindow to - The window we are loading
         *  @return void
         */
        void TransitionWindow(IWindow to);
    }
}
