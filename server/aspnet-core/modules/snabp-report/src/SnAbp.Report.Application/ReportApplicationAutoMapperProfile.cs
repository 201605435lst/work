using AutoMapper;
using SnAbp.Report.Dtos;
using SnAbp.Report.Entities;

namespace SnAbp.Report
{
    public class ReportApplicationAutoMapperProfile : Profile
    {
        public ReportApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<ReportDto, Report>();
            CreateMap<Report, ReportDto>();
            CreateMap<ReportCreateDto, Report>();
            CreateMap<Report, ReportCreateDto>();
            CreateMap<ReportUpdateDto, Report>();
            CreateMap<Report, ReportUpdateDto>();

            CreateMap<ReportRltFileDto, ReportRltFile>();
            CreateMap<ReportRltFile, ReportRltFileDto>();

            CreateMap<ReportRltUser, ReportRltUserDto>();
            CreateMap<ReportRltUserDto, ReportRltUser>();
        }
    }
}