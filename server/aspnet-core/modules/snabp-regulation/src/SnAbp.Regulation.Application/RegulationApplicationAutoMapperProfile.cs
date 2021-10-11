using AutoMapper;
using SnAbp.Regulation.Dtos;
using SnAbp.Regulation.Dtos.Institution;
using SnAbp.Regulation.Dtos.Label;
using SnAbp.Regulation.Entities;

namespace SnAbp.Regulation
{
    public class RegulationApplicationAutoMapperProfile : Profile
    {
        public RegulationApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Label, LabelDto>();
            CreateMap<LabelDto, Label>();
            CreateMap<LabelCreateDto, Label>();

            CreateMap<Institution, InstitutionDto>();
            CreateMap<InstitutionDto, Institution>();
            CreateMap<Institution,InstitutionExportDto>();

            CreateMap<InstitutionRltFile, InstitutionRltFileDto>();
            CreateMap<InstitutionRltFileDto, InstitutionRltFile>();

            CreateMap<InstitutionRltLabel, InstitutionRltLabelDto>();
            CreateMap<InstitutionRltLabelDto, InstitutionRltLabel>();

            CreateMap<InstitutionRltAuthority, InstitutionRltAuthorityDto>();
            CreateMap<InstitutionRltAuthorityDto, InstitutionRltAuthority>();

            CreateMap<InstitutionRltEdition, InstitutionRltEditionDto>();
            CreateMap<InstitutionRltEditionDto, InstitutionRltEdition>();
        }
    }
}