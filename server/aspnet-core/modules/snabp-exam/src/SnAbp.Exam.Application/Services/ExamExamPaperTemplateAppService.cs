using Microsoft.AspNetCore.Authorization;
using SnAbp.Exam.Authorization;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace SnAbp.Exam.Services
{
    [Authorize]
    public class ExamExamPaperTemplateAppService : ExamAppService, IExamExamPaperTemplateAppService
    {
        private readonly IRepository<ExamPaperTemplate, Guid> _examExamPaperTemplateRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ExamExamPaperTemplateAppService(
        IRepository<ExamPaperTemplate, Guid> examExamPaperTemplateRepository,
            IGuidGenerator guidGenerator)
        {
            _examExamPaperTemplateRepository = examExamPaperTemplateRepository;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 获取试卷模板信息
        /// </summary>
        public async Task<ExamPaperTemplateDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var examPaperTemplate = _examExamPaperTemplateRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (examPaperTemplate == null) throw new UserFriendlyException("此试卷模板不存在");
            return ObjectMapper.Map<ExamPaperTemplate, ExamPaperTemplateDto>(examPaperTemplate);
        }

        /// <summary>
        /// 查询试卷模板信息
        /// </summary>
        public async Task<PagedResultDto<ExamPaperTemplateDto>> GetList(ExamPaperTemplateSearchDto input)
        {
            PagedResultDto<ExamPaperTemplateDto> result = new PagedResultDto<ExamPaperTemplateDto>();
            await Task.Run(() =>
            {
                var examPaperTemplate = _examExamPaperTemplateRepository.WithDetails()
                    .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name))
                    .WhereIf(input.CategoryId != null && input.CategoryId != Guid.Empty, x => x.CategoryId == input.CategoryId);
                result.TotalCount = examPaperTemplate.Count();
                var res = ObjectMapper.Map<List<ExamPaperTemplate>, List<ExamPaperTemplateDto>>(examPaperTemplate.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                result.Items = res;
            });
            return result;
        }

        /// <summary>
        /// 创建试卷模板
        /// </summary>
        [Authorize(ExamPermissions.PaperTemplate.Create)]
        public async Task<ExamPaperTemplateDto> Create(ExamPaperTemplateCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("名称不能为空");
            if (input.CategoryId == null || input.CategoryId == Guid.Empty) throw new UserFriendlyException("试卷分类不能为空");

            var examPaperTemplate = new ExamPaperTemplate(_guidGenerator.Create());
            examPaperTemplate.Name = input.Name;
            examPaperTemplate.Remark = input.Remark;
            examPaperTemplate.CategoryId = input.CategoryId;

            examPaperTemplate.ExamPaperTemplateConfigs = new List<ExamPaperTemplateConfig>();

            foreach (var examPaperTemplateConfig in input.ExamPaperTemplateConfigs)
            {
                examPaperTemplate.ExamPaperTemplateConfigs.Add(new ExamPaperTemplateConfig(_guidGenerator.Create())
                {
                    Count = examPaperTemplateConfig.Count,
                    Order = examPaperTemplateConfig.Order,
                    DifficultyDegree = examPaperTemplateConfig.DifficultyDegree,
                    Type = examPaperTemplateConfig.Type,
                    Score = examPaperTemplateConfig.Score,
                    ExamPaperTemplateId = examPaperTemplate.Id
                });
            }
            await _examExamPaperTemplateRepository.InsertAsync(examPaperTemplate);


            return ObjectMapper.Map<ExamPaperTemplate, ExamPaperTemplateDto>(examPaperTemplate);
        }

        /// <summary>
        /// 修改试卷模板
        /// </summary>
        [Authorize(ExamPermissions.PaperTemplate.Update)]
        public async Task<ExamPaperTemplateDto> Update(ExamPaperTemplateUpdateDto input)
        {
            if (string.IsNullOrEmpty(input.Name.Trim())) throw new UserFriendlyException("请输入正确的名称");
            if (input.CategoryId == null || input.CategoryId == Guid.Empty) throw new UserFriendlyException("试卷分类不能为空");
            var examPaperTemplate = await _examExamPaperTemplateRepository.GetAsync(input.Id);
            if (examPaperTemplate == null)
            {
                throw new UserFriendlyException("该试卷模板不存在");
            }
            // 清除之前保存的关联表信息
            await _examExamPaperTemplateRepository.DeleteAsync(x => x.CategoryId == examPaperTemplate.Id);

            // 重新保存关联表信息
            examPaperTemplate.Name = input.Name;
            examPaperTemplate.Remark = input.Remark;
            examPaperTemplate.CategoryId = input.CategoryId;

            examPaperTemplate.ExamPaperTemplateConfigs = new List<ExamPaperTemplateConfig>();
                foreach (var examPaperTemplateConfig in input.ExamPaperTemplateConfigs)
            {
                    examPaperTemplate.ExamPaperTemplateConfigs.Add(new ExamPaperTemplateConfig(_guidGenerator.Create())
                    {
                        Count = examPaperTemplateConfig.Count,
                        Order = examPaperTemplateConfig.Order,
                        DifficultyDegree = examPaperTemplateConfig.DifficultyDegree,
                        Type = examPaperTemplateConfig.Type,
                        Score = examPaperTemplateConfig.Score,
                        ExamPaperTemplateId = examPaperTemplate.Id
                    });
            }
            await _examExamPaperTemplateRepository.UpdateAsync(examPaperTemplate);
            return ObjectMapper.Map<ExamPaperTemplate, ExamPaperTemplateDto>(examPaperTemplate);
        }

        /// <summary>
        /// 删除试卷模板
        /// </summary>
        [Authorize(ExamPermissions.PaperTemplate.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            //if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            //var ExamPaperTemplate = await _examExamPaperTemplateRepository.GetAsync(id);
            //if (ExamPaperTemplate == null) throw new UserFriendlyException("此试卷模板不存在");
            //await _examExamPaperTemplateRepository.DeleteAsync(ExamPaperTemplate);
            //return true;
            await _examExamPaperTemplateRepository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }

    }
}
