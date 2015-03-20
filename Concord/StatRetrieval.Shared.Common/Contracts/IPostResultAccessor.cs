using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface IPostResultAccessor : IBaseService
    {
        bool PostResult(string data, long timeStamp);
    }
}
