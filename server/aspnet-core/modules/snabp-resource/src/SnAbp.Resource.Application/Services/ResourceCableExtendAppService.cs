using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices.Equipment;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using SnAbp.Resource.Enums;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Authorization;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceCableExtendAppService : ResourceAppService, IResourceCableExtendAppService
    {
        private readonly IRepository<Equipment, Guid> equipmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<CableExtend, Guid> _cableExtendRespository;

        public ResourceCableExtendAppService(
            IRepository<Equipment, Guid> equipmentRep,
            IRepository<CableExtend, Guid> cableExtendRespository,
            IGuidGenerator guidGenerator
            )
        {
            equipmentRepository = equipmentRep;
            _guidGenerator = guidGenerator;
            _cableExtendRespository = cableExtendRespository;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CableExtendDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var cableExtend = _cableExtendRespository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (cableExtend == null)
            {
                throw new UserFriendlyException("该电缆特性不存在");
            }

            return Task.FromResult(ObjectMapper.Map<CableExtend, CableExtendDto>(cableExtend));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.CableExtend.Update)]
        public async Task<bool> Update(CableExtendDto input)
        {
            var cableExtend = _cableExtendRespository.FirstOrDefault(x => x.Id == input.Id);

            if (cableExtend == null) throw new UserFriendlyException("电缆特性不存在");

            cableExtend.Number = input.Number;
            cableExtend.RailwayNumber = input.RailwayNumber;
            cableExtend.SpareNumber = input.SpareNumber;
            cableExtend.Length = input.Length;
            cableExtend.LayType = input.LayType;
            await _cableExtendRespository.UpdateAsync(cableExtend);
            return true;
        }



    }
}