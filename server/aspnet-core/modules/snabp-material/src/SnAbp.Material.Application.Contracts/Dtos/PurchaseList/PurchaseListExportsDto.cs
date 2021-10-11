using SnAbp.Bpm.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

/************************************************************************************
*命名空间：SnAbp.Material.Dtos
*文件名：PurchaseListExportsDto
*创建人： liushengtao
*创建时间：2021/7/12 14:18:52
*描述：
*
***********************************************************************/
namespace SnAbp.Material.Dtos
{
   public class PurchaseListExportsDto : PurchaseListDto
    {
        public List<SingleFlowNodeDto> Nodes { get; set; }
    }
}
