using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.DataContracts
{
    public class FileLastLine
    {
        public string FilePath { get; set; }
        public int LastLine { get; set; }
    }
}
