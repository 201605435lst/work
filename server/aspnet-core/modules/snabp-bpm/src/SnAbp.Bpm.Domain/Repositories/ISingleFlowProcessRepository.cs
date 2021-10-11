/**********************************************************************
*******命名空间： SnAbp.Bpm.Repositories
*******接口名称： ISingleFlowProcessRepository
*******接口说明： 单一流程出进度处理接口，其他使用单一流程的模块使用
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 13:45:21
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Bpm.Entities;

using Volo.Abp.Domain.Services;

namespace SnAbp.Bpm.Repositories
{
    public interface ISingleFlowProcessRepository : IDomainService
    {
        Task<List<T>> GetList<T>(Expression<Func<T, bool>> func) where T : class;
        Task Insert<T>(T t) where T : class;
    }
}
