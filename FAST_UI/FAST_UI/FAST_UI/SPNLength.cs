/*
* FILE			:	SPNLength
* PROJECT		:   FAST Dashboard
* PROGRAMMER	:   Connor Lynch
* FIRST VERSION :   Feb 7 2020
* DESCRIPTION	:   defines the class for the SPN Length . parses the value from the DB record
*                   
*/
namespace FAST_UI
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
        public int messagePartsLength = 2;

        public SPNLength(string length)
        {
            string[] elements = length.Split(' ');
            if (elements.Length <= messagePartsLength) 
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
