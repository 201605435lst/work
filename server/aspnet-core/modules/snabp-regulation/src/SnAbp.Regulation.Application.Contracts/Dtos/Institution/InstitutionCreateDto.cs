using SnAbp.Identity;
using SnAbp.Regulation.Dtos.Label;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionCreateDto : EntityDto<Guid>
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
        ///制度分类
        ///</summary>
        public int Classify { get; set; }

        ///<summary>
        ///所属部门
        ///</summary>
        public virtual Guid OrganizationId { get; set; }

        ///<summary>
        ///生效时间
        ///</summary>
        public DateTime EffectiveTime { get; set; }

        ///<summary>
        ///过期时间
        ///</summary>
        public DateTime ExpireTime { get; set; }

        ///<summary>
        ///摘要
        ///</summary>
        public string Abstract { get; set; }


        ///<summary>
        ///是否发布
        ///</summary>
        public bool IsPublish { get; set; }

        ///<summary>
        ///发布类别
        ///</summary>
        public int NewsClassify { get; set; }

        ///<summary>
        ///是否审批
        ///</summary>
        public bool IsApprove { get; set; }

        ///<summary>
        ///文件附件
        ///</summary>
        public List<Guid> InstitutionRltFiles { get; set; }

        ///<summary>
        ///标签
        ///</summary>
        public List<Guid> InstitutionRltLabels { get; set; }

        /// <summary>
        /// 可查看者
        /// </summary>
        public List<MemberInfoDto> ListView { get; set; }

        /// <summary>
        /// 可编辑者
        /// </summary>
        public List<MemberInfoDto> ListEdit { get; set; }

        /// <summary>
        /// 可下载者
        /// </summary>
        public List<MemberInfoDto> ListDownLoad { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public decimal Version { get; set; }

        /// <summary>
        /// 修改权限(1:批量修改权限)
        /// </summary>
        public int Flag { get; set; }

        public List<Guid> SelectedIds { get; set; }
    }
}
