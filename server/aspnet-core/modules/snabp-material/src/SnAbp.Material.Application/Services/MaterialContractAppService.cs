/**********************************************************************
*******命名空间： SnAbp.Material.Services
*******类 名 称： MaterialContractAppService
*******类 说 明： 物资合同服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:34:00
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using NPOI.HSSF.Util;

using SnAbp.Material.Dto;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;
using SnAbp.Material.Enums;
using SnAbp.Utils;
using SnAbp.Utils.DataExport;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Material.Services
{
    /// <summary>
    /// 物资合同服务
    /// </summary>
    public class MaterialContractAppService : MaterialAppService, IApplicationService
    {
        IRepository<Contract> _contractRepository;
        IRepository<ContractRltFile> _contractRltFileRepository;
        IGuidGenerator _guidGenerator;
        public MaterialContractAppService(
            IRepository<Contract> contractRepository,
            IRepository<ContractRltFile> contractRltFileRepository,
            IGuidGenerator guidGenerator
            )
        {
            _contractRepository = contractRepository;
            _contractRltFileRepository = contractRltFileRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取合同列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<MaterialContractDto>> GetList(ContractSearchDto input)
        {
            var context = _contractRepository
                .WithDetails()
                .WhereIf(!input.KeyWords.IsNullOrEmpty(), a => a.Name.Contains(input.KeyWords) || a.Code.Contains(input.KeyWords));
            var result = new PagedResultDto<MaterialContractDto>();
            result.Items = ObjectMapper.Map<List<Contract>, List<MaterialContractDto>>(context.PageBy(input.SkipCount, input.MaxResultCount).ToList());
            result.TotalCount = context.Count();
            return result;
        }
        [UnitOfWork]
        /// <summary>
        /// 创建物资合同
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(MaterialContractCreateDto input)
        {
            var entity = ObjectMapper.Map<MaterialContractCreateDto, Contract>(input);
            entity.SetId(_guidGenerator.Create());
            entity.Code = $"WZHT{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            await _contractRepository.InsertAsync(entity);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            if (input.FileIds != null && input.FileIds.Any())
            {

                input.FileIds.ForEach(a =>
                {
                    var rltEntity = new ContractRltFile(entity.Id, a);
                    _contractRltFileRepository.InsertAsync(rltEntity);
                });
            }
            return true;
        }

        [UnitOfWork]
        public async Task<bool> Update(MaterialContractCreateDto input)
        {
            var contract = _contractRepository.WithDetails(a => a.Files).FirstOrDefault(a => a.Id == input.Id);
            contract.Name = input.Name;
            contract.Date = input.Date;
            contract.Amount = input.Amount;
            contract.Remark = input.Remark;
            await _contractRepository.UpdateAsync(contract);
            await _contractRltFileRepository.DeleteAsync(a => a.ContractId == contract.Id);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            if (input.FileIds != null && input.FileIds.Any())
            {
                input.FileIds.ForEach(a =>
                {
                    var rltEntity = new ContractRltFile(contract.Id, a);
                    _contractRltFileRepository.InsertAsync(rltEntity);
                });
            }
            return true;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _contractRepository.DeleteAsync(a => a.Id == id);
            return true;
        }
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            await _contractRepository.DeleteAsync(a => ids.Contains(a.Id));
            return default;
        }
        /// <summary>
        /// 文件导出
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<Stream> Export(List<Guid> ids)
        {

            if (ids.Any())
            {
                var list = _contractRepository
                    .WithDetails(a => a.Creator)
                    .Where(a => ids.Contains(a.Id))
                    .ToList();
                var handler = DataExportHandler.CreateExcelFile(Utils.ExcelHelper.ExcelFileType.Xlsx);
                handler.CreateSheet("物资合同");
                var titles = new string[]
                {
                    "编号",
                    "合同编号",
                    "合同名称",
                    "合同日期",
                    "合同金额（万）",
                    "上传人",
                    "上传时间",
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
                    row.CreateCell(1).SetCellStyle(cellStyle).SetCellValue(a.Code);
                    row.CreateCell(2).SetCellStyle(cellStyle).SetCellValue(a.Name);
                    row.CreateCell(3).SetCellStyle(cellStyle).SetCellValue(a.Date.ToString("yyyy-MM-dd"));
                    row.CreateCell(4).SetCellStyle(cellStyle).SetCellValue((double)a.Amount);
                    row.CreateCell(5).SetCellStyle(cellStyle).SetCellValue(a.Creator?.UserName);
                    row.CreateCell(6).SetCellStyle(cellStyle).SetCellValue(a.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                });
                Stream stream = handler.GetExcelDataBuffer().BytesToStream();
                return Task.FromResult(stream);
            }
            else
            {
                return default;
            }
        }

        public Task<List<File.FileDomainDto>> GetFileByIds(Guid id)
        {
            var files = _contractRltFileRepository.WithDetails(a => a.File)
                .Where(a => a.ContractId == id).Select(a => a.File).ToList();
            return Task.FromResult(ObjectMapper.Map<List<File.Entities.File>, List<File.FileDomainDto>>(files));
        }
    }
}
