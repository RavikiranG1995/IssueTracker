using IssueTracker.Domain.Entities.Employee;
using IssueTracker.Domain.Repositories;
using IssueTracker.Infrastructure.Database.Helpers;
using System.Data;

namespace IssueTracker.Infrastructure.Database.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDataBaseWrapper _dataBaseProxy;
        public EmployeeRepository(IDataBaseWrapper dataBaseProxy)
        {
            _dataBaseProxy = dataBaseProxy;
        }
        public async Task<List<IEmployee>> GetAll()
        {
            using var employeeDT = await _dataBaseProxy.GetDataTableAsync("usp_Employee_GetAll", null, "", CancellationToken.None);
            var employeeList = new List<IEmployee>();
            foreach (DataRow dataRow in employeeDT.Rows)
            {
                var employee = new Domain.Entities.Employee.Employee
                {
                    Id = (int)dataRow["Id"],
                    Name = (string)dataRow["Name"]
                };
                employeeList.Add(employee);
            }
            return employeeList;
        }
    }
}
