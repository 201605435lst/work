using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Basic.Authorization;
using SnAbp.Basic.Dtos;
using SnAbp.Basic.Dtos.Export;
using SnAbp.Basic.Dtos.Import;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Enums;
using SnAbp.Basic.IServices;
using SnAbp.Basic.Template;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Utils;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace SnAbp.Basic.Services
{
    [Authorize]
    public class BasicRailwayAppService : SnAbp.Basic.BasicAppService, IBasicRailwayAppService
    {
        private readonly IRepository<Railway, Guid> railwayRepository;
        private readonly IRepository<StationRltRailway, Guid> sta_RaRepository;
        private readonly IRepository<Station, Guid> stationRepository;
        private readonly IRepository<Organization, Guid> organizationRepository;
        protected OrganizationManager OrganizationManager { get; }
        private readonly IRepository<RailwayRltOrganization, Guid> railway_OrgRepository;
        protected IDataFilter DataFilter { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IFileImportHandler _fileImport;
        public BasicRailwayAppService(
            IRepository<Railway, Guid> _railwayRepository,
            IRepository<StationRltRailway, Guid> _sta_RaRepository,
            IRepository<Station, Guid> _stationRepository,
            IRepository<Organization, Guid> _organizationRepository,
            OrganizationManager organizationManager,
            IDataFilter dataFilter,
            IHttpContextAccessor httpContextAccessor,
            IRepository<RailwayRltOrganization, Guid> _railway_OrgRepository,
            IUnitOfWorkManager unitOfWorkManager, IFileImportHandler fileImport)
        {
            railwayRepository = _railwayRepository;
            sta_RaRepository = _sta_RaRepository;
            stationRepository = _stationRepository;
            organizationRepository = _organizationRepository;
            OrganizationManager = organizationManager;
            railway_OrgRepository = _railway_OrgRepository;
            DataFilter = dataFilter;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWorkManager = unitOfWorkManager;
            _fileImport = fileImport;
        }

        /// <summary>
        /// 根据ID获取线路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(BasicPermissions.Railway.Detail)]
        public async Task<RailwayDetailDto> Get(Guid id)
        {
            try
            {
                RailwayDetailDto result = new RailwayDetailDto();
                await Task.Run((Action)(() =>
                {
                    var ents = railwayRepository.WithDetails().FirstOrDefault(r => r.Id == id && r.IsDeleted == false);
                    result = ObjectMapper.Map<Railway, RailwayDetailDto>(ents);
                    List<RailwayDetailDto> temp = new List<RailwayDetailDto>() { result };
                    result = SetStationInfos(temp).First();
                    SetBelongOrgStr(ents, result);
                    foreach (var item in result.RailwayRltOrganizations)
                    {
                        if (item.Organization == null) item.OrganizationId = null;
                    }
                }));
                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 根据条件查询线路
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<RailwaySimpleDto>> GetList(RailwaySearchDto input)
        {
            try
            {
                PagedResultDto<RailwaySimpleDto> result = new PagedResultDto<RailwaySimpleDto>();
                await Task.Run((Action)(() =>
                {
                    // 新增条件过滤
                    //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                    //var organization = !string.IsNullOrEmpty(organizationIdString) ? organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                    //var organizationCode = organization != null ? organization.Code : null;

                    var ents = railwayRepository.WithDetails().Where(r => r.IsDeleted == false).ToList();
                    using (DataFilter.Disable<ISoftDelete>())
                    {
                        //if (organization != null && !string.IsNullOrEmpty(organizationCode))
                        //{
                        //    List<Railway> outOfRailways = new List<Railway>();
                        //    foreach (var item in ents)
                        //    {
                        //        if (railway_OrgRepository.FirstOrDefault(x => x.RailwayId == item.Id) == null)
                        //        {
                        //            outOfRailways.Add(item);
                        //        }
                        //    }
                        //  //  var railwayIds = railway_OrgRepository.Where(s => s.Organization.Code.StartsWith(organizationCode)).Select(s => s.RailwayId).ToList();
                        //    var railwayIds = railway_OrgRepository.Select(s => s.RailwayId).ToList();
                        //    ents = ents.Where(s => railwayIds.Contains(s.Id)).ToList();
                        //    ents.AddRange(outOfRailways);
                        //    ents.Distinct();
                        //}
                        List<Railway> outOfRailways = new List<Railway>();
                        var railwayIds = railway_OrgRepository.Select(s => s.RailwayId).ToList();
                        ents = ents.Where(s => railwayIds.Contains(s.Id)).ToList();
                        ents.AddRange(outOfRailways);
                        ents.Distinct();
                    }

                    ents = ents.WhereIf(!string.IsNullOrEmpty(input.RailwayName), r => r.Name.Contains(input.RailwayName))
                        .WhereIf(input.Type != null, r => r.Type == input.Type).ToList();

                    if (string.IsNullOrEmpty(input.StationName) == false)
                    {
                        var stationIds = stationRepository.Where(s => !s.IsDeleted && s.Name.Contains(input.StationName)).Select(s => s.Id);
                        var railIds = sta_RaRepository.Where(s => stationIds.Contains(s.StationId)).Select(s => s.RailwayId).ToList();
                        ents = ents.Where(s => railIds.Contains(s.Id)).OrderBy(s => s.Name).ToList();
                    }
                    //if (input.BelongOrgId != null && input.BelongOrgId != Guid.Empty)
                    //{
                    //    var belongOrgCode = organizationRepository.FirstOrDefault(x => x.Id == input.BelongOrgId)?.Code;
                    //    if (belongOrgCode != null || !string.IsNullOrEmpty(belongOrgCode))
                    //    {
                    //        var belongOrgIds = organizationRepository.Where(x => x.Code.StartsWith(belongOrgCode)).Select(s => s.Id).ToList();
                    //        ents = ents.Where(s => s.RailwayRltOrganizations.Any(m => belongOrgIds.Contains(m.OrganizationId.GetValueOrDefault()))).ToList();
                    //    }
                    //}

                    result.TotalCount = ents.Count;
                    if (!input.IsAll)
                    {
                        ents = ents.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                    }
                    List<RailwaySimpleDto> tempResult = new List<RailwaySimpleDto>();
                    foreach (var item in ents)
                    {
                        RailwaySimpleDto dto = ObjectMapper.Map<Railway, RailwaySimpleDto>(item);
                        SetBelongOrgStr(item, dto);
                        tempResult.Add(dto);
                    }
                    result.Items = SetStationInfos(tempResult);
                }));
                return result;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }


        /// <summary>
        /// 添加线路
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Railway.Create)]
        public async Task<RailwayDto> Create(RailwayCreateDto input)
        {
            try
            {
                if (string.IsNullOrEmpty(input.Name) || string.IsNullOrWhiteSpace(input.Name))
                {
                    throw new Exception("线路名称不能为空");
                }
                if (CheckRailwayIsExist(input.Name))
                {
                    throw new Exception("该线路已存在");
                };
                Railway ent = new Railway(Guid.NewGuid());
                ent.Name = input.Name;
                ent.Type = input.Type;
                ent.Remark = input.Remark;
                ent.IsDeleted = false;
                ValidationMaxlength.Validate(ent);
                await railwayRepository.InsertAsync(ent);

                //foreach (var item in input.RailwayOrgs)
                //{
                //    RailwayRltOrganization railwayRltOrg = new RailwayRltOrganization(GuidGenerator.Create());
                //    railwayRltOrg.DownLinkEndKM = item.DownLinkEndKM;
                //    railwayRltOrg.DownLinkStartKM = item.DownLinkStartKM;
                //    railwayRltOrg.OrganizationId = item.OrganizationId;
                //    railwayRltOrg.RailwayId = ent.Id;
                //    railwayRltOrg.UpLinkEndKM = item.UpLinkEndKM;
                //    railwayRltOrg.UpLinkStartKM = item.UpLinkStartKM;
                //    await railway_OrgRepository.InsertAsync(railwayRltOrg);
                //}
                return ObjectMapper.Map<Railway, RailwayDto>(ent);
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 给线路关联站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(BasicPermissions.Railway.Relate)]
        public async Task<bool> UpdateStations(RelateSta_RaInputDto input)
        {
            if (input.RaliwayId == null) throw new UserFriendlyException("站点信息错误");
            var railway = await railwayRepository.GetAsync(input.RaliwayId);
            var allStaRltRailways = sta_RaRepository.WithDetails().ToList();
            var allStations = stationRepository.Where(s => !s.IsDeleted).ToList();
            if (railway.Type == RailwayType.MONGLINE)
            {
                var temp = allStaRltRailways.Where(s => s.RailwayId == input.RaliwayId && s.RailwayType == RelateRailwayType.SINGLELINK).OrderBy(s => s.PassOrder).ToList();
                await UpdateStationsToSingleLink(temp, allStations, input.RaliwayId, input.RelateInfos, RelateRailwayType.SINGLELINK);
            }
            else
            {
                //上行更新
                var temp4UpLink = allStaRltRailways.Where(s => s.RailwayId == input.RaliwayId && s.RailwayType == RelateRailwayType.UPLINK).OrderBy(s => s.PassOrder).ToList();
                await UpdateStationsToSingleLink(temp4UpLink, allStations, input.RaliwayId, input.RelateInfos, RelateRailwayType.UPLINK);
                //下行更新
                var temp4DownLink = allStaRltRailways.Where(s => s.RailwayId == input.RaliwayId && s.RailwayType == RelateRailwayType.DOWNLINK).OrderBy(s => s.PassOrder).ToList();
                await UpdateStationsToSingleLink(temp4DownLink, allStations, input.RaliwayId, input.DownLinkRelateInfos, RelateRailwayType.DOWNLINK);
            }
            //var rail_stas = sta_RaRepository.WithDetails().Where(s => s.RailwayId == input.RaliwayId).OrderBy(s => s.PassOrder).ToList();
            //var stations = stationRepository.Where(s => !s.IsDeleted).ToList();
            return true;
        }

        private async Task UpdateStationsToSingleLink(List<StationRltRailway> staRltsRas, List<Station> stations, Guid raliwayId, List<Station_RailwayInputDto> infos, RelateRailwayType type)
        {
            //已关联的站点以及区间信息
            var existStaInfos = (from r in staRltsRas
                                 join s in stations on r.StationId equals s.Id
                                 select new
                                 {
                                     Station = s,
                                     r.PassOrder,
                                     r.KMMark,
                                     Rail_Sta = r
                                 }).OrderBy(s => s.KMMark).ToList();
            //要删除的关联关系
            List<Guid> deleteRail_StaIds = existStaInfos.Select(s => s.Rail_Sta.Id).ToList();
            List<Station> deleteSections = existStaInfos.Where(s => s.Station.Type == 1).Select(s => s.Station).ToList();
            //待执行的关联关系集合
            List<Railway_StaOperator> rail_StasOperator = new List<Railway_StaOperator>();

            infos = infos.OrderBy(s => s.KMMark).ToList();
            for (int i = 0; i < infos.Count; i++)
            {
                var ele = infos[i];
                //站点是否已关联
                var existSta = existStaInfos.FirstOrDefault(s => s.Station.Type == 0 && s.Station.Id == ele.StaId);
                if (existSta != null)
                {
                    existSta.Rail_Sta.KMMark = ele.KMMark;
                    rail_StasOperator.Add(new Railway_StaOperator(existSta.Rail_Sta, false));
                    deleteRail_StaIds.Remove(existSta.Rail_Sta.Id);
                }
                else
                {
                    StationRltRailway sta_Ra = new StationRltRailway(GuidGenerator.Create());
                    sta_Ra.KMMark = ele.KMMark;
                    sta_Ra.RailwayId = raliwayId;
                    sta_Ra.StationId = ele.StaId;
                    sta_Ra.RailwayType = type;
                    rail_StasOperator.Add(new Railway_StaOperator(sta_Ra, true));
                }
                //区间是否存在且已关联
                if (i < infos.Count - 1)
                {
                    var nextEle = infos[i + 1];
                    var existSection = existStaInfos.FirstOrDefault(s => s.Station.Type == 1 && s.Station.SectionStartStationId == ele.StaId && s.Station.SectionEndStationId == nextEle.StaId);
                    if (existSection != null)
                    {
                        rail_StasOperator.Add(new Railway_StaOperator(existSection.Rail_Sta, false));
                        deleteRail_StaIds.Remove(existSection.Rail_Sta.Id);
                        deleteSections.Remove(deleteSections.FirstOrDefault(s => s.Id == existSection.Station.Id));
                    }
                    else
                    {
                        Station newSec = new Station(GuidGenerator.Create());
                        var sta = stations.FirstOrDefault(s => s.Id == ele.StaId);
                        var nexSta = stations.FirstOrDefault(s => s.Id == nextEle.StaId);
                        newSec.Name = sta.Name + "-" + nexSta.Name + "区间";
                        newSec.SectionStartStationId = ele.StaId;
                        newSec.SectionEndStationId = nextEle.StaId;
                        newSec.Type = 1;
                        newSec.Remark = type == RelateRailwayType.SINGLELINK ? "单线" : (type == RelateRailwayType.UPLINK ? "上行" : "下行");
                        ValidationMaxlength.Validate(newSec);
                        await stationRepository.InsertAsync(newSec);

                        StationRltRailway sec_Ra = new StationRltRailway(GuidGenerator.Create());
                        sec_Ra.KMMark = 0;
                        sec_Ra.RailwayId = raliwayId;
                        sec_Ra.StationId = newSec.Id;
                        sec_Ra.RailwayType = type;
                        rail_StasOperator.Add(new Railway_StaOperator(sec_Ra, true));
                    }
                }
            }
            //修改关联关系
            int passOrder = 0;
            foreach (var item in rail_StasOperator)
            {
                item.Sta_Railway.PassOrder = passOrder++;
                if (item.IsAdd)
                {
                    await sta_RaRepository.InsertAsync(item.Sta_Railway);
                }
                else
                {
                    await sta_RaRepository.UpdateAsync(item.Sta_Railway);
                }
            }
            //删除无用关联关系 以及区间
            await sta_RaRepository.DeleteAsync(s => deleteRail_StaIds.Contains(s.Id));
            foreach (var item in deleteSections)
            {
                item.IsDeleted = true;
                await stationRepository.UpdateAsync(item);
            }
        }

        /// <summary>
        /// 修改线路信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Railway.Update)]
        public async Task<bool> Update(RailwayUpdateDto input)
        {
            if (input.Id == Guid.Empty || input.Id == null) throw new UserFriendlyException("id有误");
            var oldEnt = railwayRepository.FirstOrDefault(r => r.IsDeleted == false && r.Id == input.Id);
            if (oldEnt == null) throw new UserFriendlyException("此线路不存在");
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrWhiteSpace(input.Name)) throw new UserFriendlyException("线路名称不能为空");
            var sameRailWays = railwayRepository.Where(r => r.IsDeleted == false && r.Name == input.Name && r.Id != input.Id).ToList();
            if (sameRailWays != null && sameRailWays.Count > 0) throw new UserFriendlyException("该线路已存在");
            oldEnt.Name = input.Name;
            oldEnt.Type = input.Type;
            oldEnt.Remark = input.Remark;
            oldEnt.IsDeleted = false;
            ValidationMaxlength.Validate(oldEnt);
            await railwayRepository.UpdateAsync(oldEnt);

            await railway_OrgRepository.DeleteAsync(s => s.RailwayId == input.Id);
            //foreach (var item in input.RailwayOrgs)
            //{
            //    RailwayRltOrganization railwayRltOrg = new RailwayRltOrganization(GuidGenerator.Create());
            //    railwayRltOrg.DownLinkEndKM = item.DownLinkEndKM;
            //    railwayRltOrg.DownLinkStartKM = item.DownLinkStartKM;
            //    railwayRltOrg.OrganizationId = item.OrganizationId;
            //    railwayRltOrg.RailwayId = input.Id;
            //    railwayRltOrg.UpLinkEndKM = item.UpLinkEndKM;
            //    railwayRltOrg.UpLinkStartKM = item.UpLinkStartKM;
            //    await railway_OrgRepository.InsertAsync(railwayRltOrg);
            //}
            return true;
        }


        /// <summary>
        /// 根据ID删除线路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Railway.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var ent = railwayRepository.FirstOrDefault(z => z.IsDeleted == false && z.Id == id);
                if (id == Guid.Empty || ent == null)
                {
                    throw new Exception("删除对象不存在");
                }
                ent.IsDeleted = true;
                await railwayRepository.UpdateAsync(ent);
                await railway_OrgRepository.DeleteAsync(r => r.RailwayId == id);
                //软删除区间
                var deleteSections = sta_RaRepository.WithDetails().Where(s => s.RailwayId == id && s.Station.Type == 1);
                foreach (var item in deleteSections)
                {
                    item.Station.IsDeleted = true;
                    await stationRepository.UpdateAsync(item.Station);
                }
                await sta_RaRepository.DeleteAsync(s => s.RailwayId == id);
                await railway_OrgRepository.DeleteAsync(s => s.RailwayId == id);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <param name="belongOrgCode">所属段编码</param>
        /// <returns></returns>
        ///
        [Authorize(BasicPermissions.Railway.Import)]
        public async Task<string> Upload([FromForm] ImportData input)
        {
            await _fileImport.Start(input.ImportKey, 100);
            string failMsg = "";
            int addCount = 0;
            int updateCount = 0;
            int deleteCount = 0;
            DataTable dt = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0);
                dt = ExcelHelper.ImportBaseDataToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, out var workbook1);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (dt == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未找到任何数据,请检查文件格式");
            }
            await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
            #region 验证列是否存在
            //验证列是否存在
            if (!dt.Columns.Contains(RailwayInportCol.SeenSun.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + RailwayInportCol.SeenSun.ToString() + "不存在");
            }
            //if (!dt.Columns.Contains(RailwayInportCol.Organization.ToString()))
            //{
            //    await _fileImport.Cancel(input.ImportKey);
            //    throw new UserFriendlyException("列" + RailwayInportCol.Organization.ToString() + "不存在");
            //}
            if (!dt.Columns.Contains(RailwayInportCol.Name.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + RailwayInportCol.Name.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(RailwayInportCol.Type.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + RailwayInportCol.Type.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(RailwayInportCol.DownLink.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + RailwayInportCol.DownLink.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(RailwayInportCol.UpLink.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + RailwayInportCol.UpLink.ToString() + "不存在");
            }
            #endregion

            int dataRowIndex = 4;
            //var organizationName = string.Empty;//单位名称
            //var organizationId = Guid.Empty;//单位id
            Dictionary<string, Guid> railwayNameId = new Dictionary<string, Guid>();//key线路名称,val线路id//防止重复
            //数据获取                                                                        
            try
            {
                List<Railway> allRailway; // 数据库中已经存在的数据。
                using (DataFilter.Disable<ISoftDelete>())
                {
                    allRailway = railwayRepository.Where(a => true).ToList();
                }

                var orgs = organizationRepository.Where(s => s.Code.StartsWith(input.BelongOrgCode) && s.IsDeleted == false);
                List<WrongInfo> wrongInfos = new List<WrongInfo>();
                using var unitWork = _unitOfWorkManager.Begin(true, false);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var row = dt.Rows[i];
                    await _fileImport.UpdateState(input.ImportKey, i);
                    #region 为单位赋值，检查excel当前行是否为有效数据

                    //string organization = Convert.ToString(row[RailwayInportCol.Organization.ToString()]);
                    //if (!string.IsNullOrEmpty(organization))
                    //{
                    //    //1查单位是否存在，存在赋值给organizationName，
                    //    //不存在，1.1添加单位不存在提示failMsg，
                    //    //1.2 organizationName赋值为空 
                    //    //1.3 后面单位organizationName不存在的数据都不添加，直到organization有值
                    //    var organizationCurrent = orgs.FirstOrDefault(s => s.Name == organization);
                    //    if (organizationCurrent != null)
                    //    {
                    //        organizationName = organization;//单位赋值
                    //        organizationId = organizationCurrent.Id;//单位ID赋值
                    //    }
                    //    else
                    //    {
                    //        failMsg += "导入失败，原因：单位" + organization + "不存在" + "\r\n";
                    //        organizationName = string.Empty;
                    //        organizationId = Guid.Empty;

                    //        WrongInfo wrong = new WrongInfo(i + dataRowIndex);
                    //        wrong.WrongMsg = "单位" + organization + "不存在";
                    //        wrongInfos.Add(wrong);
                    //        continue;//读取下一行
                    //    }
                    //}

                    //if (string.IsNullOrEmpty(organizationName))
                    //{
                    //    WrongInfo wrong = new WrongInfo(i + dataRowIndex);
                    //    wrong.WrongMsg = "单位" + organizationName + "不存在";
                    //    wrongInfos.Add(wrong);
                    //    continue;//单位不存在，继续读取下一行。
                    //}
                    string name = Convert.ToString(row[RailwayInportCol.Name.ToString()]);
                    //if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(organization))
                    //{
                    //    WrongInfo wrong = new WrongInfo(i + dataRowIndex);
                    //    wrong.WrongMsg = "线别" + name + "不存在";
                    //    wrongInfos.Add(wrong);
                    //    continue;//线别为空时，跳过当前行，读取下一行
                    //}


                    #endregion

                    string type = Convert.ToString(row[RailwayInportCol.Type.ToString()]);
                    string downLink = Convert.ToString(row[RailwayInportCol.DownLink.ToString()]);
                    string upLink = Convert.ToString(row[RailwayInportCol.UpLink.ToString()]);

                    #region model 接收model数据

                    #region 填充线路model

                    Railway railwaymodel = new Railway(Guid.NewGuid());

                    railwaymodel.Name = name;
                    if (type == EnumHelper.GetDescription(RailwayType.MONGLINE))
                    {
                        railwaymodel.Type = RailwayType.MONGLINE;
                    }
                    else if (type == EnumHelper.GetDescription(RailwayType.COMPLEXLINE))
                    {
                        railwaymodel.Type = RailwayType.COMPLEXLINE;
                    }

                    #endregion

                    #region 填充单位线路关系model

                   // RailwayRltOrganization railway_Orgmodel = new RailwayRltOrganization(Guid.NewGuid());

                    //railway_Orgmodel.OrganizationId = organizationId;
                    if (!string.IsNullOrEmpty(downLink))
                    {
                        string[] arr = downLink.Split('-'); //downLink.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                        //if (arr.Length > 2)
                        //{
                        //    railway_Orgmodel.DownLinkStartKM = int.Parse('-' + arr[1].Trim());
                        //    railway_Orgmodel.DownLinkEndKM = int.Parse(arr[2].Trim());
                        //}
                        //else if (arr.Length > 0)
                        //{
                        //    railway_Orgmodel.DownLinkStartKM = int.Parse(arr[0]);
                        //    railway_Orgmodel.DownLinkEndKM = int.Parse(arr[1]);
                        //}
                    }

                    if (!string.IsNullOrEmpty(upLink))
                    {
                        string[] arr = upLink.Split('-');
                        //if (arr.Length > 2)
                        //{
                        //    railway_Orgmodel.UpLinkStartKM = int.Parse('-' + arr[1].Trim());
                        //    railway_Orgmodel.UpLinkEndKM = int.Parse(arr[2].Trim());
                        //}
                        //else if (arr.Length > 0)
                        //{
                        //    railway_Orgmodel.UpLinkStartKM = int.Parse(arr[0].Trim());
                        //    railway_Orgmodel.UpLinkEndKM = int.Parse(arr[1].Trim());
                        //}
                    }
                    #endregion

                    #endregion

                    try
                    {
                        var oldRailway = allRailway.Where(o => o.Name == name).FirstOrDefault();
                        if (oldRailway != null || railwayNameId.ContainsKey(name))
                        {
                            var oldId = oldRailway != null ? oldRailway.Id : railwayNameId[name];//从字典或者数据库中读取id

                            //var oldRailwayOrg = railway_OrgRepository.Where(o => o.OrganizationId == organizationId && o.RailwayId == oldId).FirstOrDefault();
                            //if (oldRailwayOrg != null)
                            //{
                            //    //更新单位线路关系表
                            //    oldRailwayOrg.DownLinkStartKM = railway_Orgmodel.DownLinkStartKM;
                            //    oldRailwayOrg.DownLinkEndKM = railway_Orgmodel.DownLinkEndKM;
                            //    oldRailwayOrg.UpLinkStartKM = railway_Orgmodel.UpLinkStartKM;
                            //    oldRailwayOrg.UpLinkEndKM = railway_Orgmodel.UpLinkEndKM;
                            //    await railway_OrgRepository.UpdateAsync(oldRailwayOrg);
                            //}
                            //else
                            //{
                            //    //添加单位线路关系表
                            //    railway_Orgmodel.RailwayId = oldId;
                            //    railway_Orgmodel.OrganizationId = organizationId;
                            //    await railway_OrgRepository.InsertAsync(railway_Orgmodel);
                            //}

                            if (!railwayNameId.ContainsKey(name))//字典中已存在则不更新，说明之前已处理过
                            {
                                //字典中不存在，1、添加到字典中；2、更新到数据库
                                using (var unitwork = _unitOfWorkManager.Begin(true, false))
                                {
                                    railwayNameId.Add(name, oldId);

                                    oldRailway.Name = railwaymodel.Name;
                                    oldRailway.Type = railwaymodel.Type;
                                    oldRailway.IsDeleted = false;
                                    await railwayRepository.UpdateAsync(oldRailway);
                                    updateCount++;
                                    await unitwork.CompleteAsync();
                                }
                            }
                        }
                        else
                        {
                            if (!railwayNameId.ContainsKey(name))
                            {

                                using (var unitwork = _unitOfWorkManager.Begin(true, false))
                                {
                                    //不存在则 1、添加线路，2、添加到字典中
                                    railwaymodel = await railwayRepository.InsertAsync(railwaymodel);
                                    addCount++;
                                    await unitwork.CompleteAsync();
                                }
                                railwayNameId.Add(name, railwaymodel.Id);
                            }

                            //添加单位线路关系表
                            //railway_Orgmodel.RailwayId = railwayNameId[name];
                            //railway_Orgmodel.OrganizationId = organizationId;
                            //await railway_OrgRepository.InsertAsync(railway_Orgmodel);
                        }
                    }
                    catch (Exception e)
                    {
                        //failMsg += organizationName + "(" + name + ")导入失败，原因：" + e.Message + "\r\n";
                    }
                }

                //软删除excel中不存在的数据
                var softDeleteManu = allRailway.Where(s => !railwayNameId.Values.ToList().Contains(s.Id));
                foreach (var item in softDeleteManu)
                {
                    item.IsDeleted = true;
                    await railwayRepository.UpdateAsync(item);
                    deleteCount++;
                }
                await _fileImport.UpdateState(input.ImportKey, dt.Rows.Count - 1);
                await unitWork.SaveChangesAsync();
                await _fileImport.Complete(input.ImportKey);
                failMsg += string.Format($"成功导入{addCount}条线路数据，更新{updateCount}条线路数据，删除{deleteCount}条数据。");

                // 处理错误信息
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
            catch (Exception e)
            {
                await _fileImport.Cancel(input.ImportKey);
            }
            return failMsg;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        ///
        public async Task<Stream> Export(RailwayExportData input)
        {
            List<IGrouping<Guid?, RailwayRltOrganization>> rroList;
            var RRO = await railway_OrgRepository.GetListAsync();
            var railwayRltOrgs = new List<RailwayRltOrganization>();

            if (input.paramter.RailwayName == null && input.paramter.StationName == null && input.paramter.BelongOrgId == null && string.IsNullOrEmpty(input.paramter.Type.ToString()))
            {
                rroList = RRO.GroupBy(x => x.OrganizationId).ToList();
            }
            else
            {
                var rList = GetRailwayDataAsync(input.paramter);
                foreach (var r in rList)
                {
                    var _railwayRltOrg = RRO.Where(x => x.RailwayId == r.Id);
                    foreach (var rro in _railwayRltOrg)
                    {
                        railwayRltOrgs.Add(rro);
                    }
                }
                rroList = railwayRltOrgs.GroupBy(x => x.OrganizationId).ToList();
            }


            var railways = new List<RailwayImportTemplate>();
            var railway = new RailwayImportTemplate();
            Organization org;
            foreach (var item in rroList)
            {
                foreach (var ite in item)
                {
                    railway = new RailwayImportTemplate();
                    using (DataFilter.Disable<ISoftDelete>())
                    {
                        org = await OrganizationManager.GetAsync((Guid)ite.OrganizationId);
                    }
                    if (org != null && org.IsDeleted == false)
                    {
                        if (!railways.Any(x => x.Origanization == org.Name))
                        {
                            railway.Origanization = org.Name;
                            railways.Add(railway);
                            railway = new RailwayImportTemplate();
                        }
                    }
                    railway.DownLink = (ite.DownLinkStartKM != 0 || ite.DownLinkEndKM != 0) ? ite.DownLinkStartKM.ToString() + '-' + ite.DownLinkEndKM.ToString() : null;
                    railway.UpLink = (ite.UpLinkStartKM != 0 || ite.UpLinkEndKM != 0) ? ite.UpLinkStartKM.ToString() + '-' + ite.UpLinkEndKM.ToString() : null;

                    railway.Name = ite.Railway.Name;
                    railway.Type = ite.Railway.Type == RailwayType.MONGLINE ? "单线" : (ite.Railway.Type == RailwayType.COMPLEXLINE ? "复线" : null);
                    if (!string.IsNullOrEmpty(railway.Name) && railway.Type != null)
                    {
                        railways.Add(railway);
                    }
                }
            }
            var dtoList = ObjectMapper.Map<List<RailwayImportTemplate>, List<RailwayImportTemplate>>(railways);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }


        #region 私有方法

        /// <summary>
        /// 检测站点信息是否存在
        /// </summary>
        /// <returns></returns>
        private bool CheckRailwayIsExist(string raName)
        {
            var sameRailWays = railwayRepository.Where(r => r.IsDeleted == false && r.Name == raName).ToList();
            if (sameRailWays.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置线路信息中的包含站点信息
        /// </summary>
        /// <param name="railways">需要设置的站点信息集合</param>
        /// <returns></returns>
        private List<T> SetStationInfos<T>(List<T> railways) where T : RailwaySimpleDto
        {
            var stations = stationRepository.Where(s => s.IsDeleted == false).ToList();
            var allRelates = sta_RaRepository.Where(s => s.Id != null);
            var stationDtos = ObjectMapper.Map<List<Station>, List<StationDto>>(stations);
            foreach (var item in railways)
            {
                var staRelates = allRelates.Where(r => r.RailwayId == item.Id).OrderBy(s => s.PassOrder).ToList();
                List<StationDto> staDtos = new List<StationDto>();
                List<StationDto> downLinkStaDtos = new List<StationDto>();
                foreach (var relate in staRelates)
                {
                    var dto = stationDtos.Find(s => s.Id == relate.StationId);
                    if (dto != null)
                    {
                        StationDto copyDto = new StationDto();
                        copyDto.Id = dto.Id;
                        copyDto.PassOrder = relate.PassOrder;
                        copyDto.Remark = dto.Remark;
                        copyDto.Name = dto.Name;
                        copyDto.Type = dto.Type;
                        copyDto.KMMark = relate.KMMark;
                        switch (relate.RailwayType)
                        {
                            case RelateRailwayType.SINGLELINK:
                            case RelateRailwayType.UPLINK:
                                staDtos.Add(copyDto);
                                break;
                            case RelateRailwayType.DOWNLINK:
                                downLinkStaDtos.Add(copyDto);
                                break;
                        }
                    }
                }
                item.Stations = staDtos;
                item.DownLinkStations = downLinkStaDtos;
            }
            return railways;
        }

        /// <summary>
        /// 设置所属单位的显示str
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="item">ent类型对象</param>
        /// <param name="dto">dto类型对象</param>
        private void SetBelongOrgStr<T>(Railway item, T dto) where T : RailwaySimpleDto
        {
            string spltStr = " / ";
            foreach (var temp in item.RailwayRltOrganizations)
            {
                if (temp.Organization != null)
                {
                    var orgName = temp.Organization.Name;
                    dto.BelongOrgsStr += orgName + spltStr;
                }
            }
            dto.BelongOrgsStr = string.IsNullOrEmpty(dto.BelongOrgsStr) ? "" : dto.BelongOrgsStr.Substring(0, dto.BelongOrgsStr.Length - spltStr.Length);
        }


        private List<Railway> GetRailwayDataAsync(RailwaySearchDto input)
        {
            // 新增条件过滤
            //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            //var organization = !string.IsNullOrEmpty(organizationIdString) ? organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            //var organizationCode = organization != null ? organization.Code : null;

            var ents = railwayRepository.WithDetails().Where(r => r.IsDeleted == false).ToList();
            using (DataFilter.Disable<ISoftDelete>())
            {
                //if (organization != null && !string.IsNullOrEmpty(organizationCode))
                //{
                //    List<Railway> outOfRailways = new List<Railway>();
                //    foreach (var item in ents)
                //    {
                //        if (railway_OrgRepository.FirstOrDefault(x => x.RailwayId == item.Id) == null)
                //        {
                //            outOfRailways.Add(item);
                //        }
                //    }
                //    var railwayIds = railway_OrgRepository.Where(s => s.Organization.Code.StartsWith(organizationCode)).Select(s => s.RailwayId).ToList();
                //    ents = ents.Where(s => railwayIds.Contains(s.Id)).ToList();
                //    ents.AddRange(outOfRailways);
                //    ents.Distinct();
                //}
            }

            ents = ents.WhereIf(!string.IsNullOrEmpty(input.RailwayName), r => r.Name.Contains(input.RailwayName))
                .WhereIf(input.Type != null, r => r.Type == input.Type).ToList();

            if (string.IsNullOrEmpty(input.StationName) == false)
            {
                var stationIds = stationRepository.Where(s => !s.IsDeleted && s.Name.Contains(input.StationName)).Select(s => s.Id);
                var railIds = sta_RaRepository.Where(s => stationIds.Contains(s.StationId)).Select(s => s.RailwayId).ToList();
                ents = ents.Where(s => railIds.Contains(s.Id)).OrderBy(s => s.Name).ToList();
            }
            //if (input.BelongOrgId != null && input.BelongOrgId != Guid.Empty)
            //{

            //    ents = ents.Where(s => s.RailwayRltOrganizations.Find(s => s.OrganizationId == input.BelongOrgId) != null).ToList();
            //}
            return ents;
        }


        #endregion
    }

    public struct Railway_StaOperator
    {
        public Railway_StaOperator(StationRltRailway sta_Railway, bool isAdd)
        {
            Sta_Railway = sta_Railway;
            IsAdd = isAdd;
        }
        public StationRltRailway Sta_Railway { get; set; }
        public bool IsAdd { get; set; }
    }
}