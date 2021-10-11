/**********************************************************************
*******命名空间： SnAbp.Material.Services
*******类 名 称： InventoryAppService
*******类 说 明： 库存管理服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 17:19:23
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using NPOI.HSSF.Util;
using NPOI.XWPF.UserModel;
using SnAbp.Material.Dto;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Utils;
using SnAbp.Utils.DataExport;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    /// <summary>
    /// $$
    /// </summary>
    public class InventoryAppService : MaterialAppService, IApplicationService
    {
        readonly IRepository<Inventory> _inventories;
        readonly IRepository<OutRecordRltMaterial, Guid> _outRecordRltMaterial;
        readonly IRepository<EntryRecordRltMaterial, Guid> _entryRecordRltMaterial;
        public InventoryAppService(
            IRepository<Inventory> inventories,
            IRepository<OutRecordRltMaterial, Guid> outRecordRltMaterial,
            IRepository<EntryRecordRltMaterial, Guid> entryRecordRltMaterial)
        {
            _inventories = inventories;
            _outRecordRltMaterial = outRecordRltMaterial;
            _entryRecordRltMaterial = entryRecordRltMaterial;
        }

        public Task<InventoryDetailDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var inventoryDto = new InventoryDetailDto();
            var inventory = _inventories.WithDetails().FirstOrDefault(x => x.Id == id);

            if (inventory == null) throw new UserFriendlyException("当前库存数据不存在");

            inventoryDto = ObjectMapper.Map<Inventory, InventoryDetailDto>(inventory);

            var entryRecords = _entryRecordRltMaterial.WithDetails().Where(x => x.InventoryId == inventory.Id).ToList();
            inventoryDto.EntryRecords = ObjectMapper.Map<List<EntryRecordRltMaterial>, List<EntryRecordRltMaterialDto>>(entryRecords);

            var outRecords = _outRecordRltMaterial.WithDetails().Where(x => x.InventoryId == inventory.Id).ToList();
            inventoryDto.OutRecords = ObjectMapper.Map<List<OutRecordRltMaterial>, List<OutRecordRltMaterialDto>>(outRecords);

            return Task.FromResult(inventoryDto);
        }

        /// <summary>
        /// 根据材料id获取库存列表
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public Task<List<InventoryDto>> GetListByMaterialId(Guid materialId)
        {
            if (materialId == null || materialId == Guid.Empty) throw new UserFriendlyException("材料id不正确");
            var inventories = _inventories.WithDetails().Where(x => x.MaterialId == materialId).ToList();
            if (inventories.Count == 0) throw new UserFriendlyException("当前材料的库存信息不存在");

            return Task.FromResult(ObjectMapper.Map<List<Inventory>, List<InventoryDto>>(inventories));
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<InventoryDto>> GetList(InventorySearchDto input)
        {
            var result = new PagedResultDto<InventoryDto>();
            var query = _inventories
                .WithDetails(a => a.Material, a => a.Supplier, a => a.Partition)
                .WhereIf(!input.Keyword.IsNullOrEmpty(),
                    a => a.Material.Name.Contains(input.Keyword) || a.Material.Name.Contains(input.Keyword))
                .WhereIf(input.STime != null && input.ETime != null, a => a.EntryTime >= input.STime && a.EntryTime <= input.ETime)
                .WhereIf(input.PartitionId.HasValue, a => a.PartitionId == input.PartitionId);
            var totalCount = query.Count();
            var items = query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToList();
            result.Items = ObjectMapper.Map<List<Inventory>, List<InventoryDto>>(items);
            result.TotalCount = totalCount;
            return Task.FromResult(result);
        }

        /// <summary>
        /// 删除单条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _inventories.DeleteAsync(a => a.Id == id);
            return default;
        }


        /// <summary>
        /// 库存整体信息导出
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<Stream> Export(List<Guid> ids)
        {

            if (ids.Any())
            {
                var list = _inventories
                    .WithDetails(a => a.Material, a => a.Partition, a => a.Supplier)
                    .Where(a => ids.Contains(a.Id))
                    .ToList();
                var handler = DataExportHandler.CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
                handler.CreateSheet("库存信息表");
                var titles = new string[]
                {
                    "编号",
                    "材料名称",
                    "规格",
                    "库存地点",
                    "库存量",
                    "入库时间",
                    "价格",
                    "供应商",
                };
                var rowIndex = 0;
                var headRow = handler.CreateRow(rowIndex);
                var cellStyle = handler.CreateCellStyle(
                    CellBorder.CreateBorder(NPOI.SS.UserModel.BorderStyle.Thin, lineColor: HSSFColor.Black.Index));
                for (int i = 0; i < titles.Length; i++)
                {
                    headRow.CreateCell(i)
                        .SetCellStyle(cellStyle)
                        .SetCellValue(titles[i]);
                }
                list?.ForEach(a =>
                {
                    var row = handler.CreateRow(++rowIndex);
                    row.CreateCell(0).SetCellStyle(cellStyle).SetCellValue(list.IndexOf(a) + 1);
                    row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue(a.Material?.Name);
                    row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue(a.Material?.Spec);
                    row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue(a.Partition?.Name);
                    row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue((double)a.Amount);
                    row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue(a.EntryTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    row.CreateCell(6).SetCellStyle(cellStyle).SetCellValue(a.Price);
                    row.CreateCell(7).SetCellStyle(cellStyle).SetCellValue(a.Supplier?.Name);
                });
                Stream stream = handler.GetExcelDataBuffer().BytesToStream();
                return Task.FromResult(stream);
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 单个库存物资出入库详情导出
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Stream> ExportDetail(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var inventory = _inventories.WithDetails().FirstOrDefault(x => x.Id == id);
            if (inventory == null) throw new UserFriendlyException("该库存信息不存在");

            var entryRecords = _entryRecordRltMaterial.WithDetails().Where(x => x.InventoryId == inventory.Id).ToList();
            var outRecords = _outRecordRltMaterial.WithDetails().Where(x => x.InventoryId == inventory.Id).ToList();

            string fileName = "物资出入库明细";

            string entryTableName = "入库信息";
            string outTableName = "出库信息";

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 17, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 2);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "基本信息", false, 11, "宋体", ParagraphAlignment.LEFT, true), 3);
            #region 文档第一个表格对象实例(基础信息)
            //创建文档中的表格对象实例
            XWPFTable basicInfo = document.CreateTable(4, 4);//显示的行列数rows:4行,cols:4列
            basicInfo.Width = 5200;//总宽度
            basicInfo.SetColumnWidth(0, 1000); /* 设置列宽 */
            basicInfo.SetColumnWidth(1, 1600); /* 设置列宽 */
            basicInfo.SetColumnWidth(2, 1000); /* 设置列宽 */
            basicInfo.SetColumnWidth(3, 1600); /* 设置列宽 */

            //Table 表格第一行展示...后面的都是一样，只改变GetRow中的行数
            basicInfo.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "材料名称", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Material?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
            basicInfo.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "材料规格", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Material?.Spec}", ParagraphAlignment.CENTER, 22, false, 11));

            //Table 表格第二行
            basicInfo.GetRow(1).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "材料型号", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(1).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Material?.Model}", ParagraphAlignment.CENTER, 22, false, 11));
            basicInfo.GetRow(1).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "库存位置", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(1).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Partition?.Name}", ParagraphAlignment.CENTER, 22, false, 11));


            //Table 表格第三行
            basicInfo.GetRow(2).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "库存数量", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(2).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Amount}", ParagraphAlignment.CENTER, 24, false, 11));
            basicInfo.GetRow(2).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "价格", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(2).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Price}", ParagraphAlignment.CENTER, 22, false, 11));

            //Table 表格第三行
            basicInfo.GetRow(3).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "登记时间", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(3).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.EntryTime}", ParagraphAlignment.CENTER, 24, false, 11));
            basicInfo.GetRow(3).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, "供应商", ParagraphAlignment.CENTER, 22, true, 11));
            basicInfo.GetRow(3).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, basicInfo, $"{inventory.Supplier?.Name}", ParagraphAlignment.CENTER, 22, false, 11));

            #endregion

            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 11, "宋体", ParagraphAlignment.LEFT, true), 4);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{entryTableName}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 5);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable entryRecordTable = document.CreateTable(entryRecords.Count + 1, 5);//显示的行列数rows:8行,cols:7列
            entryRecordTable.Width = 5200;//总宽度
            entryRecordTable.SetColumnWidth(0, 600); /* 设置列宽 */
            entryRecordTable.SetColumnWidth(1, 1100); /* 设置列宽 */
            entryRecordTable.SetColumnWidth(2, 1100); /* 设置列宽 */
            entryRecordTable.SetColumnWidth(3, 1200); /* 设置列宽 */
            entryRecordTable.SetColumnWidth(4, 1200); /* 设置列宽 */

            //遍历表格标题
            entryRecordTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            entryRecordTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, "入库数量", ParagraphAlignment.CENTER, 22, true, 11));
            entryRecordTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, "入库时间", ParagraphAlignment.CENTER, 22, true, 11));
            entryRecordTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, "登记人", ParagraphAlignment.CENTER, 22, true, 11));
            entryRecordTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, "备注", ParagraphAlignment.CENTER, 22, true, 11));
            var i = 1;
            //遍历数据
            foreach (var item in entryRecords)
            {
                entryRecordTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, $"{i}", ParagraphAlignment.CENTER, 22, false, 11));
                entryRecordTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, $"{item.Count}", ParagraphAlignment.CENTER, 22, false, 11));
                entryRecordTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, $"{item.EntryRecord?.Time}", ParagraphAlignment.CENTER, 22, false, 11));
                entryRecordTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, $"{item.EntryRecord?.Creator?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                entryRecordTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, entryRecordTable, $"{item.Remark }", ParagraphAlignment.CENTER, 22, false, 11));
                i++;
            }
            #endregion

            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 11, "宋体", ParagraphAlignment.LEFT, true), 6);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{outTableName}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 7);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable outRecordTable = document.CreateTable(outRecords.Count + 1, 5);//显示的行列数rows:8行,cols:7列
            outRecordTable.Width = 5200;//总宽度
            outRecordTable.SetColumnWidth(0, 600); /* 设置列宽 */
            outRecordTable.SetColumnWidth(1, 1100); /* 设置列宽 */
            outRecordTable.SetColumnWidth(2, 1100); /* 设置列宽 */
            outRecordTable.SetColumnWidth(3, 1200); /* 设置列宽 */
            outRecordTable.SetColumnWidth(4, 1200); /* 设置列宽 */

            //遍历表格标题
            outRecordTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            outRecordTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, "出库数量", ParagraphAlignment.CENTER, 22, true, 11));
            outRecordTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, "出库时间", ParagraphAlignment.CENTER, 22, true, 11));
            outRecordTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, "登记人", ParagraphAlignment.CENTER, 22, true, 11));
            outRecordTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, "备注", ParagraphAlignment.CENTER, 22, true, 11));
            var j = 1;
            //遍历数据
            foreach (var item in outRecords)
            {
                outRecordTable.GetRow(j).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, $"{j}", ParagraphAlignment.CENTER, 22, false, 11));
                outRecordTable.GetRow(j).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, $"{item.Count}", ParagraphAlignment.CENTER, 22, false, 11));
                outRecordTable.GetRow(j).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, $"{item.OutRecord?.Time}", ParagraphAlignment.CENTER, 22, false, 11));
                outRecordTable.GetRow(j).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, $"{item.OutRecord?.Creator?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                outRecordTable.GetRow(j).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, outRecordTable, $"{item.Remark }", ParagraphAlignment.CENTER, 22, false, 11));
                j++;
            }

            #endregion
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream;
        }
    }
}
