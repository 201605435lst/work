﻿using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Problem.Dtos
{
    public class ProblemCategoryUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 全称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
