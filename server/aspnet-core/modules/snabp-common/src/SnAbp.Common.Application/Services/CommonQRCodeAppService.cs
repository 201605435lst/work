using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Volo.Abp.Application.Dtos;
using SnAbp.Common.Entities;
using SnAbp.Common.IServices;
using SnAbp.Common.Dtos;
using System.IO;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace SnAbp.Common.Services
{
    public class CommonQRCodeAppService : CommonAppService, ICommonQRCodeAppService
    {
        private readonly IRepository<QRCode> _repository;

        public CommonQRCodeAppService(IRepository<QRCode> repository)
        {
            _repository = repository;
            // 每次初始化时需要检查是否有配置信息，没有则默认添加一条
            if (!_repository.Any())
            {
                var mode = new QRCode(Guid.NewGuid())
                {
                    Border = true,
                    Size = 5,
                    Version = 7,
                    ImageBase64Str = string.Empty,
                    ImageSize=10,
                };
                _repository.InsertAsync(mode);
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public Task<QRCodeDto> Get()
        {
            var qrcode = _repository.FirstOrDefault();
            var res = ObjectMapper.Map<QRCode, QRCodeDto>(qrcode);
            return Task.FromResult(res);
        }

        [HttpPost]
        public Task<string> QRCode([FromForm] QRCodeDto input)
        {
            var base64 = string.Empty;
            var qrCode = CreateQRCode(input.Content, (int)input.Size, (int)input.Version, showLogo:input.ShowLogo, form:input.Image, imageSize:(int)input.ImageSize);
            Bitmap bimg = new Bitmap(qrCode.Width + 30, qrCode.Height + 30);
            Graphics g = Graphics.FromImage(bimg);
            var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(161, 161, 161), 2);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, bimg.Width, bimg.Height);
            g.DrawImage(qrCode, new PointF(15, 15));
            if (input.Border)
            {
                g.DrawRectangle(pen, 0, 0, bimg.Width, bimg.Height);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bimg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                base64 = Convert.ToBase64String(arr);
            }
            return Task.FromResult(base64);
        }

        public async Task<string> GetQRCode(string content)
        {
            var input = _repository.FirstOrDefault();
            var base64 = string.Empty;
            var qrCode = CreateQRCode(content,  (int)input.Size, (int)input.Version, showLogo: input.ShowLog, imageSize: (int)input.ImageSize);
            Bitmap bimg = new Bitmap(qrCode.Width + 30, qrCode.Height + 30);
            Graphics g = Graphics.FromImage(bimg);
            var pen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(161, 161, 161), 2);
            g.FillRectangle(System.Drawing.Brushes.White, 0, 0, bimg.Width, bimg.Height);
            g.DrawImage(qrCode, new PointF(15, 15));
            if (input.Border)
            {
                g.DrawRectangle(pen, 0, 0, bimg.Width, bimg.Height);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bimg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                base64 = Convert.ToBase64String(arr);
            }
            return await Task.FromResult(base64);
        }

        private Bitmap CreateQRCode(string content, int size, int version,bool showLogo=false, IFormFile form = null ,int imageSize=0)
        {
            QRCodeEncoder qrEncoder = new QRCodeEncoder();
            qrEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrEncoder.QRCodeScale = size;
            qrEncoder.QRCodeVersion = version;
            qrEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            try
            {
                Bitmap qrcode = qrEncoder.Encode(content, Encoding.UTF8);
                Image image = null;
                Graphics g = Graphics.FromImage(qrcode);
                if (showLogo && form == null)
                {
                    // 使用当前存储de 图标
                    var str = _repository.FirstOrDefault().ImageBase64Str;
                    var arrs = Convert.FromBase64String(str);
                    image = Image.FromStream(new MemoryStream(arrs), true);
                }
                else if (form!=null)
                {
                    // 渲染小图标
                    var logoArr = form.OpenReadStream();
                    image = Image.FromStream(logoArr, true);
                }
                else
                {
                    return qrcode;
                }
                Bitmap bitMap = new Bitmap(image);
                bitMap = new Bitmap(bitMap, new System.Drawing.Size(imageSize, imageSize));
                PointF point = new PointF(qrcode.Width / 2 - imageSize / 2, qrcode.Height / 2 - imageSize / 2);
                g.DrawImage(bitMap, point);
                return qrcode;
            }
            catch (Exception ex)
            {
                return new Bitmap(100, 100);
            }
        }

        /// <summary>
        /// 更新二维码配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update([FromForm] QRCodeDto input)
        {
            var stream = input.Image?.OpenReadStream();
            var baseStr = string.Empty;
            if (stream != null)
            {
                var arr = new byte[stream.Length];
                stream.Read(arr, 0, arr.Length);
                stream.Seek(0, SeekOrigin.Begin);
                baseStr = Convert.ToBase64String(arr, Base64FormattingOptions.InsertLineBreaks);
            }
            var mode = _repository.FirstOrDefault();
            mode.Border = input.Border;
            mode.Size = input.Size;
            mode.Version = input.Version;
            mode.ImageBase64Str = baseStr;
            mode.ImageSize = input.ImageSize;
            mode.ShowLog = input.ShowLogo;
            await _repository.UpdateAsync(mode);
            return true;
        }
      
        [HttpGet]
        /// <summary>
        /// 二维码下载
        /// </summary>
        /// <returns></returns>
        public async Task<MemoryStream> Download(string content)
        {
            var qrcode = _repository.FirstOrDefault();
            var input = new QRCodeDto()
            {
                Content = content,
                Border = qrcode.Border,
                ImageSize = qrcode.ImageSize,
                Version = qrcode.Version,
                Size = qrcode.Size,
                ShowLogo=false,
            };
            var data =await QRCode(input);
            var arrs = Convert.FromBase64String(data);
            var ms = new MemoryStream(arrs);
            return ms;
        }

        /// <summary>
        /// 根据字符串获取文件对象
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private IFormFile GetFile(string base64String)
        {
            var arrs = Convert.FromBase64String(base64String);
            using(var ms=new MemoryStream(arrs))
            {
                var file = new FormFile(ms,ms.Position,ms.Length,"","");
                return file;
            }
        }
    }
}
