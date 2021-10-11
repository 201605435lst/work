/**********************************************************************
*******命名空间： SnAbp.Technology
*******接口名称： IEFCoreQuanityRepository
*******接口说明： 工程里统计仓储服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 4:03:52 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnAbp.Technology
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEFCoreQuanityRepository
    {
        Task<List<Quanity>> GetList();
    }
}
