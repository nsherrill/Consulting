using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface IFileAccessor : IBaseService
    {
        DataTable HitBasicFile(QueryRequest query, out bool goodData, out DateTime exactQueryTime);
        DataTable HitBinaryFile(QueryRequest query, out bool goodData, out DateTime exactQueryTime);

        //void SaveLocalResult(string seralizedResult, string fileName, bool posted);
        //LocalResult[] RetrievePreviouslyUnpostedResults();
    }
}
