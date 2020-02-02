


using J1939Converter.Communication;
using J1939Converter.Support;
using System;

namespace J1939Converter
{
    class Converter
    {
        private static SPN _spn;
        private static CANid _canID;
        private static int[] dataLengths = new int[7] { 3, 1, 1, 8, 8, 8, 8};
        private enum dataNames
        {
            priority = 0,
            reserved = 1,
            dataPage = 2,
            pduFormat = 3,
            pduSpecific = 4,
            sourceAddress = 5,
            total = 6
        };

        public static string ConvertToJ1939(SPN spn)
        {
            _spn = spn;
            _canID = Database.GetSPN(ref spn);

            string canIdString = GenerateCanID();

            string data = GenerateMessage();

            return canIdString + " " + data;
        }

        private static string GenerateMessage()
        {
            int val = Convert.ToInt32(_spn.value / _canID.resolution);
            string hex = val.ToString("X");

            hex = hex.PadLeft((_spn.position - 1 + _spn.length) * 2, 'F');
            hex = hex.PadRight(16, 'F');

            return hex;
        }

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
