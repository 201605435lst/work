using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace SnAbp.ComponentTrack.MongoDB
{
    [ConnectionStringName(ComponentTrackDbProperties.ConnectionStringName)]
    public class ComponentTrackMongoDbContext : AbpMongoDbContext, IComponentTrackMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureComponentTrack();
        }
    }
}