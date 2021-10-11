using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Model.ModelMVD
{
    public class ModelFileRltConnectorCreateDto
    {
        public string Name { get; set; }

        public string Position { get; set; }

        public Guid ModelFileId { get; set; }

        public Guid Id { get; set; }
    }

    //public class ModelFileRltConnectorCreateDto : PagedAndSortedResultRequestDto
    //{
    //    public Guid ModelFileId { get; set; }

    //    public List<RltConnectorDto> list { get; set; }

    //    public bool IsAll { get; set; }
    //}
    
}
