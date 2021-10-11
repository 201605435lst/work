using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompanyName.MyProjectName.TemplateModel
{
    public class ProjectTemplate
    {
        public int Index { get; set; }
        // 模块名称
        public string ModuleName { get; set; }
        // 子模块
        public string SubModuleName { get; set; }
        // 内容
        public string Content { get; set; }
        // 工作量
        public float WorkDays { get; set; }
        // 进度
        public float Progress { get; set; }
        // 备注
        public string Remark { get; set; }
    }
}
