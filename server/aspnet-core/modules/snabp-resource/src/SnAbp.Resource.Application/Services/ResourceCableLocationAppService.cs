using Microsoft.AspNetCore.Authorization;
using SnAbp.Resource.Authorization;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.IServices.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Resource.Services
{
    [Authorize]
    public class ResourceCableLocationAppService : ResourceAppService, IResourceCableLocationAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<CableLocation, Guid> _cableLocationRespository;

        public ResourceCableLocationAppService(
            IRepository<CableLocation, Guid> cableLocationRespository,
            IGuidGenerator guidGenerator
            )
        {
            _cableLocationRespository = cableLocationRespository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CableLocationDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null)
            {
                throw new UserFriendlyException("Id不能为空");
            }

            var cableLocation = _cableLocationRespository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (cableLocation == null)
            {
                throw new UserFriendlyException("该电缆埋深信息实体不存在");
            }

            return Task.FromResult(ObjectMapper.Map<CableLocation, CableLocationDto>(cableLocation));
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        public Task<List<CableLocationDto>> GetList(Guid cableId)
        {
            var cableLocations = _cableLocationRespository.Where(x => x.CableId == cableId).ToList();

            return Task.FromResult(ObjectMapper.Map<List<CableLocation>, List<CableLocationDto>>(cableLocations));
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.CableLocation.Create)]
        public async Task<bool> Create(CableLocationCreateDto input)
        {
            CheckSameTitle(input.Name, null);
            var cableLocation = new CableLocation(_guidGenerator.Create())
            {
                CableId = input.CableId,
                Name = input.Name,
                Order = input.Order,
                Value = input.Value,
                Positions = input.Positions,
                Direction = input.Direction,
            };
            await _cableLocationRespository.InsertAsync(cableLocation);
            return true;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.CableLocation.Update)]
        public async Task<bool> Update(CableLocationCreateDto input)
        {
            CheckSameTitle(input.Name, input.Id);

            var cableLocation = _cableLocationRespository.FirstOrDefault(x => x.Id == input.Id);

            if (cableLocation == null) throw new UserFriendlyException("该电缆埋深信息实体不存在");

            cableLocation.CableId = input.CableId;
            cableLocation.Name = input.Name;
            cableLocation.Value = input.Value;
            cableLocation.Order = input.Order;
            cableLocation.Positions = input.Positions;
            cableLocation.Direction = input.Direction;
            await _cableLocationRespository.UpdateAsync(cableLocation);
            return true;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        //[Authorize(ResourcePermissions.CableLocation.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                if (id == null || Guid.Empty == id) throw new Exception("id不正确");
                var ent = _cableLocationRespository.FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new Exception("该电缆埋深信息不存在");
                await _cableLocationRespository.DeleteAsync(id);
                return true;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }

        /// <summary>
        /// 私有方法
        /// </summary>
        //验证文章标题唯一性
        private bool CheckSameTitle(string name, Guid? id)
        {
            var sameNames = _cableLocationRespository.Where(o => o.Name.ToUpper() == name.ToUpper());

            if (id.HasValue)
            {
                sameNames = sameNames.Where(o => o.Id != id.Value);
            }

            if (sameNames.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("已存在相同名称的埋深信息！！！");
            }

            return true;
        }

    }
}