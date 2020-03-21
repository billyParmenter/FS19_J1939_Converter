/*
* FILE			:	Resolution Ratio
* PROJECT		:   J1939Converter
* PROGRAMMER	:   Connor Lynch
* FIRST VERSION :   Feb 7 2020
* DESCRIPTION	:   defines the class for storing the resolution of the spn. its abit more complex than a simple number. parses into this object from DB record
*                   
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J1939Converter
{   
    /*
    * Class 		:   Resolution Ratio
    *
    * Description	:	encapsulates the complex and varying units of the resolution field
    */
    public class ResolutionRatio
    {
        public double value;
        public string unit;


        public ResolutionRatio(string resolution)
        {
            string[] deliminated = { };
            switch(resolution)
            {
                case "ASCII":
                    //determine how to handle later
                   
                    break;

                case null:
                    //leave values null
                    break;

                case "Variant Determined":
                    //No idea what to do for this 
                    break;
                case "Binary":
                    //make unit binary and have the value be the value in base 10
                    break;
                default:
                    deliminated = resolution.Split(' ');
                    if (resolution.Contains("bit-mapped"))
                    {
                        value = double.Parse(deliminated[0]);
                        unit = "bit-mapped";
                    }
                    else
                    {
                        //determine if value is a fraction and grab value
                        if (deliminated[0].Contains('/'))
                        {
                            string[] erators = deliminated[0].Split('/');
                            value = double.Parse(erators[0]) / double.Parse(erators[1]); //use proper divide operation

                        }
                        else
                        {
                            value = double.Parse(deliminated[0]);
                        }

                        //handles instances of per by adding bit to denominator of unit. the math should still work
                        if (resolution.Contains("per"))
                        {
                            if (deliminated[1].Contains('/'))
                            {
                                unit = "bit * " + deliminated[1] ;
                            }
                            else
                            {
                                unit = deliminated[1] + " * bit";
                            }

                        }
                        else
                        {
                            unit = deliminated[1];
                        }
                    }
                    break;
            }
        }
    }
}
