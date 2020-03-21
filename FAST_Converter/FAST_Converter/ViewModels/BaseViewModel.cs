/**
 * @file    BaseViewModel.cs
 * @author  Trent Thompson
 * 
 */

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FAST_Converter.ViewModel
{

    /** 
     * @brief   Implements the INotifyPropertyChanged class on all children classes 
     * 
     * Abstract class that provides ViewModel functionality for objects
     * The BaseViewModel class was retrieved from the project at https://github.com/Joben28/WPFChat
     * The generic code in this file provides an easily adaptable interface to give ViewModel
     * functionality to Model data objects
     */
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// Event that is raised in a ViewModel object to indicate to either the
        /// Model or the View that the data in the object has been modified
        public event PropertyChangedEventHandler PropertyChanged;


        /**
         * @brief   Invokes the property changed event on the affected UI element.
         *          Results in an updated UI element.
         *          
         * @param   propertyName string - The name of the UI Element
         * @return  void
         */
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /**
         * @brief   Invokes the property changed event on the affected UI element.
         *          Results in an updated UI element.
         * 
         * @param   backingField ref T
         * @param   value T
         * @param   propertyName string [CallerMemberName] - Default value is "" (string.Empty)
         * @return  bool
         */
        protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
