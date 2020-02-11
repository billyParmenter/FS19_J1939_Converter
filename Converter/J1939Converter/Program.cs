
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using J1939Converter.Support;

namespace J1939Converter
{
    class Program
    {
        public static List<SPN> spnList = new List<SPN>();
        public static bool stop = false;
        public static bool pause = false;

        static void Main()
        {

            if(Init() == false)
            {
                Console.ReadKey();
                return;
            }



            Thread t = new Thread(Convert);

            t.Start();

            while (stop == false)
            {

                char key = Console.ReadKey().KeyChar;

                if(key == 'x')
                {
                    pause = false;
                    stop = true;
                }
                else if(key == 'p')
                {
                    pause = !pause;
                }

            }


        }

        private static void Convert()
        {
            while (stop == false)
            {
                Dictionary<string, double> latestValues = FileReader.ReadLatest("");

                while (pause == true)
                {
                    Thread.Sleep(1);
                }

                foreach (KeyValuePair<string, double> pair in latestValues)
                {
                    string spnKey = pair.Key;
                    double value = pair.Value;

                    if (spnList.Any(_ => _.spnKey == spnKey))
                    {
                        int spnNumber = spnList.Find(_ => _.spnKey == spnKey).spnNumber;

                        SPN spn = new SPN() { spnKey = spnKey, spnNumber = spnNumber, value = value };
                        CANid canID = new CANid();
                        string J1939string = Converter.ConvertToJ1939(spn, ref canID);
                        Display(spn, canID, J1939string);

                    }
                    else
                    {
                        Console.WriteLine("SPN not found: " + spnKey);
                    }
                }
                Thread.Sleep(1000);

            }
        }

        private static void Display(SPN spn, CANid canID, string J1939string)
        {
            Console.WriteLine(canID);
            Console.WriteLine(spn);

            string labels = "|CAN ID";
            labels = labels.PadRight(16, ' ');
            labels += "|Data";
            labels = labels.PadRight(labels.Length + 12, ' ');
            labels += "|\n";

            string[] vals = J1939string.Split(' ');

            string values = "|" + vals[0];
            values = values.PadRight(16, ' ');
            values += "|" + vals[1];
            values = values.PadRight(labels.Length - vals[1].Length + 14, ' ');
            values += "|\n";

            int numberOfValues = 2;

            string top = ("┌");
            top = top.PadRight(16, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                top += "─";
                top = top.PadRight(top.Length + 16, '─');
            }
            top += "┐\n";

            string tableName = ("J1939 Message");
            tableName = tableName.PadLeft((top.Length / 2) + (tableName.Length / 2) - 2, ' ');
            tableName = tableName.PadRight(top.Length - 3, ' ');
            string title = "|" + tableName + "|\n";

            string mid1 = ("├");
            mid1 = mid1.PadRight(16, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid1 += "┬";
                mid1 = mid1.PadRight(mid1.Length + 16, '─');
            }
            mid1 += "┤\n";

            string mid2 = ("├");
            mid2 = mid2.PadRight(16, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                mid2 += "┼";
                mid2 = mid2.PadRight(mid2.Length + 16, '─');
            }
            mid2 += "┤\n";


            string bottom = ("└");
            bottom = bottom.PadRight(16, '─');
            for (int i = 0; i < numberOfValues - 1; i++)
            {
                bottom += "┴";
                bottom = bottom.PadRight(bottom.Length + 16, '─');
            }
            bottom += "┘\n\n";

            Console.WriteLine(top + title + mid1 + labels + mid2 + values + bottom);
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
