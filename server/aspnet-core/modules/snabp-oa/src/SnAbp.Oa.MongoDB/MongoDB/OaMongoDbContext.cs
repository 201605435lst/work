using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Oa.MongoDB
{
    [ConnectionStringName(OaDbProperties.ConnectionStringName)]
    public class OaMongoDbContext : AbpMongoDbContext, IOaMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureOa();
        }
    }
}