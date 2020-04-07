using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAST_UI
{
    public class SPN
    {
        public string SpnKey { get; set; }
        public int SpnNumber { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public double Value { get; private set; }
        public SPNLength SpnLength { get; set; }
        public ResolutionRatio pgnResolution { get; set; }

        public void SetValue(string data)
        {
            string valueString = data.Substring((Position-1) * 2, SpnLength.value * 2);
            
            int rawValue = int.Parse(valueString, System.Globalization.NumberStyles.HexNumber);

            Value = rawValue * pgnResolution.value;
        }
    }
}