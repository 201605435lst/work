using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using SnAbp.Identity;

namespace SnAbp.Technology
{
    public class TechnologyDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public TechnologyDataSeedContributor(IDataDictionaryRepository dataDictionaryRepository) => DataDictionaryRepository = dataDictionaryRepository;
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
                    Name = "土建单位",
                    Key = "ConstructionUnit",
                    IsStatic = true
                },
              new DataDictionary(Guid.NewGuid())
              {
                  Name="材料类别",
                  Key="MaterialType",
                  IsStatic=true,
                  Children=new List<DataDictionary>()
                  {
                      new DataDictionary(Guid.NewGuid())
                      {
                          Name="工程构件",
                          Key="MaterialType.Component",
                          IsStatic=true
                      },
                      new DataDictionary(Guid.NewGuid())
                      {
                          Name="主要材料",
                          Key="MaterialType.MainMaterial"
                      },
                       new DataDictionary(Guid.NewGuid())
                      {
                          Name="辅助材料",
                          Key="MaterialType.AuxiliaryMaterial"
                      }
                  }
              },
              new DataDictionary(Guid.NewGuid())
              {
                  Name="接口管理类型",
                  Key="InterfaceManagementType",
                  IsStatic=true,
                  Children=new List<DataDictionary>()
                  {
                      new DataDictionary(Guid.NewGuid())
                      {
                          Name="土建与机电",
                          Key="InterfaceManagementType.CillRltElectrical",
                          IsStatic=true
                      },
                       new DataDictionary(Guid.NewGuid())
                      {
                          Name="土建与系统",
                          Key="InterfaceManagementType.CillRltCill"
                      },
                      new DataDictionary(Guid.NewGuid())
                      {
                          Name="机电与系统",
                          Key="InterfaceManagementType.ElectricalRltSystem"
                      },
                       new DataDictionary(Guid.NewGuid())
                      {
                          Name="系统与系统",
                          Key="InterfaceManagementType.SystemRltSystem"
                      }
                      
                  }
              }
            };

        }
    }
}