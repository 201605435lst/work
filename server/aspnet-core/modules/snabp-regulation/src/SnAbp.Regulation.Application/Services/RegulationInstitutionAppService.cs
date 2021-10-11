using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Identity;
using SnAbp.Regulation.Commons;
using SnAbp.Regulation.Dtos.Institution;
using SnAbp.Regulation.Entities;
using SnAbp.Regulation.Enums;
using SnAbp.Regulation.IServices;
using SnAbp.Regulation.Permissions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;


namespace SnAbp.Regulation.Services
{
    [Authorize]
    public class RegulationInstitutionAppService : RegulationAppService, IRegulationInstitutionAppService
    {
        private readonly IRepository<Institution, Guid> _repositoryInstitution;
        private readonly IRepository<InstitutionRltLabel, Guid> _repositoryRltLabel;
        private readonly IRepository<InstitutionRltEdition, Guid> _repositoryRltEdition;
        private readonly IRepository<InstitutionRltFile, Guid> _repositoryRltFile;
        private readonly IRepository<InstitutionRltAuthority, Guid> _repositoryRltAuthority;
        private readonly IGuidGenerator _guidGenerator;

        public RegulationInstitutionAppService(IRepository<Institution, Guid> repositoryInstitution, IRepository<InstitutionRltLabel, Guid> repositoryRltLabel, IRepository<InstitutionRltEdition, Guid> repositoryRltEdition, IRepository<InstitutionRltFile, Guid> repositoryRltFile, IRepository<InstitutionRltAuthority, Guid> repositoryRltAuthority, IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _repositoryInstitution = repositoryInstitution;
            _repositoryRltLabel = repositoryRltLabel;
            _repositoryRltEdition = repositoryRltEdition;
            _repositoryRltFile = repositoryRltFile;
            _repositoryRltAuthority = repositoryRltAuthority;
        }


