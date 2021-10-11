/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： FileAppService
*******类 说 明： 文件服务接口实现,需要考虑的问题：所有的文件先上传到个人目录下，不公开
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/12 9:26:44
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.IServices;
using SnAbp.File.OssSdk;
using SnAbp.Message.MessageDefine;
using SnAbp.Message.Notice;
using SnAbp.Utils;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

using NoticeMessage = SnAbp.Message.Notice.NoticeMessage;

namespace SnAbp.File.Services
{
    //[Authorize]
    public class FileFileAppService : FileAppService, IFileFileAppService
    {
        private readonly IRepository<Entities.File, Guid> _fileResp;
        private readonly IRepository<FileVersion, Guid> _fileVersionResp;
        private readonly IRepository<Folder, Guid> _folderResp;
        private readonly IRepository<FolderRltTag, Guid> _folderTagResp;
        private readonly IGuidGenerator _generator;
        private readonly IOssRepository _ossResp;
        private readonly IFileManager _fileManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMessageNoticeProvider _messageNotice;
        public FileFileAppService(
            IOssRepository ossResp,
            IGuidGenerator generator,
            IFileManager fileManager,
            IRepository<Entities.File, Guid> fileResp,
            IRepository<Folder, Guid> folderResp,
            IRepository<FolderRltTag, Guid> folderTagResp,
            IRepository<FileVersion, Guid> fileVersionResp,
            IConfiguration configuration,
            IUnitOfWorkManager unitOfWorkManager,
            IMessageNoticeProvider messageNotice
        )
        {
            _ossResp = ossResp;
            _fileResp = fileResp;
            _generator = generator;
            _folderResp = folderResp;
            _fileManager = fileManager;
            _folderTagResp = folderTagResp;
            _fileVersionResp = fileVersionResp;
            _configuration = configuration;
            _unitOfWorkManager = unitOfWorkManager;
            _messageNotice = messageNotice;
        }

        /// <summary>
        ///     获取文件上传的签名信息，需要文件的类型生成对应的上传签名地址
        /// </summary>
        /// <param name="fileType">文件的类型</param>
        /// <returns>文件上传后的文件对象</returns>
        public async Task<FileDto> GetPresignUrl(FileInputDto input)
        {
            var fileId = _generator.Create(); // 生成一个guid ，用于传递到前端
            if (string.IsNullOrEmpty(input.Sufixx))
            {
                throw new UserFriendlyException("请检查文件格式");
            }
            if (string.IsNullOrEmpty(input.Sufixx))
            {
                throw new UserFriendlyException("请检查文件格式");
            }
            var ossFileName = $"{fileId}{input.Sufixx}";
            var signUrl = await _ossResp.GetPresignUrl(ossFileName);

            var fileDto = new FileDto();

            fileDto.FileId = fileId;
            fileDto.OssFileName = ossFileName;
            fileDto.PresignUrl = signUrl;
            fileDto.OssType = _ossResp.OssServer?.Type;
            if (!string.IsNullOrEmpty(signUrl))
            {
                fileDto.RelativePath = signUrl.Substring(0, signUrl.LastIndexOf('?'))
                    .Replace(fileDto.OssType == OssServerType.Aliyun ? $"http://{_configuration["OssConfig:PublicBucket"]}.{_ossResp.OssServer.EndPoint}" : $"http://{_ossResp.OssServer.EndPoint}/{_configuration["OssConfig:PublicBucket"]}", "");
            }
            return fileDto;
        }

        /// <summary>
        /// 根据文件id获取文件的版本列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<FileVersionDto>> GetVersionList(Guid id)
        {
            var list = _fileVersionResp.WhereIf(id != Guid.Empty, a => a.FileId == id).ToList();
            return Task.FromResult(ObjectMapper.Map<List<FileVersion>, List<FileVersionDto>>(list));
        }

