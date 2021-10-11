using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 模型嵌套关系表
    /// </summary>
    public class ModelRltModel : Entity<Guid>
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        public Guid? ModelId { get; set; }
        public virtual Model Model { get; set; }

        /// <summary>
        /// 父级模型Id
        /// </summary>
        public Guid? ParentId { get; set; }
        public virtual Model Parent { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Position { get; set; }

        protected ModelRltModel() { }
        public ModelRltModel(Guid id)
        {
            Id = id;
        }
    }
}