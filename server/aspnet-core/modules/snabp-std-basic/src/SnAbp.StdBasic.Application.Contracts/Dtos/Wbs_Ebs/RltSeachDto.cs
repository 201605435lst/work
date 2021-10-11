using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
   public class RltSeachDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }


        public bool IsAll { get; set; }
    }
}
