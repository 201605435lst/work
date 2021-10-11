/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： FileManager
*******类 说 明： 领域层文件管理服务接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 10:16:15
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.File.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using NPOI.OpenXmlFormats.Spreadsheet;
using SnAbp.File.OssSdk;
using SnAbp.File.Repositories;
using SnAbp.Identity;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;
using Volo.Abp;
using Volo.Abp.Uow;

namespace SnAbp.File.Services
{
    public class FileManager : DomainService, IFileManager
    {
        private readonly IRepository<Organization, Guid> _organizationResp;
        private readonly IRepository<IdentityUserRltOrganization> _organizationUsersResp;
        private readonly IRepository<Folder, Guid> _folderResp;
        private readonly IRepository<Entities.File, Guid> _fileResp;
        private readonly IRepository<Tag, Guid> _tagResp;
        private readonly IRepository<FolderRltTag, Guid> _folderRltTagsResp;
        private readonly IRepository<FolderRltPermissions, Guid> _folderRltPermissionResp;
        private readonly IRepository<FolderRltShare, Guid> _folderRltShareResp;
        private readonly IRepository<FileRltTag, Guid> _fileRltTagsResp;
        private readonly IRepository<FileRltShare, Guid> _fileRltShareResp;
        private readonly IRepository<FileRltPermissions, Guid> _fileRltPermissionResp;
        private readonly IDataFilter _dataFilter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        readonly IOssRepository _ossRepository;

        // 自定义仓储
        private readonly ICustomRepository _customRepository;
        public FileManager(
            IRepository<Organization, Guid> organizationResp,
            IRepository<IdentityUserRltOrganization> organizationUsersResp,
            IRepository<Folder, Guid> folderResp,
            IRepository<Entities.File, Guid> fileResp,
            IRepository<Tag, Guid> tagResp,
            IRepository<FolderRltTag, Guid> folderRltTagsResp,
            IRepository<FileRltTag, Guid> fileRltTagsResp,
            ICustomRepository customRepository,
            IRepository<FolderRltPermissions, Guid> folderRltPermissionResp,
            IRepository<FileRltPermissions, Guid> fileRltPermissionResp,
            IRepository<FolderRltShare, Guid> folderRltShareResp,
            IRepository<FileRltShare, Guid> fileRltShareResp,
            IDataFilter dataFilter,
            IUnitOfWorkManager unitOfWorkManager,
            IOssRepository ossRepository

            )
        {
            _organizationResp = organizationResp;
            _organizationUsersResp = organizationUsersResp;
            _folderResp = folderResp;
            _fileResp = fileResp;
            _folderRltTagsResp = folderRltTagsResp;
            _fileRltTagsResp = fileRltTagsResp;
            _tagResp = tagResp;
            _customRepository = customRepository;
            _folderRltPermissionResp = folderRltPermissionResp;
            _fileRltPermissionResp = fileRltPermissionResp;
            _folderRltShareResp = folderRltShareResp;
            _fileRltShareResp = fileRltShareResp;
            _dataFilter = dataFilter;
            _unitOfWorkManager = unitOfWorkManager;
            _ossRepository = ossRepository;
        }

        public Task<Folder> GetFolder(Expression<Func<Folder, bool>> expression)
        {
            return Task.FromResult(_folderResp.WithDetails(a=>a.Files,a=>a.Tags).SingleOrDefault(expression));
        }

        public Task<Folder> GetFolder(Guid id)
        {
            return Task.FromResult(_folderResp.WithDetails().SingleOrDefault(a => a.Id == id));
        }

        public async Task<List<Folder>> GetFolderList(Guid?[] orgIds)
        {
            var folders = _folderResp
                .WithDetails()
                .WhereIf(true, a => !a.IsDeleted || a.IsPublic)
                .WhereIf(true, a => orgIds.Contains(a.OrganizationId))
                .ToList();
            return await Task.FromResult(folders);
        }


