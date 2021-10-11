using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Identity;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceStoreEquipmentAppService : ResourceAppService, IResourceStoreEquipmentAppService
    {
        private readonly IRepository<StoreEquipment, Guid> _storeEquipmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;

        private readonly IRepository<StoreEquipmentTransferRltEquipment, Guid> _storeEquipmentTransferRltEquipmentRepository;
        private readonly IRepository<StoreEquipmentTransfer, Guid> _storeEquipmentTransferRepository;
        private readonly IRepository<StoreEquipmentTestRltEquipment, Guid> _storeEquipmentTestRltEquipmentRepository;
        private readonly IRepository<StoreEquipmentTest, Guid> _storeEquipmentTestRepository;

        private readonly IRepository<Manufacturer, Guid> _manufacturerRepository;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private readonly IRepository<Organization, Guid> orgRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResourceStoreEquipmentAppService(IRepository<StoreEquipment, Guid> storeEquipmentRepository, IGuidGenerator guidGenerator,
            IRepository<Manufacturer, Guid> manufacturerRepository, IRepository<ProductCategory, Guid> productCategoryRepository,

            IRepository<StoreEquipmentTransferRltEquipment, Guid> storeEquipmentTransferRltEquipmentRepository,
            IRepository<StoreEquipmentTransfer, Guid> storeEquipmentTransferRepository,
            IRepository<StoreEquipmentTestRltEquipment, Guid> storeEquipmentTestRltEquipmentRepository,
            IRepository<StoreEquipmentTest, Guid> storeEquipmentTestRepository,

            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Organization, Guid> orgRep,
            IRepository<IdentityUser, Guid> identityUserRepository
            )
        {
            _storeEquipmentTransferRltEquipmentRepository = storeEquipmentTransferRltEquipmentRepository;
            _storeEquipmentTransferRepository = storeEquipmentTransferRepository;
            _storeEquipmentTestRltEquipmentRepository = storeEquipmentTestRltEquipmentRepository;
            _storeEquipmentTestRepository = storeEquipmentTestRepository;

            _identityUserRepository = identityUserRepository;
            _storeEquipmentRepository = storeEquipmentRepository;
            _guidGenerator = guidGenerator;
            _productCategoryRepository = productCategoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _httpContextAccessor = httpContextAccessor;
            orgRepository = orgRep;
            _componentCategoryRepository = componentCategoryRepository;
        }

        /// <summary>
        /// 批量添加库存设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreEquipment.Create)]
        public async Task<List<StoreEquipmentDto>> Create(List<StoreEquipmentCreateDto> input)
        {
            if (input.Count == 0) throw new UserFriendlyException("请传入要创建的库存设备");
            var list = new List<StoreEquipment>();
            var componentCategoryRepository = _componentCategoryRepository.FirstOrDefault();
            var count = _storeEquipmentRepository.Count();
            //获取当前登录人的信息
            var currentUser = await _identityUserRepository.GetAsync(CurrentUser.Id.GetValueOrDefault());

            foreach (var item in input)
            {
                count++;
                var storeEquipmentProductCategoryName = _productCategoryRepository.FirstOrDefault(x => x.Id == item.ProductCategoryId);
                var storeEquipmentManufacturerName = _manufacturerRepository.FirstOrDefault(x => x.Id == item.ManufacturerId);
                var storeEquipmentCode = SaveCode(count);
                var storeEquipment = new StoreEquipment(_guidGenerator.Create());
                storeEquipment.Code = storeEquipmentCode;
                storeEquipment.Name = storeEquipmentProductCategoryName?.Name + "-" + count.ToString();
                storeEquipment.ComponentCategoryId = componentCategoryRepository.Id;
                storeEquipment.ProductCategoryId = item.ProductCategoryId;
                storeEquipment.ManufacturerId = item.ManufacturerId;
                storeEquipment.ManufactureDate = item.ManufactureDate;
                storeEquipment.State = StoreEquipmentState.UnActived;
                if (currentUser != null)
                {
                    storeEquipment.CreatorId = currentUser.Id;
                }
                SaveCheckSameCode(storeEquipment.Code);
                ////saveCheckSameName(storeEquipment.Name, storeEquipment.Id);
                await _storeEquipmentRepository.InsertAsync(storeEquipment);
                list.Add(storeEquipment);

            }

            return ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentDto>>(list);
        }
        /// <summary>
        /// 将数据导出到excel表格中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Produces("application/octet-stream")]
        public async Task<Stream> ExportStoreEquipments(List<StoreEquipmentDto> input)
        {
            //var storeEquipments = _storeEquipmentRepository
            //    .WithDetails()
            //    .WhereIf(ids.Count() > 0, x => ids.Contains(x.Id));
            if (input.Any()) throw new UserFriendlyException("请传入要导出的库存设备数据");
            var dt = (DataTable)null;
            var col = (DataColumn)null;
            var row = (DataRow)null;
            Stream st = null;
            //string[] s = { "编码", "名称", "构件分类Id", "构件分类名称", "产品分类Id", "产品分类名称", "厂家分类Id", "厂家分类名称"};
            for (var i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            col = new DataColumn("编码");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 1:
                        {
                            col = new DataColumn("名称");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 2:
                        {
                            col = new DataColumn("构件分类Id");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 3:
                        {
                            col = new DataColumn("构件分类名称");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 4:
                        {
                            col = new DataColumn("产品分类Id");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 5:
                        {
                            col = new DataColumn("产品分类名称");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 6:
                        {
                            col = new DataColumn("厂家分类Id");
                            dt.Columns.Add(col);
                            break;
                        }
                    case 7:
                        {
                            col = new DataColumn("厂家分类名称");
                            dt.Columns.Add(col);
                            break;
                        }
                    default:
                        {
                            throw new UserFriendlyException("导入失败");
                        }
                }
            }
            foreach (var item in input)
            {
                row = dt.NewRow();
                row["编码"] = item.Code;
                row["名称"] = item.Name;
                row["构件分类Id"] = item.ComponentCategoryId;
                row["构件分类名称"] = item.ComponentCategory.Name;
                row["产品分类Id"] = item.ProductCategoryId;
                row["产品分类名称"] = item.ProductCategory.Name;
                row["厂家分类Id"] = item.ManufacturerId;
                row["厂家分类名称"] = item.Manufacturer.Name;
                dt.Rows.Add(row);
            }

            var sbuf = ExcelHelper.DataTableToExcel(dt, "库存设备.xlsx", new List<int>() { 0 });
            st = new MemoryStream(sbuf);
            return st;
        }

        /// <summary>
        /// 根据条件获取库存设备集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreEquipmentDto>> GetList(StoreEquipmentSearchDto input)
        {
            //获取当前登录用户的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationCode = organization != null ? organization.Code : null;
            // 1、获取设备数据
            var storeEquipmentResult = _storeEquipmentRepository.WithDetails()
                .WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode), x => x.StoreHouse.Organization.Code.StartsWith(organizationCode))
                .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.StoreHouse.OrganizationId == input.OrganizationId)
                .WhereIf(!string.IsNullOrEmpty(input.Code), x => x.Code.Contains(input.Code))
                .WhereIf(input.ProductCategoryId != Guid.Empty, x => x.ProductCategoryId == input.ProductCategoryId)
                .WhereIf(input.StoreHouseId != null && input.StoreHouseId != Guid.Empty, x => x.StoreHouseId == input.StoreHouseId)
                .WhereIf(input.State.IsIn(StoreEquipmentState.OnService, StoreEquipmentState.Scrap, StoreEquipmentState.Spare, StoreEquipmentState.UnActived, StoreEquipmentState.WaitForTest), x => x.State == input.State);
            var storeEquipments = ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentDto>>(storeEquipmentResult.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            foreach (var item in storeEquipments)
            {
                // 1、1 获取设备数据的id;
                var storeEquipmentsId = item.Id;
                // 2、获取关联数据信息
                //2、1 获取关联表StoreEquipmentTransferRltEquipments的数据StoreEquipmentTransferId
                var storeEquipmentTransferIds = _storeEquipmentTransferRltEquipmentRepository.Where(x => storeEquipmentsId == x.StoreEquipmentId).Select(x => x.StoreEquipmentTransferId);
                //2.1.1 获取StoreEquipmentTransfer信息
                var storeEquipmentTransfer = _storeEquipmentTransferRepository.WithDetails(x => x.User).Where(x => storeEquipmentTransferIds.Contains(x.Id)).OrderByDescending(x => x.CreationTime).ToList();
                if (storeEquipmentTransfer.Count > 0)
                {
                    item.StoreEquipmentTransfer = storeEquipmentTransfer;
                }
                //2、2 获取关联表StoreEquipmentTestRltEquipments的数据StoreEquipmentTestId
                var storeEquipmentTestIds = _storeEquipmentTestRltEquipmentRepository.Where(x => storeEquipmentsId == x.StoreEquipmentId).Select(x => x.StoreEquipmentTestId);

                //2.2.1 获取StoreEquipmentTest信息Tester
                var storeEquipmentTest = _storeEquipmentTestRepository.WithDetails(x => x.Tester).Where(x => storeEquipmentTestIds.Contains(x.Id)).ToList();

                if (storeEquipmentTest.Count > 0)
                {
                    item.StoreEquipmentTest = storeEquipmentTest;
                }
            }
            var result = new PagedResultDto<StoreEquipmentDto>();
            result.TotalCount = storeEquipmentResult.Count();
            result.Items = storeEquipments;

            return Task.FromResult(result);
        }

        /// <summary>
        /// 通过code集合来获取对应的库存设备信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreEquipmentDto>> GetListByCode(StoreEquipmentSearchSimpleDto input)
        {
            var storeEquipments = _storeEquipmentRepository.WithDetails().WhereIf(input.Codes.Count() > 0, x => input.Codes.Contains(x.Code));
            var result = new PagedResultDto<StoreEquipmentDto>();
            result.TotalCount = storeEquipments.Count();
            result.Items = ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentDto>>(storeEquipments.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            return Task.FromResult(result);
        }

        /// <summary>
        /// 通过ids数组来获取相应的库存设备信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreEquipmentDto>> GetListByIds(List<Guid> ids)
        {
            var storeEquipments = _storeEquipmentRepository
                .WithDetails()
                .WhereIf(ids.Any(), x => ids.Contains((Guid)x.Id));
            var result = new PagedResultDto<StoreEquipmentDto>();
            result.TotalCount = storeEquipments.Count();
            result.Items = ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentDto>>(storeEquipments.OrderBy(x => x.Code).ToList());

            return Task.FromResult(result);
        }

        public Task<StoreEquipmentDto> GetStoreEquipmentRecords(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入你要查询的库存设备的id");
            var storeEquipmentResult = _storeEquipmentRepository
                .WithDetails()
                .FirstOrDefault(x => x.Id == id);
            if (storeEquipmentResult == null) throw new UserFriendlyException("不存在此id的库存设备");
            var storeEquipment = ObjectMapper.Map<StoreEquipment, StoreEquipmentDto>(storeEquipmentResult);
            if (storeEquipment != null)
            {
                // 1、1 获取设备数据的id;
                var storeEquipmentsId = storeEquipment.Id;
                // 2、获取关联数据信息
                //2、1 获取关联表StoreEquipmentTransferRltEquipments的数据StoreEquipmentTransferId
                var storeEquipmentTransferIds = _storeEquipmentTransferRltEquipmentRepository.Where(x => storeEquipmentsId == x.StoreEquipmentId).Select(x => x.StoreEquipmentTransferId);
                //2.1.1 获取StoreEquipmentTransfer信息
                var storeEquipmentTransfer = _storeEquipmentTransferRepository.WithDetails().Where(x => storeEquipmentTransferIds.Contains(x.Id)).OrderByDescending(x => x.CreationTime).ToList();
                if (storeEquipmentTransfer.Count > 0)
                {
                    storeEquipment.StoreEquipmentTransfer = storeEquipmentTransfer;
                }
                //2、2 获取关联表StoreEquipmentTestRltEquipments的数据StoreEquipmentTestId
                var storeEquipmentTestIds = _storeEquipmentTestRltEquipmentRepository.Where(x => storeEquipmentsId == x.StoreEquipmentId).Select(x => x.StoreEquipmentTestId);

                //2.2.1 获取StoreEquipmentTest信息Tester
                var storeEquipmentTest = _storeEquipmentTestRepository.WithDetails(x => x.Tester).Where(x => storeEquipmentTestIds.Contains(x.Id)).ToList();

                if (storeEquipmentTest.Count > 0)
                {
                    storeEquipment.StoreEquipmentTest = storeEquipmentTest;
                }
            }
            return Task.FromResult(storeEquipment);
        }


        // 检查是否有相同编号的库存设备
        private bool SaveCheckSameCode(string code)
        {
            var sameStoreEquipments = _storeEquipmentRepository.Where(o => o.Code.ToUpper() == code.ToUpper());
            if (sameStoreEquipments.Any())
            {
                throw new UserFriendlyException("当前库存中存在相同编号的库存设备");
            }
            return true;
        }

        private string SaveCode(decimal code)
        {

            return code.ToString(CultureInfo.InvariantCulture).PadLeft(9, '0');
        }

    }
}
