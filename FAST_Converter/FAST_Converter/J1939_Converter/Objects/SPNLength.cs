/*
* FILE			:	SPNLength
* PROJECT		:   J1939Converter
* PROGRAMMER	:   Connor Lynch
* FIRST VERSION :   Feb 7 2020
* DESCRIPTION	:   defines the class for the SPN Length . parses the value from the DB record
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
    * Class 		:   SPNLength
    *
    * Description	:	encapsulates the oddly formatted SPN length field

    */
    public class SPNLength
    {
        public int value;
        public string unit;
        public string deliminator;

        public SPNLength(string length)
        {
           string[] elements = length.Split(' ');
           if(elements.Length <= 2) //magic number
            {
                value = int.Parse(elements[0]);
                unit = elements[1];
            }
            else
            {
               value = int.Parse(elements[4]);
               unit = elements[5];
               deliminator = elements[9].Trim('"');
            }


        }
    }
}
