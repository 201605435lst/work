using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    public class ComponentCategoryRltProductCategory : Entity<Guid>
    {
        /// <summary>
        /// 构件分类Id
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? ProductionCategoryId { get; set; }
        public virtual ProductCategory ProductionCategory { get; set; }

        protected ComponentCategoryRltProductCategory() { }
        public ComponentCategoryRltProductCategory(Guid id)
        {
            Id = id;
        }
    }
}