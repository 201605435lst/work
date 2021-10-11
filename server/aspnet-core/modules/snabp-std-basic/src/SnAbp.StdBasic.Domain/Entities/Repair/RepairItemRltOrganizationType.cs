using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{
    public class RepairItemRltOrganizationType : Entity<Guid>
    {
        protected RepairItemRltOrganizationType() { }
        public RepairItemRltOrganizationType(Guid id) { Id = id; }


        /// <summary>
        /// 维修项 Id
        /// </summary>
        public Guid RepairItemId { get; set; }
        public RepairItem RepairItem { get; set; }
       
        /// <summary>
        /// 部门类型
        /// </summary>
        public Guid OrganizationTypeId { get; set; }
        public DataDictionary OrganizationType { get; set; }
    }
}
