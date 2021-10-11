using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using SnAbp.Common;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using SnAbp.StdBasic.TempleteModel;
using SnAbp.Utils;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    public class StdBasicRevitConnectorAppService : StdBasicAppService, IStdBasicRevitConnectorAppService
    {
        private readonly IRepository<RevitConnector, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IFileImportHandler _fileImport;

        public StdBasicRevitConnectorAppService(IRepository<RevitConnector, Guid> repository, IGuidGenerator guidGenerator, IFileImportHandler fileImport
       )
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
            _fileImport = fileImport;
        }

        //public async Task<PagedResultDto<ModelFileRltConnectorDto>> EditList(ModelFileRltConnectorCreateDto input)
        //{

        //    if (input.ModelFileId == null || input.ModelFileId == Guid.Empty) throw new UserFriendlyException("请输入正确的模型id");
        //    if (input.list?.Count > 0)
        //    {
        //        var error = input.list.Find(x => !StringUtil.CheckSpaceValidity(x.Name));
        //        if (error != null)
        //        {
        //            throw new UserFriendlyException("名称包含空格，请修改");
        //        }
        //    }
        //    var res = new PagedResultDto<ModelFileRltConnectorDto>();
        //    var rltItems = new List<RevitConnector>();

        //    await Task.Run(() =>
        //    {
        //        var allEnt = _repository.WithDetails()
        //               .Where(x => input.ModelFileId != null && x.ModelFileId == input.ModelFileId).ToList();
        //        if (allEnt?.Count > 0)
        //        {
        //            //删除数据
        //            allEnt.ForEach(m =>
        //            {
        //                _repository.DeleteAsync(m);
        //            });

        //        }
        //        if (input.list?.Count > 0)
        //        {
        //            List<RevitConnector> rltList = new List<RevitConnector>();
        //            //添加数据
        //            input.list.ForEach(m =>
        //            {
        //                RevitConnector model = new RevitConnector(_guidGenerator.Create());
        //                model.ModelFileId = input.ModelFileId;
        //                model.Name = m.Name;
        //                model.Position = m.Position;
        //                _repository.InsertAsync(model);

        //                if (rltList?.Count > 0)
        //                {
        //                    rltList.ForEach(a =>
        //                    {
        //                        if (a.Name == m.Name)
        //                        {
        //                            throw new UserFriendlyException(a.Name + "重名连接件，请修改");
        //                        }
        //                    });
        //                }
        //                rltList.Add(model);
        //            });

        //            res.TotalCount = rltList.Count();
        //            if (input.IsAll == false)
        //            {

        //                rltItems = rltList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

        //            }
        //            else
        //            {
        //                rltItems = rltList;
        //            }
        //            var items = new List<ModelFileRltConnectorDto>();
        //            rltItems.ForEach(s =>
        //            {
        //                ModelFileRltConnectorDto model = new ModelFileRltConnectorDto();
        //                model.Id = s.Id;
        //                model.ModelFileId = s.ModelFileId;
        //                model.Name = s.Name;
        //                model.Position = s.Position;
        //                items.Add(model);
        //            });
        //            res.Items = items;
        //        }
        //        else
        //        {
        //            res.TotalCount = 0;
        //        }

        //    });
        //    return res;
        //}

        public async Task<bool> Get(Guid id, string name)
        {
            var entity = _repository.WithDetails().Where(x => x.ModelFileId == id).ToList();
            RevitConnector connector = null;
            if (entity.Count > 0)
                connector = entity.FirstOrDefault(x => x.Name == name);
            if (connector != null)
                return true;
            return false;
        }

        public async Task<ModelFileRltConnectorDto> Create(ModelFileRltConnectorCreateDto input)
        {
            if (!StringUtil.CheckSpaceValidity(input.Name))
                throw new UserFriendlyException("连接件名称不能包含空格");
            if (_repository.Any(x => x.Name == input.Name && !string.IsNullOrEmpty(input.Name)))
                throw new UserFriendlyException(input.Name + "连接件已存在,请修改!");

            var entity = new RevitConnector(input.Id)
            {
                Name = input.Name,
                Position = input.Position,
                ModelFileId = input.ModelFileId,
            };
            await _repository.InsertAsync(entity);
            return ObjectMapper.Map<RevitConnector, ModelFileRltConnectorDto>(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<ModelFileRltConnectorDto> Update(ModelFileRltConnectorDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("请输入名称");
            if (!StringUtil.CheckSpaceValidity(input.Name))
            {
                throw new UserFriendlyException("连接件名称不能包含空格");
            }
            if (_repository.Any(x =>!string.IsNullOrEmpty(input.Name) && x.Name == input.Name&&x.Id!=input.Id ))
                throw new UserFriendlyException(input.Name + "连接件已存在,请修改!");
            var entity = await _repository.GetAsync(input.Id);
            if (entity == null) throw new UserFriendlyException("此连接件不存在");
            entity.Name = input.Name;
            entity.Position = input.Position;
            await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<RevitConnector, ModelFileRltConnectorDto>(entity);
        }

        public Task<PagedResultDto<ModelFileRltConnectorDto>> GetListByModelFileId(ModelFileRltConnectorSearchDto input)
        {
            var entityList = _repository.WithDetails().Where(x => x.ModelFileId == input.ModelFileId);
            var res = new PagedResultDto<ModelFileRltConnectorDto>();

            res.TotalCount = entityList.Count();
            var entities = new List<RevitConnector>();
            entities = input.IsAll ? entityList.ToList() : entityList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var items = new List<ModelFileRltConnectorDto>();
            entities.ForEach(x =>
            {
                var model = new ModelFileRltConnectorDto();
                model.Id = x.Id;
                model.ModelFileId = x.ModelFileId;
                model.Name = x.Name;
                model.Position = x.Position;
                items.Add(model);
            });
            res.Items = items;
            return Task.FromResult(res);
        }

        /// <summary>
        /// Revit连接件导入
        /// </summary>
        /// <param name="input"></param>
        /// <param name="modelFileId"></param>
        /// <returns></returns>
        public async Task Upload([FromForm] ImportData input, Guid modelFileId)
        {

            //虚拟进度0%
            await _fileImport.Start(input.ImportKey, 100);
            // 获取excel表格，判断表格是否满足模板
            var rowIndex = 5;  //有效数据得起始索引
            IWorkbook workbook = null;
            ISheet sheet = null;
            List<RevitConnectorTemplate> datalist = null;

            try
            {
                workbook = input.File.ConvertToWorkbook();
                sheet = workbook.GetSheetAt(0).CheckColumnAccordTempleModel<RevitConnectorTemplate>(rowIndex);
                datalist = sheet
                    .TryTransToList<RevitConnectorTemplate>(rowIndex)
                    .CheckNull();
            }
            catch (Exception)
            {
                await _fileImport.Cancel(input.ImportKey);
                throw new UserFriendlyException("所选文件有错误，请重新选择");
            }

            // 定义错误信息列
            List<WrongInfo> wrongInfos = new List<WrongInfo>();
            RevitConnector RevitConnector;
            if (datalist.Any())
            {
                await _fileImport.ChangeTotalCount(input.ImportKey, datalist.Count());
                var updateIndex = 1;
                foreach (var item in datalist)
                {
                    await _fileImport.UpdateState(input.ImportKey, updateIndex);
                    updateIndex++;
                    var canInsert = true;
                    var newInfo = new WrongInfo(item.Index);
                    if (item.Name.IsNullOrEmpty())
                    {
                        canInsert = false;
                        newInfo.AppendInfo("名称为空");
                    }
                    if (!canInsert)
                    {
                        wrongInfos.Add(newInfo);
                        continue;
                    }
                    //using var uow = _unitOfWork.Begin(true, false);
                    //using (_dataFilter.Disable<ISoftDelete>())
                    //{ }
                    RevitConnector = _repository.Where(x => x.ModelFileId == modelFileId).FirstOrDefault(a => a.Name == item.Name);
                    if (RevitConnector != null)
                    {
                        newInfo.AppendInfo($"{item.Name}已存在，且已更新");
                        RevitConnector.Name = item.Name;
                        RevitConnector.Position = item.Position;
                        await _repository.UpdateAsync(RevitConnector);
                        //await uow.SaveChangesAsync();
                        wrongInfos.Add(newInfo);
                    }
                    else
                    {
                        RevitConnector = new RevitConnector(_guidGenerator.Create());
                        RevitConnector.ModelFileId = modelFileId;
                        RevitConnector.Name = item.Name;
                        RevitConnector.Position = item.Position;
                        await _repository.InsertAsync(RevitConnector);
                        //await uow.SaveChangesAsync();
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

        public Task<Stream> Export(RevitConnectorData input)
        {
            var list = _repository.WithDetails().Where(x => x.ModelFileId == input.Paramter.ModelFileId);
            var dtoList = ObjectMapper.Map<List<RevitConnector>, List<RevitConnectorTemplate>>(list.ToList());
            var stream = ExcelHelper.ExcelExportStream(dtoList, input.TemplateKey, input.RowIndex);
            return Task.FromResult(stream);
        }
    }
}
