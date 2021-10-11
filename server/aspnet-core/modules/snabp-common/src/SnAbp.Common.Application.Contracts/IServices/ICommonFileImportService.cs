/**********************************************************************
*******命名空间： SnAbp.Common.IServices
*******类 名 称： ICommonFileImportService
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 11:00:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Common.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Common.IServices
{
    public interface ICommonFileImportService: IApplicationService
    {
        Task<FileImportDto> Check(string key);

        Task<FileImportDto> GetProgress(string key);

        Task<Stream> Download(string key);

        Stream DownloadTemplate(string key);
        Task<FileInfoDto> GetFileInfo(string key);
    }
}
