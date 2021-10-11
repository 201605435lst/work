/**********************************************************************
*******命名空间： SnAbp.Resource.Dtos.Import
*******类 名 称： DataImportDto
*******类 说 明： 数据导入dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 13:52:56
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Utils.DataImport;
using Volo.Abp;

namespace SnAbp.Resource.Dtos
{
    public class DataImportDto
    {
        public FileUploadDto File { get; set; }

        public string ImportKey { get; set; }

        public DataImportType Type { get; set; }

        public void CheckNull()
        {
            if (File == null)
            {
                throw new UserFriendlyException("导入文件不能为空");
            }

            if (this.ImportKey.IsNullOrEmpty())
            {
                throw new UserFriendlyException("关键字不能为空");
            }
        }
    }
}
