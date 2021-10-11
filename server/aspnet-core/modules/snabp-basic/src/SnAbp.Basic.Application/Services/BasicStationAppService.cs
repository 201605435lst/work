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
using SnAbp.Basic.Settings;
using SnAbp.Basic.Template;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Utils;
using SnAbp.Utils.DataImport;
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
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Basic.Services
{
    [Authorize]
    public class BasicStationAppService : SnAbp.Basic.BasicAppService, IBasicStationAppService
    {
        private readonly IRepository<Station, Guid> _stationRepository;
        private readonly IRepository<StationRltOrganization, Guid> _station_OrgsRepository;
        private readonly IRepository<Organization, Guid> _organizationRepository;
        private readonly IRepository<StationRltRailway, Guid> _sta_RaRepository;
        private readonly IRepository<RailwayRltOrganization, Guid> _railwayRltOrganizations;
        private readonly IRepository<StationRltRailway, Guid> _stationRltRailways;
        private readonly IRepository<Railway, Guid> _railwayRepository;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected IDataFilter DataFilter { get; }
        private readonly IGuidGenerator _guidGenerator;
        public BasicStationAppService(
            IRepository<Station, Guid> stationRepository,
            IRepository<StationRltOrganization, Guid> station_OrgsRepository,
            IRepository<Organization, Guid> organizationRepository,
            IRepository<StationRltRailway, Guid> sta_RaRepository,
            IRepository<Railway, Guid> railwayRepository,
            IRepository<RailwayRltOrganization, Guid> railwayRltOrganizations,
            IRepository<StationRltRailway, Guid> stationRltRailways,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _stationRepository = stationRepository;
            _station_OrgsRepository = station_OrgsRepository;
            _organizationRepository = organizationRepository;
            _sta_RaRepository = sta_RaRepository;
            _railwayRepository = railwayRepository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWorkManager = unitOfWorkManager;
            DataFilter = dataFilter;
            _railwayRltOrganizations = railwayRltOrganizations;
            _stationRltRailways = stationRltRailways;
        }

        #region 获得对象实体===========================================
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(BasicPermissions.Station.Detail)]
        public async Task<StationDetailDto> Get(Guid id)
        {
            StationDetailDto res = new StationDetailDto();
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("id不正确");
            var ent = await _stationRepository.GetAsync(id);
            if (ent == null || ent.IsDeleted) throw new UserFriendlyException("对象不存在");
            res = ObjectMapper.Map<Station, StationDetailDto>(ent);

            var sta_ras = _sta_RaRepository.WithDetails().Where(s => s.StationId == ent.Id);
            foreach (var item in sta_ras)
            {
                BelongRailwayInfo temp = new BelongRailwayInfo(ObjectMapper.Map<Railway, RailwayDto>(item.Railway), item.KMMark);
                if (item.RailwayType == Enums.RelateRailwayType.DOWNLINK || item.RailwayType == Enums.RelateRailwayType.UPLINK)
                {
                    temp.Railway.Name += "(" + EnumHelper.GetDescription(item.RailwayType) + ")";
                }
                res.BelongRailways.Add(temp);
            }

            var staOrgIds = _station_OrgsRepository.Where(s => s.StationId == id).Select(s => s.OrganizationId).ToList();
            var allOrg = _organizationRepository.Where(s => s.IsDeleted == false && staOrgIds.Contains(s.Id)).ToList();
           // res.RepairTeams = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(allOrg);
            return res;
        }
        #endregion

        #region 获取列表数据===========================================
        /// <summary>
        /// 根据条件获取站点
        /// </summary>

        public async Task<PagedResultDto<StationSimpleDto>> GetList(StationSearchDto input)
        {
            PagedResultDto<StationSimpleDto> result = new PagedResultDto<StationSimpleDto>();
            await Task.Run(() =>
            {
                // 获取当前登录用户的组织机构
                //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
                //var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
                //var organizationCode = organization != null ? organization.Code : null;
                /*获取的线路站点的关联表信息*/
                var allStaRa = _sta_RaRepository.WithDetails().Where(s => s.RailwayId != null && s.StationId != null);
                /*获取站的数据*/
                var staEnts = _stationRepository.Where(s => !s.IsDeleted && s.Type == 0).WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name)).ToList();

                using (DataFilter.Disable<ISoftDelete>())
                {
                    //if (organization != null && !string.IsNullOrEmpty(organizationCode))
                    //{
                    //    List<Station> outOfOrgStations = new List<Station>();
                    //    foreach (var item in staEnts)
                    //    {
                    //        /*获取车站维护班组*/
                    //        if (_station_OrgsRepository.FirstOrDefault(x => x.StationId == item.Id) == null)
                    //        {

                    //            outOfOrgStations.Add(item);
                    //        }
                    //    }
                    //    /*获取车站维护班组中当前组织机构下的站点id*/
                    //    var staIds = _station_OrgsRepository.Where(s => s.Organization.Code.StartsWith(organizationCode)).Select(s => s.StationId).ToList();

                    //    staEnts = staEnts.Where(s => staIds.Contains(s.Id)).ToList();
                    //    staEnts.AddRange(outOfOrgStations);
                    //    staEnts.Distinct();
                    //}
                }

                if (input.BelongRaId != Guid.Empty && input.BelongRaId != null)
                {
                    var rail_staEnts = allStaRa.Where(s => s.RailwayId == input.BelongRaId);
                    var rail_staIds = rail_staEnts.Select(s => s.StationId).ToList();
                    staEnts = staEnts.Where(s => rail_staIds.Contains(s.Id)).ToList();
                }
                if (input.RepairTeamId != Guid.Empty && input.RepairTeamId != null)
                {
                    //var staIds = _station_OrgsRepository.Where(s => s.OrganizationId == input.RepairTeamId).Select(s => s.StationId).ToList();
                    //staEnts = staEnts.Where(s => staIds.Contains(s.Id)).ToList();
                }
                List<StationSimpleDto> res = ObjectMapper.Map<List<Station>, List<StationSimpleDto>>(staEnts);

                result.TotalCount = res.Count();
                if (input.IsAll == false)
                    res = res.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                //var allOrg = _organizationRepository.Where(s => s.Id != null).ToList();
                //var allStaOrg = _station_OrgsRepository.Where(s => s.StationId != null).ToList();
                string spltStr = " / ";
                foreach (var item in res)
                {
                    var temp = allStaRa.Where(s => s.StationId == item.Id);
                    foreach (var rail in temp)
                    {
                        var railwayName = rail.Railway.Name;
                        if (rail.RailwayType == Enums.RelateRailwayType.UPLINK || rail.RailwayType == Enums.RelateRailwayType.DOWNLINK)
                        {
                            var lingTip = EnumHelper.GetDescription(rail.RailwayType);
                            railwayName += "(" + lingTip + ")";
                        }
                        item.BelongRailways += railwayName + spltStr;
                    }
                    item.BelongRailways = string.IsNullOrEmpty(item.BelongRailways) ? "" : item.BelongRailways.Substring(0, item.BelongRailways.Length - spltStr.Length);

                    //var staOrg = allStaOrg.Where(s => s.StationId == item.Id);
                    //foreach (var staOrgItem in staOrg)
                    //{
                    //    var temp2 = allOrg.FirstOrDefault(s => s.Id == staOrgItem.OrganizationId);
                    //    if (temp2 != null)
                    //    {
                    //        item.RepairTeams += temp2.Name + spltStr;
                    //    }
                    //}
                    item.RepairTeams = string.IsNullOrEmpty(item.RepairTeams) ? "" : item.RepairTeams.Substring(0, item.RepairTeams.Length - spltStr.Length);
                }
                result.Items = res.Distinct().ToList();
            });
            return result;
        }

        /// <summary>
        /// 获取极简数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<StationVerySimpleDto>> GetSimpleList(StationSimpleSearchDto input)
        {
            List<StationVerySimpleDto> res = new List<StationVerySimpleDto>();
            await Task.Run(() =>
            {
                var staEnts = _stationRepository.Where(s => !s.IsDeleted)
                    .WhereIf(input.Type != null, s => s.Type == input.Type)
                    .WhereIf(!string.IsNullOrEmpty(input.Keyword), s => s.Name.Contains(input.Keyword)).ToList();
                res = ObjectMapper.Map<List<Station>, List<StationVerySimpleDto>>(staEnts);
            });
            return res;
        }

        #endregion

        #region 获取组织机构-站点级联数据

        /// <summary>
        /// 根据组织机构（维护车间）获取 线路-站点级联数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<StationRailDto>> GetCascaderListWithOrg(StationCascaderDto input)
        {
            List<StationRailDto> res = new List<StationRailDto>();
            //if (input.organizationId == null) return res;
            //var totalOrg = _organizationRepository.Where(s => s.IsDeleted == false).ToList(); ;
            //var allOrg = totalOrg;
            //if (input.organizationId != null && input.organizationId != Guid.Empty)
            //{
            //    var org = allOrg.FirstOrDefault(s => s.Id == input.organizationId);
            //    if (org == null) throw new UserFriendlyException("组织机构错误");
            //    else
            //    {
            //        allOrg = allOrg.Where(s => s.Code.StartsWith(org.Code)).ToList();
            //    }
            //}
            //var allOrgIds = allOrg.Select(s => s.Id).ToList();
            var railwaysList = _railwayRltOrganizations.WithDetails().Select(s => s.Railway).Distinct().ToList();
            //var railways = railwaysList.WhereIf(input.railwayId != null && input.organizationId != Guid.Empty, x => x.Id == input.railwayId);
            var railways = railwaysList.Where(x => x.Id == input.railwayId);
            var aa = (from r in railways
                      join sr in _stationRltRailways.WithDetails().WhereIf(input.isShowStation,x=>x.Station.Type == 0) on
                      r.Id equals sr.RailwayId
                      select new { Railway = r, Rlt = sr }).ToList();
            var bb = from a in aa
                     group a by a.Railway.Id into g
                     select new
                     {
                         RailwayId = g.Key,
                         StaRlts = g.Select(s => s.Rlt)
                     };
            foreach (var item in railways)
            {
                StationRailDto dto = new StationRailDto();
                dto.Id = item.Id;
                dto.Name = item.Name;
                dto.Type = 0;
                List<StationRailDto> children = new List<StationRailDto>();
                var tempRlts = bb.FirstOrDefault(s => s.RailwayId == item.Id)?.StaRlts;
                if (tempRlts != null && tempRlts.Count() > 0)
                {
                    if (item.Type == Enums.RailwayType.MONGLINE)
                    {
                        var resRlts = tempRlts.Where(s => s.RailwayType == Enums.RelateRailwayType.SINGLELINK).OrderBy(s => s.PassOrder);
                        foreach (var r in resRlts)
                        {
                            StationRailDto ch = new StationRailDto();
                            ch.Id = r.Id;
                            ch.StaId = r.StationId;
                            ch.Name = r.Station.Name;
                            ch.Type = 11;
                            dto.Children.Add(ch);
                        }
                    }
                    else
                    {
                        StationRailDto upLink = new StationRailDto();
                        upLink.Id = Guid.NewGuid();
                        upLink.Name = "上行";
                        upLink.Type = -1;
                        StationRailDto downLink = new StationRailDto();
                        downLink.Id = Guid.NewGuid();
                        downLink.Name = "下行";
                        downLink.Type = -2;

                        tempRlts.Where(s => s.RailwayType == Enums.RelateRailwayType.UPLINK).OrderBy(s => s.PassOrder).ToList().ForEach(item =>
                        {

                            StationRailDto ch = new StationRailDto();
                            ch.Id = item.Id;
                            ch.StaId = item.StationId;
                            ch.Name = item.Station.Name;
                            ch.Type = 1;
                            upLink.Children.Add(ch);
                        });
                        tempRlts.Where(s => s.RailwayType == Enums.RelateRailwayType.DOWNLINK).OrderBy(s => s.PassOrder).ToList().ForEach(item =>
                        {

                            StationRailDto ch = new StationRailDto();
                            ch.Id = item.Id;
                            ch.StaId = item.StationId;
                            ch.Name = item.Station.Name;
                            ch.Type = 2;
                            downLink.Children.Add(ch);
                        });
                        dto.Children.Add(upLink);
                        dto.Children.Add(downLink);
                        if (input.IsShowUpAndDown)
                        {
                            StationRailDto upDownLink = new StationRailDto();
                            upDownLink.Id = Guid.NewGuid();
                            upDownLink.Name = "上下行";
                            upDownLink.Type = -3;
                            List<StationRailDto> sameStas = new List<StationRailDto>();
                            foreach (var up in upLink.Children)
                            {
                                foreach (var down in downLink.Children)
                                {
                                    //相同区间
                                    if ((up.StaId != down.StaId && up.Name.Contains('-') && up.Name == down.Name) || (
                                        up.StaId == down.StaId && up.Name == down.Name))
                                    {
                                        sameStas.Add(up);
                                    }
                                }
                            }
                            foreach (var s in sameStas)
                            {
                                StationRailDto ch = new StationRailDto();
                                ch.Id = Guid.NewGuid();
                                ch.StaId = s.StaId;
                                ch.Name = s.Name;
                                ch.Type = 3;
                                upDownLink.Children.Add(ch);
                            }
                            dto.Children.Add(upDownLink);
                        }
                    }
                }
                if (dto.Children.Count > 0)
                    res.Add(dto);
            }
            return res;



            //await Task.Run(() =>
            //{
            //    var totalOrg = _organizationRepository.Where(s => s.IsDeleted == false).ToList(); ;
            //    var allOrg = totalOrg;
            //    if (input.organizationId != null)
            //    {
            //        var org = allOrg.FirstOrDefault(s => s.Id == input.organizationId);
            //        if (org == null) throw new UserFriendlyException("组织机构错误");
            //        else
            //        {
            //            allOrg = allOrg.Where(s => s.Code.StartsWith(org.Code)).ToList();
            //        }
            //    }
            //    var allSta = _stationRepository.Where(s => s.IsDeleted == false);
            //    var allStaOrg = _station_OrgsRepository.Where(s => s.Id != null);
            //    var temp = from s in allSta
            //               join so in allStaOrg on s.Id equals so.StationId
            //               select new
            //               {
            //                   Station = s,
            //                   OrgId = so.OrganizationId
            //               };
            //    var allOrgIds = allOrg.Select(s => s.Id);
            //    foreach (var item in temp.Where(s => allOrgIds.Contains(s.OrgId)))
            //    {
            //        var code = allOrg.Find(s => s.Id == item.OrgId).Code;
            //        var index = code.LastIndexOf('.');
            //        if (index <= -1) index = code.Length;
            //        //var parentCode = code.Substring(0, code.Length > 4 ? code.Length - 4 : code.Length);
            //        var parentCode = code.Substring(0, index);
            //        var parentOrg = totalOrg.FirstOrDefault(z => z.Code == parentCode);
            //        var target = res.FirstOrDefault(s => s.Id == parentOrg.Id);
            //        StationRailDto staDto = new StationRailDto
            //        {
            //            Id = item.Station.Id,
            //            Name = item.Station.Name,
            //            Type = 1
            //        };
            //        if (target == null)
            //        {
            //            StationRailDto dto = new StationRailDto();
            //            dto.Id = parentOrg.Id;
            //            dto.Name = parentOrg.Name;
            //            dto.Type = 0;
            //            dto.Children.Add(staDto);
            //            res.Add(dto);
            //        }
            //        else if (target.Children.Find(s => s.Type == 1 && s.Id == staDto.Id) == null)
            //        {
            //            target.Children.Add(staDto);
            //        }
            //    }
            //});
            return res;
        }
        #endregion

        #region 根据组织机构获取车站
        /// <summary>
        /// 根据组织机构获取车站
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<List<StationDto>> GetListByOrg(Guid orgId)
        {
            List<StationDto> res = new List<StationDto>();
            if (orgId == null || orgId == Guid.Empty) return res;
            var org = await _organizationRepository.GetAsync(orgId);
            var code = org.Code;
            var orgIds = _organizationRepository.Where(s => s.Code.StartsWith(code)).Select(s => s.Id).ToList();
           // var staIds = _station_OrgsRepository.Where(s => orgIds.Contains(s.OrganizationId)).Select(s => s.StationId).Distinct();
            var staIds = _station_OrgsRepository.Select(s => s.StationId).Distinct();
            var stas = _stationRepository.Where(s => s.IsDeleted == false && staIds.Contains(s.Id));
            res = ObjectMapper.Map<List<Station>, List<StationDto>>(stas.ToList());
            return res;
        }
        #endregion

        #region 根据线路获取站点=======================================
        /// <summary>
        /// 根据线路主键获取站点列表
        /// </summary>
        public async Task<List<StationDto>> GetListByRailwayId(Guid railwayId)
        {
            var list = (List<StationDto>)null;

            await Task.Run(() =>
            {
                var q = from a in _sta_RaRepository
                        join b in _stationRepository
                        on a.StationId equals b.Id
                        where a.RailwayId == railwayId && b.IsDeleted == false
                        select b;
                list = ObjectMapper.Map<List<Station>, List<StationDto>>(q.Distinct().ToList());
                foreach (var item in list)
                {
                    var stationRltRailway = _sta_RaRepository.WithDetails().Where(x => x.StationId == item.Id).FirstOrDefault();
                    item.KMMark = stationRltRailway.KMMark;
                }
                var result = list.OrderBy(x=>x.KMMark);
               
            });

            return list;
        }
        #endregion

        #region 添加站点===============================================
        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Station.Create)]
        public async Task<StationSimpleDto> Create(StationInputDto input)
        {
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrWhiteSpace(input.Name))
                throw new UserFriendlyException("站点名称不能为空");
            if (_stationRepository.FirstOrDefault(s => !s.IsDeleted && s.Type == 0 && s.Name == input.Name) != null)
                throw new UserFriendlyException("此站点已存在");
            StationSimpleDto result = new StationSimpleDto();
            Station sta = new Station(_guidGenerator.Create());
            sta.IsDeleted = false;
            sta.Name = input.Name;
            sta.Remark = input.Remark;
            sta.Type = 0;
            ValidationMaxlength.Validate(sta);
            await _stationRepository.InsertAsync(sta);
            result = ObjectMapper.Map<Station, StationSimpleDto>(sta);
            //添加维护班组
            foreach (var item in input.RepairTeam)
            {
                StationRltOrganization staOrg = new StationRltOrganization(GuidGenerator.Create());
                staOrg.OrganizationId = item;
                staOrg.StationId = sta.Id;
                await _station_OrgsRepository.InsertAsync(staOrg);
            }
            return result;
        }

        #endregion

        #region 编辑站点===============================================
        /// <summary>
        /// 编辑站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Station.Update)]
        public async Task<StationSimpleDto> Update(StationUpdateDto input)
        {
            if (input.Id == Guid.Empty || input.Id == null) throw new UserFriendlyException("id不正确");
            if (string.IsNullOrEmpty(input.Name) || string.IsNullOrWhiteSpace(input.Name)) throw new UserFriendlyException("站点名称不能为空");
            var oldEnt = await _stationRepository.GetAsync(input.Id);
            if (oldEnt == null || oldEnt.IsDeleted) throw new UserFriendlyException("此站点不存在");
            if (_stationRepository.FirstOrDefault(s => !s.IsDeleted && s.Type == 0 && s.Name == input.Name && s.Id != input.Id) != null)
                throw new UserFriendlyException("此站点已存在");
            var oldName = oldEnt.Name.ToString();
            oldEnt.Name = input.Name;
            oldEnt.Remark = input.Remark;
            ValidationMaxlength.Validate(oldEnt);
            await _stationRepository.UpdateAsync(oldEnt);
            var oldSections = _stationRepository.Where(s => s.Type == 1 && !s.IsDeleted && (s.SectionStartStationId == input.Id || s.SectionEndStationId == input.Id));
            foreach (var item in oldSections)
            {
                item.Name = item.Name.Replace(oldName, input.Name);
                await _stationRepository.UpdateAsync(item);
            }
            //更新维护班组
            await _station_OrgsRepository.DeleteAsync(s => s.StationId == input.Id);
            foreach (var item in input.RepairTeam)
            {
                StationRltOrganization staOrg = new StationRltOrganization(GuidGenerator.Create());
                staOrg.OrganizationId = item;
                staOrg.StationId = input.Id;
                await _station_OrgsRepository.InsertAsync(staOrg);
            }
            return ObjectMapper.Map<Station, StationSimpleDto>(oldEnt);
        }
        #endregion

        #region 删除站点===============================================
        /// <summary>
        /// 删除站点
        /// </summary>
        /// <param name="id">站点Guid</param>
        /// <returns></returns>
        [Authorize(BasicPermissions.Station.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("id不正确");
            if (_stationRepository.FirstOrDefault(z => z.Id == id) == null) throw new UserFriendlyException("删除对象不存在");
            var allSta_Rail = _sta_RaRepository.WithDetails().ToList();
            //要删除站点相关区间
            var deleStas = _stationRepository.Where(s => s.IsDeleted == false && (s.SectionEndStationId == id || s.SectionStartStationId == id || s.Id == id)).ToList();
            var deleSta_Rails = allSta_Rail.Where(s => deleStas.Select(s => s.Id).Contains(s.StationId)).ToList();

            //相关线下的区间重新生成,依据删除后哪两个站之间不存在区间进行判断
            var railwayIds = allSta_Rail.Where(s => s.StationId == id).Select(s => s.RailwayId);
            foreach (var item in railwayIds)
            {
                //当前线下的 除去要删除的站以及区间
                var newSta_Rail = allSta_Rail.Where(s => s.RailwayId == item && deleSta_Rails.Contains(s) == false).OrderBy(s => s.PassOrder).ToList();
                bool isChange = false;//修改标识
                int passOrder = 0;
                for (int i = 0; i < newSta_Rail.Count; i++)
                {
                    var ele = newSta_Rail[i];
                    //修改剩余的站以及区间排序
                    if (isChange)
                    {
                        ele.PassOrder = passOrder++;
                        await _sta_RaRepository.UpdateAsync(ele);
                        continue;
                    }
                    if (i < newSta_Rail.Count - 1)
                    {
                        var nextEle = newSta_Rail[i + 1];
                        //相邻的两个都为站 即中间的站以及已生成的区间将要被删除 创建新的区间
                        if (ele.KMMark != 0 && nextEle.KMMark != 0)
                        {
                            passOrder = ele.PassOrder + 1;
                            Station newSection = new Station(GuidGenerator.Create());
                            newSection.Name = ele.Station.Name + "-" + nextEle.Station.Name + "区间";
                            newSection.Type = 1;
                            newSection.SectionEndStationId = nextEle.StationId;
                            newSection.SectionStartStationId = ele.StationId;
                            await _stationRepository.InsertAsync(newSection);
                            StationRltRailway sec_rail = new StationRltRailway(GuidGenerator.Create());
                            sec_rail.KMMark = 0;
                            sec_rail.PassOrder = passOrder++;
                            sec_rail.RailwayId = item;
                            sec_rail.StationId = newSection.Id;
                            await _sta_RaRepository.InsertAsync(sec_rail);
                            isChange = true;
                        }
                    }
                }
            }

            //删除站点以及区间
            await _sta_RaRepository.DeleteAsync(s => deleSta_Rails.Select(s => s.Id).Contains(s.Id));
            foreach (var item in deleStas)
            {
                item.IsDeleted = true;
                await _stationRepository.UpdateAsync(item);
            }

            //删除维护班组关联
            await _station_OrgsRepository.DeleteAsync(s => s.StationId == id);
            return true;
        }
        #endregion
        [Authorize(BasicPermissions.Station.Import)]
        /// <summary>
        /// 导入线路-站点excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Upload([FromForm] ImportData input)
        {
            await _fileImport.Start(input.ImportKey, 100);
            int successStaCount = 0;
            int successSectionCount = 0;
            int deleteStaCount = 0;
            string failMsg = "";
            DataTable dt = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0);
                dt = ExcelHelper.ExcelToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName);
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
            int dataRowIndex = -1;
            int nameIndex = -1;
            int railwayNameIndex = -1;
            int kmMarkIndex = -1;
            int downLinkKMMarkIndex = -1;
            await _fileImport.ChangeTotalCount(input.ImportKey, dt.Rows.Count);
            //寻找数据开始行序号以及对应列号
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows[i].ItemArray.Length; j++)
                {
                    var cellTxt = dt.Rows[i][j].ToString();
                    if (cellTxt.Contains(BasicSettings.DataImportRowFlag)) dataRowIndex = i;
                    if (cellTxt.Contains(BasicSettings.Name)) nameIndex = j;
                    if (cellTxt.Contains(BasicSettings.RailwayName)) railwayNameIndex = j;
                    if (cellTxt.Contains(BasicSettings.KilometerMark)) kmMarkIndex = j;
                    if (cellTxt.Contains(BasicSettings.DownLinkKMMark)) downLinkKMMarkIndex = j;

                }
                if (dataRowIndex != -1) break;
            }

            #region 标识列验证
            if (dataRowIndex == -1)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + BasicSettings.DataImportRowFlag + "不存在");
            }
            if (nameIndex == -1)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + BasicSettings.Name + "不存在");
            }
            if (railwayNameIndex == -1)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + BasicSettings.RailwayName + "不存在");
            }
            if (kmMarkIndex == -1)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + BasicSettings.KilometerMark + "不存在");
            }
            if (downLinkKMMarkIndex == -1)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + BasicSettings.DownLinkKMMark + "不存在");
            }
            #endregion

            await _fileImport.UpdateState(input.ImportKey, (decimal)(dt.Rows.Count * 0.015));
            List<Guid> stationIds = new List<Guid>();//站点id集合
            try
            {
                var allStation = _stationRepository.Where(s => s.IsDeleted == false).ToList();
                var allRailway = _railwayRepository.Where(s => !s.IsDeleted).ToList();
                var allSta_Rail = _sta_RaRepository.Where(s => s.Id != null).ToList();

                List<ImportTemplate> importTemplates = new List<ImportTemplate>();          //总添加数据
                List<Station> addedStations = new List<Station>();
                //数据获取
                bool isSkiping = false;

                List<WrongInfo> wrongInfos = new List<WrongInfo>();

                for (int i = dataRowIndex + 1; i < dt.Rows.Count; i++)
                {
                    var railwayName = dt.Rows[i][railwayNameIndex].ToString().Trim();
                    var railEnt = allRailway.FirstOrDefault(s => s.Name == railwayName);
                    var staName = dt.Rows[i][nameIndex].ToString().Trim();
                    string kmMark1 = dt.Rows[i][kmMarkIndex].ToString().Trim();
                    string kmMark2 = dt.Rows[i][downLinkKMMarkIndex].ToString().Trim();
                    //bool isRightKmMark = int.TryParse(dt.Rows[i][kmMarkIndex].ToString().Trim(), out kmMark);
                    //存在线路名称 添加线路
                    if (string.IsNullOrEmpty(railwayName) == false)
                    {
                        if (railEnt == null)
                        {
                            isSkiping = true;
                            failMsg += "线路:" + railwayName + "不存在，相关站点未添加成功\r\n";
                            WrongInfo wrong = new WrongInfo(i, "线路" + railwayName + "不存在");
                            wrongInfos.Add(wrong);
                            continue;
                        }
                        isSkiping = false;
                        Guid railID = railEnt.Id;
                        ImportTemplate tem = new ImportTemplate(railwayName, railID, railEnt.Type);
                        importTemplates.Add(tem);

                    }
                    else if (string.IsNullOrEmpty(staName) == false && !isSkiping) //不存在线路名称 存在车站名称
                    {
                        //将车站信息进行上下行区分 并放入importTemplates中的站点信息集合中
                        var sameSta = addedStations.FirstOrDefault(s => s.Name == staName);
                        int kmMark = -1;
                        //单线类型 或者复线的上行
                        if (!string.IsNullOrEmpty(kmMark1) && int.TryParse(kmMark1, out kmMark))
                        {
                            if (sameSta == null)
                            {
                                Station sta = new Station(Guid.NewGuid());
                                sta.Name = staName;
                                sta.Type = 0;
                                //sta.KMMark = kmMark;
                                importTemplates.Last().StationInfos.Add(new StationTemp(sta, kmMark));
                                addedStations.Add(sta);
                            }
                            else
                            {
                                importTemplates.Last().StationInfos.Add(new StationTemp(sameSta, kmMark));
                            }
                        }
                        if (importTemplates.Last().RailwayType == RailwayType.COMPLEXLINE)
                        {
                            int kmMark2Num = -1;
                            //存在下行
                            if (!string.IsNullOrEmpty(kmMark2) && int.TryParse(kmMark2, out kmMark2Num))
                            {
                                sameSta = addedStations.FirstOrDefault(s => s.Name == staName);
                                if (sameSta == null)
                                {
                                    Station sta = new Station(Guid.NewGuid());
                                    sta.Name = staName;
                                    sta.Type = 0;
                                    //sta.KMMark = kmMark;
                                    importTemplates.Last().StationInfos2.Add(new StationTemp(sta, kmMark2Num));
                                    addedStations.Add(sta);
                                }
                                else
                                {
                                    importTemplates.Last().StationInfos2.Add(new StationTemp(sameSta, kmMark2Num));
                                }
                            }

                        }

                        //if (sameSta == null)
                        //{
                        //    Station sta = new Station(Guid.NewGuid());
                        //    sta.Name = staName;
                        //    sta.Type = 0;
                        //    //sta.KMMark = kmMark;
                        //    importTemplates.Last().StationInfos.Add(new StationTemp(sta, kmMark, relateType));
                        //    addedStations.Add(sta);
                        //}
                        //else
                        //{
                        //    importTemplates.Last().StationInfos.Add(new StationTemp(sameSta, kmMark, relateType));
                        //}
                    }
                }
                using var unitWork = _unitOfWorkManager.Begin(true, false);
                //添加或更新站点数据
                for (int i = 0; i < addedStations.Count; i++)
                {
                    var sta = addedStations[i];
                    var oldSta = allStation.FirstOrDefault(s => s.Name == sta.Name && s.IsDeleted == false && s.Type == 0);
                    if (oldSta != null)
                    {
                        oldSta.Remark = sta.Remark;
                        await _stationRepository.UpdateAsync(oldSta);
                        sta = new Station(oldSta.Id);
                        sta.Name = oldSta.Name;
                        sta.Remark = oldSta.Remark;
                        sta.Type = 0;
                        addedStations[i] = sta;
                        foreach (var item in importTemplates)
                        {
                            for (int j = 0; j < item.StationInfos.Count; j++)
                            {
                                var info = item.StationInfos[j];
                                if (info.Station.Name == sta.Name)
                                {
                                    info.Station = sta;
                                }
                            }
                        }
                        successStaCount++;
                    }
                    else
                    {
                        await _stationRepository.InsertAsync(sta);
                        successStaCount++;
                    }
                }
                await unitWork.SaveChangesAsync();
                //数据添加
                foreach (var item in importTemplates)
                {
                    if (importTemplates.IndexOf(item) > (decimal)(dt.Rows.Count * 0.015))
                        await _fileImport.UpdateState(input.ImportKey, importTemplates.IndexOf(item));
                    List<Station> newStas4SingleOrUplink = new List<Station>();
                    var oldStas = (from r in allSta_Rail.Where(s => s.RailwayId == item.RailwayId) join s in allStation on r.StationId equals s.Id select s).ToList();
                    var olsSection = oldStas.Where(s => s.Type == 1).ToList();
                    var sllStaList = allStation.ToList();
                    for (int i = 0; i < item.StationInfos.Count; i++)
                    {
                        var sta = item.StationInfos[i].Station;
                        newStas4SingleOrUplink.Add(addedStations.FirstOrDefault(s => s.Name == sta.Name));
                    }
                    if (item.RailwayType == RailwayType.MONGLINE)
                    {
                        await AddOrUpdateStaRltRailways(sllStaList, item, newStas4SingleOrUplink, oldStas, RelateRailwayType.SINGLELINK);
                    }
                    else if (item.RailwayType == RailwayType.COMPLEXLINE)
                    {
                        await AddOrUpdateStaRltRailways(sllStaList, item, newStas4SingleOrUplink, oldStas, RelateRailwayType.UPLINK);

                        List<Station> downlinkNewStas = new List<Station>();
                        for (int i = 0; i < item.StationInfos2.Count; i++)
                        {
                            var sta = item.StationInfos2[i].Station;
                            downlinkNewStas.Add(addedStations.FirstOrDefault(s => s.Name == sta.Name));
                        }
                        await AddOrUpdateStaRltRailways(sllStaList, item, downlinkNewStas, oldStas, RelateRailwayType.DOWNLINK);
                    }
                }

                await _fileImport.UpdateState(input.ImportKey, dt.Rows.Count - 1);
                await _fileImport.Complete(input.ImportKey);

                //软删除excel中不存在的数据
                //var softDeleteManu = allStation.Where(s => !stationIds.Contains(s.Id));
                //foreach (var item in softDeleteManu)
                //{
                //    item.IsDelete = true;
                //    await _stationRepository.UpdateAsync(item);
                //}
                // failMsg += "成功导入" + successStaCount + "条站点数据，自动添加" + successSectionCount + "条区间数据。删除" + deleteStaCount + "条站点数据。";
                if (wrongInfos.Count > 0)
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
            catch (Exception e)
            {
                await _fileImport.Cancel(input.ImportKey);
            }
        }

        private async Task AddOrUpdateStaRltRailways(List<Station> allStation, ImportTemplate item, List<Station> newStas, List<Station> oldStas, RelateRailwayType relateType)
        {
            using var unitWork = _unitOfWorkManager.Begin(true, false);
            //添加或处理已有区间
            List<Station> staWithSection = new List<Station>();
            for (int i = 0; i < newStas.Count; i++)
            {
                var sta = newStas[i];
                staWithSection.Add(sta);
                var nextSta = i < newStas.Count - 1 ? newStas[i + 1] : null;
                if (nextSta != null)
                {
                    var sectionName = sta.Name + '-' + nextSta.Name + "区间";
                    var existSection = allStation.FirstOrDefault(s => s.Name == sectionName && s.Type == 1);
                    if (existSection == null)
                    {
                        Station section = new Station(Guid.NewGuid());
                        section.Name = sectionName;
                        section.SectionStartStationId = sta.Id;
                        section.SectionEndStationId = nextSta.Id;
                        section.Type = 1;
                        staWithSection.Add(await _stationRepository.InsertAsync(section));
                    }
                    else
                    {
                        staWithSection.Add(existSection);
                    }
                }
            }
            //添加关联关系
            await _sta_RaRepository.DeleteAsync(s => s.RailwayId == item.RailwayId && s.RailwayType == relateType);
            for (int i = 0; i < staWithSection.Count; i++)
            {
                var sta = staWithSection[i];
                StationRltRailway sta_rail = new StationRltRailway(Guid.NewGuid());
                sta_rail.RailwayId = item.RailwayId;
                sta_rail.StationId = sta.Id;
                sta_rail.PassOrder = i;
                sta_rail.RailwayType = relateType;
                if (sta.Type == 0)
                {
                    StationTemp existSta = null;
                    switch (relateType)
                    {
                        case RelateRailwayType.SINGLELINK:
                        case RelateRailwayType.UPLINK:
                            existSta = item.StationInfos.FirstOrDefault(s => s.Station.Name == sta.Name);
                            break;
                        case RelateRailwayType.DOWNLINK:
                            existSta = item.StationInfos2.FirstOrDefault(s => s.Station.Name == sta.Name);
                            break;
                    }
                    if (existSta == null) continue;
                    sta_rail.KMMark = existSta.KMMark;
                }
                await _sta_RaRepository.InsertAsync(sta_rail);
            }
            //软删除 站点以及相关区间
            var softDeleteSta = oldStas.Where(s => !newStas.Select(s => s.Name).Contains(s.Name) && s.Type == 0);
            foreach (var deleteSta in softDeleteSta)
            {
                deleteSta.IsDeleted = true;
                await _stationRepository.UpdateAsync(deleteSta);
                await _sta_RaRepository.DeleteAsync(s => s.StationId == deleteSta.Id && s.RailwayType == relateType);
                var deleteSections = oldStas.Where(s => s.Type == 1 && (s.SectionStartStationId == deleteSta.Id || s.SectionEndStationId == deleteSta.Id));
                foreach (var sec in deleteSections)
                {
                    sec.IsDeleted = true;
                    await _stationRepository.UpdateAsync(sec);
                    await _sta_RaRepository.DeleteAsync(s => s.StationId == sec.Id && s.RailwayType == relateType);
                }
            }
            await unitWork.CompleteAsync();
        }

        /// <summary>
        /// 导出线路-站点excel
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Stream> Export(StationExportData input)
        {
            List<StationRltRailway> stationList = new List<StationRltRailway>();
            var list = GetStationData(input.paramter);
            var SRR = await _sta_RaRepository.GetListAsync();
            var railwayList = await _railwayRepository.GetListAsync();

            foreach (var item in list)
            {
                var srr = SRR.Where(x => x.StationId == item.Id);
                foreach (var ite in srr)
                {
                    stationList.Add(ite); //根据查到的Station获取其和关联线路的具体信息。
                }
            }
            var staList = stationList.GroupBy(x => x.RailwayId).ToList(); //将查到的关联信息按线路分组

            if (staList.Count == 0)
            {
                throw new UserFriendlyException("不存在任何线路，无法导出");
            }

            var stations = new List<StationImportTemplate>();
            var station = new StationImportTemplate();

            foreach (var item in staList)
            {
                foreach (var ite in item)
                {
                    station = new StationImportTemplate();
                    var srr = stationList.Where(x => x.StationId == ite.StationId && x.RailwayId == ite.RailwayId).ToList();
                    if (srr.Count > 0)
                    {
                        var railway = railwayList.FirstOrDefault(x => x.Id == ite.RailwayId);
                        if (!stations.Any(x => x.RailwayName == railway.Name)) //同一线路下：先找线是否存在
                        {
                            station.RailwayName = railway.Name;
                            stations.Add(station);
                            station = new StationImportTemplate();
                        }
                        if (!stations.Any(x => x.Name == ite.Station.Name && x.RailwayName == railway.Name)) //再找站点是否存在
                        {
                            station.RailwayName = railway.Name; //为了此判断，先将线名和站名挨个全加上，
                            foreach (var s in srr)
                            {
                                if (s.RailwayType == RelateRailwayType.SINGLELINK)
                                {
                                    station.KilometerMark = s.KMMark.ToString();
                                }
                                else if (s.RailwayType == RelateRailwayType.UPLINK)
                                {
                                    station.KilometerMark = s.KMMark.ToString();
                                }
                            }
                            station.DownLinkKMMark = srr.FirstOrDefault(x => x.RailwayType == RelateRailwayType.DOWNLINK)?.KMMark.ToString();
                            station.Name = ite.Station.Name;
                            stations.Add(station);
                        }
                    }
                }
            }

            foreach (var item in stations) //遍历stations，将线名与站名同时存在的一条数据，线名去掉。(格式化)
            {
                if ((item.RailwayName != null && item.Name != null) || (item.RailwayName != null && item.KilometerMark != null) || (item.RailwayName != null && item.DownLinkKMMark != null))
                {
                    item.RailwayName = null;
                }
            }

            var dtoList = ObjectMapper.Map<List<StationImportTemplate>, List<StationImportTemplate>>(stations);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        #region 私有方法

        private List<Station> GetStationData(StationSearchDto input)
        {
            // 获取当前登录用户的组织机构
            //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            //var organization = !string.IsNullOrEmpty(organizationIdString) ? _organizationRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            //var organizationCode = organization != null ? organization.Code : null;

            var allStaRa = _sta_RaRepository.WithDetails().Where(s => s.RailwayId != null && s.StationId != null);
            var staEnts = _stationRepository.Where(s => !s.IsDeleted && s.Type == 0).WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name)).ToList();

            using (DataFilter.Disable<ISoftDelete>())
            {
                //if (organization != null && !string.IsNullOrEmpty(organizationCode))
                //{
                //    List<Station> outOfOrgStations = new List<Station>();
                //    foreach (var item in staEnts)
                //    {
                //        if (_station_OrgsRepository.FirstOrDefault(x => x.StationId == item.Id) == null)
                //        {
                //            outOfOrgStations.Add(item);
                //        }
                //    }
                //    var staIds = _station_OrgsRepository.Where(s => s.Organization.Code.StartsWith(organizationCode)).Select(s => s.StationId).ToList();
                //    staEnts = staEnts.Where(s => staIds.Contains(s.Id)).ToList();
                //    staEnts.AddRange(outOfOrgStations);
                //    staEnts.Distinct();
                //}
                //List<Station> outOfOrgStations = new List<Station>();
                //var staIds = _station_OrgsRepository.Select(s => s.StationId).ToList();
                //staEnts = staEnts.Where(s => staIds.Contains(s.Id)).ToList();
                //staEnts.AddRange(outOfOrgStations);
                //staEnts.Distinct();
            }

            if (input.BelongRaId != Guid.Empty && input.BelongRaId != null)
            {
                var rail_staEnts = allStaRa.Where(s => s.RailwayId == input.BelongRaId);
                var rail_staIds = rail_staEnts.Select(s => s.StationId).ToList();
                staEnts = staEnts.Where(s => rail_staIds.Contains(s.Id)).ToList();
            }
            //if (input.RepairTeamId != Guid.Empty && input.RepairTeamId != null)
            //{
            //    var staIds = _station_OrgsRepository.Where(s => s.OrganizationId == input.RepairTeamId).Select(s => s.StationId).ToList();
            //    staEnts = staEnts.Where(s => staIds.Contains(s.Id)).ToList();
            //}

            return staEnts;
        }


        /// <summary>
        /// 重新生成所属线路下的站/区间-线路关联数据  弃用：此关系在线路中设置
        /// </summary>
        /// <param name="input">站点所属信息</param>
        /// <param name="sta">新站点信息</param>
        /// <returns></returns>
        private /*async Task*/ void ReBuildStaSec_RailRelation(StationInputDto input, Station sta)
        {
            //var railwayIds = input.BelongRailway.Select(s => s.RailwayId).ToList();
            //var rail_stas = _sta_RaRepository.WithDetails().Where(s => railwayIds.Contains(s.RailwayId)).ToList();
            //var rail_Section = rail_stas.Where(s => s.KMMark == 0).ToList();
            //foreach (var item in input.BelongRailway)
            //{
            //    var itemRail_sta = rail_stas.Where(s => s.RailwayId == item.RailwayId && s.KMMark != 0).OrderBy(s => s.PassOrder).ToList();
            //    for (int i = 0; i < itemRail_sta.Count; i++)
            //    {
            //        var nowEle = itemRail_sta[i];
            //        //小于所有站点里程
            //        if (item.KMMark < nowEle.KMMark && i == 0)
            //        {
            //            Station staSec = new Station(GuidGenerator.Create());
            //            staSec.Name = sta.Name + "-" + nowEle.Station.Name + "区间";
            //            staSec.SectionStartStationId = sta.Id;
            //            staSec.SectionEndStationId = nowEle.StationId;
            //            staSec.Type = 1;
            //            await _stationRepository.InsertAsync(staSec);

            //            int passOrder = 0;
            //            Station_Railway newSta_Rail = new Station_Railway(GuidGenerator.Create());
            //            newSta_Rail.KMMark = item.KMMark;
            //            newSta_Rail.PassOrder = passOrder++;
            //            newSta_Rail.RailwayId = item.RailwayId;
            //            newSta_Rail.StationId = sta.Id;
            //            await _sta_RaRepository.InsertAsync(newSta_Rail);

            //            Station_Railway foreSection_Rail = new Station_Railway(GuidGenerator.Create());
            //            foreSection_Rail.KMMark = 0;
            //            foreSection_Rail.PassOrder = passOrder++;
            //            foreSection_Rail.RailwayId = item.RailwayId;
            //            foreSection_Rail.StationId = staSec.Id;
            //            await _sta_RaRepository.InsertAsync(foreSection_Rail);
            //            //更新原来所有的线路站点数据
            //            var oldSta_Rails = rail_stas.Where(s => s.RailwayId == item.RailwayId).OrderBy(s => s.PassOrder);
            //            foreach (var old in oldSta_Rails)
            //            {
            //                old.PassOrder = passOrder++;
            //                await _sta_RaRepository.UpdateAsync(old);
            //            }
            //        }
            //        else if (i < itemRail_sta.Count - 1)
            //        {
            //            var nextEle = itemRail_sta[i + 1];
            //            //在已有站点之间，删除原来两站点的区间信息，生产新的区间信息
            //            if (item.KMMark >= nowEle.KMMark && item.KMMark <= nextEle.KMMark)
            //            {
            //                var oldSection = rail_Section.FirstOrDefault(s => s.RailwayId == item.RailwayId && s.PassOrder == (nextEle.PassOrder - 1));
            //                Station foreStaSec = new Station(GuidGenerator.Create());
            //                foreStaSec.Name = nowEle.Station.Name + "-" + sta.Name + "区间";
            //                foreStaSec.SectionStartStationId = nowEle.StationId;
            //                foreStaSec.SectionEndStationId = sta.Id;
            //                foreStaSec.Type = 1;
            //                await _stationRepository.InsertAsync(foreStaSec);
            //                Station afterStaSection = new Station(GuidGenerator.Create());
            //                afterStaSection.Name = sta.Name + "-" + nextEle.Station.Name + "区间";
            //                afterStaSection.SectionStartStationId = sta.Id;
            //                afterStaSection.SectionEndStationId = nextEle.StationId;
            //                afterStaSection.Type = 1;
            //                await _stationRepository.InsertAsync(afterStaSection);

            //                int passOrder = nowEle.PassOrder;
            //                Station_Railway foreSection_Rail = new Station_Railway(GuidGenerator.Create());
            //                foreSection_Rail.KMMark = 0;
            //                foreSection_Rail.PassOrder = ++passOrder;
            //                foreSection_Rail.RailwayId = item.RailwayId;
            //                foreSection_Rail.StationId = foreStaSec.Id;
            //                await _sta_RaRepository.InsertAsync(foreSection_Rail);
            //                Station_Railway theNewSta_Rail = new Station_Railway(GuidGenerator.Create());
            //                theNewSta_Rail.KMMark = item.KMMark;
            //                theNewSta_Rail.PassOrder = ++passOrder;
            //                theNewSta_Rail.RailwayId = item.RailwayId;
            //                theNewSta_Rail.StationId = sta.Id;
            //                await _sta_RaRepository.InsertAsync(theNewSta_Rail);
            //                Station_Railway afterSection_Rail = new Station_Railway(GuidGenerator.Create());
            //                afterSection_Rail.KMMark = 0;
            //                afterSection_Rail.PassOrder = ++passOrder;
            //                afterSection_Rail.RailwayId = item.RailwayId;
            //                afterSection_Rail.StationId = afterStaSection.Id;
            //                await _sta_RaRepository.InsertAsync(afterSection_Rail);

            //                //修改原有在插入位置之后的线路站点关系
            //                var oldSta_Rails = rail_stas.Where(s => s.RailwayId == item.RailwayId && s.PassOrder > nowEle.PassOrder);
            //                if (oldSection != null)
            //                {
            //                    await _sta_RaRepository.DeleteAsync(oldSection.Id);
            //                    oldSta_Rails = oldSta_Rails.Where(s => s.Id != oldSection.Id).OrderBy(s => s.PassOrder);
            //                }
            //                foreach (var old in oldSta_Rails)
            //                {
            //                    old.PassOrder = ++passOrder;
            //                    await _sta_RaRepository.UpdateAsync(old);
            //                }
            //            }
            //        }
            //        //大于所有站点里程
            //        else if (i == itemRail_sta.Count - 1 && itemRail_sta[i].KMMark < item.KMMark)
            //        {
            //            var oldSta_Rails = rail_stas.Where(s => s.RailwayId == item.RailwayId).OrderBy(s => s.PassOrder);

            //            Station staSec = new Station(GuidGenerator.Create());
            //            staSec.Name = nowEle.Station.Name + "-" + sta.Name + "区间";
            //            staSec.SectionStartStationId = nowEle.StationId;
            //            staSec.SectionEndStationId = sta.Id;
            //            staSec.Type = 1;
            //            await _stationRepository.InsertAsync(staSec);
            //            Station_Railway section_Rail = new Station_Railway(GuidGenerator.Create());
            //            section_Rail.KMMark = 0;
            //            section_Rail.PassOrder = oldSta_Rails.Last().PassOrder + 1;
            //            section_Rail.RailwayId = item.RailwayId;
            //            section_Rail.StationId = staSec.Id;
            //            await _sta_RaRepository.InsertAsync(section_Rail);

            //            Station_Railway sta_Rail = new Station_Railway(GuidGenerator.Create());
            //            sta_Rail.KMMark = item.KMMark;
            //            sta_Rail.PassOrder = section_Rail.PassOrder + 1;
            //            sta_Rail.RailwayId = item.RailwayId;
            //            sta_Rail.StationId = sta.Id;
            //            await _sta_RaRepository.InsertAsync(sta_Rail);
            //        }
            //    }
            //}
        }

        #endregion
    }
}
struct ImportTemplate
{
    public ImportTemplate(string railName, Guid railwayId, RailwayType railwayType = RailwayType.MONGLINE)
    {
        RailwayId = railwayId;
        RailwayName = railName;
        RailwayType = railwayType;
        StationInfos = new List<StationTemp>();
        StationInfos2 = new List<StationTemp>();
    }
    public string RailwayName { get; set; }
    public Guid RailwayId { get; set; }
    public RailwayType RailwayType { get; set; }
    public List<StationTemp> StationInfos { get; set; }
    public List<StationTemp> StationInfos2 { get; set; }
}

class StationTemp
{
    public StationTemp(Station sta, int mark)
    {
        Station = sta;
        KMMark = mark;
    }
    public Station Station { get; set; }
    public int KMMark { get; set; }
}
