using SnAbp.Exam.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.Exam
{
    public abstract class ExamController : AbpController
    {
        protected ExamController()
        {
            LocalizationResource = typeof(ExamResource);
        }
    }
}
