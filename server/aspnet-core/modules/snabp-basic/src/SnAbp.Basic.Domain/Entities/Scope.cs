using SnAbp.Basic.Enums;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SnAbp.Basic.Entities
{
    [NotMapped]
    public class Scope
    {

        /// <summary>
        /// 范围编码
        /// </summary>
        public string ScopeCode { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public ScopeType? Type
        {
            get
            {
                if (ScopeCode == null)
                {
                    return null;
                }
                else
                {
                    var scopes = ScopeCode.Split('.');
                    if (scopes.Count() > 0)
                    {
                        return (ScopeType)int.Parse(scopes.Last().Split('@').First());
                    }
                    return null;
                }
            }
        }

        public Guid? Id
        {
            get
            {
                if (ScopeCode == null)
                {
                    return null;
                }
                else
                {
                    var scopes = ScopeCode.Split('.');
                    if (scopes.Count() > 0)
                    {
                        return new Guid(scopes.Last().Split('@').Last());
                    }
                    return null;
                }
            }
        }

        public Scope(string scopeCode)
        {
            ScopeCode = scopeCode;
        }
    }
}
