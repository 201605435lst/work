using Microsoft.AspNetCore.Http;

using System;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Common.Dtos
{
    public class QRCodeDto : EntityDto<Guid>
    {
        /// <summary>
        /// 二维码内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 二维码大小
        /// </summary>
        public decimal Size { get; set; }

        /// <summary>
        /// 二维码中间图片
        /// </summary>
        public string ImageBase64Str { get; set; }

        public IFormFile Image { get; set; }
        /// <summary>
        /// 图片大小
        /// </summary>
        public decimal ImageSize { get; set; }

        /// <summary>
        /// 二维码版本
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 是否显示边框
        /// </summary>
        public bool Border { get; set; }
        /// <summary>
        /// 是否显示logo
        /// </summary>
        public bool ShowLogo { get; set; }
    }
}
