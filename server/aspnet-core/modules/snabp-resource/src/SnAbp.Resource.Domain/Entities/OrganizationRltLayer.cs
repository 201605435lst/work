using SnAbp.Identity;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Resource.Entities
{
    /// <summary>
    ///  组织机构与图层关联实体
    /// </summary>
    public class OrganizationRltLayer : Entity<Guid>
    {
        protected OrganizationRltLayer() { }
        public OrganizationRltLayer(Guid id) { Id = id; }
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }
        /// <summary>
        /// 图层id
        /// </summary>
        public Guid LayerId { get; set; }

    }
}
