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

        public static string ReadLatest()
        {
           
            if (File.Exists(readPath))
            {
                data = File.ReadAllText(readPath);
                dataSeperated = data.Split(',');

                if (File.Exists(writePath) == false)
                {
                    File.CreateText(writePath).Close();
                }
                File.AppendAllText(writePath, (DateTime.Now + ": " + data + "\n"));
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
                Console.WriteLine("File does not exist in destination");
            }

            return ("speed, " + dataSeperated[1]);
            //Mike loop
        }
    }
}