using IssueTracker.Domain.Entities.Employee;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;

namespace IssueTracker.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<IEmployee>> GetAll()
        {
            return await _employeeRepository.GetAll();
        }
    }
}
