using IssueTracker.Domain.Entities.Employee;

namespace IssueTracker.Domain.Services
{
    public interface IEmployeeService
    {
        Task<List<IEmployee>> GetAll();
    }
}
