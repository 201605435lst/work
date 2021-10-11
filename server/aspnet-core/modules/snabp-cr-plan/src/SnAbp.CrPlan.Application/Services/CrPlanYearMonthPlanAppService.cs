using Microsoft.AspNetCore.Mvc;
using SnAbp.CrPlan.Commons;
using SnAbp.CrPlan.Dto;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.Enumer;
using SnAbp.CrPlan.IServices;
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SnAbp.CrPlan.Repositories;
using Newtonsoft.Json.Linq;
using SnAbp.Bpm.Services;
using SnAbp.StdBasic.Entities;
using System.IO;
using SnAbp.Identity;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Enums;
using SnAbp.Utils.EnumHelper;
using SnAbp.CrPlan.Dtos.Import;
using SnAbp.Common;
using SnAbp.Bpm;
using SnAbp.File.Services;
using Volo.Abp.Uow;
using SnAbp.StdBasic.IServices;
using SnAbp.CrPlan.Dtos;
using Volo.Abp.Guids;

namespace SnAbp.CrPlan.Services
{
    /// <summary>
    /// 年月表计划
    /// </summary>
    //[Authorize]
    public class CrPlanYearMonthPlanAppService : CrPlanAppService, ICrPlanYearMonthPlanService
    {
        private readonly IRepository<YearMonthPlan, Guid> _service;//年月表计划
        private readonly IRepository<DailyPlan, Guid> _dailyPlanService;//日计划
        private readonly IRepository<YearMonthPlanTestItem, Guid> _testService;//年月表测试项
        private readonly IRepository<YearMonthPlanAlter, Guid> _alterPlanService;//年月表变更记录
        private readonly IRepository<RepairItem, Guid> _repariService;//维修项管理
        private readonly IRepository<RepairItemRltComponentCategory, Guid> _reparIFDService;  //维修项关联ifd
        private readonly IRepository<RepairItemRltOrganizationType, Guid> _reparRltTypeService;  //维修项执行单位
        private readonly IRepository<RepairTestItem, Guid> _repairTestService;//维修测试项
        private readonly OrganizationManager _orgService;//组织机构
        private readonly IRepository<Equipment, Guid> _equipmentService;  //设备
        private readonly IRepository<EquipmentRltOrganization, Guid> _equipmentOrgService;  //设备关联组织机构
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;  //产品
        private readonly ICrPlanDaliyPlanRepository _daliyPlanRepos;//日计划(可以操作dbcontxt)
        private readonly BpmManager _bpmManager;
        private readonly IFileImportHandler _fileImport;
        private readonly CrPlanManager _crPlanManager;
        private readonly FileManager _fileManager;
        private readonly IStdBasicRepairTestItemAppService _basicRepairTestItemAppService;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaries;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<SkylightPlan, Guid> _skylightPlanService;
        private readonly IRepository<PlanDetail, Guid> _planDetailsService;
        private readonly IRepository<MaintenanceWorkRltSkylightPlan, Guid> _maintenanceWorksRltService;
        private readonly IRepository<WorkOrder, Guid> _workOrderService;
        private readonly IRepository<WorkOrganization, Guid> _workOrganization;
        private readonly AppOrganizationAppService _organizationAppService;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<YearMonthAlterRecord, Guid> _yearMonthAlterRecord;
        private readonly IRepository<PlanRelateEquipment, Guid> _planRelateEquipmentRepository;  //计划内容关联设备
        private readonly IRepository<Organization, Guid> _organizationRepository;
        //private readonly IDbContextProvider<> _dbContext;
        public CrPlanYearMonthPlanAppService(
            IRepository<YearMonthPlan, Guid> service,
            IRepository<DailyPlan, Guid> dailyPlanService,
            IRepository<YearMonthPlanTestItem, Guid> testservice,
            IRepository<YearMonthPlanAlter, Guid> alterPlanService,
            IRepository<RepairItem, Guid> repariService,
            IRepository<RepairItemRltComponentCategory, Guid> reparIFDService,
            IRepository<RepairItemRltOrganizationType, Guid> reparRltTypeService,
            IRepository<RepairTestItem, Guid> repairTestService,
            OrganizationManager orgService,
            IRepository<Equipment, Guid> equipmentService,
            IRepository<EquipmentRltOrganization, Guid> equipmentOrgService,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            ICrPlanDaliyPlanRepository daliyPlanRepos,
            BpmManager bpmManager,
            CrPlanManager crPlanManager,
            IFileImportHandler fileImport,
            FileManager fileManager,
            IStdBasicRepairTestItemAppService basicRepairTestItemAppService,
            IRepository<DataDictionary, Guid> dataDictionaries,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<SkylightPlan, Guid> skylightPlanService,
            IRepository<PlanDetail, Guid> planDetailsService,
            IRepository<MaintenanceWorkRltSkylightPlan, Guid> maintenanceWorksRltService,
            IRepository<WorkOrder, Guid> workOrdersService,
            IRepository<WorkOrganization, Guid> workOrganization,
            AppOrganizationAppService organizationAppService,
            IGuidGenerator guidGenerator,
            IRepository<YearMonthAlterRecord, Guid> yearMonthAlterRecord,
            IRepository<PlanRelateEquipment, Guid> planRelateEquipmentRepository,
             IRepository<Organization, Guid> organizationRepository
            )
        {
            _service = service;
            _dailyPlanService = dailyPlanService;
            _testService = testservice;
            _alterPlanService = alterPlanService;
            _repariService = repariService;
            _reparIFDService = reparIFDService;
            _reparRltTypeService = reparRltTypeService;
            _repairTestService = repairTestService;
            _orgService = orgService;
            _equipmentService = equipmentService;
            _equipmentOrgService = equipmentOrgService;
            _componentCategoryRepository = componentCategoryRepository;
            _daliyPlanRepos = daliyPlanRepos;
            _bpmManager = bpmManager;
            _crPlanManager = crPlanManager;
            _fileImport = fileImport;
            _fileManager = fileManager;
            _basicRepairTestItemAppService = basicRepairTestItemAppService;
            _dataDictionaries = dataDictionaries;
            _unitOfWorkManager = unitOfWorkManager;
            _skylightPlanService = skylightPlanService;
            _planDetailsService = planDetailsService;
            _maintenanceWorksRltService = maintenanceWorksRltService;
            _workOrderService = workOrdersService;
            _workOrganization = workOrganization;
            _organizationAppService = organizationAppService;
            _guidGenerator = guidGenerator;
            _yearMonthAlterRecord = yearMonthAlterRecord;
            _planRelateEquipmentRepository = planRelateEquipmentRepository;
            _organizationRepository = organizationRepository;
        }

        #region 获得天窗类型列表=======================================
        /// <summary>
        /// 获得天窗类型列表
        /// </summary>
        public List<string> GetSkyligetTypeList()
        {
            var list = new List<string>();
            var values = Enum.GetValues(typeof(Enumer.SkyligetType));
            if (values.Length > 0)
            {
                foreach (int item in values)
                {
                    list.Add(Enum.GetName(typeof(Enumer.SkyligetType), item));
                }
            }
            return list;
        }
        #endregion

