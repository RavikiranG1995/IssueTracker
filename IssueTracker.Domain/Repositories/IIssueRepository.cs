﻿using IssueTracker.Domain.Entities.IssueFiles;
using IssueTracker.Domain.Entities.Issues;

namespace IssueTracker.Domain.Repositories
{
    public interface IIssueRepository
    {
        Task<int> Upsert(IIssue issue);
        Task<IIssue> GetIssueById(int id);
        Task<IEnumerable<IIssue>> GetAllIssues();
        Task DeleteIssue(int id);
    }
}