        public IQueryable<Folder> GetFolderList(Guid id, int type)
        {
            return type switch
            {
                1 => _folderResp  // 根据组织获取
                    .WithDetails()
                   .WhereIf(true, a => a.OrganizationId == id && a.ParentId == null),
                2 => _folderResp  // 根据文件夹id获取
                    .WithDetails()
                   .WhereIf(true, a => a.ParentId == id),
                3 => _folderRltTagsResp  // 根据标签id获取
                    .WithDetails(a => a.Folder)
                   .WhereIf(true, a => a.TagId == id)
                   .Select(a => a.Folder),
                _ => null,
            };
        }

        public IQueryable<Folder> GetFolderList(Guid id, int type, Expression<Func<Folder, bool>> expression)
        {
            var query = GetFolderList(id, type);
            return query.Where(expression);
        }

        public IQueryable<Folder> GetFolderList(Guid id, int type, ResourceQueryType queryType)
        {
            return GetFolderList(id, type).Concat(GetFolderList(queryType, a => true));
        }

        public IQueryable<Folder> GetFolderList(Expression<Func<Folder, bool>> expression = null)
        {
            return _folderResp.Where(expression);
        }
        public IQueryable<Folder> GetFolderList(ResourceQueryType type, Expression<Func<Folder, bool>> expression = null)
        {
            return type switch
            {
                ResourceQueryType.Deleted => _folderResp
                    .WithDetails()
                    .Where(a => a.IsDeleted)
                    .Where(expression),
                ResourceQueryType.NotDelete => _folderResp
                    .WithDetails()
                    .Where(a => !a.IsDeleted)
                    .Where(expression),
                ResourceQueryType.NotShared => _folderResp
                    .WithDetails()
                    .Where(a => !a.IsShare)
                    .Where(expression),
                ResourceQueryType.Shared => _folderResp
                    .WithDetails()
                    .Where(a => a.IsShare)
                    .Where(expression),
                ResourceQueryType.Publish => _folderResp
                    .WithDetails()
                    .Where(a => a.IsPublic)
                    .Where(expression),
                ResourceQueryType.UnPublish => _folderResp
                    .WithDetails()
                    .Where(a => !a.IsPublic)
                    .Where(expression),
                _ => null
            };
        }

        public IQueryable<Entities.File> GetFileList(Guid id, int type)
        {
            return type switch
            {
                1 => _fileResp
                    .WithDetails()
                    .WhereIf(true, a => a.FolderId == null && a.OrganizationId == id)
                    .WhereIf(true, a => !a.IsHidden || !a.IsDeleted),
                2 => _fileResp
                    .WithDetails()
                    .WhereIf(true, a => a.FolderId == id)
                    .WhereIf(true, a => !a.IsDeleted || !a.IsHidden),
                3 => _fileRltTagsResp
                    .WithDetails()
                    .WhereIf(true, a => a.TagId == id)
                    .Select(a => a.File)
                    .Where(a => !a.IsHidden || !a.IsDeleted),
                _ => null
            };
        }

        public IQueryable<Entities.File> GetFileList(ResourceQueryType type, Expression<Func<Entities.File, bool>> expression = null)
        {
            return type switch
            {
                ResourceQueryType.Deleted => _fileResp
                    .WithDetails()
                    .Where(a => a.IsDeleted)
                    .Where(expression),
                ResourceQueryType.NotDelete => _fileResp
                    .WithDetails()
                    .Where(a => !a.IsDeleted)
                    .Where(expression),
                ResourceQueryType.NotShared => _fileResp
                    .WithDetails()
                    .Where(a => !a.IsShare)
                    .Where(expression),
                ResourceQueryType.Shared => _fileResp
                    .WithDetails()
                    .Where(a => a.IsShare)
                    .Where(expression),
                ResourceQueryType.Publish => _fileResp
                    .WithDetails()
                    .Where(a => a.IsPublic)
                    .Where(expression),
                ResourceQueryType.UnPublish => _fileResp
                    .WithDetails()
                    .Where(a => !a.IsPublic)
                    .Where(expression),
                _ => null
            };
        }

        public IQueryable<Entities.File> GetFileList(Expression<Func<Entities.File, bool>> expression)
        {
            return _fileResp.WithDetails().Where(expression);
        }

        public IQueryable<Entities.File> GetFileList(Guid id, int type, Expression<Func<Entities.File, bool>> expression)
        {
            return GetFileList(id, type).Where(expression);
        }

