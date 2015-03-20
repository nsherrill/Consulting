using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class Permissions
    {
        static int _tipshareLevel = 1;
        static int _issuesLevel = 5;
        static int _projectsLevel = 5;
        static int _usersLevel = 6;

        public static int TipshareLevel
        {
            get
            {
                string temp = System.Configuration.ConfigurationSettings.AppSettings["TipsharePermissionLevel"];
                if (!string.IsNullOrEmpty(temp))
                    int.TryParse(temp, out _tipshareLevel);

                return _tipshareLevel;
            }
        }

        public static int ProjectsLevel
        {
            get
            {
                string temp = System.Configuration.ConfigurationSettings.AppSettings["ProjectsPermissionLevel"];
                if (!string.IsNullOrEmpty(temp))
                    int.TryParse(temp, out _projectsLevel);

                return _projectsLevel;
            }
        }

        public static int IssuesLevel
        {
            get
            {
                string temp = System.Configuration.ConfigurationSettings.AppSettings["IssuesPermissionLevel"];
                if (!string.IsNullOrEmpty(temp))
                    int.TryParse(temp, out _issuesLevel);

                return _issuesLevel;
            }
        }

        public static int UsersLevel
        {
            get
            {
                string temp = System.Configuration.ConfigurationSettings.AppSettings["UsersPermissionLevel"];
                if (!string.IsNullOrEmpty(temp))
                    int.TryParse(temp, out _usersLevel);

                return _usersLevel;
            }
        }
    }
}
