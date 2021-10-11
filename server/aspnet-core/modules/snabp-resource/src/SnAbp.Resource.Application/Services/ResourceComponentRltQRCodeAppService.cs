using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SnAbp.Common.IServices;
using SnAbp.Identity;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

/************************************************************************************
*命名空间：SnAbp.Resource.Services
*文件名：ResourceComponentRltQRCodeAppService
*创建人： liushengtao
*创建时间：2021/6/24 10:39:12
*描述：导出二维码表
*
***********************************************************************/
namespace SnAbp.Resource.Services
{
    public class ResourceComponentRltQRCodeAppService : ResourceAppService, IResourceComponentRltQRCodeAppService
    {
        private readonly IRepository<Equipment, Guid> _repositoryEquipment;
        private readonly IRepository<ComponentRltQRCode, Guid> _ComponentRltQRCodeRepository;
        private readonly IRepository<ComponentCategory, Guid> _ComponentCategoryRepository;//标准库构件
        private readonly IRepository<Organization, Guid> _orgRepository;
        private readonly IRepository<ComponentTrackRecord, Guid> _ComponentTrackRecordRepository;
        public ICommonQRCodeAppService _ICommonQRCode { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Equipment, Guid> _EquipmentRepository;
        public ResourceComponentRltQRCodeAppService(
            IRepository<Organization, Guid> orgRepository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<Equipment, Guid> repositoryEquipment,
            IRepository<ComponentTrackRecord, Guid> ComponentTrackRecordRepository,
            ICommonQRCodeAppService ICommonQRCode,
            IRepository<ComponentCategory, Guid> ComponentCategoryRepository,
             IRepository<ComponentRltQRCode, Guid> ComponentRltQRCodeRepository,
             IRepository<Equipment, Guid> EquipmentRepository,
             IGuidGenerator guidGenerator
            )
        {
            _orgRepository = orgRepository;
            _httpContextAccessor = httpContextAccessor;
            _repositoryEquipment = repositoryEquipment;
            _ComponentTrackRecordRepository = ComponentTrackRecordRepository;
            _ComponentCategoryRepository = ComponentCategoryRepository;
            _ICommonQRCode = ICommonQRCode;
            _EquipmentRepository = EquipmentRepository;
            _guidGenerator = guidGenerator;
            _ComponentRltQRCodeRepository = ComponentRltQRCodeRepository;
        }
        /// <summary>
        /// 导出二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> GenerateCode(ComponentRltQRCodeCreateDto input)
        {
            var equipment = await GetEquipmentInfo(input);
            equipment = equipment.WhereIf(input.Ids == null || input.Ids.Count == 0, x => x.ComponentRltQRCodes == null || x.ComponentRltQRCodes.Count == 0).ToList();
            foreach (var item in equipment)
            {
                var ComponentRltQRCodeId = _guidGenerator.Create();
                /*1.判断该设备之前是否生成过二维码*/
                var iscomponentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.GenerateEquipmentId == item.Id && x.State == ActivatedState.Activated).FirstOrDefault();
                /*2.如果有。则将他设置成未激活*/
                if (iscomponentRltQRCode != null)
                {
                    iscomponentRltQRCode.State = ActivatedState.NoActivated;
                    await _ComponentRltQRCodeRepository.UpdateAsync(iscomponentRltQRCode);
                }

                /*3.如果没有。则将他设置成激活状态*/
                var componentRltQRCode = new ComponentRltQRCode(ComponentRltQRCodeId)
                {
                    GenerateEquipmentId = item.Id,
                    QRCode = item.ComponentCategory.Code + "@" + ComponentRltQRCodeId,
                    State = ActivatedState.Activated,
                };
                await _ComponentRltQRCodeRepository.InsertAsync(componentRltQRCode);
            }
            return true;
        }

