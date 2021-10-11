using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using SnAbp.Project.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Project.Service
{
    public class ProjectFileCategoryAppService : ProjectAppService, IProjectFileCategoryAppService
    {
        private readonly IRepository<FileCategory, Guid> _FileCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ProjectFileCategoryAppService(
            IGuidGenerator guidGenerator,
            IRepository<FileCategory, Guid> FileCategoryRepository
            )
        {
            _FileCategoryRepository = FileCategoryRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<FileCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的文件分类");
            var FileCategory = _FileCategoryRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (FileCategory == null) throw new UserFriendlyException("当前文件分类不存在");
            var result = ObjectMapper.Map<FileCategory, FileCategoryDto>(FileCategory);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<FileCategoryDto>> GetList()
        {
            var result = new PagedResultDto<FileCategoryDto>();
            var FileCategory = _FileCategoryRepository.WithDetails();

            var res = ObjectMapper.Map<List<FileCategory>, List<FileCategoryDto>>(FileCategory.OrderBy(x => x.Name).ToList());

            result.Items = res;
            result.TotalCount = FileCategory.Count();
            return Task.FromResult(result);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(FileCategoryDto input)
        {
            FileCategoryDto FileCategoryDto = new FileCategoryDto();
            var FileCategory = new FileCategory();
            CheckSameName(input.Name,null);

            ObjectMapper.Map(input, FileCategory);
            FileCategory.SetId(_guidGenerator.Create());
            await _FileCategoryRepository.InsertAsync(FileCategory);
           
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(FileCategoryDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的文件分类");
            var FileCategory = await _FileCategoryRepository.GetAsync(input.Id);
            CheckSameName(input.Name,input.Id);

            if (FileCategory == null) throw new UserFriendlyException("当前文件分类不存在");
            ObjectMapper.Map(input, FileCategory);
            await _FileCategoryRepository.UpdateAsync(FileCategory);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _FileCategoryRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此文件分类不存在");
            await _FileCategoryRepository.DeleteAsync(id);
            return true;
        }

        #region
        bool CheckSameName(string Name,Guid?Id)
        {
            var fileCategory = _FileCategoryRepository.WithDetails()
                .Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (Id.HasValue)
            {
                fileCategory = fileCategory.Where(x => x.Id != Id.Value);
            }
            if (fileCategory.Count() > 0)
            {
                throw new UserFriendlyException("已存在相同名字的类型名!!!");
            }

            return true;
        }
        #endregion
    }
}