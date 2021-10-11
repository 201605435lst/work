using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicTerminalAppService : StdBasicAppService, IStdBasicStandardEquipmentTerminalAppService
    {
        private readonly IRepository<ModelTerminal, Guid> _modelTerminalRepository;
        private readonly IRepository<Model, Guid> _modelRepository;
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;
        public StdBasicTerminalAppService(
            IRepository<ModelTerminal, Guid> modelTerminalRepository,
            IGuidGenerator guidGenerator,
            IRepository<Model, Guid> modelRepository,
            IRepository<ProductCategory, Guid> productCategoryRepository)
        {
            _modelTerminalRepository = modelTerminalRepository;
            _modelRepository = modelRepository;
            _productCategoryRepository = productCategoryRepository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardEquipmentTerminalDto> Get(Guid id)
        {
            StandardEquipmentTerminalDto ModelTerminal = null;

            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");

            await Task.Run(() =>
            {
                var entity = _modelTerminalRepository.WithDetails().FirstOrDefault(s => s.Id == id);
                if (entity == null) throw new UserFriendlyException("此端子类型不存在");
                ModelTerminal = ObjectMapper.Map<ModelTerminal, StandardEquipmentTerminalDto>(entity);
            });
            return ModelTerminal;
        }

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        public async Task<PagedResultDto<StandardEquipmentTerminalDto>> GetList(StandardEquipmentTerminalSearchInputDto input)
        {
            if (input.ModelId == Guid.Empty && !input.IsAll)
            {
                throw new UserFriendlyException("标准设备主键不能为空");
            }

            PagedResultDto<StandardEquipmentTerminalDto> result = new PagedResultDto<StandardEquipmentTerminalDto>();
            await Task.Run(() =>
            {
                var ent = _modelTerminalRepository.WithDetails()
                          .WhereIf(!string.IsNullOrEmpty(input.Name), s => s.Name.Contains(input.Name))
                          .WhereIf(input.ModelId != null && input.ModelId != Guid.Empty, x => x.ModelId == input.ModelId);
                result.TotalCount = ent.Count();
                var resEnt = ent.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                result.Items = ObjectMapper.Map<List<ModelTerminal>, List<StandardEquipmentTerminalDto>>(resEnt);
                result.Items = input.IsAll
                ? ObjectMapper.Map<List<ModelTerminal>, List<StandardEquipmentTerminalDto>>(resEnt)
                : ObjectMapper.Map<List<ModelTerminal>, List<StandardEquipmentTerminalDto>>(resEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            });
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.Terminal.Create)]
        public async Task<StandardEquipmentTerminalDto> Create(StandardEquipmentTerminalCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("端子名称不能为空");
            if (input.ModelId == null || input.ModelId == Guid.Empty) throw new UserFriendlyException("关联设备Id不能为空");
            if (input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) throw new UserFriendlyException("关联端子分类Id不能为空");

            if (_modelTerminalRepository.Any(z => z.Name == input.Name && z.ModelId == input.ModelId)) throw new UserFriendlyException("端子名称已存在");

            if (!_modelRepository.Any(z => z.Id == input.ModelId)) throw new Exception("关联设备不存在");

            if (!_productCategoryRepository.Any(z => z.Id == input.ProductCategoryId)) throw new Exception("关联端子类型不存在");
            var modelTerminal = new ModelTerminal(_guidGenerator.Create())
            {
                Name = input.Name,
                ModelId = input.ModelId,
                ProductCategoryId = input.ProductCategoryId,
                Remark = input.Remark,
            };

            await _modelTerminalRepository.InsertAsync(modelTerminal);
            return ObjectMapper.Map<ModelTerminal, StandardEquipmentTerminalDto>(modelTerminal);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.Terminal.Update)]
        public async Task<StandardEquipmentTerminalDto> Update(StandardEquipmentTerminalUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("端子名称不能为空");

            if (input.ModelId == null || input.ModelId == Guid.Empty) throw new UserFriendlyException("关联设备Id不能为空");

            if (input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) throw new UserFriendlyException("关联端子分类Id不能为空");

            if (_modelTerminalRepository.Any(z => z.Name == input.Name && z.ModelId == input.ModelId && z.Id != input.Id)) throw new UserFriendlyException("端子名称已存在");

            if (!_modelRepository.Any(z => z.Id == input.ModelId)) throw new Exception("关联设备不存在");

            if (!_productCategoryRepository.Any(z => z.Id == input.ProductCategoryId)) throw new Exception("关联端子类型不存在");

            var modelTerminal = await _modelTerminalRepository.GetAsync(input.Id);
            if (modelTerminal == null) throw new UserFriendlyException("更新对象不存在");

            modelTerminal.Name = input.Name;
            modelTerminal.ModelId = input.ModelId;
            modelTerminal.ProductCategoryId = input.ProductCategoryId;
            modelTerminal.Remark = input.Remark;

            await _modelTerminalRepository.UpdateAsync(modelTerminal);
            return ObjectMapper.Map<ModelTerminal, StandardEquipmentTerminalDto>(modelTerminal);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        [Authorize(StdBasicPermissions.Terminal.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var modelTerminal = _modelTerminalRepository.FirstOrDefault(s => s.Id == id);
            if (modelTerminal == null) throw new UserFriendlyException("此设备端子不存在");
            await _modelTerminalRepository.DeleteAsync(id);
            return true;
        }
    }
}
