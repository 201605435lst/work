using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 模型图块关联表
    /// </summary>
    public class ModelRltBlock : Entity<Guid>
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        [ForeignKey("Model")]
        public Guid? ModelId { get; set; }
        public virtual Model Model { get; set; }
        
        /// <summary>
        /// 图块Id
        /// </summary>
        [ForeignKey("Block")]
        public Guid? BlockId { get; set; }
        public virtual Block Block { get; set; }


        protected ModelRltBlock() { }
        public ModelRltBlock(Guid id)
        {
            Id = id;
        }
    }
}
