﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface IPostErrorAccessor : IBaseService
    {
        bool PostError(string text, LogLocation logLoc);
    }
}
