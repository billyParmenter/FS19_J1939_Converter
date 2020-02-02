
using System;
using J1939Converter.Support;

namespace J1939Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            string latestString = FileReader.ReadLatest();

            string[] parts = latestString.Split(',');

            string spnKey = parts[0];

            string spnValue = parts[1];

            int spnNumber = SPNs.FindSPN(spnKey);

            SPN spn = new SPN();

            if (spnNumber > 0)
            {
                spn = new SPN() { spnKey = spnKey, spnNumber = spnNumber, value = Double.Parse(spnValue) };

                string J1939string = Converter.ConvertToJ1939(spn);

                Console.WriteLine(J1939string);
            }
            else
            {
                Console.WriteLine("SPN not found: " + spnKey);
            }


            Console.ReadKey();


        }

        public static void Init()
        {
            try
            {
                Logger.InitializeLogger();
                Config.Init();
                SPNs.Init();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return;
            }
        }
    }
}
