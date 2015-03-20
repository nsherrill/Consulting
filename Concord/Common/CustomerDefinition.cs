using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Common
{
    public partial class CustomerDefinition
    {
        public int Id { get; set; }
        public long LastGuestCount { get; set; }
        public DateTime LastFullDate { get; set; }
        public string IP { get; set; }
    }
}
