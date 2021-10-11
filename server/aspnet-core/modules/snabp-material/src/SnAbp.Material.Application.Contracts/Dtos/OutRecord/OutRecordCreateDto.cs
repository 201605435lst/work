/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Inventory
*******类 名 称： OutRecord
*******类 说 明： 出库记录
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 14:39:50
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 出库记录
    /// </summary>
    public class OutRecordCreateDto
    {
        /// <summary>
        /// 出库编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 关联仓库
        /// </summary>
        public virtual Guid PartitionId { get; set; }
        
        /// <summary>
        /// 登记人
        /// </summary>
        public virtual Guid CreatorId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 关联资料
        /// </summary>
        public List<OutRecordRltFileCreateDto> OutRecordRltFiles { get; set; }
        /// <summary>
        /// 出库材料
        /// </summary>
        public List<OutRecordRltMaterialCreateDto> OutRecordRltMaterials { get; set; }
        /// <summary>
        /// 关联二维码
        /// </summary>
        public List<OutRecordRltQRCodeCreateDto> OutRecordRltQRCodes { get; set; }
    }
}
