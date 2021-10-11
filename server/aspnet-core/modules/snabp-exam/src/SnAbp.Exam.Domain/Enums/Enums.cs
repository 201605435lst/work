using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SnAbp.Exam.Enums
{
    public enum QuestionType
    {
        [Description("单选")]
        SingleChoice = 1,
        [Description("多选")]
        MultipleChoice = 2,
        [Description("判断")]
        TrueOrFalseQuestions = 3,
        [Description("填空")]
        GapFilling = 4,
        [Description("简答")]
        ShortAnswerQuestion = 5,
    }


    public enum GroupQuestionType
    {
        [Description("随机")]
        Random = 1,
        [Description("手动")]
        Manual = 2,
    }
}
