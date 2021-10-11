/**********************************************************************
*******命名空间： SnAbp.Quality.Entities
*******类 名 称： QualityProblemLibraryRltDataDictionary
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:28:50
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Quality.Entities
{
    /// <summary>
    /// $$
    /// </summary>
    public class QualityProblemLibraryRltScop : Entity<Guid>
    {
        public Guid QualityProblemLibraryId { get; set; }
        public QualityProblemLibrary QualityProblemLibrary { get; set; }
        public Guid ScopId { get; set; }
        public virtual Identity.DataDictionary Scop { get; set; }
        public QualityProblemLibraryRltScop(Guid id)
        {
            Id = id;
        }
    }
}
