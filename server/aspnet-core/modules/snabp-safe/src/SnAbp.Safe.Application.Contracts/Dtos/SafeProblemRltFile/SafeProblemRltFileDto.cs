﻿using SnAbp.Safe.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemRltFileDto : EntityDto<Guid>
    {
        public virtual Guid SafeProblemId { get; set; }
        public virtual SafeProblem SafeProblem { get; set; }
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
    }
}
