/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： FolderAppService
*******类 说 明： 文件夹管理服务接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 18:56:06
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.IServices;
using SnAbp.File.OssSdk;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.File.Services
{
    [Authorize]
    public class FileFolderAppService : FileAppService, IFileFolderAppService
    {
        private readonly IRepository<Folder, Guid> _folderResp;
        private readonly IGuidGenerator _generator;
        private readonly IFileManager _fileManager;
        private readonly IOssRepository _ossResp;
        public FileFolderAppService(
            IGuidGenerator generator,
            IFileManager fileManager,
            IOssRepository ossResp,
            IRepository<Folder, Guid> folderResp
        )
        {
            _generator = generator;
            _folderResp = folderResp;
            _fileManager = fileManager;
            _ossResp = ossResp;
        }
        /// <summary>
        /// 根据id获取文件夹详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileFolderDto> Get(Guid id)
        {
            var model = await _folderResp.FindAsync(id);
            return ObjectMapper.Map<Folder, FileFolderDto>(model);
        }
        /// <summary>
        /// 获取文件夹中的所有文件，文件下载结构组装
        /// </summary>
        /// <param name="id">文件夹的id</param>
        /// <returns></returns>
        public Task<List<FileDownloadDto>> GetDownloadFile(Guid id)
        {
            var list = new List<FileDownloadDto>();
            var folderList = new List<Folder>();
            // 思路:获取到指定的文件夹，查找该文件夹下的空文件夹及文件，并组合成需要的数据格式
            var folder = _folderResp.SingleOrDefault(a => a.Id == id);
            if (folder != null)
            {
                folderList.Add(folder);
                var folders = _folderResp.WithDetails(a => a.Files).Where(a => a.OrganizationId == folder.OrganizationId).ToList();
                GetFolderChildren(folders, folderList, folder.Id);

                // 由获取到的子文件夹进行数据拼接
                folderList?.ForEach(a =>
                {
                    if (!a.Files.Any())
                    {
                        //var file = new FileFileDownloadDto();
                        //// 没有子文件夹，
                        //file.Name = $"{a.Path.Replace("\\","/")}";
                        //file.Path = a.Path;
                        //file.Size = 0;
                        //file.Url = "";
                        //list.Add(file);
                    }
                    else
                    {
                        a.Files?.ForEach(b =>
                        {
                            var file = new FileDownloadDto();
                            file.Name = $"{b.Path.Replace("\\", "/")}/{b.Name}";
                            file.Path = b.Path;
                            file.Size = b.Size;
                            file.Url = b.Url;
                            file.Type = b.Type;
                            list.Add(file);
                        });
                    }
                });
            }
            return Task.FromResult(list);
        }

        private void GetFolderChildren(List<Folder> folders, List<Folder> folderList, Guid pid)
        {
            folders?.ForEach(a =>
            {
                if (a.ParentId != pid)
                {
                    return;
                }

                folderList.Add(a);
                GetFolderChildren(folders, folderList, a.Id);
            });
        }


        /// <summary>
        ///     新建文件夹
        /// </summary>
        /// <param name="input">需要输入的参数</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Folder.Create)]
        public async Task<bool> Create(FileFolderInputDto input)
        {
            //新建文件夹的逻辑，新增了“我的”当组织结构为null 时为私有文件夹，不为空时为公开的文件夹

            var folder = new Folder(_generator.Create());
            ObjectMapper.Map(input, folder);

            folder.IsPublic = !string.IsNullOrEmpty(input.StaticKey) || folder.OrganizationId != null;

            // 记录文件夹的父级文件夹名称，存储格式：父文件夹1\父文件夹2,若没有父级，默认是组织机构id
            if (input.ParentId != null && input.ParentId != Guid.Empty)
            {
                //folder.Path = await _fileManager.GetResourcePath(folder.OrganizationId.GetValueOrDefault(),input.ParentId.Value, input.Name);
                var pFolder = _folderResp.SingleOrDefault(a => a.Id == input.ParentId);
                folder.Path = $"{pFolder?.Path}\\{folder.Name}";
            }
            else
            {
                if (!string.IsNullOrEmpty(folder.StaticKey))
                {
                    var staticFolder = _folderResp.SingleOrDefault(a => a.StaticKey == folder.StaticKey && a.ParentId == null && a.Name == "StaticFolder");
                    if (staticFolder == null)
                    {
                        var newFolder = new Folder(_generator.Create())
                        {
                            Name = "StaticFolder",
                            Path = "StaticFolder",
                            IsPublic = true,
                            Parent = null,
                            StaticKey = folder.StaticKey,
                        };
                        await _folderResp.InsertAsync(newFolder);
                        folder.ParentId = newFolder.Id;
                        folder.Path = $"{newFolder.Path}\\{folder.Name}";
                    }
                    else
                    {
                        folder.ParentId = staticFolder.Id;
                        folder.Path = $"{staticFolder.Path}\\{folder.Name}";
                    }
                }
                else
                {
                    folder.Path = folder.Name;
                }
            }
            var hasFolder = _folderResp.Where(a =>
                a.OrganizationId == folder.OrganizationId && a.ParentId == folder.ParentId && a.Name.StartsWith(folder.Name) && a.StaticKey == folder.StaticKey);
            if (hasFolder.Any())
            {
                var first = hasFolder.OrderByDescending(a => a.Name).First();
                // 存在同名文件夹，需要在名称后+1
                if (first.Name.Contains('_'))
                {
                    var tag = first.Name.Substring(first.Name.LastIndexOf('_') + 1);
                    if (!string.IsNullOrEmpty(tag))
                    {
                        folder.Name = int.TryParse(tag, out var index) ? $"{folder.Name}_{index + 1}" : $"{folder.Name}_1";
                    }
                    else
                    {
                        folder.Name = $"{folder.Name}_1";
                    }
                }
                else
                {
                    folder.Name = $"{folder.Name}_1";
                }
            }
            await _folderResp.InsertAsync(folder);
            return true;
        }

        /// <summary>
        ///     文件夹重命名
        /// </summary>
        /// <param name="input">重命名输入的参数</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Folder.Update)]
        public async Task<bool> Update(FileFolderUpdateDto input)
        {
            if (input.Id == null || string.IsNullOrEmpty(input.Name))
            {
                throw new UserFriendlyException("请检查参数是否正确");
            }

            var model = await _folderResp.GetAsync(input.Id);
            if (model == null)
            {
                return false;
            }
            model.Name = input.Name;


            var hasFolder = _folderResp.FirstOrDefault(a =>
                a.OrganizationId == model.OrganizationId
                && a.ParentId == model.ParentId
                && a.Name == model.Name
                && a.Id != model.Id);
            if (hasFolder != null)
            {
                throw new UserFriendlyException("该目录下存在同名文件夹，名称更新失败！");
            }
            // 文件夹重命名需要同时修改文件路径信息，同时更新该文件夹下所有文件的路径信息。
            var pathArr = model.Path.Split(@"\");
            pathArr[pathArr.Length - 1] = input.Name;
            model.Path = pathArr.JoinAsString(@"\");
            model.Files?.ForEach(a => a.Path = model.Path);
            await _folderResp.UpdateAsync(model);
            // 更新子文件夹及子文件夹中文件的路径
            var folders = _folderResp.WithDetails().Where(a => a.OrganizationId == model.OrganizationId).ToList();
            UpdateFolderPath(model);
            return true;

        }

        private void UpdateFolderPath(Folder folder)
        {
            folder.Folders?.ForEach(a =>
            {
                a.Path = $"{folder.Path}\\{a.Name}";
                a.Files?.ForEach(b => { b.Path = a.Path; });
                _folderResp.UpdateAsync(a);
                UpdateFolderPath(a);
            });
        }

        /// <summary>
        ///     删除指定文件夹
        /// </summary>
        /// <param name="id">需要删除的文件夹id</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Folder.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null)
            {
                throw new UserFriendlyException("文件夹主键不能为空");
            }

            var folder = await _folderResp.GetAsync(id);
            if (folder == null)
            {
                return true;
            }

            folder.IsDeleted = true;
            await _folderResp.UpdateAsync(folder);

            return true;
        }


    }
}