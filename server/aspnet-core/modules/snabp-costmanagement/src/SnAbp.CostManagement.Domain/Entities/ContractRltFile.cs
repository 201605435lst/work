using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CostManagement.Entities
{
    public class ContractRltFile : Entity<Guid>
    {
        /// <summary>
        /// 合同
        /// </summary>
        public virtual Guid ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected ContractRltFile() { }
        public ContractRltFile(Guid id)
        {
            Id = id;
        }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}