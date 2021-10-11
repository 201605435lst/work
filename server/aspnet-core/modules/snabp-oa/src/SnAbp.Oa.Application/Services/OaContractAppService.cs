using Microsoft.AspNetCore.Authorization;
using SnAbp.Oa.Dtos;
using SnAbp.Oa.Entities;
using SnAbp.Oa.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Oa.IServices;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using SnAbp.Oa.Enums;
using SnAbp.Utils.EnumHelper;
using SnAbp.Oa.Commons;

namespace SnAbp.Oa.Services
{
    [Authorize]
    public class OaContractAppService : OaAppService, IOaContractAppService
    {
        private readonly IRepository<Contract, Guid> _repositoryContract;
        private readonly IRepository<ContractRltFile, Guid> _repositoryContractRltFile;
        private readonly IGuidGenerator _guidGenerate;

        public OaContractAppService(
             IRepository<Contract, Guid> repositoryContract,
              IRepository<ContractRltFile, Guid> repositoryContractRltFile,
            IGuidGenerator guidGenerate)
        {
            _repositoryContract = repositoryContract;
            _repositoryContractRltFile = repositoryContractRltFile;
            _guidGenerate = guidGenerate;
        }
        #region Get接口==========================================
        /// <summary>
        /// 获取单个合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ContractDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的合同");
            var contract = _repositoryContract.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (contract == null) throw new UserFriendlyException("当前合同不存在");
            var result = ObjectMapper.Map<Contract, ContractDto>(contract);
            return Task.FromResult(result);
        }
        #endregion


        #region 获取最大的编码==========================================
        /// <summary>
        /// 获取编号最大的合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ContractDto> GetMaxCode()
        {
            var list = _repositoryContract.OrderBy(x => x.Code).ToList();
            var dtos = Task.FromResult(ObjectMapper.Map<Contract, ContractDto>(list.Count > 0 ? list.Last() : null));
            return dtos;
        }
        #endregion


