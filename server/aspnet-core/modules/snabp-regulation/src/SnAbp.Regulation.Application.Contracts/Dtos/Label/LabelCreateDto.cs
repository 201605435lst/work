using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Label
{
    public class LabelCreateDto:EntityDto
    {
        ///<summary>
        ///名称
        ///</summary>
        public string Name { get; set; }

        ///<summary>
        ///类别
        ///</summary>
        public string Classify { get; set; }
    }
}
