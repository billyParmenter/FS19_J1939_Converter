namespace FAST_UI
{
    public class SPNLength
    {
        public int value;
        public string unit;
        public string deliminator;

        public SPNLength(string length)
        {
            string[] elements = length.Split(' ');
            if (elements.Length <= 2) //magic number
            {
                value = int.Parse(elements[0]);
                unit = elements[1];
            }
            else
            {
                value = int.Parse(elements[4]);
                unit = elements[5];
                deliminator = elements[9].Trim('"');
            }


        }
    }
}
