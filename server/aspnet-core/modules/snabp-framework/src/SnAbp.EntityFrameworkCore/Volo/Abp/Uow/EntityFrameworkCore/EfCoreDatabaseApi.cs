using System.Threading;
using System.Threading.Tasks;
using SnAbp.EntityFrameworkCore;

using Volo.Abp.Uow;

namespace SnAbp.Uow.EntityFrameworkCore
{
    public class EfCoreDatabaseApi<TDbContext> : IDatabaseApi, ISupportsSavingChanges
        where TDbContext : IEfCoreDbContext
    {
        public TDbContext DbContext { get; }

        public EfCoreDatabaseApi(TDbContext dbContext)
        {
            DbContext = dbContext;
        }
        
        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}