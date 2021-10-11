using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Project.Entities
{
    public class BooksClassification : Entity<Guid>
    {
        /// <summary>
        /// 案卷分类
        /// </summary>
        public string Name { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        public BooksClassification()
        {

        }
        public void SetId(Guid id)
        {
            Id = id;
        }
        public BooksClassification(Guid id)
        {
            Id = id;
        }
    }
}
