using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.Project.enums;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Project.Dtos
{
    public class UnitDto 
    {
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public UnitType Type { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankCode { get; set; }
    }
}
