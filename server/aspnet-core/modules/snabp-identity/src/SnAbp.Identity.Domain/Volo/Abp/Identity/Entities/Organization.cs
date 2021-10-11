using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using SnAbp.MultiProject.MultiProject;

namespace SnAbp.Identity
{
    /// <summary>
    /// Represents an organization unit (OU).
    /// </summary>
    public class Organization : FullAuditedAggregateRoot<Guid>, IMultiTenant, ICodeTree<Organization>, IGuidKeyTree<Organization>
    {
        public virtual Guid? TenantId { get; set; }

        /// <summary>
        /// Hierarchical Code of this organization unit.
        /// Example: "0001.0042.0005".
        /// This is a unique code for a Tenant.
        /// It's changeable if OU hierarchy is changed.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Display name of this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 铁路编码 <code>Easten新增</code>
        /// </summary>
        [MaxLength(50)] public string CSRGCode { get; set; }

        /// <summary>
        /// 简介 
        /// </summary>
        [MaxLength(200)] public string Description { get; set; }

        /// <summary>
        /// 排序编码  <code>Easten新增</code>
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否根级组织
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 单位性质
        /// </summary>
        [MaxLength(100)] public string Nature { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)] public string Remark { get; set; }

        /// <summary>
        /// Parent <see cref="Organization"/> Id.
        /// Null, if this OU is a root.
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        public virtual Organization Parent { get; set; }
        public List<Organization> Children { get; set; }

        /// <summary>
        /// 组织机构类型
        /// </summary>
        [ForeignKey("Type")]
        public Guid? TypeId { get; set; }
        public DataDictionary Type { get; set; }


        /// <summary>
        /// 印章图片地址
        /// </summary>
        [CanBeNull]
        public string SealImageUrl { get; set; }


        /// <summary>
        /// Roles of this OU.
        /// </summary>
        public virtual ICollection<OrganizationRltRole> Roles { get; set; }

       
        /// <summary>
        /// Initializes a new instance of the <see cref="Organization"/> class.
        /// </summary>
        public Organization()
        {

        }

        public void SetId(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Creates code for given numbers.
        /// Example: if numbers are 4,2 then returns "00004.00002";
        /// </summary>
        /// <param name="numbers">Numbers</param>
        public static string CreateCode(params int[] numbers)
        {
            if (numbers.IsNullOrEmpty())
            {
                return null;
            }

            return numbers.Select(number => number.ToString(new string('0', OrganizationConsts.CodeUnitLength))).JoinAsString(".");
        }

        /// <summary>
        /// Appends a child code to a parent code. 
        /// Example: if parentCode = "0001", childCode = "0042" then returns "0001.0042".
        /// </summary>
        /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
        /// <param name="childCode">Child code.</param>
        public static string AppendCode(string parentCode, string childCode)
        {
            if (childCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return childCode;
            }

            return parentCode + "." + childCode;
        }

        /// <summary>
        /// Gets relative code to the parent.
        /// Example: if code = "0019.0055.0001" and parentCode = "0009" then returns "0005.0001".
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="parentCode">The parent code.</param>
        public static string GetRelativeCode(string code, string parentCode)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            if (parentCode.IsNullOrEmpty())
            {
                return code;
            }

            if (code.Length == parentCode.Length)
            {
                return null;
            }

            return code.Substring(parentCode.Length + 1);
        }

        /// <summary>
        /// Calculates next code for given code.
        /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string CalculateNextCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var parentCode = GetParentCode(code);
            var lastUnitCode = GetLastUnitCode(code);

            return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
        }

        /// <summary>
        /// Gets the last unit code.
        /// Example: if code = "0019.0055.0001" returns "0001".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetLastUnitCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            return splittedCode[splittedCode.Length - 1];
        }

        /// <summary>
        /// Gets parent code.
        /// Example: if code = "0019.0055.00001" returns "0019.0055".
        /// </summary>
        /// <param name="code">The code.</param>
        public static string GetParentCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
            }

            var splittedCode = code.Split('.');
            if (splittedCode.Length == 1)
            {
                return null;
            }

            return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
        }

        public virtual void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new OrganizationRltRole(roleId, Id, TenantId));
        }

        public virtual void RemoveRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (!IsInRole(roleId))
            {
                return;
            }

            Roles.RemoveAll(r => r.RoleId == roleId);
        }

        public virtual bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }
    }
}