/**********************************************************************
*******命名空间： SnAbp.Safe.IServices
*******类 名 称： ISafeSpeechVideoAppService
*******类 说 明： 班前讲话接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 15:02:58
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Safe.Dtos;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Safe.IServices
{
    /// <summary>
    /// 班前讲话接口
    /// </summary>
    public interface ISafeSpeechVideoAppService: IApplicationService
    {
        //Task<PagedResultDto<SpeechVideoQueryDto>> GetList(SpeechVideoQueryDto input);
        //Task<bool> Create(SpeechVideoCreateDto input);
    }
}
