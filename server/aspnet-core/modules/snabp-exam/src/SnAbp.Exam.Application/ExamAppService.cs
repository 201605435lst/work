using SnAbp.Exam.Localization;
using Volo.Abp.Application.Services;

namespace SnAbp.Exam
{
    public abstract class ExamAppService : ApplicationService
    {
        protected ExamAppService()
        {
            LocalizationResource = typeof(ExamResource);
            ObjectMapperContext = typeof(ExamApplicationModule);
        }
    }
} 
