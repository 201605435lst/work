using SnAbp.StdBasic.Dtos.Model;
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
    public class StdBasicModelFileAppService : StdBasicAppService, IStdBasicModelFileAppService
    {
        private readonly IRepository<ModelFile, Guid> _repository;
        private readonly IGuidGenerator _guidGenerator;

        public StdBasicModelFileAppService(IRepository<ModelFile, Guid> repository, IGuidGenerator guidGenerator)
        {
            _repository = repository;
            _guidGenerator = guidGenerator;
        }
        public async Task<ModelFileDto> Create(ModelFileCreateDto input)
        {
            var modelFile = _repository.WithDetails().Where(x => x.ModelId == input.ModelId).Where(x => x.DetailLevel == input.DetailLevel).FirstOrDefault();
            if (modelFile!=null) throw new UserFriendlyException("该模型精细等级已经存在!!!");
            ModelFile entity = new ModelFile(_guidGenerator.Create())
            {
                ModelId = input.ModelId,
                FamilyFileId = input.FamilyFileId,
                ThumbId = input.ThumbId,
                DetailLevel = input.DetailLevel
            };
            await _repository.InsertAsync(entity);
            return ObjectMapper.Map<ModelFile, ModelFileDto>(entity);
        }

        public async Task<ModelFileDto> Update(ModelFileDto input)
        {
            var entity = await _repository.GetAsync(input.Id);
            if (entity == null) throw new UserFriendlyException("当前信息交换模板分类不存在");
            entity.FamilyFileId = input.FamilyFileId;
            entity.ThumbId = input.ThumbId;
            entity.DetailLevel = input.DetailLevel;
            await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<ModelFile, ModelFileDto>(entity);
        }

        public Task<PagedResultDto<ModelFileDto>> GetList(Guid ModelId)
        {
            var entityList = _repository.WithDetails().Where(x => x.ModelId == ModelId);
            var res = new PagedResultDto<ModelFileDto>()
            {
                TotalCount = entityList.Count(),
                Items = ObjectMapper.Map<List<ModelFile>, List<ModelFileDto>>(entityList.ToList()),
            };
            return Task.FromResult(res);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("该模型文件不存在");
            if (_repository.Any(x => x.Id == id))
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            else
                return false;
        }
    }
}
