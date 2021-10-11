using AutoMapper;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Dtos;

using SnAbp.Exam.Dtos;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using Volo.Abp.AutoMapper;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Dtos;

namespace SnAbp.Exam
{
    public class ExamApplicationAutoMapperProfile : Profile
    {
        public ExamApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            //CreateMap<QuestionCreateDto, Question>();
            //CreateMap<QuestionSimpleDto, QuestionDto>();
            //CreateMap<OrganizationInputDto, Organization>();
            //CreateMap<OrganizationUpdateDto, Organization>();
            //CreateMap<Organization, OrganizationDto>();


            CreateMap<ExamPaperTemplate, ExamPaperTemplateDto>();
            CreateMap<ExamPaperTemplateDto, ExamPaperTemplate>();
            CreateMap<ExamPaperTemplate, ExamPaperTemplateCreateDto>();
            CreateMap<ExamPaperTemplateCreateDto, ExamPaperTemplate>();
            CreateMap<ExamPaperTemplate, ExamPaperTemplateSearchDto>();
            CreateMap<ExamPaperTemplateSearchDto, ExamPaperTemplate>();
            CreateMap<ExamPaperTemplateUpdateDto, ExamPaperTemplate>();
            CreateMap<ExamPaperTemplate, ExamPaperTemplateUpdateDto>();
            CreateMap<ExamPaperTemplateConfig, ExamPaperTemplateConfigDto>();
            CreateMap<ExamPaperTemplateConfigDto, ExamPaperTemplateConfig>();
            CreateMap<ExamPaperTemplateConfig, ExamPaperTemplateConfigCreateDto>();
            CreateMap<ExamPaperTemplateConfigCreateDto, ExamPaperTemplateConfig>();

            CreateMap<ExamPaperTemplate, ExamPaperTemplateSimpleDto>();
            CreateMap<QuestionRltCategory, ExamPaperQuestionRltCategorySimpleDto>();
            CreateMap<QuestionRltKnowledgePoint, ExamPaperQuestionRltKnowledgePointSimpleDto>();
            CreateMap<ExamPaperTemplateSimpleDto, ExamPaperTemplate>();
            CreateMap<ExamPaperQuestionRltCategorySimpleDto, QuestionRltCategory>(); 
            CreateMap<ExamPaperQuestionRltKnowledgePointSimpleDto, QuestionRltKnowledgePoint>();



            CreateMap<Category, ExamCategorySimpleDto>();
            CreateMap<Category, ExamCategoryUpdateDto>();
            CreateMap<Category, ExamCategoryDto>();
            CreateMap<ExamCategorySimpleDto, Category>();
            CreateMap<ExamCategoryUpdateDto, Category>();
            CreateMap<ExamCategoryDto, Category>();

            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>();
            CreateMap<Question, QuestionUpdateDto>();
            CreateMap<QuestionUpdateDto, Question>();
            CreateMap<Question, QuestionSimpleDto>();
            CreateMap<QuestionSimpleDto, Question>();
            CreateMap<Question, QuestionSearchDto>();
            CreateMap<QuestionSearchDto, Question>();
            CreateMap<Question, ExamPaperRltQuestionConfigDto>();
            CreateMap<ExamPaperRltQuestionConfigDto, Question>();


            CreateMap<ExamPaper, ExamPaperDto>();
            CreateMap<ExamPaper, ExamPaperCreateDto>();
            CreateMap<ExamPaper, ExamPaperUpdateDto>();
            CreateMap<ExamPaperDto, ExamPaper>();
            CreateMap<ExamPaperCreateDto, ExamPaper>();
            CreateMap<ExamPaperUpdateDto, ExamPaper>();

            CreateMap<ExamPaperRltQuestionDto, ExamPaperRltQuestion>();
            CreateMap<ExamPaperRltQuestion, ExamPaperRltQuestionDto>();


            CreateMap<KnowledgePoint, KnowledgePointDto>();
            CreateMap<KnowledgePointDto, KnowledgePoint>();

            CreateMap<KnowledgePoint, KnowledegPointUpdateDto>();
            CreateMap<KnowledegPointUpdateDto, KnowledgePoint>();

            CreateMap<KnowledgePointRltCategory, KnowledgePointRltCategorySimpleDto>();
            CreateMap<KnowledgePointRltCategorySimpleDto, KnowledgePointRltCategory>();


            CreateMap<AnswerOption, AnswerOptionSimpleDto>();
            CreateMap<AnswerOptionSimpleDto, AnswerOption>();

            CreateMap<QuestionRltKnowledgePoint, QuestionRltKnowledgePointSimpleDto>();
            CreateMap<QuestionRltKnowledgePoint, string>().ConvertUsing(source => source.KnowledgePoint != null ? source.KnowledgePoint.Name : null);

            CreateMap<QuestionRltKnowledgePointSimpleDto, QuestionRltKnowledgePoint>();

            CreateMap<QuestionRltCategory, QuestionRltCategorySimpleDto>();
            CreateMap<QuestionRltCategory, string>().ConvertUsing(source => source.Category != null ? source.Category.Name : null);

            CreateMap<QuestionRltCategorySimpleDto, QuestionRltCategory>();
        }
    } 
}