/**
 * @file    ButtonCommand.cs
 * @author  Trent Thompson & Drew Hoffer
 *
 */
using System;
using System.Windows.Input;

namespace FAST_Converter.ViewModel
{
    /**
     * @brief   This class helps link view-level buttons to appropriate commands based on what data they need to access.
     */
    public class ButtonCommand : ICommand {
        private Action WhatToExecute;
        private Func<bool> WhenToExecute;

        /**
         *  @brief  ButtonCommand() - Constructor.  Takes two delegates and assigns them  
         *          to the internal delegates WhatToExecute and WhenToExecute.
         *          
         *  @param  Action What
         *          delegate which has no return value
         *          Func<bool> When
         *          delegate which has a bool return value
         *          
         *          
         *  @return none
         *          
         */
        public ButtonCommand(Action What, Func<bool> When) {
            WhatToExecute = What;
            WhenToExecute = When;
        }

        public event EventHandler CanExecuteChanged;

        /**
         *  @brief  CanExecute() - Function which returns whether or not the command associated with 
         *          the button can execute.
         *          
         *  @param  object parameter
         *          Default parameter.
         *          
         *          
         *  @return WhenToExecute()
         *          The bool-returning delegate specified in the class.  It is called here as a function.
         */
        public bool CanExecute(object parameter) {
            return WhenToExecute();
        }

        /**
         *  @brief  Execute() - Function which executes the command associated with the button.
         *          
         *  @param  object parameter
         *          Default parameter.
         *          
         *          
         *  @return none
         */
        public void Execute(object parameter) {
            WhatToExecute();
            ExecuteChanged();
        }

        /**
         *  @brief  ExecuteChanged() - Function used by the class's event handler CanExecuteChanged.
         *          Changes the event handler's definition if it is able to be changed.
         *          
         *  @param  none
         *          
         *          
         *  @return none
         */
        public void ExecuteChanged() {
            if (CanExecuteChanged != null) {
                CanExecuteChanged(this, new EventArgs());
            }
        }
    }
}
