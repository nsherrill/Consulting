using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface ICycleManager : IBaseService
    {
        void InitFirstRun();

        void RunCycle();

        void RunTests();
    }
}
