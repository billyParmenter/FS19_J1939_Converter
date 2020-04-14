/*
 * FILE          : Config.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 * Description   : This interface allows the Config class to read a list of an object 
 *                      inheriting this class to bre read from a file
 */
using System.Collections.Generic;

namespace J1939Converter.Support
{
    public interface IConfig
    {
        string fileName { get; }
        object Convert(KeyValuePair<string, string> pair);
        string objectName { get; }
    }
}