        /// <summary>
        ///     新增文件信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(FilePermissions.File.Create)]
        public async Task<bool> Create(FileCreateDto input)
        {
            //文件创建
            if (input.FileId == Guid.Empty)
            {
                throw new UserFriendlyException("文件id不能为空，请检查服务接口");
            }

            // 判断文件是否已经存在
            if (_fileResp.Any(a => a.Id == input.FileId))
            {
                return true;
            }
            using var uow = _unitOfWorkManager.Begin(true, false);
            // 私有的文件
            var file = new Entities.File();
            ObjectMapper.Map(input, file);
            file.SetId(input.FileId);
            file.FolderId = (await GetFolderId(input));
            if (file.FolderId != null)
            {
                var folder = _folderResp.SingleOrDefault(a => a.Id == file.FolderId);
                if (folder != null)
                {
                    file.Path = folder.Path ?? folder.Name;
                    file.OrganizationId = folder.OrganizationId;
                }
            }
            file.Url = input.Url;

            var ossServer = _ossResp.OssServer;
            var fileVersion = new FileVersion(_generator.Create())
            {
                Oss = ossServer,
                File = file,
                OssId = ossServer?.Id ?? Guid.Empty,
                OssUrl = file.Url, // 记录最新文件的url
                FileId = file.Id,
                Version = 1,
                Size = file.Size
            };


            // 判断是否存在同名的文件
            var hasFile = _fileResp.Where(a =>
                a.OrganizationId == file.OrganizationId && a.FolderId == file.FolderId && a.Name == file.Name && a.Type == file.Type);
            if (hasFile.Any())
            {
                var first = hasFile.OrderByDescending(a => a.Name).First();
                if (first.Name.Contains('_'))
                {
                    var tag = first.Name.Substring(first.Name.LastIndexOf('_') + 1);
                    if (!string.IsNullOrEmpty(tag))
                    {
                        file.Name = int.TryParse(tag, out var index) ? $"{file.Name}_{index + 1}" : $"{file.Name}_1";
                    }
                    else
                    {
                        file.Name = $"{file.Name}_1";
                    }
                }
                else
                {
                    file.Name = $"{file.Name}_1";
                }

            }
            await _fileResp.InsertAsync(file);
            await _fileVersionResp.InsertAsync(fileVersion);
            await uow.SaveChangesAsync();
            return true;
        }



