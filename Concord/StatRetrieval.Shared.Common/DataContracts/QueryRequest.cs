using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.DataContracts
{
    public class QueryRequest
    {
        public DateTime DateToUse { get; set; }
        public string UniqueKey { get; set; }
        public string Query { get; set; }
        public QueryType Type { get; set; }
        public string ResultPath { get; set; }
        public int Interval { get; set; }
    }
}
