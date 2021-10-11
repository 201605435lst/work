/**********************************************************************
*******命名空间： Volo.Abp.Identity.Repository
*******接口名称： IDataDictionaryRepository
*******接口说明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 17:47:32
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    public interface IDataDictionaryRepository: IBasicRepository<DataDictionary, Guid>,IQueryable<DataDictionary>, IReadOnlyRepository<DataDictionary>
    {

    }
}
