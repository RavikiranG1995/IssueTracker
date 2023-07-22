using IssueTracker.Domain.Entities.Employee;

namespace IssueTracker.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<IEmployee>> GetAll();
    }
}
