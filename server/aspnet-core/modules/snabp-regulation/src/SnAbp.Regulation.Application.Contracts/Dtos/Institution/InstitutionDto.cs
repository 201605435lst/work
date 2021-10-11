using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionDto : AuditedEntityDto<Guid>
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
        
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        ///<summary>
        ///所属部门
        ///</summary>
        public virtual Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        /// <summary>
        /// 文件附件
        /// </summary>
        public List<InstitutionRltFileDto> InstitutionRltFiles { get; set; }

        /// <summary>
        /// 关联标签
        /// </summary>
        public List<InstitutionRltLabelDto> InstitutionRltLabels { get; set; }

        ///<summary>
        ///权限
        ///</summary>
        public List<InstitutionRltAuthorityDto> InstitutionRltAuthorities { get; set; }

        public List<InstitutionRltEditionDto> InstitutionRltEditinos { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public decimal Version { get; set; }

        public List<MemberInfoDto> ListView { get; set; }= new List<MemberInfoDto>();
        public List<MemberInfoDto> ListEdit { get; set; } = new List<MemberInfoDto>();
        public List<MemberInfoDto> ListDownLoad { get; set; } = new List<MemberInfoDto>();

        //当前用户可以查看的制度
        public Guid ViewInstitution { get; set; }
        public Guid EditInstitution { get; set; }
        public Guid DownLoadInstitution { get; set; } 

    }
}