        #region 创建制度
        [Authorize(RegulationPermissions.Institution.Create)]
        public async Task<InstitutionDto> Create(InstitutionCreateDto input)
        {
            //批量修改权限
            if (input.Flag == 1)
            {
                //清除之前保存的关联表信息
                await _repositoryRltAuthority.DeleteAsync(x => input.SelectedIds.Contains(x.InstitutionId));
                if (input.ListView != null || input.ListEdit != null || input.ListDownLoad != null)
                {
                    List<Guid> ListView = input.ListView.Select(a => a.Id).ToList();
                    List<Guid> ListEdit = input.ListEdit.Select(a => a.Id).ToList();
                    List<Guid> ListDownLoad = input.ListDownLoad.Select(a => a.Id).ToList();

                    var merge = Merge1(ListView, ListEdit, ListDownLoad);
                    foreach (var institutionId in input.SelectedIds)
                    {
                        foreach (var id in merge)
                        {
                            var institutionRltAuthority = new InstitutionRltAuthority(_guidGenerator.Create());
                            institutionRltAuthority.InstitutionId = institutionId;
                            institutionRltAuthority.MemberId = id;
                            if (ListView.Contains(id))
                            {
                                var view = input.ListView.Where(x => x.Id == id).FirstOrDefault();
                                if (view != null)
                                {
                                    institutionRltAuthority.IsView = true;
                                    institutionRltAuthority.Type = view.Type;
                                }
                            }
                            if (ListEdit.Contains(id))
                            {
                                var edit = input.ListEdit.Where(x => x.Id == id).FirstOrDefault();
                                if (edit != null)
                                {
                                    institutionRltAuthority.IsEdit = true;
                                    institutionRltAuthority.Type = edit.Type;
                                }
                            }
                            if (ListDownLoad.Contains(id))
                            {
                                var download = input.ListDownLoad.Where(x => x.Id == id).FirstOrDefault();
                                if (download != null)
                                {
                                    institutionRltAuthority.IsDownLoad = true;
                                    institutionRltAuthority.Type = download.Type;
                                }
                            }
                            await _repositoryRltAuthority.InsertAsync(institutionRltAuthority);
                        }
                    }
                }
                return ObjectMapper.Map<Institution, InstitutionDto>(null);
            }
            else
            {
                CheckSameName(input.Header, input.Id);
                var institution = new Institution(_guidGenerator.Create())
                {
                    Header = input.Header,
                    Code = input.Code,
                    Classify = input.Classify,
                    EffectiveTime = input.EffectiveTime,
                    ExpireTime = input.ExpireTime,
                    Abstract = input.Abstract,
                    IsPublish = input.IsPublish,
                    NewsClassify = input.NewsClassify,
                    IsApprove = input.IsApprove,
                    OrganizationId = input.OrganizationId
                };
                await _repositoryInstitution.InsertAsync(institution);

                foreach (var label in input.InstitutionRltLabels)
                {
                    var model = new InstitutionRltLabel(_guidGenerator.Create())
                    {
                        InstitutionId = institution.Id,
                        LabelId = label,
                    };
                    await _repositoryRltLabel.InsertAsync(model);
                }

                foreach (var file in input.InstitutionRltFiles)
                {
                    var model = new InstitutionRltFile()
                    {
                        InstitutionId = institution.Id,
                        FileId = file
                    };
                    await _repositoryRltFile.InsertAsync(model);
                }

                if (input.ListView != null || input.ListEdit != null || input.ListDownLoad != null)
                {
                    List<Guid> ListView = input.ListView.Select(a => a.Id).ToList();
                    List<Guid> ListEdit = input.ListEdit.Select(a => a.Id).ToList();
                    List<Guid> ListDownLoad = input.ListDownLoad.Select(a => a.Id).ToList();

                    var merge = Merge1(ListView, ListEdit, ListDownLoad);
                    foreach (var id in merge)
                    {
                        var institutionRltAuthority = new InstitutionRltAuthority(_guidGenerator.Create());
                        institutionRltAuthority.MemberId = id;
                        institutionRltAuthority.InstitutionId = institution.Id;

                        if (ListView.Contains(id))
                        {
                            var view = input.ListView.Where(x => x.Id == id).FirstOrDefault();
                            if (view != null)
                            {
                                institutionRltAuthority.IsView = true;
                                institutionRltAuthority.Type = view.Type;
                            }
                        }
                        if (ListEdit.Contains(id))
                        {
                            var edit = input.ListEdit.Where(x => x.Id == id).FirstOrDefault();
                            if (edit != null)
                            {
                                institutionRltAuthority.IsEdit = true;
                                institutionRltAuthority.Type = edit.Type;
                            }
                        }
                        if (ListDownLoad.Contains(id))
                        {
                            var download = input.ListDownLoad.Where(x => x.Id == id).FirstOrDefault();
                            if (download != null)
                            {
                                institutionRltAuthority.IsDownLoad = true;
                                institutionRltAuthority.Type = download.Type;
                            }
                        }
                        await _repositoryRltAuthority.InsertAsync(institutionRltAuthority);
                    }
                }

                var institutionRltEdition = new InstitutionRltEdition(_guidGenerator.Create())
                {
                    InstitutionId = institution.Id,
                    Header = input.Header,
                    Code = input.Code,
                    Classify = input.Classify,
                    EffectiveTime = input.EffectiveTime,
                    ExpireTime = input.ExpireTime,
                    Abstract = input.Abstract,
                    OrganizationId = input.OrganizationId,
                    Version = input.Version
                };
                await _repositoryRltEdition.InsertAsync(institutionRltEdition);
                return ObjectMapper.Map<Institution, InstitutionDto>(institution);
            }
        }
        #endregion

