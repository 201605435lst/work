using NPOI.XWPF.UserModel;
using SnAbp.Identity;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Material.IServices;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Material.Services
{
    public class MaterialAcceptanceAppService : MaterialAppService, IMaterialAcceptanceAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<MaterialAcceptance, Guid> _materialAcceptanceRepository;
        private readonly IRepository<IdentityUser, Guid> _userRepository;
        private readonly IRepository<DataDictionary, Guid> _dataDictionaryrRepository;
        private readonly IRepository<ComponentTrackRecord, Guid> _componentTrackRecordRepository;
        private readonly IRepository<MaterialAcceptanceRltMaterial, Guid> _testMaterialsRepository;
        private readonly IRepository<ComponentCategory, Guid> _componentCategoryRepository;

        public MaterialAcceptanceAppService(
            IGuidGenerator guidGenerator,
            IRepository<MaterialAcceptance, Guid> materialAcceptanceRepository,
            IRepository<IdentityUser, Guid> userRepository,
            IRepository<DataDictionary, Guid> dataDictionaryrRepository,
            IRepository<ComponentTrackRecord, Guid> componentTrackRecordRepository,
            IRepository<MaterialAcceptanceRltMaterial, Guid> testMaterialsRepository,
            IRepository<ComponentCategory, Guid> componentCategoryRepository
            )
        {
            _guidGenerator = guidGenerator;
            _materialAcceptanceRepository = materialAcceptanceRepository;
            _userRepository = userRepository;
            _dataDictionaryrRepository = dataDictionaryrRepository;
            _testMaterialsRepository = testMaterialsRepository;
            _componentTrackRecordRepository = componentTrackRecordRepository;
            _componentCategoryRepository = componentCategoryRepository;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MaterialAcceptanceDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var query = _materialAcceptanceRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (query == null) throw new UserFriendlyException("当前数据不存在");
            var result = ObjectMapper.Map<MaterialAcceptance, MaterialAcceptanceDto>(query);
            if (result.MaterialAcceptanceRltQRCodes.Count > 0)
            {
                foreach (var item in result.MaterialAcceptanceRltQRCodes)
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
            return Task.FromResult(result);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<MaterialAcceptanceDto>> GetList(MaterialAcceptanceSearchDto input)
        {
            var result = new PagedResultDto<MaterialAcceptanceDto>();
            var query = _materialAcceptanceRepository.WithDetails()
                .WhereIf(input.TestingOrganizationId != null && input.TestingOrganizationId != Guid.Empty, x => x.TestingOrganizationId == input.TestingOrganizationId)
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.ReceptionTime >= input.StartTime && x.ReceptionTime <= input.EndTime)
                .WhereIf(input.TestingType != null, x => x.TestingType == input.TestingType)
                .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Remark.Contains(input.Keyword) || x.Code.Contains(input.Keyword))
                .WhereIf(input.TestingStatus != null, x => x.TestingStatus == input.TestingStatus);
            var res = ObjectMapper.Map<List<MaterialAcceptance>, List<MaterialAcceptanceDto>>(query.OrderBy(y => y.TestingStatus).ThenByDescending(x => x.ReceptionTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            /// 获取验收单与二维码关联表
            foreach (var item_ in res)
            {
                if (item_.MaterialAcceptanceRltQRCodes.Count > 0)
                {
                    foreach (var item in item_.MaterialAcceptanceRltQRCodes)
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
            }
            result.Items = res;
            result.TotalCount = query.Count();
            return Task.FromResult(result);

        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MaterialAcceptanceDto> Create(MaterialAcceptanceCreateDto input)
        {
            if (input.TestingOrganizationId == Guid.Empty || input.TestingOrganizationId == null) throw new Volo.Abp.UserFriendlyException("检测机构不能为空");
            if (input.ReceptionTime == null) throw new Volo.Abp.UserFriendlyException("验收时间不能为空");
            if (input.Code == null) throw new Volo.Abp.UserFriendlyException("报告编号不能为空");
            if (input.CreatorId == null || input.CreatorId == Guid.Empty) throw new Volo.Abp.UserFriendlyException("登记人不正确");
            var result = new MaterialAcceptance(_guidGenerator.Create());
            ObjectMapper.Map(input, result);
            /// 创建验收单与物资关联表
            result.MaterialAcceptanceRltMaterials = new List<MaterialAcceptanceRltMaterial>();
            foreach (var item in input.MaterialAcceptanceRltMaterials)
            {
                result.MaterialAcceptanceRltMaterials.Add(new MaterialAcceptanceRltMaterial(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    TestState = item.TestState,
                    MaterialId = item.MaterialId,
                    Number = item.Number,
                });
            }
            /// 创建验收单与二维码关联表
            result.MaterialAcceptanceRltQRCodes = new List<MaterialAcceptanceRltQRCode>();
            foreach (var item in input.MaterialAcceptanceRltQRCodes)
            {
                result.MaterialAcceptanceRltQRCodes.Add(new MaterialAcceptanceRltQRCode(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    QRCode = item.QRCode,
                });

                //跟踪构件信息存储
                var array = item.QRCode.Split("@");
                if (array.Length == 2)
                {
                    var componentRltQRCodeId = Guid.Parse(array[1]);
                    //删除之前保存的跟踪记录
                    var oldRecork = _componentTrackRecordRepository.FirstOrDefault(x => x.ComponentRltQRCodeId == componentRltQRCodeId && x.NodeType == NodeType.CheckOut);
                    if (oldRecork != null)
                    {
                        await _componentTrackRecordRepository.DeleteAsync(oldRecork.Id);
                    }

                    var content = input.TestingType == TestingType.Inspect ? "送检" : "自检" +
                        input.TestingOrganizationId != null && input.TestingOrganizationId != Guid.Empty ? $"/检测机构：{_dataDictionaryrRepository.FirstOrDefault(x => x.Id == input.TestingOrganizationId)?.Name}" : "";
                    var userName = _userRepository.FirstOrDefault(x => x.Id == input.CreatorId)?.Name;
                    var componentTrackRecord = new ComponentTrackRecord(_guidGenerator.Create())
                    {
                        ComponentRltQRCodeId = componentRltQRCodeId,
                        TrackingId = _guidGenerator.Create(),
                        NodeType = NodeType.CheckOut,
                        Time = result.ReceptionTime,
                        Content = content,
                        UserId = result.CreatorId,
                        UserName = userName,
                    };

                    await _componentTrackRecordRepository.InsertAsync(componentTrackRecord);
                }
            }
            /// 关联资料
            result.MaterialAcceptanceRltFiles = new List<MaterialAcceptanceRltFile>();
            foreach (var item in input.MaterialAcceptanceRltFiles)
            {
                result.MaterialAcceptanceRltFiles.Add(new MaterialAcceptanceRltFile(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    FileId = item.FileId,
                });
            }
            await _materialAcceptanceRepository.InsertAsync(result);
            var results = ObjectMapper.Map<MaterialAcceptance, MaterialAcceptanceDto>(result);
            return results;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<MaterialAcceptanceDto> Update(MaterialAcceptanceUpdateDto input)
        {
            var result = await _materialAcceptanceRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, result);
            await _testMaterialsRepository.DeleteAsync(x => x.MaterialAcceptanceId == input.Id);
            /// 创建验收单与物资关联表
            result.MaterialAcceptanceRltMaterials = new List<MaterialAcceptanceRltMaterial>();
            foreach (var item in input.MaterialAcceptanceRltMaterials)
            {
                result.MaterialAcceptanceRltMaterials.Add(new MaterialAcceptanceRltMaterial(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    TestState = item.TestState,
                    MaterialId = item.MaterialId,
                    Number = item.Number,
                });
            }
            /// 创建验收单与二维码关联表
            result.MaterialAcceptanceRltQRCodes = new List<MaterialAcceptanceRltQRCode>();
            foreach (var item in input.MaterialAcceptanceRltQRCodes)
            {
                result.MaterialAcceptanceRltQRCodes.Add(new MaterialAcceptanceRltQRCode(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    QRCode = item.QRCode,
                });

                //跟踪构件信息存储
                var array = item.QRCode.Split("@");
                if (array.Length == 2)
                {
                    var componentRltQRCodeId = Guid.Parse(array[1]);
                    //删除之前保存的跟踪记录
                    var oldRecork = _componentTrackRecordRepository.FirstOrDefault(x => x.ComponentRltQRCodeId == componentRltQRCodeId && x.NodeType == NodeType.CheckOut);
                    if (oldRecork != null)
                    {
                        await _componentTrackRecordRepository.DeleteAsync(oldRecork.Id);
                    }
                    var content = input.TestingType == TestingType.Inspect ? "送检" : "自检" +
                        input.TestingOrganizationId != null && input.TestingOrganizationId != Guid.Empty ? $"/检测机构：{_dataDictionaryrRepository.FirstOrDefault(x => x.Id == input.TestingOrganizationId)?.Name}" : "";
                    var userName = _userRepository.FirstOrDefault(x => x.Id == input.CreatorId)?.Name;
                    var componentTrackRecord = new ComponentTrackRecord(_guidGenerator.Create())
                    {
                        ComponentRltQRCodeId = componentRltQRCodeId,
                        TrackingId = _guidGenerator.Create(),
                        NodeType = NodeType.CheckOut,
                        Time = result.ReceptionTime,
                        Content = content,
                        UserId = result.CreatorId,
                        UserName = userName,
                    };

                    await _componentTrackRecordRepository.InsertAsync(componentTrackRecord);
                }
            }
            /// 关联资料
            result.MaterialAcceptanceRltFiles = new List<MaterialAcceptanceRltFile>();
            foreach (var item in input.MaterialAcceptanceRltFiles)
            {
                result.MaterialAcceptanceRltFiles.Add(new MaterialAcceptanceRltFile(_guidGenerator.Create())
                {
                    MaterialAcceptanceId = result.Id,
                    FileId = item.FileId,
                });
            }
            await _materialAcceptanceRepository.UpdateAsync(result);
            var results = ObjectMapper.Map<MaterialAcceptance, MaterialAcceptanceDto>(result);
            return results;
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Stream> Export(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var acceptanceRecord = _materialAcceptanceRepository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (acceptanceRecord == null) throw new UserFriendlyException("该入验收不存在");

            string fileName = "BIM施工-物资验收明细";

            string tableName = "物资列表:";
            string TestingType = GetAcceptanceTypeName(acceptanceRecord.TestingType);
            string TestingStatus = GetAcceptanceStatusName(acceptanceRecord.TestingStatus);

            //创建document文档对象对象实例
            XWPFDocument document = new XWPFDocument();

            //文本标题
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, fileName, true, 19, "宋体", ParagraphAlignment.CENTER), 0);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 1);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", true, 11, "宋体", ParagraphAlignment.CENTER), 2);

            //TODO:这里一行需要显示两个文本
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"报告编号：{acceptanceRecord.Code}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 3);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"登记人：{acceptanceRecord.Creator?.Name}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 4);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"检测类型：{TestingType}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 5);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"检测状态：{TestingStatus}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 6);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"检测机构：{acceptanceRecord.TestingOrganization?.Name}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 7);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"报告日期：{acceptanceRecord.ReceptionTime}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 8);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"备注：{acceptanceRecord.Remark}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 9);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, "", false, 11, "宋体", ParagraphAlignment.LEFT, true), 10);
            document.SetParagraph(NpoiWordParagraphTextStyleHelper._.ParagraphInstanceSetting(document, $"{tableName}", false, 11, "宋体", ParagraphAlignment.LEFT, true), 11);

            #region 文档第表格对象实例（遍历表格项）
            //创建文档中的表格对象实例
            XWPFTable xwpfTable = document.CreateTable(acceptanceRecord.MaterialAcceptanceRltMaterials.Count + 3, 6);//显示的行列数rows:8行,cols:7列
            xwpfTable.Width = 5200;//总宽度
            xwpfTable.SetColumnWidth(0, 560); /* 设置列宽 */
            xwpfTable.SetColumnWidth(1, 928); /* 设置列宽 */
            xwpfTable.SetColumnWidth(2, 928); /* 设置列宽 */
            xwpfTable.SetColumnWidth(3, 928); /* 设置列宽 */
            xwpfTable.SetColumnWidth(4, 928); /* 设置列宽 */
            xwpfTable.SetColumnWidth(5, 928); /* 设置列宽 */

            //遍历表格标题
            xwpfTable.GetRow(0).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "序号", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "物资名称", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "规格/型号", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "类别", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "数量", ParagraphAlignment.CENTER, 22, true, 11));
            xwpfTable.GetRow(0).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, "检测结果", ParagraphAlignment.CENTER, 22, true, 11));
            var i = 1;
            foreach (var item in acceptanceRecord.MaterialAcceptanceRltMaterials)
            {
                xwpfTable.GetRow(i).GetCell(0).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{i}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(1).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(2).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Spec}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(3).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Material?.Type?.Name}", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(4).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{item.Number }", ParagraphAlignment.CENTER, 22, false, 11));
                xwpfTable.GetRow(i).GetCell(5).SetParagraph(NpoiWordParagraphTextStyleHelper._.SetTableParagraphInstanceSetting(document, xwpfTable, $"{GetAcceptanceTestState(item.TestState)}", ParagraphAlignment.CENTER, 22, false, 11));
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid Id)
        {
            if (Id == null || Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var result = _materialAcceptanceRepository.WithDetails().FirstOrDefault(s => s.Id == Id);
            await _materialAcceptanceRepository.DeleteAsync(Id);
            return true;
        }

        ///// <summary>
        ///// 物料验收
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public async Task<MaterialAcceptanceDto> UpdateCheck(MaterialAcceptanceCheckDto input)
        //{
        //    if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要验收的数据");
        //    var materialAcceptance = _materialAcceptanceRepository.WithDetails().FirstOrDefault(s => s.Id == input.Id);
        //    if (materialAcceptance == null) throw new UserFriendlyException("数据无效，请刷新页面重新尝试提交");
        //    materialAcceptance.TestingStatus = input.TestingStatus;
        //    if (input.SubmissionTime != null) materialAcceptance.SubmissionTime = input.SubmissionTime;
        //    if (input.ReceptionTime != null) materialAcceptance.ReceptionTime = input.ReceptionTime;

        //    //清除保存的文件关联表信息
        //    await _materialAcceptanceRltFileRepository.DeleteAsync(x => x.MaterialAcceptanceId == input.Id);
        //    materialAcceptance.Files = new List<MaterialAcceptanceRltFile>();
        //    foreach (var file in input.Files)
        //    {
        //        materialAcceptance.Files.Add(new MaterialAcceptanceRltFile(_guidGenerator.Create())
        //        {
        //            MaterialAcceptanceId = input.Id,
        //            FileId = file.FileId,
        //        });
        //    }

        //    //清除保存的关联表信息
        //    await _testMaterialsRepository.DeleteAsync(x => x.MaterialAcceptanceId == input.Id);
        //    materialAcceptance.UsePlanRltMaterials = new List<MaterialAcceptanceRltMaterial>();
        //    foreach (var item in input.UsePlanRltMaterials)
        //    {
        //        materialAcceptance.UsePlanRltMaterials.Add(new MaterialAcceptanceRltMaterial(_guidGenerator.Create())
        //        {
        //            MaterialId = item.MaterialId,
        //            TestState = item.TestState,
        //            Number = item.Number,
        //        });
        //    }
        //    ////如果验收通过，需要在仓库里面加材料
        //    // if(input.TestingStatus== TestingStatus.Approved && materialAcceptance.UsePlanRltMaterials.Count>0)
        //    // {
        //    //     var material
        //    //     foreach(var item in materialAcceptance.UsePlanRltMaterials)
        //    //     {

        //    //     }
        //    // }
        //    await _materialAcceptanceRepository.UpdateAsync(materialAcceptance);
        //    return ObjectMapper.Map<MaterialAcceptance, MaterialAcceptanceDto>(materialAcceptance);
        //}

        //public async Task<bool> Submit(MaterialAcceptanceSubmitDto input)
        //{
        //    if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要提交的数据");
        //    var materialAcceptance = _materialAcceptanceRepository.WithDetails().FirstOrDefault(s => s.Id == input.Id);
        //    if (materialAcceptance == null) throw new UserFriendlyException("数据无效，请刷新页面重新尝试提交");
        //    materialAcceptance.TestingStatus = input.TestingStatus;
        //    if (input.SubmissionTime != null) materialAcceptance.SubmissionTime = input.SubmissionTime;
        //    if (input.ReceptionTime != null) materialAcceptance.ReceptionTime = input.ReceptionTime;
        //    await _materialAcceptanceRepository.UpdateAsync(materialAcceptance);
        //    return true;
        //}

        //public async Task<bool> CreateAcceptanceQRCode(List<MaterialAcceptanceRltQRCodeCreateDto> input)
        //{
        //    if (input.Count > 0)
        //    {
        //        foreach (var item in input)
        //        {
        //            if (item.MaterialAcceptanceId == null || item.MaterialAcceptanceId == Guid.Empty) throw new UserFriendlyException("验收单Id不能为空");
        //            if (item.MaterialId == null || item.MaterialId == Guid.Empty) throw new UserFriendlyException("验收材料Id不能为空");
        //            if (string.IsNullOrEmpty(item.QRCode)) throw new UserFriendlyException("二维码信息不能为空");
        //            var materialAcceptanceRltQRCode = new MaterialAcceptanceRltQRCode(_guidGenerator.Create())
        //            {
        //                MaterialAcceptanceId = item.MaterialAcceptanceId,
        //                MaterialId = item.MaterialId,
        //                QRCode = item.QRCode,
        //            };
        //            await _QRCodeRepository.InsertAsync(materialAcceptanceRltQRCode);
        //        }

        //    }
        //    return true;
        //}
        #region 私有放法

        /// <summary>
        /// 获取检测状态内容
        /// </summary>
        /// <returns></returns>
        private string GetAcceptanceStatusName(TestingStatus Type)
        {
            string typeName = null;

            if (Type == TestingStatus.ForAcceptance)
            {
                typeName = "待验收";
            }
            if (Type == TestingStatus.Approved)
            {
                typeName = "已验收";
            }
            return typeName;
        }

        /// <summary>
        /// 获取检测类型内容
        /// </summary>
        /// <returns></returns>
        private string GetAcceptanceTypeName(TestingType Type)
        {
            string typeName = null;

            if (Type == TestingType.Inspect)
            {
                typeName = "送检";
            }
            if (Type == TestingType.SelfInspection)
            {
                typeName = "自检";
            }
            return typeName;
        }
        /// <summary>
        /// 获取检测结果内容
        /// </summary>
        /// <returns></returns>
        private string GetAcceptanceTestState(TestState Type)
        {
            string typeName = null;

            if (Type == TestState.Qualified)
            {
                typeName = "合格";
            }
            if (Type == TestState.Disqualification)
            {
                typeName = "不合格";
            }
            return typeName;
        }

        #endregion
    }
}
