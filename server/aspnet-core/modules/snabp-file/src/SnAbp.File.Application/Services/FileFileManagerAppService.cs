/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： FileFileManagerAppService
*******类 说 明： 文件管理服务接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 10:18:32
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.IServices;
using SnAbp.File.OssSdk;
using SnAbp.File.Repositories;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.File.Services
{
    [Authorize]
    public class FileFileManagerAppService : FileAppService, IFileFileManagerAppService
    {
        protected IFileManager FileManager { get; }
        private readonly IdentityUserManager _identityUserManager;
        private readonly IGuidGenerator _generator;
        private readonly IDataFilter _dataFilter;
        private readonly ICustomRepository _customRespository;
        private readonly IOssRepository _ossResp;
        private readonly IRepository<FileVersion, Guid> _fileVersionResp;
        private readonly IRepository<Tag, Guid> _tagResp;
        private readonly IRepository<Folder, Guid> _folderResp;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public FileFileManagerAppService(
            IFileManager fileManager,
            IGuidGenerator generator,
            IDataFilter dataFilter,
            IOssRepository ossResp,
            IRepository<FileVersion, Guid> fileVersionResp,
            ICustomRepository customRepository,
            IRepository<Tag, Guid> tagResp,
            IConfiguration configuration,
            IUnitOfWorkManager unitOfWorkManager,
            IdentityUserManager identityUserManager
, IRepository<Folder, Guid> folderResp = null)
        {
            FileManager = fileManager;
            _identityUserManager = identityUserManager;
            _generator = generator;
            _dataFilter = dataFilter;
            _customRespository = customRepository;
            _ossResp = ossResp;
            _fileVersionResp = fileVersionResp;
            _tagResp = tagResp;
            _configuration = configuration;
            _unitOfWorkManager = unitOfWorkManager;
            _folderResp = folderResp;
        }

        /// <summary>
        /// 文件查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ResourceDto>> Get(ResourceSearchInput input)
        {
            var result = new PagedResultDto<ResourceDto>();

            // 查询逻辑变更，当查询“我的”目录时，需要进行单独的判读。

            IQueryable<Entities.File> query;
            switch (input.NodeType)
            {
                case ResourceNodeType.Folder:
                    var folder = await FileManager.GetFolder(input.NodeId);
                    var folderIds = FileManager.GetFolderList(input.NodeId, (int)(input.NodeType + 1), a => a.IsPublic && !a.IsDeleted).Select(x => x.Id)?.ToList();
                    folderIds.Add(input.NodeId);
                    query = FileManager.GetFileList(a =>
                        !a.IsDeleted && a.Name.Contains(input.Name) && a.OrganizationId == folder.OrganizationId && folderIds.Contains(a.FolderId.Value));
                    break;
                case ResourceNodeType.Organization:
                    query = FileManager.GetFileList(a =>
                        !a.IsDeleted && a.Name.Contains(input.Name) && a.OrganizationId == input.NodeId);
                    break;
                default:
                    var tags = FileManager.GetFileList(input.TagId, 3);
                    query = tags.Where(a => !a.IsHidden && a.IsPublic);
                    break;
            }

            if (query == null)
            {
                return null;
            }

            if (input.TagId != Guid.Empty)
            {
                query = query.Where(a => a.Tags.Select(b => b.TagId).Contains(input.TagId))
                    .WhereIf(condition: !input.Name.IsNullOrEmpty(), a => a.Name.Contains(input.Name));
            }
            result.TotalCount = query.Count();
            result.Items = GetResource(query, input.Page, input.Size);
            return await Task.FromResult(result);

        }

        /// <summary>
        /// 获取当前启用的对象存储服务地址
        /// </summary>
        /// <returns></returns>
        public Task<string> GetEndPoint()
        {
            try
            {
                var oss = _ossResp?.OssServer;
                return oss != null ? Task.FromResult(oss.Type == OssServerType.Aliyun ? $"//{_configuration["OssConfig:PublicBucket"]}.{oss.EndPoint}" : $"//{oss?.EndPoint}/{_configuration["OssConfig:PublicBucket"]}") : Task.FromResult("");

            }
            catch (Exception e)
            {
                throw new UserFriendlyException("请配置服务");
            }
        }

        /// <summary>
        /// 获取组织结构id
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns></returns>
        public async Task<Guid> GetOrganizationId(OrganizationInputDto input)
        {
            var organizationId = input.Type switch
            {
                ResourceType.File => (await FileManager.GetFile(input.Id))?.OrganizationId,
                ResourceType.Folder => (await FileManager.GetFolder(a => a.Id == input.Id))?.OrganizationId,
                ResourceType.Tag => _tagResp.FirstOrDefault(a => a.Id == input.Id)?.OrganizationId,
                _ => Guid.Empty
            };
            return organizationId.GetValueOrDefault();
        }

        /// <summary>
        /// 获取组织树结构
        /// </summary>
        /// <returns>组织结构树，包括组织和文件夹</returns>
        public async Task<List<OrganizationTreeDto>> GetOrganizationTreeList()
        {
            var userid = CurrentUser.Id;
            if (userid == null)
            {
                return null;
            }
            // 用户所在的组织机构
            //var organizations = await FileManager.GetOrganizations(userid.Value);
            var organizations = await _identityUserManager.GetOrganizationsAsync(CurrentUser.Id);
            if (!organizations.Any())
            {
                return null;
            }

            var folders = FileManager.GetFolderList(a => organizations.Select(b => b.Id).ToGuidArray().Contains(a.OrganizationId) && a.IsPublic).ToList();
            return BuildOrganizationTree(organizations, folders);

        }

        /// <summary>
        /// 根据节点获取资源信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>getStaticTreeList
        public async Task<PagedResultDto<ResourceDto>> GetResourceList(ResourceInputDto input)
        {
            List<Folder> folders = null;
            List<Entities.File> files = null;
            List<Entities.File> allFiles = null; // 用来计算文件夹的大小
            var pageResult = new PagedResultDto<ResourceDto>();

            if (input.IsDelete)
            {
                /* 获取回收站数据
                 * 1、获取所有被删除切没有隐藏的数据
                 * 2、判断删除的文件夹和文件中有无层级关系
                 * 3、对存在层级关系的文件夹及文件计算其资源大小
                 * **/


                using (_dataFilter.Disable<ISoftDelete>())
                {
                    if (input.Id != Guid.Empty)
                    {
                        // 根据文件夹id进行查询
                        folders = FileManager.GetFolderList(input.Id, input.Type, a => a.IsDeleted).ToList();
                        allFiles = FileManager.GetFileList(input.Id, input.Type, a => a.IsDeleted).ToList();
                        files = allFiles.Where(a => a.FolderId == input.Id).ToList();
                    }
                    else
                    {
                        var deleteFolders = FileManager.GetFolderList(a => a.IsDeleted).ToList();
                        var deleteFiles = FileManager.GetFileList(ResourceQueryType.Deleted, a => !a.IsHidden);
                        CheckResource(deleteFolders.ToList(), deleteFiles.ToList(), out folders, out files);
                    }
                }
            }
            else if (input.IsMine)
            {
                // 获取“我的”
                if (input.Id == Guid.Empty)
                {
                    // 顶级节点点击获取
                    folders = FileManager
                        .GetFolderList(ResourceQueryType.UnPublish, a => a.CreatorId == CurrentUser.Id && a.ParentId == null && !a.IsDeleted)?.ToList();
                    files = FileManager.GetFileList(ResourceQueryType.UnPublish, a => a.CreatorId == CurrentUser.Id && a.FolderId == null && !a.IsDeleted && !a.IsHidden)?.ToList();
                }
                else
                {
                    folders = FileManager.GetFolderList(input.Id, input.Type, a => !a.IsPublic && !a.IsDeleted)?.ToList();
                    files = FileManager.GetFileList(input.Id, input.Type, a => !a.IsPublic && !a.IsDeleted && !a.IsHidden)?.ToList();
                }
                allFiles = FileManager.GetInFolderFiles(input.Id, input.Type);

            }
            else if (input.IsShare)
            {
                // 获取共享中心数据，点击组织结构是只返回被共享的文件或文件夹，点击文件夹时返回文件夹中的内容即可

                if (input.Type == 1)
                {
                    folders = FileManager.GetFolderList(ResourceQueryType.Shared, a => a.OrganizationId == input.Id && !a.IsDeleted && a.IsPublic)?.ToList();
                    files = FileManager.GetFileList(ResourceQueryType.Shared, a => a.OrganizationId == input.Id && a.IsPublic && !a.IsDeleted && !a.IsHidden)?.ToList();
                }
                else
                {
                    folders = FileManager.GetFolderList(input.Id, input.Type, a => a.IsPublic && !a.IsDeleted)?.ToList();
                    files = FileManager.GetFileList(input.Id, input.Type, a => !a.IsHidden && a.IsPublic && !a.IsDeleted)?.ToList();
                }
                allFiles = FileManager.GetInFolderFiles(input.Id, input.Type);
            }
            else if (input.Approval)
            {
                var foldersIds = FileManager.GetFolderList(a => a.StaticKey == input.StaticKey && a.IsPublic && !a.IsDeleted)?.Select(s => s.Id).ToList();
                files = FileManager.GetFileList(ResourceQueryType.NotDelete, a => !a.IsDeleted && foldersIds.Contains((Guid)a.FolderId))?.ToList();

                allFiles = FileManager.GetInFolderFiles(input.Id, input.Type);
            }
            else if (!string.IsNullOrEmpty(input.StaticKey))
            {
                // 获取静态文件列表
                folders = FileManager.GetFolderList(input.Id, input.Type, a => a.StaticKey == input.StaticKey && a.IsPublic && !a.IsDeleted)?.ToList();
                files = FileManager.GetFileList(input.Id, input.Type, a => !a.IsHidden && a.IsPublic && !a.IsDeleted)?.ToList();
                allFiles = FileManager.GetInFolderFiles(input.Id, input.Type);
            }
            else
            {
                // 获取其他的列表
                folders = FileManager.GetFolderList(input.Id, input.Type, a => a.IsPublic && !a.IsDeleted)?.ToList();
                files = FileManager.GetFileList(input.Id, input.Type, a => !a.IsHidden && a.IsPublic && !a.IsDeleted)?.ToList();
                allFiles = FileManager.GetInFolderFiles(input.Id, input.Type);
            }

            var result = GetResourceList(folders, files, allFiles);

            if (input.IsApprove)
            {
                pageResult.Items = result;
            }
            else
            {
                pageResult.TotalCount = result.Count;
                pageResult.Items = result
                    .Skip(input.Page)
                    .Take(input.Size)
                    .ToList();
            }
            return await Task.FromResult(pageResult);
        }


        /// <summary>
        /// 获取共享中心目录树列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrganizationTreeDto>> GetShareCenterTreeList()
        {
            /* 逻辑：
             *1、过滤所有的被共享的文件夹，并判断出其组织结构层级
             *   如果组织机构跟目录下的文件共享，则在共享中心中构造出该文件对应的组织机构
             *2、点击树节点，获取资源，需要区分权限信息，如果共享内容当前用户（角色/组织不可见）
             */

            // 获取当前用户的成员类型
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.GetValueOrDefault());

            var memberIds = members.Select(x => x.Id).ToList();
            // 只获取当前用户成员id内的全新信息并绑定到结果中
            var enumerable = FileManager
                .GetFolderList(ResourceQueryType.Shared, a => !a.IsDeleted);
            if (enumerable.Any())
            {
                // 处理每个文件夹的权限信息
                foreach (var item in enumerable.AsEnumerable())
                {
                    item.Permissions = item.Permissions.Where(a => members.Select(b => b.Id).Contains(a.MemberId.GetValueOrDefault())).ToList();
                }
            }

            // 逻辑修改，共享文件的组织机构，不能显示子组织结构，但是可以显示被共享组织机构的直系父级。

            //var organizationIds = enumerable
            //    .WhereIf(enumerable.Any(), a => a.ParentId == null)
            //    .Select(a => a.OrganizationId)
            //    .ToList();

            var organizationIds = new List<Guid>();
            var organizationList = new List<Organization>();

            // 获取被共享的文件信息
            var shareFiles = FileManager.GetFileList(a => a.IsShare).ToList();
            var shareFolder = FileManager.GetFolderList(a => a.IsShare).ToList();

            //获取分享成员信息
            var currentMembers = members.GroupBy(x => x.Type);
            if (shareFiles.Any() || shareFolder.Any())
            {
                // 判断这些文件是否在上面查找到的文件夹中，如果不在，则需要将该文件的组织结构拿出来
                shareFiles?.ForEach(a =>
                {
                    //if (!enumerable.Any(b => b.Id == a.FolderId))
                    //{
                    //    organizationIds.Add(a.OrganizationId.GetValueOrDefault());
                    //}
                    var fileMember = a.Shares.Select(x => x.MemberId).ToList();
                    foreach (var item in fileMember)
                    {
                        foreach (var currentMember in memberIds)
                        {
                            if (item == currentMember)
                            {
                                organizationIds.Add(a.OrganizationId.GetValueOrDefault());
                            }
                        }
                    }

                    //if(a.Shares.Select(x=>x.MemberId).Contains())
                });

                shareFolder?.ForEach(b =>
                {
                    var folderMember = b.Shares.Select(x => x.MemberId).ToList();
                    foreach (var item in folderMember)
                    {
                        foreach (var currentMember in memberIds)
                        {
                            if (item == currentMember)
                            {
                                organizationIds.Add(b.OrganizationId.GetValueOrDefault());
                            }
                        }
                    }
                });
            }

            //根据guid查找到组织结构，并根据组织结构查找直系节点
            var organizations = await FileManager.GetOrganizations(a => organizationIds.Contains(a.Id));

            organizations?.ForEach(a =>
            {
                var parentCodes = GetParentCode(a.Code);
                var organizationArr = FileManager.GetOrganizations(a => parentCodes.Contains(a.Code)).Result;
                organizationList.AddRange(organizationArr);
            });
            organizationList = organizationList.Distinct().ToList();
            // 根据已有节点向前查找所有的文件夹
            return BuildOrganizationTree(organizationList, enumerable.ToList(), true);
        }

        private static List<string> GetParentCode(string code)
        {
            var arr = code.Split(".");
            return arr.Select((a, b) => arr.Take(b + 1).JoinAsString(".")).ToList();
        }

        /// <summary>
        /// 获取“我的”文件目录，只获取当前用户上传的文件信息，并且是未公开的
        /// </summary>
        /// <returns></returns>
        public async Task<List<MineTreeDto>> GetMineTreeList()
        {
            var result = new List<MineTreeDto>();
            var data = FileManager
                .GetFolderList(ResourceQueryType.UnPublish, a => a.CreatorId == CurrentUser.Id && !a.IsDeleted)
                .ToList();
            if (!data.Any())
            {
                return await Task.FromResult(result);
            }

            {
                var topNodes = data.Where(a => a.ParentId == null).ToList();
                topNodes?.ForEach(a =>
                {
                    var m = new MineTreeDto()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        ParentId = Guid.Empty,
                        Type = 1,
                        Field = a.Path ?? a.Name
                    };
                    BuildMineTree(data, m, a.Id);
                    result.Add(m);
                });
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取静态文件夹数据
        /// </summary>
        /// <param name="staticKey"></param>
        /// <returns></returns>
        public async Task<List<MineTreeDto>> GetStaticTreeList(string staticKey)
        {
            var result = new List<MineTreeDto>();
            var data = FileManager
                .GetFolderList(ResourceQueryType.Publish, a => a.StaticKey == staticKey && !a.IsDeleted)
                .ToList();
            if (!data.Any())
            {
                return await Task.FromResult(result);
            }

            {
                var topNodes = data.Where(a => a.ParentId == null).ToList();
                topNodes?.ForEach(a =>
                {
                    var m = new MineTreeDto()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        ParentId = Guid.Empty,
                        Type = 1,
                        Field = a.Path ?? a.Name
                    };
                    BuildMineTree(data, m, a.Id);
                    result.Add(m);
                });
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 获取资源的具体权限，用于反向绑定
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<ResourcePermissionDto> GetResourcePermission(ResourcePermissionInputDto input)
        {
            switch (input.Type)
            {
                case ResourceType.File:
                    Expression<Func<FileRltPermissions, bool>> where = a => a.FileId == input.Id;
                    var fileQuery = FileManager.GetList(where);
                    var resourceDto1 = new ResourcePermissionDto();
                    // TODO 需要注意，每一条成员记录都包含一个权限，所以需要在返回的对象中进行并集处理
                    resourceDto1.Users = fileQuery.Where(a => a.Type == MemberType.User).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.Rolers = fileQuery.Where(a => a.Type == MemberType.Role).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.Organizations = fileQuery.Where(a => a.Type == MemberType.Organization).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.View = fileQuery.Select(a => a.View).Count().Equals(fileQuery.Count(a => a.View)); // 有一个不满足就是false
                    resourceDto1.Edit = fileQuery.Select(a => a.Edit).Count().Equals(fileQuery.Count(a => a.Edit)); // 有一个不满足就是false
                    resourceDto1.Delete = fileQuery.Select(a => a.Delete).Count().Equals(fileQuery.Count(a => a.Delete)); // 有一个不满足就是false
                    resourceDto1.Use = fileQuery.Select(a => a.Use).Count().Equals(fileQuery.Count(a => a.Use)); // 有一个不满足就是false
                    return Task.FromResult(resourceDto1);
                case ResourceType.Folder:
                    Expression<Func<FolderRltPermissions, bool>> folderWhere = a => a.FolderId == input.Id;
                    var folderQuery = FileManager.GetList(folderWhere);
                    var resourceDto2 = new ResourcePermissionDto();
                    // TODO 需要注意，每一条成员记录都包含一个权限，所以需要在返回的对象中进行并集处理
                    resourceDto2.Users = folderQuery.Where(a => a.Type == MemberType.User).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto2.Rolers = folderQuery.Where(a => a.Type == MemberType.Role).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto2.Organizations = folderQuery.Where(a => a.Type == MemberType.Organization).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto2.View = folderQuery.Select(a => a.View).Count().Equals(folderQuery.Count(a => a.View)); // 有一个不满足就是false
                    resourceDto2.Edit = folderQuery.Select(a => a.Edit).Count().Equals(folderQuery.Count(a => a.Edit)); // 有一个不满足就是false
                    resourceDto2.Delete = folderQuery.Select(a => a.Delete).Count().Equals(folderQuery.Count(a => a.Delete)); // 有一个不满足就是false
                    resourceDto2.Use = folderQuery.Select(a => a.Use).Count().Equals(folderQuery.Count(a => a.Use)); // 有一个不满足就是false
                    return Task.FromResult(resourceDto2);
                default:
                    throw new UserFriendlyException("查询条件有误");
            }
        }

        /// <summary>
        /// 获取共享资源的权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResourcePermissionDto> GetResourceShare(ResourcePermissionInputDto input)
        {
            switch (input.Type)
            {
                case ResourceType.File:
                    Expression<Func<FileRltPermissions, bool>> where = a => a.FileId == input.Id;
                    var fileQuery = FileManager.GetList(where);
                    var resourceDto1 = new ResourcePermissionDto();
                    // TODO 需要注意，每一条成员记录都包含一个权限，所以需要在返回的对象中进行并集处理
                    resourceDto1.Users = fileQuery.Where(a => a.Type == MemberType.User).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.Rolers = fileQuery.Where(a => a.Type == MemberType.Role).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.Organizations = fileQuery.Where(a => a.Type == MemberType.Organization).Select(a => a.MemberId.Value).ToGuidArray();
                    resourceDto1.View = fileQuery.Select(a => a.View).Count().Equals(fileQuery.Count(a => a.View)); // 有一个不满足就是false
                    resourceDto1.Edit = fileQuery.Select(a => a.Edit).Count().Equals(fileQuery.Count(a => a.Edit)); // 有一个不满足就是false
                    resourceDto1.Delete = fileQuery.Select(a => a.Delete).Count().Equals(fileQuery.Count(a => a.Delete)); // 有一个不满足就是false
                    resourceDto1.Use = fileQuery.Select(a => a.Use).Count().Equals(fileQuery.Count(a => a.Use)); // 有一个不满足就是false
                    return resourceDto1;
                case ResourceType.Folder:
                    // 获取当前指定的文件夹
                    var folder = FileManager.GetFolder(input.Id).Result;
                    // 获取当前的成员信息，然后过滤
                    var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.GetValueOrDefault());
                    var result = new ResourcePermissionDto();
                    if (folder == null || !folder.Shares.Any())
                    {
                        return result;
                    }
                    result.Users = folder.Shares
                        .Where(a => a.Type == MemberType.User)
                        .Select(a => a.MemberId.GetValueOrDefault())
                        .ToGuidArray();
                    result.Organizations = folder.Shares?
                        .Where(a => a.Type == MemberType.Organization)
                        .Select(a => a.MemberId.GetValueOrDefault())
                        .ToGuidArray();
                    result.Rolers = folder.Shares?
                        .Where(a => a.Type == MemberType.Role)
                        .Select(a => a.MemberId.GetValueOrDefault())
                        .ToGuidArray();

                    // 获取权限
                    var datas = folder.Shares
                        .Where(a => members.Select(b => b.Id).Contains(a.MemberId.GetValueOrDefault()))
                        .ToList();
                    result.Edit = !datas.Any() || (datas.Any() && !datas.Select(a => a.Edit).Contains(false));
                    result.View = !datas.Any() || (datas.Any() && !datas.Select(a => a.View).Contains(false));
                    result.Delete = !datas.Any() || (datas.Any() && !datas.Select(a => a.Delete).Contains(false));
                    result.Use = !datas.Any() || (datas.Any() && !datas.Select(a => a.Use).Contains(false));
                    return result;
                default:
                    throw new UserFriendlyException("查询条件有误");
            }
        }

        /// <summary>
        /// 批量设置资源标签信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<bool> CreateResourceTag(ResourceTagCreateDto input)
        {
            if (input.Resources == null)
            {
                throw new UserFriendlyException("资源信息不能为空");
            }

            if (input.TagIds == null)
            {
                throw new UserFriendlyException("标签信息不能为空");
            }

            var files = input.Resources
                .Where(a => a.Type == ResourceType.File)
                .ToList();
            var folders = input.Resources
                .Where(a => a.Type == ResourceType.Folder)
                .ToList();

            var fileRltTags = new List<FileRltTag>();
            var folderRltTags = new List<FolderRltTag>();
            files?.ForEach(a =>
            {
                input.TagIds?.ForEach(b =>
                {
                    var model = new FileRltTag(_generator.Create()) { FileId = a.Id, TagId = b };
                    fileRltTags.Add(model);
                });
            });

            folders?.ForEach(a =>
            {
                input.TagIds?.ForEach(b =>
                {
                    var model = new FolderRltTag(_generator.Create()) { FolderId = a.Id, TagId = b };
                    folderRltTags.Add(model);
                });
            });

            if (fileRltTags.Any())
                FileManager.CreateResourceRltTag(fileRltTags);
            if (folderRltTags.Any())
                FileManager.CreateResourceRltTag(folderRltTags);
            return Task.FromResult(true);

        }

        /// <summary>
        /// 文件移动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CreateFileMove(FileOperationDto input)
        {
            Guid? folderId = null;
            Guid? organizationId = null;


            switch (input.TargetType)
            {
                case ResourceNodeType.Folder:
                    folderId = input.TargetId;
                    break;
                case ResourceNodeType.Organization:
                    organizationId = input.TargetId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (input.Type)
            {
                case ResourceType.File:
                    {
                        // 移动文件
                        var file = await FileManager.GetFile(input.Id);
                        file.FolderId = folderId;
                        file.OrganizationId = (await FileManager.GetFolder(a => a.Id == folderId))?.OrganizationId;
                        if (file.FolderId != null)
                        {
                            file.Path = (await FileManager.GetFolder(a => a.Id == folderId))?.Path;
                        }
                        return await FileManager.Update(file);
                    }
                case ResourceType.Folder:
                    {
                        // 移动文件夹,处理
                        var folder = await FileManager.GetFolder(a => a.Id == input.Id);
                        folder.ParentId = folderId;
                        if (folderId != null)
                        {
                            // 文件路径为目标路径+当前文件夹名称
                            folder.Path = (await FileManager.GetFolder(a => a.Id == folderId))?.Path +
                                          $"\\{folder.Name}";
                        }
                        if (organizationId != null)
                        {
                            folder.OrganizationId = organizationId;
                            folder.ParentId = null;
                        }
                        return await FileManager.Update(folder);
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// 文件复制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CreateFileCopy(FileOperationDto input)
        {
            /* 资源复制逻辑
             *1、文件复制，判断指定目标路径是否在当前文件夹，在则文件名为：‘文件名_复制(num)’,其他信息不变
             *   不在当前文件夹，也需要判断该文件夹中有没有重命名的，无则需要目标id及组织id，目标时文件夹，需要根据文件夹得到组织结构的id
             *2、文件夹复制，在当前组织结构目录下，只需要重命名当前文件夹名称，文件夹中的资源不需要重新命名，只需重新添加一遍
             *   不在当前组织结构目录下，也需要判断该机构下有没有相同名字的文件夹，有相同名字，则进行合并
             */

            Guid? organizationId = null;
            Guid? folderId = null;

            switch (input.TargetType)
            {
                case ResourceNodeType.Folder:
                    folderId = input.TargetId;
                    break;
                case ResourceNodeType.Organization:
                    organizationId = input.TargetId;
                    break;
                default:
                    throw new UserFriendlyException("条件异常");
            }

            switch (input.Type)
            {
                case ResourceType.File:
                    {
                        //复制文件
                        var file = await FileManager.GetFile(input.Id);
                        // 查询目标文件夹是否在存在

                        // 判断在目标目录下是否存在同名同类型的文件
                        if (file != null)
                        {
                            // 文件复制过程中需要考虑复制的是共享中心中的数据，需要将复制后的文件共享状态还原。
                            var newFile = new Entities.File();
                            ObjectMapper.Map(file, newFile);
                            newFile.SetId(_generator.Create());
                            newFile.Name = GetNewFileName(file.Name, file.Type, input.TargetType, input.TargetId);
                            switch (input.TargetType)
                            {
                                case ResourceNodeType.Folder:
                                    {
                                        var folder = await FileManager.GetFolder(input.TargetId);
                                        newFile.OrganizationId = organizationId ?? folder.OrganizationId;
                                        newFile.Folder = folder;
                                        newFile.Path = folder.Path;
                                        break;
                                    }
                                case ResourceNodeType.Organization:
                                    newFile.OrganizationId = input.TargetId;
                                    newFile.Folder = null;
                                    newFile.Path = null;
                                    break;
                                default:
                                    throw new UserFriendlyException("条件异常");
                            }
                            // 同时创建一个文件版本
                            var ossServer = _ossResp.OssServer;
                            var fileVersion = new FileVersion(_generator.Create())
                            {
                                Oss = ossServer,
                                File = newFile,
                                OssId = ossServer?.Id ?? Guid.Empty,
                                OssUrl = newFile.Url, // 记录最新文件的url
                                FileId = newFile.Id,
                                Version = 1,
                                Size = newFile.Size
                            };
                            newFile.Versions = null;
                            newFile.IsShare = false;
                            await _fileVersionResp.InsertAsync(fileVersion);
                            await FileManager.Insert(newFile);
                            return true;
                        }
                        break;
                    }
                case ResourceType.Folder:
                    {
                        // 复制文件夹
                        var folder = await FileManager.GetFolder(input.Id);
                        if (folder != null)
                        {
                            // 复制到指定组织机构
                            if (organizationId != null)
                            {
                                return CopyToRoot(organizationId.GetValueOrDefault(), folder);
                            }
                            // 复制都指定的文件夹
                            if (folderId != Guid.Empty)
                            {
                                return CopyToFolder(input.TargetId, folder);
                            }
                        }

                        break;
                    }
                default:
                    throw new UserFriendlyException("处理异常");
            }

            return false;
        }

        private bool CopyToFolder(Guid targetFolderId, Folder folder)
        {
            // 获取目标的文件夹
            var targetFolder = FileManager.GetFolder(a => a.Id == targetFolderId).Result;
            var targetOragniaztionId = targetFolder?.OrganizationId;

            // 获取目标文件夹下的文件夹，看有无重名的情况
            var folders = FileManager.GetFolderList(ResourceQueryType.NotDelete,
                a => a.ParentId == targetFolderId && a.Name == folder.Name).ToList();
            if (folders.Any())
            {
                // 有重名,则合并子文件
                SaveMergeNewFolder(folder, targetOragniaztionId.GetValueOrDefault(), targetFolderId);
                return true;
            }
            else
            {
                SaveMergeNewFolder(folder, targetOragniaztionId, targetFolderId);
                return true;
            }
        }

        /// <summary>
        /// 将文件复制到根节点（组织结构）
        /// </summary>
        /// <returns></returns>
        private bool CopyToRoot(Guid rootId, Folder currentFolder)
        {
            if (currentFolder == null)
            {
                throw new UserFriendlyException("未查询到指定文件夹");
            }

            if (currentFolder.OrganizationId != rootId)
            {
                // 二者不在同一个机构
                //获取该节点下的文件夹，判断有无重名的
                var folders =
                    FileManager.GetFolderList(ResourceQueryType.NotDelete, a => a.OrganizationId == rootId).ToList();
                if (folders.Any(a => a.Name == currentFolder.Name))
                {
                    var target = folders.FirstOrDefault(a => a.Name == currentFolder.Name);
                    // 存在相同的文件夹名，对子内容进行合并（合并的同时还需递归判断子文件中有无重复的文件夹，再继续合并文件，直到最后一个文件夹）
                    SaveMergeNewFolder(currentFolder, target.OrganizationId.GetValueOrDefault(), target.Id);
                    return true;
                }
                else
                {
                    // 顶级不存在同名文件夹，直接进行复制，同时子级文件夹需要判断是否有重名
                    SaveMergeNewFolder(currentFolder, rootId, null);
                    return true;
                }
            }
            else
            {
                //要复制的对象在同一个组织机构下，直接对提示报错
                throw new UserFriendlyException("不能将文件夹复制到同级目录!");
            }
        }

        /// <summary>
        /// 保存合并新的文件及文件夹
        /// </summary>
        /// <param name="source"></param>
        /// <param name="organizationId"></param>
        /// <param name="tid"></param>
        private void SaveMergeNewFolder(Folder source, Guid? organizationId, Guid? tid)
        {
            //var newFiles = new List<File>();//需要复制的新文件
            var newFolders = new List<Folder>();// 需要复制的新文件夹

            //获取原文件夹下的所有文件及文件夹，用于递归使用，减少数据库查询次数
            var allFolders = FileManager
                .GetFolderList(ResourceQueryType.NotDelete, a => a.OrganizationId == source.OrganizationId).ToList();

            // 需要注意： 复制文件的同时i，还需要将已复制文件的版本信息同时复制一份，只保证版本1即可。

            // 需要注意： 对从共享中心复制来的文件夹，需要将其的权限全部置空，包括其子文件夹及文件
            var newFolder = new Folder();
            //newFolder.Files = source.Files;
            newFolder.Tags = source.Tags;
            newFolder.IsPublic = true;
            newFolder.StaticKey = source.StaticKey;
            newFolder.Name = source.Name;
            //ObjectMapper.Map(source, newFolder);
            newFolder.SetId(_generator.Create());
            newFolder.OrganizationId = organizationId;
            newFolder.ParentId = tid;
            // 处理复制文件的路径问题
            newFolder.Path = tid != null ? $"{(FileManager.GetFolder(a => a.Id == tid).Result.Path)}\\{newFolder.Name}" : string.Empty;

            newFolder.Shares = null;
            newFolder.Permissions = null;

            var _ = _folderResp.InsertAsync(newFolder).Result;
            source.Files?.ForEach(a =>
            {
                a.SetId(_generator.Create());
                a.FolderId = newFolder.Id;
                a.OrganizationId = organizationId;
                a.Path = newFolder.Path;
                if (a.Versions != null)
                {
                    // 同步处理文件版本信息
                    a.Versions = a.Versions.Where(b => b.Version == 1).ToList();
                    a.Versions.FirstOrDefault()?.SetId(_generator.Create());

                    a.Versions = a.Versions.Where(c => c.Version == 1).ToList();
                    var fileVersion = a.Versions.FirstOrDefault();
                    if (fileVersion != null)
                    {
                        fileVersion.SetId(_generator.Create());
                        var copyVersion = _fileVersionResp.InsertAsync(fileVersion);
                    }
                    a.Versions = null;
                }

                a.Shares = null;
                a.Permissions = null;
                var _ = FileManager.Insert(a);
            });
            //UnitOfWorkManager.Current.SaveChangesAsync().Wait();
            newFolders.Add(newFolder);

            // 获取要复制的文件夹
            allFolders?.ForEach(a =>
            {
                if (a.ParentId != source.Id)
                {
                    return;
                }

                // 获取到子文件夹
                var folder = new Folder(_generator.Create())
                {
                    Name = a.Name,
                    IsPublic = a.IsPublic,
                    OrganizationId = organizationId,
                    ParentId = tid,
                    Path = $"{(FileManager.GetFolder(b => b.Id == tid).Result.Path)}\\{a.Name}",
                    Shares = null,
                    Permissions = null,
                    Files = new List<Entities.File>(),
                };
                // 处理子文件
                a.Files?.ForEach(b =>
                {
                    b.SetId(_generator.Create());
                    b.FolderId = folder.Id;
                    b.OrganizationId = organizationId;
                    b.Path = folder.Path;
                    if (b.Versions != null)
                    {
                        // 同步处理文件版本信息
                        b.Versions = b.Versions.Where(c => c.Version == 1).ToList();
                        var fileVersion = b.Versions.FirstOrDefault();
                        if (fileVersion != null)
                        {
                            fileVersion.SetId(_generator.Create());
                            var coypVerdion = _fileVersionResp.InsertAsync(fileVersion);
                        }
                        b.Versions = null;
                    }
                    b.Shares = null;
                    b.Permissions = null;

                    var _ = FileManager.Insert(b);
                    //folder.Files.Add(b);
                });
                var _ = _folderResp.InsertAsync(folder).Result;
                //UnitOfWorkManager.Current.SaveChangesAsync().Wait();
                // 处理子文件夹
                ResolveFolderChildren(allFolders, a.Id, folder, newFolders);

                newFolders.Add(folder);
            });

            // 批量添加
            if (newFolders.Any())
            {
                //FileManager.Insert(newFolders);
            }
        }

        private void ResolveFolderChildren(List<Folder> folders, Guid pId, Folder target, List<Folder> newFolders)
        {
            var path = $"{(FileManager.GetFolder(b => b.Id == target.Id).Result?.Path)}";
            folders?.ForEach(a =>
            {
                if (a.ParentId != pId)
                {
                    return;
                }

                var folder = new Folder(_generator.Create())
                {
                    Name = a.Name,
                    OrganizationId = target.OrganizationId,
                    ParentId = target.Id,
                    Path = $"{path}\\{a.Name}",
                    Shares = null,
                    Permissions = null
                };
                // 处理子文件
                a.Files?.ForEach(b =>
                {
                    b.SetId(_generator.Create());
                    b.FolderId = folder.Id;
                    b.OrganizationId = target.OrganizationId;
                    b.Path = folder.Path;
                    if (b.Versions != null)
                    {
                        // 同步处理文件版本信息
                        b.Versions = b.Versions.Where(c => c.Version == 1).ToList();
                        b.Versions.FirstOrDefault()?.SetId(_generator.Create());
                    }

                    b.Shares = null;
                    b.Permissions = null;
                    folder.Files.Add(b);
                });
                var _ = _folderResp.InsertAsync(folder).Result;
                UnitOfWorkManager.Current.SaveChangesAsync().Wait();
                // 处理子文件夹
                ResolveFolderChildren(folders, a.Id, folder, newFolders);
                //newFolders.Add(folder);
            });
        }

        /// <summary>
        /// 获取新的文件的名称
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="suffix">文件后缀</param>
        /// <param name="type">目标路径类型</param>
        /// <param name="tId">目标路径id</param>
        /// <returns></returns>
        private string GetNewFileName(string filename, string suffix, ResourceNodeType type, Guid tId)
        {
            switch (type)
            {
                case ResourceNodeType.Folder:
                    {
                        var queryFiles = FileManager.GetFileList(a => !a.IsHidden && !a.IsDeleted && a.FolderId == tId);
                        var sameFiles = queryFiles.Where(a => a.Name.StartsWith(filename) && a.Type == suffix).ToList();
                        return sameFiles.Any() ? GetFileName(sameFiles) : filename;
                    }
                case ResourceNodeType.Organization:
                    {
                        var queryFiles = FileManager.GetFileList(a => !a.IsHidden && !a.IsDeleted && a.FolderId == null && a.OrganizationId == tId);
                        var sameFiles = queryFiles.Where(a => a.Name.StartsWith(filename) && a.Type == suffix).ToList();
                        return sameFiles.Any() ? GetFileName(sameFiles) : filename;
                    }
                default:
                    throw new UserFriendlyException("条件异常");
            }
        }
        /// <summary>
        ///根据条件获取文件名称
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string GetFileName(IReadOnlyCollection<Entities.File> list)
        {
            if (list.Count == 1)
            {
                return list.FirstOrDefault()?.Name + "_复制";
            }
            else
            {
                var lastFileName = list.OrderByDescending(a => a.Name).FirstOrDefault()?.Name;
                return (lastFileName.Substring(0, lastFileName.LastIndexOf('_')) +
                        int.Parse(lastFileName.Substring(lastFileName.LastIndexOf('_'))) + 1) ?? "";
            }
        }

        /// <summary>
        /// 资源还原
        /// </summary>
        /// <param name="input">输入的资源数据</param>
        /// <returns></returns>
        public async Task<bool> Restore(ResourceRestoreDto input)
        {
            List<Folder> folders = null;
            List<Entities.File> files = null;

            if (input.TargetId == Guid.Empty)
            {
                // 恢复到之前的路径，即只修改isdelete的状态
                // 还原指定的文件夹，需要判断有无删除的子文件夹，如果有子文件夹，就需要将子文件夹也同时还原

                if (input.FolderIds != null)
                {
                    await _customRespository.RestoreFolder(input.FolderIds);
                }
                if (input.FileIds != null)
                {
                    await _customRespository.RestoreFile(input.FileIds);
                }
            }
            else
            {
                using (_dataFilter.Disable<ISoftDelete>())
                {
                    // 获取已经删除的文件夹对象
                    folders = FileManager.GetFolderList(ResourceQueryType.Deleted, a => input.FolderIds.Contains(a.Id)).ToList();
                    // 获取已经删除的文件对象
                    files = FileManager.GetFileList(ResourceQueryType.Deleted, a => input.FileIds.Contains(a.Id)).ToList();
                }
                switch (input.TargetType)
                {
                    // 还原文件到指定路径
                    case ResourceNodeType.Folder:
                        {
                            // 还原到指定的文件夹
                            var folder = await FileManager.GetFolder(a => a.Id == input.TargetId);
                            if (folder != null)
                            {
                                folders?.ForEach(a =>
                                {
                                    a.OrganizationId = folder.OrganizationId;
                                    a.ParentId = folder.Id;
                                    a.IsDeleted = false;
                                    a.IsPublic = folder.IsPublic;
                                    a.Path = $"{ folder.Path}\\{a.Name}";
                                    a.Files?.ForEach(b =>
                                    {
                                        b.IsDeleted = false;
                                        b.Path = a.Path;
                                    });
                                    FileManager.Update(a);
                                });

                                files?.ForEach(a =>
                                {
                                    a.OrganizationId = folder.OrganizationId;
                                    a.FolderId = folder.Id;
                                    a.IsDeleted = false;
                                    a.Path = folder.Path;
                                    a.IsPublic = folder.IsPublic;
                                    FileManager.Update(a);
                                });
                            }

                            break;
                        }
                    case ResourceNodeType.Organization:
                        // 还原到指定的组织
                        folders?.ForEach(a =>
                        {
                            a.OrganizationId = input.TargetId;
                            a.ParentId = null;
                            a.Path = string.Empty;
                            a.IsDeleted = false;
                            FileManager.Update(a);
                        });

                        files?.ForEach(a =>
                        {
                            a.OrganizationId = input.TargetId;
                            a.FolderId = null;
                            a.IsDeleted = false;
                            a.Path = string.Empty;
                            FileManager.Update(a);
                        });
                        break;
                    default:
                        throw new UserFriendlyException("数据还原失败！");
                }
            }
            return true;
        }

        /// <summary>
        /// 设置资源的权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<bool> SetResourcePermission(ResourcePermissionCreateDto input)
        {
            // TODO 保存逻辑，每次保存都需要将上一次的记录删除掉，数据前后做对比
            switch (input.Type)
            {
                case
                    ResourceType.File:
                    var fileRlts = BuildFileRlt(input);
                    return fileRlts.Any() ? FileManager.Insert(fileRlts) : Task.FromResult(true);
                case
                    // TODO 文件夹的权限需要考虑子文件夹及包含的文件的问题
                    ResourceType.Folder:
                    var folderRlts = BuildFolderRlt(input);
                    return folderRlts.Any() ? FileManager.Insert(folderRlts) : Task.FromResult(true);
                default:
                    throw new UserFriendlyException("权限配置数据有误!");
            }
        }

        /// <summary>
        /// 设置资源共享
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> SetResourceShare(ResourcePermissionCreateDto input)
        {
            // 文件共享，需要改变share 的状态
            // 当某一个文件的成员都为空时，表示该文件不被分享了，需要修改状态
            switch (input.Type)
            {
                case
                    ResourceType.File:

                    var file = BuildFileShareRlt(input);
                    var currentFile = await FileManager.GetFile(input.Id);
                    if (!file.Any())
                    {
                        currentFile.IsShare = false;
                        await FileManager.Delete<FileRltShare>(a => a.FileId == input.Id);
                    }
                    else
                    {
                        currentFile.IsShare = true;
                        // 添加之前先删除
                        await FileManager.Insert(file); // 更新权限关联
                    }
                    await FileManager.Update(currentFile); // 更新当前的文件
                    return true;
                case
                    // TODO 文件夹的权限需要考虑子文件夹及包含的文件的问题,是否也需要共享
                    ResourceType.Folder:
                    var folderRlts = BuildFolderRltShare(input);
                    var folder = await FileManager.GetFolder(input.Id);
                    if (!folderRlts.Any())
                    {
                        folder.IsShare = false;
                        //删除之前的分享信息
                        await FileManager.Delete<FolderRltShare>(a => a.FolderId == input.Id);
                    }
                    else
                    {
                        folder.IsShare = true;
                        await FileManager.Insert(folderRlts);
                    }
                    //设置子集文件及文件夹
                    //await SetFloderShareAsync(input);
                    await FileManager.Update(folder);
                    return true;
                default:
                    throw new UserFriendlyException("权限配置数据有误!");
            }
        }

        /// <summary>
        /// 发布资源到指定位置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> PublishResource(ResourceRestoreDto input)
        {
            if ((input.FileIds == null && input.FolderIds == null) || input.TargetId == null)
            {
                throw new UserFriendlyException("参数有误");
            }

            if (input.FileIds?.Length > 0)
            {
                await this.PublishFile(input.FileIds, input);
            }

            if (input.FolderIds?.Length > 0)
            {
                await this.PublishFolder(input.FolderIds, input);
            }
            return true;
        }
        /// <summary>
        /// 发布文件到指定位置
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="input"></param>
        /// <returns></returns>

        private async Task PublishFile(Guid[] ids, ResourceRestoreDto input)
        {
            var files = FileManager.GetFileList(a => ids.Contains(a.Id)).ToList();
            GetParam(input, out var organizationId, out var folderId);

            foreach (var file in files)
            {
                // 修改文件的状态
                // 公开字段，计算文件路径
                file.IsPublic = true;
                file.FolderId = folderId;
                file.OrganizationId = organizationId;
                file.Path = await FileManager.GetResourcePath(organizationId, folderId);
            }
            await FileManager.Update(files);
        }
        /// <summary>
        /// 发布文件夹，包含文件夹中的文件也要处理
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task PublishFolder(IEnumerable<Guid> ids, ResourceRestoreDto input)
        {
            var allFolders = new List<Folder>();// 所有的文件夹信息
            var allFiles = new List<Entities.File>();
            var folders = FileManager
                .GetFolderList(ResourceQueryType.UnPublish, a => ids.Contains(a.Id) && a.CreatorId == CurrentUser.Id)
                .ToList();
            GetParam(input, out var organizationId, out var folderId);
            foreach (var folder in folders)
            {
                folder.IsPublic = true;
                folder.OrganizationId = organizationId;
                folder.ParentId = folderId;
                folder.Path = await FileManager.GetResourcePath(organizationId, folder.Id, folder.Name);

                // 处理该文件夹下的子文件
                if (folder.Files.Any())
                {
                    foreach (var folderFile in folder.Files)
                    {
                        folderFile.OrganizationId = organizationId;
                        folderFile.IsPublic = true;
                        folderFile.Path = $"{folder.Path}\\{folderFile.Name}";
                        allFiles.Add(folderFile);
                    }
                }

                // 获取子文件夹
                await GetPublishFolderChildren(allFolders, allFiles, folder);
                allFolders.Add(folder);
            }

            await FileManager.Update(allFolders);
            await FileManager.Update(allFiles);
        }

        /// <summary>
        /// 获取需要发布的子文件夹
        /// </summary>
        /// <param name="foldeList"></param>
        /// <param name="fileList"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private async Task GetPublishFolderChildren(ICollection<Folder> foldeList, ICollection<Entities.File> fileList, Folder folder)
        {
            // 获取是否有子文件夹
            var children = FileManager
                .GetFolderList(ResourceQueryType.UnPublish, a => a.ParentId == folder.Id)
                .ToList();
            if (children.Any())
            {
                foreach (var item in children)
                {
                    item.IsPublic = true;
                    item.OrganizationId = folder.OrganizationId;
                    item.ParentId = folder.Id;
                    item.Path = $"{folder.Path}\\{item.Name}";

                    if (item.Files.Any())
                    {
                        foreach (var folderFile in folder.Files)
                        {
                            folderFile.OrganizationId = item.OrganizationId;
                            folderFile.IsPublic = true;
                            folderFile.Path = $"{folder.Path}\\{item.Name}";
                            fileList.Add(folderFile);
                        }
                    }
                    await GetPublishFolderChildren(foldeList, fileList, item);
                    foldeList.Add(item);
                }
            }
        }

        /// <summary>
        /// 获取目标参数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="organizationId"></param>
        /// <param name="folderId"></param>
        private void GetParam(ResourceRestoreDto input, out Guid? organizationId, out Guid? folderId)
        {
            switch (input.TargetType)
            {
                case ResourceNodeType.Folder:
                    var folder = FileManager.GetFolder(input.TargetId).Result; // 目标文件夹
                    organizationId = folder?.OrganizationId;
                    folderId = input.TargetId;
                    break;
                case ResourceNodeType.Organization:
                    organizationId = input.TargetId;
                    folderId = null;
                    break;
                default:
                    throw new UserFriendlyException("目标类型有误");
            }
        }


        /// <summary>
        /// 清空回收站
        /// </summary>
        /// <returns></returns>
        //[Authorize(FilePermissions.FileManager.Delete)]
        public async Task<bool> DeleteAll()
        {
            await FileManager.Clear();
            return true;
        }
        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(FilePermissions.FileManager.Delete)]
        public async Task<bool> Delete(ResourceDeleteDto input)
        {
            if (input.IsTure)
            {
                //真实删除
                await FileManager.Delete(input.Files, input.Folders, true);
            }
            else
            {
                // 软删除
                await FileManager.Delete(input.Files, input.Folders);
            }
            return true;
        }


        #region 私有方法

        #region 组织结构树私有方法
        /// <summary>
        /// 构造组织树结构
        /// </summary>
        /// <param name="organizations">组织结构</param>
        /// <param name="folders">隶属组织结构的文件夹</param>
        /// <param name="shareUse">共享中心调用</param>
        /// <returns></returns>
        private List<OrganizationTreeDto> BuildOrganizationTree(List<Organization> organizations, IReadOnlyCollection<Folder> folders, bool shareUse = false)
        {
            var tree = CodeTreeHelper<Organization>.GetTree(organizations, 4);

            var list = new List<OrganizationTreeDto>();
            list = ObjectMapper.Map<List<Organization>, List<OrganizationTreeDto>>(tree);

            if (list.Any())
            {
                // 该节点添加文件夹
                list?.ForEach(a =>
                {
                    a.Type = ResourceNodeType.Organization;
                    GetFolder(folders, a);
                    if (a.Children.Any())
                    {
                        SetChildrenFolder(a.Children, folders);
                    }
                });
            }
            return list;
        }

        /// <summary>
        /// 给组织机构节点挂在文件夹
        /// </summary>
        /// <param name="organizations"></param>
        /// <param name="folders"></param>
        private void SetChildrenFolder(List<OrganizationTreeDto> organizations, IReadOnlyCollection<Folder> folders)
        {
            organizations?.ForEach(a =>
            {
                GetFolder(folders, a);
                if (a.Children.Any())
                {
                    SetChildrenFolder(a.Children, folders);
                }
            });
        }


        /// <summary>
        /// 获取组织节点下的文件夹
        /// </summary>
        /// <param name="folders">文件夹集合</param>
        /// <param name="dto"></param>
        private static void GetFolder(IEnumerable<Folder> folders, OrganizationTreeDto dto)
        {
            if (folders == null)
            {
                return;
            }

            var list = folders.Where(a => a.OrganizationId == dto.Id && a.ParentId == null)
                .ToList();
            list?.ForEach(a =>
            {
                var m = new OrganizationTreeDto
                {
                    Name = a.Name,
                    Id = a.Id,
                    ParentId = Guid.Empty,
                    Type = ResourceNodeType.Folder,
                    IsShare = a.IsShare,
                    Field = a.Path ?? a.Name
                };
                GetFolderChildren(folders, m);
                dto.Children.Add(m);
            });
        }

        /// <summary>
        /// 获取文件夹子目录
        /// </summary>
        /// <param name="folders">文件夹集合</param>
        /// <param name="dto">条件</param>
        private static void GetFolderChildren(IEnumerable<Folder> folders, OrganizationTreeDto dto)
        {
            dto.Children = new List<OrganizationTreeDto>();
            var enumerable = folders as Folder[] ?? folders.ToArray();
            enumerable.ToList()?.ForEach(a =>
            {
                if (a.ParentId != dto.Id)
                {
                    return;
                }
                var m = new OrganizationTreeDto
                {
                    Name = a.Name,
                    Id = a.Id,
                    ParentId = dto.Id,
                    Type = ResourceNodeType.Folder,
                    IsShare = a.IsShare,
                    Field = a.Path
                };
                dto.Children.Add(m);
                GetFolderChildren(enumerable, m);
            });
        }

        #endregion

        #region 资源信息私有方法

        /// <summary>
        /// 绑定资源数据
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="files"></param>
        /// <param name="allFiles"></param>
        /// <returns></returns>
        private IReadOnlyList<ResourceDto> GetResourceList(List<Folder> folders, List<Entities.File> files, List<Entities.File> allFiles)
        {
            // 注意： 文件的权限信息和共享的信息中，成员id只获取到当前用户所在的成员id，然后再返回。
            var members = _identityUserManager.GetUserMembers(CurrentUser.Id.GetValueOrDefault())
                .Result
                .Select(a => a.Id)
                .ToList();
            var list = new List<ResourceDto>();
            folders?.ForEach(a =>
            {
                var dto = new ResourceDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = "文件夹",
                    EditTime = a.LastModificationTime ?? a.CreationTime,
                    ResourceType = ResourceType.Folder,
                    IsShare = a.IsShare,
                    Tags = a.Tags?.Select(a => a.Tag).ToList(),
                    FolderPermissions = a.Permissions?.Where(b => members.Contains(b.MemberId.GetValueOrDefault())).ToList(),
                    FolderShares = a.Shares?.Where(b => members.Contains(b.MemberId.GetValueOrDefault())).ToList()
                };
                list.Add(dto);
            });

            files?.ForEach(a =>
            {
                // 需要注意，如果此文件的有父文件夹，且父文件夹被分享了，需要获取其父组件的分享权限，
                // 如果子文件和父文件夹都有权限，就以子文件的权限为主。
                var dto = new ResourceDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,


                    EditTime = a.LastModificationTime ?? a.CreationTime,
                    ResourceType = ResourceType.File,
                    IsShare = a.IsShare,
                    Tags = a.Tags?.Select(a => a.Tag).ToList(),
                    Versions = a.Versions.OrderBy(b => b.Version).Select(b => new FileVersion(b.Id)
                    {
                        CreationTime = b.CreationTime,
                        Size = b.Size,
                        Version = b.Version,
                        OssUrl = b.OssUrl,
                    }).ToList(),
                    //FileShares = 
                    // FilePermissions =
                    Size = a.Size,
                    Url = a.Url
                };

                // 判断权限
                var fileSharePermissions =
                    a.Shares?.Where(b => members.Contains(b.MemberId.GetValueOrDefault())).ToList();
                var filePermissions = a.Permissions?.Where(b => members.Contains(b.MemberId.GetValueOrDefault()))
                    .ToList();

                if (a.Folder != null && a.Folder.IsShare)
                {
                    if (fileSharePermissions != null && fileSharePermissions.Any())
                    {
                        dto.FileShares = fileSharePermissions;
                    }
                    else
                    {
                        dto.FileShares = new List<FileRltShare>();
                        // 将文件夹的权限给复制进入
                        a.Folder.Shares?.ForEach(b =>
                        {
                            if (!members.Contains(b.MemberId.GetValueOrDefault()))
                            {
                                return;
                            }

                            var fileP = new FileRltShare();
                            fileP.SetId(b.Id);
                            fileP.Type = b.Type;
                            fileP.View = b.View;
                            fileP.Delete = b.Delete;
                            fileP.MemberId = b.MemberId;
                            fileP.Edit = b.Edit;
                            fileP.Use = b.Use;
                            dto.FileShares.Add(fileP);
                        });
                    }

                    if (filePermissions != null && filePermissions.Any())
                    {
                        dto.FilePermissions = filePermissions;
                    }
                    else
                    {
                        dto.FilePermissions = new List<FileRltPermissions>();
                        // 将文件夹的权限给复制进入
                        a.Folder.Permissions?.ForEach(b =>
                        {
                            if (!members.Contains(b.MemberId.GetValueOrDefault()))
                            {
                                return;
                            }
                            var fileP = new FileRltPermissions();
                            fileP.SetId(b.Id);
                            fileP.Type = b.Type;
                            fileP.View = b.View;
                            fileP.Delete = b.Delete;
                            fileP.MemberId = b.MemberId;
                            fileP.Edit = b.Edit;
                            fileP.Use = b.Use;
                            dto.FilePermissions.Add(fileP);
                        });
                    }
                }
                else
                {
                    dto.FileShares = fileSharePermissions;
                    dto.FilePermissions = filePermissions;
                }
                list.Add(dto);
            });

            return list;
        }

        #endregion

        /// <summary>
        /// 文件资源搜索
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="page">分页数</param>
        /// <param name="size">每页数量</param>
        /// <returns></returns>
        private List<ResourceDto> GetResource(IQueryable<Entities.File> query, int page, int size)
        {
            var list = query.Skip(page).Take(size).ToList();
            var dtoList = new List<ResourceDto>();
            list.ForEach(a =>
            {
                var dto = new ResourceDto()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Type = a.Type,
                    EditTime = a.LastModificationTime ?? a.CreationTime,
                    ResourceType = ResourceType.File,
                    IsShare = a.IsShare,
                    Tags = a.Tags?.Select(a => a.Tag).ToList(),
                    Versions = a.Versions.OrderBy(b => b.Version).ToList(),
                    FileShares = a.Shares,
                    FilePermissions = a.Permissions,
                    Size = a.Size,
                    Url = a.Url
                };
                dtoList.Add(dto);
            });
            return dtoList;
        }

        /// <summary>
        /// 构造文件权限实体对象
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="mType"></param>
        /// <param name="mId"></param>
        /// <returns></returns>

        private FolderRltPermissions CreateFolderRltPermissions(Guid fileId, Guid mId)
        {
            return new FolderRltPermissions(_generator.Create())
            {
                FolderId = fileId,
                MemberId = mId
            };
        }

        private FolderRltShare CreateFolderRltShare(Guid fileId, Guid mId)
        {
            return new FolderRltShare()
            {
                FolderId = fileId,
                MemberId = mId
            };
        }

        private FileRltShare CreateFileRltShare(Guid fileId, MemberType mType, Guid mId)
        {
            return new FileRltShare()
            {
                FileId = fileId,
                Type = mType,
                MemberId = mId
            };
        }
        private FileRltPermissions CreateFileRltPermissions(Guid fileId, MemberType mType, Guid mId)
        {
            return new FileRltPermissions()
            {
                FileId = fileId,
                Type = mType,
                MemberId = mId
            };
        }
        /// <summary>
        /// 构造“我的目录下的文件夹”
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        private static void BuildMineTree(List<Folder> list, MineTreeDto dto, Guid id)
        {
            try
            {
                dto.Children = new List<MineTreeDto>();
                list?.ForEach(a =>
                {
                    if (a.ParentId != id)
                    {
                        return;
                    }

                    var m = new MineTreeDto()
                    {
                        Name = a.Name,
                        Id = a.Id,
                        ParentId = Guid.Empty,
                        Field = a.Path ?? a.Name,
                        Type = 1,
                    };
                    BuildMineTree(list, m, a.Id);
                    dto.Children.Add(m);
                });
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 判断文件层级关系
        /// </summary>
        private static void CheckResource(List<Folder> allFolders, List<Entities.File> allFiles, out List<Folder> folders, out List<Entities.File> files)
        {
            var pageResult = new PagedResultDto<ResourceDto>();
            var _folders = new List<Folder>();
            var _files = new List<Entities.File>();

            var folderIds = allFolders.Select(a => a.Id);//父文件夹id

            allFolders?.ForEach(a =>
            {
                if (!folderIds.Contains(a.ParentId.GetValueOrDefault()))
                {
                    // 没有父文件夹
                    _folders.Add(a);
                }
            });

            allFiles?.ForEach(a =>
            {
                if (!folderIds.Contains(a.FolderId.GetValueOrDefault()))
                {
                    // 没有父文件夹
                    _files.Add(a);
                }
            });
            folders = _folders;
            files = _files;
        }
        /// <summary>
        /// 构造文件权限关联集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        private IEnumerable<FolderRltPermissions> BuildFolderRlt(ResourcePermissionCreateDto dto)
        {
            var folderPermission = new List<FolderRltPermissions>();
            if (dto.Organizations != null && dto.Organizations.Length > 0)
            {
                foreach (var dtoOrganization in dto.Organizations)
                {
                    //var m = CreateFolderRltPermissions(dto.Id, dtoOrganization);
                    var model = new FolderRltPermissions()
                    {
                        FolderId = dto.Id,
                        MemberId = dtoOrganization,
                        Type = MemberType.Organization
                    };
                    ObjectMapper.Map(dto, model);
                    model.SetId(_generator.Create());
                    folderPermission.Add(model);
                }
            }

            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var user in dto.Users)
                {
                    var model = new FolderRltPermissions()
                    {
                        FolderId = dto.Id,
                        MemberId = user,
                        Type = MemberType.User
                    };
                    ObjectMapper.Map(dto, model);
                    model.SetId(_generator.Create());
                    folderPermission.Add(model);
                }
            }

            if (dto.Rolers != null && dto.Rolers.Length > 0)
            {
                foreach (var role in dto.Rolers)
                {
                    var model = new FolderRltPermissions()
                    {
                        FolderId = dto.Id,
                        MemberId = role,
                        Type = MemberType.Role
                    };
                    ObjectMapper.Map(dto, model);
                    model.SetId(_generator.Create());
                    folderPermission.Add(model);
                }
            }

            return folderPermission;
        }

        private IEnumerable<FolderRltShare> BuildFolderRltShare(ResourcePermissionCreateDto dto)
        {
            var folderShares = new List<FolderRltShare>();
            if (dto.Organizations != null && dto.Organizations.Length > 0)
            {
                foreach (var dtoOrganization in dto.Organizations)
                {
                    var m = CreateFolderRltShare(dto.Id, dtoOrganization);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Organization;
                    folderShares.Add(m);
                }
            }

            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var user in dto.Users)
                {
                    var m = CreateFolderRltShare(dto.Id, user);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.User;
                    folderShares.Add(m);
                }
            }

            if (dto.Rolers != null && dto.Rolers.Length > 0)
            {
                foreach (var role in dto.Rolers)
                {
                    var m = CreateFolderRltShare(dto.Id, role);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Role;
                    folderShares.Add(m);
                }
            }

            return folderShares;
        }

        private IEnumerable<FileRltShare> BuildFileShareRlt(ResourcePermissionCreateDto dto)
        {
            var fileRltShares = new List<FileRltShare>();

            if (dto.Organizations != null && dto.Organizations.Length > 0)
            {
                foreach (var dtoOrganization in dto.Organizations)
                {
                    var m = CreateFileRltShare(dto.Id, MemberType.Organization, dtoOrganization);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Organization;
                    fileRltShares.Add(m);
                }
            }

            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var user in dto.Users)
                {
                    var m = CreateFileRltShare(dto.Id, MemberType.User, user);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.User;
                    fileRltShares.Add(m);
                }
            }

            if (dto.Rolers != null && dto.Rolers.Length > 0)
            {
                foreach (var role in dto.Rolers)
                {
                    var m = CreateFileRltShare(dto.Id, MemberType.Role, role);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Role;
                    fileRltShares.Add(m);
                }
            }
            return fileRltShares;
        }
        private IEnumerable<FileRltPermissions> BuildFileRlt(ResourcePermissionCreateDto dto)
        {
            var fileRltPermissions = new List<FileRltPermissions>();

            if (dto.Organizations != null && dto.Organizations.Length > 0)
            {
                foreach (var dtoOrganization in dto.Organizations)
                {
                    var m = CreateFileRltPermissions(dto.Id, MemberType.Organization, dtoOrganization);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Organization;
                    ObjectMapper.Map(dto, m);
                    fileRltPermissions.Add(m);
                }
            }

            if (dto.Users != null && dto.Users.Length > 0)
            {
                foreach (var user in dto.Users)
                {
                    var m = CreateFileRltPermissions(dto.Id, MemberType.User, user);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.User;
                    fileRltPermissions.Add(m);
                }
            }

            if (dto.Rolers != null && dto.Rolers.Length > 0)
            {
                foreach (var role in dto.Rolers)
                {
                    var m = CreateFileRltPermissions(dto.Id, MemberType.Role, role);
                    ObjectMapper.Map(dto, m);
                    m.SetId(_generator.Create());
                    m.Type = MemberType.Role;
                    fileRltPermissions.Add(m);
                }
            }
            return fileRltPermissions;
        }

        /// <summary>
        /// 设置文件夹下的文件及文件夹为共享状态
        /// </summary>
        /// <param name="floderId">文件夹id</param>
        /// <returns></returns>
        private async Task<bool> SetFloderShareAsync(ResourcePermissionCreateDto input)
        {
            //1、查找子集文件
            var files = FileManager.GetFileList(input.Id, 2).ToList();
            foreach (var item in files)
            {
                input.Id = item.Id;
                var file = BuildFileShareRlt(input);
                var currentFile = await FileManager.GetFile(input.Id);
                if (!file.Any())
                {
                    currentFile.IsShare = false;
                    await FileManager.Delete<FileRltShare>(a => a.FileId == input.Id);
                }
                else
                {
                    currentFile.IsShare = true;
                    // 添加之前先删除
                    await FileManager.Insert(file); // 更新权限关联
                }
                await FileManager.Update(currentFile); // 更新当前的文件
                return true;
            }

            //2、查找子集文件夹


            return true;
        }

        #endregion
    }
}
