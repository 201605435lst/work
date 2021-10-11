using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace SnAbp.Quality.Entities
{
    public class QualityDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public QualityDataSeedContributor(IDataDictionaryRepository dataDictionaryRepository) => DataDictionaryRepository = dataDictionaryRepository;
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var dataSeeds = GetDataSeeder();
            if (dataSeeds.Any())
                foreach (var item in dataSeeds) await DataDictionaryRepository.InsertAsync(item);
        }
        private static List<DataDictionary> GetDataSeeder()
        {
            var qualityId = Guid.NewGuid();
            return new List<DataDictionary>()
            {
                  new DataDictionary(qualityId){
                    Name = "质量问题",
                    Key = "QualityManager",
                    IsStatic = true,
                    Children= new List<DataDictionary>(){
                          new DataDictionary(Guid.NewGuid()){Name = "适用范围",Key = "QualityManager.Scop",IsStatic = true,ParentId=qualityId},
                    }
                },
            };
        }
    }
}
