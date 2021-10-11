using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Basic.Authorization;
using SnAbp.Basic.Dtos;
using SnAbp.Basic.Dtos.Import;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Enums;
using SnAbp.Basic.IServices;
using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using SnAbp.Utils;
using Volo.Abp.Uow;
using Volo.Abp.Data;
using System.IO;
using SnAbp.Utils.DataImport;
using SnAbp.Basic.Template;

namespace SnAbp.Basic.Services
{
    [Authorize]
    public class BasicInstallationSiteAppService : ApplicationService, IBasicInstallationSiteAppService
    {
        private readonly IRepository<InstallationSite, Guid> installationSitesRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<DataDictionary, Guid> dataDicRepository;
        private readonly IRepository<Organization, Guid> orgRepository;
        private readonly IRepository<Railway, Guid> railwayRepository;
        private readonly IRepository<RailwayRltOrganization, Guid> _railwayRltOrganization;
        private readonly IRepository<StationRltRailway, Guid> _stationRltRailways;
        private readonly IRepository<Station, Guid> stationRepository;
        private readonly IRepository<StationRltRailway, Guid> sta_railwayRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;  //获取当前用户登录的组织机构仓储
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        protected IDataFilter DataFilter { get; }
        public BasicInstallationSiteAppService(
            IRepository<InstallationSite, Guid> _installationSitesRep,
            IRepository<DataDictionary, Guid> dataDicRep,
            IRepository<Organization, Guid> orgRep,
            IRepository<Railway, Guid> railwayRep,
            IRepository<Station, Guid> stationRep,
            IRepository<RailwayRltOrganization, Guid> railwayRltOrganization,
            IRepository<StationRltRailway, Guid> stationRltRailways,
            IRepository<StationRltRailway, Guid> sta_railwayRep,
            IGuidGenerator guidGenerator,
            IHttpContextAccessor httpContextAccessor,
            IFileImportHandler fileImport,
            IDataFilter dataFilter,
            IUnitOfWorkManager unitOfWorkManager)
        {
            installationSitesRepository = _installationSitesRep;
            dataDicRepository = dataDicRep;
            orgRepository = orgRep;
            railwayRepository = railwayRep;
            stationRepository = stationRep;
            _railwayRltOrganization = railwayRltOrganization;
            _stationRltRailways = stationRltRailways;
            sta_railwayRepository = sta_railwayRep;
            _guidGenerator = guidGenerator;
            _httpContextAccessor = httpContextAccessor;
            _fileImport = fileImport;
            _unitOfWorkManager = unitOfWorkManager;
            DataFilter = dataFilter;
        }

        #region 根据组织机构id获取安装位置信息=======================================
        public async Task<List<GetListByScopeOutDto>> GetListByScope(GetListByScopeInputDto input)
        {
            //将传入的字符串解析
            List<Scope> scopeList = new List<Scope>();
            string orgId = null;
            string railwayId = null;
            string stationId = null;
            string installationSiteId = null;

            if (input.ParentScopeCode != null || input.InitialScopeCode != null)
            {
                bool temp = false;
                if (input.ParentScopeCode != null)
                {
                    string[] list = input.ParentScopeCode.Split('.');
                    foreach (var item in list)
                    {
                        Scope sc = new Scope(item);
                        scopeList.Add(sc);
                    }
                }
                else
                {
                    string[] list = input.InitialScopeCode.Split('.');
                    foreach (var item in list)
                    {
                        Scope sc = new Scope(item);
                        if (sc.Type == ScopeType.Railway)
                        {
                            temp = true;
                        }
                        scopeList.Add(sc);
                    }
                }
                if (input.Type == ScopeType.Organization)
                {
                    orgId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Organization).Id.ToString();
                }

                if (input.Type == ScopeType.Railway)
                {
                    orgId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Organization).Id.ToString();
                    railwayId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Railway).Id.ToString();

                }

