using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Entities
{
    public class FileCategory:Entity<Guid>
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string  Name { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public  FileCategory()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public FileCategory(Guid id)
        {
            Id = id;
        }
    }
}
