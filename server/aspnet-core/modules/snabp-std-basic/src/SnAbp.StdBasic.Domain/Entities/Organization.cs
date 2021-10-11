using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    public class Organization: Entity<Guid> //FullAudited
    {
        /// <summary>
        /// 全称 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 简称 
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// 简介 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 备注 
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 编码 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 排序 
        /// </summary>
        public int Order { get; set; }

        protected Organization() { }

        public Organization(Guid id)
        {
            Id = id;
        }
    }
}
