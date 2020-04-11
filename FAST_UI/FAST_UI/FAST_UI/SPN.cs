/*
 * FILE          : SPN.cs
 * PROJECT       : FAST Dashboard
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST_UI
{
    /*
     * NAME    : SPN
     * PURPOSE : The SPN clas has been created to
     *              hold the values that are related to an SPN
     */
    public class SPN
    {
        public string SpnKey { get; set; }
        public int SpnNumber { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public double Value { get; private set; }
        public SPNLength SpnLength { get; set; }
        public ResolutionRatio pgnResolution { get; set; }

        /*
         * FUNCTION    : SetValue
         * DESCRIPTION : Takes the raw data value and converts it to human readable values
         * PARAMETERS  : string data
         * RETURNS     : NONE
         */
        public void SetValue(string data)
        {
            string valueString = data.Substring((Position-1) * 2, SpnLength.value * 2);
            
            int rawValue = int.Parse(valueString, System.Globalization.NumberStyles.HexNumber);

            Value = rawValue * pgnResolution.value;
        }
    }
}