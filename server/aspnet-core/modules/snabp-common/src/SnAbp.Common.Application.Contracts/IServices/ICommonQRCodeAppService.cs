using SnAbp.Common.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Common.IServices
{
   public interface ICommonQRCodeAppService: IApplicationService
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        Task<QRCodeDto> Get();
        Task<string> QRCode(QRCodeDto input);
        Task<string> GetQRCode(string content);
        Task<bool> Update(QRCodeDto input);
        Task<MemoryStream> Download(string content);
    }
}
