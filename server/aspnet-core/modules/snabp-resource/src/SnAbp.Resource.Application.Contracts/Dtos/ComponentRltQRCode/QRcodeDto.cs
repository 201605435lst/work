using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Resource.Dtos.ComponentRltQRCode
*文件名：QRcodeDto
*创建人： liushengtao
*创建时间：2021/6/25 10:28:24
*描述：
*
***********************************************************************/
namespace SnAbp.Resource.Dtos
{
   public class QRcodeDto
    {
        public QRcodeDto()
        {

        }
        /// <summary>
        /// base64编码
        /// </summary>
        public string QrCode { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
    }
}
