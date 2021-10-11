using Microsoft.AspNetCore.Authorization;
using SnAbp.Exam.Authorization;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.Enums;
using SnAbp.Exam.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Exam.Services
{
    [Authorize]
    public class ExamQuestionAppService : ExamAppService, IExamQuestionAppService
    {
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<QuestionRltKnowledgePoint, Guid> _questionRltKnowledgePointRepository;
        private readonly IRepository<QuestionRltCategory, Guid> _questionRltCategoryRepository;
        private readonly IRepository<AnswerOption, Guid> _answerOptionRepository;

        public ExamQuestionAppService(
            IRepository<Question, Guid> questionRepository,
            IGuidGenerator guidGenerator,
            IRepository<QuestionRltKnowledgePoint, Guid> questionRltKnowledgePointRepository,
            IRepository<QuestionRltCategory, Guid> questionRltCategoryRepository,
            IRepository<AnswerOption, Guid> answerOptionRepository)
        {
            _questionRepository = questionRepository;
            _guidGenerator = guidGenerator;
            _questionRltKnowledgePointRepository = questionRltKnowledgePointRepository;
            _questionRltCategoryRepository = questionRltCategoryRepository;
            _answerOptionRepository = answerOptionRepository;
        }

        [Authorize(ExamPermissions.Question.Create)]
        public async Task<QuestionDto> Create(QuestionCreateDto input)
        {
            if (input.QuestionRltCategories == null && input.QuestionRltCategories.Count > 0) throw new UserFriendlyException("请选择分类");
            if (input.QuestionRltKnowledgePoints == null && input.QuestionRltKnowledgePoints.Count > 0) throw new UserFriendlyException("请选择知识点");
            if (input.QuestionType.Equals("")) throw new UserFriendlyException("请输入类型");
            if (input.DifficultyCoefficient == 0) throw new UserFriendlyException("请调整难度系数");
            CheckSameTitle(input.Title, null);//题目验证

            var QuestionId = _guidGenerator.Create();
            var question = new Question(QuestionId);

            question.Analysis = input.Analysis;
            question.DifficultyCoefficient = input.DifficultyCoefficient;
            question.Title = input.Title;
            question.QuestionType = input.QuestionType;
            question.Answer = input.Answer;

            //重新保存答案信息
            question.AnswerOptions = new List<AnswerOption>();
            if (input.AnswerOptions.Count > 0)
                foreach (var answerOptions in input.AnswerOptions)
                {
                    question.AnswerOptions.Add(new AnswerOption(answerOptions.Id)
                    {
                        Content = answerOptions.Content,
                        Order = 0,
                    });
            }

            //重新保存关联分类信息
            question.QuestionRltCategories = new List<QuestionRltCategory>();
            foreach(var questionRltCategory in input.QuestionRltCategories)
            {
                question.QuestionRltCategories.Add(new QuestionRltCategory(_guidGenerator.Create()) { 
                    CategoryId = questionRltCategory.Id
                });
            }

            //重新保存关联知识点信息
            question.QuestionRltKnowledgePoints = new List<QuestionRltKnowledgePoint>();
            foreach (var questionRltKnowledgePoint in input.QuestionRltKnowledgePoints)
            {
                question.QuestionRltKnowledgePoints.Add(new QuestionRltKnowledgePoint(_guidGenerator.Create())
                {
                    KnowledgePointId = questionRltKnowledgePoint.Id
                });
            }
            await _questionRepository.InsertAsync(question);
            return ObjectMapper.Map<Question, QuestionDto>(question);
        }

        public Task<QuestionDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("Id不能为空");

            var question = _questionRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (question == null) throw new UserFriendlyException("该题目不存在");

            return Task.FromResult(ObjectMapper.Map<Question, QuestionDto>(question));
        }

        public Task<PagedResultDto<QuestionSimpleDto>> GetList(QuestionSearchDto input)
        {

            var questions = _questionRepository.WithDetails()
            .WhereIf(input.QuestionType.IsIn(QuestionType.GapFilling, QuestionType.MultipleChoice, QuestionType.ShortAnswerQuestion, QuestionType.TrueOrFalseQuestions, QuestionType.SingleChoice), x => x.QuestionType == input.QuestionType)
            .WhereIf(!string.IsNullOrEmpty(input.Title), x => x.Title.Contains(input.Title))
            .WhereIf(input.StartDifficultyCoefficient >= 0 && input.EndDifficultyCoefficient >= 0, x => x.DifficultyCoefficient >= input.StartDifficultyCoefficient && x.DifficultyCoefficient <= input.EndDifficultyCoefficient)
            .WhereIf(input.CategoryIds != null && input.CategoryIds.Count > 0, x => x.QuestionRltCategories.Any(y => input.CategoryIds.Contains(y.CategoryId)));
            var result = new PagedResultDto<QuestionSimpleDto>();
            result.TotalCount = questions.Count();
            result.Items = ObjectMapper.Map<List<Question>, List<QuestionSimpleDto>>(questions.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

            return Task.FromResult(result);
        }

        [Authorize(ExamPermissions.Question.Update)]
        public async Task<QuestionDto> Update(QuestionUpdateDto input)
        {
            if (input.QuestionRltCategories == null && input.QuestionRltCategories.Count > 0) throw new UserFriendlyException("请选择分类");
            if (input.QuestionRltKnowledgePoints == null) throw new UserFriendlyException("请选择知识点");
            if (input.QuestionType.Equals("")) throw new UserFriendlyException("请输入类型");
            if (input.DifficultyCoefficient == 0) throw new UserFriendlyException("请调整难度系数");

            CheckSameTitle(input.Title, input.Id);  //标题验证

            var question = await _questionRepository.GetAsync(input.Id);
            if (question == null) throw new UserFriendlyException("该题目不存在");

            var questionRltKnowledgePoints = _questionRltKnowledgePointRepository.Where(x => x.QuestionId == input.Id);

            //清除之前关联知识点信息
            await _questionRltKnowledgePointRepository.DeleteAsync(x => x.QuestionId == input.Id);
            //重新保存
            question.QuestionRltKnowledgePoints = new List<QuestionRltKnowledgePoint>();
            foreach (var questionRltKnowledgePoint in input.QuestionRltKnowledgePoints)
            {
                question.QuestionRltKnowledgePoints.Add(new QuestionRltKnowledgePoint(_guidGenerator.Create())
                {
                    KnowledgePointId = questionRltKnowledgePoint.Id
                });
            }

            //清除之前关联分类信息
            await _questionRltCategoryRepository.DeleteAsync(x => x.QuestionId == input.Id);
            //重新保存
            question.QuestionRltCategories = new List<QuestionRltCategory>();
            foreach (var questionRltCategory in input.QuestionRltCategories)
            {
                question.QuestionRltCategories.Add(new QuestionRltCategory(_guidGenerator.Create())
                {
                    CategoryId = questionRltCategory.Id
                });
            }

            //清除之前答案信息
            question.Answer = input.Answer;
            await _answerOptionRepository.DeleteAsync(x => x.QuestionId == input.Id);
            //重新保存答案信息
            question.AnswerOptions = new List<AnswerOption>();
            if (input.AnswerOptions.Count > 0)
                foreach (var answerOptions in input.AnswerOptions)
                {
                    question.AnswerOptions.Add(new AnswerOption(answerOptions.Id)
                    {
                        Content = answerOptions.Content,
                        Order = 0,
                    });
                }


            question.Analysis = input.Analysis;

            question.DifficultyCoefficient = input.DifficultyCoefficient;
            question.Title = input.Title;
            question.QuestionType = input.QuestionType;

            await _questionRepository.UpdateAsync(question);

            return ObjectMapper.Map<Question, QuestionDto>(question);

            throw new NotImplementedException();
        }

        [Authorize(ExamPermissions.Question.Delete)]
        public async Task<bool> delete(List<Guid> ids)
        {

            await _answerOptionRepository.DeleteAsync(x => x.Id != null && ids.Contains((Guid)x.QuestionId));
            await _questionRepository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
        
        private bool CheckSameTitle(string title, Guid? id)
        {
            var sameArticles = _questionRepository.Where(o => o.Title.ToUpper() == title.ToUpper());

            if (id.HasValue)
            {
                sameArticles = sameArticles.Where(o => o.Id != id.Value);
            }

            if (sameArticles.Count() > 0)
            {
                throw new Volo.Abp.UserFriendlyException("已存在相同标题的文章！！！");
            }

            return true;
        }
    }
}
