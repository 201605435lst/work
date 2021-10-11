
namespace SnAbp.Quality
{
    /// <summary>
    /// 问题库导入模板
    /// </summary>
    public class QualityProblemLibraryUploadTemplate
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 工作内容
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 所属专业
        /// </summary>
        public string Profession { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 问题等级
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///  控制措施
        /// </summary>
        public string Measures { get; set; }
        /// <summary>
        /// 使用范围 单位名称之间用逗号隔开
        /// </summary>
        public string Scops { get; set; }
    }
}
