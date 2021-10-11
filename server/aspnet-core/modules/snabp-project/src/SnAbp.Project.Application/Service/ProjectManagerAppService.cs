using Microsoft.AspNetCore.Mvc;
using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using SnAbp.Project.enums;
using SnAbp.Project.IServices;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Project.Service
{
    public class ProjectManagerAppService : ProjectAppService, IProjectAppServices

    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Project, Guid> _projectRepository;
        private readonly IRepository<Unit, Guid> _unitRepository;
        private readonly IUnitOfWorkManager _unitOfWork;
        private readonly IRepository<ProjectRltContract, Guid> _projectRltContracts;
        private readonly IRepository<ProjectRltMember, Guid> _projectRltMember;
        //private readonly IRepository<Contract, Guid> _contracts;
        private readonly IRepository<ProjectRltUnit, Guid> _projectRltUnit;
        private readonly IRepository<ProjectRltFile, Guid> _projectRltFile;

        public ProjectManagerAppService(
            IGuidGenerator guidGenerator,
            IRepository<Project, Guid> projectRepository,
            IRepository<Unit, Guid> unitRepository,
            IRepository<ProjectRltContract, Guid> projectRltContracts,
            IRepository<ProjectRltMember, Guid> projectRltMember,
            IUnitOfWorkManager unitOfWork,
            //IRepository<Contract, Guid> contracts,
            IRepository<ProjectRltFile, Guid> projectRltFile,
            IRepository<ProjectRltUnit, Guid> projectRltUnit
            )
        {
            _guidGenerator = guidGenerator;
            _projectRepository = projectRepository;
            _unitRepository = unitRepository;
            _unitOfWork = unitOfWork;
            _projectRltContracts = projectRltContracts;
            _projectRltMember = projectRltMember;
            _projectRltFile = projectRltFile;
            _projectRltUnit = projectRltUnit;
            //_contracts = contracts;
        }

        public async Task<ProjectDto> Create(ProjectCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("项目名称不能为空");
            if (Guid.Empty == input.TypeId) throw new UserFriendlyException("请选择项目类型");
            if (Guid.Empty == input.OrganizationId) throw new UserFriendlyException("请选择建设单位");
            if (Guid.Empty == input.ManagerId) throw new UserFriendlyException("请选择负责人");
            if (string.IsNullOrEmpty(input.Scale)) throw new UserFriendlyException("建设规模不能为空");
            if (input.PlannedStartTime == null) throw new UserFriendlyException("请选择项目开始时间");
            if (input.PlannedEndTime == null) throw new UserFriendlyException("请选择项目结束时间");

            var dto = ObjectMapper.Map<ProjectCreateDto, ProjectDto>(input);
            dto.Id = _guidGenerator.Create();
            var ent = ObjectMapper.Map<ProjectDto, Project>(dto);
            using var unow = _unitOfWork.Begin(true, false);

            //生成项目code
            var nowDate = DateTime.Now;
            ent.Code = "XM" + nowDate.Year.ToString() + nowDate.Month.ToString().PadLeft(2, '0') + nowDate.Day.ToString().PadLeft(2, '0');

            //保存合同关联关系
            ent.ProjectRltContracts = new List<ProjectRltContract>();
            foreach (var contractId in input.ProjectRltContractIds)
            {
                ent.ProjectRltContracts.Add(new ProjectRltContract(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    ContractId = contractId
                });
            }
            //保存文件关联
            ent.ProjectRltFiles = new List<ProjectRltFile>();
            foreach (var fileId in input.ProjectRltFileIds)
            {
                ent.ProjectRltFiles.Add(new ProjectRltFile(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    FileId = fileId
                });
            }

            var unitList = new List<Guid>();
            //保存参建单位

            foreach (var unit in input.Units)
            {
                var unitEnt = new Unit(_guidGenerator.Create())
                {
                    Code = unit?.Code,
                    Name = unit?.Name,
                    BankAccount = unit?.BankAccount,
                    BankCode = unit?.BankCode,
                    Type = unit.Type,
                    Leader = unit.Leader
                };
                unitList.Add(unitEnt.Id);
                await _unitRepository.InsertAsync(unitEnt);
                await unow.SaveChangesAsync();
            };
            //保存参建单位关联
            ent.ProjectRltUnits = new List<ProjectRltUnit>();
            foreach (var unitId in unitList)
            {
                ent.ProjectRltUnits.Add(new ProjectRltUnit(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    UnitId = unitId
                });
            }

            await _projectRepository.InsertAsync(ent);
            await unow.SaveChangesAsync();
            //关联项目成员
            foreach (var memberId in input.ProjectRltMemberIds)
            {
                var memberEnt = new ProjectRltMember(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    ManagerId = memberId
                };
                await _projectRltMember.InsertAsync(memberEnt);
                await unow.SaveChangesAsync();
            }
            return await Task.FromResult(ObjectMapper.Map<Project, ProjectDto>(ent));
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");

            await _projectRltUnit.DeleteAsync(x => x.ProjectId == id);
            await _projectRltContracts.DeleteAsync(x => x.ProjectId == id);
            await _projectRltMember.DeleteAsync(x => x.ProjectId == id);
            await _projectRltFile.DeleteAsync(x => x.ProjectId == id);

            await _projectRepository.DeleteAsync(id);

            return true;
        }


        public Task<ProjectDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不正确");
            }

            var result = _projectRepository.WithDetails().FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                throw new UserFriendlyException("项目信息不存在");
            }

            return System.Threading.Tasks.Task.FromResult(ObjectMapper.Map<Project, ProjectDto>(result));
        }

        public Task<PagedResultDto<ProjectDto>> GetList(ProjectSearchDto input)
        {
            var list = _projectRepository.WithDetails().WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Code.Contains(input.KeyWords) || x.Name.Contains(input.KeyWords));
            var result = new PagedResultDto<ProjectDto>()
            {
                TotalCount = list.Count(),
                Items = input.IsAll ? ObjectMapper.Map<List<Project>, List<ProjectDto>>(list.OrderByDescending(x => x.CreationTime).ToList()) :
                                    ObjectMapper.Map<List<Project>, List<ProjectDto>>(list.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList()),
            };

            return Task.FromResult(result);
        }

        /// <summary>
        /// 通过ids获取数据列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<ProjectDto>> GetListByIds(ProjectSearchByIdsDto input)
        {
            var list = _projectRepository.Where(x => input.Ids.Contains(x.Id)).ToList();

            return Task.FromResult(ObjectMapper.Map<List<Project>, List<ProjectDto>>(list));
        }

        public async Task<ProjectDto> Update(ProjectUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("项目名称不能为空");
            if (Guid.Empty == input.TypeId) throw new UserFriendlyException("请选择项目类型");
            if (Guid.Empty == input.OrganizationId) throw new UserFriendlyException("请选择建设单位");
            if (Guid.Empty == input.ManagerId) throw new UserFriendlyException("请选择负责人");
            if (string.IsNullOrEmpty(input.Scale)) throw new UserFriendlyException("建设规模不能为空");
            if (input.PlannedStartTime == null) throw new UserFriendlyException("请选择项目开始时间");
            if (input.PlannedEndTime == null) throw new UserFriendlyException("请选择项目结束时间");

            var ent = await _projectRepository.GetAsync(input.Id);
            if (ent == null) throw new UserFriendlyException("该项目信息不存在");

            ent.Name = input.Name;
            ent.ManagerId = input.ManagerId;
            ent.OrganizationId = input.OrganizationId;
            ent.Description = input.Description;
            ent.PlannedStartTime = input.PlannedStartTime;
            ent.PlannedEndTime = input.PlannedEndTime;
            ent.Area = input.Area;
            ent.Address = input.Address;
            ent.TypeId = input.TypeId;
            ent.State = input.State;
            ent.Remark = input.Remark;
            ent.Lat = input.Lat;
            ent.Lng = input.Lng;
            ent.DetailAddress = input.DetailAddress;
            ent.Cost = input.Cost;
            ent.Scale = input.Scale;
            await _projectRepository.UpdateAsync(ent);
            //删除合同关联关系
            await _projectRltContracts.DeleteAsync(x => x.ProjectId == ent.Id);
            foreach (var contractId in input.ProjectRltContractIds)
            {
                await _projectRltContracts.InsertAsync(new ProjectRltContract(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    ContractId = contractId
                });
            }
            //删除文件关联关系
            await _projectRltFile.DeleteAsync(x => x.ProjectId == ent.Id);
            foreach (var fileId in input.ProjectRltFileIds)
            {
                await _projectRltFile.InsertAsync(new ProjectRltFile(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    FileId = fileId
                });
            }
            //删除成员关联关系

            await _projectRltMember.DeleteAsync(x => x.ProjectId == ent.Id);
            foreach (var memberId in input.ProjectRltMemberIds)
            {
                await _projectRltMember.InsertAsync(new ProjectRltMember(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    ManagerId = memberId
                });
            }
            //删除参建单位关联关系
            using var unow = _unitOfWork.Begin(true, false);
            await _projectRltUnit.DeleteAsync(x => x.ProjectId == ent.Id);
            var unitList = new List<Guid>();
            foreach (var unit in input.Units)
            {
                var unitEnt = new Unit(_guidGenerator.Create())
                {
                    Code = unit?.Code,
                    Name = unit?.Name,
                    BankAccount = unit?.BankAccount,
                    BankCode = unit?.BankCode,
                    BankName = unit?.BankName,
                    Type = unit.Type,
                    Leader = unit.Leader
                };
                unitList.Add(unitEnt.Id);
                await _unitRepository.InsertAsync(unitEnt);
                await unow.SaveChangesAsync();
            };
            //保存参建单位关联
            foreach (var unitId in unitList)
            {
                await _projectRltUnit.InsertAsync(new ProjectRltUnit(_guidGenerator.Create())
                {
                    ProjectId = ent.Id,
                    UnitId = unitId
                });
                await unow.SaveChangesAsync();
            }
            return await System.Threading.Tasks.Task.FromResult(ObjectMapper.Map<Project, ProjectDto>(ent));
        }

        public async Task<bool> UpdateState(ProjectUpdateStateDto input)
        {
            var ent = await _projectRepository.GetAsync(input.Id);
            if (ent == null) throw new UserFriendlyException("该项目信息不存在");

            ent.State = input.State;

            return await System.Threading.Tasks.Task.FromResult(true);
        }

        [Produces("application/octet-stream")]
        public Task<Stream> Export(ExportDto input)
        {
            var projects = _projectRepository.WithDetails().Where(x => input.ProjectIds.Contains(x.Id)).ToList();
            var list = ObjectMapper.Map<List<Project>, List<Project>>(projects.OrderBy(x => x.CreationTime).ToList());
            Stream stream = null;
            byte[] sbuf;
            var dt = (DataTable)null;
            var dataColumn = (DataColumn)null;
            var dataRow = (DataRow)null;
            dt = new DataTable();
            //添加表头
            var enumValues = Enum.GetValues(typeof(ExportProject));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(ExportProject), item));
                    dt.Columns.Add(dataColumn);
                }
            }
            //添加内容
            foreach (var row in list)
            {
                dataRow = dt.NewRow();
                dataRow[ExportProject.项目名称.ToString()] = row.Name;
                dataRow[ExportProject.项目编号.ToString()] = row.Code;
                dataRow[ExportProject.负责人员.ToString()] = row.Manager.Name;
                dataRow[ExportProject.项目类型.ToString()] = row.Type?.Name;
                dataRow[ExportProject.建设单位.ToString()] = row.Organization?.Name;
                dataRow[ExportProject.项目状态.ToString()] = row.State.GetDescription();
                dataRow[ExportProject.计划开工.ToString()] = row.PlannedStartTime.ToString("yyyy-mm-dd");
                dataRow[ExportProject.计划竣工.ToString()] = row.PlannedEndTime.ToString("yyyy-mm-dd");
                dataRow[ExportProject.项目造价.ToString()] = row.Cost.ToString();
                dataRow[ExportProject.项目地址.ToString()] = row.Address;
                dataRow[ExportProject.详细地址.ToString()] = row.DetailAddress;
                dataRow[ExportProject.经纬度.ToString()] = row.Lng + row.Lat;
                dataRow[ExportProject.项目简介.ToString()] = row.Description;
                dataRow[ExportProject.备注.ToString()] = row.Remark;
                dt.Rows.Add(dataRow);
            }
            sbuf = ExcelHelper.DataTableToExcel(dt, "项目台账表.xlsx");
            stream = new MemoryStream(sbuf);
            return System.Threading.Tasks.Task.FromResult(stream);
        }
    }
}
