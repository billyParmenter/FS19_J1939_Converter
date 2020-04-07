using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST_UI
{
    public static class PgnReader
    {
        public static int ParseString(string pgn)
        {
            string binary = ConvertHexToBinary(pgn);
            List<KeyValuePair<string, int>> pgnParts = ParseBinary(binary);

            return pgnParts[6].Value;
        }

        /*  ~~~ FUNCTION ~~~
         * 
         * NAME: 
         *  ParseBinary
         *  
         * DESCRIPTION:
         *  Breaks the binary string into the different parts of a PGN
         *  
         * PARAMETERS:
         *  string binary - the string of binary numbers to parse
         *  
         * RETURN:
         *  List<KeyValuePair<string, int>> - the different parts of the PGN with the key as the name and the value as the decimal value
         */
        public static List<KeyValuePair<string, int>> ParseBinary(string binary)
        {
            List<KeyValuePair<string, int>> PGN_Parts = InitializePgnPartsList(); //Fills the list with the parts names and values of 0
            List<int> PGN_Parts_Sizes = new List<int>() { 8, 8, 8, 1, 1, 3, 18 }; //The sizes for each part
            string tmpBinary = binary;
            int i = 0;


            while (tmpBinary.Length != 0)
            {
                string partName = PGN_Parts[i].Key;
                int partLength = PGN_Parts_Sizes[i];

                // If the string is smaller than the parts length then just save it into the value and break
                if (tmpBinary.Length < partLength)
                {
                    PGN_Parts[i] = new KeyValuePair<string, int>(partName, Convert.ToInt32(tmpBinary, 2));
                    break;
                }

                // If there is still more to parse after this value then get the current value, save it, remove
                // that from the end of the string, and then continu to the next value
                else
                {
                    string part = tmpBinary.Substring(tmpBinary.Length - partLength, partLength);
                    int partValue = Convert.ToInt32(part, 2);


                    PGN_Parts[i] = new KeyValuePair<string, int>(partName, partValue);
                    tmpBinary = tmpBinary.Substring(0, tmpBinary.Length - partLength);
                }

                i++;
            }

            // Get the PGN ID from the original binary
            PGN_Parts[PGN_Parts.Count - 1] = new KeyValuePair<string, int>(PGN_Parts[PGN_Parts.Count - 1].Key, Convert.ToInt32(binary.Substring(binary.Length - 8 - PGN_Parts_Sizes.Last(), PGN_Parts_Sizes.Last()), 2));

            return PGN_Parts;
        }





        /*  ~~~ FUNCTION ~~~
         * 
         * NAME: 
         *  ConvertHexToBinary
         *  
         * DESCRIPTION:
         *  Converts the given hex string to a binary string
         *  
         * PARAMETERS:
         *  string input - the hex string to convert
         *  
         * RETURN:
         *  string - binary string
         */
        public static string ConvertHexToBinary(string input)
        {
            return Convert.ToString(Convert.ToInt64(input, 16), 2);
        }





        /*  ~~~ FUNCTION ~~~
         * 
         * NAME: 
         *  InitializePgnPartsList
         *  
         * DESCRIPTION:
         *  Converts the given hex string to a binary string
         *  
         * PARAMETERS:
         *  NONE
         *  
         * RETURN:
         *  List<KeyValuePair<string, int>> - The list of parts names with values set to 0
         */
        public static List<KeyValuePair<string, int>> InitializePgnPartsList()
        {
            return new List<KeyValuePair<string, int>>()
            {
                new KeyValuePair<string, int> ("Source Address", 0 ),
                new KeyValuePair<string, int> ("PDU Specific", 0 ),
                new KeyValuePair<string, int> ("PDU Format", 0 ),
                new KeyValuePair<string, int> ("Data Page", 0 ),
                new KeyValuePair<string, int> ("Reserved", 0 ),
                new KeyValuePair<string, int> ("Priority", 0 ),
                new KeyValuePair<string, int> ("Parameter Group Number", 0 )
            };

        }





        /*  ~~~ FUNCTION ~~~
         * 
         * NAME: 
         *  PrintList
         *  
         * DESCRIPTION:
         *  Prints the given list of key value pairs
         *  
         * PARAMETERS:
         *  List<KeyValuePair<string, int>> PGN_Parts - the list to print
         *  
         * RETURN:
         *  NONE
         */
        public static void PrintList(List<KeyValuePair<string, int>> PGN_Parts)
        {

            foreach (KeyValuePair<string, int> pair in PGN_Parts)
            {
                Console.WriteLine("\t" + pair.Key + ": " + pair.Value);
            }

        }
    }
}
