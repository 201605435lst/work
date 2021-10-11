using AutoMapper;
using SnAbp.File.Dtos;
using SnAbp.FileApprove.Dto;

namespace SnAbp.FileApprove
{
    public class FileApproveApplicationAutoMapperProfile : Profile
    {
        public FileApproveApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<FileApprove, FileApproveDto>();
            CreateMap<FileApproveDto, FileApprove>();
            CreateMap<FileApprove, FileApproveSimpleDto>();
            CreateMap<FileApproveSimpleDto, FileApprove>();
            CreateMap<ResourceDto, FileRltFileApproveDto>();
            CreateMap<FileRltFileApproveDto,ResourceDto>();

        }
    }
}