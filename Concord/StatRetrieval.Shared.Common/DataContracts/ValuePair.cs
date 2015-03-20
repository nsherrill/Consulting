using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.DataContracts
{
    public class ValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public ValuePair(string key, string Value)
        {
            this.Key = key;
            this.Value = Value;
        }

        public override string ToString()
        {
            return string.Format("\"{0}\":\"{1}\"", Key, Value);
        }
    }
}
