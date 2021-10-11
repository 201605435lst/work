using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.ConstructionBase.MongoDB
{
    [ConnectionStringName(ConstructionBaseDbProperties.ConnectionStringName)]
    public class ConstructionBaseMongoDbContext : AbpMongoDbContext, IConstructionBaseMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureConstructionBase();
        }
    }
}