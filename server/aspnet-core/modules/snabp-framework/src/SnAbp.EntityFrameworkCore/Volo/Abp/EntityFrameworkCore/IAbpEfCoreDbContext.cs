namespace SnAbp.EntityFrameworkCore
{
    public interface IAbpEfCoreDbContext : IEfCoreDbContext
    {
        void Initialize(AbpEfCoreDbContextInitializationContext initializationContext);
    }
}