        public IQueryable<Entities.File> GetFileList(IQueryable<Folder> queryable, ResourceQueryType type)
        {
            // 获取文件夹中的文件
            var orgIds = queryable.Select(a => a.OrganizationId);
            var query = _fileResp
                .WithDetails()
                .WhereIf(orgIds.Any(), a => orgIds.Contains(a.OrganizationId))
                .Where(a => !a.IsHidden);
            return query.Concat(GetFileList(type));
        }

        public IQueryable<Entities.File> GetFileList(Guid id, int type, ResourceQueryType queryType)
        {
            return GetFileList(id, type).Concat(GetFileList(queryType, a => true));
        }

        public async Task<Entities.File> GetFile(Expression<Func<Entities.File, bool>> expression)
        {
            return await Task.FromResult(_fileResp.WithDetails().SingleOrDefault(expression));
        }

        public Task<Entities.File> GetFile(Guid id)
        {
            return Task.FromResult(_fileResp.WithDetails().SingleOrDefault(a => a.Id == id));
        }

        public IQueryable<FileRltPermissions> GetList(Expression<Func<FileRltPermissions, bool>> expression)
        {
            return _fileRltPermissionResp.WithDetails().Where(expression);
        }

        public IQueryable<FolderRltPermissions> GetList(Expression<Func<FolderRltPermissions, bool>> expression)
        {
            return _folderRltPermissionResp.WithDetails().Where(expression);
        }

