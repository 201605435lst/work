using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SnAbp.Basic.Enums;
using SnAbp.Identity;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Basic.Dtos
{
    public class InstallationSiteSearchMiniDto : EntityDto<Guid>
    {

        /// <summary>
        /// 安装位置(机房、区域、位置)
        /// </summary>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }

        /// <summary>
        /// 是否显示线路信息
        /// </summary>
        [MaxLength(100)]
        public bool ?HaveRailway { get; set; }



    }
}
