using SnAbp.Material.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class MaterialAcceptanceRltMaterialCreateDto
    {
        /// 物料检验状态
        public TestState TestState { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 关联材料
        /// </summary>
        public virtual Guid MaterialId { get; set; }
    }
}
