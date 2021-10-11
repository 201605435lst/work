using SnAbp.File.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Quality.Dtos
{
    public class QualityProblemRecordRltFileDto : EntityDto<Guid>
    {
        public Guid QualityProblemRecordId { get; set; }
        public QualityProblemRecordDto QualityProblemRecord { get; set; }
        public Guid FileId { get; set; }
        public virtual FileSimpleDto File { get; set; }
    }
}
