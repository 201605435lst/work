using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace SnAbp.CostManagement
{
    public class CostManagementDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public CostManagementDataSeedContributor(IDataDictionaryRepository dataDictionaryRepository) => DataDictionaryRepository = dataDictionaryRepository;
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var dataSeeds = GetDataSeeder(); ;
            if (dataSeeds.Any())
                foreach (var item in dataSeeds) await DataDictionaryRepository.InsertAsync(item);
        }
        private static List<DataDictionary> GetDataSeeder()
        {
            var safeId = Guid.NewGuid();
            return new List<DataDictionary>()
            {
                new DataDictionary(Guid.NewGuid())
                {
                    Name = "费用类型",
                    Key = "CostmanagementCostType",
                    IsStatic = true
                },
                new DataDictionary(Guid.NewGuid())
                {
                    Name = "成本合同类别",
                    Key = "CostmanagementContract",
                    IsStatic = true
                },
                new DataDictionary(Guid.NewGuid())
                {
                    Name = "收款单位",
                    Key = "CostmanagementCostPayee",
                    IsStatic = true
                },
                new DataDictionary(Guid.NewGuid())
                {
                    Name = "资金类别",
                    Key = "CostmanagementCapitalCategory",
                    IsStatic = true
                },
            };

        }
    }
}
