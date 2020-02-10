
﻿using System.Collections.Generic;

namespace J1939Converter.Support
{
    public interface IConfig
    {
        string fileName { get; }
        object Convert(KeyValuePair<string, string> pair);
    }
}
