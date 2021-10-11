using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace SnAbp.Construction
{
    public class ConstructionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public ConstructionDataSeedContributor(IDataDictionaryRepository dataDictionaryRepository) => DataDictionaryRepository = dataDictionaryRepository;
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var dataSeeds = GetDataSeeder(); ;
            if (dataSeeds.Any())
                foreach (var item in dataSeeds) await DataDictionaryRepository.InsertAsync(item);
        }
        private static List<DataDictionary> GetDataSeeder()
        {
            return new List<DataDictionary>()
            {
            };
        }
    }
}
