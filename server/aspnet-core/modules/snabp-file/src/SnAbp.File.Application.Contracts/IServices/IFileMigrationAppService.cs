/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******接口名称： IFileMigrationAppService
*******接口说明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/10/10 9:02:15
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.OssSdk;
using Volo.Abp.Application.Services;

namespace SnAbp.File.IServices
{
    public interface IFileMigrationAppService: IApplicationService
    {
        Task<FileMigrationDto> DataContrast(FileMigrationInputDto input);
        Task<FileMigrationDto> Start();
        Task<FileMigrationDto> GetProcess();
        Task<bool> Cancel();
    }
}
