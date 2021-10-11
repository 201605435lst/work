using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 入库关联二维码
    /// </summary>
    public class EntryRecordRltQRCodeCreateDto
    {
        /// <summary>
        /// 二维码
        /// </summary>
        public string QRCode { get; set; }
    }
}
