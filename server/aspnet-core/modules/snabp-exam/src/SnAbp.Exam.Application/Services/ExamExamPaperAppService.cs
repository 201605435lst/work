using Microsoft.AspNetCore.Authorization;
using SnAbp.Exam.Authorization;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.Enums;
using SnAbp.Exam.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Exam.Services
{
    [Authorize]
    public class ExamExamPaperAppService : ExamAppService, IExamExamPaperAppService
    {
        private readonly IRepository<ExamPaperRltQuestion, Guid> _examPaperRltQuestions;
        private readonly IRepository<ExamPaper, Guid> _repository;
        private readonly IRepository<Question, Guid> _repositoryQuestion;
        private readonly IGuidGenerator _guidGenerator;

        public ExamExamPaperAppService(IRepository<ExamPaper, Guid> repository, IRepository<ExamPaperRltQuestion,
            Guid> examPaperRltQuestion, IGuidGenerator guidGenerator,
            IRepository<Question, Guid> repositoryQuestion)
        {
            _examPaperRltQuestions= examPaperRltQuestion;
            _repository = repository;
            _repositoryQuestion = repositoryQuestion;
            _guidGenerator = guidGenerator;
        }

        /// <summary>
        /// 创建试卷
        /// </summary>
        [Authorize(ExamPermissions.Paper.Create)]
        public async Task<ExamPaperDto> Create(ExamPaperCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("试卷名称不能为空");
            var examPaper = new ExamPaper(_guidGenerator.Create());
            CheckSameName(input.Name, input.Id);
            
            examPaper.CategoryId = input.CategoryId;
            examPaper.Name = input.Name;
            examPaper.ExamPaperTemplateId = input.ExamPaperTemplateId;
            examPaper.GroupQuestionType = input.GroupQuestionType;
            examPaper.QuestionTotalNumber = input.QuestionTotalNumber;
            examPaper.TotalScore = input.TotalScore;
            examPaper.ExaminationDuration = input.ExaminationDuration;
            examPaper.ExamPaperRltQuestions = new List<ExamPaperRltQuestion>();

            foreach (var question in input.ExamPaperRltQuestions)
            {
              
                examPaper.ExamPaperRltQuestions.Add(new ExamPaperRltQuestion(_guidGenerator.Create())
                {
                    QuestionId = question.QuestionId,
                    Order = question.Order,
                    Score = question.Score,
                });
            }
            await _repository.InsertAsync(examPaper);
            return ObjectMapper.Map<ExamPaper, ExamPaperDto>(examPaper);
        }

        /// <summary>
        /// 删除试卷
        /// </summary>
        [Authorize(ExamPermissions.Paper.Delete)]
        public async Task<bool> Delete(List<Guid> id)
        {
            await _repository.DeleteAsync(x => x.Id != null && id.Contains((Guid)x.Id));
            //await _repository.DeleteAsync(x => id.Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 修改试卷
        /// </summary>
        [Authorize(ExamPermissions.Paper.Update)]
        public async Task<ExamPaperDto> Update(ExamPaperUpdateDto input)
        {
            //throw new NotImplementedException();
            var examPaper = await _repository.GetAsync(input.Id);  
            if (examPaper == null) throw new UserFriendlyException("该试卷不存在，无法修改");
            CheckSameName(input.Name, input.Id);

            examPaper.Name = input.Name;
            examPaper.ExamPaperTemplateId = input.ExamPaperTemplateId;
            examPaper.GroupQuestionType = input.GroupQuestionType;
            examPaper.QuestionTotalNumber = input.QuestionTotalNumber;
            examPaper.TotalScore = input.TotalScore;
            examPaper.ExaminationDuration = input.ExaminationDuration;
            //examPaper.CreateTime = input.CreateTime;
            examPaper.CategoryId = input.CategoryId;


            await _repository.UpdateAsync(examPaper);

            return ObjectMapper.Map<ExamPaper, ExamPaperDto>(examPaper);
        }

        /// <summary>
        /// 查找试卷
        /// </summary>
        public Task<ExamPaperDto> Get(Guid id)
        {
            // throw new NotImplementedException();
            //var examPaper =  await _repository.GetAsync(id);
            var examPaper = _repository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            return Task.FromResult(ObjectMapper.Map<ExamPaper, ExamPaperDto>(examPaper));
        }

        /// <summary>
        /// 查找试卷
        /// </summary>
         public async Task<PagedResultDto<ExamPaperDto>> GetList(ExamPaperSearchDto input)
         {
              var result = new PagedResultDto<ExamPaperDto>();
              await Task.Run(()=>
              {
                  
                  var examPaper = _repository.WithDetails()
                  .WhereIf(input.CategoryId!=null&&input.CategoryId != Guid.Empty,x=>x.CategoryId == input.CategoryId)
                  .WhereIf(!string.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name));
                  result.TotalCount = examPaper.Count();
                  var res = ObjectMapper.Map<List<ExamPaper>, List<ExamPaperDto>>(examPaper.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
                  result.Items = res;
              });
              return result;
          }


        /// <summary>
        /// 查找题库中的题目
        /// </summary>
        public Task<PagedResultDto<ExamPaperRltQuestionConfigDto>> GetQuestionList(ExamPaperRltQuestionSearchDto input)
        {
            var examPaper = _repositoryQuestion.WithDetails()
            .WhereIf(input.Type.IsIn(QuestionType.GapFilling, QuestionType.MultipleChoice, QuestionType.ShortAnswerQuestion, QuestionType.TrueOrFalseQuestions, QuestionType.SingleChoice), x => x.QuestionType == input.Type)
            .WhereIf(input.StartDifficultyCoefficient >= 0 && input.EndDifficultyCoefficient >= 0, x => x.DifficultyCoefficient >= input.StartDifficultyCoefficient && x.DifficultyCoefficient <= input.EndDifficultyCoefficient)
            .WhereIf(input.KnowledgePointId != null && input.KnowledgePointId != Guid.Empty, x => x.QuestionRltKnowledgePoints.Any(y => input.KnowledgePointId == y.KnowledgePointId))
            .ToList();
            var result = new PagedResultDto<ExamPaperRltQuestionConfigDto>();
            result.TotalCount = examPaper.Count();
            result.Items = ObjectMapper.Map<List<Question>, List<ExamPaperRltQuestionConfigDto>>(examPaper.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return Task.FromResult(result);
        }

        private bool CheckSameName(string name, Guid? id)
        {
            var samePapers = _repository.Where(o => o.Name.ToUpper() == name.ToUpper());
            if (id.HasValue)
            {
                samePapers = samePapers.Where(o => o.Id != id.Value);
            }
            if (samePapers.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("当前试卷中存在相同名称的试卷");
            }
            return true;
        }
    }
}
