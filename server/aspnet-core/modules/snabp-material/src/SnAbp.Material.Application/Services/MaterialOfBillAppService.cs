using NPOI.XWPF.UserModel;
using SnAbp.Identity;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Material.IServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    public class MaterialOfBillAppService : MaterialAppService, IMaterialOfBillAppService
    {
        private readonly IGuidGenerator _guidMaterialOfBill;
        private readonly IRepository<MaterialOfBill, Guid> _MaterialOfBill;
        private readonly IRepository<Inventory, Guid> _InventoryRepository;
        protected IIdentityUserRepository _userRepository;
        private readonly IRepository<MaterialOfBillRltAccessory, Guid> _MaterialOfBillRltAccessory;
        private readonly IRepository<MaterialOfBillRltMaterial, Guid> _MaterialOfBillRltMaterial;
        public MaterialOfBillAppService(
            IIdentityUserRepository userRepository,
            IGuidGenerator guidMaterialOfBill,
            IRepository<MaterialOfBill, Guid> MaterialOfBill,
            IRepository<Inventory, Guid> InventoryRepository,
            IRepository<MaterialOfBillRltAccessory, Guid> MaterialOfBillRltAccessory,
            IRepository<MaterialOfBillRltMaterial, Guid> MaterialOfBillRltMaterial
            )
        {
            _userRepository = userRepository;
            _guidMaterialOfBill = guidMaterialOfBill;
            _MaterialOfBill = MaterialOfBill;
            _InventoryRepository = InventoryRepository;
            _MaterialOfBillRltAccessory = MaterialOfBillRltAccessory;
            _MaterialOfBillRltMaterial = MaterialOfBillRltMaterial;
        }
        public async Task<MaterialOfBillDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("id不能为空");
            var material = _MaterialOfBill.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (material == null) throw new UserFriendlyException("当前数据不存在");
            var res = ObjectMapper.Map<MaterialOfBill, MaterialOfBillDto>(material);
            var userDtos = await _userRepository.GetAsync((Guid)res.CreatorId);
            res.UserName = userDtos.Name;
            return await Task.FromResult(res);
        }

        public async Task<PagedResultDto<MaterialOfBillDto>> GetList(MaterialOfBillSearchDto input)
        {
            var materialOfBills = _MaterialOfBill.WithDetails()
                .WhereIf(input.ConstructionTeam != null, x => x.ConstructionTeam == input.ConstructionTeam)
                .WhereIf(input.SectionId != null && input.SectionId != Guid.Empty, x => x.SectionId == input.SectionId)
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Time >= input.StartTime && x.Time <= input.EndTime)
                .WhereIf(input.State!=null, x => x.State==input.State)
                .WhereIf(input.IsChecking, x => x.State==MaterialOfBillState.Passed|| x.State == MaterialOfBillState.Waitting)
                .OrderBy(x => x.State);

            var ofBillDtos = new List<MaterialOfBillDto>();
            var users = await _userRepository.GetListAsync();
            foreach (var pur in materialOfBills)
            {
                var ofBillDto = new MaterialOfBillDto();
                ofBillDto = ObjectMapper.Map<MaterialOfBill, MaterialOfBillDto>(pur);
                var user = users.Find(x => x.Id == pur.CreatorId);
                ofBillDto.UserName = user.Name;
                ofBillDtos.Add(ofBillDto);
            }
            var result = new PagedResultDto<MaterialOfBillDto>();
            result.TotalCount = ofBillDtos.Count;

            if (input.IsAll)
            {
                result.Items = ofBillDtos;
            }
            else
            {
                result.Items = ofBillDtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }
            return await Task.FromResult(result);
        }

        public async Task<MaterialOfBillDto> Create(MaterialOfBillCreateDto input)
        {
            if (input.ConstructionTeam == null) throw new Volo.Abp.UserFriendlyException("施工队不能为空");
            if (input.SectionId == Guid.Empty || input.SectionId == null) throw new Volo.Abp.UserFriendlyException("施工区段不能为空");
            if (input.Time == null) throw new Volo.Abp.UserFriendlyException("请选择领料日期");
            if (input.MaterialOfBillRltAccessories == null || input.MaterialOfBillRltAccessories.Count == 0) throw new Volo.Abp.UserFriendlyException("请上传附件");
            if (input.MaterialOfBillRltMaterials == null || input.MaterialOfBillRltMaterials.Count == 0) throw new Volo.Abp.UserFriendlyException("请添加要领取的物料");

            var materialOfBill = new MaterialOfBill(_guidMaterialOfBill.Create())
            {
                ConstructionTeam = input.ConstructionTeam,
                SectionId = input.SectionId,
                Time = input.Time,
                Remark = input.Remark,
                State = input.State
            };

            materialOfBill.MaterialOfBillRltAccessories = new List<MaterialOfBillRltAccessory>();

            // 重新保存附件关联表
            foreach (var accessory in input.MaterialOfBillRltAccessories)
            {
                materialOfBill.MaterialOfBillRltAccessories.Add(new MaterialOfBillRltAccessory(_guidMaterialOfBill.Create())
                {
                    FileId = accessory.FileId,
                    MaterialOfBillId = materialOfBill.Id,
                });
            }

            materialOfBill.MaterialOfBillRltMaterials = new List<MaterialOfBillRltMaterial>();
            // 重新保存物料关联表
            foreach (var Materials in input.MaterialOfBillRltMaterials)
            {
                materialOfBill.MaterialOfBillRltMaterials.Add(new MaterialOfBillRltMaterial(_guidMaterialOfBill.Create())
                {
                    InventoryId = Materials.InventoryId,
                    MaterialOfBillId = materialOfBill.Id,
                    count = Materials.Count
                });
            }

            await _MaterialOfBill.InsertAsync(materialOfBill);

            return await Task.FromResult(ObjectMapper.Map<MaterialOfBill, MaterialOfBillDto>(materialOfBill));
        }

        public async Task<MaterialOfBillDto> Update(MaterialOfBillUpdateDto input)
        {
            var materialOfBill = _MaterialOfBill.FirstOrDefault(x => x.Id == input.Id);
            if (materialOfBill == null) throw new Volo.Abp.UserFriendlyException("该领料单不存在");

            if (input.ConstructionTeam == null) throw new Volo.Abp.UserFriendlyException("施工队不能为空");
            if (input.SectionId == Guid.Empty || input.SectionId == null) throw new Volo.Abp.UserFriendlyException("施工区段不能为空");
            if (input.Time == null) throw new Volo.Abp.UserFriendlyException("请选择领料日期");
            if (input.MaterialOfBillRltAccessories == null || input.MaterialOfBillRltAccessories.Count == 0) throw new Volo.Abp.UserFriendlyException("请上传附件");
            if (input.MaterialOfBillRltMaterials == null || input.MaterialOfBillRltMaterials.Count == 0) throw new Volo.Abp.UserFriendlyException("请添加要领取的物料");

            materialOfBill.ConstructionTeam = input.ConstructionTeam;
            materialOfBill.SectionId = input.SectionId;
            materialOfBill.Time = input.Time;
            materialOfBill.Remark = input.Remark;
            materialOfBill.State = input.State;
            materialOfBill.MaterialOfBillRltAccessories = new List<MaterialOfBillRltAccessory>();

            //删除物料关联信息
            await _MaterialOfBillRltMaterial.DeleteAsync(x => x.MaterialOfBillId == materialOfBill.Id);
            materialOfBill.MaterialOfBillRltMaterials = new List<MaterialOfBillRltMaterial>();
            // 重新保存物料关联表
            foreach (var Materials in input.MaterialOfBillRltMaterials)
            {
                materialOfBill.MaterialOfBillRltMaterials.Add(new MaterialOfBillRltMaterial(_guidMaterialOfBill.Create())
                {
                    InventoryId = Materials.InventoryId,
                    MaterialOfBillId = materialOfBill.Id,
                    count = Materials.count
                });
            }



            // 清楚之前关联文件信息
            await _MaterialOfBillRltAccessory.DeleteAsync(x => x.MaterialOfBillId == materialOfBill.Id);

            materialOfBill.MaterialOfBillRltAccessories = new List<MaterialOfBillRltAccessory>();

            // 重新保存附件关联表
            foreach (var accessory in input.MaterialOfBillRltAccessories)
            {
                materialOfBill.MaterialOfBillRltAccessories.Add(new MaterialOfBillRltAccessory(_guidMaterialOfBill.Create())
                {
                    FileId = accessory.FileId,
                    MaterialOfBillId = materialOfBill.Id,
                });
            }

            //获取所有库存数据
            //var allInventories = _InventoryRepository.Where(x => x.Id != null).ToList();

            // 清楚之前关联物资信息
            await _MaterialOfBillRltMaterial.DeleteAsync(x => x.MaterialOfBillId == materialOfBill.Id);

            materialOfBill.MaterialOfBillRltMaterials = new List<MaterialOfBillRltMaterial>();
            // 重新保存物料关联表
            foreach (var Materials in input.MaterialOfBillRltMaterials)
            {
                materialOfBill.MaterialOfBillRltMaterials.Add(new MaterialOfBillRltMaterial(_guidMaterialOfBill.Create())
                {
                    InventoryId = Materials.InventoryId,
                    MaterialOfBillId = materialOfBill.Id,
                    count = Materials.count
                });
                //if (materialOfBill.State == Enums.MaterialOfBillState.Passed)
                //{
                //    var inventory = allInventories.Find(x => x.Id == Materials.InventoryId);
                //    if (inventory != null)
                //    {
                //        inventory.Amount = inventory.Amount - Materials.count;
                //        await _InventoryRepository.UpdateAsync(inventory);
                //    }
                //}
            }

            await _MaterialOfBill.UpdateAsync(materialOfBill);

            return await Task.FromResult(ObjectMapper.Map<MaterialOfBill, MaterialOfBillDto>(materialOfBill));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _MaterialOfBill.DeleteAsync(id);

            return true;
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> Export(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var materialOfBill = _MaterialOfBill.WithDetails().FirstOrDefault(x => x.Id == id);
            if (materialOfBill == null) throw new UserFriendlyException("该领料单数据不存在");

            var user = _userRepository.FirstOrDefault(x => x.Id == materialOfBill.CreatorId);

            string fileName = "施工领料单明细";

            string tableName = "物料明细";

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 10, "宋体", ParagraphAlignment.CENTER), 2);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"施工队：{materialOfBill.ConstructionTeam}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 3);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"施工区段：{materialOfBill.Section.Name}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 4);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"领料人：{user.Name}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 5);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"领料日期：{materialOfBill.Time}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 6);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 10, "宋体", ParagraphAlignment.LEFT, true), 7);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{tableName}", false, 10, "宋体", ParagraphAlignment.LEFT, true), 8);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(materialOfBill.MaterialOfBillRltMaterials.Count + 3, 7);//显示的行列数rows:8行,cols:7列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(4, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(5, 816); /* 设置列宽 */
            xwpfTable.SetColumnWidth(6, 560); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "名称", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "型号", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "规格", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "库存地点", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "厂家", ParagraphAlignment.CENTER, 22, true));
            xwpfTable.GetRow(0).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "领料量", ParagraphAlignment.CENTER, 22, true));
            var i = 1;
            //遍历数据
            foreach (var item in materialOfBill.MaterialOfBillRltMaterials)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Inventory.Material?.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Inventory.Material?.Spec}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Inventory.Material?.Model}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Inventory.Partition?.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Inventory.Supplier?.Name}", ParagraphAlignment.CENTER, 22, false));
                xwpfTable.GetRow(i).GetCell(6).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.count}", ParagraphAlignment.CENTER, 22, false));
                i++;
            }

            #endregion
            //向文档流中写入内容，生成word
            MemoryStream stream = new MemoryStream();
            document.Write(stream);
            var buf = stream.ToArray();
            stream = new MemoryStream(buf);
            return stream ;
        }
    }
}
