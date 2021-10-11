//using SnAbp.MultiProject.MultiProject;

//using System;
//using System.Collections.Generic;
//using System.Text;
//using Volo.Abp.Domain.Entities.Auditing;

//namespace SnAbp.Material.Entities
//{
//    public class ConstructionTeam : AuditedEntity<Guid>
//    {
//        /// <summary>
//        /// 名称
//        /// </summary>
//        public string Name { get; set; }
//        /// <summary>
//        /// 施工点
//        /// </summary>
//        public virtual Guid ConstructionSectionId { get; set; }
//        public virtual ConstructionSection ConstructionSection { get; set; }

//        /// <summary>
//        /// 联系人
//        /// </summary>
//        public string PeopleName { get; set; }
//        /// <summary>
//        /// 联系电话
//        /// </summary>
//        public string Phone { get; set; }

//        public Guid? ProjectId => throw new NotImplementedException();

//        public void SetId(Guid id)
//        {
//            Id = id;
//        }
//        public ConstructionTeam() { }
//        public ConstructionTeam(Guid id)
//        {
//            Id = id;
//        }
//    }
//}

