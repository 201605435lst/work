using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    public class RepairItemRltComponentCategory : Entity<Guid>
    {
        protected RepairItemRltComponentCategory() { }
        public RepairItemRltComponentCategory(Guid id) { Id = id; }


        /// <summary>
        /// 维修项 Id
        /// </summary>
        public Guid RepairItemId { get; set; }
        public RepairItem RepairItem { get; set; }
       
        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public ComponentCategory ComponentCategory { get; set; }
    }
}
