using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NPOI.SS.UserModel;

using SnAbp.Common;
using SnAbp.Identity;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.StdBasic.Services
{
    /// <summary>
    /// 单项工程管理
    /// </summary>
    [Authorize]
    public class StdBasicIndividualProjectAppService : StdBasicAppService, IStdBasicIndividualProjectAppService
    {
        private readonly IRepository<IndividualProject, Guid> _repositoryIndividualProject;
        private readonly IRepository<ProjectItemRltIndividualProject, Guid> _repositoryProjectItemRltIndividualProject;
        private readonly IRepository<DataDictionary, Guid> _repositorySpecialty;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IDataFilter _dataFilter;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repositoryIndividualProject"></param>
        /// <param name="repositorySpecialty"></param>
        /// <param name="guidGenerator"></param>
        /// <param name="fileImport"></param>
        /// <param name="dataFilter"></param>
        /// <param name="unitOfWork"></param>
        public StdBasicIndividualProjectAppService(IRepository<IndividualProject, Guid> repositoryIndividualProject,
            IRepository<DataDictionary, Guid> repositorySpecialty,
            IRepository<ProjectItemRltIndividualProject, Guid> repositoryProjectItemRltIndividualProject,
        IGuidGenerator guidGenerator,
            IFileImportHandler fileImport,
            IDataFilter dataFilter, IUnitOfWorkManager unitOfWork

        )
        {
            _repositoryProjectItemRltIndividualProject = repositoryProjectItemRltIndividualProject;
            _repositoryIndividualProject = repositoryIndividualProject;
            _repositorySpecialty = repositorySpecialty;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 创建单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Create)]
        public async Task<IndividualProjectDto> Create(IndividualProjectCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("单项工程名称不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("单项工程编码不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("定额名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            var before = _repositoryIndividualProject.FirstOrDefault(x => x.Name == input.Name);
            if (before != null) throw new UserFriendlyException("单项工程已存在");
            if (!string.IsNullOrEmpty(input.Code))
            {
                var codeModel = _repositoryIndividualProject.FirstOrDefault(x => !string.IsNullOrEmpty(x.Code) && x.Code == input.Code);
                if (codeModel != null) throw new UserFriendlyException("编码已存在");
            }
            var individualProject = new IndividualProject(_guidGenerator.Create());
            individualProject.Name = input.Name;
            individualProject.ParentId = input.ParentId;
            individualProject.Code = input.Code;
            individualProject.SpecialtyId = input.SpecialtyId;

            individualProject.Remark = input.Remark;
            await _repositoryIndividualProject.InsertAsync(individualProject);
            return ObjectMapper.Map<IndividualProject, IndividualProjectDto>(individualProject);
        }

        /// <summary>
        /// 删除单项工程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            var individualProject = _repositoryIndividualProject.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (string.IsNullOrEmpty(individualProject.Name)) throw new UserFriendlyException("该单项工程不存在");
            //var children = _repositoryIndividualProject.WithDetails().Where(x => x.ParentId != null && x.ParentId == id);
            //if (children != null) throw new UserFriendlyException("请先删除该单项工程的下级分类");
            if (individualProject.Children.Count > 0) throw new UserFriendlyException("请先删除该单项工程的下级分类");
            var list = _repositoryProjectItemRltIndividualProject.Where(x => x.IndividualProjectId == id).ToList();
            if (list?.Count > 0) throw new UserFriendlyException("该单项工程关联工程工项，无法删除");
            await _repositoryIndividualProject.DeleteAsync(id);

            return true;
        }
        /// <summary>
        /// 导出单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Export)]
        public async Task<Stream> Export(IndividualProjectData input)
        {
            List<IndividualProject> list;
            if (input.Paramter.KeyWords != "" || input.Paramter.ParentId != null)
            {
                list = GetIndividualProjectData(input.Paramter).AsEnumerable().OrderBy(y => y.Code).ToList();
            }
            else
            {
                list = _repositoryIndividualProject.WithDetails().Where(x => x.Id != Guid.Empty).OrderBy(y => y.Code).ToList();
            }
            var dtoList = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectTemplate>>(list);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }

        /// <summary>
        /// 根据Id获取单项工程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Detail)]
        public async Task<IndividualProjectDto> Get(Guid id)
        {
            IndividualProjectDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryIndividualProject.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此单项工程不存在");
                result = ObjectMapper.Map<IndividualProject, IndividualProjectDto>(ent);
                if (ent.Specialty != null)
                {
                    result.SpecialtyName = ent.Specialty.Name;
                }

            });
            return result;
        }

        /// <summary>
        /// 获取单项工程树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<IndividualProjectDto>> GetList(IndividualProjectGetListByIdsDto input)
        {
            var result = new PagedResultDto<IndividualProjectDto>();

            var productCategories = new List<IndividualProject>();

            var dtos = new List<IndividualProjectDto>();

            if (input.Ids != null && input.Ids.Count > 0)
            {

                foreach (var id in input.Ids)
                {
                    var componentCategory = _repositoryIndividualProject.WithDetails().FirstOrDefault(x => x.Id == id);
                    if (componentCategory == null) continue;
                    productCategories.Add(componentCategory);
                    if (componentCategory.ParentId.HasValue && componentCategory.ParentId != Guid.Empty)
                    {
                        FindAllParents(componentCategory.ParentId.Value, ref productCategories);
                    }
                    var parentIds = productCategories.Select(s => s.Id).ToList();
                    var filterList = _repositoryIndividualProject.WithDetails()
                        .Where(x => parentIds.Contains(x.ParentId.Value) || x.ParentId == null)
                        .ToList();
                    productCategories.AddRange(filterList);
                }

                // 数据去重并转成dto
                var listDtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(productCategories.Distinct().ToList());

                //如果子集为空设置children为null
                foreach (var item in listDtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<IndividualProjectDto>();
                    }
                }

                dtos = GuidKeyTreeHelper<IndividualProjectDto>.GetTree(listDtos);
            }
            else //按需加载
            {
                //根据关键字查询
                if (!string.IsNullOrEmpty(input.KeyWords))
                {
                    productCategories = _repositoryIndividualProject.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                    dtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(productCategories);
                    foreach (var item in dtos)
                    {
                        item.Children = null;
                    }
                }

                else
                {
                    productCategories = _repositoryIndividualProject.WithDetails()
                        .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                        .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                        .ToList();

                    dtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(productCategories);

                    foreach (var item in dtos)
                    {

                        if (item.Children.Count == 0)
                        {
                            item.Children = null;
                        }
                        else
                        {
                            item.Children = new List<IndividualProjectDto>();
                        }
                    }
                }
            }

            result.TotalCount = dtos.Count();
            result.Items = input.IsAll ? dtos.OrderBy(x => x.Code).ToList()
                : dtos.OrderBy(x => x.Code).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return Task.FromResult(result);
        }

        /// <summary>
        /// 根据ids获得当前对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<IndividualProjectDto>> GetListIndividualProject(IndividualProjectGetListByIdsDto input)
        {
            var individualProducts = new List<IndividualProject>();
            var result = new PagedResultDto<IndividualProjectDto>();
            if (input.Ids == null)
                input.Ids = new List<Guid>();
            //获取当前对象实体
            if (input.Ids.Count > 0)
            {
                individualProducts = _repositoryIndividualProject.WithDetails().Where(x => input.Ids.Contains(x.Id)).ToList();
                var dtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(individualProducts);
                result.TotalCount = dtos.Count;
                result.Items = input.IsAll
                ? dtos.ToList()
                : dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }
            return Task.FromResult(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IndividualProjectDto> GetListCode(Guid? id)
        {
            var list = new List<IndividualProject>();
            if (id != null && id != Guid.Empty)
            {
                list = _repositoryIndividualProject.Where(x => x.ParentId == id).OrderBy(x => x.Code).ToList();
            }
            else
            {
                list = _repositoryIndividualProject.Where(x => x.ParentId == null).OrderBy(x => x.Code).ToList();
            }
            var dtos = Task.FromResult(ObjectMapper.Map<IndividualProject, IndividualProjectDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }

        /// <summary>
        /// 修改单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Update)]
        public async Task<IndividualProjectDto> Update(IndividualProjectUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入单项工程的id");
            var individualProject = await _repositoryIndividualProject.GetAsync(input.Id);
            if (individualProject == null) throw new UserFriendlyException("该单项工程不存在");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("单项工程名不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("单项工程编码不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            var before = _repositoryIndividualProject.FirstOrDefault(x => x.Name == input.Name && x.Id != input.Id);
            if (before != null) throw new UserFriendlyException("已存在同名单项工程");
            if (!string.IsNullOrEmpty(input.Code))
            {
                var codeModel = _repositoryIndividualProject.FirstOrDefault(x => !string.IsNullOrEmpty(x.Code) && x.Code == input.Code && x.Id != input.Id);
                if (codeModel != null) throw new UserFriendlyException("编码已存在");
            }
            individualProject.Name = input.Name;
            individualProject.ParentId = input.ParentId;
            individualProject.Code = input.Code;
            if (input.ParentId == null || input.ParentId == Guid.Empty)
            {
                List<IndividualProject> catelist = new List<IndividualProject>();
                if (individualProject.SpecialtyId != input.SpecialtyId)
                {
                    individualProject.SpecialtyId = input.SpecialtyId;

                    ChangeChildrens(individualProject, ref catelist);
                    if (catelist?.Count > 0)
                    {
                        foreach (var item in catelist)
                        {
                            await _repositoryIndividualProject.UpdateAsync(item);
                        }

                    }

                }
            }
            individualProject.Remark = input.Remark;
            await _repositoryIndividualProject.UpdateAsync(individualProject);
            return ObjectMapper.Map<IndividualProject, IndividualProjectDto>(individualProject);
        }

        /// <summary>
        /// 导入单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.IndividualProject.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<IndividualProjectTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<IndividualProjectTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<IndividualProjectTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            IndividualProject individualProjectModel;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var updateIndex = 1;

                foreach (var item in datalist.ToList())
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    var profession = _repositorySpecialty.Where(x => x.Name == item.SpecialtyName).FirstOrDefault();
                    if (item.Code.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"编码为空");
                    }
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"名称为空");
                    }
                    if (profession==null)
                    {
                        canInsert = false;
                        newInfo.AppendInfo($"此专业不存在");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    using var uow = _unitOfWork.Begin(true);
                    using (_dataFilter.Disable<ISoftDelete>())
                    {
                        individualProjectModel = _repositoryIndividualProject.FirstOrDefault(a => a.Code == item.Code && a.Name == item.Name);
                    }
                    if (individualProjectModel != null)
                    {
                        newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                        //修改自己本身数据
                        individualProjectModel.Name = item.Name;
                        individualProjectModel.Remark = item.Remark;
                        individualProjectModel.Code = item.Code;
                        if (profession != null)
                            individualProjectModel.SpecialtyId = profession.Id;
                        if (item.ParentName != null && !string.IsNullOrEmpty(item.ParentName))
                        {
                            var parent = _repositoryIndividualProject.FirstOrDefault(a => a.Name == item.ParentName&&a.SpecialtyId== individualProjectModel.SpecialtyId);
                            if (parent != null)
                            {
                                individualProjectModel.ParentId = parent.Id;
                            }
                            else
                            {
                                var parentModel = datalist.ToList().Find(x => x.Name == item.ParentName&&x.SpecialtyName==item.SpecialtyName);
                                if (parentModel != null)
                                {
                                    var parentEnt = new IndividualProject(_guidGenerator.Create());
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Remark = parentModel.Remark;
                                    parentEnt.Code = parentModel.Code;
                                    if (profession != null)
                                        parentEnt.SpecialtyId = profession.Id;
                                    await _repositoryIndividualProject.InsertAsync(parentEnt);
                                    individualProjectModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }

                        }
                        await _repositoryIndividualProject.UpdateAsync(individualProjectModel);
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {

                        individualProjectModel = new IndividualProject(_guidGenerator.Create());
                        individualProjectModel.Name = item.Name;
                        individualProjectModel.Code = item.Code;
                        individualProjectModel.Remark = item.Remark;
                        if (profession != null)
                            individualProjectModel.SpecialtyId = profession.Id;
                        if (item.ParentName != null && !string.IsNullOrEmpty(item.ParentName))
                        {
                            var parent = _repositoryIndividualProject.FirstOrDefault(a => a.Name == item.ParentName && a.SpecialtyId == individualProjectModel.SpecialtyId);
                            if (parent == null)
                            {
                                var parentModel = datalist.ToList().Find(x => x.Name == item.ParentName && x.SpecialtyName == item.SpecialtyName);
                                if (parentModel != null)
                                {
                                    var parentEnt = new IndividualProject(_guidGenerator.Create());
                                    if (profession != null)
                                        parentEnt.SpecialtyId = profession.Id;
                                    parentEnt.Name = parentModel.Name;
                                    parentEnt.Remark = parentModel.Remark;
                                    parentEnt.Code = parentModel.Code;

                                    await _repositoryIndividualProject.InsertAsync(parentEnt);
                                    individualProjectModel.ParentId = parentEnt.Id;
                                    await uow.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                individualProjectModel.ParentId = parent.Id;
                            }
                        }
                        await _repositoryIndividualProject.InsertAsync(individualProjectModel);
                        await uow.SaveChangesAsync();
                    }
                }


                await _fileImport.Complete(input.ImportKey);
                if (wrongInfos.Any())
                {
                    sheet.CreateInfoColumn(wrongInfos);
                    await _fileImport.SaveExceptionFile(CurrentUser.Id.GetValueOrDefault(), input.ImportKey, workbook.ConvertToBytes());
                }
            }
        }


        private List<IndividualProject> GetIndividualProjectData(IndividualProjectGetListByIdsDto input)
        {
            var individualProjects = new List<IndividualProject>();
            var dtos = new List<IndividualProjectDto>();

            //根据关键字查询
            if (!string.IsNullOrEmpty(input.KeyWords))
            {
                individualProjects = _repositoryIndividualProject.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords)).Take(200).ToList();
                dtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(individualProjects);
                foreach (var item in dtos)
                {
                    item.Children = null;
                }
            }
            else
            {
                individualProjects = _repositoryIndividualProject.WithDetails()
                               .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
                               .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.Parent == null) // 顶级
                               .ToList();
                dtos = ObjectMapper.Map<List<IndividualProject>, List<IndividualProjectDto>>(individualProjects);
                foreach (var item in dtos)
                {
                    if (item.Children.Count == 0)
                    {
                        item.Children = null;
                    }
                    else
                    {
                        item.Children = new List<IndividualProjectDto>();
                    }
                }
            }

            var resource = ObjectMapper.Map<List<IndividualProjectDto>, List<IndividualProject>>(dtos);
            return resource;
        }

        private void FindAllParents(Guid parentid, ref List<IndividualProject> list)
        {
            var parentModel = _repositoryIndividualProject.WithDetails().FirstOrDefault(a => a.Id == parentid);
            if (parentModel != null)
            {
                list.Add(parentModel);
                if (parentModel.ParentId != null && parentModel.ParentId != Guid.Empty)
                {
                    FindAllParents(parentModel.ParentId.Value, ref list);
                }
            }

        }

        private void ChangeChildrens(IndividualProject input, ref List<IndividualProject> catelist)
        {
            if (catelist == null) catelist = new List<IndividualProject>();
            var childrens = _repositoryIndividualProject.WithDetails().Where(x => x.ParentId == input.Id).ToList();
            if (childrens?.Count > 0)
            {

                foreach (var item in childrens)
                {
                    item.SpecialtyId = input.SpecialtyId;
                    catelist.Add(item);

                    if (item.Children.Count > 0)
                    {
                        ChangeChildrens(item, ref catelist);
                    }
                }

            }

        }
    }
}
