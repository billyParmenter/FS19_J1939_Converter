/*
 * FILE          : Converter.cs
 * PROJECT       : J1939Converter
 * PROGRAMMER    : Billy Parmenter
 * FIRST VERSION : Jan 27 2020
 */
using J1939Converter.Communication;
using J1939Converter.Support;
using System;

namespace J1939Converter
{
    class Converter
    {
        private static SPN _spn;
        private static CANid _canID;
        private static int[] dataLengths = new int[7] { 3, 1, 1, 8, 8, 8, 8}; // the lengths of each part of the canid
        private enum dataNames // The canID parts
        {
            priority = 0,
            reserved = 1,
            dataPage = 2,
            pduFormat = 3,
            pduSpecific = 4,
            sourceAddress = 5,
            total = 6
        };


        /*
         * FUNCTION    : ConvertToJ1939
         * DESCRIPTION : Takes the spn number and value, gets the required info from the database,
         *                  and converts them to a J1939 hex value string
         * PARAMETERS  : SPN spn - the spn to convert
         * RETURNS     : string - the converted string
         */
        public static string ConvertToJ1939(SPN spn)
        {
            _spn = spn;
            _canID = Database.GetSPN(ref spn);

            string canIdString = GenerateCanID();

            string data = GenerateDataMessage();

            return canIdString + " " + data;
        }





        /*
         * FUNCTION    : GenerateMessage
         * DESCRIPTION : Converts the spn value into a hex string data string
         * PARAMETERS  : NONE
         * RETURNS     : string - the Hex value string
         */
        private static string GenerateDataMessage()
        {
            int val = Convert.ToInt32(_spn.value / _canID.resolution);
            string hex = val.ToString("X").PadLeft(_spn.length * 2, '0');

            hex = hex.PadLeft((_spn.position - 1 + _spn.length) * 2, 'F');
            hex = hex.PadRight(16, 'F');

            return hex;
        }





        /*
         * FUNCTION    : GenerateCanID
         * DESCRIPTION : Converts the info from the database for the canID to A Hex value string
         * PARAMETERS  : NONE
         * RETURNS     : string - The converted Hex canID
         */
        private static string GenerateCanID()
        {
            string convertedString = "";

            bool isLittleEndian = BitConverter.IsLittleEndian;

            if (isLittleEndian == false)
            {
                string message = "This devices endian ness is not little endian! Results may vary";
                Console.WriteLine(message);
                Logger.Log(Logger.ErrorLevel.ERROR, message);
            }
            
            string binCANid = Convert.ToString(_canID.priority, 2).PadLeft(dataLengths[(int)dataNames.priority], '0');
            binCANid += Convert.ToString(_canID.reserved, 2).PadLeft(dataLengths[(int)dataNames.reserved], '0');
            binCANid += Convert.ToString(_canID.dataPage, 2).PadLeft(dataLengths[(int)dataNames.dataPage], '0');
            binCANid += Convert.ToString(_canID.pduFormat, 2).PadLeft(dataLengths[(int)dataNames.pduFormat], '0');
            binCANid += Convert.ToString(_canID.pduSpecific, 2).PadLeft(dataLengths[(int)dataNames.pduSpecific], '0');
            binCANid += Convert.ToString(_canID.sourceAddress, 2).PadLeft(dataLengths[(int)dataNames.sourceAddress], '0');

            convertedString = Convert.ToInt32(binCANid, 2).ToString("X").PadLeft(dataLengths[(int)dataNames.total], '0');

            return convertedString;
        }
    }
}