        /// <summary>
        /// 获取文件夹id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<Guid?> GetFolderId(FileCreateDto input)
        {
            if (string.IsNullOrEmpty(input.FolderPath))
            {
                //存储静态文件，若文件夹路径为空时要判断有无静态文件夹，有则存到当前存在的文件夹，无则新建静态文件夹，并存在当前新建文件夹下
                if ((input.FolderId == Guid.Empty || input.FolderId == null) && !string.IsNullOrEmpty(input.StaticKey))
                {
                    var exitFolder = _folderResp.SingleOrDefault(x => x.StaticKey == input.StaticKey && x.Name == "StaticFolder" && x.ParentId == null);
                    if (exitFolder != null)
                    {
                        return exitFolder.Id;
                    }
                    else
                    {
                        var newFolder = new Folder(_generator.Create())
                        {
                            Name = "StaticFolder",
                            Path = "StaticFolder",
                            IsPublic = true,
                            Parent = null,
                            StaticKey = input.StaticKey,
                        };
                        await _folderResp.InsertAsync(newFolder);
                        return newFolder.Id;
                    }
                }
                else
                {
                    return input.FolderId;
                }
            }
            else
            {
                // 思路：  目录上传/目录1级/目录2-1级
                // 文件夹上传，先确认有无指定已有的文件夹
                // 存在已有文件夹，将文件携带的文件夹路径加上指定文件夹的路径，进行查询，如果没有，则新增文件夹
                // 如果没有指定文件夹，只需要将文件携带的文件夹添加并记录其文件夹路径即可
                // 返回的文件夹id 根据文件携带的路径进行匹配

                // 定义一个路径，用来记录当前文件所在层级的不同的不同路径。
                // root
                var path = string.Empty;
                Guid? organizationId = input.OrganizationId;
                // 上传的目录有无指定到文件夹
                if (input.FolderId != null)
                {
                    var folder = _folderResp.FirstOrDefault(a => a.Id == input.FolderId);
                    path = folder?.Path;
                    organizationId = folder?.OrganizationId;
                }
                // 判断是否已经存在了当前文件相同的文件路径，存在则直接返回路径
                var existFolder = _folderResp.FirstOrDefault(a =>
                    a.OrganizationId == input.OrganizationId &&
                    a.Path == (string.IsNullOrEmpty(path) ? $"{input.FolderPath.Replace('/', '\\')}" : $"{path}\\{input.FolderPath.Replace('/', '\\')}"));
                if (existFolder != null)
                {
                    return existFolder.Id;
                }
                else
                {
                    return await GetOrCreateFolder(path, input.FolderPath, organizationId, input.FolderId,
                        input.IsPublic);
                }
            }
        }
        private async Task<Guid?> GetOrCreateFolder(string root, string path, Guid? organizationGuid, Guid? parentGuid, bool isPublic)
        {
            //1、 先查找当前路径是否存在
            var pathArray = path.Split(@"/");
            for (var i = 1; i <= pathArray.Count(); i++)
            {
                var folderPath = pathArray.Take(i).JoinAsString(@"\");
                var folderName = pathArray[i - 1];
                var fullPath = root.IsNullOrEmpty() ? folderPath : $"{root}\\{folderPath}";
                if (CheckFolderExit(fullPath, folderName, organizationGuid, out var folderId))
                {
                    parentGuid = folderId;
                    // 当前文件夹已存在,继续判断下一个，直到当前指定路径的最后一个
                    if (i == pathArray.Count())
                    {
                        return parentGuid;
                    }
                }
                else
                {
                    using var uow = _unitOfWorkManager.Begin(true, false);
                    // 当前指定的文件夹不存在，需要创建
                    parentGuid = await CreateFolder(folderName, isPublic, fullPath, organizationGuid, parentGuid);
                    await uow.CompleteAsync();
                }
            }
            return parentGuid;
        }
        /// <summary>
        /// 检查指定路径的文件夹是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckFolderExit(string path, string folderName, Guid? organizationId, out Guid? folderId)
        {
            using var uow = _unitOfWorkManager.Begin(true, false);
            var folder = _folderResp.FirstOrDefault(a => a.Path == path && a.OrganizationId == organizationId && a.Name == folderName);
            folderId = folder?.Id;
            return folder != null;
        }

        private async Task<Guid> CreateFolder(string name, bool isPublic, string path, Guid? organizationGuid, Guid? parentGuid)
        {
            var newId = _generator.Create();
            var folder = new Folder(newId);
            folder.Name = name;
            folder.IsPublic = isPublic;
            folder.OrganizationId = organizationGuid;
            folder.ParentId = parentGuid;
            folder.Path = path.TrimStart('\\').TrimEnd('\\');
            await _folderResp.InsertAsync(folder);
            return newId;
        }

        /// <summary>
        ///     新增一条文件的版本信息
        /// </summary>
        /// <param name="input">需要输入的全部参数</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.File.Create)]
        public async Task<bool> CreateFileVersion(FileVersionCreateDto input)
        {
            // 创建一条新的版本记录
            if (input.FileId == null)
            {
                throw new UserFriendlyException("输入信息有误");
            }

            var fileVersion = new FileVersion(_generator.Create());
            // 查找原文件
            var file = await _fileResp.GetAsync(input.FileId);
            if (file != null)
            {
                var ossServer = _ossResp.OssServer;
                fileVersion.File = file ?? throw new UserFriendlyException("未找到原文件");
                fileVersion.FileId = file.Id;
                fileVersion.Oss = ossServer;
                fileVersion.OssId = ossServer.Id;
                // 获取新版本文件在oss 中的地址
                fileVersion.OssUrl = input.Url;
                fileVersion.Size = input.Size;
                // 获取版本号
                var version = _fileVersionResp.WhereIf(true, a => a.FileId == input.FileId && !a.IsDeleted)
                    .Select(a => a.Version)
                    .OrderByDescending(a => a)
                    .First();
                fileVersion.Version = version + 1;

                await _fileVersionResp.InsertAsync(fileVersion);
                // 同时更新当前的文件，并选择文件的大小和存储地址为最新版本
                file.Url = input.Url;
                file.Size = input.Size;
                await _fileResp.UpdateAsync(file);
            }
            return true;
        }

        /// <summary>
        /// 选择关联版本文件
        /// </summary>
        /// <param name="input">文件的id</param>
        /// <returns>返回状态</returns>
        public async Task<bool> SelectNewVersion(FileVersionInputDto input)
        {
            /*
             * 处理逻辑：
             * 1、将需要关联的文件从原始地址中删除，将其信息重新创建到当前文件的版本记录中
             * 2、创建完成，并将当前文件的url信息及文件的大小更新成最新版本文件的信息
             */
            if (input.SelectId == Guid.Empty)
            {
                throw new UserFriendlyException("版本文件信息不能为空");
            }

            var file = await _fileManager.GetFile(input.Id);
            var selectedFile = await _fileManager.GetFile(input.SelectId);// 需要选中的文件
            var newFileVersion = new FileVersion();
            //获取当前文件最新的一条版本文件
            var currentFileVersion = _fileVersionResp.Where(a => a.FileId == input.Id).OrderByDescending(a => a.Version)
                .FirstOrDefault();
            if (currentFileVersion != null)
            {
                // 文件信息映射
                ObjectMapper.Map(currentFileVersion, newFileVersion);
                newFileVersion.SetId(_generator.Create());
                newFileVersion.OssUrl = selectedFile.Url;
                newFileVersion.Size = selectedFile.Size;
                newFileVersion.File = null;
                newFileVersion.FileId = file.Id;
                newFileVersion.Version = currentFileVersion.Version + 1;
                // 添加记录
                await _fileVersionResp.InsertAsync(newFileVersion);

                file.Url = selectedFile.Url;//更新当前文件的版本信息
                file.Size = selectedFile.Size;//更新当前文件的版本信息

                await _fileResp.UpdateAsync(file);
                selectedFile.IsHidden = true;
                await _fileResp.UpdateAsync(selectedFile);
                return true;
            }
            else
            {
                throw new UserFriendlyException("当前文件没有版本文件");
            }

        }

        /// <summary>
        ///     重命名文件名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(FilePermissions.File.Update)]
        public async Task<bool> Update(FileUpdateDto input)
        {
            if (input.Id == null && string.IsNullOrEmpty(input.Name))
            {
                throw new UserFriendlyException("文件名不能为空");
            }

            var file = await _fileResp.GetAsync(input.Id);
            if (file == null)
            {
                throw new UserFriendlyException("更新失败，未找到该文件！");
            }

            if (file.Name == input.Name)
            {
                return true;
            }
            file.Name = input.Name;
            var hasFile = _fileResp.FirstOrDefault(a =>
                a.OrganizationId == file.OrganizationId
                && a.FolderId == file.FolderId
                && a.Name == file.Name
                && a.Type == file.Type
                && a.Id != file.Id);
            if (hasFile != null)
            {
                throw new UserFriendlyException("该目录下存在同名文件，文件名称更新失败");
            }
            await _fileResp.UpdateAsync(file);

            return true;

        }

        /// <summary>
        /// 移动端图片上传接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FileSimpleDto> UploadForApp([FromForm] FileUploadDto input)
        {
            try
            {
                DateTime now = DateTime.Now;

                // 文件原名称
                string fileOriginName = input.File.FileName;

                // 文件后缀
                string extension = Path.GetExtension(fileOriginName);

                //获取服务器签名
                var obj = await this.GetPresignUrl(new FileInputDto() { Sufixx = extension });
                var path = obj.RelativePath;

                // 存储文件到磁盘

                Stream stream = input.File.OpenReadStream();

                await _ossResp.PutObject(stream, path.TrimStart('/'));
                // 记录数据库
                Entities.File file = new Entities.File(obj.FileId);
                file.Name = Path.GetFileNameWithoutExtension(fileOriginName);
                file.Type = extension;
                file.Url = path;
                await _fileResp.InsertAsync(file);

                return ObjectMapper.Map<Entities.File, FileSimpleDto>(file);
            }

            catch (Exception ex)
            {
                throw new Exception("文件上传失败，" + ex.Message);
            }
        }

        /// <summary>
        ///     删除指定id的文件
        /// </summary>
        /// <param name="id">需要删除的文件id</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.File.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            var file = await _fileResp.GetAsync(id);
            if (file == null)
            {
                throw new UserFriendlyException("删除失败");
            }

            file.IsDeleted = true;
            await _fileResp.UpdateAsync(file);
            return true;

        }

