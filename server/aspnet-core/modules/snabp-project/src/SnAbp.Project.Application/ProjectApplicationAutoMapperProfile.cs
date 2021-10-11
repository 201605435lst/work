using AutoMapper;
using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;

namespace SnAbp.Project
{
    public class ProjectApplicationAutoMapperProfile : Profile
    {
        public ProjectApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */


            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();

            CreateMap<ProjectDto, ProjectCreateDto>();
            CreateMap<ProjectCreateDto, ProjectDto>();

            CreateMap<ProjectRltContract, ProjectRltContractDto>();
            CreateMap<ProjectRltContractDto, ProjectRltContract>();

            CreateMap<ProjectRltFile, ProjectRltFileDto>();
            CreateMap<ProjectRltFileDto, ProjectRltFile>();

            CreateMap<ProjectRltMember, ProjectRltMemberDto>();
            CreateMap<ProjectRltMemberDto, ProjectRltMember>();

            CreateMap<ProjectRltUnit, ProjectRltUnitDto>();
            CreateMap<ProjectRltUnitDto, ProjectRltUnit>();

            CreateMap<Unit, UnitDto>();
            CreateMap<UnitDto, Unit>();

            CreateMap<Archives, ArchivesDto>();
            CreateMap<ArchivesDto, Archives>();
            CreateMap<Archives, ArchivesUpdateDto>();
            CreateMap<ArchivesUpdateDto, Archives>();
            CreateMap<Archives, ArchivesCreateDto>();
            CreateMap<ArchivesCreateDto, Archives>();

            CreateMap<ArchivesCategory, ArchivesCategoryDto>();
            CreateMap<ArchivesCategoryDto, ArchivesCategory>();
            CreateMap<ArchivesCategory, ArchivesCategoryUpdateDto>();
            CreateMap<ArchivesCategoryUpdateDto, ArchivesCategory>();
            CreateMap<ArchivesCategory, ArchivesCategoryCreateDto>();
            CreateMap<ArchivesCategoryCreateDto, ArchivesCategory>();

            CreateMap<Dossier, DossierDto>();
            CreateMap<DossierDto, Dossier>();
            CreateMap<Dossier, DossierUpdateDto>();
            CreateMap<DossierUpdateDto, Dossier>();
            CreateMap<Dossier, DossierCreateDto>();
            CreateMap<DossierCreateDto, Dossier>();

            CreateMap<DossierCategory, DossierCategoryDto>();
            CreateMap<DossierCategoryDto, DossierCategory>();
            CreateMap<DossierCategory, DossierCategoryUpdateDto>();
            CreateMap<DossierCategoryUpdateDto, DossierCategory>();
            CreateMap<DossierCategory, DossierCategoryCreateDto>();
            CreateMap<DossierCategoryCreateDto, DossierCategory>();

            CreateMap<FileCategory, FileCategoryDto>();
            CreateMap<FileCategoryDto, FileCategory>();

            CreateMap<FileCategory, FileClassificationDto>();
            CreateMap<FileClassificationDto, FileCategory>();

            CreateMap<BooksClassification, BooksClassificationDto>();
            CreateMap<BooksClassificationDto, BooksClassification>();

            CreateMap<BooksClassification, ArchivesCategory>();
            CreateMap<ArchivesCategory, BooksClassification>();
            CreateMap<BooksClassification, BooksClassificationSimpleDto>();
            CreateMap<BooksClassificationSimpleDto, BooksClassification>();

            CreateMap<ArchivesCategory, FileClassificationDto>();
            CreateMap<FileClassificationDto, ArchivesCategory>();

            CreateMap<ArchivesCategory, Archives>();
            CreateMap<Archives, ArchivesCategory>();


            CreateMap<Archives, FileClassificationDto>();
            CreateMap<FileClassificationDto, Archives>();
        }
    }
}