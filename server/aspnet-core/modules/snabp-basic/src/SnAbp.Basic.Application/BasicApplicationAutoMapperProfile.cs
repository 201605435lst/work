using AutoMapper;
using SnAbp.Basic.Dtos;
using SnAbp.Basic.Entities;
using SnAbp.Basic.Template;

namespace SnAbp.Basic
{
    public class BasicApplicationAutoMapperProfile : Profile
    {
        public BasicApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            //CreateMap<OrganizationInputDto, Organization>();
            //CreateMap<OrganizationUpdateDto, Organization>();
            //CreateMap<Organization, OrganizationDto>();

            CreateMap<Railway, RailwayDto>();
            CreateMap<RailwayDto, Railway>();
            CreateMap<Railway, RailwayDetailDto>();
            CreateMap<RailwayDetailDto, Railway>();
            CreateMap<Railway, RailwaySimpleDto>();
            CreateMap<RailwaySimpleDto, Railway>();
            CreateMap<Railway, RailwayImportTemplate>();
            CreateMap<RailwayImportTemplate, Railway>();

            CreateMap<RailwayRltOrganization, RailwayOrgDto>();
            CreateMap<RailwayOrgDto, RailwayRltOrganization>();

            CreateMap<Station, StationDto>();
            CreateMap<StationDto, Station>();
            CreateMap<Station, StationInputDto>();
            CreateMap<StationInputDto, Station>();
            CreateMap<Station, StationUpdateDto>();
            CreateMap<StationUpdateDto, Station>();
            CreateMap<Station, StationSimpleDto>();
            CreateMap<StationSimpleDto, Station>();
            CreateMap<Station, StationDetailDto>();
            CreateMap<StationDetailDto, Station>();
            CreateMap<Station, StationVerySimpleDto>();
            CreateMap<StationVerySimpleDto, Station>();
            CreateMap<Station, StationImportTemplate>();
            CreateMap<StationImportTemplate, Station>();

            CreateMap<StationRltRailway, Station_RailwayDto>();
            CreateMap<Station_RailwayDto, StationRltRailway>();


            CreateMap<InstallationSite, InstallationSiteDetailDto>();
            CreateMap<InstallationSiteDetailDto, InstallationSite>();
            CreateMap<InstallationSite, InstallationSiteSimpleDto>();
            CreateMap<InstallationSiteSimpleDto, InstallationSite>();
            CreateMap<InstallationSite, InstallationSiteDto>();
            CreateMap<InstallationSiteDto, InstallationSite>();
            CreateMap<InstallationSiteCreateDto, InstallationSite>();
            CreateMap<InstallationSite, InstallationSiteImportTemplate>();
            CreateMap<InstallationSiteImportTemplate, InstallationSite>();

            CreateMap<ScopeInstallationSite, GetListByScopeOutDto>();
            CreateMap<GetListByScopeOutDto, ScopeInstallationSite>();

        }
    } 
}