using SnAbp.Uow;

using Volo.Abp.Uow;

namespace SnAbp.EntityFrameworkCore
{
    public class AbpEfCoreDbContextInitializationContext
    {
        public IUnitOfWork UnitOfWork { get; }

        public AbpEfCoreDbContextInitializationContext(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}