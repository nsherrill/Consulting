using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common
{
    public interface IReadable
    {
        object ParseFromReader(IDataReader reader);
    }
}
