using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J1939Converter.Support
{
    class FileReader
    {
        private string readPath = Config.configValues["modOutputFile"];
        private string writePath = Config.configValues["readerLogLocation"];

        public static string ReadLatest()
        {
           
            return ("speed, 20.4");
            //Mike loop
        }
    }
}
