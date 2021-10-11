using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Identity
{
    public class OrganizationBatchUpdateTypeDto
    {
        public List<Guid> OrganizationIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 组织机构类型
        /// </summary>
        public Guid? TypeId { get; set; }
    }
}
