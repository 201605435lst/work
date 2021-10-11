using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Material.MongoDB
{
    [ConnectionStringName(MaterialDbProperties.ConnectionStringName)]
    public class MaterialMongoDbContext : AbpMongoDbContext, IMaterialMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureMaterial();
        }
    }
}