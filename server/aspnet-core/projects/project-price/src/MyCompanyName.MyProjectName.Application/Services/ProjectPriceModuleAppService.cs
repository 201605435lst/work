using MyCompanyName.MyProjectName.Dtos;
using MyCompanyName.MyProjectName.Entities;
using MyCompanyName.MyProjectName.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace MyCompanyName.MyProjectName.Services
{
    public class ProjectPriceModuleAppService : MyProjectNameAppService, IProjectPriceModuleAppService
    {
        private IGuidGenerator _guidGenerator;
        private IRepository<Module, Guid> _moduleRepository;
        private IRepository<ProjectRltModule, Guid> _projectRltModuleRepository;

        public ProjectPriceModuleAppService(
            IGuidGenerator guidGenerator,
            IRepository<Module, Guid> moduleRepository,
            IRepository<ProjectRltModule, Guid> projectRltModuleRepository
            )
        {
            _guidGenerator = guidGenerator;
            _moduleRepository = moduleRepository;
            _projectRltModuleRepository = projectRltModuleRepository;
        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ModuleDto>> GetList(ModuleGetListInput input)
        {
            var list = _moduleRepository
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords))
                .Where(x => x.IsPublic)
                .OrderBy(x => x.Order)
                .ToList()
                .Where(x => x.ParentId == null)
                .ToList();

            return ObjectMapper.Map<List<Module>, List<ModuleDto>>(list);
        }


        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleDto> Get(Guid id)
        {
            var entity = _moduleRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            return ObjectMapper.Map<Module, ModuleDto>(entity);
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(ModuleCreateDto input)
        {
            var entity = new Module(_guidGenerator.Create())
            {
                ParentId = input.ParentId,
                Name = input.Name,
                Content = input.Content,
                Remark = input.Remark,
                Order = input.Order,
                WorkDays = input.WorkDays,
                Progress = input.Progress,
                IsPublic = true,
            };

            await _moduleRepository.InsertAsync(entity);
            return true;
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(ModuleUpdateDto input)
        {
            var entity = await _moduleRepository.GetAsync(input.Id);
            entity.ParentId = input.ParentId;
            entity.Name = input.Name;
            entity.Name = input.Name;
            entity.Content = input.Content;
            entity.Remark = input.Remark;
            entity.Order = input.Order;
            entity.WorkDays = input.WorkDays;
            entity.Progress = input.Progress;

            await _moduleRepository.UpdateAsync(entity);
            return true;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _moduleRepository.DeleteAsync(id);
            return true;
        }
    }
}
