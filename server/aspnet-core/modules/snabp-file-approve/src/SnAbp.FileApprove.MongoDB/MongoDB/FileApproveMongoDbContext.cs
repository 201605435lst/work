using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.FileApprove.MongoDB
{
    [ConnectionStringName(FileApproveDbProperties.ConnectionStringName)]
    public class FileApproveMongoDbContext : AbpMongoDbContext, IFileApproveMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureFileApprove();
        }
    }
}