        [UnitOfWork]
        /// <summary>
        ///     删除指定的版本文件
        /// </summary>
        /// <param name="id">需要删除的版本的id</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.File.Delete)]
        public async Task<bool> DeleteFileVersion(Guid id)
        {
            // 删除指定版本的文件版本记录
            // 逻辑变更，当删除最新的文件版本时，需要将源文件指向未删除的最新的一个文件。
            var fileVersion = await _fileVersionResp.GetAsync(id);
            if (fileVersion == null)
            {
                throw new UserFriendlyException("删除失败，文件未找到");
            }
            // 删除的是最新的数据。
            fileVersion.IsDeleted = true;
            // 判断当前的版本是不是最新的。
            if ((_fileVersionResp.Where(a => a.FileId == fileVersion.FileId).OrderByDescending(b => b.Version)
                .FirstOrDefault() == fileVersion))
            {
                await _fileVersionResp.UpdateAsync(fileVersion);
                await CurrentUnitOfWork.SaveChangesAsync();
                // 更新未删除的最新的一条数据
                var file = await _fileResp.GetAsync(fileVersion.FileId);
                file.Url = _fileVersionResp.Where(a => a.FileId == fileVersion.FileId).OrderByDescending(b => b.Version)
                    .FirstOrDefault()?.OssUrl;
                await _fileResp.UpdateAsync(file);
            }
            else
            {
                await _fileVersionResp.UpdateAsync(fileVersion);
            }
            return true;

        }

        public async Task TTTT()
        {
            var message = new NoticeMessage();
            message.SendType = SendModeType.Default;
            var content = new NoticeMessageContent
            {
                CreateTime = DateTime.Now,
                Title = "测试通知",
                Url = "http://www.baidu.com"
            };
            message.SetContent(content);

            await _messageNotice.PushAsync(message.GetBinary());

        }
    }
}