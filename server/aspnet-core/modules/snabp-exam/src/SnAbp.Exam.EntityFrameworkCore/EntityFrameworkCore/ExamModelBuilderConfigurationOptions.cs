using JetBrains.Annotations;

using SnAbp.EntityFrameworkCore.Modeling;

namespace SnAbp.Exam.EntityFrameworkCore
{
    public class ExamModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ExamModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}