using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Safe.MongoDB
{
    [ConnectionStringName(SafeDbProperties.ConnectionStringName)]
    public class SafeMongoDbContext : AbpMongoDbContext, ISafeMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureSafe();
        }
    }
}