        public IQueryable<FileRltShare> GetList(Expression<Func<FileRltShare, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FolderRltShare> GetList(Expression<Func<FolderRltShare, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(IEnumerable<Folder> folders)
        {
            await _customRepository.InsertRange(folders);
            return true;
        }

        public async Task<bool> Insert(Folder folder)
        {
            await _folderResp.InsertAsync(folder);
            return true;
        }

        public async Task<bool> Insert(IEnumerable<Entities.File> files)
        {
            await _customRepository.InsertRange(files);
            return true;
        }

        public async Task<bool> Insert(Entities.File file)
        {
            await _fileResp.InsertAsync(file);
            return true;
        }

        public async Task<bool> Insert(IEnumerable<FolderRltPermissions> list)
        {
            //await _folderRltPermissionResp.DeleteAsync(a => a.FolderId == list.FirstOrDefault().FolderId);
            //foreach (var item in list)
            //{
            //    await _folderRltPermissionResp.InsertAsync(item);
            //}
            await _customRepository.InsertRange(list);
            return true;
        }

        public async Task<bool> Insert(IEnumerable<FileRltPermissions> list)
        {
            await _fileRltPermissionResp.DeleteAsync(a => a.FileId == list.FirstOrDefault().FileId);
            await _customRepository.InsertRange(list);
            return false;
        }

        public async Task<bool> Insert(IEnumerable<FileRltShare> list)
        {
            await _fileRltShareResp.DeleteAsync(a => a.FileId == list.FirstOrDefault().FileId);
            await _customRepository.InsertRange(list);
            return false;
        }

        public async Task<bool> Insert(IEnumerable<FolderRltShare> list)
        {
            await _folderRltShareResp.DeleteAsync(a => a.FolderId == list.FirstOrDefault().FolderId);
            await _customRepository.InsertRange(list);
            return false;
        }

        public async Task<bool> Update(IEnumerable<Folder> folders)
        {
            foreach (var folder in folders)
            {
                await _folderResp.UpdateAsync(folder);
            }
            return true;
        }

        public async Task<bool> Update(Folder folder)
        {
            await _folderResp.UpdateAsync(folder);
            return true;
        }

        public async Task<bool> Update(Entities.File file)
        {
            await _fileResp.UpdateAsync(file);
            return true;
        }

        public async Task<bool> Update(IEnumerable<Entities.File> files)
        {
            foreach (var file in files)
            {
                await _fileResp.UpdateAsync(file);
            }
            return true;
        }

        public async Task<List<Organization>> GetOrganizations(Guid userId)
        {
            var list = AbpEnumerableExtensions
                .WhereIf(_organizationUsersResp.WithDetails(a => a.Organization), true,
                    a => a.UserId == userId)
                .Select(a => a.Organization)
                .ToList();
            return await Task.FromResult(list);
        }

        public async Task<List<Organization>> GetOrganizations(Expression<Func<Organization, bool>> expression)
        {
            var a = _organizationResp.Where(expression).ToList();
            return await Task.FromResult(_organizationResp.Where(expression)
                .ToList());
        }

        public async Task<bool> CreateResourceRltTag(IEnumerable<FileRltTag> fileRltTags)
        {
            return await _customRepository.InsertRange(fileRltTags);
        }

        public async Task<bool> CreateResourceRltTag(IEnumerable<FolderRltTag> folderRltTags)
        {
            return await _customRepository.InsertRange(folderRltTags);
        }

        public List<Entities.File> GetInFolderFiles(Guid id, int type)
        {
            return type switch
            {
                1 => _fileResp.WhereIf(true, a => a.OrganizationId == id && !a.IsDeleted).ToList(),
                2 => this.GetFileByFolderId(id),
                3 => this.GetFileByTagId(id),
                _ => null
            };
        }

        public async Task<string> GetResourcePath(Guid? organizationId, Guid? folderId, string folderName = "")
        {
            /**
             * 获取资源路径逻辑实现：
             * 1、新创建的文件，获取文件路径，路径中需要包含当前文件夹名称
             *    当文件夹在组织机构下面：则path 为当前文件夹名称
             *    当文件夹在文件夹下默，path 包含父级路径+自身名称
             * 2、新建文件时，当文件挂在到组织机构，则无需path,当文件改在到指定文件夹，则路径为父文件夹的路径。
             */

            var pathArr = new List<string>();
            if (organizationId != null)
            {
                //我的组织操作
                //获取指定组织结构下的文件夹
                var folders = _folderResp.Where(a => a.OrganizationId == organizationId)?.ToList();
                if (folders.Any())
                {
                    if (folderId != null)
                    {
                        //  获取指定的文件夹
                        var folder = folders.FirstOrDefault(a => a.Id == folderId);
                        if (folder != null)
                        {
                            if (string.IsNullOrEmpty(folderName))
                            {
                                folderName = folder.Name;
                            }
                            pathArr.Add(folder.Name);
                            // 查找是否有父文件夹，有则记录名称
                            FindFolderPath(folders, folder.ParentId.GetValueOrDefault(), pathArr);
                        }
                    }
                }
            }
            else
            {
                // 文件创建在"我的"
                if (folderId != null)
                {
                    // 在指定文件夹中新建
                    var folder = await _folderResp.FindAsync(folderId.Value);
                    var folders = _folderResp.Where(a => a.OrganizationId == null);
                    if (folder != null)
                    {
                        pathArr.Add(folder.Name);
                        // 查找是否有父文件夹，有则记录名称
                        FindFolderPath(folders, folder.ParentId.GetValueOrDefault(), pathArr);
                    }
                }
            }

            pathArr.Add(folderName);
            if (pathArr.Any())
            {
                //pathArr.Reverse();
                return await Task.FromResult(pathArr.JoinAsString(@"\"));
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<bool> Delete(Expression<Func<Entities.File, bool>> expression)
        {
            // 真实删除
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var files = _fileResp.Where(a => a.IsDeleted && !a.IsHidden).ToList();
                if (files.Any())
                {
                    await Delete(files.Select(a => a.Id).ToList(), null, true);
                }
            }
            return true;
        }

        public async Task<bool> Delete(List<Guid> fileIds, List<Guid> folderIds, bool isTure = false)
        {
            if (isTure)
            {
                // 必须循环删除，为了防止删除有关联的文件
                // 删除文件夹，将文件夹中子文件夹及文件全部删掉
                // 获取文件夹中所有文件
                // 需用禁用数据过滤，否则无法执行删除

                // 获取软删除状态下的指定的文件夹和文件
                if (fileIds != null && fileIds.Any())
                {
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        var files = _fileResp.Where(a => a.IsDeleted && fileIds.Contains(a.Id)).ToList();
                        if (files.Any())
                        {
                            foreach (var item in files)
                            {
                                try
                                {
                                    using var uow = _unitOfWorkManager.Begin(true, false);
                                    await _fileResp.HardDeleteAsync(item);
                                    await uow.CompleteAsync();
                                    // 需要删除minio中的资源
                                    if (_ossRepository.OssServer != null)
                                    {
                                        await _ossRepository.Delete(item.Url);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    item.IsHidden = true;   // 文件被引用，只能隐藏
                                    item.FolderId = null;   // 隐藏的同时删除文件夹关联关系
                                    await _fileResp.UpdateAsync(item);
                                }
                            }
                        }
                    }
                }

                if (folderIds != null && folderIds.Any())
                {

                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        var folders = _folderResp.WithDetails(a => a.Files).Where(a => a.IsDeleted && folderIds.Contains(a.Id)).ToList();
                        using var uow = _unitOfWorkManager.Begin(true, false);
                        foreach (var item in folders)
                        {
                            await HardDelete(item.Id); // 先尝试删除子文件夹

                            if (item.Files != null && item.Files.Any())
                            {
                                foreach (var file in item.Files)
                                {
                                    try
                                    {
                                        await _fileResp.HardDeleteAsync(file);
                                    }
                                    catch (Exception e)
                                    {
                                        file.IsHidden = true;   // 文件被引用，只能隐藏
                                        file.FolderId = null;   // 隐藏的同时删除文件夹关联关系
                                        await _fileResp.UpdateAsync(file);
                                    }
                                }
                            }
                            await _folderResp.HardDeleteAsync(item);

                        }
                        await uow.CompleteAsync();
                    }
                }

            }
            else
            {
                // 软删除，只更新删除状态
                if (!fileIds.IsNullOrEmpty())
                {
                    var files = _fileResp.Where(a => fileIds.Contains(a.Id))
                        .ToList();
                    files?.ForEach(a =>
                    {
                        _fileResp.DeleteAsync(a);
                    });
                }
                if (!folderIds.IsNullOrEmpty())
                {
                    var folders = _folderResp.WithDetails(a => a.Files).Where(a => folderIds.Contains(a.Id)).ToList();
                    var allFolders = _folderResp.WithDetails(a => a.Files).Where(a => a.OrganizationId == folders.First().OrganizationId && a.ParentId != null).ToList();
                    folders?.ForEach(a =>
                    {
                        if (a.Files != null && a.Files.Any())
                        {
                            a.Files?.ForEach(b => _fileResp.DeleteAsync(b)); // 同时删除文件夹中文件
                        }
                        // 删除子文件夹
                        _folderResp.DeleteAsync(a);
                        DeleteFolderChildren(allFolders, a.Id);
                    });
                }
            }
            return await Task.FromResult(true);
        }

        private async Task HardDelete(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var folder = _folderResp.WithDetails(a => a.Files).FirstOrDefault(a => a.IsDeleted && a.ParentId == id);
                if (folder != null)
                {
                    if (folder.Files != null && folder.Files.Any())
                    {
                        foreach (var file in folder.Files)
                        {
                            try
                            {
                                await _fileResp.HardDeleteAsync(file);
                            }
                            catch (Exception e)
                            {
                                file.IsHidden = true;   // 文件被引用，只能隐藏
                                file.FolderId = null;   // 隐藏的同时删除文件夹关联关系
                                await _fileResp.UpdateAsync(file);
                            }
                        }
                    }
                    await HardDelete(folder.Id);
                    await _folderResp.HardDeleteAsync(folder);
                }
            }
        }

        /// <summary>
        /// 删除子文件夹
        /// </summary>
        /// <param name="folders"></param>
        /// <param name="pid"></param>
        private void DeleteFolderChildren(List<Folder> folders, Guid pid)
        {
            folders?.ForEach(a =>
            {
                if (a.ParentId != pid)
                {
                    return;
                }

                if (a.Files != null && a.Files.Any())
                {
                    a.Files.ForEach(b => _fileResp.DeleteAsync(b)); // 同时删除文件夹中所有文件。
                }
                // 删除子文件
                _folderResp.DeleteAsync(a);
                DeleteFolderChildren(folders, a.Id);
            });
        }

        public async Task<bool> Delete(Expression<Func<Folder, bool>> expression)
        {
            // 真实删除
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var folders = _folderResp.Where(a => a.IsDeleted).ToList();

                if (folders.Any())
                {
                    await Delete(null, folders.Select(a => a.Id).ToList(), true);
                }

            }
            return true;
        }

        public async Task<bool> Delete<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _customRepository.Delete<T>(expression);
        }

        /// <summary>
        /// 清空回收站
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            using var fileUow = _unitOfWorkManager.Begin(true, false);
            // 获取能删除的数据
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var files = _fileResp.Where(a => a.IsDeleted && !a.IsHidden).ToList();
                if (files.Any())
                {
                    foreach (var item in files)
                    {
                        try
                        {

                            await _fileResp.HardDeleteAsync(item);
                            if (_ossRepository.OssServer != null)
                            {
                                await _ossRepository.Delete(item.Url);
                            }
                        }
                        catch (Exception ex)
                        {
                            item.IsHidden = true;   // 文件被引用，只能隐藏
                            item.FolderId = null;   // 隐藏的同时删除文件夹关联关系
                            await _fileResp.UpdateAsync(item);
                        }
                    }
                    await fileUow.CompleteAsync();
                }
            }

