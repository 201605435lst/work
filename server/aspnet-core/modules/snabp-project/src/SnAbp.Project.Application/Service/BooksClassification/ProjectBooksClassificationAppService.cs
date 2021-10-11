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
    public class ProjectBooksClassificationAppService : ProjectAppService, IProjectBooksClassificationAppService
    {
        private readonly IRepository<BooksClassification, Guid> _booksClassificationRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ProjectBooksClassificationAppService(
            IGuidGenerator guidGenerator,
            IRepository<BooksClassification, Guid> booksClassificationRepository
            )
        {
            _booksClassificationRepository = booksClassificationRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<BooksClassificationDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的分类");
            var BooksClassification = _booksClassificationRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (BooksClassification == null) throw new UserFriendlyException("当前文件分类不存在");
            var result = ObjectMapper.Map<BooksClassification, BooksClassificationDto>(BooksClassification);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResultDto<BooksClassificationDto>> GetList()
        {
            var result = new PagedResultDto<BooksClassificationDto>();
            var BooksClassification = _booksClassificationRepository.WithDetails();

            var res = ObjectMapper.Map<List<BooksClassification>, List<BooksClassificationDto>>(BooksClassification.OrderBy(x => x.Name).ToList());

            result.Items = res;
            result.TotalCount = BooksClassification.Count();
            return Task.FromResult(result);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(BooksClassificationSimpleDto input)
        {
            BooksClassificationDto BooksClassificationDto = new BooksClassificationDto();
            var BooksClassification = new BooksClassification();
            CheckSameName(input.Name,null);

            ObjectMapper.Map(input, BooksClassification);
            BooksClassification.SetId(_guidGenerator.Create());
            await _booksClassificationRepository.InsertAsync(BooksClassification);

            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(BooksClassificationDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的分类");
            var BooksClassification = await _booksClassificationRepository.GetAsync(input.Id);
            CheckSameName(input.Name,input.Id);

            if (BooksClassification == null) throw new UserFriendlyException("当前分类不存在");
            ObjectMapper.Map(input, BooksClassification);
            await _booksClassificationRepository.UpdateAsync(BooksClassification);
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
            var ent = _booksClassificationRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此分类不存在");
            await _booksClassificationRepository.DeleteAsync(id);
            return true;
        }

        #region
        bool CheckSameName(string Name,Guid ?Id)
        {
            var BooksClassification = _booksClassificationRepository.WithDetails()
                .Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (Id.HasValue)
            {
                BooksClassification = BooksClassification.Where(x => x.Id != Id.Value);
            }
            if (BooksClassification.Count() > 0)
            {
                throw new UserFriendlyException("已存在相同名字的类型名!!!");
            }

            return true;
        }
        #endregion
    }
}
