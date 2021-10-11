using NPOI.XWPF.UserModel;
using SnAbp.Identity;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.IServices;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    public class MaterialEntryRecordAppService : MaterialAppService, IMaterialEntryRecordAppService
    {
        private readonly IRepository<EntryRecord, Guid> _entryRecordRepository;
        private readonly IRepository<ComponentTrackRecord, Guid> _componentTrackRecordRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<Partition, Guid> _partionRepository;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly IGuidGenerator _guidGenerator;

        public MaterialEntryRecordAppService(
            IRepository<EntryRecord, Guid> entryRecordRepository,
            IRepository<ComponentTrackRecord, Guid> componentTrackRecordRepository,
            IRepository<IdentityUser, Guid> userRepository,
            IRepository<Partition, Guid> partionRepository,
            IRepository<ComponentCategory, Guid> componentCategoryRepository,
            IRepository<Inventory, Guid> inventoryRepository,
            IGuidGenerator guidGenerator
            )
        {
            _entryRecordRepository = entryRecordRepository;
            _componentTrackRecordRepository = componentTrackRecordRepository;
            _userRepository = userRepository;
            _partionRepository = partionRepository;
            _componentCategoryRepository = componentCategoryRepository;
            _inventoryRepository = inventoryRepository;
            _guidGenerator = guidGenerator;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<EntryRecordDetailDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var entryRecord = _entryRecordRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (entryRecord == null)
            {
                throw new UserFriendlyException("该入库记录不存在");
            }
            var dto = ObjectMapper.Map<EntryRecord, EntryRecordDetailDto>(entryRecord);
            if (dto.EntryRecordRltQRCodes.Count > 0)
            {
                foreach (var item in dto.EntryRecordRltQRCodes)
                {
                    var array = item.QRCode.Split("@");
                    if (array.Length == 2)
                    {
                        var componentCategory = _componentCategoryRepository.FirstOrDefault(x => x.Code == array[0]);
                        if (componentCategory != null)
                        {

                            item.ComponentCategory = ObjectMapper.Map<ComponentCategory, ComponentCategoryDto>(componentCategory);
                            item.ComponentCategoryId = componentCategory.Id;
                        }
                    }
                }
            }
            return Task.FromResult(dto);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<EntryRecordDto>> GetList(EntryRecordSearchDto input)
        {
            var entryRecords = _entryRecordRepository.WithDetails()
                  .WhereIf(input.STime != null && input.ETime != null, x => x.Time >= input.STime && x.Time <= input.ETime)
                  .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Remark.Contains(input.Keyword) || x.Code.Contains(input.Keyword))
                  .WhereIf(input.PartitionId != Guid.Empty && input.PartitionId != null, x => x.PartitionId == input.PartitionId);
            var result = new PagedResultDto<EntryRecordDto>()
            {
                TotalCount = entryRecords.Count(),
                Items = ObjectMapper.Map<List<EntryRecord>, List<EntryRecordDto>>
                    (entryRecords.OrderByDescending(x => x.Time).Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
            };
            return Task.FromResult(result);
        }

        /// <summary>
        /// 创建入库记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntryRecordDto> Create(EntryRecordCreateDto input)
        {
            if (input.EntryRecordRltMaterials.Count == 0) throw new UserFriendlyException("请添加要入库的物资");
            if (input.PartitionId == null || input.PartitionId == Guid.Empty) throw new UserFriendlyException("请选择存放位置");
            var code = "ER" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss").Replace("-", "");
            var entryRecord = new EntryRecord(_guidGenerator.Create())
            {
                Code = code,
                Time = input.Time,
                PartitionId = input.PartitionId,
                CreatorId = input.CreatorId,
                Remark = input.Remark,
            };

            entryRecord.EntryRecordRltFiles = new List<EntryRecordRltFile>();

            // 重新保存关联文件信息
            foreach (var file in input.EntryRecordRltFiles)
            {
                entryRecord.EntryRecordRltFiles.Add(new EntryRecordRltFile(_guidGenerator.Create())
                {
                    FileId = file.FileId,
                    EntryRecordId = entryRecord.Id,
                });
            }

            entryRecord.EntryRecordRltMaterials = new List<EntryRecordRltMaterial>();

            // 重新保存关联物资信息
            foreach (var material in input.EntryRecordRltMaterials)
            {
                var inventoryId = new Guid();
                //更改库存信息
                var inventory = _inventoryRepository.FirstOrDefault(x =>
                                                                    x.SupplierId == material.SupplierId &&
                                                                    x.MaterialId == material.MaterialId &&
                                                                    x.Price == material.Price);
                if (inventory != null)
                {
                    inventoryId = inventory.Id;
                    inventory.Amount += material.Count;
                    await _inventoryRepository.UpdateAsync(inventory);
                }
                else
                {
                    var newInventory = new Inventory(_guidGenerator.Create())
                    {
                        MaterialId = material.MaterialId,
                        PartitionId = input.PartitionId,
                        Amount = material.Count,
                        SupplierId = material.SupplierId,
                        EntryTime = input.Time,
                        Price = material.Price,
                    };
                    inventoryId = newInventory.Id;
                    await _inventoryRepository.InsertAsync(newInventory);
                }

                entryRecord.EntryRecordRltMaterials.Add(new EntryRecordRltMaterial(_guidGenerator.Create())
                {
                    InventoryId = inventoryId,
                    MaterialId = material.MaterialId,
                    EntryRecordId = entryRecord.Id,
                    Count = material.Count,
                    Price = material.Price,
                    Time = material.Time,
                    SupplierId = material.SupplierId,
                    Remark = material.Remark,
                }); ;


            }

            entryRecord.EntryRecordRltQRCodes = new List<EntryRecordRltQRCode>();

            // 重新保存关联物资信息
            foreach (var item in input.EntryRecordRltQRCodes)
            {
                entryRecord.EntryRecordRltQRCodes.Add(new EntryRecordRltQRCode(_guidGenerator.Create())
                {
                    QRCode = item.QRCode,
                    EntryRecordId = entryRecord.Id,
                });

                //跟踪构件信息存储
                var array = item.QRCode.Split("@");
                if (array.Length == 2)
                {
                    var componentRltQRCodeId = new Guid(array[1]);
                    //删除之前保存的跟踪记录
                    var oldRecork = _componentTrackRecordRepository.FirstOrDefault(x => x.ComponentRltQRCodeId == componentRltQRCodeId && x.NodeType == NodeType.PutStorage);
                    if (oldRecork != null)
                    {
                        await _componentTrackRecordRepository.DeleteAsync(oldRecork.Id);
                    }
                    var content = input.PartitionId != null && input.PartitionId != Guid.Empty ? $"库存位置：{_partionRepository.FirstOrDefault(x => x.Id == input.PartitionId)?.Name}" : "";
                    var userName = _userRepository.FirstOrDefault(x => x.Id == input.CreatorId)?.Name;
                    var componentTrackRecord = new ComponentTrackRecord(_guidGenerator.Create())
                    {
                        ComponentRltQRCodeId = componentRltQRCodeId,
                        TrackingId = _guidGenerator.Create(),
                        NodeType = NodeType.PutStorage,
                        Time = entryRecord.Time,
                        Content = content,
                        UserId = entryRecord.CreatorId,
                        UserName = userName,
                    };

                    await _componentTrackRecordRepository.InsertAsync(componentTrackRecord);
                }
            }

            await _entryRecordRepository.InsertAsync(entryRecord);


            return ObjectMapper.Map<EntryRecord, EntryRecordDto>(entryRecord);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> Export(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var entryRecord = _entryRecordRepository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (entryRecord == null) throw new UserFriendlyException("该入库单不存在");

            string fileName = "BIM施工入库单明细";

            string tableName = "入库材料";

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 2);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"仓库名称：{entryRecord.Partition?.Name}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 3);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"录入人：{entryRecord.Creator?.Name}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 5);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"入库日期：{entryRecord.Time}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 6);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"备注：{entryRecord.Remark}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 7);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 11, "宋体", ParagraphAlignment.LEFT, true), 8);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{tableName}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 9);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(entryRecord.EntryRecordRltMaterials.Count + 3, 7);//显示的行列数rows:8行,cols:7列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(4, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(5, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(6, 560); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "材料名称", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "规格型号", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "单位", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "价格", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "供应商", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "入库数量", ParagraphAlignment.CENTER, 22, true, 11));
            var i = 1;
            //遍历数据
            foreach (var item in entryRecord.EntryRecordRltMaterials)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Spec}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Unit}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Price }", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Supplier?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Count}", ParagraphAlignment.CENTER, 22, false, 11));
                i++;
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