            using (_dataFilter.Disable<ISoftDelete>())
            {
                if (fileUow.IsCompleted)
                {
                    var folders = _folderResp.Where(a => a.IsDeleted).ToList();
                    if (folders.Any())
                    {
                        foreach (var folder in folders)
                        {
                            //using var uow = _unitOfWorkManager.Begin(true, false);
                            await _folderResp.HardDeleteAsync(folder);
                            //await uow.SaveChangesAsync();
                        }

                    }
                }

            }
        }

        /// <summary>
        /// 文具文件数据流保存并上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<FileDomainDto> CreateFileByStream(System.IO.Stream stream, string fileName)
        {
            try
            {
                if (stream == null)
                {
                    throw new UserFriendlyException("文件流不能为空");
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    throw new UserFriendlyException("文件名称不能为空");
                }
                var result = new FileDomainDto();
                result.Id = Guid.NewGuid();
                result.Size = stream.Length;
                var nameArr = fileName.Split('.');
                result.Name = nameArr[0];
                result.Type = $".{nameArr[1]}";
                var filePath = $"temporary/{result.Id}{result.Type}";
                result.Url= $"/{filePath}";
                await _ossRepository.PutObject(stream, filePath);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<FileDomainDto> GetFileInfos(Guid id)
        {
            var fileInfos = _fileResp.FirstOrDefault(x => x.Id == id);
            var result = new FileDomainDto();
            if (fileInfos == null)
            {
                return Task.FromResult(result);
            }
            result.Id = fileInfos.Id;
            result.Name = fileInfos.Name;
            result.Size = (long)fileInfos.Size;
            result.Url = fileInfos.Url;
            result.Type = fileInfos.Type;
            return Task.FromResult(result);
        }


        #region 私有方法

        private List<Entities.File> GetFileByFolderId(Guid id)
        {
            var folder = _folderResp.SingleOrDefault(a => a.Id == id);
            if (folder != null)
            {
                return _fileResp
                    .Where(a => a.OrganizationId == folder.OrganizationId && a.FolderId != null)
                    .Where(a => !a.IsDeleted)
                    .Where(a => !a.IsHidden)
                    .ToList();
            }
            else
            {
                return null;
            }
        }

        private List<Entities.File> GetFileByTagId(Guid id)
        {
            var tag = _tagResp.SingleOrDefault(a => a.Id == id);
            if (tag != null)
            {
                return _fileResp
                    .Where(a => a.OrganizationId == tag.OrganizationId && a.FolderId != null)
                    .Where(a => !a.IsHidden && !a.IsDeleted)
                    .ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 递归查找父级文件夹
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <param name="pathArr"></param>
        private static void FindFolderPath(IEnumerable<Folder> list, Guid id, ICollection<string> pathArr)
        {
            var folder = list.FirstOrDefault(a => a.Id == id);
            if (folder == null)
            {
                return;
            }

            pathArr.Add(folder.Name);
            if (folder.ParentId != null)
            {
                FindFolderPath(list, folder.ParentId.Value, pathArr);
            }
        }

        #endregion
    }
}