        /// <summary>
        /// 导出二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<QRcodeDto>> ExportCode(ComponentRltQRCodeCreateDto input)
        {
            var equipment = await GetEquipmentInfo(input);
            equipment = equipment.Where(x => x.ComponentRltQRCodes != null && x.ComponentRltQRCodes.Count > 0).ToList();
            var base64s = new List<QRcodeDto>();
            if (equipment == null || equipment.Count == 0) throw new UserFriendlyException("当前不存在设备二维码，请先生成二维码");
            foreach (var item in equipment)
            {
                var base64 = new QRcodeDto();
                var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.GenerateEquipmentId == item.Id && x.State == ActivatedState.Activated).FirstOrDefault();
                var obj = new QrCodeExportDto()
                {
                    key = "equipment",
                    value = componentRltQRCode.QRCode
                };
                var res = JsonConvert.SerializeObject(obj);
                var str = await _ICommonQRCode.GetQRCode(res);
                base64.QrCode = str;
                base64.Name = item.Name;
                base64s.Add(base64);
            }
            return base64s;
        }
        /// <summary>
        /// 获取二维码信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ComponentRltQRCodeDto>> GetList(ComponentRltQRCodeSearchDto input)
        {
            var componentCategoryIds = new List<Guid>();
            if (input.ComponentCategoryId != null || input.ComponentCategoryId != Guid.Empty)
            {
                var componentCategory = _ComponentCategoryRepository.FirstOrDefault(x => x.Id == input.ComponentCategoryId);
                if (componentCategory != null)
                    componentCategoryIds = _ComponentCategoryRepository.Where(x => x.Code.StartsWith(componentCategory.Code)).Select(s => s.Id).ToList();
            }
            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails()
                 .WhereIf(componentCategoryIds.Count > 0, x => componentCategoryIds.Contains(x.GenerateEquipment.ComponentCategory.Id))
                 .Where(x => x.State == ActivatedState.Activated)
                 .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                    x.GenerateEquipment.Name.Contains(input.Keyword) ||
                    x.GenerateEquipment.Manufacturer.Name.Contains(input.Keyword) ||
                    x.GenerateEquipment.ProductCategory.Name.Contains(input.Keyword) ||
                    x.GenerateEquipment.Code.Contains(input.Keyword));
            var result = new PagedResultDto<ComponentRltQRCodeDto>()
            {
                TotalCount = componentRltQRCode.Count(),
                Items = ObjectMapper.Map<List<ComponentRltQRCode>, List<ComponentRltQRCodeDto>>(componentRltQRCode.Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.GenerateEquipment.Name).ToList())
            };
            return Task.FromResult(result);
        }
        ///// <summary>
        ///// 获取构件二维码跟踪记录信息
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        public Task<ComponentRltQRCodeDto> Get(Guid Id)
        {
            if (Id == null || Id == Guid.Empty) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");

            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.Id == Id).FirstOrDefault();
            if (componentRltQRCode == null) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");
            var componentRltQRCodeDto = ObjectMapper.Map<ComponentRltQRCode, ComponentRltQRCodeDto>(componentRltQRCode);
            var componentTrackRecord = _ComponentTrackRecordRepository.WithDetails().Where(x => x.Id == Id).OrderBy(x => x.Time).ToList();


            if (componentRltQRCodeDto.ComponentTrackRecord != null)
            {

                componentRltQRCodeDto.ComponentTrackRecord = componentTrackRecord;
            }
            return Task.FromResult(componentRltQRCodeDto);
        }

        /// <summary>
        /// 二维码查看
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetView(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");

            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.GenerateEquipmentId == id && x.State == ActivatedState.Activated).FirstOrDefault();
            if (componentRltQRCode == null) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");
            var obj = new QrCodeExportDto()
            {
                key = "equipment",
                value = componentRltQRCode.QRCode
            };
            var res = JsonConvert.SerializeObject(obj);
            string base64Code = await _ICommonQRCode.GetQRCode(res);
            return base64Code;
        }

        /// <summary>
        /// 二维码查看
        /// </summary>
        /// <param name="installationEquipmentId"></param>
        /// <returns></returns>
        public async Task<string> GetByInstallationEquipmentId(Guid installationEquipmentId)
        {
            if (installationEquipmentId == null || installationEquipmentId == Guid.Empty) throw new UserFriendlyException("数据错误，请刷新页面重新尝试");

            var componentRltQRCode = _ComponentRltQRCodeRepository.WithDetails().Where(x => x.InstallationEquipmentId == installationEquipmentId && x.State == ActivatedState.Activated).FirstOrDefault();
            if (componentRltQRCode == null)
            {
                return null;
            }
            return componentRltQRCode.QRCode;
        }



        #region 私有方法
        /// <summary>
        /// 获取设备id
        /// </summary>
        /// <returns></returns>
        public async Task<List<Equipment>> GetEquipmentInfo(ComponentRltQRCodeCreateDto input)
        {
            return await Task.Run(() =>
            {
                var equipments = new List<Equipment>();

                //当传过来数组的时候
                if (input.Ids != null && input.Ids.Count > 0)
                {
                    equipments = _EquipmentRepository.WithDetails(x => x.ComponentCategory, y => y.ComponentRltQRCodes).Where(x => input.Ids.Contains(x.Id)).ToList();
                }
                /*当是条件的时候*/
                else
                {
                    var query = _EquipmentRepository.WithDetails().ToList();
                    //获取当前登录用户的组织机构
                    var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                    var organization = !string.IsNullOrEmpty(organizationIdString) ? _orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                    var organizationCode = organization != null ? organization.Code : null;

                    var allOrgs = new List<Guid>();
                    if (!string.IsNullOrEmpty(organizationCode))
                    {
                        allOrgs = _orgRepository.Where(x => x.Code.StartsWith(organizationCode)).Select(s => s.Id).ToList();
                    }
                    var componentCategoryIds = new List<Guid>();
                    if (input.ComponentCategoryId != null || input.ComponentCategoryId != Guid.Empty)
                    {
                        var componentCategory = _ComponentCategoryRepository.FirstOrDefault(x => x.Id == input.ComponentCategoryId);
                        if (componentCategory != null)
                            componentCategoryIds = _ComponentCategoryRepository.Where(x => x.Code.StartsWith(componentCategory.Code)).Select(s => s.Id).ToList();
                    }

                    equipments = query
                       .WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode), x => x.EquipmentRltOrganizations.Any(s => allOrgs.Contains(s.OrganizationId)) || x.EquipmentRltOrganizations.Count == 0)
                       .WhereIf(string.IsNullOrEmpty(input.Keyword) &&
                            (input.InstallationSiteId == null || input.InstallationSiteId == Guid.Empty) &&
                            (input.OrganizationIds == null || input.OrganizationIds.Count == 0) &&
                            (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty), x => x.ParentId == input.ParentId)
                   .WhereIf(input.InstallationSiteId != null && input.InstallationSiteId != Guid.Empty, x => x.InstallationSiteId == input.InstallationSiteId)
                   .WhereIf(input.OrganizationIds != null && input.OrganizationIds.Any(), x => x.EquipmentRltOrganizations.Any(y => input.OrganizationIds.Contains(y.OrganizationId)))
                   .WhereIf(componentCategoryIds.Count > 0, x => componentCategoryIds.Contains(x.ComponentCategory.Id))
                   .WhereIf(!string.IsNullOrEmpty(input.Keyword), x =>
                        x.Name.Contains(input.Keyword) ||
                        x.Manufacturer.Name.Contains(input.Keyword) ||
                        x.ProductCategory.Name.Contains(input.Keyword) ||
                        x.Code.Contains(input.Keyword)).ToList();
                }
                return equipments;
            });

        }
        #endregion

    }
}
