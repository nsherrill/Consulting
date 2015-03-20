using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using StatRetrieval.Shared.Common.DataContracts;

namespace StatRetrieval.Shared.Common.Contracts
{
    public interface IDbAccessor : IBaseService
    {
        DataTable HitDatabase(QueryRequest request, out bool goodData, out DateTime exactQueryTime);
        List<EmployeeData> GetEmployees();
    }
}