        #region 删除制度
        [Authorize(RegulationPermissions.Institution.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repositoryInstitution.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
        #endregion

        #region  修改制度
        [Authorize(RegulationPermissions.Institution.Update)]
        public async Task<InstitutionDto> Update(InstitutionUpdateDto input)
        {
            var institution = await _repositoryInstitution.GetAsync(input.Id);
            if (institution == null) throw new UserFriendlyException("当前制度不存在");
            if (input.Flag == 1)
            {
                //清除之前保存的关联表信息,修改权限
                await _repositoryRltAuthority.DeleteAsync(x => x.InstitutionId == input.Id);
                if (input.ListView != null || input.ListEdit != null || input.ListDownLoad != null)
                {
                    List<Guid> ListView = input.ListView.Select(a => a.Id).ToList();
                    List<Guid> ListEdit = input.ListEdit.Select(a => a.Id).ToList();
                    List<Guid> ListDownLoad = input.ListDownLoad.Select(a => a.Id).ToList();

                    var merge = Merge1(ListView, ListEdit, ListDownLoad);
                    foreach (var id in merge)
                    {
                        var institutionRltAuthority = new InstitutionRltAuthority(_guidGenerator.Create());
                        institutionRltAuthority.MemberId = id;
                        institutionRltAuthority.InstitutionId = institution.Id;

                        if (ListView.Contains(id))
                        {
                            var view = input.ListView.Where(x => x.Id == id).FirstOrDefault();
                            if (view != null)
                            {
                                institutionRltAuthority.IsView = true;
                                institutionRltAuthority.Type = view.Type;
                            }
                        }
                        if (ListEdit.Contains(id))
                        {
                            var edit = input.ListEdit.Where(x => x.Id == id).FirstOrDefault();
                            if (edit != null)
                            {
                                institutionRltAuthority.IsEdit = true;
                                institutionRltAuthority.Type = edit.Type;
                            }
                        }
                        if (ListDownLoad.Contains(id))
                        {
                            var download = input.ListDownLoad.Where(x => x.Id == id).FirstOrDefault();
                            if (download != null)
                            {
                                institutionRltAuthority.IsDownLoad = true;
                                institutionRltAuthority.Type = download.Type;
                            }
                        }
                        await _repositoryRltAuthority.InsertAsync(institutionRltAuthority);
                    }
                }
            }
            else
            {
                institution.Header = input.Header;
                CheckSameName(input.Header, input.Id);
                institution.Code = input.Code;
                institution.Classify = input.Classify;
                institution.EffectiveTime = input.EffectiveTime;
                institution.ExpireTime = input.ExpireTime;
                institution.Abstract = input.Abstract;
                institution.IsPublish = input.IsPublish;
                institution.NewsClassify = input.NewsClassify;
                institution.IsApprove = input.IsApprove;
                institution.OrganizationId = input.OrganizationId;
                await _repositoryInstitution.UpdateAsync(institution);

                // 清除之前保存的关联表信息
                await _repositoryRltFile.DeleteAsync(x => x.InstitutionId == institution.Id);
                // 重新保存关联表信息
                foreach (var file in input.InstitutionRltFiles)
                {
                    var model = new InstitutionRltFile()
                    {
                        InstitutionId = institution.Id,
                        FileId = file,
                    };
                    await _repositoryRltFile.InsertAsync(model);
                }

                await _repositoryRltLabel.DeleteAsync(x => x.InstitutionId == institution.Id);
                foreach (var label in input.InstitutionRltLabels)
                {
                    var model = new InstitutionRltLabel(_guidGenerator.Create())
                    {
                        InstitutionId = institution.Id,
                        LabelId = label,
                    };
                    await _repositoryRltLabel.InsertAsync(model);
                }


                //清除之前保存的关联表信息
                await _repositoryRltAuthority.DeleteAsync(x => x.InstitutionId == input.Id);
                if (input.ListView != null || input.ListEdit != null || input.ListDownLoad != null)
                {
                    List<Guid> ListView = input.ListView.Select(a => a.Id).ToList();
                    List<Guid> ListEdit = input.ListEdit.Select(a => a.Id).ToList();
                    List<Guid> ListDownLoad = input.ListDownLoad.Select(a => a.Id).ToList();

                    var merge = Merge1(ListView, ListEdit, ListDownLoad);
                    foreach (var id in merge)
                    {
                        var institutionRltAuthority = new InstitutionRltAuthority(_guidGenerator.Create());
                        institutionRltAuthority.MemberId = id;
                        institutionRltAuthority.InstitutionId = institution.Id;

                        if (ListView.Contains(id))
                        {
                            var view = input.ListView.Where(x => x.Id == id).FirstOrDefault();
                            if (view != null)
                            {
                                institutionRltAuthority.IsView = true;
                                institutionRltAuthority.Type = view.Type;
                            }
                        }
                        if (ListEdit.Contains(id))
                        {
                            var edit = input.ListEdit.Where(x => x.Id == id).FirstOrDefault();
                            if (edit != null)
                            {
                                institutionRltAuthority.IsEdit = true;
                                institutionRltAuthority.Type = edit.Type;
                            }
                        }
                        if (ListDownLoad.Contains(id))
                        {
                            var download = input.ListDownLoad.Where(x => x.Id == id).FirstOrDefault();
                            if (download != null)
                            {
                                institutionRltAuthority.IsDownLoad = true;
                                institutionRltAuthority.Type = download.Type;
                            }
                        }
                        await _repositoryRltAuthority.InsertAsync(institutionRltAuthority);
                    }
                }

                var institutionRltEdition = new InstitutionRltEdition(_guidGenerator.Create())
                {
                    InstitutionId = institution.Id,
                    Header = input.Header,
                    Code = input.Code,
                    Classify = input.Classify,
                    EffectiveTime = input.EffectiveTime,
                    ExpireTime = input.ExpireTime,
                    Abstract = input.Abstract,
                    OrganizationId = input.OrganizationId,
                    Version = input.Version
                };
                await _repositoryRltEdition.InsertAsync(institutionRltEdition);
            }
            return ObjectMapper.Map<Institution, InstitutionDto>(institution);
        }
        #endregion

        #region 权限
        [Authorize(RegulationPermissions.Institution.Authority)]
        public async void UpdateAuthority(InstitutionUpdateDto input)
        {
            //清除之前保存的关联表信息
            await _repositoryRltAuthority.DeleteAsync(x => x.InstitutionId == input.Id);
            if (input.ListView != null || input.ListEdit != null || input.ListDownLoad != null)
            {
                List<Guid> ListView = input.ListView.Select(a => a.Id).ToList();
                List<Guid> ListEdit = input.ListEdit.Select(a => a.Id).ToList();
                List<Guid> ListDownLoad = input.ListDownLoad.Select(a => a.Id).ToList();

                var merge = Merge1(ListView, ListEdit, ListDownLoad);
                foreach (var id in merge)
                {
                    var institutionRltAuthority = new InstitutionRltAuthority(_guidGenerator.Create());
                    institutionRltAuthority.MemberId = id;

                    if (ListView.Contains(id))
                    {
                        var view = input.ListView.Where(x => x.Id == id).FirstOrDefault();
                        if (view != null)
                        {
                            institutionRltAuthority.IsView = true;
                            institutionRltAuthority.Type = view.Type;
                        }
                    }
                    if (ListEdit.Contains(id))
                    {
                        var edit = input.ListEdit.Where(x => x.Id == id).FirstOrDefault();
                        if (edit != null)
                        {
                            institutionRltAuthority.IsEdit = true;
                            institutionRltAuthority.Type = edit.Type;
                        }
                    }
                    if (ListDownLoad.Contains(id))
                    {
                        var download = input.ListDownLoad.Where(x => x.Id == id).FirstOrDefault();
                        if (download != null)
                        {
                            institutionRltAuthority.IsDownLoad = true;
                            institutionRltAuthority.Type = download.Type;
                        }
                    }
                    await _repositoryRltAuthority.InsertAsync(institutionRltAuthority);
                }
            }
        }
        #endregion

        #region 查询制度
        [Authorize(RegulationPermissions.Institution.Detail)]
        public Task<InstitutionDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var institution = _repositoryInstitution.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (institution == null) throw new UserFriendlyException("此制度不存在");
            var result = ObjectMapper.Map<Institution, InstitutionDto>(institution);
            // result.ListView = new List<MemberInfoDto>();
            // result.ListEdit = new List<MemberInfoDto>();
            // result.ListDownLoad = new List<MemberInfoDto>();
            foreach (var authority in result.InstitutionRltAuthorities)
            {
                if (authority.IsView == true)
                {
                    MemberInfoDto a = new MemberInfoDto();
                    a.Id = (Guid)authority.MemberId;
                    a.Type = authority.Type;
                    result.ListView.Add(a);
                }
                if (authority.IsEdit == true)
                {
                    MemberInfoDto a = new MemberInfoDto();
                    a.Id = (Guid)authority.MemberId;
                    a.Type = authority.Type;
                    result.ListEdit.Add(a);
                }
                if (authority.IsDownLoad == true)
                {
                    MemberInfoDto a = new MemberInfoDto();
                    a.Id = (Guid)authority.MemberId;
                    a.Type = authority.Type;
                    result.ListDownLoad.Add(a);
                }
            }
            var edition = _repositoryRltEdition.WithDetails().Where(x => x.InstitutionId == id).OrderByDescending(x => x.CreationTime).FirstOrDefault();
            if (edition != null)
                result.Version = edition.Version;
            else
                result.Version = 1;

            //判断当前用户有哪些权限
            foreach (var authority in institution.InstitutionRltAuthorities)
            {
                if (CurrentUser.Id == authority.MemberId)
                {
                    if (authority.IsView == true)
                        result.ViewInstitution = id;
                    if (authority.IsEdit == true)
                        result.EditInstitution = id;
                    if (authority.IsDownLoad == true)
                        result.DownLoadInstitution = id;
                }
            }


            return Task.FromResult(result);
        }

        public Task<PagedResultDto<InstitutionDto>> GetList(InstitutionSearchDto input)
        {
            var institution = _repositoryInstitution.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Header.Contains(input.KeyWords) || x.Code.Contains(input.KeyWords));
            var result = new PagedResultDto<InstitutionDto>();
            result.TotalCount = institution.Count();
            if (input.Order == "descend")
            {
                var list = institution.OrderByDescending(x => input.ColumnKey == "creationTime" ? x.CreationTime : x.EffectiveTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                result.Items = ObjectMapper.Map<List<Institution>, List<InstitutionDto>>(list);
            }
            if (input.Order == "ascend")
            {
                var list = institution.OrderBy(x => input.ColumnKey == "creationTime" ? x.CreationTime : x.EffectiveTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                result.Items = ObjectMapper.Map<List<Institution>, List<InstitutionDto>>(list);
            }

            //判断当前用户有哪些权限
            foreach (var record in result.Items)
            {
                foreach (var authority in record.InstitutionRltAuthorities)
                {
                    if (CurrentUser.Id == authority.MemberId)
                    {
                        if (authority.IsView == true)
                            record.ViewInstitution = record.Id;
                        if (authority.IsEdit == true)
                            record.EditInstitution = record.Id;
                        if (authority.IsDownLoad == true)
                            record.DownLoadInstitution = record.Id;
                    }
                }

            }
            return Task.FromResult(result);
        }
        #endregion

        #region 导出
        [Authorize(RegulationPermissions.Institution.Export)]
        [Produces("application/octet-stream")]
        public Task<Stream> DownLoad(List<Guid> ids)
        {
            var dataRow = (DataRow)null;
            var institution = _repositoryInstitution.WithDetails(x => x.Organization)
                              .WhereIf(ids.Count > 0, x => ids.Contains(x.Id));
            var list = ObjectMapper.Map<List<Institution>, List<InstitutionExportDto>>(institution.OrderBy(x => x.CreationTime).ToList());


            var dataTable = new DataTable();

            var dataColumn = (DataColumn)null;
            //添加列表
            var enumValues = Enum.GetValues(typeof(InstitutionExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(InstitutionExcelCol), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }

            //添加数据
            foreach (var item in list)
            {
                dataRow = dataTable.NewRow();
                dataRow[InstitutionExcelCol.文件标题.ToString()] = item.Header;
                dataRow[InstitutionExcelCol.文件编号.ToString()] = item.Code;
                dataRow[InstitutionExcelCol.状态.ToString()] = item.State;
                dataRow[InstitutionExcelCol.所属部门.ToString()] = item.Organization.Name;
                dataRow[InstitutionExcelCol.制度分类.ToString()] = item.Classify;
                dataRow[InstitutionExcelCol.录入人.ToString()] = item.InputPeople;
                dataRow[InstitutionExcelCol.生效时间.ToString()] = item.EffectiveTime;
                dataRow[InstitutionExcelCol.创建时间.ToString()] = item.CreationTime;
                dataTable.Rows.Add(dataRow);
            }

            byte[] sbuf = ExcelHepler.DataTableToExcel(dataTable, "制度管理表.xlsx");
            Stream stream = new MemoryStream(sbuf);
            return Task.FromResult(stream);
        }
        #endregion

        #region 判断文件标题是否已存在
        private bool CheckSameName(string name, Guid? id)
        {
            var result = _repositoryInstitution.Where(x => x.Header.ToUpper() == name.ToUpper());
            if (id.HasValue)
            {
                result = result.Where(x => x.Id != id.Value);
            }
            if (result.Count() > 0)
            {
                throw new UserFriendlyException("文件标题已存在");
            }
            return true;
        }
        #endregion

        #region 数组去重
        private List<Guid> Merge1(List<Guid> arr1, List<Guid> arr2, List<Guid> arr3)
        {
            List<Guid> arr4 = new List<Guid>();
            arr1.AddRange(arr2);
            arr1.AddRange(arr3);

            Hashtable hashtable = new Hashtable();

            foreach (var bianliang in arr1)
            {
                if (!hashtable.ContainsKey(bianliang))
                {
                    hashtable.Add(bianliang, bianliang);
                    arr4.Add(bianliang);
                }
            }
            return arr4;
        }
        #endregion




    }
}
