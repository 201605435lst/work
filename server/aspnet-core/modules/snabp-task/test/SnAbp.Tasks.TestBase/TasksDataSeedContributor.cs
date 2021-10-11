using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace SnAbp.Tasks
{
    public class TasksDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;

        public TasksDataSeedContributor(
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
        }
        
        public Task SeedAsync(DataSeedContext context)
        {
            /* Instead of returning the Task.CompletedTask, you can insert your test data
             * at this point!
             */

            //return Task.CompletedTask;
            throw new System.NotImplementedException();
        }

        System.Threading.Tasks.Task IDataSeedContributor.SeedAsync(DataSeedContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}