using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Label
{
    public class LabelSearchDto: PagedAndSortedResultRequestDto  
    {
        public string KeyWords { get; set; }
        public string Order { get; set; }
    }
}
