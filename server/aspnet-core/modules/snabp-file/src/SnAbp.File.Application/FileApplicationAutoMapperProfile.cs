using AutoMapper;
using SnAbp.File.Dtos;
using SnAbp.File.Dtos.Tag;
using SnAbp.File.Entities;
using SnAbp.Identity;
using Volo.Abp.AutoMapper;

namespace SnAbp.File
{
    public class FileApplicationAutoMapperProfile : Profile
    {
        public FileApplicationAutoMapperProfile()
        {
            CreateMap<OssServer, OssConfigDto>();
            CreateMap<OssConfigDto, OssServer>();
            CreateMap<OssServer, OssConfigInputDto>();
            CreateMap<OrganizationTreeDto, Organization>()
                .Ignore(a => a.ExtraProperties);
            CreateMap<Organization, OrganizationTreeDto>();

            CreateMap<FileTagDto, Tag>();
            CreateMap<Tag, FileTagDto>();
            CreateMap<Tag, FileTagCreateDto>();
            CreateMap<FileTagCreateDto, Tag>();

            CreateMap<FileRltTagDto, FileRltTag>();
            CreateMap<FileRltTag, FileRltTagDto>();

            CreateMap<Folder, FileFolderInputDto>();
            CreateMap<FileFolderInputDto, Folder>();
            CreateMap<Folder, FileFolderDto>();
            CreateMap<FileFolderDto, Folder>();
            CreateMap<Folder, Folder>();

            CreateMap<Entities.File, Entities.File>();
            CreateMap<Entities.File, FileCreateDto>();
            CreateMap<FileCreateDto, Entities.File>();

            CreateMap<Entities.File, FileSimpleDto>();
            CreateMap<FileSimpleDto, Entities.File>();

            CreateMap<FileVersionCreateDto, FileVersion>();
            CreateMap<FileVersion, FileVersionCreateDto>();

            CreateMap<FileVersion, FileVersionDto>();
            CreateMap<FileVersionDto, FileVersion>();
            CreateMap<FileVersion, FileVersion>();

            CreateMap<Entities.File, ResourceDto>();
            CreateMap<ResourceDto, Entities.File>();

            CreateMap<ResourcePermissionCreateDto, FileRltShare>();
            CreateMap<ResourcePermissionCreateDto, FolderRltShare>();
            CreateMap<FileRltShare, ResourcePermissionCreateDto>();
            CreateMap<FolderRltShare, ResourcePermissionCreateDto>();
            CreateMap<ResourcePermissionCreateDto, FileRltPermissions>();
            CreateMap<ResourcePermissionCreateDto, FolderRltPermissions>();
            CreateMap<FileRltPermissions, ResourcePermissionCreateDto>();
            CreateMap<FolderRltPermissions, ResourcePermissionCreateDto>();

            CreateMap<FileMigrationState, FileMigrationDto>();;
            CreateMap<FileMigrationDto, FileMigrationState>();;


        }
    }
}