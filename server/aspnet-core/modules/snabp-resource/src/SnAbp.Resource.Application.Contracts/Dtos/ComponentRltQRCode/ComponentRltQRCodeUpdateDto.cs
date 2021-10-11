using System;
using Volo.Abp.Application.Dtos;
namespace SnAbp.Resource.Dtos
{
    public class ComponentRltQRCodeUpdateDto : EntityDto<Guid>  
    {
        /// <summary>
        /// 跟踪构件id
        /// </summary>
        public Guid? ComponentTrackId { get; set; }
        /// <summary>
        /// 跟踪构件二维码 QRCode（构件分类code+新id）
        /// </summary>
        public string QRCode { get; set; }
    }
}