                if (input.Type == ScopeType.Station)
                {
                    orgId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Organization).Id.ToString();
                    railwayId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Railway).Id.ToString();
                    stationId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Station).Id.ToString();
                }

                if (input.Type == ScopeType.InstallationSite)
                {
                    if (temp)
                    {
                        orgId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Organization).Id.ToString();
                        railwayId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Railway).Id.ToString();
                        stationId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Station).Id.ToString();
                        installationSiteId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.InstallationSite).Id.ToString();
                    }
                    else
                    {
                        orgId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.Organization).Id.ToString();
                        installationSiteId = scopeList.LastOrDefault(scopeDto => scopeDto.Type == ScopeType.InstallationSite).Id.ToString();
                    }

                }

            }

            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();

            if (input.ParentScopeCode == null && input.InitialScopeCode == null)
            {
                scopeInstallationSites = ReturnCurrentOrganization();

            }
            else if (input.ParentScopeCode != null && input.InitialScopeCode == null)
            {
                //当前组织机构
                var organizationId = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"];

                //传入的
                List<Organization> organizationList = GetParents(orgId, organizationId, new List<Organization>());

                if (organizationList.Count == 0) throw new UserFriendlyException("传入的数据有误");

                //传入组织机构时，需要加载该组织机构子级，该组织机构下的线路，该组织机构下的非沿线的安装位置
                if (input.Type == ScopeType.Organization)
                {
                    scopeInstallationSites = GetListWithOrg(input);
                }

                //传入线路时，查找该线路下的站点
                else if (input.Type == ScopeType.Railway)
                {
                    //查询站点
                    var stations = sta_railwayRepository
                        .WithDetails()
                        .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.Railway,
                            x => x.RailwayId == input.Id);
                    var organizationRltRailway = _railwayRltOrganization
                        .WithDetails(x => x.Organization, x => x.Railway)
                        .Where(x => x.OrganizationId.ToString() == orgId && x.RailwayId.ToString() == railwayId)
                        .ToList();

                    List<Guid> staList = new List<Guid>();
                    //组装数据
                    if (stations.Any())
                    {
                        foreach (var item in stations)
                        {
                            GetListByScopeOutDto sisd = new GetListByScopeOutDto
                            {
                                Name = item.Station.Name,
                                Type = ScopeType.Station,
                                Id = (Guid)item.StationId,
                                ScopeCode = GetType(ScopeType.Organization) + "@" + organizationRltRailway.First().Organization.Name + "@" + organizationRltRailway.First().OrganizationId + "." +
                                            GetType(ScopeType.Railway) + "@" + organizationRltRailway.First().Railway.Name + "@" + organizationRltRailway.First().RailwayId + "." +
                                            GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId,
                            };
                            scopeInstallationSites.Add(sisd);
                            staList.Add(item.StationId);
                        }
                    }

                    //查询站点下的安装位置
                    var installationSites = installationSitesRepository
                                .WithDetails()
                                .WhereIf(staList.Any(), x => x.RailwayId == input.Id && staList.Contains((Guid)x.StationId) && x.ParentId == null);

                    if (installationSites.Any())
                    {
                        foreach (var insItem in installationSites)
                        {
                            GetListByScopeOutDto sisd = new GetListByScopeOutDto
                            {
                                ParentId = insItem.StationId,
                                Name = insItem.Name,
                                Type = ScopeType.InstallationSite,
                                Id = (Guid)insItem.Id,
                                ScopeCode = GetType(ScopeType.Organization) + "@" + insItem.Organization.Name + "@" + insItem.OrganizationId + "." +
                                            GetType(ScopeType.Railway) + "@" + insItem.Railway.Name + "@" + insItem.Id + "." +
                                            GetType(ScopeType.Station) + "@" + insItem.Station.Name + "@" + insItem.StationId + "." +
                                            GetType(ScopeType.InstallationSite) + "@" + insItem.Name + "@" + insItem.Id
                            };
                            scopeInstallationSites.Add(sisd);
                        }
                    }
                }

                //传入站点时，查找该站点下的安装位置
                else if (input.Type == ScopeType.Station)
                {
                    //查询站点下的安装位置
                    var installationSites = installationSitesRepository
                        .WithDetails()
                        .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.Station,
                            x => x.StationId == input.Id &&
                                 x.RailwayId.ToString() == railwayId && x.ParentId == null);

                    if (installationSites.Any())
                    {
                        foreach (var item in installationSites)
                        {
                            GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                            if (item.Children != null)
                            {
                                foreach (var e in item.Children)
                                {
                                    GetListByScopeOutDto sisdChild = new GetListByScopeOutDto
                                    {
                                        ParentId = item.Id,
                                        Type = ScopeType.InstallationSite,
                                        Name = e.Name,
                                        Id = e.Id,
                                    };
                                    scopeInstallationSites.Add(sisdChild);
                                }
                            }
                            sisd.Name = item.Name;
                            sisd.Type = ScopeType.InstallationSite;
                            sisd.Id = item.Id;
                            sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                             GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                             GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId + "." +
                                             GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                            scopeInstallationSites.Add(sisd);
                        }
                    }
                }
                else if (input.Type == ScopeType.InstallationSite)
                {
                    //查询安装位置
                    var installationSites = installationSitesRepository
                        .WithDetails(x => x.Parent.Organization, x => x.Parent.Railway, x => x.Parent.Station)
                        .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.InstallationSite,
                            x => x.ParentId == input.Id);
                    if (installationSites.Any())
                    {
                        foreach (var item in installationSites)
                        {
                            GetListByScopeOutDto sisd = new GetListByScopeOutDto();

                            if (item.ParentId != Guid.Empty && item.ParentId != null)
                            {
                                sisd.ParentId = item.ParentId;
                            }

                            if (item.Children != null)
                            {
                                foreach (var e in item.Children)
                                {
                                    GetListByScopeOutDto sisdChild = new GetListByScopeOutDto
                                    {
                                        Type = ScopeType.InstallationSite,
                                        Name = e.Name,
                                        Id = e.Id,
                                        ParentId = e.ParentId,
                                        ScopeCode = GetType(ScopeType.Organization) + "@" + e.Organization.Name + "@" + e.OrganizationId + "." +
                                                    GetType(ScopeType.Railway) + "@" + e.Railway.Name + "@" + e.RailwayId + "." +
                                                    GetType(ScopeType.Station) + "@" + e.Station.Name + "@" + e.StationId + "." +
                                                    GetType(ScopeType.InstallationSite) + "@" + e.Parent.Name + "@" + e.ParentId + "." +
                                                    GetType(ScopeType.InstallationSite) + "@" + e.Name + "@" + e.Id
                                    };
                                    scopeInstallationSites.Add(sisd);
                                }
                            }
                            sisd.Name = item.Name;
                            sisd.Type = ScopeType.InstallationSite;
                            sisd.Id = item.Id;
                            sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Parent.Organization.Name + "@" + item.Parent.OrganizationId + "." +
                                             GetType(ScopeType.Railway) + "@" + item.Parent.Railway.Name + "@" + item.Parent.RailwayId + "." +
                                             GetType(ScopeType.Station) + "@" + item.Parent.Station.Name + "@" + item.Parent.StationId + "." +
                                             GetType(ScopeType.InstallationSite) + "@" + item.Parent.Name + "@" + item.ParentId + "." +
                                             GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                            scopeInstallationSites.Add(sisd);

                        }
                    }

                }
            }
            else if (input.InitialScopeCode != null && input.ParentScopeCode == null)
            {
                //当前组织机构
                var organizationId = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"];

                //传入的组织机构和用户所在组织机构之间的组织机构
                List<Organization> organizationList = GetParents(orgId, organizationId, new List<Organization>());
                List<Guid> orgsList = new List<Guid>();

                var organization = organizationList.Find(x => x.Id.ToString() == orgId);


                while (organization != null && organization.Parent != null)
                {
                    orgsList.Add(organization.Id);
                    organization = organizationList.Find(x => x.Id == organization.ParentId);
                }

                if (organization != null)
                {
                    orgsList.Add(organization.Id);
                }
                List<Organization> organizationsList = new List<Organization>();
                List<Guid> orgList = new List<Guid>();

                //if (organizationList.Count == 0)
                //{
                //    return null;
                //}
                //else
                //{
                //    if (organizationList.Any())
                //    {
                //        foreach (var item in organizationList)
                //        {
                //            if (organizationsList.Find(x => x.Id == item.Id) == null && orgsList.FindAll(x => x == item.Id).Count == 0)
                //            {
                //                organizationsList.Add(item);
                //                orgList.Add(item.Id);
                //            }
                //        }

                //    }

                //}
                //if (input.Type == ScopeType.Organization)
                //{
                //    scopeInstallationSites = ReturnScopesOrganizations(orgsList);
                //    if (orgList.Count > 0)
                //    {
                //        scopeInstallationSites.AddRange(ReturnScopesByOrgList(orgList));
                //    }

                //    var scope = scopeInstallationSites.Find(x => x.Id.ToString() == orgId);
                //    var parentScope = scopeInstallationSites.Find(x => x.Id == scope.ParentId);
                //    List<Guid> stationList = new List<Guid>();
                //    var raiwayList = scopeInstallationSites.FindAll(x => x.ParentId == scope.ParentId && x.Type == ScopeType.Railway);
                //    if (raiwayList.Any())
                //    {
                //        foreach (var item in raiwayList)
                //        {
                //            stationList.Add(item.Id);
                //        }
                //        var stations = sta_railwayRepository
                //            .Where(x => stationList.Contains((Guid)x.RailwayId));

                //        if (stations.Any())
                //        {
                //            foreach (var item in stations)
                //            {
                //                GetListByScopeOutDto sisd = new GetListByScopeOutDto
                //                {
                //                    Id = item.StationId,
                //                    ParentId = item.RailwayId,
                //                    Name = item.Station.Name,
                //                    Type = ScopeType.Station,
                //                    ScopeCode = GetType(ScopeType.Organization) + "@" + parentScope.Name + "@" + parentScope.Id + "." +
                //                                GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                //                                GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId,
                //                };
                //                scopeInstallationSites.Add(sisd);
                //            }
                //        }
                //    }
                //}
                // else 
                if (input.Type == ScopeType.Railway)
                {
                    scopeInstallationSites = ReturnScopesOrganizations(orgsList);
                    if (orgList.Count > 0)
                    {
                        scopeInstallationSites.AddRange(ReturnScopesByOrgList(orgList));
                    }
                    var organizationChildrenList = scopeInstallationSites.FindAll(x => x.ParentId.ToString() == orgId && x.Type == ScopeType.Organization);
                    List<Guid> organizations = new List<Guid>();
                    if (organizationChildrenList.Any())
                    {
                        foreach (var item in organizationChildrenList)
                        {
                            organizations.Add(item.Id);
                        }
                    }
                    scopeInstallationSites.AddRange(ReturnScopesByOrgList(organizations));
                }
                //传入站点时，查找该站点下的安装位置
                else if (input.Type == ScopeType.Station)
                {
                    scopeInstallationSites = ReturnScopesOrganizations(orgsList);
                    if (orgList.Count > 0)
                    {
                        scopeInstallationSites.AddRange(ReturnScopesByOrgList(orgList));
                    }
                    var organizationChildrenList = scopeInstallationSites.FindAll(x => x.ParentId.ToString() == orgId && x.Type == ScopeType.Organization);
                    var stationList = scopeInstallationSites.FindAll(x => x.ParentId.ToString() == railwayId && x.Type == ScopeType.Station);

                    List<Guid> organizations = new List<Guid>();
                    List<Guid> stations = new List<Guid>();

                    if (organizationChildrenList.Any())
                    {
                        foreach (var item in organizationChildrenList)
                        {
                            organizations.Add(item.Id);
                        }
                    }

                    if (stationList.Any())
                    {
                        foreach (var item in stationList)
                        {
                            stations.Add(item.Id);
                        }
                    }

                    var installationSites = installationSitesRepository
                        .WithDetails()
                        .Where(x => stations.Contains((Guid)x.StationId) && railwayId == x.RailwayId.ToString() && orgId == x.OrganizationId.ToString() && x.ParentId == null);

                    if (installationSites.Any())
                    {
                        foreach (var item in installationSites)
                        {
                            GetListByScopeOutDto sisd = new GetListByScopeOutDto
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Type = ScopeType.InstallationSite,
                                ParentId = item.StationId,
                                ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                             GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                             GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId + "." +
                                             GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id
                            };
                            scopeInstallationSites.Add(sisd);
                        }
                    }

                    scopeInstallationSites.AddRange(ReturnScopesByOrgList(organizations));
                    scopeInstallationSites.AddRange(ReturnListWithInstallationSites(orgId, railwayId, stationId));
                }
                //传入安装位置时，查找该安装的站点，线路，组织机构
                else if (input.Type == ScopeType.InstallationSite)
                {
                    scopeInstallationSites = ReturnScopesOrganizations(orgsList);
                    if (orgList.Count > 0)
                    {
                        scopeInstallationSites.AddRange(ReturnScopesByOrgList(orgList));
                    }
                    //查询安装位置
                    var installationSiteInfos = installationSitesRepository
                        .WithDetails()
                        .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.InstallationSite,
                            x => x.Id == input.Id);

                    if (railwayId == null)
                    {
                        var organizationChildrenList = scopeInstallationSites.FindAll(x => x.ParentId.ToString() == orgId && x.Type == ScopeType.Organization);
                        List<Guid> organizations = new List<Guid>();
                        if (organizationChildrenList.Any())
                        {
                            foreach (var item in organizationChildrenList)
                            {
                                organizations.Add(item.Id);
                            }
                            scopeInstallationSites.AddRange(ReturnScopesByOrgList(organizations));
                        }
                    }


                    var list = GetParentIsNullByInstallationSite(installationSiteInfos);

                    var installationSiteInfo = list.Find(x => x.ParentId == null);

                    if (list.Any())
                    {
                        foreach (var item in list)
                        {
                            if (item.Id == installationSiteInfo.Id || item.Id == input.Id)
                            {

                            }
                            else
                            {
                                GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                                sisd.Id = item.Id;
                                sisd.Name = item.Name;
                                sisd.Type = ScopeType.InstallationSite;
                                if (item.ParentId != null)
                                {
                                    sisd.ParentId = item.ParentId;
                                    sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                                     GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                                     GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId + "." +
                                                     GetType(ScopeType.InstallationSite) + "@" + item.Parent.Name + "@" + item.ParentId + "." +
                                                     GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                                }
                                else if (item.RailwayId == null)
                                {
                                    sisd.ParentId = item.OrganizationId;
                                    sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                                     GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                                }
                                else
                                {
                                    sisd.ParentId = item.StationId;
                                    sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                                     GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                                     GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId + "." +
                                                     GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                                }

                                scopeInstallationSites.Add(sisd);
                            }

                        }
                    }
                    if (railwayId != null)
                    {
                        scopeInstallationSites.AddRange(ReturnListWithInstallationSites(orgId, railwayId, stationId));
                    }

                    scopeInstallationSites = scopeInstallationSites.Distinct().ToList();
                }
            }
            else
            {
                throw new UserFriendlyException("传入的数据有误");
            }
            var scopeInstallationSiteList = new List<GetListByScopeOutDto>();

            foreach (var item in scopeInstallationSites)
            {
                if (scopeInstallationSiteList.Find(x => x.ScopeCode == item.ScopeCode) == null)
                {
                    scopeInstallationSiteList.Add(item);
                }
            }
            var notParentId = scopeInstallationSiteList.Find(x => x.ParentId == null);
            scopeInstallationSites = GuidKeyTreeHelper<GetListByScopeOutDto>.GetTree(scopeInstallationSiteList);
            //scopeInstallationSites = GuidKeyTreeHelper<GetListByScopeOutDto>.GetTree(scopeInstallationSites);
            return scopeInstallationSites.OrderBy(x => x.Type).ToList();

        }
        #endregion


        #region 获取数据对象=======================================
        //[Authorize(BasicPermissions.InstallationSite.Detail)]

        public async Task<InstallationSiteDto> Get(Guid id)
        {
            InstallationSiteDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = installationSitesRepository.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("当前位置机房实体不存在");
                result = ObjectMapper.Map<InstallationSite, InstallationSiteDto>(ent);
                //if (orgRepository.FirstOrDefault(x => x.Id == ent.OrganizationId) == null) result.OrganizationId = null;

                if (railwayRepository.FirstOrDefault(x => x.Id == ent.RailwayId) == null) result.RailwayId = null;

                if (stationRepository.FirstOrDefault(x => x.Id == ent.StationId) == null) result.StationId = null;

                if (dataDicRepository.FirstOrDefault(x => x.Id == ent.TypeId) == null) result.TypeId = null;

            });
            return result;
        }
        #endregion

        #region 获得所有集合(为前端下拉框提供数据源)===============
        public Task<PagedResultDto<InstallationSiteDto>> GetTreeList(InstallationSiteSearchDto input)
        {
            //获取当前登录用户的组织机构
            //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            //var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            //var organizationCode = organization != null ? organization.Code : null;
            var result = new PagedResultDto<InstallationSiteDto>();
            var query = installationSitesRepository.WithDetails()
                .Where(s => s.CSRGCode.StartsWith("SA") || s.CSRGCode == null)
                .WhereIf(string.IsNullOrEmpty(input.Keyword) &&
                        (input.RailwayId == null || input.RailwayId == Guid.Empty) &&
                        (input.TypeId == null || input.TypeId == Guid.Empty), x => x.ParentId == input.ParentId)
                .WhereIf(input.RailwayId != null && input.RailwayId != Guid.Empty, x => x.RailwayId == input.RailwayId)
                .WhereIf(input.StationId != null && input.StationId != Guid.Empty, x => x.StationId == input.StationId)
                .WhereIf(input.TypeId != null && input.TypeId != Guid.Empty, x => x.TypeId == input.TypeId)
                .WhereIf(input.LocationType.IsIn(InstallationSiteLocationType.RailwayOuter, InstallationSiteLocationType.SectionInner, InstallationSiteLocationType.StationInner, InstallationSiteLocationType.Other), x => x.LocationType == input.LocationType)
                .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), s =>
                     s.Name.Contains(input.Keyword) ||
                     s.Station.Name.Contains(input.Keyword) ||
                     s.Code.Contains(input.Keyword) ||
                     s.CSRGCode.Contains(input.Keyword) ||
                     s.Location.Contains(input.Keyword)
             );

            using (DataFilter.Disable<ISoftDelete>())
            {
                //query = query.WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode),
                //    x => (x.Organization.Code.StartsWith(organizationCode)
                //          || x.OrganizationId == null) && !x.IsDeleted);

                var list = input.IsAll ? query.ToList() : query.Where(x => !x.IsDeleted).Skip(input.SkipCount).Take(input.MaxResultCount).OrderBy(x => x.Railway).ThenBy(x => x.KMMark).ToList();
                var dtos = ObjectMapper.Map<List<InstallationSite>, List<InstallationSiteDto>>(list);
                foreach (var item in dtos)
                {
                    //if (item.Organization != null && item.Organization.IsDeleted)
                    //{
                    //    item.Organization = null;
                    //    item.OrganizationId = null;
                    //}
                    item.Children = item.Children.Count == 0 || (!string.IsNullOrEmpty(input.Keyword) &&
                                   (input.RailwayId != null || input.RailwayId != Guid.Empty) &&
                                   (input.TypeId != null || input.TypeId != Guid.Empty)) &&
                                   (input.CategoryId != null || input.CategoryId != Guid.Empty)
                                   ? null : new List<InstallationSiteDto>();
                }

                result.TotalCount = input.IsAll ? dtos.Count() : query.Count();
                result.Items = dtos;
                return Task.FromResult(result);
            }
        }
        #endregion


        #region 级联选择接口=======================================

        /// <summary>
        /// 获取机房级联选择框初始化数据  线-站-机房
        /// </summary>
        /// <returns></returns>
        public async Task<List<InstallationSiteCascaderDto>> GetCascaderList()
        {
            List<InstallationSiteCascaderDto> res = new List<InstallationSiteCascaderDto>();
            await Task.Run(() =>
            {
                InstallationSiteCascaderDto other = new InstallationSiteCascaderDto();
                other.Id = Guid.Empty;
                other.Name = "其它";
                other.Type = 4;
                var allSites = installationSitesRepository.Where(s => s.Id != null);
                var allRailwayEnt = railwayRepository.Where(s => allSites.Select(s => s.RailwayId).Contains(s.Id)).ToList();
                var allRailway = ObjectMapper.Map<List<Railway>, List<RailwayDetailDto>>(allRailwayEnt);
                var allStationEnt = stationRepository.Where(s => allSites.Select(s => s.StationId).Contains(s.Id)).ToList();
                var allStation = ObjectMapper.Map<List<Station>, List<StationDto>>(allStationEnt);
                foreach (var item in allRailway)
                {
                    InstallationSiteCascaderDto railway = new InstallationSiteCascaderDto();
                    railway.Id = item.Id;
                    railway.Name = item.Name;
                    railway.Type = 0;
                    var stationIds = allSites.Where(s => s.RailwayId == item.Id).Select(s => s.StationId).Distinct();
                    foreach (var station in allStation.Where(s => stationIds.Contains(s.Id)))
                    {
                        InstallationSiteCascaderDto sta = new InstallationSiteCascaderDto();
                        sta.Id = station.Id;
                        sta.Name = station.Name;
                        sta.Type = 1;
                        railway.Children.Add(sta);
                    }
                    railway.Children = railway.Children.OrderBy(s => s.Name).ToList();
                    res.Add(railway);
                }
                res = res.OrderBy(r => r.Name).ToList();
                foreach (var item in allSites)
                {
                    InstallationSiteCascaderDto site = new InstallationSiteCascaderDto();
                    site.Id = item.Id;
                    site.Name = item.Name;
                    site.Type = 3;
                    //属于其它类型
                    if (item.RailwayId == null || item.StationId == null)
                    {
                        other.Children.Add(site);
                    }
                    else
                    {
                        var rail = res.FirstOrDefault(r => r.Id == item.RailwayId);
                        if (rail != null)
                        {
                            var station = rail.Children.FirstOrDefault(s => s.Id == item.StationId);
                            if (station != null) station.Children.Add(site);
                        }
                    }
                }
                //按名称排序
                foreach (var rail in res)
                {
                    foreach (var sta in rail.Children)
                    {
                        sta.Children = sta.Children.OrderBy(s => s.Name).ToList();
                    }
                }
                other.Children = other.Children.OrderBy(s => s.Name).ToList();
                res.Add(other);
            });
            return res;
        }

        /// <summary>
        /// 获取机房级联选择框根级数据  线-站-机房
        /// </summary>
        /// <returns></returns>
        public async Task<List<InstallationSiteCascaderDto>> GetRootCascaderList()
        {
            List<InstallationSiteCascaderDto> res = new List<InstallationSiteCascaderDto>();
            await Task.Run(() =>
            {
                var allRailwayEnt = railwayRepository.Where(s => s.Id != null).ToList();
                var allRailway = ObjectMapper.Map<List<Railway>, List<RailwayDetailDto>>(allRailwayEnt).OrderBy(s => s.Name);
                foreach (var item in allRailway)
                {
                    InstallationSiteCascaderDto railway = new InstallationSiteCascaderDto();
                    railway.Id = item.Id;
                    railway.Name = item.Name;
                    railway.Type = 0;
                    res.Add(railway);
                }
                InstallationSiteCascaderDto other = new InstallationSiteCascaderDto();
                other.Id = Guid.Empty;
                other.Name = "其它";
                other.Type = 4;
                res.Add(other);
            });
            return res;
        }

        /// <summary>
        /// 获取机房级联选择框指定结点的下级数据  线-站-机房
        /// </summary>
        /// <param name="id">结点id</param>
        /// <param name="type">结点类型（线0 站1 机房3 其他4）</param>
        /// <returns></returns>
        public async Task<List<InstallationSiteCascaderDto>> GetCascaderListByParent(Guid id, int type)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("id有误");
            List<InstallationSiteCascaderDto> res = new List<InstallationSiteCascaderDto>();
            await Task.Run(() =>
            {
                switch (type)
                {
                    case 0:
                        var allStaIds = sta_railwayRepository.Where(s => s.RailwayId == id).Select(s => s.StationId);
                        var allStation = stationRepository.Where(s => allStaIds.Contains(s.Id));
                        foreach (var station in allStation)
                        {
                            InstallationSiteCascaderDto sta = new InstallationSiteCascaderDto();
                            sta.Id = station.Id;
                            sta.Name = station.Name;
                            sta.Type = 1;
                            res.Add(sta);
                        }
                        var allSiteByRailway = installationSitesRepository.Where(s => s.RailwayId == id && (s.StationId == null || s.StationId == Guid.Empty));
                        foreach (var site in allSiteByRailway)
                        {
                            InstallationSiteCascaderDto sta = new InstallationSiteCascaderDto();
                            sta.Id = site.Id;
                            sta.Name = site.Name;
                            sta.Type = 3;
                            res.Add(sta);
                        }
                        break;
                    case 1:
                        var allSiteBySta = installationSitesRepository.Where(s => s.StationId == id);
                        foreach (var site in allSiteBySta)
                        {
                            InstallationSiteCascaderDto sta = new InstallationSiteCascaderDto();
                            sta.Id = site.Id;
                            sta.Name = site.Name;
                            sta.Type = 3;
                            res.Add(sta);
                        }
                        break;
                    case 4:
                        //机房若不属于任何线、站，则机房一定不属于任何线
                        var allSiteByOther = installationSitesRepository.Where(s => s.RailwayId == null || s.RailwayId == Guid.Empty);
                        foreach (var site in allSiteByOther)
                        {
                            InstallationSiteCascaderDto sta = new InstallationSiteCascaderDto();
                            sta.Id = site.Id;
                            sta.Name = site.Name;
                            sta.Type = 3;
                            res.Add(sta);
                        }
                        break;
                }
            });
            return res;
        }

        /// <summary>
        /// 获取机房级联选择框初始化数据  组织机构-机房
        /// </summary>
        /// <param name="orgId">组织机构id</param>
        /// <returns></returns>
        public async Task<List<InstallationSiteCascaderDto>> GetCascaderListWithOrg(Guid? orgId)
        {
            //if (orgId == null || orgId == Guid.Empty) throw new UserFriendlyException("组织机构有误");
            List<InstallationSiteCascaderDto> res = new List<InstallationSiteCascaderDto>();
            await Task.Run(() =>
            {
                //IQueryable<Organization> allOrgs;
                //if (orgId == null && orgId == Guid.Empty)
                //{
                //    var org = orgRepository.FirstOrDefault(s => s.Id == orgId);
                //    if (org == null) throw new UserFriendlyException("组织机构不存在");
                //    var code = org.Code;
                //    allOrgs = orgRepository.Where(s => s.Code.StartsWith(code));
                //}
                //else
                //{
                //    allOrgs = orgRepository.Where(s => s.Id != null);
                //}
                //var allOrgIds = allOrgs.Select(s => s.Id).ToList();
                var allSites = installationSitesRepository.Where(s => s.OrganizationId != null);
                //foreach (var item in allOrgs.OrderBy(s => s.Code))
                //{
                //    InstallationSiteCascaderDto orgNode = new InstallationSiteCascaderDto();
                //    orgNode.Id = item.Id;
                //    orgNode.Name = item.Name;
                //    orgNode.Type = 2;
                //    foreach (var site in allSites.Where(s => s.OrganizationId == item.Id))
                //    {
                //        InstallationSiteCascaderDto siteNode = new InstallationSiteCascaderDto();
                //        siteNode.Id = site.Id;
                //        siteNode.Name = site.Name;
                //        siteNode.Type = 3;
                //        orgNode.Children.Add(siteNode);
                //    }
                //    orgNode.Children = orgNode.Children.OrderBy(s => s.Name).ToList();
                //    res.Add(orgNode);
                //}
                InstallationSiteCascaderDto orgNode = new InstallationSiteCascaderDto();
                orgNode.Type = 2;
                foreach (var site in allSites)
                {
                    InstallationSiteCascaderDto siteNode = new InstallationSiteCascaderDto();
                    siteNode.Id = site.Id;
                    siteNode.Name = site.Name;
                    siteNode.Type = 3;
                    orgNode.Children.Add(siteNode);
                }
                orgNode.Children = orgNode.Children.OrderBy(s => s.Name).ToList();
                res.Add(orgNode);
            });
            return res;
        }
        #endregion

        #region 添加操作===========================================
        [Authorize(BasicPermissions.InstallationSite.Create)]
        public async Task<bool> Create(InstallationSiteCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("机房名称不能为空");

            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("使用单位不能为空");

            if (input.State == 0) throw new UserFriendlyException("机房状态不能为空");

            //if (installationSitesRepository.FirstOrDefault(s => s.Name == input.Name && !string.IsNullOrEmpty(input.Name)) != null)
            //    throw new UserFriendlyException("该机房名称已存在");

            if (installationSitesRepository.FirstOrDefault(s => s.Code == input.Code && !string.IsNullOrEmpty(input.Code)) != null)
                throw new UserFriendlyException("该机房编码已存在");

            var ent = new InstallationSite(_guidGenerator.Create())
            {
                Name = input.Name,
                Code = input.Code,
                OrganizationId = input.OrganizationId,
                TypeId = input.TypeId,
                LocationType = input.LocationType,
                RailwayDirection = input.RailwayDirection,
                ParentId = input.ParentId,
                RailwayId = input.RailwayId,
                StationId = input.StationId,
                State = input.State,
                Latitude = input.Latitude,
                Longitude = input.Longitude,
                Location = input.Location,
                UseDate = input.UseDate,
                UseType = input.UseType,
                KMMark = input.KMMark,
                CategoryId = input.CategoryId
            };

            await installationSitesRepository.InsertAsync(ent);
            return true;
        }
        #endregion

        #region 更新操作===========================================
        [Authorize(BasicPermissions.InstallationSite.Update)]
        public async Task<bool> Update(InstallationSiteUpdateDto input)
        {
            var oldEntity = installationSitesRepository.FirstOrDefault(x => x.Id == input.Id);

            if (oldEntity == null) throw new UserFriendlyException("更新机房位置实体不存在");

            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("机房名称不能为空");

            if (input.OrganizationId == null || input.OrganizationId == Guid.Empty) throw new UserFriendlyException("使用单位不能为空");

            if (input.State == 0) throw new UserFriendlyException("机房状态不能为空");

            //if (installationSitesRepository.FirstOrDefault(s => s.Name == input.Name && !string.IsNullOrEmpty(input.Name) && s.Id != input.Id) != null)
            //    throw new UserFriendlyException("该机房名称已存在");

            if (installationSitesRepository.FirstOrDefault(s => s.Code == input.Code && !string.IsNullOrEmpty(input.Code) && s.Id != input.Id) != null)
                throw new UserFriendlyException("该机房编码已存在");

            if (input.Id == input.ParentId) throw new UserFriendlyException("父级不能为自身");

            oldEntity.Name = input.Name;
            oldEntity.Code = input.Code;
            oldEntity.OrganizationId = input.OrganizationId;
            oldEntity.TypeId = input.TypeId;
            oldEntity.LocationType = input.LocationType;
            oldEntity.RailwayDirection = input.RailwayDirection;
            oldEntity.ParentId = input.ParentId;
            oldEntity.RailwayId = input.RailwayId;
            oldEntity.StationId = input.StationId;
            oldEntity.State = input.State;
            oldEntity.Latitude = input.Latitude;
            oldEntity.Longitude = input.Longitude;
            oldEntity.Location = input.Location;
            oldEntity.UseDate = input.UseDate;
            oldEntity.UseType = input.UseType;
            oldEntity.KMMark = input.KMMark;
            oldEntity.CategoryId = input.CategoryId;
            await installationSitesRepository.UpdateAsync(oldEntity);
            return true;
        }
        #endregion

        #region 删除操作===========================================
        [Authorize(BasicPermissions.InstallationSite.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || Guid.Empty == id) throw new UserFriendlyException("id不正确");
            var ent = installationSitesRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("该机房不存在");
            if (ent.Children != null && ent.Children.Count > 0) throw new UserFriendlyException("当前机房位置存在下级，请先删除下级！！");
            await installationSitesRepository.DeleteAsync(id);
            return true;
        }
        #endregion

        #region 上传志入EXCEL文件==================================
        /// <summary>
        /// 上传导入文件
        /// </summary>
        [Authorize(BasicPermissions.InstallationSite.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            //获取当前登录用户的组织机构
            var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            var organizationCode = organization != null ? organization.Code : null;
            organizationCode = organizationCode.Split('.').First();
            await _fileImport.Start(input.ImportKey, 100);
            var failMsg = new StringBuilder();
            var pn = (int)0;//序号
            var failCount = (int)0;//失败数
            var addCount = (int)0;//新增数
            var upCount = (int)0;//更新数
            var delCount = (int)0;//删除数
            var celVal = "";
            DataTable dt;
            IWorkbook workbook = null;
            ISheet sheet = null;
            var model = (InstallationSite)null;
            var list = new List<InstallationSite>();

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

            #region 验证列是否存在
            //验证列是否存在
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.SeenSun.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.SeenSun.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.CSRGCode.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.CSRGCode.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Name.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Name.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Organization.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Organization.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.UseType.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.UseType.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Type.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Type.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.LocationType.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.LocationType.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Railway.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Railway.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Location.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Location.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Longitude.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Longitude.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.Latitude.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.Latitude.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.State.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.State.ToString() + "不存在");
            }
            if (!dt.Columns.Contains(SnAbp.Basic.Enums.ImportCol.UseDate.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("列" + SnAbp.Basic.Enums.ImportCol.UseDate.ToString() + "不存在");
            }
            #endregion

            try
            {


                #region 获得基础数据
                var installationSiteTypes = dataDicRepository.Where(x => x.Key.StartsWith("InstallationSiteType")).ToList();//从字典中加载机房类型
                var installationSiteCategories = dataDicRepository.Where(x => x.Key.StartsWith("InstallationSiteCategory")).ToList();//从字典中加载机房分类
                var organizationList = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(orgRepository.Where(z => z.Code.StartsWith(organizationCode)).ToList());

                //获得数据库的所有的站线关系数据
                var allRsList = (from a in railwayRepository
                                 join b in sta_railwayRepository
                                 on a.Id equals b.RailwayId
                                 join c in stationRepository
                                 on b.StationId equals c.Id
                                 select new RailwayStationDto
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     StationId = c.Id,
                                     StationName = c.Name,
                                     KMMark = b.KMMark,
                                     SectionStartStationId = c.SectionStartStationId,
                                     SectionEndStationId = c.SectionEndStationId,
                                     Type = c.Type
                                 }).ToList();

                var railStation = from a in allRsList
                                  group a by new { a.Id, a.Name } into t1
                                  select new
                                  {
                                      t1.Key.Id,
                                      t1.Key.Name,
                                      Stations = t1.Select(z => new
                                      {
                                          z.StationId,
                                          z.StationName,
                                          z.KMMark,
                                          z.SectionStartStationId,
                                          z.SectionEndStationId,
                                          z.Type
                                      })
                                  };
                List<WrongInfo> wrongInfos = new List<WrongInfo>();
                #endregion

                int dataRowIndex = 5;
                int rowIndex = -1;
                #region 验证并获取数据
                var order = 1;
                foreach (DataRow row in dt.Rows)
                {
                    rowIndex++;
                    model = new InstallationSite(Guid.NewGuid());
                    model.CSRGCode = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.CSRGCode.ToString()]);
                    if (string.IsNullOrWhiteSpace(model.CSRGCode))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "CSRGCode为空"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "CSRGCode为空");
                        wrongInfos.Add(wrong);
                        continue;
                    }

                    model.Name = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Name.ToString()]);
                    if (string.IsNullOrWhiteSpace(model.Name))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "机房名称为空"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "机房名称为空");
                        wrongInfos.Add(wrong);
                        continue;
                    }

                    //使用单位
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Organization.ToString()]);
                    if (string.IsNullOrWhiteSpace(celVal))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "使用单位为空"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "使用单位为空，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }
                    var arr = celVal.Split('-');
                    if (arr.Length == 0)
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "使用单位不正确"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "使用单位不正确，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }
                    var orgModel = organizationList.FirstOrDefault(z => z.Name == arr[arr.Length - 1]);
                    if (orgModel == null)
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "使用单位不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "使用单位不存在，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }
                    model.OrganizationId = orgModel?.Id;

                    //使用类型
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.UseType.ToString()]);
                    if (!SetUseType(model, celVal))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "使用类型不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "使用类型不存在，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }

                    //机房类型
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Type.ToString()]);
                    var dicModel = installationSiteTypes.FirstOrDefault(z => z.Name == celVal);
                    if (dicModel == null)
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "机房类型不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "机房类型不存在，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }
                    model.TypeId = dicModel?.Id;

                    //机房分类
                    celVal = Convert.ToString(row[ImportCol.Category.ToString()]);
                    var cateModel = dataDicRepository.Where(x => x.Name == celVal).FirstOrDefault();
                    if (cateModel == null)
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "机房类型不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "机房分类不存在，导入并更新机房分类");
                        wrongInfos.Add(wrong);

                        if (!string.IsNullOrEmpty(celVal))
                        {
                            var parent = installationSiteCategories.FirstOrDefault(z => z.Key == "InstallationSiteCategory");
                            using var unitWork1 = _unitOfWorkManager.Begin(true, false);
                            var installationSiteCategorie = new DataDictionary(_guidGenerator.Create())
                            {
                                ParentId = parent.Id,
                                Name = celVal,
                                Key = parent.Key + "." + order++,
                                IsStatic = true
                            };
                            await dataDicRepository.InsertAsync(installationSiteCategorie);
                            await unitWork1.SaveChangesAsync();
                            model.CategoryId = installationSiteCategorie.Id;
                        }
                    }
                    else
                    {
                        model.CategoryId = cateModel.Id;
                    }


                    //当前状态
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.State.ToString()]);
                    if (!SetState(model, celVal))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "当前状态不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "当前状态不存在，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }

                    //经纬度
                    //var decVal = (float)0;
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Longitude.ToString()]);
                    if (!string.IsNullOrWhiteSpace(celVal))
                    {
                        model.Longitude = celVal;
                    }
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Latitude.ToString()]);
                    if (!string.IsNullOrWhiteSpace(celVal))
                    {
                        model.Latitude = celVal;
                    }

                    //位置分类
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.LocationType.ToString()]);
                    if (!SetLocationType(model, celVal))
                    {
                        failCount++;
                        pn++;
                        failMsg.AppendLine(FormartErrMsg(pn, model.CSRGCode, "位置分类不存在"));
                        WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "位置分类不存在，但已导入");
                        wrongInfos.Add(wrong);
                        //continue;
                    }

                    model.Location = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Location.ToString()]);//机房位置

                    //投产时间
                    DateTime dTime;
                    celVal = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.UseDate.ToString()]);
                    if (DateTime.TryParse(celVal, out dTime))
                    {
                        model.UseDate = dTime;
                    }

                    //获得所属站线
                    if (model.LocationType != Enums.InstallationSiteLocationType.RailwayOuter)
                    {
                        var railDesc = Convert.ToString(row[SnAbp.Basic.Enums.ImportCol.Railway.ToString()]);//线路
                        if (string.IsNullOrWhiteSpace(railDesc))
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "线路为空"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "线路为空，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }

                        //获得线路类别
                        if (railDesc.Contains("上行"))
                        {
                            model.RailwayDirection = RailwayDirection.Up;
                        }
                        else if (railDesc.Contains("下行"))
                        {
                            model.RailwayDirection = RailwayDirection.Down;
                        }

                        railDesc = railDesc.Replace("铁路", "").Replace("上行", "^").Replace("下行", "^");
                        //获得线路名称
                        arr = railDesc.Split('^');
                        if (arr.Length != 2)
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "线路名称解析失败"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "线路名称解析失败，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }

                        var raillStr = arr[0].EndsWith("线") ? arr[0] : arr[0] + "线";//线名称
                        var lc = (int)0;//里程
                        if (arr.Length < 2 || string.IsNullOrWhiteSpace(arr[1]))
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "线路未找到里程信息"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "线路未找到里程信息，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }
                        if (arr.Length < 2 || !int.TryParse(arr[1].Replace("K", "").Replace("+", ""), out lc))
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "线路里程非数字"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "线路里程非数字，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }
                        var KMMarkArr = new List<string>();

                        if (arr.Length >= 2 && arr[1].Contains("K") && arr[1].Contains("+"))
                        {
                            KMMarkArr.AddRange(arr[1].Replace("K", "").Replace("+", "^").Split('^'));
                            int mark1, mark2;
                            if (int.TryParse(KMMarkArr[0], out mark1) && int.TryParse(KMMarkArr[1], out mark2))
                            {
                                model.KMMark = mark1 * 1000 + mark2;
                            }
                            //model.KMMark = int.Parse(KMMarkArr[0]) * 1000 + int.Parse(KMMarkArr[1]);
                        }
                        else if (arr.Length >= 2)
                        {
                            if (arr[1].Contains("K"))
                            {
                                KMMarkArr.Add(arr[1].Replace("K", ""));
                                int mark = 0;
                                if (int.TryParse(KMMarkArr[0], out mark))
                                {
                                    model.KMMark = mark * 1000;
                                }
                            }
                            else
                            {
                                KMMarkArr.Add(arr[1]);
                                int mark = 0;
                                if (int.TryParse(KMMarkArr[0], out mark))
                                {
                                    model.KMMark = mark;
                                }
                                //model.KMMark = int.Parse(KMMarkArr[0]);
                            }
                        }



                        var railway = railStation.FirstOrDefault(z => z.Name.Contains(raillStr));
                        if (railway == null)
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "线路不存在"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "线路不存在，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }

                        //取得线
                        model.RailwayId = railway?.Id;

                        if (railway != null && railway.Stations == null)
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "未找到站信息"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "未找到站信息，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }
                        if (railway != null && railway.Stations.Count() == 0)
                        {
                            pn++;
                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "未找到站信息"));
                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "未找到站信息，但已导入");
                            wrongInfos.Add(wrong);
                            //list.Add(model);
                            //continue;
                        }


                        //找到所有单线站
                        if (railway != null && railway.Stations != null)
                        {
                            var sigStation = railway.Stations.Where(z => z.Type == (int)Enums.RailwayType.MONGLINE).OrderBy(z => z.KMMark).ToList();
                            if (sigStation.Count() == 0)
                            {
                                pn++;
                                failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "在站里没有找到单线列表"));
                                WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "在站里没有找到单线列表，但已导入");
                                wrongInfos.Add(wrong);
                                //list.Add(model);
                                //continue;
                            }


                            if (model.LocationType == Enums.InstallationSiteLocationType.SectionInner)
                            {
                                //找出比机房公里标大的单线车站
                                var stations = sigStation.Where(x => x.KMMark >= model.KMMark).ToList();

                                //找出距离机房最近的车站
                                if (stations != null)
                                {
                                    var station = stations.OrderBy(x => Math.Abs(x.KMMark - model.KMMark)).FirstOrDefault();
                                    if (station != null)
                                    {
                                        var passOrder = _stationRltRailways.FirstOrDefault(x => x.StationId == station.StationId && x.RailwayId == model.RailwayId).PassOrder;
                                        var sectionStation = _stationRltRailways.FirstOrDefault(x => x.PassOrder == passOrder - 1 && x.RailwayId == model.RailwayId);
                                        if (sectionStation == null)
                                        {
                                            pn++;
                                            failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "未找到站信息"));
                                            WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "未找到站信息，但已导入");
                                            wrongInfos.Add(wrong);
                                            //list.Add(model);
                                            //continue;
                                        }
                                        model.StationId = sectionStation?.StationId;
                                    }
                                }
                                //按里程值查找起止站
                                //var minId = Guid.Empty;//区间起主键
                                //var maxId = Guid.Empty;//区间止主键 

                                //foreach (var ele in sigStation)
                                //{
                                //    if (ele.KMMark > lc)
                                //    {
                                //        maxId = ele.StationId;
                                //        break;
                                //    }
                                //    minId = ele.StationId;
                                //}
                                //var ownStation = railway.Stations.FirstOrDefault(z => z.SectionStartStationId == minId && z.SectionEndStationId == maxId);
                                //if (ownStation == null)
                                //{
                                //    pn++;
                                //    failMsg.AppendLine(FormartInfoMsg(pn, model.CSRGCode, "未找到站信息"));
                                //    WrongInfo wrong = new WrongInfo(dataRowIndex + rowIndex, "未找到站信息");
                                //    wrongInfos.Add(wrong);
                                //    list.Add(model);
                                //    continue;
                                //}
                                //model.StationId = ownStation.StationId;
                            }
                            else if (model.LocationType == Enums.InstallationSiteLocationType.StationInner)
                            {
                                //按里程值查找起止站
                                var station = sigStation.OrderBy(x => Math.Abs(x.KMMark - model.KMMark)).FirstOrDefault();
                                if (station != null)
                                {
                                    model.StationId = station.StationId;
                                }
                                //var indexVal = Math.Abs(sigStation.FirstOrDefault().KMMark - lc);//里程差值(哪个最小就是哪个)
                                //foreach (var ele in sigStation)
                                //{
                                //    var jj = Math.Abs(ele.KMMark - lc);
                                //    if (indexVal >= jj)
                                //    {
                                //        indexVal = jj;
                                //        model.StationId = ele.StationId;
                                //    }
                                //}
                            }
                        }
                    }
                    list.Add(model);
                }
                #endregion

                #region 执行新增或更新
                //执行新增或更新
                await _fileImport.ChangeTotalCount(input.ImportKey, list.Count);

                using (DataFilter.Disable<ISoftDelete>())
                {
                    foreach (var item in list)
                    {
                        model = installationSitesRepository.FirstOrDefault(z => z.CSRGCode == item.CSRGCode);
                        using var unitWork = _unitOfWorkManager.Begin(true, false);
                        if (model == null)
                        {
                            addCount++;
                            await installationSitesRepository.InsertAsync(item);
                            await unitWork.SaveChangesAsync();
                        }
                        else
                        {
                            upCount++;
                            model.Name = item.Name;
                            model.OrganizationId = item.OrganizationId;
                            model.UseType = item.UseType;
                            model.TypeId = item.TypeId;
                            model.CategoryId = item.CategoryId;
                            model.LocationType = item.LocationType;
                            model.RailwayDirection = item.RailwayDirection;
                            model.Location = item.Location;
                            model.Longitude = item.Longitude;
                            model.Latitude = item.Latitude;
                            model.State = item.State;
                            model.UseDate = item.UseDate;
                            model.RailwayId = item.RailwayId;
                            model.StationId = item.StationId;
                            model.KMMark = item.KMMark;
                            model.IsDeleted = false;
                            await installationSitesRepository.UpdateAsync(model);
                            await unitWork.SaveChangesAsync();
                        }
                        await _fileImport.UpdateState(input.ImportKey, list.IndexOf(item));
                    }
                }

                #endregion

                #region 执行软删除
                var codeList = list.Select(z => z.CSRGCode).ToList();
                var delList = installationSitesRepository.Where(s => !codeList.Contains(s.CSRGCode) && s.IsDeleted == false).ToList();
                foreach (var item in delList)
                {
                    delCount++;
                    item.IsDeleted = true;
                    await installationSitesRepository.UpdateAsync(item);
                }
                #endregion

                await _fileImport.Complete(input.ImportKey);

                failMsg.AppendLine(string.Format($"总数:{dt.Rows.Count};失败数:{failCount};新增数:{addCount};更新数:{upCount};删除数:{delCount}"));
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

        private string FormartErrMsg(int pn, string code, string msg)
        {
            return pn.ToString().PadLeft(4, '0') + ".错误：Code=" + code + msg;
        }

        private string FormartInfoMsg(int pn, string code, string msg)
        {
            return pn.ToString().PadLeft(4, '0') + ".警告：Code=" + code + msg;
        }

        private bool SetUseType(InstallationSite model, string useType)
        {
            if (useType == InstallationSiteUseType.Private.GetDescription())
            {
                model.UseType = InstallationSiteUseType.Private;
            }
            else if (useType == InstallationSiteUseType.Share.GetDescription())
            {
                model.UseType = InstallationSiteUseType.Share;
            }
            else
                return false;
            return true;
        }

        private bool SetState(InstallationSite model, string state)
        {
            if (state == InstallationSiteState.Building.GetDescription())
            {
                model.State = InstallationSiteState.Building;
            }
            else if (state == InstallationSiteState.Using.GetDescription())
            {
                model.State = InstallationSiteState.Using;
            }
            else
                return false;
            return true;
        }

        private bool SetLocationType(InstallationSite model, string locationType)
        {
            if (locationType == InstallationSiteLocationType.SectionInner.GetDescription())
            {
                model.LocationType = InstallationSiteLocationType.SectionInner;
            }
            else if (locationType == InstallationSiteLocationType.StationInner.GetDescription())
            {
                model.LocationType = InstallationSiteLocationType.StationInner;
            }
            else if (locationType == InstallationSiteLocationType.RailwayOuter.GetDescription())
            {
                model.LocationType = InstallationSiteLocationType.RailwayOuter;
            }
            else
                return false;
            return true;
        }

        #endregion

        #region 导出数据到EXCEL文件==================================
        public async Task<Stream> Export(InstallationExportData input)
        {
            List<InstallationSite> list = GetInstallationSiteData(input.paramter);
            var orgList = await orgRepository.GetListAsync();
            var railwayList = await railwayRepository.GetListAsync();
            var dataList = await dataDicRepository.GetListAsync();

            var installationSites = new List<InstallationSiteImportTemplate>();
            var installationSite = new InstallationSiteImportTemplate();
            var orgName = "";
            var railwayName = "";
            var dataName = "";
            foreach (var install in list)
            {
                installationSite = new InstallationSiteImportTemplate();
                installationSite.CSRGCode = install.CSRGCode;
                installationSite.Name = install.Name;
                installationSite.UseType = install.UseType != null ? install.UseType == InstallationSiteUseType.Private ? "独用" : "共用" : null;
                installationSite.LocationType = install.LocationType != null ? install.LocationType == InstallationSiteLocationType.RailwayOuter ? "非沿线" : (install.LocationType == InstallationSiteLocationType.SectionInner ? "沿线区间" : install.LocationType == InstallationSiteLocationType.StationInner ? "沿线站内" : "其他") : null;
                installationSite.Location = install.Location;
                installationSite.Longitude = install.Longitude;
                installationSite.Latitude = install.Latitude;
                installationSite.Latitude = install.Latitude;
                installationSite.State = install.State == InstallationSiteState.Using ? "在用" : "在建";
                installationSite.UseDate = install.UseDate != null ? Convert.ToDateTime(install.UseDate.ToString()).ToString("yyyy-MM-dd") : null;

                if (install.OrganizationId != null && install.OrganizationId != Guid.Empty)
                {
                    recursionGetOrganization(install.OrganizationId, installationSite, orgList); //递归获取使用单位
                }
                if (install.RailwayId != null && install.RailwayId != Guid.Empty)
                {
                    railwayName = railwayList.FirstOrDefault(x => x.Id == install.RailwayId).Name; //拼接线路为***K111+111
                    if (install.RailwayDirection != null)
                    {
                        installationSite.Railway = railwayName + (install.RailwayDirection == RailwayDirection.Up ? "上行" : "下行");
                    }
                    if (install.KMMark != 0)
                    {
                        installationSite.Railway += string.Join("+", (install.KMMark / 1000).ToString().Split("."));
                    }
                }
                if (install.TypeId != null && install.TypeId != Guid.Empty)
                {
                    dataName = dataList.FirstOrDefault(x => x.Id == install.TypeId).Name;
                    installationSite.Type = dataName;
                }

                installationSites.Add(installationSite);
            }
            var dtoList = ObjectMapper.Map<List<InstallationSiteImportTemplate>, List<InstallationSiteImportTemplate>>(installationSites);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }
        #endregion


        /// <summary>
        /// 拼接使用单位Organization
        /// </summary>
        private void recursionGetOrganization(Guid? OrganizationId, InstallationSiteImportTemplate installationSite, List<Organization> orgList)
        {
            var Org = orgList.FirstOrDefault(x => x.Id == OrganizationId);
            installationSite.Organization = Org.Name + "-" + installationSite.Organization;

            if (Org.ParentId != null && Org.ParentId != Guid.Empty)
            {
                recursionGetOrganization(Org.ParentId, installationSite, orgList);
            }
            else if (installationSite.Organization.TrimEnd('-').Split("-").Length == 1)
            {
                installationSite.Organization = installationSite.Organization.Replace("铁路", "");
            }
            else
            {
                var arr = installationSite.Organization.TrimEnd('-').Split("-");
                arr[0] = arr[0].Replace("铁路", "");
                installationSite.Organization = string.Join("-", arr);
                installationSite.Organization = "总公司-" + installationSite.Organization.TrimEnd('-');
            }
        }

        /// <summary>
        /// 查找组织机构的子级、组织机构下的线路、组织机构下非沿线的安装位置
        /// </summary>
        /// <param name="input">传入的数据</param>
        /// <returns></returns>
        private List<GetListByScopeOutDto> GetListWithOrg(GetListByScopeInputDto input)
        {
            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();
            //查询组织机构的子级
            var organizations = orgRepository
                .WithDetails(x => x.Children, x => x.Parent)
                .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.Organization,
                    x => x.ParentId == input.Id);

            //查询组织机构下的线路
            var railwaies = _railwayRltOrganization
                .WithDetails(x => x.Railway, x => x.Organization)
                .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.Organization,
                    x => x.OrganizationId == input.Id);

            //查询组织机构下的非沿线的安装位置
            var installationSites = installationSitesRepository
                .WithDetails()
                .Where(x => x.OrganizationId == input.Id && x.RailwayId == null && x.StationId == null &&
                            x.Parent == null);

            List<Guid> orgList = new List<Guid>();
            //整合数据
            if (organizations.Any())
            {
                foreach (var item in organizations)
                {

                    GetListByScopeOutDto sisd = new GetListByScopeOutDto
                    {
                        Name = item.Name,
                        Type = ScopeType.Organization,
                        Id = item.Id,
                        ScopeCode = GetType(ScopeType.Organization) + "@" + item.Name + "@" + item.Id,
                    };
                    if (item.ParentId != null)
                    {
                        sisd.ParentId = item.ParentId;
                    }
                    scopeInstallationSites.Add(sisd);
                    orgList.Add(item.Id);
                    if (item.Children.Count > 0)
                    {
                        foreach (var chile in item.Children)
                        {
                            GetListByScopeOutDto sisdChildren = new GetListByScopeOutDto
                            {
                                ParentId = item.Id,
                                Id = chile.Id,
                                Name = chile.Name,
                                Type = ScopeType.Organization,
                                ScopeCode = GetType(ScopeType.Organization) + "@" + chile.Name + "@" + chile.Id
                            };
                            scopeInstallationSites.Add(sisdChildren);
                        }

                    }
                }
            }

            //查询子级组织机构下的线路
            var organizationChildrenRailwaies = _railwayRltOrganization
               .WithDetails(x => x.Railway, x => x.Organization, x => x.Organization.Parent)
               .WhereIf(input.Id != Guid.Empty && input.Type == ScopeType.Organization,
                   x => orgList.Contains((Guid)x.OrganizationId));

            //查询子级组织机构下的非沿线的安装位置
            var organizationChildrenInstallationSites = installationSitesRepository
                .WithDetails(x => x.Organization.Parent)
                .Where(x => orgList.Contains((Guid)x.OrganizationId) && x.RailwayId == null && x.StationId == null &&
                            x.Parent == null);


            if (organizationChildrenRailwaies.Any())
            {
                foreach (var item in organizationChildrenRailwaies)
                {
                    if (item.Railway != null)
                    {
                        GetListByScopeOutDto sisd = new GetListByScopeOutDto
                        {
                            Name = item.Railway.Name,
                            Type = ScopeType.Railway,
                            Id = (Guid)item.RailwayId,
                            ParentId = item.OrganizationId,
                            ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                        GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId,
                        };
                        scopeInstallationSites.Add(sisd);
                    }

                }
            }

            if (organizationChildrenInstallationSites.Any())
            {
                foreach (var item in organizationChildrenInstallationSites)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto
                    {
                        ParentId = item.OrganizationId,
                        Name = item.Name,
                        Type = ScopeType.InstallationSite,
                        Id = item.Id,
                        ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                    GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id,
                    };
                    scopeInstallationSites.Add(sisd);
                }
            }



            List<Guid> list = new List<Guid>();
            if (railwaies.Any())
            {
                foreach (var item in railwaies)
                {
                    if (item.Railway != null)
                    {
                        GetListByScopeOutDto sisd = new GetListByScopeOutDto
                        {
                            Name = item.Railway.Name,
                            Type = ScopeType.Railway,
                            Id = (Guid)item.RailwayId,
                            ParentId = item.OrganizationId,
                            ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                        GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId,
                        };
                        scopeInstallationSites.Add(sisd);
                        list.Add((Guid)item.RailwayId);
                    }
                }
            }

            //查询站点
            var stations = sta_railwayRepository
                .WithDetails()
                .Where(x => list.Contains((Guid)x.RailwayId) && x.StationId != null);

            if (stations.Any())
            {
                foreach (var e in stations)
                {
                    if (e.Station != null)
                    {
                        GetListByScopeOutDto sisdSta = new GetListByScopeOutDto
                        {
                            ParentId = e.RailwayId,
                            Type = ScopeType.Station,
                            Id = e.StationId,
                            Name = e.Station.Name,
                            ScopeCode = GetType(ScopeType.Organization) + "@" + e.Railway.RailwayRltOrganizations.First().Organization.Name + "@" + e.Railway.RailwayRltOrganizations.First().OrganizationId + "." +
                                        GetType(ScopeType.Railway) + "@" + e.Railway.Name + "@" + e.RailwayId + "." +
                                        GetType(ScopeType.Station) + "@" + e.Station.Name + "@" + e.StationId,
                        };
                        scopeInstallationSites.Add(sisdSta);
                    }
                }
            }

            if (installationSites.Any())
            {
                foreach (var item in installationSites)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                    if (item.ParentId != Guid.Empty && item.ParentId != null)
                    {
                        sisd.ParentId = item.ParentId;
                        sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                         //GetType(ScopeType.InstallationSite) + "@" + item.Parent.Name + "@" + item.ParentId + "." +
                                         GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                    }
                    else
                    {
                        sisd.ParentId = item.OrganizationId;
                        sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                         GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                    }
                    if (item.Children != null)
                    {
                        foreach (var itemChild in item.Children)
                        {
                            GetListByScopeOutDto sisdChild = new GetListByScopeOutDto();
                            sisdChild.ParentId = item.Id;
                            sisdChild.Name = itemChild.Name;
                            sisdChild.Type = ScopeType.InstallationSite;
                            sisdChild.Id = itemChild.Id;
                            sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                             //GetType(ScopeType.InstallationSite) + "@" + item.Parent.Name + "@" + item.ParentId + "." +
                                             GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                            scopeInstallationSites.Add(sisd);
                        }


                    }
                    sisd.Name = item.Name;
                    sisd.Type = ScopeType.InstallationSite;
                    sisd.Id = item.Id;
                    scopeInstallationSites.Add(sisd);
                }
            }
            return scopeInstallationSites;
        }

        /// <summary>
        /// 查询站点下的安装位置包括兄弟节点
        /// </summary>
        /// <param name="orgId">组织机构id</param>
        /// <param name="railwayId">线路id</param>
        /// <param name="stationId">站点id</param>
        /// <returns></returns>
        private List<GetListByScopeOutDto> ReturnListWithInstallationSites(string orgId, string railwayId, string stationId)
        {
            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();

            //查询站点下的安装位置包括兄弟节点
            var installationSiteBro = installationSitesRepository
                        .WithDetails()
                        .WhereIf(orgId != null, x => x.OrganizationId.ToString() == orgId && x.RailwayId.ToString() == railwayId && x.StationId.ToString() == stationId && x.ParentId == null);


            if (installationSiteBro.Any())
            {
                foreach (var item in installationSiteBro)
                {

                    if (item.Children != null)
                    {
                        foreach (var itemChild in item.Children)
                        {
                            GetListByScopeOutDto sisdChild = new GetListByScopeOutDto
                            {
                                ParentId = item.StationId,
                                Id = itemChild.Id,
                                Type = ScopeType.InstallationSite,
                                Name = itemChild.Name
                            };
                            scopeInstallationSites.Add(sisdChild);
                        }
                    }
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto
                    {
                        ParentId = item.StationId,
                        Name = item.Name,
                        Type = ScopeType.InstallationSite,
                        Id = item.Id,
                        ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                    GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                    GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId + "." +
                                    GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id,
                    };
                    scopeInstallationSites.Add(sisd);
                }
            }

            return scopeInstallationSites;
        }

        /// <summary>
        /// 组装安装位置的子级
        /// </summary>
        /// <param name="installation">安装位置</param>
        /// <returns></returns>
        private List<InstallationSiteDto> GetParentIsNullByInstallationSite(IQueryable<InstallationSite> installation)
        {
            List<InstallationSiteDto> scopeInstallationSites = new List<InstallationSiteDto>();
            if (installation.Any())
            {
                foreach (var item in installation)
                {
                    InstallationSiteDto sisd = new InstallationSiteDto();
                    sisd.Id = item.Id;
                    sisd.Name = item.Name;
                    sisd.OrganizationId = item.OrganizationId;
                    sisd.RailwayId = item.RailwayId;
                    sisd.StationId = item.StationId;
                    if (item.ParentId != null || item.ParentId != Guid.Empty)
                    {
                        sisd.ParentId = item.ParentId;
                    }
                    var installationSite = item;
                    while (installationSite.ParentId != null)
                    {
                        installationSite = installationSite.Parent;
                        InstallationSiteDto sisdChild = new InstallationSiteDto();
                        sisdChild.Id = installationSite.Id;
                        sisdChild.Name = installationSite.Name;
                        sisdChild.OrganizationId = installationSite.OrganizationId;
                        sisdChild.RailwayId = installationSite.RailwayId;
                        sisdChild.StationId = installationSite.StationId;
                        if (installationSite.ParentId != null)
                        {
                            sisdChild.ParentId = installationSite.ParentId;

                        }
                        scopeInstallationSites.Add(sisdChild);
                    }
                    scopeInstallationSites.Add(sisd);
                }
            }
            return scopeInstallationSites;
        }

        /// <summary>
        /// 转化ScopeType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int GetType(ScopeType type)
        {
            if (type == ScopeType.Organization)
            {
                return 1;
            }
            else if (type == ScopeType.Railway)
            {
                return 2;
            }
            else if (type == ScopeType.Station)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        /// <summary>
        /// 获取用户所在组织机构和区域的组织机构之间的组织机构及兄弟节点
        /// </summary>
        /// <param name="id">传入的区域的组织机构id</param>
        /// <param name="parentId">用户所在的组织机构id</param>
        /// <param name="organizations">返回的list</param>
        /// <returns></returns>
        private List<Organization> GetParents(string? id, string? parentId, List<Organization> organizations)
        {

            var list = orgRepository
                .WithDetails(x => x.Children)
                .Where(x => x.Id.ToString() == id);
            organizations.AddRange(orgRepository.Where(x => x.Id.ToString() == id));

            var organization = organizations.FirstOrDefault(x => x.Id.ToString() == id);

            if (organization != null && organization.ParentId != null)
            {
                var organizationParents = orgRepository.Where(x => x.ParentId == organization.ParentId);
                organizations.AddRange(organizationParents);
                GetParents(organization.ParentId.ToString(), parentId, organizations);

            }

            return organizations;
        }


        /// <summary>
        /// 返回组织机构子级、组织机构下的非沿线的安装位置、组织机构下的线路及兄弟节点、线路下的站点及兄弟节点
        /// </summary>
        /// <param name="organizationsList">组织机构集合</param>
        /// <param name="orgList">组织机构id集合</param>
        /// <returns></returns>
        private List<GetListByScopeOutDto> ReturnScopesOrganizations(List<Guid> orgList)
        {
            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();
            List<Guid> raiwayList = new List<Guid>();



            //查询组织机构的子级
            var organizationChildren = orgRepository
                .WithDetails(x => x.Children)
                .WhereIf(orgList.Count > 0, x => orgList.Contains((Guid)x.ParentId) || orgList.Contains((Guid)x.Id));

            //查询组织机构下的非沿线的安装位置
            var installationSitesNoAlong = installationSitesRepository
                .WithDetails()
                .Where(x => orgList.Contains((Guid)x.OrganizationId) && x.RailwayId == null && x.StationId == null && x.ParentId == null);

            //查询组织机构下的线路
            var railwaies = _railwayRltOrganization
                .WithDetails(x => x.Railway, x => x.Organization)
                .Where(x => orgList.Contains((Guid)x.OrganizationId));

            List<Organization> organizationsChildrenList = new List<Organization>();

            //organizationsList.AddRange(organizationChildren);

            //整合组织机构数据
            foreach (var item in organizationChildren)
            {
                GetListByScopeOutDto sisd = new GetListByScopeOutDto
                {
                    Type = ScopeType.Organization,
                    Name = item.Name,
                    Id = item.Id,
                    ScopeCode = GetType(ScopeType.Organization) + "@" + item.Name + "@" + item.Id,
                };
                if (item.Parent != null && item.ParentId != null)
                {
                    sisd.ParentId = item.ParentId;
                }
                scopeInstallationSites.Add(sisd);
            }

            //组织机构去重
            var scopeByOrgList = new List<GetListByScopeOutDto>();
            foreach (var item in scopeInstallationSites)
            {
                if (scopeByOrgList.Find(x => x.ScopeCode == item.ScopeCode) == null)
                {
                    scopeByOrgList.Add(item);
                }
            }
            scopeInstallationSites = scopeByOrgList;

            //整合组织机构下非沿线的安装位置
            if (installationSitesNoAlong.Any())
            {
                foreach (var item in installationSitesNoAlong)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                    if (item.Children != null)
                    {
                        foreach (var itemChild in item.Children)
                        {
                            GetListByScopeOutDto sisdChild = new GetListByScopeOutDto();
                            sisdChild.ParentId = item.Id;
                            sisdChild.Name = itemChild.Name;
                            sisdChild.Type = ScopeType.InstallationSite;
                            sisdChild.Id = itemChild.Id;
                            scopeInstallationSites.Add(sisd);
                        }
                    }
                    sisd.ParentId = item.OrganizationId;
                    sisd.Name = item.Name;
                    sisd.Type = ScopeType.InstallationSite;
                    sisd.Id = item.Id;
                    sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.Organization.Id + "." +
                                     GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                    scopeInstallationSites.Add(sisd);
                }
            }

            //整合线路数据
            if (railwaies.Any())
            {
                foreach (var item in railwaies)
                {
                    if (item.Railway != null)
                    {
                        GetListByScopeOutDto sisd = new GetListByScopeOutDto
                        {
                            ParentId = item.OrganizationId,
                            Name = item.Railway.Name,
                            Type = ScopeType.Railway,
                            Id = (Guid)item.RailwayId,
                            ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                        GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId,
                        };
                        scopeInstallationSites.Add(sisd);
                        raiwayList.Add((Guid)item.RailwayId);
                    }
                }
            }

            // 查询站点及兄弟节点
            var stationBro = sta_railwayRepository
                .WithDetails()
                .Where(x => raiwayList.Contains(x.RailwayId));

            //整合站点数据
            if (stationBro.Any())
            {
                foreach (var item in stationBro)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                    if (item.Station != null)
                    {
                        if (item.RailwayId != Guid.Empty && item.RailwayId != null)
                        {
                            sisd.ParentId = item.RailwayId;
                        }

                        sisd.Name = item.Station.Name;
                        sisd.Type = ScopeType.Station;
                        sisd.Id = item.StationId;
                        scopeInstallationSites.Add(sisd);
                        sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Railway.RailwayRltOrganizations.FirstOrDefault().Organization.Name + "@" + item.Railway.RailwayRltOrganizations.FirstOrDefault().OrganizationId + "." +
                                         GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId + "." +
                                         GetType(ScopeType.Station) + "@" + item.Station.Name + "@" + item.StationId;
                    }
                }
            }
            return scopeInstallationSites;
        }

        /// <summary>
        /// 查找该组织机构集合下的组织机构子级、组织机构下的非沿线的安装位置、组织机构下的线路
        /// </summary>
        /// <param name="orgList">组织机构集合</param>
        /// <returns></returns>
        private List<GetListByScopeOutDto> ReturnScopesByOrgList(List<Guid> orgList)
        {
            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();
            List<Guid> raiwayList = new List<Guid>();

            //查询组织机构的子级
            var organizationChildren = orgRepository
                .WithDetails(x => x.Children)
                .WhereIf(orgList.Count > 0, x => orgList.Contains((Guid)x.ParentId));

            //查询组织机构下的非沿线的安装位置
            var installationSitesNoAlong = installationSitesRepository
                .WithDetails()
                .Where(x => orgList.Contains((Guid)x.OrganizationId) && x.RailwayId == null && x.StationId == null && x.ParentId == null);

            //查询组织机构下的线路
            var railwaies = _railwayRltOrganization
                .WithDetails(x => x.Railway, x => x.Organization)
                .Where(x => orgList.Contains((Guid)x.OrganizationId));

            //整合数据
            if (organizationChildren.Any())
            {
                foreach (var item in organizationChildren)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto
                    {
                        Name = item.Name,
                        Type = ScopeType.Organization,
                        Id = item.Id,
                        ScopeCode = GetType(ScopeType.Organization) + "@" + item.Name + "@" + item.Id,
                    };
                    if (item.Parent != null)
                    {
                        sisd.ParentId = item.ParentId;
                    }
                    scopeInstallationSites.Add(sisd);
                }
            }


            //整合组织机构下非沿线的安装位置
            if (installationSitesNoAlong.Any())
            {
                foreach (var item in installationSitesNoAlong)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto();
                    if (item.Children != null)
                    {
                        foreach (var itemChild in item.Children)
                        {
                            GetListByScopeOutDto sisdChild = new GetListByScopeOutDto();
                            sisdChild.ParentId = item.Id;
                            sisdChild.Name = itemChild.Name;
                            sisdChild.Type = ScopeType.InstallationSite;
                            sisdChild.Id = itemChild.Id;
                            scopeInstallationSites.Add(sisd);
                        }
                    }
                    sisd.ParentId = item.OrganizationId;
                    sisd.Name = item.Name;
                    sisd.Type = ScopeType.InstallationSite;
                    sisd.Id = item.Id;
                    sisd.ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.Organization.Id + "." +
                                     GetType(ScopeType.InstallationSite) + "@" + item.Name + "@" + item.Id;
                    scopeInstallationSites.Add(sisd);
                }
            }

            //整合线路数据
            if (railwaies.Any())
            {
                foreach (var item in railwaies)
                {
                    if (item.Railway != null)
                    {
                        GetListByScopeOutDto sisd = new GetListByScopeOutDto
                        {
                            ParentId = item.OrganizationId,
                            Name = item.Railway.Name,
                            Type = ScopeType.Railway,
                            Id = (Guid)item.RailwayId,
                            ScopeCode = GetType(ScopeType.Organization) + "@" + item.Organization.Name + "@" + item.OrganizationId + "." +
                                        GetType(ScopeType.Railway) + "@" + item.Railway.Name + "@" + item.RailwayId,
                        };
                        scopeInstallationSites.Add(sisd);
                        raiwayList.Add((Guid)item.RailwayId);
                    }
                }
            }
            return scopeInstallationSites;
        }

        /// <summary>
        /// 返回用户登陆的组织机构
        /// </summary>
        /// <returns></returns>
        private List<GetListByScopeOutDto> ReturnCurrentOrganization()
        {
            List<GetListByScopeOutDto> scopeInstallationSites = new List<GetListByScopeOutDto>();
            var organizationId = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"];

            //当前组织机构
            var organizations = orgRepository
                .WithDetails(x => x.Children)
                .WhereIf(!string.IsNullOrEmpty(organizationId.First()),
                    x => x.Id.ToString() == organizationId.First());
            //.Where(x => x.ParentId == null);

            List<Guid> orgList = new List<Guid>();
            //foreach (var item in organizations)
            //{
            //    GetListByScopeOutDto sisd = new GetListByScopeOutDto
            //    {
            //        Name = item.Name,
            //        Type = ScopeType.Organization,
            //        Id = item.Id,
            //        ScopeCode = GetType(ScopeType.Organization) + "@" + item.Name + "@" + item.Id.ToString()
            //    };
            //    if (item.Children.Count != 0)
            //    {
            //        GetListByScopeOutDto sisdChild = new GetListByScopeOutDto
            //        {
            //            ParentId = item.Id,
            //            Id = item.Children.First().Id,
            //            Name = item.Children.First().Name,
            //            Type = ScopeType.Organization
            //        };
            //        scopeInstallationSites.Add(sisdChild);
            //    }
            //    else
            //    {
            //        orgList.Add(item.Id);
            //    }
            //    scopeInstallationSites.Add(sisd);

            //}

            //组织机构下的线路
            //var railwaies = _railwayRltOrganization
            //    .WithDetails()
            //    .Where(x => orgList.Contains((Guid)x.OrganizationId) && x.RailwayId != null);

            //if (railwaies.Any())
            //{
            //    foreach (var orgItem in railwaies)
            //    {
            //        if (orgItem.Railway != null)
            //        {
            //            GetListByScopeOutDto sisd = new GetListByScopeOutDto
            //            {
            //                ParentId = orgItem.OrganizationId,
            //                Name = orgItem.Railway.Name,
            //                Type = ScopeType.Railway,
            //                Id = (Guid)orgItem.RailwayId,
            //            };
            //            scopeInstallationSites.Add(sisd);
            //        }
            //    }
            //}

            //查询组织机构下的非沿线的安装位置
            var installationSites = installationSitesRepository
                .WithDetails()
                .Where(x => x.RailwayId == null && x.StationId == null && x.ParentId == null);

            if (installationSites.Any())
            {
                foreach (var item in installationSites)
                {
                    GetListByScopeOutDto sisd = new GetListByScopeOutDto
                    {
                        ParentId = item.OrganizationId,
                        Name = item.Name,
                        Type = ScopeType.InstallationSite,
                        Id = (Guid)item.Id,
                    };
                    scopeInstallationSites.Add(sisd);
                }
            }

            return scopeInstallationSites;
        }


        private List<InstallationSite> GetInstallationSiteData(InstallationSiteSearchDto input)
        {
            //获取当前登录用户的组织机构
            //var organizationIdString = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"].ToString();
            //var organization = !string.IsNullOrEmpty(organizationIdString) ? orgRepository.FirstOrDefault(x => x.Id == Guid.Parse(organizationIdString)) : null;
            //var organizationCode = organization != null ? organization.Code : null;
            var result = new PagedResultDto<InstallationSiteDto>();
            var query = installationSitesRepository.WithDetails()
                .Where(x => x.CSRGCode.Substring(0, 2) == "SA")
                .WhereIf(string.IsNullOrEmpty(input.Keyword) &&
                        // (input.OrganizationId == null || input.OrganizationId == Guid.Empty) &&
                        (input.RailwayId == null || input.RailwayId == Guid.Empty) &&
                        (input.TypeId == null || input.TypeId == Guid.Empty), x => x.ParentId == input.ParentId)
                //  .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.OrganizationId == input.OrganizationId)
                .WhereIf(input.RailwayId != null && input.RailwayId != Guid.Empty, x => x.RailwayId == input.RailwayId)
                .WhereIf(input.StationId != null && input.StationId != Guid.Empty, x => x.StationId == input.StationId)
                .WhereIf(input.TypeId != null && input.TypeId != Guid.Empty, x => x.TypeId == input.TypeId)
                .WhereIf(input.LocationType.IsIn(InstallationSiteLocationType.RailwayOuter, InstallationSiteLocationType.SectionInner, InstallationSiteLocationType.StationInner, InstallationSiteLocationType.Other), x => x.LocationType == input.LocationType)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), s =>
                     s.Name.Contains(input.Keyword) ||
                     s.Station.Name.Contains(input.Keyword) ||
                     s.Code.Contains(input.Keyword) ||
                     s.CSRGCode.Contains(input.Keyword)
             );

            //query = query.WhereIf(organization != null && !string.IsNullOrEmpty(organizationCode),
            //    x => x.Organization.Code.StartsWith(organizationCode)
            //    || x.OrganizationId == null);
            return query.ToList();
        }


    }
}
