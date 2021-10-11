using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.Project.MongoDB
{
    [ConnectionStringName(ProjectDbProperties.ConnectionStringName)]
    public class ProjectMongoDbContext : AbpMongoDbContext, IProjectMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureProject();
        }
    }
}