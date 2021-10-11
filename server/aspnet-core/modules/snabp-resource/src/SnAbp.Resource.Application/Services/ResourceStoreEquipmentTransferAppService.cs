

using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.Enums;
using SnAbp.Resource.IServices.StoreEquipmentTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceStoreEquipmentTransferAppService : ResourceAppService, IResourceStoreEquipmentTransferAppService
    {
        private readonly IRepository<StoreEquipment, Guid> _storeEquipmentRepository;
        private readonly IRepository<StoreEquipmentTransfer, Guid> _storeEquipmentTransferRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;

        public ResourceStoreEquipmentTransferAppService(
            IRepository<StoreEquipment, Guid> storeEquipmentRepository,
            IRepository<StoreEquipmentTransfer, Guid> storeEquipmentTransferRepository,
            IGuidGenerator guidGenerator,
            IDataFilter dataFilter)
        {
            _storeEquipmentRepository = storeEquipmentRepository;
            _storeEquipmentTransferRepository = storeEquipmentTransferRepository;
            _guidGenerator = guidGenerator;
            _dataFilter = dataFilter;
        }
        /// <summary>
        /// 获取单个记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StoreEquipmentTransferDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var storeEquipmentTransfer = _storeEquipmentTransferRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (storeEquipmentTransfer == null) throw new UserFriendlyException("此信息不存在");
            return Task.FromResult(ObjectMapper.Map<StoreEquipmentTransfer, StoreEquipmentTransferDto>(storeEquipmentTransfer));
        }
        /// <summary>
        /// 获取出入库信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreEquipmentTransferDto>> GetList(StoreEquipmentTransferSearchDto input)
        {
            PagedResultDto<StoreEquipmentTransferDto> result = new PagedResultDto<StoreEquipmentTransferDto>();
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var storeEquipmentTransfer = _storeEquipmentTransferRepository.WithDetails()
                .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.StoreHouse.OrganizationId == input.OrganizationId)
                 .WhereIf(input.StoreHouseId != null && input.StoreHouseId != Guid.Empty, x => x.StoreHouseId == input.StoreHouseId)
                 .WhereIf(input.StartTime != null && input.EndTime != null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
                 .WhereIf(input.Type.IsIn(StoreEquipmentTransferType.Export, StoreEquipmentTransferType.Import), x => x.Type == input.Type)
                 .WhereIf(!String.IsNullOrEmpty(input.KeyWord), x =>
                  x.UserName.Contains(input.KeyWord) ||
                  x.User.Name.Contains(input.KeyWord) ||
                  x.Remark.Contains(input.KeyWord) ||
                  x.StoreEquipmentTransferRltEquipments.Select(s => s.StoreEquipment).Any(m => m.Code.Contains(input.KeyWord)) ||
                  x.StoreEquipmentTransferRltEquipments.Select(s => s.StoreEquipment).Any(m => m.Name.Contains(input.KeyWord)) ||
                  x.StoreEquipmentTransferRltEquipments.Select(s => s.StoreEquipment).Select(p => p.ProductCategory).Any(
                      m => m.Name.Contains(input.KeyWord) ||
                      m.Parent.Name.Contains(input.KeyWord) ||
                      m.Parent.Parent.Name.Contains(input.KeyWord) ||
                      m.Parent.Parent.Parent.Name.Contains(input.KeyWord) ||
                      m.Parent.Parent.Parent.Parent.Name.Contains(input.KeyWord)
                      )
                  );
                var dtos = ObjectMapper.Map<List<StoreEquipmentTransfer>, List<StoreEquipmentTransferDto>>(storeEquipmentTransfer.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                result.Items = dtos;
                result.TotalCount = storeEquipmentTransfer.Count();
            }
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取要入库的设备
        /// </summary>
        /// <returns></returns>
        public Task<List<StoreEquipmentSimpleDto>> GetEquipmentImport(StoreEquipmentSimpleSearchDto input)
        {
            var storeEquipment = _storeEquipmentRepository.WithDetails()
                .Where(x => x.Code == input.Code)
                .Where(x => x.StoreHouseId == null);
            var result = new List<StoreEquipmentSimpleDto>();
            result = ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentSimpleDto>>(storeEquipment.OrderByDescending(x => x.CreationTime).ToList());
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取要出库的设备
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<StoreEquipmentSimpleDto>> GetEquipmentExport(StoreEquipmentExportDto input)
        {
            var result = new List<StoreEquipmentSimpleDto>();
            if ((input.StoreHouseId != null && input.StoreHouseId != Guid.Empty) || !string.IsNullOrEmpty(input.Code))
            {
                var storeEquipment = _storeEquipmentRepository.WithDetails()
                                 .WhereIf(input.StoreHouseId != null && input.StoreHouseId == Guid.Empty && !string.IsNullOrEmpty(input.Code), x => x.StoreHouseId != null && x.Code == input.Code)

                                  .WhereIf(input.StoreHouseId != null && input.StoreHouseId != Guid.Empty && string.IsNullOrEmpty(input.Code), x => x.StoreHouseId != null && x.StoreHouseId == input.StoreHouseId)
                                    .WhereIf(input.StoreHouseId != null && input.StoreHouseId != Guid.Empty && !string.IsNullOrEmpty(input.Code), x => x.StoreHouseId != null && x.StoreHouseId == input.StoreHouseId && x.Code == input.Code);
                result = ObjectMapper.Map<List<StoreEquipment>, List<StoreEquipmentSimpleDto>>(storeEquipment.OrderByDescending(x => x.CreationTime).ToList());
            }



            return Task.FromResult(result);
        }
        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreEquipmentTransfer.Create)]
        public async Task<StoreEquipmentTransferDto> Import(StoreEquipmentTransferSimpleDto input)
        {
            if (input.StoreHouseId == null || input.StoreHouseId == Guid.Empty) throw new UserFriendlyException("仓库不能为空");

            if (String.IsNullOrEmpty(input.UserName) && input.UserId ==null)
            {
                    throw new UserFriendlyException("入库人不能为空");
            }

            if (String.IsNullOrEmpty(input.Remark)) throw new UserFriendlyException("备注不能为空");
            if (input.storeEquipmentTransferRltEquipments.Count == 0) throw new UserFriendlyException("入库设备不能为空");
            var storeEquipmentTransfer = new StoreEquipmentTransfer(_guidGenerator.Create());
            storeEquipmentTransfer.UserId = input.UserId;
            storeEquipmentTransfer.UserName = input.UserName;
            storeEquipmentTransfer.StoreHouseId = input.StoreHouseId;
            storeEquipmentTransfer.Remark = input.Remark;
            storeEquipmentTransfer.Type = StoreEquipmentTransferType.Import;
            storeEquipmentTransfer.StoreEquipmentTransferRltEquipments = new List<StoreEquipmentTransferRltEquipment>();
            foreach (var storeEquipmentTransferRltEquipments in input.storeEquipmentTransferRltEquipments)
            {
                storeEquipmentTransfer.StoreEquipmentTransferRltEquipments.Add(new StoreEquipmentTransferRltEquipment(_guidGenerator.Create())
                {
                    StoreEquipmentId = storeEquipmentTransferRltEquipments.StoreEquipmentId,
                }
                );
                var storeEquipment = await _storeEquipmentRepository.GetAsync(storeEquipmentTransferRltEquipments.StoreEquipmentId);
                if (storeEquipment == null) throw new UserFriendlyException("该信息不存在");
                if (storeEquipment.StoreHouseId == null)
                {
                    storeEquipment.StoreHouseId = input.StoreHouseId;
                    await _storeEquipmentRepository.UpdateAsync(storeEquipment);
                }

            }
            await _storeEquipmentTransferRepository.InsertAsync(storeEquipmentTransfer);
            return ObjectMapper.Map<StoreEquipmentTransfer, StoreEquipmentTransferDto>(storeEquipmentTransfer);

        }
        /// <summary>
        /// 出库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreEquipmentTransfer.Create)]
        public async Task<StoreEquipmentTransferDto> Export(StoreEquipmentTransferSimpleDto input)
        {
            if (input.StoreHouseId == null || input.StoreHouseId == Guid.Empty) throw new UserFriendlyException("仓库不能为空");
            if (String.IsNullOrEmpty(input.UserName) && input.UserId == null )
            {
                throw new UserFriendlyException("出库人不能为空");
            }
            if (String.IsNullOrEmpty(input.Remark)) throw new UserFriendlyException("备注不能为空");
            if (input.storeEquipmentTransferRltEquipments.Count == 0) throw new UserFriendlyException("出库设备不能为空");
            var storeEquipmentTransfer = new StoreEquipmentTransfer(_guidGenerator.Create());
            storeEquipmentTransfer.UserId = input.UserId;
            storeEquipmentTransfer.UserName = input.UserName;
            storeEquipmentTransfer.StoreHouseId = input.StoreHouseId;
            storeEquipmentTransfer.Remark = input.Remark;
            storeEquipmentTransfer.Type = StoreEquipmentTransferType.Export;
            storeEquipmentTransfer.StoreEquipmentTransferRltEquipments = new List<StoreEquipmentTransferRltEquipment>();
            foreach (var storeEquipmentTransferRltEquipments in input.storeEquipmentTransferRltEquipments)
            {
                storeEquipmentTransfer.StoreEquipmentTransferRltEquipments.Add(new StoreEquipmentTransferRltEquipment(_guidGenerator.Create())
                {
                    StoreEquipmentId = storeEquipmentTransferRltEquipments.StoreEquipmentId,
                }
                );
                var storeEquipment = await _storeEquipmentRepository.GetAsync(storeEquipmentTransferRltEquipments.StoreEquipmentId);
                if (storeEquipment == null) throw new UserFriendlyException("该信息不存在");
                if (storeEquipment.StoreHouseId != null)
                {
                    storeEquipment.StoreHouseId = null;
                    await _storeEquipmentRepository.UpdateAsync(storeEquipment);
                }

            }
            await _storeEquipmentTransferRepository.InsertAsync(storeEquipmentTransfer);
            return ObjectMapper.Map<StoreEquipmentTransfer, StoreEquipmentTransferDto>(storeEquipmentTransfer);
        }



        //public async Task<bool> Delate(Guid id)
        //{
        //    if (id == null || id != Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
        //    var storeEquipmentTransfer = _storeEquipmentTransferRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
        //    if (storeEquipmentTransfer == null) throw new UserFriendlyException("此信息不存在");
        //    await _storeEquipmentTransferRepository.DeleteAsync(id);
        //    return true;
        //}

    }
}
