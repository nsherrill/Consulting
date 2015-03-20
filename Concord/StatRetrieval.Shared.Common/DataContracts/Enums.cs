using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatRetrieval.Shared.Common.DataContracts
{
    public enum QueryType
    {
        SqlServer,
        NUFlatFile,
        KDSBinary,
        FlatFile,
    }

    public enum LogLocation
    {
        Generic,
        QueryRequestAccessor,
        RemoteConfigAccessor,
        DbAccessor,
        FileAccessor,
        LocalConfigAccessor,
        CycleManager,
        PostResultAccessor,
        ResultProcessorEng,
        SRService,
        PhpHelper,
    }

    public enum LogType
    {
        Generic,
        Error,
        Verbose,
        ExtraVerbose,
    }
}
