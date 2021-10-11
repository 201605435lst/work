/**********************************************************************
*******命名空间： SnAbp.StdBasic.Entities.Quota
*******类 名 称： QuotaCategory
*******类 说 明： 定额分类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/29 9:45:22
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Common.Entities;
using SnAbp.Identity;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 定额分类
    /// </summary>
    public class QuotaCategory :Entity<Guid>, IGuidKeyTree<QuotaCategory>, ICodeTree<QuotaCategory>
    {
        public QuotaCategory(Guid id) => this.Id = id;
        public virtual Guid? ParentId { get; set; }
        public virtual QuotaCategory Parent { get; set; }

        public virtual List<QuotaCategory> Children { get; set; }
        /// <summary>
        /// 定额分类中的章节名称
        /// </summary>
        public virtual string Name { get; set; }


        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Specialty来源于数据字典，止于专业节点（即每个专业的定额分类顶节点）
        /// </summary>
        public virtual Guid SpecialtyId { get; set; }

        /// <summary>
        /// 标准编号数据字典
        /// </summary>
        public virtual DataDictionary Specialty { get; set; }

        /// <summary>
        /// 标准编号数据字典
        /// </summary>
        public virtual DataDictionary StandardCode { get; set; }

        /// <summary>
        ///标准编号 Code 来源于数据字典
        /// </summary>
        public virtual Guid StandardCodeId { get; set; }

        /// <summary>
        /// 定额的内容
        /// </summary>
        public virtual string Content { get; set; }
        /// <summary>
        /// 行政区划
        /// </summary>
        public virtual int AreaId { get; set; }
        public virtual Area Area { get; set; }
       
    }
}
