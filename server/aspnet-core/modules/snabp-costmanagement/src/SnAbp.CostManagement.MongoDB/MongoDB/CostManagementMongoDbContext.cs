using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.CostManagement.MongoDB
{
    [ConnectionStringName(CostManagementDbProperties.ConnectionStringName)]
    public class CostManagementMongoDbContext : AbpMongoDbContext, ICostManagementMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureCostManagement();
        }
    }
}