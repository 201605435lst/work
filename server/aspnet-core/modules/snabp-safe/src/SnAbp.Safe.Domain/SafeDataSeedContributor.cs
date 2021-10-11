/**********************************************************************
*******命名空间： SnAbp.Safe
*******类 名 称： ISafeDataSeedContributor
*******类 说 明： 安全管理种子数据管理
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/11 18:12:07
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace SnAbp.Safe
{
    /// <summary>
    /// $$
    /// </summary>
    public class SafeDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public SafeDataSeedContributor(IDataDictionaryRepository dataDictionaryRepository) => DataDictionaryRepository = dataDictionaryRepository;
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
                  new DataDictionary(safeId){
                    Name = "安全问题",
                    Key = "SafeManager",
                    IsStatic = true,
                    Children= new List<DataDictionary>(){
                          new DataDictionary(Guid.NewGuid()){Name = "事件类型",Key = "SafeManager.EventType",IsStatic = true,ParentId=safeId},
                          new DataDictionary(Guid.NewGuid()){Name = "适用范围",Key = "SafeManager.Scop",IsStatic = true,ParentId=safeId},
                    }
                },

                 new DataDictionary(Guid.NewGuid())
                {
                    Name = "安全问题类型",
                    Key = "SafeProblemType",
                    IsStatic = true
                },
            };
        }
    }
}
