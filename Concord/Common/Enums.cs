using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum UserGroups
    {
        Level1=1,
        Level2=2,
        Level3=3,
        Level4=4,
        Level5=5,
        Admin=6
    }

    public enum IssueStatus
    {
        Unknown,
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public enum ProjectStatus
    {
        Unknown,
        Proposed,
        Pending,
        InProgress,
        Completed,
        Rejected,
        Paused,
        Cancelled
    }

    public enum SPRequestStatus
    {
        Requested = 0,
        Pending = 1,
        Errored = 2,
        Complete = 3
    }

    public enum SPSource
    {
        Store = 0,
        Website = 1,
        Scheduled = 2,
        Manual = 3
    }
}
