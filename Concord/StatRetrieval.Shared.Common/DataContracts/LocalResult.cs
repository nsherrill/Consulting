using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.DataContracts
{
    public class LocalResult
    {
        public string UniqueKey { get; set; }
        public QueryRequest Request { get; set; }
        public DataTable Results { get; set; }
        public bool Posted { get; set; }
        public DateTime Time { get; set; }
        public string FailedFileName { get; set; }

        public LocalResult()
        {
        }

        public LocalResult(string uniqueKey, QueryRequest request, DataTable results, DateTime queryTime, bool posted)
            : base()
        {
            this.Request = request;
            this.Time = queryTime;
            this.UniqueKey = uniqueKey;
            this.Results = results;
            this.Posted = posted;
        }
    }
}
