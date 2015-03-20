using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface IRemoteConfigAccessor : IBaseService
    {
        ConfigResult GetConfigs();
    }
}
