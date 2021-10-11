using AutoMapper;
using SnAbp.Quality.Dtos;
using SnAbp.Quality.Entities;

namespace SnAbp.Quality
{
    public class QualityApplicationAutoMapperProfile : Profile
    {
        public QualityApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */



            CreateMap<QualityProblemLibraryRltScopDto, QualityProblemLibraryRltScop>();
            CreateMap<QualityProblemLibraryRltScop, QualityProblemLibraryRltScopDto>();

            CreateMap<QualityProblem, QualityProblemDto>();
            CreateMap<QualityProblemDto, QualityProblem>();

            CreateMap<QualityProblem, QualityProblemReportDto>();
            CreateMap<QualityProblemReportDto, QualityProblem>();

            CreateMap<QualityProblem, QualityProblemCreateDto>();
            CreateMap<QualityProblemCreateDto, QualityProblem>();
            CreateMap<QualityProblem, QualityProblemUpdateDto>();
            CreateMap<QualityProblemUpdateDto, QualityProblem>();

            CreateMap<QualityProblemRltFile, QualityProblemRltFileDto>();
            CreateMap<QualityProblemRltFileDto, QualityProblemRltFile>();
            CreateMap<QualityProblemRltFile, QualityProblemRltFileCreateDto>();
            CreateMap<QualityProblemRltFileCreateDto, QualityProblemRltFile>();

            CreateMap<QualityProblemRltCcUser, QualityProblemRltCcUserDto>();
            CreateMap<QualityProblemRltCcUserDto, QualityProblemRltCcUser>();
            CreateMap<QualityProblemRltCcUser, QualityProblemRltCcUserCreateDto>();
            CreateMap<QualityProblemRltCcUserCreateDto, QualityProblemRltCcUser>();

            CreateMap<QualityProblemRltEquipment, QualityProblemRltEquipmentDto>();
            CreateMap<QualityProblemRltEquipmentDto, QualityProblemRltEquipment>();
            CreateMap<QualityProblemRltEquipment, QualityProblemRltEquipmentCreateDto>();
            CreateMap<QualityProblemRltEquipmentCreateDto, QualityProblemRltEquipment>();

            CreateMap<QualityProblemRecord, QualityProblemRecordDto>();
            CreateMap<QualityProblemRecordDto, QualityProblemRecord>();
            CreateMap<QualityProblemRecord, QualityProblemRecordCreateDto>();
            CreateMap<QualityProblemRecordCreateDto, QualityProblemRecord>();

            CreateMap<QualityProblemRecordRltFile, QualityProblemRecordRltFileDto>();
            CreateMap<QualityProblemRecordRltFileDto, QualityProblemRecordRltFile>();
            CreateMap<QualityProblemRecordRltFile, QualityProblemRecordRltFileCreateDto>();
            CreateMap<QualityProblemRecordRltFileCreateDto, QualityProblemRecordRltFile>();


            CreateMap<QualityProblemLibrary, QualityProblemLibraryDto>();
            CreateMap<QualityProblemLibraryDto, QualityProblemLibrary>();
            CreateMap<QualityProblemLibrary, QualityProblemLibraryCreateDto>();
            CreateMap<QualityProblemLibraryCreateDto, QualityProblemLibrary>();
            CreateMap<QualityProblemLibrary, QualityProblemLibraryUpdateDto>();
            CreateMap<QualityProblemLibraryUpdateDto, QualityProblemLibrary>();

            CreateMap<QualityProblemLibraryRltScop, QualityProblemLibraryRltScopDto>();
            CreateMap<QualityProblemLibraryRltScopDto, QualityProblemLibraryRltScop>();
            CreateMap<QualityProblemLibraryRltScopCreateDto, QualityProblemLibraryRltScop>();
            CreateMap<QualityProblemLibraryRltScop, QualityProblemLibraryRltScopCreateDto>();
        }
    }
}