/**********************************************************************
*******命名空间： SnAbp.Regulation.Entities
*******类 名 称： Label
*******类 说 明： 标签表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/17 16:30:26
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Regulation.Entities
{
    public class Label : AuditedEntity<Guid>
    {

        protected Label() { }
        public Label(Guid id) { Id = id; }

        ///<summary>
        ///名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///类别
        ///</summary>
        public string Classify { get; set; }

        ///<summary>
        ///引用次数
        ///</summary>
        public int Citation { get; set; }
    }
}
