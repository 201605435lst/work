/**********************************************************************
*******命名空间： Volo.Abp.Identity.IServices
*******接口名称： IDataDictionaryAppService
*******接口说明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 16:17:57
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Identity
{
    public interface IAppDataDictionaryAppService: IApplicationService
    {
        /// <summary>
        /// 获取所有字典键
        /// </summary>
        /// <returns></returns>
        Task<List<DataDictionaryDto>> GetAllKeys();
        /// <summary>
        /// 获取某个键的所有值
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        Task<List<DataDictionaryDto>> GetValues(string groupCode);
        /// <summary>
        /// 获取所有字典信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DataDictionaryDto>> GetTreeListAsync(DataDictionaryGetDto input);
    }
}
