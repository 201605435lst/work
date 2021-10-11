using SnAbp.MultiProject.MultiProject;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnAbp.Emerg.Entities
{
    [NotMapped]
    /// <summary>
    /// 流程节点
    /// </summary>
    public class EmergPlanFlowEdge
    {
        public Guid Id { get; set; }
        public Guid Source { get; set; }
        public Guid Target { get; set; }
        public int SourceAnchor { get; set; }
        public int TargetAnchor { get; set; }
    }
}
