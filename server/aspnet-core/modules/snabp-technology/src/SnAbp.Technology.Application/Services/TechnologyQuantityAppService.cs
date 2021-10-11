/**********************************************************************
*******命名空间： SnAbp.Technology.Services
*******类 名 称： TechnologyQuanityAppService
*******类 说 明： 工程量管理 服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 3:29:25 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.Extensions.Caching.Distributed;

using NPOI.HSSF.Util;

using SnAbp.StdBasic.Entities;
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;
using SnAbp.Technology.IServices;
using SnAbp.Utils;
using SnAbp.Utils.DataExport;
using SnAbp.Utils.TreeHelper;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SnAbp.Technology.Services
{
    /// <summary>
    ///  工程量统计
    /// </summary>
    public class TechnologyQuantityAppService : TechnologyAppService, IQuantityManagerAppService
    {
        readonly IRepository<StdBasic.Entities.ComponentCategory, Guid> _componentCategories;
        readonly IRepository<StdBasic.Entities.ProductCategory, Guid> _productCategories;
        readonly IRepository<Resource.Entities.Equipment, Guid> _equipment;
        readonly IRepository<Material, Guid> _materialRepository;
        readonly IRepository<MaterialPlan, Guid> _materialPlanRepository;
        readonly IDistributedCache<List<QueryModel>> _cacheQuery;
        readonly IDistributedCache<List<ProductCategory>> _cacheProductCategory;
        readonly IDistributedCache<List<ComponentCategory>> _cacheComponentCategory;
        readonly IDistributedCache<List<QuantitiesDto>> _cacheQuantity;
        readonly IRepository<MaterialPlanRltMaterial> _materialPlanRltMaterials;
        public TechnologyQuantityAppService(
            IRepository<ComponentCategory, Guid> componentCategories,
            IRepository<ProductCategory, Guid> productCategories,
            IRepository<Resource.Entities.Equipment, Guid> equipment,
            IRepository<Material, Guid> materialRepository,
            IRepository<MaterialPlan, Guid> materialPlanRepository,
            IDistributedCache<List<QueryModel>> cacheQuery, IDistributedCache<List<ProductCategory>> cacheProductCategory = null, IDistributedCache<List<ComponentCategory>> cacheComponentCategory = null, IDistributedCache<List<QuantitiesDto>> cacheQuantity = null, IRepository<MaterialPlanRltMaterial> materialPlanRltMaterials = null)
        {
            this._componentCategories = componentCategories;
            this._productCategories = productCategories;
            this._equipment = equipment;
            _materialRepository = materialRepository;
            _materialPlanRepository = materialPlanRepository;
            _cacheQuery = cacheQuery;
            _cacheProductCategory = cacheProductCategory;
            _cacheComponentCategory = cacheComponentCategory;
            _cacheQuantity = cacheQuantity;
            _materialPlanRltMaterials = materialPlanRltMaterials;
        }

        /// <summary>
        /// 获取专业列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SpecialityModel>> GetSpeciality()
        {
            var list = new List<SpecialityModel>();
            var data =await this.GetQuanityDatas();
            data.Item1?.ForEach(a =>
            {
                var component = data.Item2.FirstOrDefault(b => b.Code == a.ComponentCategoryCode);
                var parents = GuidKeyTreeHelper<ComponentCategory>
                .GetParents(data.Item2, component, false);
                parents.Reverse();
                if (parents!=null&&parents.Count > 0)
                {
                    list.Add(new SpecialityModel()
                    {
                        Name = parents[0].Name,
                        Id = parents[0].Id
                    });
                }
            });
            list = list.GroupBy(a => a.Id).Select(a => a.FirstOrDefault()).ToList();
            return list.AsEnumerable();
        }

        /// <summary>
        ///  获取工程量统计清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<QuantitiesDto>> GetList(QuantitiesSearchDto input)
        {
            if (input.Statistic)
            {
                await  _cacheComponentCategory.RemoveAsync("component");
                await _cacheProductCategory.RemoveAsync("product");
                await _cacheQuery.RemoveAsync("queryData");
                await _cacheQuantity.RemoveAsync("quantity");
            }
            // 通过设备查询
            var result = new PagedResultDto<QuantitiesDto>();
            // 添加缓存
            var list = await _cacheQuantity.GetOrAddAsync("quantity", 
                async () => await this.GetAllList(),() => new DistributedCacheEntryOptions(){AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)});
            var query = list
                .WhereIf(!input.KeyWords.IsNullOrEmpty(),
                a =>
               a.System1 != null && a.System1.Contains(input.KeyWords) ||
                a.System2 != null && a.System2.Contains(input.KeyWords) ||
                a.Speciality != null && a.Speciality.Contains(input.KeyWords) ||
               a.Name != null && a.Name.Contains(input.KeyWords) ||
                a.ProductCategoryName != null && a.ProductCategoryName.Contains(input.KeyWords)
                )
                .WhereIf(input.SpecId != Guid.Empty, a => a.SpecialityId == input.SpecId);
            result.Items = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            result.TotalCount = query.Count(); ;
            return result;
        }


        /// <summary>
        /// 获取所有的工程量数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuantitiesDto>> GetAllList()
        {
            var queryData = await this.GetQuanityDatas();
            var data = queryData.Item1;
            var componentCategories = queryData.Item2;
            var productCategories = queryData.Item3;
            var list = new List<QuantitiesDto>();
            data.ForEach(x =>
            {
                var targetComponent = componentCategories.FirstOrDefault(c => c.Code == x.ComponentCategoryCode); // 获取目标构建
                var targetProduct = productCategories.FirstOrDefault(a => a.Code == x.ProductCategoryCode);// 获取目标产品信息
                // 获取目标构建的所有父级
                var componentParents = GuidKeyTreeHelper<ComponentCategory>.GetParents(componentCategories, targetComponent, false);
                componentParents.Reverse();// 倒序排列
                var item = new QuantitiesDto();
                if (componentParents.Any())
                {
                    item.Speciality = componentParents[0]?.Name ?? "";
                    item.SpecialityId = componentParents[0].Id;
                    if (componentParents.Count() > 1)
                    {
                        item.System1 = componentParents[1] != null && componentParents[1] != targetComponent ? componentParents[1].Name : "";
                    }
                    if (componentParents.Count() > 2)
                    {
                        item.System2 = componentParents[2] != null && componentParents[2] != targetComponent ? componentParents[2].Name : "";
                    }
                }
                item.Id = targetComponent?.Id ?? Guid.Empty;
                item.Name = targetComponent?.Name;
                item.Spec = targetProduct?.Name;
                item.ProductCategoryId = targetProduct?.Id ?? Guid.Empty;
                item.ProductCategoryName = targetProduct?.Parent?.Name;
                item.Unit = targetProduct?.Unit;
                item.Count = (decimal)x.Count;
                list.Add(item);
            });
            return list;
        }
        /// <summary>
        /// 定义查询实体
        /// </summary>
        public  class QueryModel
        {
            public decimal Count { get; set; }
            public string ComponentCategoryCode { get; set; }
            public string ProductCategoryCode { get; set; }
        }

        /// <summary>
        ///  获取专业信息
        /// </summary>
        /// <returns></returns>
        private async  Task<(List<QueryModel>, List<ComponentCategory>, List<ProductCategory>)> GetQuanityDatas()
        {
            var list = new List<QuantitiesDto>();
            var equiments = _equipment.
              WithDetails(a => a.ComponentCategory, a => a.ProductCategory)
              .ToList();

            var data =await _cacheQuery.GetOrAddAsync("queryData",async ()=> equiments
                           .GroupBy(a => a.ComponentCategoryId)
                           .Select(x =>
                                    new QueryModel
                                    {
                                        Count = (decimal)x.Sum(y => y.Quantity),
                                        ComponentCategoryCode = x.FirstOrDefault()?.ComponentCategory?.Code,
                                        ProductCategoryCode = x.FirstOrDefault()?.ProductCategory?.Code
                                    })
                           .ToList(),()=>new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)});
            // 根据设备关联的构建和产品获取包括子父级关系的产品和构建code
            var componentCategoryCodes = data.SelectMany(a => this.CalsuleCode(a.ComponentCategoryCode)).Distinct().ToList();
            var productCategoryCodes = data.SelectMany(a => this.CalsuleCode(a.ProductCategoryCode)).Distinct().ToList();

            // 根据构建编码获取对应的构建信息
            var componentCategories =
              await  _cacheComponentCategory.GetOrAddAsync("component", async()=> _componentCategories
                 .WithDetails(x => x.Parent.Parent.Parent.Parent)
                 .Where(x => componentCategoryCodes.Contains(x.Code))
                 .ToList(), () => new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
            // 根据产品编码获取对应的产品信息
            var productCategories = await _cacheProductCategory
                .GetOrAddAsync("product",async () => _productCategories
                 .WithDetails(x => x.Parent.Parent.Parent.Parent)
                 .Where(x => productCategoryCodes.Contains(x.Code))
                 .ToList(), () => new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
            return (data, componentCategories, productCategories);
        }


        /// <summary>
        /// 计算编号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<string> CalsuleCode(string code)
        {
            var codes = new List<string>();
            var arr = code?.Split(" ").ToList();
            if (code !=null && arr.Count() > 1)
            {
                var index = 1;
                arr.ForEach((str) => codes.Add(arr.Take(++index).JoinAsString(" ")));
            }
            return codes;
        }

        /// <summary>
        /// 工程量导出
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Stream> Export(List<Guid> ids)
        {
            var list = await _cacheQuantity.GetOrAddAsync("quantity",
               async () => await this.GetAllList(), () => new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
            list = list.Where(a => ids.Contains(a.Id)).ToList();
            var handler = DataExportHandler
                              .CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
            handler.CreateSheet("物资材料信息");
            var rowIndex = 0;
            var headRow = handler.CreateRow(rowIndex);
            var cellStyle = handler.CreateCellStyle
                (CellBorder.CreateBorder(NPOI.SS.UserModel.BorderStyle.Thin, lineColor: HSSFColor.Black.Index));
            var title = new string[]
                                 {
                    "序号",
                    "专业名称",
                    "系统1",
                    "系统2",
                    "名称",
                    "规格/型号",
                    "工程量",
                    "计量单位",
            };
            for (int i = 0; i < title.Length; i++)
            {
                headRow.CreateCell(i)
                    .SetCellStyle(cellStyle)
                    .SetCellValue(title[i]);
            }

            list?.ForEach(a =>
            {
                var row = handler.CreateRow(++rowIndex);
                row.CreateCell(0).SetCellStyle(cellStyle).SetCellValue(rowIndex);
                row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue(a.Speciality);
                row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue(a.System1);
                row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue(a.System2);
                row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue(a.Name);
                row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue(a.Spec);
                row.CreateCell(6).SetCellStyle(cellStyle).SetCellValue(a.Count.ToString());
                row.CreateCell(7).SetCellStyle(cellStyle).SetCellValue(a.Unit);
            });
            Stream stream = handler.GetExcelDataBuffer().BytesToStream();
            return stream;
        }

        /// <summary>
        /// 通过工程量生成用料计划
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> CreateMaterialPlan(MaterialPlanCreateDto input)
        {
            if (input.PlanName.IsNullOrEmpty()) throw new UserFriendlyException("计划名称不能为空");
            if (input.PlanDate == null) input.PlanDate = DateTime.Now;
            var materialPlan = new MaterialPlan(GuidGenerator.Create());
            await CheckSameName(input.PlanName, null);
            materialPlan.PlanName = input.PlanName;
            materialPlan.PlanTime = input.PlanDate;
            materialPlan.Submit = false;
            materialPlan.Status = enums.ApprovalStatus.ToSubmit;
            // 判断工程量数据是否在材料表中
            await _materialPlanRepository.InsertAsync(materialPlan,true);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            // 根据工程量生成用料计划
            var list = await _cacheQuantity.GetOrAddAsync("quantity",
               async () => await this.GetAllList(), () => new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTimeOffset.Now.AddHours(1) });
            var quantityIds = input.Materials.Select(a => a.Id);
            list = list.Where(a => quantityIds.Contains(a.Id)).ToList();
            // 根据产品分类进行分组
            var products = list.GroupBy(a => a.ProductCategoryId)
                .Select(a => new
                {
                    ProductCategoryId = a.Key,
                    Count = a.Sum(b => b.Count)
                }).ToList();

            // 根据物资表中产品id对比，生成用料计划
            foreach (var item in products)
            {
                var material = _materialRepository.FirstOrDefault(a => a.ProductCategoryId == item.ProductCategoryId);
                if (material != null)
                {
                    var rlt = new MaterialPlanRltMaterial(materialPlan.Id, material.Id) { Count = item.Count };
                    await _materialPlanRltMaterials.InsertAsync(rlt, true);
                }
            }
            return true;
        }
     
        
        #region 私有方法
        private async Task<bool> CheckSameName(string name, Guid? id)
        {
            return await Task.Run(() =>
            {
                var sameNames = _materialPlanRepository.WhereIf(id != Guid.Empty && id != null, x => x.Id != id)
                .FirstOrDefault(a => a.PlanName == name);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前计划名称已存在！");
                }

                return true;
            });
        }
        #endregion
    }
    /// <summary>
    /// 定义专业实体用于前端调用
    /// </summary>
    public class SpecialityModel
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }

}