        #region 获取列表数据===========================================
        /// <summary>
        /// 获得年计划列表
        /// </summary>
        public async Task<dynamic> GetListYear(YearMonthSearchDto input)
        {
            var totalCount = (int)0;
            var list = (List<YearMonthPlanYearDetailDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanYearDetailDto>(
                    totalCount,
                    list = new List<YearMonthPlanYearDetailDto>()
                );
            }
            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();
            await Task.Run(() =>
            {
                list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanYearDetailDto>>(_service.Where(z =>
                z.RepairTagId == RepairTagId &&
                z.Year == input.Year &&
                (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                z.PlanType == input.PlanType &&
                (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                ).OrderBy(z => z.Number.Replace("-", "")).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                totalCount = _service.Count(z =>
                z.RepairTagId == RepairTagId &&
                  z.Year == input.Year &&
                  (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                  z.PlanType == input.PlanType &&
                  (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                  (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                );

                var repairIds = list.Select(s => s.RepairDetailsId);
                var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();
                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                    {
                        if (t.OrganizationType != null)
                            executableUnitStr += t.OrganizationType.Name + ",";
                    }
                    item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                }
            });
            var isCanCreate = true;
            if (totalCount == 0)
            {
                if (input.PlanType == (int)Enumer.YearMonthPlanType.年度月表)
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z =>
                    z.RepairTagId == RepairTagId &&
                    z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year && z.Month == input.Month));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }
                else
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z =>
                    z.RepairTagId == RepairTagId &&
                    z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }

            }

            return new
            {
                isCanCreate,
                totalCount,
                items = list
            };
        }

        /// <summary>
        /// 获得已提交的数量大于0的年计划列表
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetSubmitedListYear(YearMonthSearchDto input)
        {
            var totalCount = (int)0;
            var list = (List<YearMonthPlanYearDetailDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanYearDetailDto>(
                    totalCount,
                    list = new List<YearMonthPlanYearDetailDto>()
                );
            }
            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();
            await Task.Run(() =>
            {
                if (input.IsAll)
                {
                    list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanYearDetailDto>>(_service.Where(z =>
                       z.RepairTagId == RepairTagId &&
                       z.PlanCount > 0 &&
                       z.Year == input.Year &&
                       (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                       z.PlanType == input.PlanType &&
                       (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                       (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                       ).OrderBy(z => z.Number.Replace("-", "")).ToList());
                }
                else
                {
                    list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanYearDetailDto>>(_service.Where(z =>
                        z.RepairTagId == RepairTagId &&
                        z.PlanCount > 0 &&
                        z.Year == input.Year &&
                        (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                        z.PlanType == input.PlanType &&
                        (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                        (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                        ).OrderBy(z => z.Number.Replace("-", "")).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                }
                totalCount = _service.Count(z =>
                z.RepairTagId == RepairTagId &&
                z.PlanCount > 0 &&
                  z.Year == input.Year &&
                  (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                  z.PlanType == input.PlanType &&
                  (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                  (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                );

                var repairIds = list.Select(s => s.RepairDetailsId);
                var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                    {
                        if (t.OrganizationType != null)
                            executableUnitStr += t.OrganizationType.Name + ",";
                    }
                    item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                }
            });
            var isCanCreate = true;
            if (totalCount == 0)
            {
                if (input.PlanType == (int)Enumer.YearMonthPlanType.年度月表)
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z =>
                    z.RepairTagId == RepairTagId &&
                    z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year && z.Month == input.Month));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }
                else
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z =>
                    z.RepairTagId == RepairTagId &&
                    z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }

            }

            return new
            {
                isCanCreate,
                totalCount,
                items = list
            };
        }

        /// <summary>
        /// 获得月计划列表
        /// </summary>
        public async Task<dynamic> GetListMonth(YearMonthSearchDto input)
        {
            var totalCount = (int)0;
            var list = (List<YearMonthPlanMonthDetailDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanMonthDetailDto>(
                    totalCount,
                    list = new List<YearMonthPlanMonthDetailDto>()
                );
            }
            if (!input.Month.HasValue)
            {
                if (input.Month < 1 || input.Month > 12)
                {
                    throw new UserFriendlyException("请输入正确月份");
                }
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }

            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();

            await Task.Run(() =>
            {
                list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanMonthDetailDto>>(_service.Where(z =>
                z.RepairTagId == RepairTagId &&
                z.Year == input.Year &&
                (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                z.PlanType == input.PlanType &&
                (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                ).OrderBy(z => z.Number.Replace("-", "")).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                totalCount = _service.Count(z =>
                z.RepairTagId == RepairTagId &&
                  z.Year == input.Year &&
                  (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                  z.PlanType == input.PlanType &&
                  (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                  (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                  (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                );

                var repairIds = list.Select(s => s.RepairDetailsId);
                var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();
                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                    {
                        if (t.OrganizationType != null)
                            executableUnitStr += t.OrganizationType.Name + ",";
                    }
                    item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                }
            });

            var isCanCreate = true;
            if (totalCount == 0)
            {
                if (input.PlanType == (int)Enumer.YearMonthPlanType.年度月表)
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year && z.Month == input.Month));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }
                else
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }

            }
            return new
            {
                isCanCreate,
                totalCount,
                items = list
            };
        }

        /// <summary>
        /// 获得已提交的数量大于0的月计划列表
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetSubmitedListMonth(YearMonthSearchDto input)
        {

            var totalCount = (int)0;
            var list = (List<YearMonthPlanMonthDetailDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanMonthDetailDto>(
                    totalCount,
                    list = new List<YearMonthPlanMonthDetailDto>()
                );
            }
            if (!input.Month.HasValue)
            {
                if (input.Month < 1 || input.Month > 12)
                {
                    throw new UserFriendlyException("请输入正确月份");
                }
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }

            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();

            await Task.Run(() =>
            {
                list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanMonthDetailDto>>(_service.Where(z =>
                z.RepairTagId == RepairTagId &&
                z.Total > 0 &&
                z.Year == input.Year &&
                (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                z.PlanType == input.PlanType &&
                (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                ).OrderBy(z => z.Number.Replace("-", "")).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                totalCount = _service.Count(z =>
                z.RepairTagId == RepairTagId &&
                  z.Year == input.Year &&
                  z.Total > 0 &&
                  (!input.OrgId.HasValue || z.ResponsibleUnit == input.OrgId) &&
                  z.PlanType == input.PlanType &&
                  (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                  (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                  (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                );

                var repairIds = list.Select(s => s.RepairDetailsId);
                var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();
                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                    {
                        if (t.OrganizationType != null)
                            executableUnitStr += t.OrganizationType.Name + ",";
                    }
                    item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                }
            });

            var isCanCreate = true;
            if (totalCount == 0)
            {
                if (input.PlanType == (int)Enumer.YearMonthPlanType.年度月表)
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year && z.Month == input.Month));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }
                else
                {
                    var fModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.PlanType == input.PlanType && z.Year == input.Year));
                    if (fModel != null)
                    {
                        if (fModel.State == (int)Enumer.YearMonthPlanState.审核中 || fModel.State == (int)Enumer.YearMonthPlanState.审核通过)
                        {
                            isCanCreate = false;
                        }
                    }
                }

            }
            return new
            {
                isCanCreate,
                totalCount,
                items = list
            };
        }


        /// <summary>
        /// 获取统计后的年表数据 组织机构汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<YearMonthPlanYearStatisticalDto>> GetYearStatisticData(YearMonthSearchDto input)
        {
            #region 关键条件字段验证
            var totalCount = (int)0;
            var list = (List<YearMonthPlanYearStatisticalDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanYearStatisticalDto>(
                    totalCount,
                    list = new List<YearMonthPlanYearStatisticalDto>()
                );
            }
            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }
            #endregion
            PagedResultDto<YearMonthPlanYearStatisticalDto> result = new PagedResultDto<YearMonthPlanYearStatisticalDto>();

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var orgEnt = await _orgService.GetAsync((Guid)input.OrgId);
            if (orgEnt == null)
                return new PagedResultDto<YearMonthPlanYearStatisticalDto>(totalCount, list = new List<YearMonthPlanYearStatisticalDto>());
            //根据组织机构类型进行判断
            var isDuanOrganization = false;
            var childrenIdList = new List<Guid>();
            var childrenList = new List<Organization>();
            if (orgEnt?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
            }
            childrenIdList = orgEnt.Children?.Select(x => x.Id).ToList();
            childrenList = orgEnt?.Children?.ToList();
            childrenList.Add(orgEnt);
            //var orgCode = orgEnt.Code;
            //var orgs = (await _orgService.Where(s => s.Code.StartsWith(orgCode))).ToList();
            //var orgIds = orgs.Select(s => s.Id).ToList();
            var ents = new List<YearMonthPlan>();
            if (isDuanOrganization)
            {
                ents = _service.Where(z =>
                    z.RepairTagId == RepairTagId &&
                    z.Year == input.Year &&
                    z.State == 3 &&
                    childrenIdList.Contains(z.ResponsibleUnit) &&
                    //z.PlanCount > 0 &&
                    z.PlanType == input.PlanType &&
                    (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                    (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                    ).OrderBy(z => z.Number.Replace("-", "")).ToList();
            }
            else
            {
                ents = _service.Where(z =>
                   z.RepairTagId == RepairTagId &&
                   z.Year == input.Year &&
                   z.State == 3 &&
                   z.PlanCount > 0 &&
                   z.ResponsibleUnit == orgEnt.Id &&
                   z.PlanType == input.PlanType &&
                   (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                   (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                   ).OrderBy(z => z.Number.Replace("-", "")).ToList();
            }


            var groupedData = from a in ents
                              group a by a.RepairDetailsId into temp
                              select new
                              {
                                  temp.Key,
                                  List = temp.ToList(),
                              };
            List<YearMonthPlanYearStatisticalDto> resDto = new List<YearMonthPlanYearStatisticalDto>();
            result.TotalCount = groupedData.Count();

            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();

            var tempList = input.IsAll ? groupedData : groupedData.Skip(input.SkipCount).Take(input.MaxResultCount);
            var repairIds = tempList.Select(s => s.Key);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();
            foreach (var item in tempList)
            {
                var defaultItem = item.List.FirstOrDefault();
                YearMonthPlanYearStatisticalDto dto = new YearMonthPlanYearStatisticalDto();
                dto.Col_1 = item.List.Sum(s => s.Col_1);
                dto.Col_2 = item.List.Sum(s => s.Col_2);
                dto.Col_3 = item.List.Sum(s => s.Col_3);
                dto.Col_4 = item.List.Sum(s => s.Col_4);
                dto.Col_5 = item.List.Sum(s => s.Col_5);
                dto.Col_6 = item.List.Sum(s => s.Col_6);
                dto.Col_7 = item.List.Sum(s => s.Col_7);
                dto.Col_8 = item.List.Sum(s => s.Col_8);
                dto.Col_9 = item.List.Sum(s => s.Col_9);
                dto.Col_10 = item.List.Sum(s => s.Col_10);
                dto.Col_11 = item.List.Sum(s => s.Col_11);
                dto.Col_12 = item.List.Sum(s => s.Col_12);
                dto.DeviceCount = item.List.Sum(s => s.DeviceCount);
                dto.DeviceName = defaultItem?.DeviceName;
                dto.Number = defaultItem?.Number;
                dto.RepairContent = defaultItem?.RepairContent;
                dto.RepairDetailsId = item.Key;
                dto.RepairGroup = defaultItem.RepairGroup;
                dto.RepairTagId = defaultItem.RepairTagId;
                dto.RepairType = defaultItem.RepairType;
                dto.Times = defaultItem.Times;
                dto.Unit = defaultItem.Unit;
                dto.PlanCount = item.List.Sum(s => s.PlanCount);
                dto.Total = item.List.Sum(s => s.Total);
                dto.RepairContent = defaultItem.RepairContent;
                dto.Id = Guid.NewGuid();
                foreach (var child in item.List)
                {
                    if (child.PlanCount == 0) continue;
                    YearMonthPlanYearStatisticalChildDto ch = new YearMonthPlanYearStatisticalChildDto();
                    ch.Col_1 = child.Col_1;
                    ch.Col_2 = child.Col_2;
                    ch.Col_3 = child.Col_3;
                    ch.Col_4 = child.Col_4;
                    ch.Col_5 = child.Col_5;
                    ch.Col_6 = child.Col_6;
                    ch.Col_7 = child.Col_7;
                    ch.Col_8 = child.Col_8;
                    ch.Col_9 = child.Col_9;
                    ch.Col_10 = child.Col_10;
                    ch.Col_11 = child.Col_11;
                    ch.Col_12 = child.Col_12;
                    ch.EquipmentLocation = child.EquipmentLocation;
                    ch.PlanCount = child.PlanCount;
                    ch.ResponsibleUnit = child.ResponsibleUnit;
                    ch.ResponsibleUnitStr = childrenList.Where(x => x.Id == child.ResponsibleUnit)?.FirstOrDefault()?.Name;
                    ch.SkyligetType = child.SkyligetType;
                    ch.Times = child.Times;
                    ch.Total = child.Total;
                    ch.Id = Guid.NewGuid();
                    dto.ChildItems.Add(ch);
                }
                resDto.Add(dto);
            }
            foreach (var item in resDto)
            {
                var nums = item.Number.Split('-');
                string newNum = "";
                foreach (var num in nums)
                {
                    newNum += int.Parse(num).ToString() + "-";
                }
                item.Number = newNum.TrimEnd('-');
                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
            }
            result.Items = resDto;
            return result;
        }

        /// <summary>
        /// 获取统计后的月表数据 组织机构汇总
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<YearMonthPlanMonthStatisticalDto>> GetMonthStatisticData(YearMonthSearchDto input)
        {
            #region 关键条件字段验证
            var totalCount = (int)0;
            var list = (List<YearMonthPlanMonthStatisticalDto>)null;
            if (input.Year < 2000 && input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            if (!input.OrgId.HasValue)
            {
                return new PagedResultDto<YearMonthPlanMonthStatisticalDto>(
                    totalCount,
                    list = new List<YearMonthPlanMonthStatisticalDto>()
                );
            }
            if (!input.Month.HasValue)
            {
                if (input.Month < 1 || input.Month > 12)
                {
                    throw new UserFriendlyException("请输入正确月份");
                }
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }

            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }
            #endregion
            PagedResultDto<YearMonthPlanMonthStatisticalDto> result = new PagedResultDto<YearMonthPlanMonthStatisticalDto>();

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var orgEnt = await _orgService.GetAsync((Guid)input.OrgId);
            if (orgEnt == null)
                return new PagedResultDto<YearMonthPlanMonthStatisticalDto>(totalCount, list = new List<YearMonthPlanMonthStatisticalDto>());
            var isDuanOrganization = false;
            var childrenIdList = new List<Guid>();
            var childrenList = new List<Organization>();
            if (orgEnt?.Type?.Key == "OrganizationType.Duan")
            {
                isDuanOrganization = true;
            }
            childrenIdList = orgEnt.Children?.Select(x => x.Id).ToList();
            childrenList = orgEnt?.Children?.ToList();
            childrenList.Add(orgEnt);
            //var orgCode = orgEnt.Code;
            //var orgs = (await _orgService.Where(s => s.Code.StartsWith(orgCode))).ToList();
            //var orgIds = orgs.Select(s => s.Id).ToList();

            //var ents = _service.Where(z =>
            //z.RepairTagId == RepairTagId &&
            //z.Year == input.Year &&
            //z.State == 3 &&
            //orgIds.Contains(z.ResponsibleUnit) &&
            //z.PlanType == input.PlanType &&
            //(!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
            //(string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
            //).OrderBy(z => z.Number.Replace("-", "")).ToList();
            var ents = new List<YearMonthPlan>();
            if (isDuanOrganization)
            {
                ents = _service.Where(z =>
                    z.RepairTagId == RepairTagId &&
                    z.Year == input.Year &&
                    z.State == 3 &&
                    childrenIdList.Contains(z.ResponsibleUnit) &&
                    z.PlanType == input.PlanType &&
                    (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                    (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                    (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                    ).OrderBy(z => z.Number.Replace("-", "")).ToList();
            }
            else
            {
                ents = _service.Where(z =>
                       z.RepairTagId == RepairTagId &&
                       z.Year == input.Year &&
                       z.State == 3 &&
                       z.Total > 0 &&
                       z.ResponsibleUnit == orgEnt.Id &&
                       z.PlanType == input.PlanType &&
                       (input.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : (!input.Month.HasValue || z.Month == input.Month)) &&
                       (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                       (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                       ).OrderBy(z => z.Number.Replace("-", "")).ToList();
            }

            var groupedData = from a in ents
                              group a by a.RepairDetailsId into temp
                              select new
                              {
                                  temp.Key,
                                  List = temp.ToList(),
                              };
            List<YearMonthPlanMonthStatisticalDto> resDto = new List<YearMonthPlanMonthStatisticalDto>();
            result.TotalCount = groupedData.Count();

            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();
            var tempList = input.IsAll ? groupedData : groupedData.Skip(input.SkipCount).Take(input.MaxResultCount);
            var repairIds = tempList.Select(s => s.Key);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

            foreach (var item in tempList)
            {
                var defaultItem = item.List.FirstOrDefault();
                YearMonthPlanMonthStatisticalDto dto = new YearMonthPlanMonthStatisticalDto();
                for (var i = 1; i <= 31; i++)
                {
                    SetColVal(dto, i, item.List.Sum(s => GetColVal(s, i)));
                }
                dto.DeviceCount = item.List.Sum(s => s.DeviceCount);
                dto.DeviceName = defaultItem?.DeviceName;
                dto.Number = defaultItem?.Number;
                dto.RepairContent = defaultItem?.RepairContent;
                dto.RepairDetailsId = item.Key;
                dto.RepairGroup = defaultItem.RepairGroup;
                dto.RepairTagId = defaultItem.RepairTagId;
                dto.RepairType = defaultItem.RepairType;
                dto.Times = defaultItem.Times;
                dto.Unit = defaultItem.Unit;
                dto.Total = item.List.Sum(s => s.Total);
                dto.RepairContent = defaultItem.RepairContent;
                dto.Id = Guid.NewGuid();
                foreach (var child in item.List)
                {
                    if (child.Total == 0) continue;
                    YearMonthPlanMonthStatisticalChildDto ch = new YearMonthPlanMonthStatisticalChildDto();
                    for (var i = 1; i <= 31; i++)
                    {
                        SetColVal(ch, i, GetColVal(child, i));
                    }
                    ch.EquipmentLocation = child.EquipmentLocation;
                    ch.ResponsibleUnit = child.ResponsibleUnit;
                    ch.ResponsibleUnitStr = childrenList.FirstOrDefault(s => s.Id == child.ResponsibleUnit)?.Name;
                    ch.SkyligetType = child.SkyligetType;
                    ch.Total = child.Total;
                    ch.Id = Guid.NewGuid();
                    dto.ChildItems.Add(ch);
                }
                resDto.Add(dto);
            }
            foreach (var item in resDto)
            {
                var nums = item.Number.Split('-');
                string newNum = "";
                foreach (var num in nums)
                {
                    newNum += int.Parse(num).ToString() + "-";
                }
                item.Number = newNum.TrimEnd('-');
                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
            }
            result.Items = resDto;
            return result;
        }
        #endregion

        #region 获得可以变更的年月计划表===============================
        /// <summary>
        /// 获得可以变更的年月计划表
        /// </summary>
        public async Task<List<YearMonthPlanChangeDto>> GetCanChangePlanList(YearMonthGetChangeDto input)
        {
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }

            if (input.RepairlType.HasValue)
            {
                var repairTypeEum = (RepairType)Enum.Parse(typeof(RepairType), input.RepairlType.ToString());
                if (input.RepairlType.ToString() == repairTypeEum.ToString())
                {
                    throw new UserFriendlyException("维修类型不正确");
                }
            }

            var nowYear = DateTime.Now.Year;
            var list = new List<YearMonthPlanChangeDto>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            //数据字典中的执行单位
            var allExecutableUnit = _dataDictionaries.WithDetails().Where(s => s.Parent.Key == "ExecutiveUnit").ToList();
            await Task.Run(() =>
            {
                var dataList = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanChangeDto>>(_service.Where(z =>
                z.RepairTagId == RepairTagId &&
                    z.State == (int)Enumer.YearMonthPlanState.审核通过 &&
                    z.ResponsibleUnit == input.OrgId &&
                    z.Year == nowYear &&
                    z.PlanType == input.PlanType &&
                    (z.PlanType == (int)Enumer.YearMonthPlanType.月表 ? z.Month == 12 : 1 == 1) &&
                    (!input.RepairlType.HasValue || z.RepairType == input.RepairlType) &&
                     (string.IsNullOrEmpty(input.KeyWord) || z.RepairContent.Contains(input.KeyWord) || z.DeviceName.Contains(input.KeyWord) || z.EquipmentLocation.Contains(input.KeyWord))
                ).ToList());

                //去除已添加的列表
                foreach (var item in dataList)
                {
                    if (!_alterPlanService.Any(z => z.RepairTagId == RepairTagId && z.ExecYear == nowYear && z.PlanId == item.Id && z.State != (int)Enumer.YearMonthPlanState.审核通过))
                    {
                        list.Add(item);
                    }
                }
                list = list.OrderBy(z => z.Number).ToList();

                var repairIds = list.Select(s => s.RepairDetailsId);
                var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                    {
                        if (t.OrganizationType != null)
                            executableUnitStr += t.OrganizationType.Name + ",";
                    }
                    item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                }
            });

            return list;
        }
        #endregion

        #region 获得当前年月表变更列表(点击变更按钮后看到的列表)=======
        /// <summary>
        /// 获得当前年月表变更列表(点击变更按钮后看到的列表)
        /// </summary>
        public async Task<List<YearMonthPlanAlterDto>> GetOwnsChangPlan(YearMonthGetChangeDto input)
        {
            var nowYear = DateTime.Now.Year;
            var stationTypeEum = (YearMonthPlanType)Enum.Parse(typeof(YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("年月表类型不正确");
            }
            var list = (List<YearMonthPlanAlterDto>)null;

            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => s.Id != null).ToList();
            await Task.Run(() =>
            {
                var q = from a in _alterPlanService.
                        Where(s => s.RepairTagId == RepairTagId).
                        WhereIf(input.IsCreateRecord, x => x.YearMonthAlterRecordId == input.AlterRecordId).
                        WhereIf(!input.IsCreateRecord, x => x.State != (int)YearMonthPlanState.审核通过)
                        join b in _service.Where(s => s.RepairTagId == RepairTagId)
                        on a.PlanId equals b.Id
                        where a.IsExec == 0 &&
                        b.ResponsibleUnit == input.OrgId &&
                        b.PlanType == input.PlanType &&
                        b.Year == nowYear &&
                        b.State == (int)YearMonthPlanState.审核通过// && a.State != (int)YearMonthPlanState.审核通过
                        select new YearMonthPlanAlterDto
                        {
                            Id = a.Id,
                            Number = b.Number,
                            EquipmentLocation = a.EquipmentLocation,
                            CompiledOrganization = a.CompiledOrganization,
                            PlanId = a.PlanId,
                            ExecMonth = a.ExecMonth,
                            RepairType = b.RepairType,
                            State = a.State,
                            RepairGroup = b.RepairGroup,
                            DeviceName = b.DeviceName,
                            RepairContent = b.RepairContent,
                            Unit = b.Unit,
                            Times = b.Times,
                            Total = a.Total,
                            PlanCount = a.PlanCount,
                            SkyligetType = a.SkyligetType,
                            FileId = a.FileId,
                            FileName = a.FileName,
                            ChangeReason = a.ChangeReason,
                            Col_1 = a.Col_1,
                            Col_2 = a.Col_2,
                            Col_3 = a.Col_3,
                            Col_4 = a.Col_4,
                            Col_5 = a.Col_5,
                            Col_6 = a.Col_6,
                            Col_7 = a.Col_7,
                            Col_8 = a.Col_8,
                            Col_9 = a.Col_9,
                            Col_10 = a.Col_10,
                            Col_11 = a.Col_11,
                            Col_12 = a.Col_12,
                            Col_13 = a.Col_13,
                            Col_14 = a.Col_14,
                            Col_15 = a.Col_15,
                            Col_16 = a.Col_16,
                            Col_17 = a.Col_17,
                            Col_18 = a.Col_18,
                            Col_19 = a.Col_19,
                            Col_20 = a.Col_20,
                            Col_21 = a.Col_21,
                            Col_22 = a.Col_22,
                            Col_23 = a.Col_23,
                            Col_24 = a.Col_24,
                            Col_25 = a.Col_25,
                            Col_26 = a.Col_26,
                            Col_27 = a.Col_27,
                            Col_28 = a.Col_28,
                            Col_29 = a.Col_29,
                            Col_30 = a.Col_30,
                            Col_31 = a.Col_31,
                        };
                list = q.OrderBy(z => z.Number).ToList();

                var newYearMonthList = _service.Where(s => list.Select(x => x.PlanId).Contains(s.Id)).ToList();
                foreach (var item in list)
                {
                    item.StateStr = Enum.Parse(typeof(Enumer.YearMonthPlanState), item.State.ToString()).ToString();
                    var nums = item.Number.Split('-');
                    string newNum = "";
                    foreach (var num in nums)
                    {
                        newNum += int.Parse(num).ToString() + "-";
                    }
                    item.Number = newNum.TrimEnd('-');
                    string executableUnitStr = "";
                    var entity = newYearMonthList.Find(x => x.Id == item.PlanId);
                    if (entity != null)
                    {
                        foreach (var t in relate.Where(s => s.RepairItemId == entity.RepairDetailsId))
                        {
                            if (t.OrganizationType != null)
                                executableUnitStr += t.OrganizationType.Name + ",";
                        }
                        item.ExecutableUnitStr = executableUnitStr.TrimEnd(',');
                    }
                }

            });

            return list;

        }
        #endregion

        #region 保存年月表计划要变更数据===============================
        /// <summary>
        /// 保存年月表计划要变更数据
        /// </summary>
        public async Task<bool> CreateChangePlan(CommonGuidListGetDto input)
        {
            var planModel = (YearMonthPlanDto)null;
            var planAlter = (YearMonthPlanAlter)null;
            var nowYear = DateTime.Now.Year;
            input.Ids = input.Ids.Distinct().ToList();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            foreach (var item in input.Ids)
            {
                var entity = _service.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.Id == item);
                planModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(entity);
                if (planModel == null) throw new UserFriendlyException("添加对象不存在");
                if (planModel.State != (int)Enumer.YearMonthPlanState.审核通过) throw new UserFriendlyException("只有审核通过的计划可以保存");
                if (_alterPlanService.Any(z => z.RepairTagId == RepairTagId && z.PlanId == planModel.Id && z.State == (int)Enumer.YearMonthPlanState.未提交))
                {
                    continue;
                }
                planAlter = new YearMonthPlanAlter(Guid.NewGuid());
                planAlter.PlanId = planModel.Id;
                planAlter.ExecYear = nowYear;
                planAlter.ExecMonth = 0;
                planAlter.IsExec = 0;
                planAlter.PlanType = planModel.PlanType;
                planAlter.Total = planModel.Total;
                planAlter.PlanCount = planModel.PlanCount;
                planAlter.State = (int)Enumer.YearMonthPlanState.未提交;
                planAlter.CreateUser = CurrentUser.UserName;
                planAlter.UpdateTime = DateTime.Now;
                planAlter.CreateTime = DateTime.Now;
                planAlter.IsImport = false;
                planAlter.SkyligetType = planModel.SkyligetType;
                planAlter.EquipmentLocation = planModel.EquipmentLocation;
                planAlter.CompiledOrganization = planModel.CompiledOrganization;
                planAlter.RepairTagId = RepairTagId;
                for (var i = 1; i <= 31; i++)
                {
                    SetColVal(planAlter, i, GetColVal(planModel, i));
                }
                await _alterPlanService.InsertAsync(planAlter);
            }
            return true;
        }
        #endregion

        #region 添加数据(年表、月表、年度月表)=========================
        /// <summary>
        /// 添加数据(年表、月表、年度月表)
        /// </summary>
        public async Task<bool> Create(YearMonthCreateDto input)
        {
            if (input.Year < 2000 || input.Year > 9999)
            {
                throw new UserFriendlyException("生成年份应在2000-9999间");
            }
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("生成类型不正确");
            }
            if (input.OrgId == Guid.Empty)
            {
                throw new UserFriendlyException("生成车间不能为空");
            }
            if ((await _orgService.Where(z => z.Id == input.OrgId)).Count() == 0)
            {
                throw new UserFriendlyException("生成车间不存在");
            }
            if (input.PlanType != (int)Enumer.YearMonthPlanType.年度月表)
            {
                input.Month = (int?)null;//年度月表需要生成月份
            }
            else if (!input.Month.HasValue) throw new UserFriendlyException("年度月表生成月份不能为空");

            var repairList = (List<RepairItemDto>)null;
            var yearMonthPlanTest = (YearMonthPlanTestItem)null;
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            if (!_service.Any(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && z.PlanType == input.PlanType && (z.State == (int)Enumer.YearMonthPlanState.审核中 || z.State == (int)Enumer.YearMonthPlanState.审核通过) && (input.PlanType != (int)Enumer.YearMonthPlanType.年度月表 || z.Month == input.Month)))
            {
                //删除已有数据后重新生成
                await _service.DeleteAsync(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && z.PlanType == input.PlanType && (input.PlanType == (int)Enumer.YearMonthPlanType.年度月表 ? z.Month == input.Month : 1 == 1));
            }
            else
            {
                throw new UserFriendlyException("计划已审核中或审核完成,不能再次操作");
            }

            //年月表添加维修项测试项
            if (!_testService.Any(z => z.RepairTagId == RepairTagId && z.PlanYear == input.Year))
            {
                //await _basicRepairTestItemAppService.UpgradeTestItems(new StdBasic.Dtos.RepairTestItemUpgradeDto { Year = input.Year, IsUpgradeAll = true });
                await UpgradeTestItems(new RepairTestItemUpgradeDto { Year = input.Year, RepairTagKey = input.RepairTagKey });
                //var repairTest = ObjectMapper.Map<List<RepairTestItem>, List<RepairTestItemDto>>(_repairTestService.Where(z => z.Id != Guid.Empty).ToList());
                //foreach (var ele in repairTest)
                //{
                //    yearMonthPlanTest = new YearMonthPlanTestItem(Guid.NewGuid());
                //    yearMonthPlanTest.PlanYear = input.Year;
                //    yearMonthPlanTest.RepairDetailsID = ele.RepairItemId;
                //    yearMonthPlanTest.Name = ele.Name;
                //    yearMonthPlanTest.TestType = (int)ele.Type;
                //    yearMonthPlanTest.TestUnit = ele.Unit;
                //    yearMonthPlanTest.TestContent = "";
                //    yearMonthPlanTest.PredictedValue = ele.DefaultValue;
                //    yearMonthPlanTest.MaxRated = ele.MaxRated;
                //    yearMonthPlanTest.MinRated = ele.MinRated;
                //    yearMonthPlanTest.FileId = ele.FileId;
                //    await _testService.InsertAsync(yearMonthPlanTest);
                //}
            }
            //按生成类型找到所有维修项
            var forCount = (int)0;
            switch (input.PlanType)
            {
                case (int)Enumer.YearMonthPlanType.年表:
                    {
                        forCount = 1;
                        repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.WithDetails(x => x.Group.Parent).Where(z => z.TagId == RepairTagId && z.IsMonth == false).ToList().OrderBy(z => z.Number).ToList());
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.月表:
                    {
                        forCount = 12;//月表要生成12个月的数据
                        repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.WithDetails(x =>
                        x.Group.Parent).Where(z => z.TagId == RepairTagId && z.IsMonth == true).OrderBy(z => z.Number).ToList());
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.年度月表:
                    {
                        if (!input.Month.HasValue) throw new UserFriendlyException("生成年度月表需输入月份");
                        if (input.Month > 12 || input.Month < 1) throw new UserFriendlyException("输入月份不正确");
                        var yearPlanList = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == RepairTagId && z.PlanType == (int)Enumer.YearMonthPlanType.年表 && z.Year == input.Year && z.ResponsibleUnit == input.OrgId && z.State == (int)Enumer.YearMonthPlanState.审核通过).ToList());
                        CreateYearOfMonthPlan(yearPlanList, (int)input.Month);//生成年度月表计划
                        return true;
                    }
            }

            //找到当前生成组织机构自己及其所有子集
            var orgModel = ObjectMapper.Map<Organization, OrganizationDto>((await _orgService.Where(z => z.Id == input.OrgId)).FirstOrDefault());
            if (orgModel == null) throw new UserFriendlyException("生成组织机构不存在");
            var searchList = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>((await _orgService.Where(z => z.Code.StartsWith(orgModel.Code))).ToList());
            var codeList = searchList.Select(z => z.Id).ToList();

            var equiqCodes = (from t in ((from a in _equipmentService
                                          join b in _equipmentOrgService
                                              on a.Id equals b.EquipmentId
                                          where codeList.Contains(b.OrganizationId)
                                          select new
                                          {
                                              a.ComponentCategoryId,
                                              a.Code,
                                          }).Distinct())
                              select t.ComponentCategoryId).ToArray();
            var tt = 0;
            var i = (int)0;
            var dCount = (int)0;

            // 


            foreach (var item in repairList)
            {
                //获得设备数量
                dCount = GetPlanEquipmentsCount(item, equiqCodes);

                //维修项
                for (i = 1; i <= forCount; i++)
                {
                    var yearMonthPlanModel = new YearMonthPlan(Guid.NewGuid());
                    yearMonthPlanModel.RepairDetailsId = item.Id;
                    //yearMonthPlanModel.Number = item.Number;
                    yearMonthPlanModel.Number = item.Group?.Parent?.Order.ToString().PadLeft(3, '0') + "-" +
                        item.Group?.Order.ToString().PadLeft(3, '0') + "-" +
                        item.Number.ToString().PadLeft(3, '0');
                    yearMonthPlanModel.RepairGroup = item.Group.Parent.Name;
                    yearMonthPlanModel.RepairType = (int)item.Type;
                    yearMonthPlanModel.DeviceName = item.Group.Name;
                    yearMonthPlanModel.IsImport = false;//是否导入过文件
                    yearMonthPlanModel.RepairContent = item.Content;
                    yearMonthPlanModel.CompiledOrganization = "";
                    yearMonthPlanModel.Unit = item.Unit;
                    yearMonthPlanModel.DeviceCount = dCount;
                    yearMonthPlanModel.Total = dCount;
                    yearMonthPlanModel.PlanType = (int)input.PlanType;
                    yearMonthPlanModel.Times = item.Period;
                    yearMonthPlanModel.Year = input.Year;
                    yearMonthPlanModel.Month = forCount == 1 ? 0 : i;
                    yearMonthPlanModel.ResponsibleUnit = input.OrgId;//生成单位
                    yearMonthPlanModel.CreateUser = "";
                    yearMonthPlanModel.CreateTime = DateTime.Now;
                    yearMonthPlanModel.State = (int)Enumer.YearMonthPlanState.未提交;
                    yearMonthPlanModel.RepairTagId = RepairTagId;
                    await _service.InsertAsync(yearMonthPlanModel);
                }
            }
            return true;
        }

        /// <summary>
        /// 获得生成设备数量
        /// </summary>
        private int GetPlanEquipmentsCount(RepairItemDto repair, Guid?[] equiqCodes)
        {
            var dCount = (int)0;
            try
            {
                //找到当前维修项关联的IFDCode
                var repairIFDs = (from a in _reparIFDService
                                  join b in _componentCategoryRepository
                                  on a.ComponentCategoryId equals b.Id
                                  where a.RepairItemId == repair.Id
                                  select new
                                  {
                                      b.Id
                                  }).ToList();
                if (repairIFDs.Count() == 0) return dCount;
                repairIFDs.ForEach(m =>
                {
                    dCount += equiqCodes.Count(z => z == m.Id);
                });
            }
            catch (Exception ex)
            {
                return dCount;
            }
            return dCount;

        }

        /// <summary>
        /// 生成年度月表计划
        /// </summary>
        /// <param name="list"></param>
        private void CreateYearOfMonthPlan(List<YearMonthPlanDto> list, int month)
        {
            //if (item.Col_1 > 0) CreateYearOfMonthData(item, 1, item.Col_1);
            foreach (var model in list)
            {
                var colVal = GetColVal(model, month);
                if (colVal > 0)
                {
                    var parentId = model.Id;
                    model.Id = Guid.NewGuid();
                    var newModel = ObjectMapper.Map<YearMonthPlanDto, YearMonthPlan>(model);
                    newModel.IsImport = false;//是否导入过文件
                    newModel.ParentId = parentId;
                    newModel.Total = colVal;
                    newModel.PlanCount = 0;
                    newModel.PlanType = (int)Enumer.YearMonthPlanType.年度月表;
                    newModel.Month = month;
                    newModel.State = (int)Enumer.YearMonthPlanState.未提交;
                    newModel.CreateTime = DateTime.Now;
                    for (var i = 1; i < 31; i++)
                    {
                        SetColVal(newModel, i, 0);
                    }
                    _service.InsertAsync(newModel);
                }
            }
        }
        #endregion

        #region 编辑天窗类型===========================================
        /// <summary>
        /// 编辑天窗类型
        /// </summary>
        public async Task<bool> UpdateSkyligetType(YearMonthPlanUpdateDto input)
        {
            var oldEnt = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == input.Id));
            if (oldEnt == null)
            {
                throw new UserFriendlyException("编辑对象不存在");
            }
            input.SkyligetType = input.SkyligetType == null ? "" : input.SkyligetType;
            oldEnt.SkyligetType = input.SkyligetType.Trim(',');

            var total = (decimal)0;
            for (var i = 1; i <= 31; i++)
            {
                total += GetColVal(oldEnt, i);
            }
            if (total > 0 && string.IsNullOrWhiteSpace(oldEnt.SkyligetType))
            {
                throw new UserFriendlyException("天窗类型不能为空");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            //更改12个月的计划天窗类型
            if (oldEnt.PlanType == (int)Enumer.YearMonthPlanType.月表)
            {
                var modelList = _service.Where(z =>
                                               z.RepairTagId == RepairTagId &&
                                               z.ResponsibleUnit == oldEnt.ResponsibleUnit &&
                                               z.Year == oldEnt.Year &&
                                               z.RepairDetailsId == oldEnt.RepairDetailsId)
                                        .ToList();
                foreach (var item in modelList)
                {
                    item.SkyligetType = oldEnt.SkyligetType;
                    item.RepairTagId = RepairTagId;
                    await _service.UpdateAsync(item);
                }
            }
            else
            {
                var model = _service.FirstOrDefault(z => z.Id == input.Id);
                model.SkyligetType = oldEnt.SkyligetType;
                model.RepairTagId = RepairTagId;
                await _service.UpdateAsync(model);
            }
            return true;
        }

        /// <summary>
        /// 编辑年月计划
        /// </summary>
        public async Task<bool> UpdateYearMonthPlan(YearMonthPlanMonthUpdateDto input)
        {
            var oldEnt = _service.FirstOrDefault(z => z.Id == input.Id);
            if (oldEnt == null)
            {
                throw new UserFriendlyException("编辑对象不存在");
            }
            var total = (decimal)0;
            for (var i = 1; i <= 31; i++)
            {
                total += GetColVal(oldEnt, i);
            }
            if (total > 0 && string.IsNullOrWhiteSpace(input.SkyligetType))
            {
                throw new UserFriendlyException("天窗类型不能为空");
            }
            if (total > 0 && string.IsNullOrWhiteSpace(input.EquipmentLocation))
            {
                throw new UserFriendlyException("设备处所不能为空");
            }
            if (total > 0 && string.IsNullOrWhiteSpace(input.CompiledOrganization) && input.RepairTagKey == "RepairTag.RailwayWired")
            {
                throw new UserFriendlyException("编制执行单位不能为空");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            //更改12个月的计划天窗类型
            if (oldEnt.PlanType == (int)Enumer.YearMonthPlanType.月表)
            {
                var modelList = _service.Where(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == oldEnt.ResponsibleUnit && z.Year == oldEnt.Year && z.RepairDetailsId == oldEnt.RepairDetailsId).ToList();
                foreach (var item in modelList)
                {
                    item.SkyligetType = input.SkyligetType;
                    item.RepairTagId = RepairTagId;
                    item.EquipmentLocation = input.EquipmentLocation;
                    item.PlanCount = input.PlanCount;
                    item.CompiledOrganization = input.CompiledOrganization;
                    item.Total = input.Total;
                    item.Col_1 = input.Col_1;
                    item.Col_2 = input.Col_2;
                    item.Col_3 = input.Col_3;
                    item.Col_4 = input.Col_4;
                    item.Col_5 = input.Col_5;
                    item.Col_6 = input.Col_6;
                    item.Col_7 = input.Col_7;
                    item.Col_8 = input.Col_8;
                    item.Col_9 = input.Col_9;
                    item.Col_10 = input.Col_10;
                    item.Col_11 = input.Col_11;
                    item.Col_12 = input.Col_12;
                    item.Col_13 = input.Col_13;
                    item.Col_14 = input.Col_14;
                    item.Col_15 = input.Col_15;
                    item.Col_16 = input.Col_16;
                    item.Col_17 = input.Col_17;
                    item.Col_18 = input.Col_18;
                    item.Col_19 = input.Col_19;
                    item.Col_20 = input.Col_20;
                    item.Col_21 = input.Col_21;
                    item.Col_22 = input.Col_22;
                    item.Col_23 = input.Col_23;
                    item.Col_24 = input.Col_24;
                    item.Col_25 = input.Col_25;
                    item.Col_26 = input.Col_26;
                    item.Col_27 = input.Col_27;
                    item.Col_28 = input.Col_28;
                    item.Col_29 = input.Col_29;
                    item.Col_30 = input.Col_30;
                    item.Col_31 = input.Col_31;
                    await _service.UpdateAsync(item);
                }
            }
            else
            {
                var model = _service.FirstOrDefault(z => z.Id == input.Id);
                model.SkyligetType = input.SkyligetType;
                model.RepairTagId = RepairTagId;
                model.EquipmentLocation = input.EquipmentLocation;
                model.PlanCount = input.PlanCount;
                model.CompiledOrganization = input.CompiledOrganization;
                model.Total = input.Total;
                model.Col_1 = input.Col_1;
                model.Col_2 = input.Col_2;
                model.Col_3 = input.Col_3;
                model.Col_4 = input.Col_4;
                model.Col_5 = input.Col_5;
                model.Col_6 = input.Col_6;
                model.Col_7 = input.Col_7;
                model.Col_8 = input.Col_8;
                model.Col_9 = input.Col_9;
                model.Col_10 = input.Col_10;
                model.Col_11 = input.Col_11;
                model.Col_12 = input.Col_12;
                model.Col_13 = input.Col_13;
                model.Col_14 = input.Col_14;
                model.Col_15 = input.Col_15;
                model.Col_16 = input.Col_16;
                model.Col_17 = input.Col_17;
                model.Col_18 = input.Col_18;
                model.Col_19 = input.Col_19;
                model.Col_20 = input.Col_20;
                model.Col_21 = input.Col_21;
                model.Col_22 = input.Col_22;
                model.Col_23 = input.Col_23;
                model.Col_24 = input.Col_24;
                model.Col_25 = input.Col_25;
                model.Col_26 = input.Col_26;
                model.Col_27 = input.Col_27;
                model.Col_28 = input.Col_28;
                model.Col_29 = input.Col_29;
                model.Col_30 = input.Col_30;
                model.Col_31 = input.Col_31;
                await _service.UpdateAsync(model);
            }
            return true;
        }
        #endregion

        #region 删除年月表变更数据=====================================
        /// <summary>
        /// 删除年月表变更数据
        /// </summary>
        public async Task<bool> DeletePlanAtler(CommonGuidGetDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var model = ObjectMapper.Map<YearMonthPlanAlter, YearMonthPlanAlterDto>(_alterPlanService.FirstOrDefault(z => z.RepairTagId == RepairTagId && z.Id == input.Id));
            if (model == null) throw new UserFriendlyException("删除对象不存在");
            if (!(model.State == (int)Enumer.YearMonthPlanState.待审核 || model.State == (int)Enumer.YearMonthPlanState.未提交 || model.State == (int)Enumer.YearMonthPlanState.审核驳回)) throw new UserFriendlyException("对象已不允许删除");
            await _alterPlanService.DeleteAsync(s => s.Id == input.Id);
            return true;
        }
        #endregion

        #region 导出Excel文件==========================================
        /// <summary>
        /// 导出Excel文件
        /// </summary>
        [Produces("application/octet-stream")]
        public async Task<Stream> DownLoadAsync(YearMonthExportDto input)
        {
            Stream st = null;
            byte[] sbuf;
            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("导出类型不正确");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            await Task.Run(async () =>
            {
                switch (input.PlanType)
                {
                    case (int)Enumer.YearMonthPlanType.年表:
                        {
                            sbuf = await ExporToYearPlanData(input.OrgId, input.Year, RepairTagId, input.RepairTagKey);
                            st = new MemoryStream(sbuf);
                            break;
                        }
                    case (int)Enumer.YearMonthPlanType.月表:
                        {
                            sbuf = await ExporToMonthPlanData(input.OrgId, input.Year, RepairTagId, input.RepairTagKey);
                            st = new MemoryStream(sbuf);
                            break;
                        }
                    case (int)Enumer.YearMonthPlanType.年度月表:
                        {
                            if (!input.Month.HasValue) input.Month = 0;
                            sbuf = await ExporToMonthOfYearPlanData(input.OrgId, input.Year, (int)input.Month, RepairTagId, input.RepairTagKey);
                            st = new MemoryStream(sbuf);
                            break;
                        }
                    default:
                        {
                            throw new UserFriendlyException("导入计划类型不正确");
                        }
                }

            });
            return st;
        }

        #region 年表导出Excel文件
        private async Task<byte[]> ExporToYearPlanData(Guid orgId, int year, Guid? repairTagId, string? repairTagKey)
        {
            var dt = (DataTable)null;
            var col = (DataColumn)null;
            var row = (DataRow)null;
            var list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>
                (_service.Where(z => z.RepairTagId == repairTagId &&
                                     z.ResponsibleUnit == orgId &&
                                     z.Year == year &&
                                     z.PlanType == (int)Enumer.YearMonthPlanType.年表)
                         .OrderBy(z => z.Number)
                         .ToList());
            if (list.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            dt = new DataTable();

            var repairIds = list.Select(s => s.RepairDetailsId);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

            //添加列表
            var enumValues = Enum.GetValues(typeof(Enumer.YearPlanExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    col = new DataColumn(Enum.GetName(typeof(Enumer.YearPlanExcelCol), item));
                    if (repairTagKey == "RepairTag.RailwayHighSpeed" && col.ColumnName == "编制执行单位")
                        continue;
                    dt.Columns.Add(col);
                }
            }
            //var isHighWay = (await _dataDictionaries.GetAsync((Guid)repairTagId)).Key == "RepairTag.RailwayHighSpeed";
            //添加数据
            foreach (var item in list)
            {
                row = dt.NewRow();
                row[YearPlanExcelCol.Id.ToString()] = item.Id;
                row[YearPlanExcelCol.序号.ToString()] = item.Number;
                row[YearPlanExcelCol.维修类别.ToString()] = EnumHelper.GetDescription((RepairType)item.RepairType);
                row[YearPlanExcelCol.设备名称.ToString()] = item.DeviceName;

                //添加执行单位
                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                executableUnitStr = executableUnitStr.TrimEnd(',');
                row[YearPlanExcelCol.执行单位.ToString()] = executableUnitStr;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    row[YearPlanExcelCol.编制执行单位.ToString()] = item.CompiledOrganization;
                }
                if (repairTagKey == "RepairTag.RailwayHighSpeed")
                    row[YearPlanExcelCol.设备处所.ToString()] = item.PlanCount > 0 ? item.EquipmentLocation : "通信机房";
                else
                    row[YearPlanExcelCol.设备处所.ToString()] = item.EquipmentLocation;
                row[YearPlanExcelCol.工作内容.ToString()] = item.RepairContent;
                row[YearPlanExcelCol.天窗类型.ToString()] = item.SkyligetType;
                row[YearPlanExcelCol.单位.ToString()] = item.Unit;
                row[YearPlanExcelCol.总设备数量.ToString()] = item.Total;
                row[YearPlanExcelCol.年计划总数量.ToString()] = item.PlanCount;
                row[YearPlanExcelCol.每年次数.ToString()] = item.Times;
                row[YearPlanExcelCol.一月.ToString()] = item.Col_1;
                row[YearPlanExcelCol.二月.ToString()] = item.Col_2;
                row[YearPlanExcelCol.三月.ToString()] = item.Col_3;
                row[YearPlanExcelCol.四月.ToString()] = item.Col_4;
                row[YearPlanExcelCol.五月.ToString()] = item.Col_5;
                row[YearPlanExcelCol.六月.ToString()] = item.Col_6;
                row[YearPlanExcelCol.七月.ToString()] = item.Col_7;
                row[YearPlanExcelCol.八月.ToString()] = item.Col_8;
                row[YearPlanExcelCol.九月.ToString()] = item.Col_9;
                row[YearPlanExcelCol.十月.ToString()] = item.Col_10;
                row[YearPlanExcelCol.十一月.ToString()] = item.Col_11;
                row[YearPlanExcelCol.十二月.ToString()] = item.Col_12;
                dt.Rows.Add(row);
            }
            return ExcelHepler.DataTableToExcel(dt, "年表计划.xlsx", new List<int>() { 0 });
        }
        #endregion

        #region 月表导出Excel文件
        private async Task<byte[]> ExporToMonthPlanData(Guid orgId, int year, Guid? repairTagId, string? repairTagKey)
        {
            var dt = (DataTable)null;
            var col = (DataColumn)null;
            var row = (DataRow)null;
            var list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == repairTagId && z.ResponsibleUnit == orgId && z.Year == year && z.PlanType == (int)Enumer.YearMonthPlanType.月表 && z.Month == 12).OrderBy(z => z.Number).ToList());
            if (list.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            dt = new DataTable();

            var repairIds = list.Select(s => s.RepairDetailsId);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

            //添加列表
            var enumValues = Enum.GetValues(typeof(Enumer.MonthPlanExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    col = new DataColumn(Enum.GetName(typeof(Enumer.MonthPlanExcelCol), item));
                    if (repairTagKey == "RepairTag.RailwayHighSpeed" && col.ColumnName == "编制执行单位")
                        continue;
                    dt.Columns.Add(col);
                }
                for (var i = 1; i <= 31; i++)//1-31天
                {
                    dt.Columns.Add(i.ToString());
                }
            }
            //var isHighWay = (await _dataDictionaries.GetAsync((Guid)repairTagId)).Key == "RepairTag.RailwayHighSpeed";
            //添加数据
            foreach (var item in list)
            {
                row = dt.NewRow();
                row[MonthPlanExcelCol.Id.ToString()] = item.Id;
                row[MonthPlanExcelCol.序号.ToString()] = item.Number;
                row[MonthPlanExcelCol.维修类别.ToString()] = EnumHelper.GetDescription((RepairType)item.RepairType);
                row[MonthPlanExcelCol.设备名称.ToString()] = item.DeviceName;

                //添加执行单位
                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                executableUnitStr = executableUnitStr.TrimEnd(',');
                row[MonthPlanExcelCol.执行单位.ToString()] = executableUnitStr;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    row[YearPlanExcelCol.编制执行单位.ToString()] = item.CompiledOrganization;
                }

                if (repairTagKey == "RepairTag.RailwayHighSpeed")
                    row[MonthPlanExcelCol.设备处所.ToString()] = item.PlanCount > 0 ? item.EquipmentLocation : "通信机房";
                else
                    row[MonthPlanExcelCol.设备处所.ToString()] = item.EquipmentLocation;
                row[MonthPlanExcelCol.工作内容.ToString()] = item.RepairContent;
                row[MonthPlanExcelCol.天窗类型.ToString()] = item.SkyligetType;
                row[MonthPlanExcelCol.单位.ToString()] = item.Unit;
                row[MonthPlanExcelCol.数量.ToString()] = item.Total;
                row[MonthPlanExcelCol.每月次数.ToString()] = item.Times;

                for (var i = 1; i <= 31; i++)//获取值到DataTable
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                dt.Rows.Add(row);
            }
            return ExcelHepler.DataTableToExcel(dt, "月表计划.xlsx", new List<int>() { 0 });
        }
        #endregion

        #region 年度月表导出Excel文件
        private async Task<byte[]> ExporToMonthOfYearPlanData(Guid orgId, int year, int month, Guid? repairTagId, string? repairTagKey)
        {
            var dt = (DataTable)null;
            var col = (DataColumn)null;
            var row = (DataRow)null;
            var list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == repairTagId && z.ResponsibleUnit == orgId && z.Year == year && z.Month == month && z.PlanType == (int)Enumer.YearMonthPlanType.年度月表).OrderBy(z => z.Number).ToList());
            if (list.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            dt = new DataTable();

            var repairIds = list.Select(s => s.RepairDetailsId);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();
            //当月有多少天
            var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);

            //添加列表
            var enumValues = Enum.GetValues(typeof(Enumer.MonthOfYearPlanExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    col = new DataColumn(Enum.GetName(typeof(Enumer.MonthOfYearPlanExcelCol), item));
                    if (repairTagKey == "RepairTag.RailwayHighSpeed" && col.ColumnName == "编制执行单位")
                        continue;
                    dt.Columns.Add(col);
                }
                for (var i = 1; i <= monthDays; i++)//1-31天
                {
                    dt.Columns.Add(i.ToString());
                }
            }
            //var isHighWay = (await _dataDictionaries.GetAsync((Guid)repairTagId)).Key == "RepairTag.RailwayHighSpeed";
            //添加数据
            foreach (var item in list)
            {
                row = dt.NewRow();
                row[MonthOfYearPlanExcelCol.Id.ToString()] = item.Id;
                row[MonthOfYearPlanExcelCol.序号.ToString()] = item.Number;
                row[MonthOfYearPlanExcelCol.维修类别.ToString()] = EnumHelper.GetDescription((RepairType)item.RepairType);
                row[MonthOfYearPlanExcelCol.设备名称.ToString()] = item.DeviceName;

                //添加执行单位
                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                executableUnitStr = executableUnitStr.TrimEnd(',');
                row[MonthOfYearPlanExcelCol.执行单位.ToString()] = executableUnitStr;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    row[YearPlanExcelCol.编制执行单位.ToString()] = item.CompiledOrganization;
                }


                if (repairTagKey == "RepairTag.RailwayHighSpeed")
                    row[MonthOfYearPlanExcelCol.设备处所.ToString()] = item.PlanCount > 0 ? item.EquipmentLocation : "通信机房";
                else
                    row[MonthOfYearPlanExcelCol.设备处所.ToString()] = item.EquipmentLocation;
                row[MonthOfYearPlanExcelCol.工作内容.ToString()] = item.RepairContent;
                row[MonthOfYearPlanExcelCol.天窗类型.ToString()] = item.SkyligetType;
                row[MonthOfYearPlanExcelCol.单位.ToString()] = item.Unit;
                row[MonthOfYearPlanExcelCol.数量.ToString()] = item.Total;

                //列1-31取值
                for (var i = 1; i <= monthDays; i++)
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                dt.Rows.Add(row);
            }
            return ExcelHepler.DataTableToExcel(dt, "年度月表计划.xlsx", new List<int>() { 0 });
        }
        #endregion
        #endregion

        #region 上传Excel文件==========================================
        /// <summary>
        /// 上传Excel文件
        /// </summary>
        public async Task<bool> UploadAsync([FromForm] ImportData input)
        {
            DataTable dt = null;
            await _fileImport.Start(input.ImportKey, 0);
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            if (input.File == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("上传文件不能为空");
            }
            if (input.File.File == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("上传文件不能为空");
            }
            try
            {
                dt = ExcelHepler.ExcelToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, 1, DateTime.Now.Year + "月表");//读取EXCEL
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (dt == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未读取到任何数据");
            }
            if (dt.Rows.Count == 0)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未读取到任何数据");
            }
            var opCount = (int)0;

            switch (input.PlanType)
            {
                case (int)Enumer.YearMonthPlanType.年表:
                    {
                        await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
                        await ImportToYearPlanData(dt, input.Year, input.OrgId, input.ImportKey, RepairTagId, input.RepairTagKey);
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.月表:
                    {
                        await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
                        await ImportToMonthPlanData(dt, input.Year, input.OrgId, input.ImportKey, RepairTagId, input.RepairTagKey);
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.年度月表:
                    {
                        if (!input.Month.HasValue)
                        {
                            await _fileImport.Cancel(input.ImportKey);
                            throw new UserFriendlyException("导入年度月表请输入月份");
                        }
                        if (input.Month > 12 || input.Month < 1)
                        {
                            await _fileImport.Cancel(input.ImportKey);
                            throw new UserFriendlyException("请输入正确的月份");
                        }
                        opCount = _service.Count(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && z.Month == input.Month && z.PlanType == input.PlanType);
                        if (opCount != dt.Rows.Count)
                        {
                            await _fileImport.Cancel(input.ImportKey);
                            throw new UserFriendlyException("导入文件数量不正确");
                        }
                        await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
                        await ImportToMonthOfYearPlanData(dt, input.Year, (int)input.Month, input.OrgId, input.ImportKey, RepairTagId, input.RepairTagKey);
                        break;
                    }
                default:
                    {
                        await _fileImport.Cancel(input.ImportKey);
                        throw new UserFriendlyException("导入计划类型不正确");
                    }
            }
            return true;
        }

        #region 年表导入数据解悉
        /// <summary>
        /// 导入年表数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year"></param>
        /// <param name="orgId"></param>
        /// <param name="importKey"></param>
        private async Task ImportToYearPlanData(DataTable dt, int year, Guid orgId, string importKey, Guid? repairTagId, string? repairTagKey)
        {
            var dataList = new List<YearMonthPlanDto>();
            var planModel = (YearMonthPlanDto)null;
            var id = Guid.Empty;
            var numb = (decimal)0;
            var celVal = (string)null;

            var repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.Where(z => z.TagId == repairTagId && z.IsMonth == false).ToList());//获得所有月度维修项
            if (repairList.Count == 0)
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("未找到月维修项");
            }
            #region 关键列验证
            if (!dt.Columns.Contains(YearPlanExcelCol.Id.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.Id.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.设备处所.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.设备处所.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.编制执行单位.ToString()) && repairTagKey == "RepairTag.RailwayWired")
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.编制执行单位.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.天窗类型.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.天窗类型.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.总设备数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.总设备数量.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.年计划总数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.年计划总数量.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.每年次数.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.每年次数.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.一月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.一月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.二月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.二月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.三月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.三月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.四月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.四月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.五月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.五月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.六月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.六月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.七月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.七月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.八月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.八月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.九月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.九月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十一月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十一月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十二月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十二月.ToString());
            }

            #endregion

            #region 数据验证并赋值
            var rowNub = (int)1;
            foreach (DataRow row in dt.Rows)
            {
                rowNub++;
                planModel = new YearMonthPlanDto();
                //主键
                celVal = row[YearPlanExcelCol.Id.ToString()].ToString();
                if (!Guid.TryParse(celVal, out id)) ShowImportMsg(rowNub, YearPlanExcelCol.Id.ToString(), "主键错误", importKey);
                planModel.Id = id;

                //设备处所
                celVal = row[YearPlanExcelCol.设备处所.ToString()].ToString();
                planModel.EquipmentLocation = celVal;

                //编制执行单位
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    celVal = row[YearPlanExcelCol.编制执行单位.ToString()].ToString();
                    planModel.CompiledOrganization = celVal;
                }

                //总数量
                celVal = row[YearPlanExcelCol.总设备数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.总设备数量.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.总设备数量.ToString(), "需大于0", importKey);
                planModel.Total = Math.Round(numb, 3);

                //年计划数量
                celVal = row[YearPlanExcelCol.年计划总数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.年计划总数量.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.年计划总数量.ToString(), "需大于0", importKey);
                planModel.PlanCount = Math.Round(numb, 3);

                //月份
                celVal = row[YearPlanExcelCol.一月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.一月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.一月.ToString(), "需大于0", importKey);
                planModel.Col_1 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.二月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.二月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.二月.ToString(), "需大于0", importKey);
                planModel.Col_2 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.三月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.三月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.三月.ToString(), "需大于0", importKey);
                planModel.Col_3 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.四月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.四月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.四月.ToString(), "需大于0", importKey);
                planModel.Col_4 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.五月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.五月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.五月.ToString(), "需大于0", importKey);
                planModel.Col_5 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.六月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.六月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.六月.ToString(), "需大于0", importKey);
                planModel.Col_6 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.七月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.七月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.七月.ToString(), "需大于0", importKey);
                planModel.Col_7 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.八月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.八月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.八月.ToString(), "需大于0", importKey);
                planModel.Col_8 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.九月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.九月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.九月.ToString(), "需大于0", importKey);
                planModel.Col_9 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十月.ToString(), "需大于0", importKey);
                planModel.Col_10 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十一月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十一月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十一月.ToString(), "需大于0", importKey);
                planModel.Col_11 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十二月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十二月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十二月.ToString(), "需大于0", importKey);
                planModel.Col_12 = Math.Round(numb, 3);

                //天窗类型
                celVal = row[YearPlanExcelCol.天窗类型.ToString()].ToString();
                if (!string.IsNullOrWhiteSpace(celVal))
                {
                    var skyTypeEum = SnAbp.CrPlan.Enumer.SkyligetType.其他;
                    var skyArr = celVal.Trim(',').Split(',');
                    foreach (var item in skyArr)
                    {
                        if (!Enum.TryParse(item, out skyTypeEum))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ":天窗类型不正确(多个类型用英文','分隔)");
                        }
                    }
                    planModel.SkyligetType = celVal;
                }

                var yearTotal = planModel.Col_1 + planModel.Col_2 + planModel.Col_3 + planModel.Col_4 + planModel.Col_5 + planModel.Col_6 + planModel.Col_7 + planModel.Col_8 + planModel.Col_9 + planModel.Col_10 + planModel.Col_11 + planModel.Col_12;

                //年表数量大于0时，设备处所不能为空
                if (yearTotal > 0 && string.IsNullOrWhiteSpace(planModel.EquipmentLocation))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":设备处所不能为空");
                }

                //年表数量大于0时，设备处所不能为空
                if (yearTotal > 0 && string.IsNullOrWhiteSpace(planModel.SkyligetType))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":天窗类型不能为空");
                }

                var oldPlanModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == planModel.Id));
                if (oldPlanModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的计划不存在");
                }
                var repairModel = repairList.FirstOrDefault(z => z.Id == oldPlanModel.RepairDetailsId);
                if (repairModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的维修项已不存在");
                }
                planModel.Times = repairModel.Period;
                if (decimal.TryParse(repairModel.Period, out numb))//每年次数为数字
                {
                    if (!(numb == 1 || numb == 2 || numb == 4))
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ":每年次数只能为1,2,4");
                    }
                    //总数量
                    if (planModel.PlanCount != yearTotal)
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ":年计划量与每月数量和不等");
                    }
                    //年计划量
                    if (planModel.PlanCount != numb * planModel.Total)
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ": 年计划量不等于总数量*次数");
                    }

                    //半年
                    if (numb == 2)
                    {
                        if ((planModel.Col_1 + planModel.Col_2 + planModel.Col_3 + planModel.Col_4 + planModel.Col_5 + planModel.Col_6) != (planModel.Col_7 + planModel.Col_8 + planModel.Col_9 + planModel.Col_10 + planModel.Col_11 + planModel.Col_12))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ": 前半年与后半年数量不等");
                        }
                    }
                    //半年
                    if (numb == 4)
                    {
                        var preYear = (planModel.Col_1 + planModel.Col_2 + planModel.Col_3);
                        if (preYear != (planModel.Col_4 + planModel.Col_5 + planModel.Col_6) || preYear != (planModel.Col_7 + planModel.Col_8 + planModel.Col_9) || preYear != (planModel.Col_10 + planModel.Col_11 + planModel.Col_12))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ": 季度表数量不符合要求");
                        }
                    }
                }
                else
                {
                    if (planModel.PlanCount != yearTotal)
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ":年计划数量与每月数量和不等");
                    }
                }
                planModel.RepairTagId = repairTagId;
                dataList.Add(planModel);
            }
            #endregion

            //更新保存数据
            var newModel = (YearMonthPlan)null;
            rowNub = 1;
            using var unitWork = _unitOfWorkManager.Begin(true, false);
            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(importKey, dataList.IndexOf(item));
                newModel = _service.FirstOrDefault(z => z.RepairTagId == repairTagId && z.Id == item.Id);
                if (newModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":对象不存在");
                }
                if (!(newModel.State == (int)Enumer.YearMonthPlanState.未提交 || newModel.State == (int)Enumer.YearMonthPlanState.审核驳回)) throw new UserFriendlyException("行" + rowNub + ":当前审核状态不正确");
                if (newModel.Year != year)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":不属于" + year + "年计划");
                }
                if (newModel.ResponsibleUnit != orgId)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":非当前组织机构的数据");
                }
                newModel.EquipmentLocation = item.EquipmentLocation;
                newModel.SkyligetType = item.SkyligetType;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    newModel.CompiledOrganization = item.CompiledOrganization;
                }
                newModel.Times = item.Times;
                newModel.IsImport = true;
                newModel.Total = item.Total;
                newModel.PlanCount = item.PlanCount;
                newModel.Col_1 = item.Col_1;
                newModel.Col_2 = item.Col_2;
                newModel.Col_3 = item.Col_3;
                newModel.Col_4 = item.Col_4;
                newModel.Col_5 = item.Col_5;
                newModel.Col_6 = item.Col_6;
                newModel.Col_7 = item.Col_7;
                newModel.Col_8 = item.Col_8;
                newModel.Col_9 = item.Col_9;
                newModel.Col_10 = item.Col_10;
                newModel.Col_11 = item.Col_11;
                newModel.Col_12 = item.Col_12;
                newModel.RepairTagId = repairTagId;
                await _service.UpdateAsync(newModel);
            }
            await unitWork.CompleteAsync();
            await _fileImport.Complete(importKey);
        }
        #endregion

        #region 月表导入数据解悉
        private async Task ImportToMonthPlanData(DataTable dt, int year, Guid orgId, string importKey, Guid? repairTagId, string? repairTagKey)
        {
            var dataList = new List<YearMonthPlanDto>();
            var planModel = (YearMonthPlanDto)null;
            var id = Guid.Empty;
            var numb = (decimal)0;
            var celVal = (string)null;

            if (!dt.Columns.Contains(MonthPlanExcelCol.Id.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.Id.ToString());
            }
            if (!dt.Columns.Contains(MonthPlanExcelCol.设备处所.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.设备处所.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.编制执行单位.ToString()) && repairTagKey == "RepairTag.RailwayWired")
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.编制执行单位.ToString());
            }
            if (!dt.Columns.Contains(MonthPlanExcelCol.天窗类型.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.天窗类型.ToString());
            }
            if (!dt.Columns.Contains(MonthPlanExcelCol.数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.数量.ToString());
            }
            for (var i = 1; i <= 31; i++)
            {
                if (!dt.Columns.Contains(i.ToString()))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("表格中不存在列" + i.ToString());
                }
            }

            var repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.Where(z => z.TagId == repairTagId && z.IsMonth == true).ToList());//获得所有月度维修项
            if (repairList.Count == 0)
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("未找到月维修项");
            }

            #region 数据赋值并验证
            var rowNub = (int)1;
            foreach (DataRow row in dt.Rows)
            {
                rowNub++;
                planModel = new YearMonthPlanDto();
                //主键
                celVal = row[MonthPlanExcelCol.Id.ToString()].ToString();
                if (!Guid.TryParse(celVal, out id))
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.Id.ToString(), "主键错误", importKey);
                }
                planModel.Id = id;

                //设备处所
                celVal = row[MonthPlanExcelCol.设备处所.ToString()].ToString();
                planModel.EquipmentLocation = celVal;

                //编制执行单位
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    celVal = row[YearPlanExcelCol.编制执行单位.ToString()].ToString();
                    planModel.CompiledOrganization = celVal;
                }

                //总数量
                celVal = row[MonthPlanExcelCol.数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb))
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.数量.ToString(), "为空或非数字", importKey);
                }
                if (numb < 0)
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.数量.ToString(), "需大于0", importKey);
                }
                planModel.Total = Math.Round(numb, 3);

                //为1-31列赋值并判断
                for (var i = 1; i <= 31; i++)
                {
                    celVal = row[i.ToString()].ToString();
                    if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, i.ToString(), "为空或非数字", importKey);
                    if (numb < 0) ShowImportMsg(rowNub, i.ToString(), "需大于0", importKey);
                    SetColVal(planModel, i, Math.Round(numb, 3));
                }


                //天窗类型
                celVal = row[YearPlanExcelCol.天窗类型.ToString()].ToString();
                if (!string.IsNullOrWhiteSpace(celVal))
                {
                    var skyTypeEum = SnAbp.CrPlan.Enumer.SkyligetType.其他;
                    var skyArr = celVal.Trim(',').Split(',');
                    foreach (var item in skyArr)
                    {
                        if (!Enum.TryParse(item, out skyTypeEum))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ":天窗类型不正确(多个类型用英文','分隔)");
                        }
                    }
                    planModel.SkyligetType = celVal;
                }

                var monthTotal = (decimal)0;//所有列的总和
                for (var i = 1; i <= 31; i++)
                {
                    monthTotal += GetColVal(planModel, i);
                }

                //数量大于0时，设备处所不能为空
                if (monthTotal > 0 && string.IsNullOrWhiteSpace(planModel.EquipmentLocation))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":设备处所不能为空");
                }

                //数量大于0时，天窗类型不能为空
                if (monthTotal > 0 && string.IsNullOrWhiteSpace(planModel.SkyligetType))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":天窗类型不能为空");
                }

                var oldPlanModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == planModel.Id));
                if (oldPlanModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的计划不存在");
                }
                var repairModel = repairList.FirstOrDefault(z => z.Id == oldPlanModel.RepairDetailsId);
                if (repairModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的维修项已不存在");
                }
                planModel.Times = repairModel.Period;
                if (decimal.TryParse(repairModel.Period, out numb))//每年次数为数字
                {
                    //总数量
                    if (numb > 0)
                    {
                        if (monthTotal != planModel.Total * numb)
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ": 每日数量应等于总数量*次数");
                        }
                    }
                }
                planModel.RepairTagId = repairTagId;
                dataList.Add(planModel);
            }
            #endregion

            //更新保存数据
            rowNub = 1;
            using var unitWork = _unitOfWorkManager.Begin(true, false);
            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(importKey, dataList.IndexOf(item));
                var thisModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == item.Id));
                if (thisModel.ResponsibleUnit != orgId)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":非当前组织机构的数据");
                }
                var newModelList = _service.Where(z => z.RepairTagId == repairTagId && z.RepairDetailsId == thisModel.RepairDetailsId && z.Year == thisModel.Year && z.ResponsibleUnit == thisModel.ResponsibleUnit);
                foreach (var ele in newModelList)
                {
                    if (ele == null)
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ":对象不存在");
                    }
                    if (!(ele.State == (int)Enumer.YearMonthPlanState.未提交 || ele.State == (int)Enumer.YearMonthPlanState.审核驳回)) throw new UserFriendlyException("行" + rowNub + ":当前审核状态不正确");
                    if (ele.Year != year)
                    {
                        await _fileImport.Cancel(importKey);
                        throw new UserFriendlyException("行" + rowNub + ":不属于" + year + "年计划");
                    }
                    //当月有多少天
                    var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(ele.Year, ele.Month);
                    ele.EquipmentLocation = item.EquipmentLocation;
                    if (repairTagKey == "RepairTag.RailwayWired")
                    {
                        ele.CompiledOrganization = item.CompiledOrganization;
                    }
                    ele.SkyligetType = item.SkyligetType;
                    ele.Total = item.Total;
                    ele.Times = item.Times;
                    //为1-31列赋值
                    for (var i = 1; i <= monthDays; i++)
                    {
                        SetColVal(ele, i, GetColVal(item, i));
                    }
                    ele.IsImport = true;
                    ele.RepairTagId = repairTagId;
                    await _service.UpdateAsync(ele);
                }
            }
            await unitWork.CompleteAsync();
            await _fileImport.Complete(importKey);
        }

        private void ShowImportMsg(int row, string cel, string msg, string importKey)
        {
            _fileImport.Cancel(importKey);
            throw new UserFriendlyException(string.Format("第{0}行{1}列:{2}", row, cel, msg));
        }
        #endregion

        #region 年度月表导入数据解悉
        private async Task ImportToMonthOfYearPlanData(DataTable dt, int year, int month, Guid orgId, string importKey, Guid? repairTagId, string? repairTagKey)
        {
            var dataList = new List<YearMonthPlanDto>();
            var planModel = (YearMonthPlanDto)null;
            var id = Guid.Empty;
            var numb = (decimal)0;
            var celVal = (string)null;

            //当月有多少天
            var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(year, month);

            if (!dt.Columns.Contains(MonthOfYearPlanExcelCol.Id.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthOfYearPlanExcelCol.Id.ToString());
            }
            if (!dt.Columns.Contains(MonthOfYearPlanExcelCol.设备处所.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthOfYearPlanExcelCol.设备处所.ToString());
            }
            if (!dt.Columns.Contains(MonthOfYearPlanExcelCol.编制执行单位.ToString()) && repairTagKey == "RepairTag.RailwayWired")
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthOfYearPlanExcelCol.编制执行单位.ToString());
            }
            if (!dt.Columns.Contains(MonthOfYearPlanExcelCol.天窗类型.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthOfYearPlanExcelCol.天窗类型.ToString());
            }
            if (!dt.Columns.Contains(MonthOfYearPlanExcelCol.数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthOfYearPlanExcelCol.数量.ToString());
            }
            for (var i = 1; i <= monthDays; i++)
            {
                if (!dt.Columns.Contains(i.ToString()))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("表格中不存在列" + i.ToString());
                }
            }

            #region 数据赋值并验证
            var rowNub = (int)1;
            foreach (DataRow row in dt.Rows)
            {
                rowNub++;
                planModel = new YearMonthPlanDto();
                //主键
                celVal = row[MonthOfYearPlanExcelCol.Id.ToString()].ToString();
                if (!Guid.TryParse(celVal, out id))
                {
                    ShowImportMsg(rowNub, MonthOfYearPlanExcelCol.Id.ToString(), "主键错误", importKey);
                }
                planModel.Id = id;

                //设备处所
                celVal = row[MonthOfYearPlanExcelCol.设备处所.ToString()].ToString();
                planModel.EquipmentLocation = celVal;

                //编制执行单位
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    celVal = row[MonthOfYearPlanExcelCol.编制执行单位.ToString()].ToString();
                    planModel.CompiledOrganization = celVal;
                }

                //总数量
                celVal = row[MonthOfYearPlanExcelCol.数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb))
                {
                    ShowImportMsg(rowNub, MonthOfYearPlanExcelCol.数量.ToString(), "为空或非数字", importKey);
                }
                if (numb < 0)
                {
                    ShowImportMsg(rowNub, MonthOfYearPlanExcelCol.数量.ToString(), "需大于0", importKey);
                }
                planModel.Total = Math.Round(numb, 3);

                //日1-31赋值
                for (var i = 1; i <= monthDays; i++)
                {
                    celVal = row[i.ToString()].ToString();
                    if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, i.ToString(), "为空或非数字", importKey);
                    if (numb < 0) ShowImportMsg(rowNub, i.ToString(), "需大于0", importKey);
                    SetColVal(planModel, i, Math.Round(numb, 3));
                }

                //年表月度计划不需要导入天窗类型
                //celVal = row[YearPlanExcelCol.天窗类型.ToString()].ToString();
                //if (!string.IsNullOrWhiteSpace(celVal))
                //{
                //    var skyTypeEum = SnAbp.CrPlan.Enumer.SkyligetType.其它;
                //    var skyArr = celVal.Trim(',').Split(',');
                //    foreach (var item in skyArr)
                //    {
                //        if (!Enum.TryParse(item, out skyTypeEum))
                //        {
                //            throw new UserFriendlyException("行" + rowNub + ":天窗类型不正确");
                //        }
                //    }
                //    planModel.SkyligetType = celVal;
                //}

                var monthTotal = (decimal)0;//所有列的总和
                for (var i = 1; i <= monthDays; i++)
                {
                    monthTotal += GetColVal(planModel, i);
                }

                //数量大于0时，设备处所不能为空
                if (monthTotal > 0 && string.IsNullOrWhiteSpace(planModel.EquipmentLocation))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":设备处所不能为空");
                }

                if (planModel.Total != monthTotal)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":数量与每日数量和不等");
                }
                planModel.RepairTagId = repairTagId;
                dataList.Add(planModel);
            }
            #endregion

            //更新保存数据
            rowNub = 1;
            using var unitWork = _unitOfWorkManager.Begin(true, false);
            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(importKey, dataList.IndexOf(item));
                var newModel = _service.FirstOrDefault(z => z.RepairTagId == repairTagId && z.Id == item.Id);
                if (newModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":对象不存在");
                }
                if (!(newModel.State == (int)Enumer.YearMonthPlanState.未提交 || newModel.State == (int)Enumer.YearMonthPlanState.审核驳回))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":当前审核状态不正确");
                }
                if (newModel.Year != year || newModel.Month != month)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException(string.Format("行{0}:不属于{1}年{2}月计划", rowNub, year, month));
                }
                if (newModel.ResponsibleUnit != orgId)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":非当前组织机构的数据");
                }
                var oldParent = _service.FirstOrDefault(z => z.Id == newModel.ParentId);
                if (oldParent == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":年表对象不存在");
                }
                //获得年表的总数量
                var yearTotal = GetColVal(oldParent, month);
                if (yearTotal != item.Total)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":总数量与年表总数量不相等");
                }

                newModel.EquipmentLocation = item.EquipmentLocation;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    newModel.CompiledOrganization = item.CompiledOrganization;
                }

                newModel.Total = item.Total;

                //为1-31列赋值
                for (var i = 1; i <= monthDays; i++)
                {
                    SetColVal(newModel, i, GetColVal(item, i));
                }
                newModel.IsImport = true;
                newModel.RepairTagId = repairTagId;
                await _service.UpdateAsync(newModel);
            }
            await unitWork.CompleteAsync();
            await _fileImport.Complete(importKey);
        }
        #endregion
        #endregion

        #region 年月表变更导出EXCEL====================================
        /// <summary>
        /// 年月表变更导出EXCEL
        /// </summary>
        [Produces("application/octet-stream")]
        public async Task<Stream> DownLoadChange(YearMonthExportDto input)
        {
            Stream st = null;
            byte[] sbuf;
            var dt = (DataTable)null;
            var col = (DataColumn)null;
            var row = (DataRow)null;

            var stationTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == stationTypeEum.ToString())
            {
                throw new UserFriendlyException("导出类型不正确");
            }
            var dto = new YearMonthGetChangeDto()
            {
                PlanType = input.PlanType,
                OrgId = input.OrgId,
                RepairTagKey = input.RepairTagKey,
                IsCreateRecord = input.IsCreateRecord,
                AlterRecordId = input.AlterRecordId
            };

            var dataList = await GetOwnsChangPlan(dto);

            //var alterPlanId = _alterPlanService.Where(x => x.YearMonthAlterRecordId == input.YearMonthAlterRecordId).Select(x=>x.PlanId).ToList();

            //var dataList = _service.Where(x => alterPlanId.Contains(x.Id)).ToList();

            if (dataList.Count == 0)
            {
                throw new UserFriendlyException("未找到任何数据");
            }
            await Task.Run(() =>
            {
                switch (input.PlanType)
                {
                    case (int)Enumer.YearMonthPlanType.年表:
                        {
                            dt = new DataTable();
                            //添加列表
                            var enumValues = Enum.GetValues(typeof(Enumer.YearPlanExcelCol));
                            if (enumValues.Length > 0)
                            {
                                foreach (int item in enumValues)
                                {
                                    col = new DataColumn(Enum.GetName(typeof(Enumer.YearPlanExcelCol), item));
                                    if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && col.ColumnName == "编制执行单位")
                                        continue;
                                    dt.Columns.Add(col);
                                }
                            }
                            //添加数据
                            foreach (var item in dataList)
                            {
                                row = dt.NewRow();
                                row[YearPlanExcelCol.Id.ToString()] = item.Id;
                                row[YearPlanExcelCol.序号.ToString()] = item.Number;
                                row[YearPlanExcelCol.维修类别.ToString()] = EnumHelper.GetDescription((RepairType)item.RepairType);
                                row[YearPlanExcelCol.设备名称.ToString()] = item.DeviceName;
                                row[YearPlanExcelCol.执行单位.ToString()] = item.ExecutableUnitStr;
                                if (input.RepairTagKey == "RepairTag.RailwayWired")
                                {
                                    row[YearPlanExcelCol.编制执行单位.ToString()] = item.CompiledOrganization;
                                }
                                row[YearPlanExcelCol.设备处所.ToString()] = item.EquipmentLocation;
                                row[YearPlanExcelCol.工作内容.ToString()] = item.RepairContent;
                                row[YearPlanExcelCol.天窗类型.ToString()] = item.SkyligetType;
                                row[YearPlanExcelCol.单位.ToString()] = item.Unit;
                                row[YearPlanExcelCol.总设备数量.ToString()] = item.Total;
                                row[YearPlanExcelCol.年计划总数量.ToString()] = item.PlanCount;
                                row[YearPlanExcelCol.每年次数.ToString()] = item.Times;
                                row[YearPlanExcelCol.一月.ToString()] = item.Col_1;
                                row[YearPlanExcelCol.二月.ToString()] = item.Col_2;
                                row[YearPlanExcelCol.三月.ToString()] = item.Col_3;
                                row[YearPlanExcelCol.四月.ToString()] = item.Col_4;
                                row[YearPlanExcelCol.五月.ToString()] = item.Col_5;
                                row[YearPlanExcelCol.六月.ToString()] = item.Col_6;
                                row[YearPlanExcelCol.七月.ToString()] = item.Col_7;
                                row[YearPlanExcelCol.八月.ToString()] = item.Col_8;
                                row[YearPlanExcelCol.九月.ToString()] = item.Col_9;
                                row[YearPlanExcelCol.十月.ToString()] = item.Col_10;
                                row[YearPlanExcelCol.十一月.ToString()] = item.Col_11;
                                row[YearPlanExcelCol.十二月.ToString()] = item.Col_12;
                                dt.Rows.Add(row);
                            }
                            sbuf = ExcelHepler.DataTableToExcel(dt, "年变更计划.xlsx", new List<int>() { 0 });
                            st = new MemoryStream(sbuf);
                            break;
                        }
                    case (int)Enumer.YearMonthPlanType.月表:
                        {
                            dt = new DataTable();

                            //添加列表
                            var enumValues = Enum.GetValues(typeof(Enumer.MonthPlanExcelCol));
                            if (enumValues.Length > 0)
                            {
                                foreach (int item in enumValues)
                                {
                                    col = new DataColumn(Enum.GetName(typeof(Enumer.MonthPlanExcelCol), item));
                                    if (input.RepairTagKey == "RepairTag.RailwayHighSpeed" && col.ColumnName == "编制执行单位")
                                        continue;
                                    dt.Columns.Add(col);
                                }
                                for (var i = 1; i <= 31; i++)//1-31天
                                {
                                    dt.Columns.Add(i.ToString());
                                }
                            }
                            //添加数据
                            foreach (var item in dataList)
                            {
                                row = dt.NewRow();
                                row[MonthPlanExcelCol.Id.ToString()] = item.Id;
                                row[MonthPlanExcelCol.序号.ToString()] = item.Number;
                                row[MonthPlanExcelCol.维修类别.ToString()] = EnumHelper.GetDescription((RepairType)item.RepairType);
                                row[MonthPlanExcelCol.设备名称.ToString()] = item.DeviceName;
                                if (input.RepairTagKey == "RepairTag.RailwayWired")
                                {
                                    row[YearPlanExcelCol.编制执行单位.ToString()] = item.CompiledOrganization;
                                }
                                row[MonthPlanExcelCol.设备处所.ToString()] = item.EquipmentLocation;
                                row[MonthPlanExcelCol.工作内容.ToString()] = item.RepairContent;
                                row[MonthPlanExcelCol.天窗类型.ToString()] = item.SkyligetType;
                                row[MonthPlanExcelCol.单位.ToString()] = item.Unit;
                                row[MonthPlanExcelCol.数量.ToString()] = item.Total;
                                row[MonthPlanExcelCol.每月次数.ToString()] = item.Times;

                                for (var i = 1; i <= 31; i++)//获取值到DataTable
                                {
                                    row[i.ToString()] = GetColVal(item, i);
                                }
                                dt.Rows.Add(row);
                            }
                            sbuf = ExcelHepler.DataTableToExcel(dt, "月表变更计划.xlsx", new List<int>() { 0 });
                            st = new MemoryStream(sbuf);
                            break;
                        }
                    default:
                        {
                            throw new UserFriendlyException("导入计划类型不正确");
                        }
                }
            });
            return st;
        }
        #endregion

        #region 年月表变更导入EXCEL====================================
        /// <summary>
        /// 年月表变更导入EXCEL
        /// </summary>
        public async Task<bool> UploadChange([FromForm] ImportData input)
        {
            await _fileImport.Start(input.ImportKey, 0);
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            DataTable dt = null;
            await Task.Run(async () =>
            {
                if (input.File == null)
                {
                    await _fileImport.Cancel(input.ImportKey);
                    throw new UserFriendlyException("上传文件不能为空");
                }
                if (input.File.File == null)
                {
                    await _fileImport.Cancel(input.ImportKey);
                    throw new UserFriendlyException("上传文件不能为空");
                }
                try
                {
                    dt = ExcelHepler.ExcelToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, 1, null);//读取EXCEL
                }
                catch (Exception)
                {
                    await _fileImport.Cancel(input.ImportKey);
                    throw new UserFriendlyException("所选文件有错误，请重新选择");
                }
                if (dt == null)
                {
                    await _fileImport.Cancel(input.ImportKey);
                    throw new UserFriendlyException("未读取到任何数据");
                }
                if (dt.Rows.Count == 0)
                {
                    await _fileImport.Cancel(input.ImportKey);
                    throw new UserFriendlyException("未读取到任何数据");
                }

                switch (input.PlanType)
                {
                    case (int)Enumer.YearMonthPlanType.年表:
                        {
                            await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
                            await ImportToYearChangeAsync(dt, input.OrgId, input.ImportKey, RepairTagId, input.RepairTagKey);
                            break;
                        }
                    case (int)Enumer.YearMonthPlanType.月表:
                        {
                            await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
                            await ImportToMonthChangeAsync(dt, input.OrgId, input.ImportKey, RepairTagId, input.RepairTagKey);
                            break;
                        }
                    default:
                        {
                            await _fileImport.Cancel(input.ImportKey);
                            throw new UserFriendlyException("导入计划类型不正确");
                        }
                }
            });
            return true;
        }

        /// <summary>
        /// 年表变更导入
        /// </summary>
        private async Task ImportToYearChangeAsync(DataTable dt, Guid orgId, string importKey, Guid? repairTagId, string repairTagKey)
        {
            var dataList = new List<YearMonthPlanAlterDto>();
            var planModel = (YearMonthPlanAlterDto)null;
            var id = Guid.Empty;
            var numb = (decimal)0;
            var celVal = (string)null;
            var nowYear = DateTime.Now.Year;

            if (!dt.Columns.Contains(YearPlanExcelCol.Id.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.Id.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.总设备数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.总设备数量.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.年计划总数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.年计划总数量.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.每年次数.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.每年次数.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.一月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.一月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.二月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.二月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.三月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.三月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.四月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.四月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.五月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.五月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.六月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.六月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.七月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.七月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.八月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.八月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.九月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.九月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十一月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十一月.ToString());
            }
            if (!dt.Columns.Contains(YearPlanExcelCol.十二月.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + YearPlanExcelCol.十二月.ToString());
            }
            var repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.Where(z => z.TagId == repairTagId && z.IsMonth == false).ToList());//获得所有月度维修项
            if (repairList.Count == 0)
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("未找到月维修项");
            }

            #region 数据验证并赋值
            var rowNub = (int)1;
            foreach (DataRow row in dt.Rows)
            {
                rowNub++;
                planModel = new YearMonthPlanAlterDto();
                //主键
                celVal = row[YearPlanExcelCol.Id.ToString()].ToString();
                if (!Guid.TryParse(celVal, out id)) ShowImportMsg(rowNub, YearPlanExcelCol.Id.ToString(), "主键错误", importKey);
                planModel.Id = id;

                //总数量
                celVal = row[YearPlanExcelCol.总设备数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.总设备数量.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.总设备数量.ToString(), "需大于0", importKey);
                planModel.Total = Math.Round(numb, 3);

                //年计划数量
                celVal = row[YearPlanExcelCol.年计划总数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.年计划总数量.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.年计划总数量.ToString(), "需大于0", importKey);
                planModel.PlanCount = Math.Round(numb, 3);

                //月份
                celVal = row[YearPlanExcelCol.一月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.一月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.一月.ToString(), "需大于0", importKey);
                planModel.Col_1 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.二月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.二月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.二月.ToString(), "需大于0", importKey);
                planModel.Col_2 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.三月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.三月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.三月.ToString(), "需大于0", importKey);
                planModel.Col_3 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.四月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.四月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.四月.ToString(), "需大于0", importKey);
                planModel.Col_4 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.五月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.五月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.五月.ToString(), "需大于0", importKey);
                planModel.Col_5 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.六月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.六月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.六月.ToString(), "需大于0", importKey);
                planModel.Col_6 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.七月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.七月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.七月.ToString(), "需大于0", importKey);
                planModel.Col_7 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.八月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.八月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.八月.ToString(), "需大于0", importKey);
                planModel.Col_8 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.九月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.九月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.九月.ToString(), "需大于0", importKey);
                planModel.Col_9 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十月.ToString(), "需大于0", importKey);
                planModel.Col_10 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十一月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十一月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十一月.ToString(), "需大于0", importKey);
                planModel.Col_11 = Math.Round(numb, 3);

                celVal = row[YearPlanExcelCol.十二月.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, YearPlanExcelCol.十二月.ToString(), "为空或非数字", importKey);
                if (numb < 0) ShowImportMsg(rowNub, YearPlanExcelCol.十二月.ToString(), "需大于0", importKey);
                planModel.Col_12 = Math.Round(numb, 3);

                var yearTotal = planModel.Col_1 + planModel.Col_2 + planModel.Col_3 + planModel.Col_4 + planModel.Col_5 + planModel.Col_6 + planModel.Col_7 + planModel.Col_8 + planModel.Col_9 + planModel.Col_10 + planModel.Col_11 + planModel.Col_12;

                var alterModel = ObjectMapper.Map<YearMonthPlanAlter, YearMonthPlanAlterDto>(_alterPlanService.FirstOrDefault(z => z.Id == planModel.Id));
                if (alterModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的变更计划不存在");
                }
                var oldPlanModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == alterModel.PlanId));
                if (oldPlanModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的计划不存在");
                }
                var repairModel = repairList.FirstOrDefault(z => z.Id == oldPlanModel.RepairDetailsId);
                if (repairModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的维修项已不存在");
                }

                if (yearTotal != planModel.PlanCount)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 年计划数量应等于各月份之和");
                }
                //if (decimal.TryParse(repairModel.Period, out numb))//每年次数为数字
                //{
                //    //总数量
                //    if (numb > 0)
                //    {
                //        if (planModel.PlanCount != planModel.Total * numb)
                //        {
                //            await _fileImport.Cancel(importKey);
                //            throw new UserFriendlyException("行" + rowNub + ": 年计划数量应等于总数量*次数");
                //        }
                //    }
                //}

                //天窗类型
                celVal = row[YearPlanExcelCol.天窗类型.ToString()].ToString();
                if (!string.IsNullOrWhiteSpace(celVal))
                {
                    var skyTypeEum = SkyligetType.其他;
                    var skyArr = celVal.Trim(',').Split(',');
                    foreach (var item in skyArr)
                    {
                        if (!Enum.TryParse(item, out skyTypeEum))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ":天窗类型不正确(多个类型用英文','分隔)");
                        }
                    }
                    planModel.SkyligetType = celVal;
                }
                else
                {
                    throw new UserFriendlyException("行" + rowNub + ":天窗类型不能为空");
                }

                //设备处所
                celVal = row[MonthOfYearPlanExcelCol.设备处所.ToString()].ToString();
                planModel.EquipmentLocation = celVal;

                //执行单位
                celVal = row[MonthOfYearPlanExcelCol.执行单位.ToString()].ToString();
                planModel.ExecutableUnitStr = celVal;

                //编制执行单位
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    celVal = row[MonthOfYearPlanExcelCol.编制执行单位.ToString()].ToString();
                    planModel.CompiledOrganization = celVal;
                }

                var monthTotal = (decimal)0;//所有列的总和
                for (var i = 1; i <= 31; i++)
                {
                    monthTotal += GetColVal(planModel, i);
                }

                //数量大于0时，设备处所不能为空
                if (monthTotal > 0 && string.IsNullOrWhiteSpace(planModel.EquipmentLocation))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":设备处所不能为空");
                }
                dataList.Add(planModel);
            }
            #endregion

            //更新保存数据
            var newModel = (YearMonthPlanAlter)null;
            rowNub = 1;
            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(importKey, dataList.IndexOf(item));
                newModel = _alterPlanService.FirstOrDefault(z => z.Id == item.Id);
                if (newModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":对象不存在");
                }
                if (!(newModel.State == (int)Enumer.YearMonthPlanState.未提交 || newModel.State == (int)Enumer.YearMonthPlanState.审核驳回))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":当前审核状态不正确");
                }
                if (newModel.ExecYear != nowYear)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":不属于" + nowYear + "年变更计划");
                }
                var unint = (from a in _service.Where(s => s.RepairTagId == repairTagId)
                             join b in _alterPlanService.Where(s => s.RepairTagId == repairTagId)
                             on a.Id equals b.PlanId
                             where b.Id == item.Id
                             select new { a.ResponsibleUnit }).FirstOrDefault();
                if (unint?.ResponsibleUnit != orgId)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":非当前组织机构的数据");
                }
                newModel.Total = item.Total;
                newModel.PlanCount = item.PlanCount;
                newModel.SkyligetType = item.SkyligetType;
                newModel.EquipmentLocation = item.EquipmentLocation;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    newModel.CompiledOrganization = item.CompiledOrganization;
                }
                newModel.Col_1 = item.Col_1;
                newModel.Col_2 = item.Col_2;
                newModel.Col_3 = item.Col_3;
                newModel.Col_4 = item.Col_4;
                newModel.Col_5 = item.Col_5;
                newModel.Col_6 = item.Col_6;
                newModel.Col_7 = item.Col_7;
                newModel.Col_8 = item.Col_8;
                newModel.Col_9 = item.Col_9;
                newModel.Col_10 = item.Col_10;
                newModel.Col_11 = item.Col_11;
                newModel.Col_12 = item.Col_12;
                newModel.IsImport = true;
                newModel.RepairTagId = repairTagId;
                await _alterPlanService.UpdateAsync(newModel);
            }
            await _fileImport.Complete(importKey);
        }

        /// <summary>
        /// 月表变更导入
        /// </summary>
        private async Task ImportToMonthChangeAsync(DataTable dt, Guid orgId, string importKey, Guid? repairTagId, string repairTagKey)
        {
            var dataList = new List<YearMonthPlanAlterDto>();
            var planModel = (YearMonthPlanAlterDto)null;
            var id = Guid.Empty;
            var numb = (decimal)0;
            var celVal = (string)null;
            var nowYear = DateTime.Now.Year;

            if (!dt.Columns.Contains(MonthPlanExcelCol.Id.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.Id.ToString());
            }
            if (!dt.Columns.Contains(MonthPlanExcelCol.数量.ToString()))
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("表格中不存在列" + MonthPlanExcelCol.数量.ToString());
            }
            for (var i = 1; i <= 31; i++)
            {
                if (!dt.Columns.Contains(i.ToString()))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("表格中不存在列" + i.ToString());
                }
            }

            var repairList = ObjectMapper.Map<List<RepairItem>, List<RepairItemDto>>(_repariService.Where(z => z.TagId == repairTagId && z.IsMonth == true).ToList());//获得所有月度维修项
            if (repairList.Count == 0)
            {
                await _fileImport.Cancel(importKey);
                throw new UserFriendlyException("未找到月维修项");
            }

            #region 数据赋值并验证
            var rowNub = (int)1;
            foreach (DataRow row in dt.Rows)
            {
                rowNub++;
                planModel = new YearMonthPlanAlterDto();
                //主键
                celVal = row[MonthPlanExcelCol.Id.ToString()].ToString();
                if (!Guid.TryParse(celVal, out id))
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.Id.ToString(), "主键错误", importKey);
                }
                planModel.Id = id;

                //总数量
                celVal = row[MonthPlanExcelCol.数量.ToString()].ToString();
                if (!decimal.TryParse(celVal, out numb))
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.数量.ToString(), "为空或非数字", importKey);
                }
                if (numb < 0)
                {
                    ShowImportMsg(rowNub, MonthPlanExcelCol.数量.ToString(), "需大于0", importKey);
                }
                planModel.Total = Math.Round(numb, 3);

                //为1-31列赋值并判断
                for (var i = 1; i <= 31; i++)
                {
                    celVal = row[i.ToString()].ToString();
                    if (!decimal.TryParse(celVal, out numb)) ShowImportMsg(rowNub, i.ToString(), "为空或非数字", importKey);
                    if (numb < 0) ShowImportMsg(rowNub, i.ToString(), "需大于0", importKey);
                    SetColVal(planModel, i, Math.Round(numb, 3));
                }

                var monthTotal = (decimal)0;//所有列的总和
                for (var i = 1; i <= 31; i++)
                {
                    monthTotal += GetColVal(planModel, i);
                }
                var alterModel = ObjectMapper.Map<YearMonthPlanAlter, YearMonthPlanAlterDto>(_alterPlanService.FirstOrDefault(z => z.Id == planModel.Id));
                if (alterModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的变更计划不存在");
                }
                var oldPlanModel = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == alterModel.PlanId));
                if (oldPlanModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的计划不存在");
                }
                var repairModel = repairList.FirstOrDefault(z => z.Id == oldPlanModel.RepairDetailsId);
                if (repairModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ": 导入的维修项已不存在");
                }
                if (decimal.TryParse(repairModel.Period, out numb))//每年次数为数字
                {
                    //总数量
                    if (numb > 0)
                    {
                        if (monthTotal != planModel.Total * numb)
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ": 每日数量应等于总数量*次数");
                        }
                    }
                }

                //天窗类型
                celVal = row[YearPlanExcelCol.天窗类型.ToString()].ToString();
                if (!string.IsNullOrWhiteSpace(celVal))
                {
                    var skyTypeEum = SkyligetType.其他;
                    var skyArr = celVal.Trim(',').Split(',');
                    foreach (var item in skyArr)
                    {
                        if (!Enum.TryParse(item, out skyTypeEum))
                        {
                            await _fileImport.Cancel(importKey);
                            throw new UserFriendlyException("行" + rowNub + ":天窗类型不正确(多个类型用英文','分隔)");
                        }
                    }
                    planModel.SkyligetType = celVal;
                }

                //设备处所
                celVal = row[MonthOfYearPlanExcelCol.设备处所.ToString()].ToString();
                planModel.EquipmentLocation = celVal;

                //执行单位
                celVal = row[MonthOfYearPlanExcelCol.执行单位.ToString()].ToString();
                planModel.ExecutableUnitStr = celVal;

                //编制执行单位
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    celVal = row[MonthOfYearPlanExcelCol.编制执行单位.ToString()].ToString();
                    planModel.CompiledOrganization = celVal;
                }

                //数量大于0时，设备处所不能为空
                if (monthTotal > 0 && string.IsNullOrWhiteSpace(planModel.EquipmentLocation))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":设备处所不能为空");
                }
                dataList.Add(planModel);
            }
            #endregion

            //更新保存数据
            rowNub = 1;

            foreach (var item in dataList)
            {
                await _fileImport.UpdateState(importKey, dataList.IndexOf(item));
                var thisModel = _alterPlanService.FirstOrDefault(z => z.Id == item.Id);
                if (thisModel == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":对象不存在");
                }
                if (!(thisModel.State == (int)Enumer.YearMonthPlanState.未提交 || thisModel.State == (int)Enumer.YearMonthPlanState.审核驳回))
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":当前审核状态不正确");
                }
                if (thisModel.ExecYear != nowYear)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":不属于" + nowYear + "年变更计划");
                }

                var unint = (from a in _service.Where(s => s.RepairTagId == repairTagId)
                             join b in _alterPlanService.Where(s => s.RepairTagId == repairTagId)
                             on a.Id equals b.PlanId
                             where b.Id == item.Id
                             select new { a.ResponsibleUnit }).FirstOrDefault();
                if (unint?.ResponsibleUnit != orgId)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":非当前组织机构的数据");
                }
                thisModel.Total = item.Total;

                var thisPlan = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == thisModel.PlanId));//当前变更的计划
                if (thisPlan == null)
                {
                    await _fileImport.Cancel(importKey);
                    throw new UserFriendlyException("行" + rowNub + ":原计划已不存在");
                }
                //当月有多少天
                var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(thisPlan.Year, thisPlan.Month);
                //为1-31列赋值
                for (var i = 1; i <= monthDays; i++)
                {
                    SetColVal(thisModel, i, GetColVal(item, i));
                }
                thisModel.IsImport = true;
                thisModel.SkyligetType = item.SkyligetType;
                thisModel.EquipmentLocation = item.EquipmentLocation;
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    thisModel.CompiledOrganization = item.CompiledOrganization;
                }
                thisModel.RepairTagId = repairTagId;
                await _alterPlanService.UpdateAsync(thisModel);
            }
            await _fileImport.Complete(importKey);
        }
        #endregion

        #region 年月表提交审核=========================================
        /// <summary>
        /// 年月表提交审核
        /// </summary>
        public async Task<bool> SubmitForExam(YearMonthExamDto input)
        {

            if (input.Year < 2000 || input.Year > 9999) throw new UserFriendlyException("生成年份应在2000-9999间");
            switch (input.PlanType)
            {
                case (int)Enumer.YearMonthPlanType.年表:
                    {
                        await ExamToYearPlan(input);//年表提交审核业务操作
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.月表:
                    {
                        await ExamToMonthPlan(input);//月表提交审核业务操作
                        break;
                    }
                case (int)Enumer.YearMonthPlanType.年度月表:
                    {
                        await ExamToMonthYearPlan(input);//年度月表提交审核业务操作
                        break;
                    }
                default:
                    {
                        throw new UserFriendlyException("计划类型不正确");
                    }
            }
            return true;
        }

        /// <summary>
        /// 年表提交审核业务操作
        /// </summary>
        private async Task<bool> ExamToYearPlan(YearMonthExamDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var planList = _service.Where(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && (z.State == (int)Enumer.YearMonthPlanState.未提交 || z.State == (int)Enumer.YearMonthPlanState.审核驳回) && z.PlanType == input.PlanType);
            if (planList.Count() == 0) throw new UserFriendlyException("未找到需要审核的数据");


            //未导入数量
            var importCount = planList.Count(z => z.IsImport == false);
            if (importCount == planList.Count()) throw new UserFriendlyException("请导入数据后再提交审核");
            //年月表提交审核
            var examId = await ExamCreateYearMonthExam(input.OrgId, input.PlanType, input.Year, 0, RepairTagId, input.RepairTagKey);
            if (examId == Guid.Empty) throw new UserFriendlyException("审核工作流提交失败");
            foreach (var item in planList)
            {
                item.State = (int)Enumer.YearMonthPlanState.审核中;
                item.IsImport = true;
                item.AR_Key = examId;//审批id
                item.RepairTagId = RepairTagId;
                await _service.UpdateAsync(item);
            }
            return true;
        }

        /// <summary>
        /// 月表提交审核业务操作
        /// </summary>
        private async Task<bool> ExamToMonthPlan(YearMonthExamDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var planList = _service.Where(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && (z.State == (int)Enumer.YearMonthPlanState.未提交 || z.State == (int)Enumer.YearMonthPlanState.审核驳回) && z.PlanType == input.PlanType);
            if (planList.Count() == 0) throw new UserFriendlyException("未找到需要审核的数据");

            //未导入数量
            var importCount = planList.Count(z => z.IsImport == false);
            if (importCount == planList.Count()) throw new UserFriendlyException("请导入数据后再提交审核");

            //年月表提交审核
            var examId = await ExamCreateYearMonthExam(input.OrgId, input.PlanType, input.Year, 0, RepairTagId, input.RepairTagKey);
            if (examId == Guid.Empty) throw new UserFriendlyException("审核工作流提交失败");
            foreach (var item in planList)
            {
                item.State = (int)Enumer.YearMonthPlanState.审核中;
                item.IsImport = true;
                item.AR_Key = examId;
                item.RepairTagId = RepairTagId;
                await _service.UpdateAsync(item);
            }
            return true;
        }

        /// <summary>
        /// 年度月表提交审核业务操作
        /// </summary>
        private async Task<bool> ExamToMonthYearPlan(YearMonthExamDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            if (input.Month > 12 || input.Month < 1) throw new UserFriendlyException("月份不正确");
            var planList = _service.Where(z => z.RepairTagId == RepairTagId && z.ResponsibleUnit == input.OrgId && z.Year == input.Year && (z.State == (int)Enumer.YearMonthPlanState.未提交 || z.State == (int)Enumer.YearMonthPlanState.审核驳回) && z.PlanType == input.PlanType && z.Month == input.Month);
            if (planList.Count() == 0) throw new UserFriendlyException("未找到需要审核的数据");

            //未导入数量
            var importCount = planList.Count(z => z.IsImport == false);
            if (importCount == planList.Count()) throw new UserFriendlyException("请导入数据后再提交审核");

            //年月表提交审核
            var examId = await ExamCreateYearMonthExam(input.OrgId, input.PlanType, input.Year, input.Month, RepairTagId, input.RepairTagKey);
            if (examId == Guid.Empty) throw new UserFriendlyException("审核工作流提交失败");
            foreach (var item in planList)
            {
                item.State = (int)Enumer.YearMonthPlanState.审核中;
                item.IsImport = true;
                item.AR_Key = examId;
                item.RepairTagId = RepairTagId;
                await _service.UpdateAsync(item);
            }
            return true;
        }
        #endregion

        #region 年月表审核完成=========================================
        /// <summary>
        /// 年月表审核完成
        /// </summary>
        public async Task<bool> FinishForExam(Guid id, WorkflowState state)
        {
            //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == repairTagKey))?.Id;
            var planList = _service.Where(z => z.AR_Key == id).ToList();
            if (planList.Count() == 0)
            {
                throw new UserFriendlyException("未找到待审核的数据");
            }

            var validModel = planList.FirstOrDefault();
            if (!(validModel.State == (int)Enumer.YearMonthPlanState.待审核 || validModel.State == (int)Enumer.YearMonthPlanState.审核中))
            {
                throw new UserFriendlyException("未找到待审核的数据");
            }

            var planType = validModel.PlanType;//获得年月表类型
            bool isPass = false;
            switch (state)
            {
                case WorkflowState.Finished:
                    isPass = true;
                    break;
            }
            switch (planType)
            {
                case (int)YearMonthPlanType.年表:
                    {
                        if (isPass)
                        {
                            foreach (var item in planList)
                            {
                                if (!(item.State == (int)Enumer.YearMonthPlanState.审核中 ||
                                      item.State == (int)Enumer.YearMonthPlanState.审核驳回))
                                {
                                    throw new UserFriendlyException("当前状态不正确");
                                }
                                item.State = (int)YearMonthPlanState.审核通过;
                                //await _service.UpdateAsync(item);
                            }
                        }
                        else
                        {
                            await RejectForExam(id);
                        }
                        await CurrentUnitOfWork.SaveChangesAsync();
                        break;
                    }
                case (int)YearMonthPlanType.月表:
                    {
                        if (isPass)
                        {
                            await FinishToMonthPlan(planList);//月表提交审核业务操作
                        }
                        else
                        {
                            await RejectForExam(id);
                        }

                        break;
                    }
                case (int)YearMonthPlanType.年度月表:
                    {
                        if (isPass)
                        {
                            await FinishToMonthPlan(planList);//年度月表提交审核业务操作
                        }
                        else
                        {
                            await RejectForExam(id);
                        }
                        break;
                    }
                default:
                    {
                        throw new UserFriendlyException("计划类型不正确");
                    }
            }
            return true;
        }

        /// <summary>
        /// 月表完成审核业务操作
        /// </summary>
        private async Task<bool> FinishToMonthPlan(List<YearMonthPlan> planList)
        {
            var addDaliyList = new List<DailyPlan>();//要添加的日计划
            var updateDaliyList = new List<DailyPlan>();//要更新的日计划
            await Task.Run(async () =>
            {
                foreach (var item in planList)
                {
                    if (!(item.State == (int)Enumer.YearMonthPlanState.审核中 || item.State == (int)Enumer.YearMonthPlanState.审核驳回))
                    {
                        throw new UserFriendlyException("当前状态不正确");
                    }
                    item.State = (int)Enumer.YearMonthPlanState.审核通过;
                    //_service.UpdateAsync(item);
                    var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(item.Year, item.Month);//当月有多少天
                    for (var i = 1; i <= monthDays; i++)
                    {
                        var dCount = GetColVal(item, i);//获得当天的数量
                        if (dCount > 0)
                        {
                            var planDate = new DateTime(item.Year, item.Month, i);
                            var dailyPlanModel = _dailyPlanService.FirstOrDefault(z => z.PlanId == item.Id && z.PlanDate == planDate && z.PlanType == item.PlanType);
                            if (dailyPlanModel != null)
                            {
                                dailyPlanModel.Count = dCount;
                                //dailyPlanModel.RepairTagId = repairTagId;
                                updateDaliyList.Add(dailyPlanModel);
                            }
                            else
                            {
                                dailyPlanModel = new DailyPlan(Guid.NewGuid());
                                dailyPlanModel.PlanId = item.Id;
                                dailyPlanModel.PlanDate = planDate;
                                dailyPlanModel.PlanType = item.PlanType;
                                dailyPlanModel.State = 0;
                                dailyPlanModel.Count = dCount;
                                dailyPlanModel.RepairTagId = item.RepairTagId;
                                addDaliyList.Add(dailyPlanModel);
                            }
                        }
                    }
                }
                await _daliyPlanRepos.OperDaliyPlan(addDaliyList, updateDaliyList, null);
                await CurrentUnitOfWork.SaveChangesAsync(); ;
            });

            return true;
        }
        #endregion

        #region 年月表(变更)审核驳回===================================
        /// <summary>
        /// 年月表(变更)计划审核驳回
        /// </summary>
        public async Task<bool> RejectForExam(Guid examId)
        {
            //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == repairTagKey))?.Id;
            //先找年月表计划
            var planList = _service.Where(z => z.AR_Key == examId).ToList();
            if (planList.Count > 0)
            {
                foreach (var item in planList)
                {
                    if (item.State != (int)Enumer.YearMonthPlanState.审核中)
                    {
                        throw new UserFriendlyException("当前状态非" + Enumer.YearMonthPlanState.审核中.ToString());
                    }
                    item.State = (int)Enumer.YearMonthPlanState.审核驳回;
                    item.AR_Key = null;
                    //await _service.UpdateAsync(item);
                }
            }
            else
            {
                var alterList = _alterPlanService.Where(z => z.AR_Key == examId).ToList();
                if (alterList.Count > 0)
                {
                    foreach (var item in alterList)
                    {
                        item.State = (int)Enumer.YearMonthPlanState.审核驳回;
                        //await _alterPlanService.UpdateAsync(item);
                    }
                }
                else
                {
                    throw new UserFriendlyException("未找到要操作的对象");
                }
            }
            //await CurrentUnitOfWork.SaveChangesAsync();
            return true;
        }
        #endregion

        #region 年月表变更提交审核=====================================
        /// <summary>
        /// 年月表变更提交审核
        /// </summary>
        public async Task<bool> SubmitChangeForExam(YearMonthChangeExamDto input)
        {
            if (input.Month < 1 || input.Month > 12) throw new UserFriendlyException("变更月份不正确");

            var planTypeEum = (SnAbp.CrPlan.Enumer.YearMonthPlanType)Enum.Parse(typeof(SnAbp.CrPlan.Enumer.YearMonthPlanType), input.PlanType.ToString());
            if (input.PlanType.ToString() == planTypeEum.ToString())
            {
                throw new UserFriendlyException("计划类型不正确");
            }
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;

            //新增需求 

            var yearMonthAlterRecord = new YearMonthAlterRecord(_guidGenerator.Create());
            yearMonthAlterRecord.State = (int)YearMonthPlanState.审核中;
            //yearMonthAlterRecord.PlanAlterId = _guidGenerator.Create();
            yearMonthAlterRecord.PlanType = input.PlanType;
            yearMonthAlterRecord.ChangeReason = input.Remark;
            yearMonthAlterRecord.OrganizationId = input.OrgId;

            var dataList = (from a in _service.Where(s => s.RepairTagId == RepairTagId)
                            join b in _alterPlanService.Where(s => s.RepairTagId == RepairTagId)
                            on a.Id equals b.PlanId
                            where a.ResponsibleUnit == input.OrgId && b.ExecYear == DateTime.Now.Year && (b.State == (int)Enumer.YearMonthPlanState.未提交 || b.State == (int)Enumer.YearMonthPlanState.审核驳回) && b.PlanType == input.PlanType
                            select b).ToList();

            if (dataList.Count() == 0) throw new UserFriendlyException("未找到任何要审核的数据");
            //未导入数量
            var importCount = dataList.Count(z => z.IsImport == false);
            if (importCount == dataList.Count()) throw new UserFriendlyException("请导入数据后再提交审核");

            var examId = await ExamCreateYearMonthChangeExam(input);
            if (examId == Guid.Empty) throw new UserFriendlyException("审核工作流提交失败");
            yearMonthAlterRecord.ARKey = examId;

            await _yearMonthAlterRecord.InsertAsync(yearMonthAlterRecord);
            await CurrentUnitOfWork.SaveChangesAsync();

            foreach (var item in dataList)
            {
                item.State = (int)Enumer.YearMonthPlanState.审核中;
                item.IsImport = true;
                item.ChangeReason = input.Remark;
                item.ExecMonth = input.Month;
                item.AR_Key = examId;
                item.FileId = input.FileId.GetValueOrDefault();
                item.FileName = input.FileName;
                item.RepairTagId = RepairTagId;
                item.YearMonthAlterRecordId = yearMonthAlterRecord.Id;
                await _alterPlanService.UpdateAsync(item);
            }
            return true;

        }
        #endregion

        #region 年月表变更完成审核=====================================
        /// <summary>
        /// 年月表变更完成审核
        /// </summary>
        public async Task<bool> FinishChangeForExam(Guid id, WorkflowState state)
        {
            var dataList = _alterPlanService.Where(z => z.AR_Key == id).ToList();

            var alterRecord = _yearMonthAlterRecord.Where(x => x.ARKey == id).FirstOrDefault();

            if (state == WorkflowState.Finished)
            {
                alterRecord.State = (int)YearMonthPlanState.审核通过;
                //var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == repairTagKey))?.Id;
                //本次审核完成的变更计划
                if (dataList.Count() == 0) throw new UserFriendlyException("未找到任何要审核的数据");
                var addDaliyList = new List<DailyPlan>();//要添加的日计划
                var delDaliyList = new List<DailyPlan>();//要删除的日计划
                var delFlag = false;

                foreach (var item in dataList)
                {
                    if (!(item.State == (int)Enumer.YearMonthPlanState.审核中 || item.State == (int)Enumer.YearMonthPlanState.审核驳回)) throw new UserFriendlyException("当前状态不正确");
                    var exDate = new DateTime(DateTime.Now.Year, item.ExecMonth, 1, 0, 0, 0);//开始执行月份
                                                                                             //获得当前计划对象
                    var plan = ObjectMapper.Map<YearMonthPlan, YearMonthPlanDto>(_service.FirstOrDefault(z => z.Id == item.PlanId));
                    if (plan == null) throw new UserFriendlyException("变更的原计划已不存在");

                    item.State = (int)Enumer.YearMonthPlanState.审核通过;
                    //item.RepairTagId = RepairTagId;
                    await _alterPlanService.UpdateAsync(item);

                    if (item.PlanType == (int)Enumer.YearMonthPlanType.月表)//月表变更业务
                    {
                        //找到受影响的月计划(受影响月份之后)
                        var operPlanList = _service.Where(z => z.RepairTagId == plan.RepairTagId && z.ResponsibleUnit == plan.ResponsibleUnit && z.RepairDetailsId == plan.RepairDetailsId && z.Year == DateTime.Now.Year && z.Month >= item.ExecMonth).ToList();
                        foreach (var ele in operPlanList)
                        {
                            //删除变更后月份生成的日计划
                            var dailyList = _dailyPlanService.Where(z => z.PlanId == ele.Id && z.PlanDate >= exDate).ToList();
                            delDaliyList.AddRange(dailyList);//要删除的日计划数据

                            //开始变更月计划数量
                            ele.Total = item.Total;
                            ele.PlanCount = item.PlanCount;
                            ele.SkyligetType = item.SkyligetType;
                            ele.EquipmentLocation = item.EquipmentLocation;
                            //当月有多少天
                            var monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, ele.Month);
                            for (var i = 1; i <= monthDays; i++)
                            {
                                var dCount = GetColVal(item, i);
                                SetColVal(ele, i, dCount);
                                if (dCount > 0)
                                {
                                    var dailyPlanModel = new DailyPlan(Guid.NewGuid());
                                    dailyPlanModel.PlanId = ele.Id;
                                    dailyPlanModel.PlanDate = new DateTime(DateTime.Now.Year, ele.Month, i);
                                    dailyPlanModel.PlanType = ele.PlanType;
                                    dailyPlanModel.State = 0;
                                    dailyPlanModel.Count = dCount;
                                    dailyPlanModel.RepairTagId = ele.RepairTagId;
                                    addDaliyList.Add(dailyPlanModel);//要添加的日计划数据
                                                                     //await _dailyPlanService.InsertAsync(dailyPlanModel);
                                }
                            }
                            //ele.RepairTagId = RepairTagId;
                            await _service.UpdateAsync(ele);
                        }
                    }
                    else//年表变更业务
                    {
                        //找到受影响的年计划
                        var operYearPlan = _service.FirstOrDefault(z => z.RepairTagId == plan.RepairTagId && z.ResponsibleUnit == plan.ResponsibleUnit && z.Year == DateTime.Now.Year && z.Id == item.PlanId);

                        if (delFlag == false)
                        {
                            delFlag = true;
                            //找到该计划生成的年度月计划并删除(执行月份之后)
                            var monthOfYearList = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == plan.RepairTagId && z.ResponsibleUnit == operYearPlan.ResponsibleUnit && z.Year == operYearPlan.Year && z.Month >= item.ExecMonth && z.PlanType == (int)Enumer.YearMonthPlanType.年度月表).ToList());
                            foreach (var n in monthOfYearList)
                            {
                                await _service.DeleteAsync(n.Id);//删除年度月计划

                                //删除年度月计划生成的日计划
                                var dailyList = _dailyPlanService.Where(z => z.RepairTagId == plan.RepairTagId && z.PlanId == n.Id && z.PlanDate >= exDate).ToList();
                                delDaliyList.AddRange(dailyList);//要删除的日计划数据
                            }
                        }

                        //开始变更月计划数量
                        operYearPlan.Total = item.Total;
                        operYearPlan.PlanCount = item.PlanCount;
                        operYearPlan.EquipmentLocation = item.EquipmentLocation;
                        operYearPlan.SkyligetType = item.SkyligetType;
                        for (var i = item.ExecMonth; i <= 31; i++)
                        {
                            var dCount = GetColVal(item, i);
                            SetColVal(operYearPlan, i, dCount);
                        }
                        //operYearPlan.RepairTagId = RepairTagId;
                        await _service.UpdateAsync(operYearPlan);
                    }
                }
                foreach (var item in delDaliyList)
                {
                    //要删除天窗计划的id
                    var skylightIds = _planDetailsService.Where(s => s.DailyPlanId == item.Id).Select(s => s.SkylightPlanId).ToList();
                    //删除维修作业关联
                    await _maintenanceWorksRltService.DeleteAsync(s => skylightIds.Contains(s.SkylightPlanId));
                    //删除派工作业关联
                    var workorderIds = _workOrderService.Where(s => skylightIds.Contains(s.SkylightPlanId)).Select(s => s.Id).ToList();
                    await _workOrganization.DeleteAsync(s => workorderIds.Contains(s.WorkOrderId));
                    await _workOrderService.DeleteAsync(s => workorderIds.Contains(s.Id));
                    await _skylightPlanService.DeleteAsync(s => skylightIds.Contains(s.Id));
                }
                await _daliyPlanRepos.OperDaliyPlan(addDaliyList, null, delDaliyList);//操作日计划数据
            }
            else
            {
                alterRecord.State = (int)YearMonthPlanState.审核驳回;

                foreach (var item in dataList)
                {
                    item.State = (int)YearMonthPlanState.审核驳回;
                    //item.RepairTagId = RepairTagId;
                    await _alterPlanService.UpdateAsync(item);
                }
            }
            await _yearMonthAlterRecord.UpdateAsync(alterRecord);
            return true;

        }
        #endregion

        #region 获取或设置年月表计划1-31列数据=========================
        /// <summary>
        /// 设置数据
        /// </summary>
        public void SetColVal<T>(T model, int col, decimal val)
        {
            Type modelType = typeof(T);
            PropertyInfo proInfo = modelType.GetProperty("Col_" + col);
            if (proInfo != null)
            {
                proInfo.SetValue(model, val, null);//用索引值设置属性值
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        public decimal GetColVal<T>(T model, int col)
        {
            var result = (string)null;
            PropertyInfo proInfo = model.GetType().GetProperty("Col_" + col);
            if (proInfo != null)
            {
                result = proInfo.GetValue(model, null).ToString();
            }
            var val = (decimal)0;
            if (decimal.TryParse(result, out val)) return val;
            else throw new UserFriendlyException("数据获取失败");
        }
        #endregion

        #region 年月表提交审核至工作流业务=============================
        /// <summary>
        /// 年月表变更提交审核至工作流
        /// </summary>
        /// <returns></returns>
        private async Task<Guid> ExamCreateYearMonthChangeExam(YearMonthChangeExamDto input)
        {
            var key = "";
            var list = (List<YearMonthPlanAlterDto>)null;
            var tableName = "";
            var userId = CurrentUser.Id.GetValueOrDefault();
            var dto = new YearMonthGetChangeDto()
            {
                PlanType = input.PlanType,
                OrgId = input.OrgId,
                RepairTagKey = input.RepairTagKey
            };
            list = await GetOwnsChangPlan(dto);

            if (input.PlanType == (int)Enumer.YearMonthPlanType.年表)
            {
                tableName = "年表变更";
                key = "YearChangeExam";
            }
            else if (input.PlanType == (int)Enumer.YearMonthPlanType.月表)
            {
                tableName = "月表变更";
                key = "MonthChangeExam";
            }

            if (list.Count == 0) throw new UserFriendlyException("未找到任何审核数据");
            var firstModel = list.FirstOrDefault();
            if (firstModel.State == (int)Enumer.YearMonthPlanState.审核中 || firstModel.State == (int)Enumer.YearMonthPlanState.审核通过) throw new UserFriendlyException("审核状态不正确");

            // 构造数据
            var value = new JObject();
            var rows = new JArray();

            var pn = (int)0;
            foreach (var item in list.OrderBy(x => x.Number))
            {
                pn++;
                var row = new JObject();

                var nums = item.Number.Split("-");
                var newNums = "";
                foreach (var num in nums)
                {
                    newNums += int.Parse(num) + "-";
                }

                //添加执行单位
                //var repairDetails = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == input.repairTagId && z.ResponsibleUnit == orgId && z.Year == year && z.Month == month && z.PlanType == planType).OrderBy(z => z.Number).ToList());
                //if (list.Count == 0) throw new UserFriendlyException("未找到任何数据");
                //var repairIds = list.Select(s => s.RepairDetailsId);
                //var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

                //string executableUnitStr = "";
                //foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                //{
                //    if (t.OrganizationType != null)
                //        executableUnitStr += t.OrganizationType.Name + ",";
                //}
                //executableUnitStr = executableUnitStr.TrimEnd(',');

                row["序号"] = newNums.TrimEnd('-');//序号
                row["维修类别"] = EnumHelper.GetDescription((Enumer.StatisticalRepairType)item.RepairType) ?? "";//维修类别
                row["设备名称"] = item.DeviceName ?? "";//设备名称
                                                    //row["执行单位"] = executableUnitStr;//执行单位
                if (input.RepairTagKey == "RepairTag.RailwayWired")
                {
                    row["编制执行单位"] = item.CompiledOrganization ?? "";
                }
                row["设备处所"] = item.EquipmentLocation ?? "";//设备处所
                row["工作内容"] = item.RepairContent ?? "";//工作内容
                row["天窗类型"] = item.SkyligetType ?? "";//天窗类型
                row["单位"] = item.Unit ?? "";//单位
                row["总数量"] = item.Total;//总数量
                row["计划数量"] = item.PlanCount;//计划数量
                row["每年次数"] = item.Times ?? "";//每年次数

                var count = input.PlanType == (int)YearMonthPlanType.年表 ? 12 : 31;
                //为1-31列赋值
                for (var i = 1; i <= count; i++)
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                rows.Add(row);
            }



            //获得当前车间
            var orgModel = (await _orgService.Where(z => z.Id == input.OrgId)).FirstOrDefault();
            if (orgModel != null)
            {
                value["input_orgName"] = orgModel.Name;
                tableName += orgModel.Name;
            }
            value["input_planTime"] = DateTime.Now.Year + "年";

            value["input_startTime"] = input.Month + "月";//执行时间
            value["input_remark"] = input.Remark;//变更原因

            var file = await _fileManager.GetFileInfos(input.FileId.GetValueOrDefault());

            if (file == null)
            {
                value["uploadFile_file"] = null;//附件文件
            }
            //附件
            var oFile = new JObject
            {
                ["id"] = file.Id,
                ["name"] = file.Name,
                ["type"] = file.Type,
                ["url"] = file.Url,
                ["size"] = file.Size
            };

            var oFiles = new JArray { oFile };

            value["uploadFile_file"] = oFiles;//附件文件

            //将数据写入excel
            var fileJObject = await _crPlanManager.JsonToDataTable(rows, tableName, false);

            var fileArray = new JArray { fileJObject };

            value["uploadFile_" + key] = fileArray;

            var workflow = await _bpmManager.CreateWorkflowByWorkflowTemplateKey(key, value.ToString(), userId);

            if (workflow != null)
            {
                return workflow.Id;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// 年月表提交审核至工作流
        /// </summary>
        /// <returns></returns>
        private async Task<Guid> ExamCreateYearMonthExam(Guid orgId, int planType, int year, int month, Guid? repairTagId, string repairTagKey)
        {
            var key = "";
            var list = (List<YearMonthPlanDto>)null;

            //获取当前车间名称
            var orgModel = (await _orgService.Where(z => z.Id == orgId)).FirstOrDefault();
            var tableName = "";
            //当月有多少天
            var monthDays = 12;
            var dto = new YearMonthSearchDto()
            {
                RepairTagKey = repairTagKey,
                Year = year,
                Month = month,
                OrgId = orgId,
                PlanType = planType,
                IsAll = true
            };
            if (planType == (int)YearMonthPlanType.年表)
            {
                tableName = year + "年" + orgModel.Name + "年表计划";
                key = "YearPlanExam";

                dynamic yearDto = await GetSubmitedListYear(dto);
                List<YearMonthPlanYearDetailDto> plandetails = yearDto.items;

                list = ObjectMapper.Map<List<YearMonthPlanYearDetailDto>, List<YearMonthPlanDto>>(plandetails);

            }
            else if (planType == (int)Enumer.YearMonthPlanType.月表)
            {
                tableName = year + "年" + orgModel.Name + "月表计划";
                key = "MonthPlanExam";

                dynamic yearDto = await GetSubmitedListMonth(dto);
                List<YearMonthPlanMonthDetailDto> plandetails = yearDto.items;

                list = ObjectMapper.Map<List<YearMonthPlanMonthDetailDto>, List<YearMonthPlanDto>>(plandetails);
            }
            else
            {
                tableName = year + "年" + month + "月" + orgModel.Name + "年表月度计划";
                key = "MonthOfYearPlanExam";
                list = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == repairTagId && z.ResponsibleUnit == orgId && z.PlanType == planType && z.Year == year && z.Month == month).ToList());
            }

            if (list.Count == 0) throw new UserFriendlyException("未找到任何审核数据");
            var firstModel = list.FirstOrDefault();
            if (firstModel.State == (int)Enumer.YearMonthPlanState.审核中 || firstModel.State == (int)Enumer.YearMonthPlanState.审核通过) throw new UserFriendlyException("审核状态不正确");

            // 构造数据
            var value = new JObject();
            var rows = new JArray();

            //添加执行单位

            var repairIds = list.Select(s => s.RepairDetailsId);
            var relate = _reparRltTypeService.WithDetails(s => s.OrganizationType).Where(s => repairIds.Contains(s.RepairItemId)).ToList();

            var pn = 0;
            foreach (var item in list.OrderBy(x => x.Number))
            {
                pn++;
                var row = new JObject();

                var nums = item.Number.Split("-");
                var newNums = "";
                foreach (var num in nums)
                {
                    newNums += int.Parse(num) + "-";
                }

                //var repairDetails = ObjectMapper.Map<List<YearMonthPlan>, List<YearMonthPlanDto>>(_service.Where(z => z.RepairTagId == repairTagId && z.ResponsibleUnit == orgId && z.Year == year && z.Month == month && z.PlanType == planType).OrderBy(z => z.Number).ToList());
                //if (list.Count == 0) throw new UserFriendlyException("未找到任何数据");

                string executableUnitStr = "";
                foreach (var t in relate.Where(s => s.RepairItemId == item.RepairDetailsId))
                {
                    if (t.OrganizationType != null)
                        executableUnitStr += t.OrganizationType.Name + ",";
                }
                executableUnitStr = executableUnitStr.TrimEnd(',');

                row["序号"] = newNums.TrimEnd('-');//序号
                row["维修类别"] = EnumHelper.GetDescription((Enumer.StatisticalRepairType)item.RepairType) ?? "";//维修类别
                row["设备名称"] = item.DeviceName ?? "";//设备名称
                if (repairTagKey == "RepairTag.RailwayWired")
                {
                    row["编制执行单位"] = item.CompiledOrganization ?? "";
                }
                row["执行单位"] = executableUnitStr;
                row["设备处所"] = item.EquipmentLocation ?? "";//设备处所
                row["工作内容"] = item.RepairContent ?? "";//工作内容
                row["天窗类型"] = item.SkyligetType ?? "";//天窗类型
                row["单位"] = item.Unit ?? "";//单位
                int times = 1;
                if (planType == (int)YearMonthPlanType.年度月表)
                {
                    row["数量"] = item.Total;
                }
                else
                {
                    if (planType == (int)YearMonthPlanType.月表)
                    {
                        row["总设备数量"] = item.Total;//总数量
                        row["每月次数"] = item.Times ?? "";//每月次数
                    }
                    if (planType == (int)YearMonthPlanType.年表)
                    {
                        row["总设备数量"] = item.Total;//总数量
                        row["年计划总数量"] = item.PlanCount;//年计划总数量
                        row["每年次数"] = item.Times ?? "";//每年次数
                    }
                }

                int.TryParse(item.Times, out times);

                if (planType == (int)Enumer.YearMonthPlanType.月表 || planType == (int)Enumer.YearMonthPlanType.年度月表)
                {
                    monthDays = Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(item.Year, item.Month);
                }
                //为1-31列赋值
                for (var i = 1; i <= monthDays; i++)
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                //row["key"] = pn;
                rows.Add(row);
            }
            //获得当前车间
            if (orgModel != null)
            {
                value["input_orgName"] = orgModel.Name;
            }
            value["input_planTime"] = year + "年" + (month > 0 ? (month + "月") : "");

            var fileJObject = await _crPlanManager.JsonToDataTable(rows, tableName, false);

            var fileArray = new JArray { fileJObject };
            value["uploadFile_" + key] = fileArray;

            //获取当前用户id
            var userId = CurrentUser.Id.GetValueOrDefault();

            var workflow = await _bpmManager.CreateWorkflowByWorkflowTemplateKey(key, value.ToString(), userId);

            if (workflow != null)
            {
                return workflow.Id;
            }

            return Guid.Empty;
        }
        #endregion

        /// <summary>
        /// 更新测试项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpgradeTestItems(RepairTestItemUpgradeDto input)
        {
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            var nowRepairTestItems = await _repairTestService.GetListAsync();
            var existYearMonthPlamTestItems = _testService.Where(s => s.RepairTagId == RepairTagId && s.PlanYear == input.Year && !s.IsDeleted);
            //在指定年份未生成年月表测试项 生成测试项
            if (existYearMonthPlamTestItems.Count() == 0)
            {
                foreach (var item in nowRepairTestItems)
                {
                    YearMonthPlanTestItem temp = new YearMonthPlanTestItem(Guid.NewGuid());
                    temp.PlanYear = input.Year;
                    temp.RepairDetailsID = item.RepairItemId;
                    temp.Name = item.Name;
                    temp.TestType = (int)item.Type;
                    temp.TestUnit = item.Unit;
                    temp.TestContent = "";
                    temp.PredictedValue = item.DefaultValue;
                    temp.MaxRated = item.MaxRated;
                    temp.MinRated = item.MinRated;
                    temp.FileId = item.FileId;
                    temp.RepairTagId = RepairTagId;
                    temp.Order = item.Order;
                    await _testService.InsertAsync(temp);
                }
            }
            //已在指定年份生成年月表测试下  更新测试项
            else
            {
                var groupedNewTestItems = from a in nowRepairTestItems group a by a.RepairItemId into b select new { type = b.Key, item = b.ToList() };
                foreach (var item in groupedNewTestItems)
                {
                    //逻辑修改：更新测试项目后不能直接删除年月测试项，应当设置属性，否则维修项无法查询
                    //await _testService.DeleteAsync(s => s.RepairTagId == RepairTagId && s.RepairDetailsID == item.type && s.PlanYear == input.Year);

                    //var oldYearMonthTests = _testService.Where(s => s.RepairTagId == RepairTagId && s.RepairDetailsID == item.type && s.PlanYear == input.Year).ToList();

                    foreach (var oldYearMonthTest in existYearMonthPlamTestItems)
                    {
                        oldYearMonthTest.IsDeleted = true;
                        await _testService.UpdateAsync(oldYearMonthTest);
                    }

                    foreach (var newTestItem in item.item)
                    {
                        YearMonthPlanTestItem temp = new YearMonthPlanTestItem(Guid.NewGuid())
                        {
                            PlanYear = input.Year,
                            RepairDetailsID = newTestItem.RepairItemId,
                            Name = newTestItem.Name,
                            TestType = (int)newTestItem.Type,
                            TestUnit = newTestItem.Unit,
                            TestContent = "",
                            PredictedValue = newTestItem.DefaultValue,
                            MaxRated = newTestItem.MaxRated,
                            MinRated = newTestItem.MinRated,
                            FileId = newTestItem.FileId,
                            RepairTagId = RepairTagId,
                            Order = newTestItem.Order
                        };
                        await _testService.InsertAsync(temp);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 清空已填报项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> ClearFilled(YearMonthCreateDto input)
        {
            List<YearMonthPlan> data = new List<YearMonthPlan>();
            var RepairTagId = (await _dataDictionaries.FindAsync(s => s.Key == input.RepairTagKey))?.Id;
            data = _service.Where(s => s.RepairTagId == RepairTagId && s.Year == input.Year && s.ResponsibleUnit == input.OrgId && s.PlanType == input.PlanType).ToList();

            foreach (var item in data)
            {
                item.PlanCount = 0;
                item.Total = 0;
                item.Col_1 = 0;
                item.Col_2 = 0;
                item.Col_3 = 0;
                item.Col_4 = 0;
                item.Col_5 = 0;
                item.Col_6 = 0;
                item.Col_7 = 0;
                item.Col_8 = 0;
                item.Col_9 = 0;
                item.Col_10 = 0;
                item.Col_11 = 0;
                item.Col_12 = 0;
                item.Col_13 = 0;
                item.Col_14 = 0;
                item.Col_15 = 0;
                item.Col_16 = 0;
                item.Col_17 = 0;
                item.Col_18 = 0;
                item.Col_19 = 0;
                item.Col_20 = 0;
                item.Col_21 = 0;
                item.Col_22 = 0;
                item.Col_23 = 0;
                item.Col_24 = 0;
                item.Col_25 = 0;
                item.Col_26 = 0;
                item.Col_27 = 0;
                item.Col_28 = 0;
                item.Col_29 = 0;
                item.Col_30 = 0;
                item.Col_31 = 0;
                item.IsImport = false;
                item.EquipmentLocation = "";
                item.SkyligetType = null;
                await _service.UpdateAsync(item);
            }
            return true;
        }

        /// <summary>
        /// 月表记录导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Stream> ExportMonthStatisticData(YearMonthSearchDto input)
        {
            input.IsAll = true;
            var monthStatisticList = await GetMonthStatisticData(input);
            var planList = monthStatisticList.Items.ToList();

            var dataTable = new DataTable();
            //创建表头
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.序号.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.维修类别.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备名称.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.执行单位.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.工作内容.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.单位.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.总设备数量.ToString()));
            dataTable.Columns.Add(new DataColumn(MonthPlanExcelCol.每月次数.ToString()));
            for (int i = 1; i <= 31; i++)
            {
                dataTable.Columns.Add(new DataColumn(i.ToString()));
            }
            //填充数据
            foreach (var item in planList)
            {
                var repairType = "";
                repairType = item.RepairType switch
                {
                    1 => "集中检修",
                    2 => "日常检修",
                    3 => "重点检修",
                    _ => "其他",
                };
                var row = dataTable.NewRow();
                row[YearPlanExcelCol.序号.ToString()] = item.Number ?? "";
                row[YearPlanExcelCol.维修类别.ToString()] = repairType;
                row[YearPlanExcelCol.设备名称.ToString()] = item.DeviceName ?? "";
                row[YearPlanExcelCol.执行单位.ToString()] = item.ExecutableUnitStr ?? "";
                row[YearPlanExcelCol.工作内容.ToString()] = item.RepairContent ?? "";
                row[YearPlanExcelCol.单位.ToString()] = item.Unit ?? "";
                row[YearPlanExcelCol.总设备数量.ToString()] = item.Total;
                row[MonthPlanExcelCol.每月次数.ToString()] = item.Times ?? "";
                for (int i = 1; i <= 31; i++)
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                dataTable.Rows.Add(row);
            }
            var subf = ExcelHepler.DataTableToExcel(dataTable, "月表记录表.xlsx"); ;
            var st = new MemoryStream(subf);

            return st;
        }

        /// <summary>
        /// 年表记录导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Stream> ExporYearStatisticData(YearMonthSearchDto input)
        {
            input.IsAll = true;
            var monthStatisticList = await GetYearStatisticData(input);
            var planList = monthStatisticList.Items.ToList();

            var dataTable = new DataTable();
            //创建表头
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.序号.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.维修类别.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.设备名称.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.执行单位.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.工作内容.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.单位.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.总设备数量.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.年计划总数量.ToString()));
            dataTable.Columns.Add(new DataColumn(YearPlanExcelCol.每年次数.ToString()));
            for (int i = 1; i <= 12; i++)
            {
                dataTable.Columns.Add(new DataColumn(i.ToString()));
            }
            //填充数据
            foreach (var item in planList)
            {
                var repairType = "";
                repairType = item.RepairType switch
                {
                    1 => "集中检修",
                    2 => "日常检修",
                    3 => "重点检修",
                    _ => "其他",
                };
                var row = dataTable.NewRow();
                row[YearPlanExcelCol.序号.ToString()] = item.Number ?? "";
                row[YearPlanExcelCol.维修类别.ToString()] = repairType;
                row[YearPlanExcelCol.设备名称.ToString()] = item.DeviceName ?? "";
                row[YearPlanExcelCol.执行单位.ToString()] = item.ExecutableUnitStr ?? "";
                row[YearPlanExcelCol.工作内容.ToString()] = item.RepairContent ?? "";
                row[YearPlanExcelCol.单位.ToString()] = item.Unit ?? "";
                row[YearPlanExcelCol.总设备数量.ToString()] = item.Total;
                row[YearPlanExcelCol.年计划总数量.ToString()] = item.PlanCount;
                row[YearPlanExcelCol.每年次数.ToString()] = item.Times ?? "";
                for (int i = 1; i < 13; i++)
                {
                    row[i.ToString()] = GetColVal(item, i);
                }
                dataTable.Rows.Add(row);
            }
            var subf = ExcelHepler.DataTableToExcel(dataTable, "年表记录表.xlsx"); ;
            var st = new MemoryStream(subf);

            return st;
        }

        /// <summary>
        /// 提报车间数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetYearMonthTotalSimpleDto> GetTotal(GetTotalSearchDto input)
        {
            //获取当前组织机构下所有的子集（车间）
            var currentOrganizationList = await _organizationAppService.GetLoginUserOrganizationIds();
            var currentOrganizations = await _orgService.GetListAsync(currentOrganizationList);
            var currentOrganizationChildernIds = new List<Guid>();
            if (currentOrganizations.Any(x => x.Children != null))
            {
                foreach (var item in currentOrganizations)
                {
                    foreach (var child in item.Children)
                    {
                        currentOrganizationChildernIds.Add(child.Id);
                    }
                }
            }
            var copyChilderenList = currentOrganizationChildernIds;
            var totalSimpleDto = new GetYearMonthTotalSimpleDto();

            if (currentOrganizationChildernIds.Count == 0)
            {
                return totalSimpleDto;
            }
            //总数量
            totalSimpleDto.Total = currentOrganizationChildernIds.Count();
            //年表
            //通过
            var yearArroveList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 3, 1, true);
            totalSimpleDto.ApprovedYearTotal = yearArroveList.Total;
            totalSimpleDto.ArrovedYearOrganizationIds = yearArroveList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !yearArroveList.OrganizationIds.Contains(x)).ToList();

            //拒绝
            var yearRefuseList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 4, 1, true);
            totalSimpleDto.RefausedYearTotal = yearRefuseList.Total;
            totalSimpleDto.RefausedYearOrganizationIds = yearRefuseList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !yearRefuseList.OrganizationIds.Contains(x)).ToList();

            //待审核
            var yearWaittingList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 2, 1, true);
            totalSimpleDto.WaittingYearTotal = yearWaittingList.Total;
            totalSimpleDto.WaittingYearOrganizationIds = yearWaittingList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !yearWaittingList.OrganizationIds.Contains(x)).ToList();

            //未提报
            totalSimpleDto.UnSubmitedYearTotal = currentOrganizationChildernIds.Count();

            totalSimpleDto.UnSubmitedYearOrganizationIds = currentOrganizationChildernIds;

            /**
             *月表
             */
            currentOrganizationChildernIds = copyChilderenList;

            var monthArroveList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 3, 2);
            totalSimpleDto.ApprovedMonthTotal = monthArroveList.Total;
            totalSimpleDto.ArrovedMonthOrganizationIds = monthArroveList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthArroveList.OrganizationIds.Contains(x)).ToList();

            //拒绝
            var monthRefuseList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 4, 2);
            totalSimpleDto.RefausedMonthTotal = monthRefuseList.Total;
            totalSimpleDto.RefausedMonthOrganizationIds = monthRefuseList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthRefuseList.OrganizationIds.Contains(x)).ToList();

            //待审核
            var monthWaittingList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 2, 2);
            totalSimpleDto.WaittingMonthTotal = monthWaittingList.Total;
            totalSimpleDto.WaittingMonthOrganizationIds = monthWaittingList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthWaittingList.OrganizationIds.Contains(x)).ToList();

            //未提报
            totalSimpleDto.UnSubmitedMonthTotal = currentOrganizationChildernIds.Count();
            totalSimpleDto.UnSubmitedMonthOrganizationIds = currentOrganizationChildernIds;

            /**
             * 年表月度
             */
            currentOrganizationChildernIds = copyChilderenList;

            var monthOfYearArroveList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 3, 3);
            totalSimpleDto.ApprovedMonthOfYearTotal = monthOfYearArroveList.Total;
            totalSimpleDto.ArrovedMonthOfYearOrganizationIds = monthOfYearArroveList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthOfYearArroveList.OrganizationIds.Contains(x)).ToList();

            //拒绝
            var monthOfYearRefuseList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 4, 3);
            totalSimpleDto.RefausedMonthOfYearTotal = monthOfYearRefuseList.Total;
            totalSimpleDto.RefausedMonthOfYearOrganizationIds = monthOfYearRefuseList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthOfYearRefuseList.OrganizationIds.Contains(x)).ToList();

            //待审核
            var monthOfYearWaittingList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 2, 3);
            totalSimpleDto.WaittingMonthOfYearTotal = monthOfYearWaittingList.Total;
            totalSimpleDto.WaittingMonthOfYearOrganizationIds = monthOfYearWaittingList.OrganizationIds;
            currentOrganizationChildernIds = currentOrganizationChildernIds.Where(x => !monthOfYearWaittingList.OrganizationIds.Contains(x)).ToList();

            //未提报
            //currentOrganizationChildernIds = copyChilderenList;
            var monthOfYearUnSubmitList = GetYearMonthTotalSimple(currentOrganizationChildernIds, input.Time, 3, 3, false, true);
            totalSimpleDto.UnSubmitedMonthOfYearTotal = monthOfYearUnSubmitList.Total;
            totalSimpleDto.UnSubmitedMonthOfYearOrganizationIds = monthOfYearUnSubmitList.OrganizationIds;

            return await Task.FromResult(totalSimpleDto);
        }
        private YearMonthSimpleDto GetYearMonthTotalSimple(List<Guid> allOrganizationList, DateTime time, int state, int planType, bool isYear = false, bool isYearOfMonth = false)
        {
            var yearMonthTotalSimpleDto = new YearMonthSimpleDto();
            var planListOrgIds = new List<Guid>();

            //判断车间是否计划安排了当月的年表计划
            var currentMonth = time.Month.ToString();
            //var makePlanOrgList = new List<Guid>();
            if (isYearOfMonth)
            {
                var col = "Col_" + currentMonth;
                var planListByOrgIds = _service.Where(x => x.Year == time.Year &&
                                       x.Month == 0 &&
                                        x.PlanType == 1 &&
                                       x.State == state && x.AR_Key != null &&
                                      allOrganizationList.Contains(x.ResponsibleUnit)
                                      ).Distinct().ToList();

                foreach (var item in planListByOrgIds)
                {
                    if ((decimal)item.GetType().GetProperty(col).GetValue(item, null) == 0 || planListOrgIds.Contains(item.ResponsibleUnit))
                    {
                        continue;
                    }
                    planListOrgIds.Add(item.ResponsibleUnit);
                }
            }
            else
            {
                planListOrgIds = _service.
                   Where(x => x.Year == time.Year && (x.Month == (isYear ? 0 : time.Month)) && x.PlanType == planType && x.State == state && x.AR_Key != null && allOrganizationList.Contains(x.ResponsibleUnit))
                   .Select(y => y.ResponsibleUnit).Distinct().ToList();
            }

            yearMonthTotalSimpleDto.Total = planListOrgIds.Count();
            yearMonthTotalSimpleDto.OrganizationIds = planListOrgIds;

            return yearMonthTotalSimpleDto;
        }

        public async Task<List<string>> GetOrganizationNames(YearMonthSimpleDto input)
        {

            var organizationList = await _orgService.GetListAsync(input.OrganizationIds);
            var orgamozationNames = organizationList.Select(x => x.Name).ToList();

            return orgamozationNames;
        }

        public async Task<PagedResultDto<YearMonthAlterRecordDto>> GetYearMonthAlterRecords(YearMonthAlterRecordSearchDto input)
        {
            var res = new PagedResultDto<YearMonthAlterRecordDto>();

            if (input.OrganizationId == Guid.Empty)
            {
                return await Task.FromResult(res);
            }
            var org = await _orgService.GetAsync(input.OrganizationId);
            var allOrg = _organizationRepository.Where(s => s.Code.StartsWith(org.Code)).ToList();
            var allOrgIds = allOrg.Select(s => s.Id).ToList();


            var yeatMonthAlterRecords = _yearMonthAlterRecord
                .Where(x => allOrgIds.Contains(x.OrganizationId))
                .Where(x => x.State == input.State)
                .WhereIf(input.PlanType != 0, x => x.PlanType == input.PlanType);

            res.TotalCount = yeatMonthAlterRecords.Count();

            var list = yeatMonthAlterRecords.OrderBy(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();



            var yearMonthAlterRecordDto = ObjectMapper.Map<List<YearMonthAlterRecord>, List<YearMonthAlterRecordDto>>(list);

            foreach (var item in yearMonthAlterRecordDto)
            {
                var changeOrg = await _orgService.GetAsync(item.OrganizationId);
                item.OrganizationName = changeOrg.Name;
            }
            res.Items = yearMonthAlterRecordDto;

            return await Task.FromResult(res);
        }

        public async Task<bool> BackMonthOfYearPalns(YearMonthSearchDto input)
        {
            var repairTagId = _dataDictionaries.FirstOrDefault(s => s.Key == input.RepairTagKey).Id;

            if (Guid.Empty == repairTagId || repairTagId == null)
            {
                return false;
            }
            //1、查询年表月度的数据
            var monthOfYearPlanIds = _service.Where(x => x.ResponsibleUnit == input.OrgId && x.Year == input.Year
               && x.Month == input.Month && x.PlanType == 3 && x.RepairTagId == repairTagId).Select(y => y.Id);

            //2、查询日计划
            if (!monthOfYearPlanIds.Any()) return false;
            var dailyPlanIds = _dailyPlanService.Where(x => monthOfYearPlanIds.Contains(x.PlanId)).Select(x => x.Id);

            //3、查询详细计划
            var detailsPlanIds = _planDetailsService.Where(x => dailyPlanIds.Contains(x.DailyPlanId)).Select(x => x.SkylightPlanId);
            if (!detailsPlanIds.Any())
            {

                //删除日计划
                await _dailyPlanService.DeleteAsync(x => dailyPlanIds.Contains(x.Id));

                //删除垂直天窗计划
                await _service.DeleteAsync(x => monthOfYearPlanIds.Contains(x.Id));
                return true;
            }
            else
            {
                throw new UserFriendlyException("本月年表月度计划已经编制计划，不可退回！");
            }
        }
    }
}
