using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.Contracts;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Accessors.DataAccessor
{
    public class LocalConfigAccessor : ILocalConfigAccessor
    {
        public string TestMe(string param)
        {
            return param + ",LocalConfigAccessor";
        }

        public LocalData GetLocalData()
        {
            var result = new LocalData();

            result.HostName = Environment.MachineName;

            return result;
        }
    }
}
