using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Resource.Dtos
{
   public class StoreEquipmentTransferExportDto
    {
        /// <summary>
        /// 库存设备Id
        /// </summary>
        public Guid StoreEquipmentId { get; set; }
        /// <summary>
        /// 入库信息id
        /// </summary>
        public List<Guid> Ids { get; set; }

    }
}
