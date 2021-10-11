using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.BackgroundJobs
{
    public interface IBackgroundJobRepository : IBasicRepository<BackgroundJobRecord, Guid>
    {
        Task<List<BackgroundJobRecord>> GetWaitingListAsync(int maxResultCount);
    }
}