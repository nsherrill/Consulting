using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public partial class CustomerDefinition : IReadable
    {
        public object ParseFromReader(System.Data.IDataReader reader)
        {
            return new CustomerDefinition()
            {
                Id = reader.ReadValue<int>("Id"),
                LastFullDate = reader.ReadValue<DateTime>("LastFullDate"),
                LastGuestCount = reader.ReadValue<int>("LastGuestCount"),
            };
        }
    }
}
