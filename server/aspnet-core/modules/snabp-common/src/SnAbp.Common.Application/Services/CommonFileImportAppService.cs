/**********************************************************************
*******命名空间： SnAbp.Common.Services
*******类 名 称： CommonFileImportAppService
*******类 说 明： 文件导入服务接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 10:59:33
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Common.Dtos;
using SnAbp.Common.IServices;
using System.Net;
using Volo.Abp;
using System;

namespace SnAbp.Common.Services
{
    public class CommonFileImportAppService : CommonAppService, ICommonFileImportService
    {
        readonly FileImportHandler _fileImportHandler;
        public CommonFileImportAppService(FileImportHandler fileImportHandler) => _fileImportHandler = fileImportHandler;
        public async Task<FileImportDto> Check(string key) => await _fileImportHandler.GetImportState(key);
        public async Task<FileImportDto> GetProgress(string key) => await _fileImportHandler.GetImportState(key);
        /// <summary>
        /// 根据文件导入的key 下载指定的报错或者导入异常的文件。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [Produces("application/octet-stream")]
        public async Task<Stream> Download(string key)
        {
            var importKey = $"{CurrentUser.Id}{key}";
            var dto = _fileImportHandler.GetDownloadFile(importKey);
            await _fileImportHandler.RemoveCache(importKey);
            return dto?.Stream;
        }
        
        public Stream DownloadTemplate(string name)
        {
            //获取文件模板
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                path2: $"import_templates\\import_templates_{name}.xls");
            try
            {
                using (var client = new WebClient())
                {
                    //string tempFile = Path.GetDirectoryName(filePath);
                    client.DownloadFile(filePath, name);
                    return FileToStream(name, true);
                }

            }
            catch (Exception)
            {
                throw new UserFriendlyException("文件模板不存在");
            }

        }
        public static Stream FileToStream(string fileName, bool isDelete)
        {
            //打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);

            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();

            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            if (isDelete)
            {
                File.Delete(fileName);//删除临时文件
            }
            return stream;
        }
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<FileInfoDto> GetFileInfo(string key)
        {
            //获取文件模板
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                path2: $"import_templates\\import_templates_{key}.xls");
            var info = new FileInfoDto();
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                info.Name = fileInfo.Name;
                info.Size = fileInfo.Length;
                info.Date = fileInfo.CreationTime;
                return Task.FromResult(info);
            }
            catch (Exception)
            {
                throw new UserFriendlyException("文件模板不存在");
            }
           
          
        }
    }
}
