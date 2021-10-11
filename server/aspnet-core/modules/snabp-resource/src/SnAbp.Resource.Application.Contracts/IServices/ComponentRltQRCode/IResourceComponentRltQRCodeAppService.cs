using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
   public interface IResourceComponentRltQRCodeAppService : IApplicationService
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> GenerateCode(ComponentRltQRCodeCreateDto input);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<QRcodeDto>> ExportCode(ComponentRltQRCodeCreateDto input);

        /// <summary>
        /// 获取构件二维码跟踪记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ComponentRltQRCodeDto> Get(Guid id);

        /// <summary>
        /// 获取构件二维码跟踪记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetView(Guid id);

        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public Task<Stream> Export(Guid id);
    }
}