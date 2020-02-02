using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J1939Converter
{
    class CANid
    {
        public int priority;
        public int reserved;
        public int dataPage;
        public int pduFormat;
        public int pduSpecific;
        public int sourceAddress;
        public int PGN;
        public double resolution;
    }
}
