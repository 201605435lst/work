using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.File2.MongoDB
{
    [ConnectionStringName(File2DbProperties.ConnectionStringName)]
    public class File2MongoDbContext : AbpMongoDbContext, IFile2MongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureFile2();
        }
    }
}