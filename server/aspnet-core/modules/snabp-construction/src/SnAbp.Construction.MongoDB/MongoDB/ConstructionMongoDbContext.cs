using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Construction.MongoDB
{
    [ConnectionStringName(ConstructionDbProperties.ConnectionStringName)]
    public class ConstructionMongoDbContext : AbpMongoDbContext, IConstructionMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureConstruction();
        }
    }
}