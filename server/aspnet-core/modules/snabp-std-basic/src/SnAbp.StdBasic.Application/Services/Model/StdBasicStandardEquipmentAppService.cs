using SnAbp.StdBasic.IServices;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.Dtos;
using Volo.Abp.Guids;
using Volo.Abp;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SnAbp.StdBasic.Enums;
using SnAbp.Utils.Validation;
using SnAbp.Utils.ExcelHelper;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Dtos.Import;
using Volo.Abp.Uow;
using SnAbp.Utils;
using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Authorization;
using Volo.Abp.Data;
using System.IO;
using SnAbp.Utils.DataImport;
using SnAbp.StdBasic.Dtos.Export;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    /// <summary>
    /// 标准设备管理
    /// </summary>
    public class StdBasicStandardEquipmentAppService : StdBasicAppService, IStdBasicStandardEquipmentAppService
    {
        private readonly IRepository<Model, Guid> _modelRepository;//标准设备
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;//设备类型
        private readonly IRepository<Manufacturer, Guid> _manufacturerRepository;//设备厂家关系
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private int count = 0;//计数器
        private readonly IDataFilter _dataFilter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public StdBasicStandardEquipmentAppService(
            IRepository<Model, Guid> modelRepository,
            IRepository<ProductCategory, Guid> productCategoryRepository,
            IRepository<Manufacturer, Guid> manufacturerRepository,
            IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IUnitOfWorkManager unitOfWorkManager,
            IDataFilter dataFilter
            )
        {
            _modelRepository = modelRepository;
            _productCategoryRepository = productCategoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _unitOfWorkManager = unitOfWorkManager;
            _dataFilter = dataFilter;
        }

        const int CodeLength = 3;

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StandardEquipmentDetailDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");

            var entity = _modelRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (entity == null) throw new UserFriendlyException("此标准设备不存在");
            return Task.FromResult(ObjectMapper.Map<Model, StandardEquipmentDetailDto>(entity));
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        public Task<PagedResultDto<StandardEquipmentDto>> GetList(StandardEquipmentSearchInputDto input)
        {
            var query = _modelRepository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Manufacturer.Name.Contains(input.Keywords) || x.Code.Contains(input.Keywords) || x.CSRGCode.Contains(input.Keywords))
                .WhereIf(input.ProductCategoryId != null && input.ProductCategoryId != Guid.Empty,
                x => x.ProductCategoryId == input.ProductCategoryId ||
                x.ProductCategory.ParentId == input.ProductCategoryId ||
                x.ProductCategory.Parent.ParentId == input.ProductCategoryId);

            var result = new PagedResultDto<StandardEquipmentDto>()
            {
                TotalCount = query.Count(),
                Items = input.IsAll
                ? ObjectMapper.Map<List<Model>, List<StandardEquipmentDto>>(query.ToList())
                : ObjectMapper.Map<List<Model>, List<StandardEquipmentDto>>(query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.StandardEquipment.Create)]
        public async Task<StandardEquipmentDto> Create(StandardEquipmentCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("设备名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("设备编码不合法");
            }
            if (_modelRepository.Any(z => z.Code == input.Code && !string.IsNullOrEmpty(input.Code))) throw new UserFriendlyException("设备Code编码已存在！");

            if (input.ManufacturerId == null || input.ManufacturerId == Guid.Empty) throw new UserFriendlyException("产品厂商不能为空！");

            if ((input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) && (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty)) throw new UserFriendlyException("产品类型和构件类型不能同时为空！");

            if (!_manufacturerRepository.Any(z => z.Id == input.ManufacturerId)) throw new UserFriendlyException("供应商不存在");

            var befor = _modelRepository.FirstOrDefault(x => x.Name == input.Name && x.ManufacturerId == input.ManufacturerId &&
            (x.ProductCategoryId.HasValue && x.ProductCategoryId == input.ProductCategoryId || (!x.ProductCategoryId.HasValue && x.ComponentCategoryId.HasValue && x.ComponentCategoryId == input.ComponentCategoryId)));
            if (befor != null)
            {
                throw new UserFriendlyException("已经有相同的标准设备数据，请检查");
            }

            var model = new Model(_guidGenerator.Create())
            {
                Code = input.Code,
                ProductCategoryId = input.ProductCategoryId,
                ComponentCategoryId = input.ComponentCategoryId,
                ManufacturerId = input.ManufacturerId,
                ServiceLife = input.ServiceLife,
                ServiceLifeUnit = input.ServiceLifeUnit,
                CSRGCode = input.CSRGCode,
                Name = input.Name,
            };

            await _modelRepository.InsertAsync(model);

            return ObjectMapper.Map<Model, StandardEquipmentDto>(model);
        }

        #region 标准设备导入EXCEL(先不用）
        /*public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度
            await _fileImport.Start(input.ImportKey, 0);
            IWorkbook workbook = null;
            ISheet sheet = null;
            var rowIndex = 5;  //有效数据得起始索引
            List<StandardEquipmentModel> datalist = null;
            // 获取excel表格，判断报个是否满足模板
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<StandardEquipmentModel>(rowIndex);
                datalist = sheet
                        .TryTransToList<StandardEquipmentModel>(rowIndex)
                        .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            var wrongInfos = new List<WrongInfo>();
            List<Model> allModel;
            using (_dataFilter.Disable<ISoftDelete>())
            {
                allModel = _modelRepository.Where(s => true).ToList();
            }
            var allProductCategory = _productCategoryRepository.Where(s => s.IsDeleted == false).ToList();
            var allManufacturer = _manufacturerRepository.Where(s => s.IsDeleted == false).ToList();

            ProductCategory hasProductCategoryModel;
            Manufacturer hasManufacturerModel;

            List<ProductCategory> addedProductCategories = new List<ProductCategory>(); //添加过产品分类信息
            List<Manufacturer> addedManufacturers = new List<Manufacturer>(); //添加过的厂家信息
            List<string> isPro = new List<string>(); //分组后每一组的产品分类IFDParent；
            var codeList = new List<string>(); //往标准设备表中添加数据的所有Code

            var proParent = new List<string>();//产品分类没有的IFDParent和IFD数据
            var proChild = new List<string>(); //产品分类没有的Sepc数据
            var manData = new List<string>(); //厂家分类里面没有的数据
            var StringIsPro = "";



            //根据IFD产品名称进行分组
            var groupDataList = datalist.GroupBy(X => X.IFD).ToList();
            await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count);

            var updateIndex = 1; //进度条

            foreach (var groupList in groupDataList)
            {
                var index = 0;
                var canImport = true;
                isPro = new List<string>();
                StringIsPro = "";

                if (groupList.First().IFDParent != null && groupList.First().IFD != null)
                {
                    var ifdParents = groupList.First().IFDParent.Split('-');
                    for (var i = 0; i < ifdParents.Length; i++)
                    {
                        isPro.Add(ifdParents[i]);
                    }
                    StringIsPro = string.Join(",", isPro);
                }
                else
                {
                    foreach (var ifdParent in groupList)
                    {
                        var newInfo = new WrongInfo(ifdParent.Index);
                        newInfo.AppendInfo("不存在产品分类或产品名称信息");
                        wrongInfos.Add(newInfo);
                    }
                    continue;
                }

                using var unitWork = _unitOfWorkManager.Begin(true, false);
                if (!proParent.Contains(StringIsPro)) //如果存在IfDParent，证明之前没有且已加。就不去查数据库
                {
                    StringIsPro = "";
                    isPro.Add(groupList.First().IFD);
                    for (var i = 0; i < isPro.Count; i++)
                    {
                        hasProductCategoryModel = addedProductCategories.FirstOrDefault(x => x.Name == isPro[i]);
                        if (hasProductCategoryModel == null)
                        {
                            hasProductCategoryModel = allProductCategory.FirstOrDefault(x => x.Name == isPro[i]);
                            if (hasProductCategoryModel == null && index == 0)
                            {
                                hasProductCategoryModel = new ProductCategory(_guidGenerator.Create());
                                hasProductCategoryModel.Name = isPro[i];
                                await _productCategoryRepository.InsertAsync(hasProductCategoryModel);
                                addedProductCategories.Add(hasProductCategoryModel);
                                StringIsPro += isPro[i];
                                if (i == isPro.Count - 2)
                                {
                                    proParent.Add(StringIsPro);
                                }
                                await unitWork.SaveChangesAsync();
                                canImport = false;
                            }
                            else if ((hasProductCategoryModel == null && index != 0) || canImport == false)
                            {
                                hasProductCategoryModel = new ProductCategory(_guidGenerator.Create());
                                hasProductCategoryModel.Name = isPro[i];
                                if (addedProductCategories.FirstOrDefault(x => x.Name == isPro[i - 1]) != null)
                                {
                                    hasProductCategoryModel.ParentId = addedProductCategories.First(x => x.Name == isPro[i - 1]).Id;
                                }
                                else
                                {
                                    hasProductCategoryModel.ParentId = allProductCategory.First(x => x.Name == isPro[i - 1]).Id;
                                }
                                await _productCategoryRepository.InsertAsync(hasProductCategoryModel);
                                addedProductCategories.Add(hasProductCategoryModel);
                                StringIsPro += isPro[i];
                                if (i == isPro.Count - 2)
                                {
                                    proParent.Add(StringIsPro);
                                }
                                await unitWork.SaveChangesAsync();
                            }
                            index++;
                        }
                    }
                }
                else
                {
                    hasProductCategoryModel = addedProductCategories.FirstOrDefault(x => x.Name == groupList.First().IFD);
                    if ()
                    {

                    }
                    canImport = false;
                }


                foreach (var group in groupList)
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;

                    var newInfo = new WrongInfo(group.Index);
                    if (string.IsNullOrEmpty(group.CSRGCode) || string.IsNullOrEmpty(group.Spec) || string.IsNullOrEmpty(group.Unit) || string.IsNullOrEmpty(group.Manufacture))
                    {

                        newInfo.AppendInfo("信息不完全,请检查");
                        wrongInfos.Add(newInfo);
                        continue;
                    }

                    //如果shu。
                    hasProductCategoryModel = allProductCategory.FirstOrDefault(x => x.Name == group.Spec);

                    if (hasProductCategoryModel == null || (canImport == false && proChild.Contains(group.Spec + group.CSRGCode)))
                    {
                        hasProductCategoryModel = new ProductCategory(_guidGenerator.Create());
                        hasProductCategoryModel.Name = group.Spec;
                        hasProductCategoryModel.Unit = group.Unit;
                        hasProductCategoryModel.ParentId = allProductCategory.FirstOrDefault(x => x.Name == group.IFD).Id;
                        await _productCategoryRepository.InsertAsync(hasProductCategoryModel);
                        proChild.Add(group.Spec + group.CSRGCode);
                        addedProductCategories.Add(hasProductCategoryModel);
                        await unitWork.SaveChangesAsync();
                    }



                    //判断生产厂家是否存在，若不存在，添加。
                    if (!manData.Contains(group.Manufacture))
                    {
                        hasManufacturerModel = allManufacturer.FirstOrDefault(x => x.Name == group.Manufacture);
                        if (hasManufacturerModel == null)
                        {
                            hasManufacturerModel = new Manufacturer(_guidGenerator.Create());
                            hasManufacturerModel.Name = group.Manufacture;
                            await _manufacturerRepository.InsertAsync(hasManufacturerModel);
                            addedManufacturers.Add(hasManufacturerModel);
                            manData.Add(group.Manufacture);
                            await unitWork.SaveChangesAsync();
                        }
                    }


                    //同一个code只允许保存一次
                    if (codeList.Any(z => z == group.CSRGCode)) continue;
                    codeList.Add(group.CSRGCode);
                    var isUpdate = true;//执行新增或更新标志
                    using var unitWork1 = _unitOfWorkManager.Begin(true, false);
                    var stdModel = allModel.FirstOrDefault(z => z.CSRGCode == group.CSRGCode);
                    if (stdModel == null)//执行新增
                    {
                        isUpdate = false;
                        stdModel = new Model(Guid.NewGuid());

                        //设备编码
                        stdModel.Code = group.CSRGCode;
                        stdModel.CSRGCode = group.CSRGCode;

                        stdModel.CreationTime = DateTime.Now;
                        stdModel.CreatorId = CurrentUser.Id;
                        stdModel.DeleterId = CurrentUser.Id;
                        stdModel.DeletionTime = DateTime.Now;
                    }
                    stdModel.LastModificationTime = DateTime.Now;
                    stdModel.LastModifierId = CurrentUser.Id;

                    //设备名称
                    stdModel.Name = group.Spec;
                    stdModel.IsDeleted = false;
                    //设备关联构件编码
                    stdModel.ComponentCategoryId = null;

                    //设备关联产品编码
                    var currentPro = addedProductCategories.FirstOrDefault(x => x.Name == group.Spec);
                    if (currentPro != null)
                    {
                        stdModel.ProductCategoryId = currentPro.Id;
                    }
                    else
                    {
                        currentPro = allProductCategory.FirstOrDefault(x => x.Name == group.Spec);
                        stdModel.ProductCategoryId = currentPro.Id;
                    }

                    //关联设备厂家
                    var currentMan = addedManufacturers.FirstOrDefault(x => x.Name == group.Manufacture);
                    if (currentMan == null)
                    {
                        currentMan = allManufacturer.First(x => x.Name == group.Manufacture);
                        stdModel.ManufacturerId = currentMan.Id;
                    }
                    else
                    {
                        stdModel.ManufacturerId = currentMan.Id;
                    }
                    *//*string sql = @"INSERT INT ""Sn_StdBasic_Model"" (ID,""Code"",""CSRGCode"",""CreationTime"",""CreatorId"",""DeleterId"",""DeletionTime"",
                                ""LastModificationTime"",""LastModifierId"",""Name"",""IsDeleted"",""ComponentCategoryId"",""ProductCategoryId"",""ManufacturerId"") VALUES ";

                    sql += "(" + stdModel.Id + "," + stdModel.Code + "," + stdModel.CSRGCode + "," + stdModel.CreationTime + "," + stdModel.CreatorId + "," + stdModel.DeleterId + ","
                            + stdModel.DeletionTime + "," + stdModel.LastModificationTime + "," + stdModel.LastModifierId + "," + stdModel.Name + "," + stdModel.IsDeleted + ","
                            + stdModel.ComponentCategoryId + "," + stdModel.ManufacturerId + ")";
                    if ((updateIndex % 1000 == 0 && updateIndex != 0) || updateIndex == datalist.Count - 1)
                    {

                    }*//*

                    if (isUpdate)
                    {
                        await _modelRepository.UpdateAsync(stdModel);
                    }
                    else
                    {
                        await _modelRepository.InsertAsync(stdModel);
                    }
                    await unitWork1.SaveChangesAsync();
                }
            }
            await _fileImport.Complete(input.ImportKey);
            if (wrongInfos.Any())
            {
                sheet.CreateInfoColumn(wrongInfos);
                await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
            }
        }*/
        #endregion


        #region 标准设备导入EXCEL(新)
        /// <summary>
        /// 导入产品数据
        /// </summary>
        public async Task Upload([FromForm] ImportData input)
        {
            await _fileImport.Start(input.ImportKey, 100);
            var failMsg = new StringBuilder();
            DataTable dt = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            //虚拟的进度开始
            //读取EXCEL
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<StandardEquipmentModel>(4);
                dt = ExcelHelper.ImportBaseDataToDataTable(input.File.File.OpenReadStream(), input.File.File.FileName, out var workbook1);
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (dt == null)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("未找到任何数据,请检查文件格式");
            }

            var treeList = new List<ProductTree>();
            var dataList = new List<ProductImport>();

            #region 验证列是否存在
            //验证列是否存在
            if (!dt.Columns.Contains(ProductImportCol.SeenSun.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.SeenSun.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.CSRGCode.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.CSRGCode.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.IFDParent.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.IFDParent.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.IFD.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.IFD.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.Spec.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.Spec.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.Unit.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.Unit.ToString()}不存在"));
            }
            if (!dt.Columns.Contains(ProductImportCol.Manufacture.ToString()))
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException(string.Format($"导入表格式列{ProductImportCol.Manufacture.ToString()}不存在"));
            }
            #endregion

            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            #region 验证并获取数据
            string rowNmb = "";
            int index = 5;
            foreach (DataRow item in dt.Rows)
            {
                index++;
                rowNmb = item[ProductImportCol.SeenSun.ToString()].ToString();
                //产品编码
                var colVal = Convert.ToString(item[ProductImportCol.CSRGCode.ToString()]);
                if (string.IsNullOrEmpty(colVal))
                {
                    failMsg.AppendLine(FormartErrMsg(rowNmb, "编码为空"));
                    WrongInfo wrong = new WrongInfo(index - 1, "编码为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                var dataModel = new ProductImport();
                dataModel.RowIndex = index - 1;
                dataModel.Code = colVal;

                //设备编码
                colVal = Convert.ToString(item[ProductImportCol.IFDParent.ToString()]);
                if (string.IsNullOrEmpty(colVal))
                {
                    failMsg.AppendLine(FormartErrMsg(rowNmb, "产品分类为空"));
                    WrongInfo wrong = new WrongInfo(index - 1, "产品分类为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.IFDParent = colVal;

                //设备名称
                colVal = Convert.ToString(item[ProductImportCol.IFD.ToString()]);
                if (string.IsNullOrEmpty(colVal))
                {
                    failMsg.AppendLine(FormartErrMsg(rowNmb, "产品名称为空"));
                    WrongInfo wrong = new WrongInfo(index - 1, "产品名称为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.IFD = colVal;

                //设备型号
                colVal = Convert.ToString(item[ProductImportCol.Spec.ToString()]);
                if (string.IsNullOrEmpty(colVal))
                {
                    failMsg.AppendLine(FormartErrMsg(rowNmb, "产品型号为空"));
                    WrongInfo wrong = new WrongInfo(index - 1, "产品型号为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.Spec = colVal;

                //设备厂家
                colVal = Convert.ToString(item[ProductImportCol.Manufacture.ToString()]);
                if (string.IsNullOrEmpty(colVal))
                {
                    failMsg.AppendLine(FormartErrMsg(rowNmb, "产品厂家为空"));
                    WrongInfo wrong = new WrongInfo(index - 1, "产品厂家为空");
                    wrongInfos.Add(wrong);
                    continue;
                }
                dataModel.Manufacture = colVal;

                //单位
                colVal = Convert.ToString(item[ProductImportCol.Unit.ToString()]);
                dataModel.Unit = colVal;

                //产品分类
                dataModel.Items = new List<ProductItem>();
                var names = dataModel.IFDParent.Split('-');
                var childItem = (ProductItem)null;
                for (var i = 0; i < names.Length; i++)
                {
                    childItem = new ProductItem();
                    childItem.Name = names[i];
                    childItem.ParentName = "";
                    foreach (var ele in dataModel.Items)
                    {
                        childItem.ParentName = childItem.ParentName + (ele.Name + "✪");
                    }
                    childItem.ParentName = childItem.ParentName.TrimEnd('✪');
                    dataModel.Items.Add(childItem);
                }

                //产品名称
                colVal = dataModel.IFD;
                childItem = new ProductItem();
                childItem.Name = colVal;
                childItem.ParentName = "";
                foreach (var ele in dataModel.Items)
                {
                    childItem.ParentName = childItem.ParentName + (ele.Name + "✪");
                }
                childItem.ParentName = childItem.ParentName.TrimEnd('✪');
                dataModel.Items.Add(childItem);

                //产品型号
                colVal = dataModel.Spec;
                childItem = new ProductItem();
                childItem.Name = colVal;
                childItem.ParentName = "";
                foreach (var ele in dataModel.Items)
                {
                    childItem.ParentName = childItem.ParentName + (ele.Name + "✪");
                }
                childItem.ParentName = childItem.ParentName.TrimEnd('✪');
                dataModel.Items.Add(childItem);
                dataList.Add(dataModel);
            }
            #endregion

            #region 构建数据结构树
            var parentNode = (ProductTree)null;
            var treeCount = 0;
            foreach (var item in dataList)
            {
                foreach (var ele in item.Items)
                {
                    parentNode = GetNodesParentNode(treeList, ele);
                    if (parentNode != null && string.IsNullOrEmpty(ele.ParentName)) continue;
                    var thisNode = (ProductTree)null;
                    if (parentNode == null)
                    {
                        thisNode = new ProductTree
                        {
                            Id = Guid.NewGuid(),
                            ParentId = Guid.Empty,
                            Name = ele.Name,
                            ParentName = ele.ParentName,
                            Code = item.Code,
                            Level = 1,
                            Unit = item.Unit,
                            Content = item.Unit + "✪" + item.Manufacture,
                            Child = new List<ProductTree>()
                        };
                        treeList.Add(thisNode);
                    }
                    else
                    {
                        //var unit = item.Items.FirstOrDefault().Unit;
                        //if(parentNode.Child.Any(x=>x.Name == item.Spec))
                        //{
                        //    unit = item.Unit;
                        //}

                        var unit = ele.Name != item.Spec ? ele.Unit : item.Unit;

                        if (parentNode.Child.Any(z => z.Name == ele.Name && z.Unit == item.Unit))
                        {
                            continue;
                        }
                        treeCount++;
                        thisNode = new ProductTree
                        {
                            Id = Guid.NewGuid(),
                            ParentId = parentNode.Id,
                            Name = ele.Name,
                            Code = item.Code,
                            Unit = unit,
                            ParentName = ele.ParentName,
                            Level = parentNode.Level + 1,
                            Content = item.Unit + "✪" + item.Manufacture,
                            Child = new List<ProductTree>()
                        };
                        parentNode.Child.Add(thisNode);
                    }

                }
                if (parentNode?.Child != null)
                {
                    item.ProductId = parentNode.Child.LastOrDefault().Id;
                }

            }
            #endregion

            //更改总数为实际数据数量，开始真实数据导入
            await _fileImport.ChangeTotalCount(input.ImportKey, dataList.Count + treeCount);
            count = 0;

            try
            {
                //using var unow = _unitOfWorkManager.Begin(true, false);

                #region 开始往标准库写入产品数据(Sn_StdBasic_ProductCategory表)
                using var unitWork = _unitOfWorkManager.Begin(true, false);
                var productCodeList = new Dictionary<Guid, Guid>();//产品主键与树结构关系
                //获得数据库里所有产品数据列表
                _allProductList = _productCategoryRepository.Where(z => z.Id != Guid.Empty).ToList();
                //递归写入数据到产品表
                DgInsertToStandProductCategory(treeList, null, productCodeList, input);
                await unitWork.CompleteAsync();
                #endregion

                //using var unow = _unitOfWorkManager.Begin(true);
                #region 开始往标准库写入标准设备数据(Sn_StdBasic_Model表)
                using var unitWork2 = _unitOfWorkManager.Begin(true, false);
                List<Model> allModel; // 数据库中已经存在的数据。
                using (_dataFilter.Disable<ISoftDelete>())
                {
                    allModel = _modelRepository.Where(a => true).ToList();
                }
                //var allModel = _modelRepository.Where(z => z.Id != Guid.Empty).ToList();//获得所有标准设备数据
                var codeList = new List<string>();
                foreach (var item in dataList)
                {
                    //同一个code只允许保存一次
                    if (codeList.Any(z => z == item.Code)) continue;
                    codeList.Add(item.Code);
                    var isUpdate = true;//执行新增或更新标志
                    var stdModel = allModel.FirstOrDefault(z => z.CSRGCode == item.Code);
                    if (stdModel == null)//执行新增
                    {
                        isUpdate = false;
                        stdModel = new Model(Guid.NewGuid());

                        //设备编码
                        stdModel.Code = item.Code;
                        stdModel.CSRGCode = item.Code;

                        stdModel.CreationTime = DateTime.Now;
                        stdModel.CreatorId = CurrentUser.Id;
                        stdModel.DeleterId = CurrentUser.Id;
                        stdModel.DeletionTime = DateTime.Now;
                    }
                    stdModel.LastModificationTime = DateTime.Now;
                    stdModel.LastModifierId = CurrentUser.Id;

                    //设备名称
                    stdModel.Name = item.Spec;
                    stdModel.IsDeleted = false;
                    //设备关联构件编码
                    stdModel.ComponentCategoryId = null;
                    //设备关联产品编码
                    if (productCodeList.All(z => z.Key != item.ProductId))
                    {
                        failMsg.AppendLine(FormartErrMsg(rowNmb, "设备关联产品不存在"));
                        WrongInfo wrong = new WrongInfo(item.RowIndex, "设备关联产品不存在");
                        wrongInfos.Add(wrong);
                        continue;
                    }
                    stdModel.ProductCategoryId = productCodeList[item.ProductId];
                    //设备厂家
                    stdModel.ManufacturerId = GetManufacturerId(item.Manufacture);//根据厂家名称获取厂家主键
                    if (!stdModel.ManufacturerId.HasValue)
                    {
                        failMsg.AppendLine(FormartErrMsg(rowNmb, "设备关联厂家不存在"));
                        WrongInfo wrong = new WrongInfo(item.RowIndex, "设备关联厂家不存在");
                        wrongInfos.Add(wrong);
                        continue;
                    }

                    if (isUpdate)
                    {
                        await _modelRepository.UpdateAsync(stdModel);
                    }
                    else
                    {
                        await _modelRepository.InsertAsync(stdModel);
                    }
                    await _fileImport.UpdateState(input.ImportKey, treeCount + dataList.IndexOf(item));
                }
                await unitWork2.CompleteAsync();
                #endregion

                //强行设置进度为99 等待数据库保存数据
                //await _fileImport.UpdateState(input.ImportKey, (dataList.Count * 2) - 1);
                //数据库保存完数据后 设置为100%
                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
            catch (Exception e)
            {
                await _fileImport.Cancel(input.ImportKey);
            }
        }

        /// <summary>
        /// 构件树结构获取当前节点父级节点
        /// </summary>
        private readonly string _partCode = "SPC.";
        private List<ProductCategory> _allProductList;
        private ProductTree GetNodesParentNode(List<ProductTree> treeList, ProductItem thisNode)
        {
            var arr = thisNode.ParentName.Split('✪');
            var node = (ProductTree)null;
            var index = 0;
            if (string.IsNullOrEmpty(thisNode.ParentName))
            {
                node = treeList.FirstOrDefault(z => z.Name == thisNode.Name);
                return node;
            }

            if (arr.Length > index)//第一级
            {
                node = treeList.FirstOrDefault(z => z.Name == arr[index]);
            }

            for (var i = 0; i < 20; i++)
            {
                index++;
                if (index > arr.Length) break;
                if (arr.Length > index && node != null)//第二级及其子集
                {
                    node = node.Child.FirstOrDefault(z => z.Name == arr[index]);
                    if (node == null) break;
                }
            }

            return node;
        }

        /// <summary>
        /// 递归写入标准库产品数据
        /// </summary>
        private async void DgInsertToStandProductCategory(List<ProductTree> treeData, Guid? parentId, Dictionary<Guid, Guid> dic, ImportData input)
        {
            var forData = treeData.OrderBy(z => z.Name).ToList();
            var model = (ProductCategory)null;
            foreach (var item in forData)
            {
                model = _allProductList.FirstOrDefault(z => z.ParentId == parentId && z.Name == item.Name && z.Unit == item.Unit);
                if (model == null)
                {
                    model = new ProductCategory(Guid.NewGuid());
                    model.ParentId = parentId;
                    model.CreationTime = DateTime.Now;
                    model.CreatorId = CurrentUser.Id;
                    model.LastModificationTime = DateTime.Now;
                    model.LastModifierId = CurrentUser.Id;
                    model.DeleterId = CurrentUser.Id;
                    model.DeletionTime = DateTime.Now;
                    model.Name = item.Name;
                    model.Unit = item.Unit;
                    model.Code = GetProductCode(parentId);//获得产品编码
                    model.ExtendName = model.Name;
                    model.ExtendCode = model.Code;
                    _allProductList.Add(model);
                    await _productCategoryRepository.InsertAsync(model);
                }
                count++;
                await _fileImport.UpdateState(input.ImportKey, count);
                DgInsertToStandProductCategory(item.Child, model.Id, dic, input);
                dic[item.Id] = model.Id;
            }
        }

        /// <summary>
        /// 获得一个新产品编码
        /// </summary>
        private string GetProductCode(Guid? parentId)
        {
            var parentModle = (ProductCategory)null;
            if (parentId.HasValue)
            {
                parentModle = _allProductList.FirstOrDefault(z => z.Id == parentId);
                if (parentModle == null) return null;
            }

            var model = _allProductList.Where(z => z.ParentId == parentId).OrderByDescending(z => z.Code).FirstOrDefault();//找到同级最大的编号
            if (model == null)
            {
                if (!parentId.HasValue)
                {
                    return _partCode + "001";
                }
                else
                {
                    return parentModle.Code + ".001";
                }
            }
            var arr = model.Code.Split('.');

            var val = (int)0;
            if (!int.TryParse(arr.Last(), out val))
            {
                return null;
            }
            val++;
            var code = "";
            if (arr.Length > 2)
            {
                for (var i = 0; i < arr.Length - 1; i++)
                {
                    code += arr[i] + ".";
                }
            }
            else
            {
                code = this._partCode;
            }
            code += val.ToString().PadLeft(3, '0');
            return code.Trim('.');
        }

        /// <summary>
        /// 根据厂家名称获得厂家主键
        /// </summary>
        private List<Manufacturer> _manufacturerList;//厂家列表
        private Guid? GetManufacturerId(string manufacturerName)
        {
            //获得所有厂家列表
            if (_manufacturerList == null)
            {
                _manufacturerList = _manufacturerRepository.Where(z => z.Id != Guid.Empty).ToList();
                if (_manufacturerList.Count == 0) throw new UserFriendlyException("未在标准库中找到厂家数据");
            }
            var manufacture = this._manufacturerList.FirstOrDefault(z => z.Name == manufacturerName);
            if (manufacture != null)
            {
                return manufacture.Id;
            }
            return null;
        }

        /// <summary>
        /// 格式化导入存在的错误消息
        /// </summary>
        private string FormartErrMsg(string rowIndex, string msg)
        {
            return string.Format("序号为{0}的数据导入失败:{1}\r\n", rowIndex, msg);
        }
        #endregion

        #region 数据导入（旧）

        //需要导入的数据
        List<ImportProductTemplate> Products = new List<ImportProductTemplate>();
        List<Model> AddModels = new List<Model>();
        List<Model> UpdateModels = new List<Model>();
        const string CodePrefix = "SPC.";
        int successAddPeoductCateCount = 0;
        int successAddModelCount = 0;
        int successUpadteModelCount = 0;
        StringBuilder failMsg = new StringBuilder();
        StringBuilder modelFailMsg = new StringBuilder();

        List<ProductCategoryDto> allProductCate = new List<ProductCategoryDto>();
        List<Manufacturer> allManufacture = new List<Manufacturer>();
        List<Model> allModels = new List<Model>();
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> Import([FromForm] ImportData input)
        {
            Products = new List<ImportProductTemplate>();
            AddModels = new List<Model>();
            UpdateModels = new List<Model>();
            successAddPeoductCateCount = 0;
            successAddModelCount = 0;
            successUpadteModelCount = 0;
            failMsg = new StringBuilder();
            modelFailMsg = new StringBuilder();
            DataSet ds = new DataSet();
            try
            {
                ds = ExcelHelper.ImportBaseDataToDataSet(input.File.File.OpenReadStream(), input.File.File.FileName);
            }
            catch (Exception)
            {
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }
            if (ds == null) throw new UserFriendlyException("未找到任何数据,请检查文件格式");

            var allProductCateEnt = _productCategoryRepository.WithDetails().Where(s => s.IsDeleted == false && s.ExtendCode != null).ToList();
            allProductCate = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(allProductCateEnt);
            SetProduceCateFullName(allProductCate);
            allManufacture = _manufacturerRepository.Where(s => s.IsDeleted == false).ToList();
            allModels = _modelRepository.WithDetails().Where(s => !s.IsDeleted).ToList();
            await _fileImport.Start(input.ImportKey, ds.Tables.Count);
            foreach (DataTable dt in ds.Tables)
            {
                List<string> columnNames = new List<string>();
                foreach (DataColumn item in dt.Columns)
                {
                    columnNames.Add(item.ColumnName);
                }
                #region 验证关键列                
                if (!columnNames.Contains(ImportCol.SeenSun.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.SeenSun.ToString() + "不存在。"); continue; };
                if (!columnNames.Contains(ImportCol.CSRGCode.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.CSRGCode.ToString() + "不存在。"); continue; }
                if (!columnNames.Contains(ImportCol.IFDParent.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.IFDParent.ToString() + "不存在。"); continue; }
                if (!columnNames.Contains(ImportCol.IFD.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.IFD.ToString() + "不存在。"); continue; }
                if (!columnNames.Contains(ImportCol.Spec.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.Spec.ToString() + "不存在。"); continue; }
                if (!columnNames.Contains(ImportCol.Unit.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.Unit.ToString() + "不存在。"); continue; }
                if (!columnNames.Contains(ImportCol.Manufacture.ToString())) { failMsg.Append("表" + dt.TableName + "导入失败，列" + ImportCol.Manufacture.ToString() + "不存在。"); continue; }
                #endregion


                // 组织 产品分类 以及 标准设备 数据
                var allRows = dt.Select();

                //对数据按照根级节点名称进行分组
                var rootNodes = allRows.Where(s => s[ImportCol.IFDParent.ToString()].ToString().Contains('-') == false).Select(s => s[ImportCol.IFDParent.ToString()].ToString()).Distinct().ToList();
                List<GroupImportData> groupDatas = new List<GroupImportData>();
                var actions = new Action[rootNodes.Count()];
                for (int i = 0; i < rootNodes.Count; i++)
                {
                    var item = rootNodes[i];

                    GroupImportData g = new GroupImportData();
                    g.GroupName = item;
                    g.DataRows = allRows.Where(s => s[ImportCol.IFDParent.ToString()].ToString().StartsWith(item)).ToList();
                    actions[i] = () => CreateProductsAndModels(g.ImportProductTemplates, g.AddModels, g.UpdateModels, g.DataRows, g.FailMsg, g.ModelFailMsg);
                    groupDatas.Add(g);
                }
                //不同的组各自并行进行处理
                Parallel.Invoke(actions);
                //汇总Products AddModels UpdateModels错误信息等
                foreach (var item in groupDatas)
                {
                    Products.AddRange(item.ImportProductTemplates);
                    AddModels.AddRange(item.AddModels);
                    UpdateModels.AddRange(item.UpdateModels);
                    failMsg.Append(item.FailMsg);
                    modelFailMsg.Append(item.ModelFailMsg);
                }

                int maxRootNodeCodeIndex = 0;
                //根据数据库已有数据修改Code以及其子项的code以及父子级id 或者生成新的code以及id
                var existRootNodes = allProductCate.Where(s => s.ExtendCode.Split('.').Length == 2).OrderByDescending(s => s.ExtendCode);
                if (existRootNodes.Count() > 0)
                {
                    var maxRootNode = existRootNodes.First();
                    var flag = maxRootNode.ExtendCode.LastIndexOf('.');
                    maxRootNodeCodeIndex = int.Parse(maxRootNode.ExtendCode.Substring(flag + 1, maxRootNode.ExtendCode.Length - 1 - flag));
                }
                int splitCount = 2;
                foreach (var item in Products.Where(s => s.ProductCategory.ExtendCode.Split('.').Length == splitCount))
                {
                    //此数据为新数据
                    if (item.IsOldData == false)
                    {
                        maxRootNodeCodeIndex++;
                        item.ProductCategory.ExtendCode = item.ProductCategory.ExtendCode.Substring(0, item.ProductCategory.ExtendCode.Length - CodeLength) + maxRootNodeCodeIndex.ToString().PadLeft(CodeLength, '0');
                        item.ProductCategory.Code = item.ProductCategory.ExtendCode;
                    }

                    SetChildNodeExtendCode(Products, splitCount, item);
                }

                //插入数据
                foreach (var item in Products)
                {
                    if (!item.IsOldData)
                    {
                        var dto = item.ProductCategory;
                        ProductCategory category = new ProductCategory(dto.Id);
                        category.Code = dto.Code;
                        category.ExtendCode = dto.ExtendCode;
                        category.ExtendName = dto.ExtendName;
                        category.LevelName = dto.LevelName;
                        category.Name = dto.Name;
                        category.ParentId = dto.ParentId;
                        category.Unit = dto.Unit;
                        ValidationMaxlength.Validate(category);
                        await _productCategoryRepository.InsertAsync(category);
                        successAddPeoductCateCount++;
                    }
                }
                foreach (var item in AddModels)
                {
                    ValidationMaxlength.Validate(item);
                    await _modelRepository.InsertAsync(item);
                    successAddModelCount++;
                }

                //更新数据
                foreach (var item in UpdateModels)
                {
                    await _modelRepository.UpdateAsync(item);
                    successUpadteModelCount++;
                }
                await _fileImport.UpdateState(input.ImportKey, ds.Tables.IndexOf(dt));
            }
            string res = string.Format("成功导入{0}条产品分类数据，{1}条产品型号数据，更新{2}条已有产品型号。", successAddPeoductCateCount, successAddModelCount, successUpadteModelCount);
            res += "\r\n" + failMsg.ToString() + "\r\n" + modelFailMsg.ToString();
            await _fileImport.Complete(input.ImportKey);
            return res;
        }

        /// <summary>
        /// 组织表格数据
        /// </summary>
        /// <param name="products"></param>
        /// <param name="addModels"></param>
        /// <param name="updateModels"></param>
        /// <param name="allRows">行数据集合</param>
        /// <param name="failMsg"></param>
        /// <param name="modelFailMsg"></param>
        private void CreateProductsAndModels(List<ImportProductTemplate> products, List<Model> addModels, List<Model> updateModels, List<DataRow> allRows, StringBuilder failMsg, StringBuilder modelFailMsg)
        {
            foreach (DataRow item in allRows)
            {
                var number = item[ImportCol.SeenSun.ToString()].ToString();
                var csrgCode = item[ImportCol.CSRGCode.ToString()].ToString();
                var ifdParent = item[ImportCol.IFDParent.ToString()].ToString();
                var ifd = item[ImportCol.IFD.ToString()].ToString();
                var spec = item[ImportCol.Spec.ToString()].ToString();
                var unit = item[ImportCol.Unit.ToString()].ToString();
                var manufacture = item[ImportCol.Manufacture.ToString()].ToString();
                //产品分类 产品名称 产品型号处理
                if (csrgCode == null) { failMsg.Append("第" + number + "行数据导入失败，原因：编码为空。\r\n"); continue; }
                if (ifdParent == null) { failMsg.Append("第" + number + "行数据导入失败，原因：产品分类为空。\r\n"); continue; }
                if (ifd == null) { failMsg.Append("第" + number + "行数据导入失败，原因：产品名称为空。\r\n"); continue; }
                if (spec == null) { modelFailMsg.Append("第" + number + "行数据导入失败，原因：产品型号为空。\r\n"); continue; }
                if (manufacture == null) { modelFailMsg.Append("第" + number + "行数据导入失败，原因：产品厂商为空。\r\n"); continue; }
                Guid? manufactureId = allManufacture.FirstOrDefault(s => s.Name == manufacture)?.Id;

                string[] ifdNames = ifdParent.Split('-');
                List<string> tempNames = ifdNames.ToList();
                tempNames.AddRange(new List<string> { ifd, spec });
                ifdNames = tempNames.ToArray();
                for (int i = 0; i < ifdNames.Length; i++)
                {
                    string eleName = ifdNames[i];
                    string remark = "";
                    for (int j = 0; j <= i; j++)
                    {
                        remark += ifdNames[j] + "@";
                    }
                    remark = remark == "" ? "" : remark.Substring(0, remark.Length - 1);

                    //不存在则进行添加 条件 上级名称 名称相同，编码.分隔后 数组个数与当前名称层级一致
                    var sameNode = products.FirstOrDefault(s => (s.IsOldData ? s.ProductCategory.FullName == remark : s.ProductCategory.Remark == remark) && s.ProductCategory.ExtendName == eleName && s.ProductCategory.ExtendCode.Split('.').Length == i + 2);
                    if (sameNode == null)
                    {
                        //数据库现有数据存在 不需要添加
                        var sameNameNode = allProductCate.Where(s => s.ExtendName == eleName && s.ExtendCode.Split('.').Length == i + 2);
                        var sameFullPathNode = sameNameNode.FirstOrDefault(s => s.FullName == remark);
                        if (sameFullPathNode != null)
                        {
                            products.Add(new ImportProductTemplate(sameFullPathNode, true));

                            if (i == ifdNames.Length - 1) //叶子节点 关联标准设备
                            {
                                if (manufactureId == null)
                                {
                                    modelFailMsg.Append("第" + number + "行数据导入失败，原因：产品厂商不存在。\r\n");
                                    continue;
                                }
                                AddOrUpdateModel(allModels, addModels, updateModels, csrgCode, spec, manufactureId, sameFullPathNode.Id);

                            }
                            continue;
                        }

                        //数据库现有数据不存在 需要添加
                        int code = 0;//当前编码
                        string parentCode = CodePrefix; //父级编码
                        string parentName = i > 0 ? ifdNames[i - 1] : ""; //父级名称
                        if (i > 0)
                        {
                            var parentNode = products.FirstOrDefault(s => s.ProductCategory.ExtendName == parentName && s.ProductCategory.ExtendCode.Split('.').Length == i + 1);
                            if (parentNode != null)
                            {
                                parentCode = parentNode.ProductCategory.ExtendCode + '.';
                            }
                            else
                            {
                                parentCode = CodePrefix;
                            }
                        }
                        //查找层级相同的节点的最大编码
                        var addedCodeEnt = products.Where(s => s.ProductCategory.ExtendCode.StartsWith(parentCode) && s.ProductCategory.ExtendCode.Split('.').Length == i + 2).OrderByDescending(s => s.ProductCategory.ExtendCode);
                        var addMaxCodeEnt = addedCodeEnt.Count() > 0 ? addedCodeEnt.First() : null;
                        if (addMaxCodeEnt != null)
                        {
                            var lastPointIndex = addMaxCodeEnt.ProductCategory.ExtendCode.LastIndexOf('.');
                            code = int.Parse(addMaxCodeEnt.ProductCategory.ExtendCode.Substring(lastPointIndex + 1, addMaxCodeEnt.ProductCategory.ExtendCode.Length - 1 - lastPointIndex));
                        }
                        ProductCategoryDto temp = new ProductCategoryDto();
                        temp.Id = _guidGenerator.Create();
                        temp.Name = eleName;
                        temp.ExtendName = eleName;
                        temp.ExtendCode = parentCode + (++code).ToString().PadLeft(CodeLength, '0');
                        temp.Code = temp.ExtendCode;
                        temp.Remark = remark;
                        if (i == ifdNames.Length - 1)//叶子节点 关联标准设备
                        {
                            temp.Unit = unit;

                            if (manufactureId == null)
                            {
                                modelFailMsg.Append("第" + number + "行数据导入失败，原因：产品厂商不存在。\r\n");
                                continue;
                            }

                            AddOrUpdateModel(allModels, addModels, updateModels, csrgCode, spec, manufactureId, temp.Id);
                        }
                        products.Add(new ImportProductTemplate(temp, false));
                    }
                    else
                    {
                        if (i == ifdNames.Length - 1) //叶子节点 关联标准设备
                        {
                            if (manufactureId == null)
                            {
                                modelFailMsg.Append("第" + number + "行数据导入失败，原因：产品厂商不存在。\r\n");
                                continue;
                            }
                            AddOrUpdateModel(allModels, addModels, updateModels, csrgCode, spec, manufactureId, sameNode.ProductCategory.Id);
                        }
                    }
                }
            }
        }

        private static void SetChildNodeExtendCode(List<ImportProductTemplate> Products, int splitCount, ImportProductTemplate item)
        {
            int childNodeStartIndex = 0;
            //若存在旧数据 且有子项 获取最后子项的code顺序值
            if (item.IsOldData && item.ProductCategory.Children?.Count > 0)
            {
                var lastedChildNode = item.ProductCategory.Children.OrderByDescending(s => s.ExtendCode).First().ExtendCode;
                var p = lastedChildNode.LastIndexOf('.');
                childNodeStartIndex = int.Parse(lastedChildNode.Substring(p + 1, lastedChildNode.Length - 1 - p));
            }

            //设置子项
            var fullName = (item.IsOldData ? item.ProductCategory.FullName : item.ProductCategory.Remark) + "@";
            var childs = Products.Where(s => (s.IsOldData ? s.ProductCategory.FullName.StartsWith(fullName) : s.ProductCategory.Remark.StartsWith(fullName)) && s.ProductCategory.ExtendCode.Split('.').Length == splitCount + 1);
            foreach (var ch in childs)
            {
                if (ch.IsOldData == false)//旧数据不改变code
                {
                    ch.ProductCategory.ParentId = item.ProductCategory.Id;
                    ch.ProductCategory.ExtendCode = item.ProductCategory.ExtendCode + '.' + (++childNodeStartIndex).ToString().PadLeft(CodeLength, '0');
                    ch.ProductCategory.Code = ch.ProductCategory.ExtendCode;
                }

                SetChildNodeExtendCode(Products, (splitCount + 1), ch);
            }
        }


        /// <summary>
        /// 获取节点的全路径
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        string FindFullName(ProductCategoryDto node)
        {
            if (node.Parent != null)
            {
                return FindFullName(node.Parent) + node.ExtendName + "@";
            }
            else
            {
                return node.ExtendName + "@";
            }
        }

        /// <summary>
        /// 设置节点的全路径
        /// </summary>
        /// <param name="allProductCate"></param>
        private void SetProduceCateFullName(List<ProductCategoryDto> allProductCate)
        {
            foreach (var item in allProductCate)
            {
                var re = FindFullName(item);
                re = string.IsNullOrEmpty(re) ? "" : re.Substring(0, re.Length - 1);
                item.FullName = re;
            }
        }

        /// <summary>
        /// 对model进行更新 或添加新的model至待添加的Models中
        /// </summary>
        /// <param name="allModels">所有的model</param>
        /// <param name="AddModels">待添加的model集合</param>
        /// <param name="csrgCode"></param>
        /// <param name="spec"></param>
        /// <param name="manufactureId"></param>
        /// <param name="productCategoryId"></param>
        /// <param name="UpdateModels"></param>
        /// <returns></returns>
        private void AddOrUpdateModel(List<Model> allModels, List<Model> AddModels, List<Model> UpdateModels, string csrgCode, string spec, Guid? manufactureId, Guid productCategoryId)
        {
            //已存在的model
            var existModel = allModels.FirstOrDefault(s => s.CSRGCode == csrgCode);
            if (existModel != null)
            {
                existModel.Name = spec;
                existModel.ManufacturerId = manufactureId;
                //await _modelRepository.UpdateAsync(existModel);
                UpdateModels.Add(existModel);
            }
            else
            {
                Model model = new Model(_guidGenerator.Create());
                model.Name = spec;
                model.ProductCategoryId = productCategoryId;
                model.ManufacturerId = manufactureId;
                model.CSRGCode = csrgCode;
                AddModels.Add(model);
            }
        }

        #endregion

        #region 数据导出
        public async Task<Stream> Export(ModelData input)
        {
            /*List<Model> list;
            if (input.Paramter.ProductCategoryId != null)
            {*/
            var list = GetModelsData(input.Paramter);
            /*            }
                        else
                        {
                            list = await _modelRepository.GetListAsync();
                        }*/

            var proList = await _productCategoryRepository.GetListAsync();
            var manList = await _manufacturerRepository.GetListAsync();

            var models = new List<StandardEquipmentModel>();
            var model = new StandardEquipmentModel();
            foreach (var item in list)
            {
                model = new StandardEquipmentModel();
                if (item.ManufacturerId != null && item.ManufacturerId != Guid.Empty)
                {
                    var manufacturer = manList.FirstOrDefault(x => x.Id == item.ManufacturerId);
                    model.Manufacture = manufacturer?.Name;
                }
                if (item.ProductCategoryId != null && item.ProductCategoryId != Guid.Empty)
                {
                    var pros = proList.FirstOrDefault(x => x.Id == item.ProductCategoryId);
                    model.Unit = pros.Unit;
                    if (pros.ParentId != null && pros.ParentId != Guid.Empty)
                    {
                        pros = proList.FirstOrDefault(x => x.Id == pros.ParentId);
                        model.IFD = pros.Name;
                        if (pros.ParentId != null && pros.ParentId != Guid.Empty)
                        {
                            GetParentProductName(proList, (Guid)pros.ParentId, model);
                        }
                    }
                }
                model.CSRGCode = item.CSRGCode;
                model.Spec = item.Name;
                models.Add(model);
            }

            var stream = ExcelHelper.ExcelExportStream(models, input.TemplateKey, input.RowIndex);
            return stream;
        }
        #endregion

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.StandardEquipment.Update)]
        public async Task<StandardEquipmentDto> Update(StandardEquipmentUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("设备名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("设备编码不合法");
            }
            if (_modelRepository.Where(z => !string.IsNullOrEmpty(input.Code) && z.Code == input.Code && z.Id != input.Id).FirstOrDefault() != null) throw new UserFriendlyException("设备Code编码已存在");

            if (input.ManufacturerId == null || input.ManufacturerId == Guid.Empty) throw new UserFriendlyException("产品厂商不能为空！");

            if ((input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) && (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty)) throw new UserFriendlyException("产品类型和构件类型不能同时为空！");

            if (!_manufacturerRepository.Any(z => z.Id == input.ManufacturerId)) throw new UserFriendlyException("供应商不存在");
            var befor = _modelRepository.FirstOrDefault(x=>x.Name==input.Name&&x.ManufacturerId==input.ManufacturerId&&
            (x.ProductCategoryId.HasValue&& x.ProductCategoryId == input.ProductCategoryId||(!x.ProductCategoryId.HasValue&&x.ComponentCategoryId.HasValue&&x.ComponentCategoryId==input.ComponentCategoryId))
            &&x.Id!=input.Id);
            if(befor!=null)
            {
                throw new UserFriendlyException("已经有相同的标准设备数据，请检查");
            }
            var model = await _modelRepository.GetAsync(input.Id);
            if (model == null) throw new UserFriendlyException("更新标准设备不存在");
            model.Name = input.Name;
            model.Code = input.Code;
            model.CSRGCode = input.CSRGCode;
            model.ProductCategoryId = input.ProductCategoryId;
            model.ComponentCategoryId = input.ComponentCategoryId;
            model.ManufacturerId = input.ManufacturerId;
            model.ServiceLife = input.ServiceLife;
            model.ServiceLifeUnit = input.ServiceLifeUnit;

            await _modelRepository.UpdateAsync(model);

            return ObjectMapper.Map<Model, StandardEquipmentDto>(model);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.StandardEquipment.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var model = _modelRepository.FirstOrDefault(s => s.Id == id);
            if (model == null) throw new UserFriendlyException("此设备不存在");
            await _modelRepository.DeleteAsync(id);
            return true;
        }


        private void GetParentProductName(List<ProductCategory> proList, Guid id, StandardEquipmentModel model)
        {
            var IFDparent = proList.FirstOrDefault(x => x.Id == id);
            model.IFDParent = IFDparent.Name + "-" + model.IFDParent;
            if (IFDparent.ParentId != null && IFDparent.ParentId != Guid.Empty)
            {
                GetParentProductName(proList, (Guid)IFDparent.ParentId, model);
            }
            else
            {
                model.IFDParent = model.IFDParent.TrimEnd('-');
            }
        }

        private List<Model> GetModelsData(StandardEquipmentSearchInputDto input)
        {
            var query = _modelRepository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Manufacturer.Name.Contains(input.Keywords) || x.Code.Contains(input.Keywords) || x.CSRGCode.Contains(input.Keywords))
                .WhereIf(input.ProductCategoryId != null && input.ProductCategoryId != Guid.Empty,
                x => x.ProductCategoryId == input.ProductCategoryId ||
                x.ProductCategory.ParentId == input.ProductCategoryId ||
                x.ProductCategory.Parent.ParentId == input.ProductCategoryId);

            return query.ToList();
        }

        //#endregion

        ///// <summary>
        ///// 导入数据
        ///// </summary>
        ///// <param name="files"></param>
        ///// <returns></returns>
        //public async Task<bool> Import([FromForm] FileUploadDto files)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        try
        //        {
        //            dt = ExcelToDataTable(files.File.OpenReadStream(), files.File.FileName, 5);
        //        }
        //        catch (Exception)
        //        {
        //            throw new UserFriendlyException("所选文件有错误，请重新选择");
        //        }
        //        if (dt == null) throw new UserFriendlyException("未读取到任何数据");
        //        if (dt.Rows.Count == 0) throw new UserFriendlyException("未读取到任何数据");
        //        var celVal = (string)null;
        //        //获取数据库数据
        //        var modelList = _service.Where(z => true).ToList();
        //        var facList = _manufacturerService.Where(z => true).ToList();
        //        //设备厂家添加列表
        //        var addFacList = new List<Manufacturer>();
        //        //标准设备添加列表
        //        var addModelList = new List<StandardEquipment>();
        //        //标准设备修改列表
        //        var updateModelList = new List<StandardEquipment>();
        //        //标准设备删除列表
        //        var deleteModelList = new List<StandardEquipment>();
        //        deleteModelList.AddRange(modelList);
        //        await Task.Run(() =>
        //        {
        //            //获取数据
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                StandardEquipment standModel = new StandardEquipment(Guid.NewGuid());
        //                //编码
        //                celVal = row[2].ToString();
        //                standModel.EquipmentCode = celVal;
        //                //名称
        //                celVal = row[5].ToString();
        //                standModel.Name = celVal;

        //                //单位
        //                celVal = row[6].ToString();
        //                standModel.Unit = celVal;

        //                //厂家
        //                celVal = row[7].ToString();
        //                Manufacturer fac = new Manufacturer(Guid.NewGuid());
        //                fac.Name = celVal;
        //                if (facList?.Count > 0)
        //                {
        //                    var beforFac = facList.Find(m => m.Name == fac.Name);
        //                    if (beforFac != null)
        //                    {
        //                        fac = beforFac;
        //                    }
        //                    else
        //                    {
        //                        var addfac = addFacList.Find(x => x.Name == fac.Name);
        //                        if (addfac != null)
        //                        {
        //                            addFacList.Add(fac);
        //                        }
        //                        else
        //                        {
        //                            fac = addfac;
        //                        }
        //                    }
        //                }
        //                standModel.ManufacturerId = fac.Id;
        //                //标准设备
        //                if (modelList?.Count > 0)
        //                {
        //                    var model = modelList.Find(m => m.Name == standModel.Name && m.ManufacturerId == standModel.ManufacturerId);
        //                    if (model == null)
        //                    {
        //                        var addModel = addModelList.Find(m => m.Name == standModel.Name && m.ManufacturerId == standModel.ManufacturerId);
        //                        if (addModel == null)
        //                        {
        //                            addModelList.Add(standModel);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (model.IsDelete == true)
        //                        {
        //                            model.IsDelete = false;
        //                            updateModelList.Add(model);
        //                        }
        //                        if (model.EquipmentCode != standModel.EquipmentCode)
        //                        {
        //                            var update = updateModelList.Find(x => x.Id == model.Id);
        //                            if (update != null)
        //                            {
        //                                update.EquipmentCode = standModel.EquipmentCode;
        //                            }
        //                            else
        //                            {
        //                                model.EquipmentCode = standModel.EquipmentCode;
        //                                updateModelList.Add(model);
        //                            }
        //                        }

        //                        deleteModelList.RemoveAll(m => m.Id == model.Id);
        //                    }
        //                }
        //                else
        //                {
        //                    var addModel = addModelList.Find(m => m.Name == standModel.Name && m.ManufacturerId == standModel.ManufacturerId);
        //                    if (addModel == null)
        //                    {
        //                        addModelList.Add(standModel);
        //                    }
        //                }


        //            }
        //            //添加设备厂家
        //            if (addFacList?.Count > 0)
        //            {
        //                addFacList.ForEach(m =>
        //                {
        //                    _manufacturerService.InsertAsync(m);
        //                });
        //            }
        //            //添加标准设备
        //            if (addModelList?.Count > 0)
        //            {
        //                addModelList.ForEach(m =>
        //                {
        //                    _service.InsertAsync(m);
        //                });
        //            }
        //            //修改标准设备
        //            if (updateModelList?.Count > 0)
        //            {
        //                updateModelList.ForEach(m =>
        //                {
        //                    _service.UpdateAsync(m);
        //                });
        //            }
        //            //删除标准设备
        //            if (deleteModelList?.Count > 0)
        //            {
        //                deleteModelList.ForEach(m =>
        //                {
        //                    m.IsDelete = true;
        //                    _service.UpdateAsync(m);
        //                });
        //            }
        //        });
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new UserFriendlyException(e.Message);
        //    }



        //}

        //#region Excel文件导出DataTable===========================================
        ///// <summary>
        ///// Excel数据导出DataTable
        ///// </summary>
        //private DataTable ExcelToDataTable(Stream fs, string fileName, int startRow = 0, string sheetName = null)
        //{
        //    DataTable dataTable = null;
        //    DataColumn column = null;
        //    DataRow dataRow = null;
        //    IWorkbook workbook = null;
        //    ISheet sheet = null;
        //    IRow row = null;
        //    ICell cell = null;
        //    try
        //    {
        //        // 2007版本
        //        if (fileName.IndexOf(".xlsx") > 0)
        //            workbook = new XSSFWorkbook(fs);
        //        // 2003版本
        //        else if (fileName.IndexOf(".xls") > 0)
        //            workbook = new HSSFWorkbook(fs);
        //        else
        //        {
        //            return null;
        //        }

        //        if (workbook != null)
        //        {
        //            sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet
        //            dataTable = new DataTable();
        //            if (sheet != null)
        //            {
        //                var rowCount = sheet.LastRowNum;//总行数
        //                if (rowCount > 0)
        //                {
        //                    IRow firstRow = sheet.GetRow(0);//第一行
        //                    int cellCount = firstRow.LastCellNum;//列数

        //                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
        //                    {
        //                        cell = firstRow.GetCell(i);
        //                        if (cell != null)
        //                        {
        //                            switch (cell.CellType)
        //                            {
        //                                case CellType.String:
        //                                    {
        //                                        column = new DataColumn(cell.StringCellValue);
        //                                        dataTable.Columns.Add(column);
        //                                        break;
        //                                    }
        //                                case CellType.Numeric:
        //                                    {
        //                                        column = new DataColumn(cell.NumericCellValue.ToString());
        //                                        dataTable.Columns.Add(column);
        //                                        break;
        //                                    }
        //                                default:
        //                                    {
        //                                        throw new UserFriendlyException("导入的内容数据格式错误");
        //                                    }
        //                            }
        //                        }
        //                    }


        //                    //填充行
        //                    for (int i = startRow; i <= rowCount; ++i)
        //                    {
        //                        row = sheet.GetRow(i);
        //                        if (row == null) continue;

        //                        dataRow = dataTable.NewRow();
        //                        for (int j = row.FirstCellNum; j < cellCount; ++j)
        //                        {
        //                            cell = row.GetCell(j);
        //                            if (cell == null)
        //                            {
        //                                dataRow[j] = "";
        //                            }
        //                            else
        //                            {
        //                                //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)
        //                                switch (cell.CellType)
        //                                {
        //                                    case CellType.Blank:
        //                                        dataRow[j] = "";
        //                                        break;
        //                                    case CellType.Numeric:
        //                                        short format = cell.CellStyle.DataFormat;
        //                                        //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
        //                                        if (format == 14 || format == 31 || format == 57 || format == 58)
        //                                            dataRow[j] = cell.DateCellValue;
        //                                        else
        //                                            dataRow[j] = cell.NumericCellValue;
        //                                        break;
        //                                    case CellType.String:
        //                                        dataRow[j] = cell.StringCellValue;
        //                                        break;
        //                                }
        //                            }
        //                        }
        //                        dataTable.Rows.Add(dataRow);
        //                    }
        //                }
        //            }
        //        }
        //        return dataTable;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (fs != null)
        //        {
        //            fs.Close();
        //        }
        //        return null;
        //    }
        //}
        //#endregion
    }

    /// <summary>
    /// 产品分类导入模板
    /// </summary>
    public class ImportProductTemplate
    {
        public ImportProductTemplate(ProductCategoryDto productCate, bool isOldData)
        {
            ProductCategory = productCate;
            IsOldData = isOldData;
        }

        public ProductCategoryDto ProductCategory { get; set; }
        public bool IsOldData { get; set; }
    }

    /// <summary>
    /// 分组导入数据模板
    /// </summary>
    public class GroupImportData
    {
        public string GroupName { get; set; }
        public List<ImportProductTemplate> ImportProductTemplates { get; set; } = new List<ImportProductTemplate>();
        public List<Model> AddModels { get; set; } = new List<Model>();
        public List<Model> UpdateModels { get; set; } = new List<Model>();
        public List<DataRow> DataRows { get; set; } = new List<DataRow>();
        public StringBuilder FailMsg { get; set; } = new StringBuilder();
        public StringBuilder ModelFailMsg { get; set; } = new StringBuilder();
    }
}
