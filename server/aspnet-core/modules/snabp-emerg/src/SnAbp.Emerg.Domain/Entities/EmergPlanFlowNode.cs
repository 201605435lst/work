using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Emerg.Entities
{
    [NotMapped]
    /// <summary>
    /// 流程节点
    /// </summary>
    public class EmergPlanFlowNode 
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "size")]
        public List<float> Size { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "x")]
        public float X { get; set; }
        [JsonProperty(PropertyName = "y")]
        public float Y { get; set; }
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; } = false;
        [JsonProperty(PropertyName = "processed")]
        public bool Processed { get; set; } = false;
        [JsonProperty(PropertyName = "members")]
        public virtual List<Member> Members { get; set; }
        [JsonProperty(PropertyName = "comments")]
        /// <summary>
        /// 意见
        /// </summary>
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "processTime")]
        /// <summary>
        /// 节点处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }

    }
}
