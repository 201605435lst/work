using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Resource.Dtos.QrCodeExportDto
*文件名：QRcodeDto
*创建人： liushengtao
*创建时间：2021/6/25 10:28:24
*描述：
*
***********************************************************************/
namespace SnAbp.Resource.Dtos
{
   public class QrCodeExportDto
    {
        public QrCodeExportDto()
        {

        }
        /// <summary>
        /// 键
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
    }
}
