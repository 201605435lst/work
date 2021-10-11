using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceStoreEquipmentTestAppService : ResourceAppService, IResourceStoreEquipmentTestAppService
    {
        private readonly IRepository<StoreEquipmentTest, Guid> _storeEquipmentTestRepository;
        private IRepository<StoreEquipment, Guid> _storeEquipmentRepository;
        private IRepository<StoreEquipmentTestRltEquipment, Guid> _storeEquipmentTestRltEquipmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ResourceStoreEquipmentTestAppService(IRepository<StoreEquipmentTest, Guid> storeEquipmentTestRepository, IRepository<StoreEquipment, Guid> storeEquipmentRepository,
            //IRepository<StoreEquipmentTestRltEquipment, Guid> storeEquipmentTestRltEquipmentRepository, IRepository<StoreEquipmentTestRltEquipment, Guid> storeEquipmentTestRltEquipmentRepository,
            IGuidGenerator guidGenerator, IUnitOfWorkManager unitOfWorkManager)
        {
            _storeEquipmentTestRepository = storeEquipmentTestRepository;
            _storeEquipmentRepository = storeEquipmentRepository;
            //_storeEquipmentTestRltEquipmentRepository = storeEquipmentTestRltEquipmentRepository;
            _guidGenerator = guidGenerator;
            _unitOfWorkManager = unitOfWorkManager;
        }

        /// <summary>
        /// 创建检测单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ResourcePermissions.StoreEquipmentTest.Create)]
        public async Task<StoreEquipmentTestDto> Create(StoreEquipmentTestCreateDto input)
        {
            if (input.StoreEquipmentTestRltEquipments.Count() <= 0) throw new UserFriendlyException("请传入待检设备，请检查后输入");
            if (string.IsNullOrEmpty(input.OrganizationName)) throw new UserFriendlyException("检测机构名称为空，请检查后输入");
            if (string.IsNullOrEmpty(input.Address)) throw new UserFriendlyException("检测地点为空，请检查后输入");
            if (input.Date == null) throw new UserFriendlyException("检测时间为空，请检查后输入");
            if (string.IsNullOrEmpty(input.Content)) throw new UserFriendlyException("检测内容为空，请检查后输入");
            if (string.IsNullOrEmpty(input.TesterName)) throw new UserFriendlyException("检测人姓名为空，请检查后输入");

            var storeEquipmentTest = new StoreEquipmentTest(_guidGenerator.Create());
            if (SaveCheckSameCode(input.Code))
            {
                storeEquipmentTest.Code = input.Code;
                storeEquipmentTest.OrganizationId = input.OrganizationId;
                storeEquipmentTest.OrganizationName = input.OrganizationName;
                storeEquipmentTest.Address = input.Address;
                storeEquipmentTest.TesterId = input.TesterId;
                storeEquipmentTest.TesterName = input.TesterName;
                storeEquipmentTest.Passed = input.Passed;
                storeEquipmentTest.Date = input.Date;
                storeEquipmentTest.Content = input.Content;
                //重新保存关联库存设备信息
                storeEquipmentTest.StoreEquipmentTestRltEquipments = new List<StoreEquipmentTestRltEquipment>();

                foreach (var storeEquipmentTestRltEquipment in input.StoreEquipmentTestRltEquipments)
                {
                    //重新修改库存设备的状态
                    var storeEquipment = await _storeEquipmentRepository.GetAsync(storeEquipmentTestRltEquipment.StoreEquipmentId);
                    storeEquipment.State = (input.Passed == true) ? (Enums.StoreEquipmentState.Spare) : (Enums.StoreEquipmentState.Scrap);
                    await _storeEquipmentRepository.UpdateAsync(storeEquipment);
                    storeEquipmentTest.StoreEquipmentTestRltEquipments.Add(new StoreEquipmentTestRltEquipment(_guidGenerator.Create())
                    {
                        StoreEquipmentId = storeEquipmentTestRltEquipment.StoreEquipmentId
                    });
                }

                
            }

            await _storeEquipmentTestRepository.InsertAsync(storeEquipmentTest);

            return ObjectMapper.Map<StoreEquipmentTest, StoreEquipmentTestDto>(storeEquipmentTest);

        }


        /// <summary>
        /// 根据id获取单个检测单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<StoreEquipmentTestDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入id");
            var storeEquipmentTest = _storeEquipmentTestRepository
                .WithDetails()
                .Where(x => x.Id == id).FirstOrDefault();
            if (storeEquipmentTest == null) throw new UserFriendlyException("不存在此id的设备检测单");
            return Task.FromResult(ObjectMapper.Map<StoreEquipmentTest, StoreEquipmentTestDto>(storeEquipmentTest));
        }

        /// <summary>
        /// 获取检测单的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<StoreEquipmentTestDto>> GetList(StoreEquipmentTestSearchDto input)
        {
            //利用外键获取别的模块的匹配字段
            var storeEquipmentTests = _storeEquipmentTestRepository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Code), x => x.Code.Contains(input.Code))
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Date >= input.StartTime && x.Date <= input.EndTime)
                .WhereIf(input.Passed.HasValue, x => x.Passed == input.Passed)
                .WhereIf(input.OrganizationId!=null && input.OrganizationId!=Guid.Empty,x=>x.OrganizationId==input.OrganizationId)
                .WhereIf(input.StoreHouseId != null && input.StoreHouseId != Guid.Empty , x=>x.StoreEquipmentTestRltEquipments.Any(rlt => rlt.StoreEquipment.StoreHouseId == input.StoreHouseId))
                .WhereIf(!string.IsNullOrEmpty(input.KeyWord), x =>
                     x.Content.Contains(input.KeyWord) ||
                     x.StoreEquipmentTestRltEquipments.Any(rlt => rlt.StoreEquipment.Name.Contains(input.KeyWord) || rlt.StoreEquipment.ProductCategory.Name.Contains(input.KeyWord))
                );

            var result = new PagedResultDto<StoreEquipmentTestDto>();
            result.TotalCount = storeEquipmentTests.Count();
            result.Items = ObjectMapper.Map<List<StoreEquipmentTest>, List<StoreEquipmentTestDto>>(storeEquipmentTests.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return Task.FromResult(result);

        }

        // 检查是否有相同编号的检测单
        private bool SaveCheckSameCode(string code)
        {
            var sameStoreEquipmentTest = _storeEquipmentTestRepository.Where(o => o.Code.ToUpper() == code.ToUpper());
            if (sameStoreEquipmentTest.Count() > 0)
            {
                throw new UserFriendlyException("当前检测单编号重复");
            }
            return true;
        }



    }
}
