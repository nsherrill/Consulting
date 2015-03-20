using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class DbParam
    {
        public string UniqueKey { get; set; }
        public string Value { get; set; }

        public DbParam(string uniqueKey, string value)
        {
            this.UniqueKey = uniqueKey;
            this.Value = value;
        }
    }
}
