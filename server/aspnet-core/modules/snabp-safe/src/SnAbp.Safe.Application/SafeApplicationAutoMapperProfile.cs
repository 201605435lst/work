using AutoMapper;

using SnAbp.Safe.Dtos;
using SnAbp.Safe.Entities;

namespace SnAbp.Safe
{
    public class SafeApplicationAutoMapperProfile : Profile
    {
        public SafeApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<SafeSpeechVideoDto, SafeSpeechVideo>();
            CreateMap<SafeSpeechVideo, SafeSpeechVideoDto>();
    

            CreateMap<SafeProblemLibraryRltScopDto, SafeProblemLibraryRltScop>();
            CreateMap<SafeProblemLibraryRltScop, SafeProblemLibraryRltScopDto>();

            CreateMap<SafeProblem, SafeProblemDto>();
            CreateMap<SafeProblemDto,SafeProblem>();

            CreateMap<SafeProblem, SafeProblemReportDto>();
            CreateMap<SafeProblemReportDto, SafeProblem>();

            CreateMap<SafeProblem, SafeProblemCreateDto>();
            CreateMap<SafeProblemCreateDto,SafeProblem>();
            CreateMap<SafeProblem, SafeProblemUpdateDto>();
            CreateMap<SafeProblemUpdateDto,SafeProblem>();

            CreateMap<SafeProblemRltFile, SafeProblemRltFileDto>();
            CreateMap<SafeProblemRltFileDto,SafeProblemRltFile>();
            CreateMap<SafeProblemRltFile, SafeProblemRltFileSimpleDto>();
            CreateMap<SafeProblemRltFileSimpleDto, SafeProblemRltFile>();

            CreateMap<SafeProblemRltCcUser, SafeProblemRltCcUserDto>();
            CreateMap<SafeProblemRltCcUserDto,SafeProblemRltCcUser>();
            CreateMap<SafeProblemRltCcUser, SafeProblemRltCcUserSimpleDto>();
            CreateMap<SafeProblemRltCcUserSimpleDto,SafeProblemRltCcUser>();

            CreateMap<SafeProblemRltEquipment, SafeProblemRltEquipmentDto>();
            CreateMap<SafeProblemRltEquipmentDto,SafeProblemRltEquipment>();
            CreateMap<SafeProblemRltEquipment, SafeProblemRltEquipmentSimpleDto>();
            CreateMap<SafeProblemRltEquipmentSimpleDto,SafeProblemRltEquipment>();

            CreateMap<SafeProblemRecord, SafeProblemRecordDto>();
            CreateMap<SafeProblemRecordDto,SafeProblemRecord>();
            CreateMap<SafeProblemRecord, SafeProblemRecordCreateDto>();
            CreateMap<SafeProblemRecordCreateDto,SafeProblemRecord>();

            CreateMap<SafeProblemRecordRltFile, SafeProblemRecordRltFileDto>();
            CreateMap<SafeProblemRecordRltFileDto,SafeProblemRecordRltFile>();
            CreateMap<SafeProblemRecordRltFile, SafeProblemRecordRltFileSimpleDto>();
            CreateMap<SafeProblemRecordRltFileSimpleDto,SafeProblemRecordRltFile>();


            CreateMap<SafeProblemLibrary, SafeProblemLibraryDto>();
            CreateMap<SafeProblemLibraryDto,SafeProblemLibrary>();
            CreateMap<SafeProblemLibrary, SafeProblemLibraryCreateDto>();
            CreateMap<SafeProblemLibraryCreateDto,SafeProblemLibrary>();
            CreateMap<SafeProblemLibrary, SafeProblemLibraryUpdateDto>();
            CreateMap<SafeProblemLibraryUpdateDto,SafeProblemLibrary>();
            
            CreateMap<SafeProblemLibraryRltScop, SafeProblemLibraryRltScopDto>();
            CreateMap<SafeProblemLibraryRltScopDto,SafeProblemLibraryRltScop>();
            CreateMap<SafeProblemLibraryRltScopSimpleDto, SafeProblemLibraryRltScop>();
            CreateMap<SafeProblemLibraryRltScop, SafeProblemLibraryRltScopSimpleDto>();



        }
    }
}