        #region GetList接口==========================================
        /// <summary>
        /// 获取合同列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<ContractDto>> GetList(ContractSearchDto input)
        {

            var result = new PagedResultDto<ContractDto>();
            var contract = _repositoryContract.WithDetails()
                .WhereIf(!String.IsNullOrEmpty(input.KeyWords), x => x.Name.Contains(input.KeyWords) || x.Code.Contains(input.KeyWords));
            if (input.Order == "descend")
            {
                result.Items = ObjectMapper.Map<List<Contract>, List<ContractDto>>(contract.OrderByDescending(x => input.ColumnKey == "creationTime" ? x.CreationTime : x.SignTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            if (input.Order == "ascend")
            {
                result.Items = ObjectMapper.Map<List<Contract>, List<ContractDto>>(contract.OrderBy(x => input.ColumnKey == "creationTime" ? x.CreationTime : x.SignTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            result.TotalCount = contract.Count();
            return Task.FromResult(result);
        }
        #endregion

        #region 导出Excel文件==========================================
        [Authorize(OaPermissions.Contract.Export)]
        [Produces("application/octet-stream")]
        public Task<Stream> DownLoad(List<Guid> ids)
        {
            Stream stream = null;
            byte[] sbuf;
            var dataTable = (DataTable)null;
            var dataColumn = (DataColumn)null;
            var dataRow = (DataRow)null;
            var contract = _repositoryContract.WithDetails()
                          .WhereIf(ids.Count > 0, x => ids.Contains(x.Id));
            var list = ObjectMapper.Map<List<Contract>, List<ContractDto>>(contract.OrderBy(x => x.SignTime).ToList());
            if (list.Count == 0) throw new UserFriendlyException("未找到任何导出数据");
            dataTable = new DataTable();

            //添加列表
            var enumValues = Enum.GetValues(typeof(Enums.ContractExcelCol));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(Enums.ContractExcelCol), item));
                    dataTable.Columns.Add(dataColumn);
                }
            }
            //添加数据
            foreach (var item in list)
            {
                dataRow = dataTable.NewRow();
                dataRow[ContractExcelCol.合同编号.ToString()] = item.Code;
                dataRow[ContractExcelCol.合同名称.ToString()] = item.Name;
                dataRow[ContractExcelCol.合同甲方.ToString()] = item.PartyA;
                dataRow[ContractExcelCol.合同乙方.ToString()] = item.PartyB;
                dataRow[ContractExcelCol.合同丙方.ToString()] = item.PartyC;
                dataRow[ContractExcelCol.主办部门.ToString()] = item.HostDepartment.Name;
                dataRow[ContractExcelCol.承办部门.ToString()] = item.UnderDepartment.Name;
                dataRow[ContractExcelCol.承办人.ToString()] = item.Undertaker.Name;
                dataRow[ContractExcelCol.合同金额.ToString()] = item.Amount;
                dataRow[ContractExcelCol.成本预算.ToString()] = item.Budge;
                dataRow[ContractExcelCol.签订时间.ToString()] = item.SignTime.ToString("yyyy-MM-dd hh:mm:ss");
                dataRow[ContractExcelCol.合同类型.ToString()] = item.Type.Name;
                dataTable.Rows.Add(dataRow);
            }
            sbuf = ExcelHepler.DataTableToExcel(dataTable, "合同管理表.xlsx", new List<int>() { 0 });
            stream = new MemoryStream(sbuf);
            return Task.FromResult(stream);
        }

        #endregion

        #region 创建合同==========================================
        /// <summary>
        /// 创建合同
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(OaPermissions.Contract.Create)]
        public async Task<ContractDto> Create(ContractCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("合同编号不能为空");
            CheckSameCode(input.Code,null);
            var contract = new Contract();
            ObjectMapper.Map(input, contract);
            contract.SetId(_guidGenerate.Create());
            contract.ContractRltFiles = new List<ContractRltFile>();
            foreach (var contractRltFiles in input.ContractRltFiles)
            {
                contract.ContractRltFiles.Add(
                    new ContractRltFile(_guidGenerate.Create())
                    {
                        FileId = contractRltFiles.FileId,
                        ContractId = contract.Id,
                    });
            }
            await _repositoryContract.InsertAsync(contract);
            return ObjectMapper.Map<Contract, ContractDto>(contract);
        }
        #endregion


        #region 修改合同==========================================
        /// <summary>
        /// 修改合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(OaPermissions.Contract.Update)]
        public async Task<ContractDto> Update(ContractUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的合同");
            var contract = await _repositoryContract.GetAsync(input.Id);
            if (contract == null) throw new UserFriendlyException("当前合同不存在");
            //清除之前保存的关联表信息
            await _repositoryContractRltFile.DeleteAsync(x => x.ContractId == input.Id);
            contract.Abstract = input.Abstract;
            contract.Amount = input.Amount;
            contract.Budge = input.Budge;
            contract.AmountWords = input.AmountWords;
            contract.Budge = input.Budge;
            contract.Code = input.Code;
            contract.Name = input.Name;
            contract.PartyA = input.PartyA;
            contract.PartyB = input.PartyB;
            contract.PartyC = input.PartyC;
            contract.SignTime = input.SignTime;
            contract.TypeId = input.TypeId;
            contract.OtherPartInfo = input.OtherPartInfo;
            contract.HostDepartmentId = input.HostDepartmentId;
            contract.UnderDepartmentId = input.UnderDepartmentId;
            contract.UndertakerId = input.UndertakerId;
            CheckSameCode(input.Code, input.Id);
            //重新保存关联表信息
            contract.ContractRltFiles = new List<ContractRltFile>();
            foreach (var contractRltFiles in input.ContractRltFiles)
            {
                contract.ContractRltFiles.Add(
                    new ContractRltFile(_guidGenerate.Create())
                    {
                        FileId = contractRltFiles.FileId,
                    });
            }
            await _repositoryContract.UpdateAsync(contract);
            return ObjectMapper.Map<Contract, ContractDto>(contract);
        }
        #endregion


        #region 删除合同==========================================
        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(OaPermissions.Contract.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repositoryContract.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
        #endregion


        #region 私有方法==========================================
        private bool CheckSameCode(string code, Guid? id)
        {
            var contract = _repositoryContract.Where(x => x.Code == code).WhereIf(id == null && id != Guid.Empty, x => x.Id == id);
            if (contract.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前合同编号已存在！！！");
            }
            return true;
        }
        #endregion

    }
}
