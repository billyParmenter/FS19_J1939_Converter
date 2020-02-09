
using System;
using System.Collections.Generic;
using System.Linq;
using J1939Converter.Support;

namespace J1939Converter
{
    class Program
    {
        public static List<SPN> spnList = new List<SPN>();
        static void Main(string[] args)
        {

            if(Init() == false)
            {
                Console.ReadKey();
                return;
            }

            string latestString = FileReader.ReadLatest();

            string[] parts = latestString.Split(',');

            string spnKey = parts[0];

            string spnValue = parts[1];


            if (spnList.Any(_ => _.spnKey == spnKey))
            {
                int spnNumber = spnList.Find(_ => _.spnKey == spnKey).spnNumber;

                SPN spn = new SPN() { spnKey = spnKey, spnNumber = spnNumber, value = Double.Parse(spnValue) };
                CANid canID = new CANid();
                string J1939string = Converter.ConvertToJ1939(spn, ref canID);

                Console.WriteLine(spn);
                Console.WriteLine(canID);
                Console.WriteLine("CANid    Data");
                Console.WriteLine(J1939string);

            }
            else
            {
                Console.WriteLine("SPN not found: " + spnKey);
            }


            Console.ReadKey();


        }

        public static bool Init()
        {
            try
            {
                Logger.InitializeLogger();
                Config.Init();
                spnList = SPN.GetSPNs();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
