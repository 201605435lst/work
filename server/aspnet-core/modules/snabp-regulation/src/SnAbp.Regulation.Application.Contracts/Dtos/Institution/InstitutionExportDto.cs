using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;


namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionExportDto:AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public string State { get; set; }

        ///<summary>
        ///所属部门
        ///</summary>
        public virtual Organization Organization { get; set; }

        ///<summary>
        ///制度分类
        ///</summary>
        public int Classify { get; set; }

        ///<summary>
        ///录入人
        ///</summary>
        public string InputPeople { get; set; }

        ///<summary>
        ///生效时间
        ///</summary>
        public DateTime EffectiveTime { get; set; }
    }
}
