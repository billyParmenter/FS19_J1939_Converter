using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace J1939Converter.Support
{
    class FileReader
    {
        private static string readPath = Config.configValues["modOutputFile"];
        private static string writePath = Config.configValues["readerLogLocation"];
        private static string data = string.Empty;
        private static string[] dataSeperated;


        public static Dictionary<string, double> ReadLatest()
        {
            Dictionary<string, double> keyValueData = new Dictionary<string, double>();

            if (File.Exists(readPath))
            {
                data = File.ReadAllText(readPath);
                dataSeperated = data.Split(',');

                if (File.Exists(writePath) == false)
                {
                    File.CreateText(writePath).Close();
                }
                //keyValueData.Add("model", Convert.ToDouble(dataSeperated[0]));
                keyValueData.Add("speed", Math.Round(Convert.ToDouble(dataSeperated[1]), 2));
                keyValueData.Add("fuelUsed", Math.Round(Convert.ToDouble(dataSeperated[2]), 2));
                keyValueData.Add("fuelLevel", Math.Round(Convert.ToDouble(dataSeperated[3]), 2));

                File.AppendAllText(writePath, (DateTime.Now + ": model, " + dataSeperated[0] +
                                                                ", speed, " + dataSeperated[1] +
                                                                ", fuelUsed, " + dataSeperated[2] +
                                                                ", fuelLevel, " + dataSeperated[3] + "\n"));
                Console.WriteLine("New data stored");

                try
                {
                    //File.Delete(readPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                //Console.WriteLine("File does not exist in destination");
                keyValueData.Add("FNF", -1);
            }

            return keyValueData;
        }
    }
}