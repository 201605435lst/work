using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
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

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicProjectItemAppService : StdBasicAppService, IStdBasicProjectItemAppService
    {
        private readonly IRepository<ProjectItem, Guid> _repositoryProjectItem;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;
        private readonly IDataFilter _dataFilter;

        public StdBasicProjectItemAppService(IRepository<ProjectItem, Guid> repositoryProjectItem,
           IGuidGenerator guidGenerator,
           IFileImportHandler fileImport,
           IDataFilter dataFilter
       )
        {
            _repositoryProjectItem = repositoryProjectItem;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
            _dataFilter = dataFilter;
        }

        [Authorize(StdBasicPermissions.ProjectItem.Create)]
        public async Task<ProjectItemDto> Create(ProjectItemCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("工程工项名称不能为空");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
            var before = _repositoryProjectItem.FirstOrDefault(x => x.Name == input.Name) ;
            if (before != null) throw new UserFriendlyException("已存在同名工程工项");
            if (!string.IsNullOrEmpty(input.Code))
            {
                var codeModel = _repositoryProjectItem.FirstOrDefault(x => !string.IsNullOrEmpty(x.Code) && x.Code == input.Code);
                if (codeModel != null) throw new UserFriendlyException("编码已存在");
            }
            var projectItem = new ProjectItem(_guidGenerator.Create());
            projectItem.Name = input.Name;

            projectItem.Code = input.Code;

            projectItem.Remark = input.Remark;
            await _repositoryProjectItem.InsertAsync(projectItem);
            return ObjectMapper.Map<ProjectItem, ProjectItemDto>(projectItem);
        }

        [Authorize(StdBasicPermissions.ProjectItem.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProjectItem.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此工程工项不存在");
            await _repositoryProjectItem.DeleteAsync(id);
            return true;
        }
        [Authorize(StdBasicPermissions.ProjectItem.Export)]
        public async Task<Stream> Export(ProjectItemData input)
        {
            var list = GetProjectItemData(input.Paramter);
            //var list = await _manufacturerRepository.GetListAsync();
            var dtoList = ObjectMapper.Map<List<ProjectItem>, List<ProjectItemTemplate>>(list);
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return stream;
        }
        [Authorize(StdBasicPermissions.ProjectItem.Detail)]
        public async Task<ProjectItemDto> Get(Guid id)
        {
            ProjectItemDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProjectItem.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此工程工项不存在");
                result = ObjectMapper.Map<ProjectItem, ProjectItemDto>(ent);
            });
            return result;
        }

        public async Task<PagedResultDto<ProjectItemDto>> GetList(ProjectItemSearchDto input)
        {
            PagedResultDto<ProjectItemDto> res = new PagedResultDto<ProjectItemDto>();
            await Task.Run(() =>
            {
                var allEnt =_repositoryProjectItem.WithDetails()
                        .WhereIf(!string.IsNullOrEmpty(input.Keyword), x => x.Code.Contains(input.Keyword) || x.Name.Contains(input.Keyword)).ToList();
                if (input.IsAll == false)
                {
                    res.TotalCount = allEnt.Count();
                    res.Items = ObjectMapper.Map<List<ProjectItem>, List<ProjectItemDto>>(allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                }
                else
                {
                    res.Items = ObjectMapper.Map<List<ProjectItem>, List<ProjectItemDto>>(allEnt.ToList());
                    res.TotalCount = res.Items.Count;
                }
            });
            return res;
        }

        public Task<PagedResultDto<ProjectItemDto>> GetListProjectItem(ProjectItemSearchDto input)
        {
            var projectItems = new List<ProjectItem>();
            var result = new PagedResultDto<ProjectItemDto>();
            if (input.Ids == null)
                input.Ids = new List<Guid>();
            //获取当前对象实体
            if (input.Ids.Count > 0)
            {
                projectItems = _repositoryProjectItem.Where(x => input.Ids.Contains(x.Id)).ToList();
                var dtos = ObjectMapper.Map<List<ProjectItem>, List<ProjectItemDto>>(projectItems);
                result.TotalCount = dtos.Count;
                result.Items = input.IsAll
                ? dtos.ToList()
                : dtos.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            }
            return Task.FromResult(result);
        }

        [Authorize(StdBasicPermissions.ProjectItem.Update)]
        public async Task<ProjectItemDto> Update(ProjectItemDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("名称不能包含空格");
            }
            if (!string.IsNullOrEmpty(input.Code) && !StringUtil.CheckCodeValidity(input.Code))
            {
                throw new UserFriendlyException("编码不合法");
            }
           
            var ent = _repositoryProjectItem.FirstOrDefault(s => input.Id == s.Id);
            if (ent == null) throw new UserFriendlyException("此工程工项不存在");
            var before = _repositoryProjectItem.FirstOrDefault(x => x.Name == input.Name && x.Id != input.Id);
            if (before != null) throw new UserFriendlyException("已存在同名工程工项");
            if (!string.IsNullOrEmpty(input.Code))
            {
                var codeModel = _repositoryProjectItem.FirstOrDefault(x => !string.IsNullOrEmpty(x.Code) && x.Code == input.Code && x.Id != input.Id);
                if (codeModel != null) throw new UserFriendlyException("编码已存在");
            }
            ent.Name = input.Name;
            ent.Code = input.Code;
            ent.Remark = input.Remark;

            var resEnt = await _repositoryProjectItem.UpdateAsync(ent);
            return ObjectMapper.Map<ProjectItem, ProjectItemDto>(resEnt);
        }
        [Authorize(StdBasicPermissions.ProjectItem.Import)]
        public async Task Upload([FromForm] ImportData input)
        {
            // 虚拟进度0 %
            await _fileImport.Start(input.ImportKey, 100);
            //ProjectItemTemplate
            // 获取excel表格，判断报个是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            ISheet sheet = null;
            IWorkbook workbook = null;
            List<ProjectItemTemplate> datalist = null;
            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<ProjectItemTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<ProjectItemTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            ProjectItem projectItemModel;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var groupList = datalist.GroupBy(a => a.Code.Split(".").Length).ToList();
                var updateIndex = 1;
                foreach (var ite in groupList)
                {
                    foreach (var item in ite.ToList())
                    {
                        await _fileImport.UpdateState(input.ImportKey, updateIndex);
                        updateIndex++;
                        var canInsert = true;
                        var newInfo = new WrongInfo(item.Index);
                        if (item.Name.IsNullOrEmpty())
                        {
                            canInsert = false;
                            newInfo.AppendInfo($"名称为空");
                        }
                        if (!canInsert)
                        {
                            wrongInfos.Add(newInfo);
                            continue;
                        }
                        using (_dataFilter.Disable<ISoftDelete>())
                        {
                            projectItemModel = _repositoryProjectItem.FirstOrDefault(a => a.Name == item.Name);
                        }
                        if (projectItemModel != null)
                        {
                            newInfo.AppendInfo($"{item.Code}已存在，且已更新");
                            //修改自己本身数据
                            projectItemModel.Name = item.Name;
                            projectItemModel.Remark = item.Remark;
                            await _repositoryProjectItem.UpdateAsync(projectItemModel);
                            wrongInfos.Add(newInfo);
                        }
                        else
                        {

                            projectItemModel = new ProjectItem(_guidGenerator.Create());
                            projectItemModel.Name = item.Name;
                            projectItemModel.Code = item.Code;
                            projectItemModel.Remark = item.Remark;

                            await _repositoryProjectItem.InsertAsync(projectItemModel);
                        }
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

        private List<ProjectItem> GetProjectItemData(ProjectItemGetListDto input)
        {
            var query = _repositoryProjectItem
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Keyword),
                    s => s.Name.Contains(input.Keyword) ||
                        s.Remark.Contains(input.Keyword) ||
                        s.Code.Contains(input.Keyword)
                 );

            return query.ToList();
        }
    }
}
