using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    public class SupplierRltContactsUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        public string LandlinePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
    }
}
