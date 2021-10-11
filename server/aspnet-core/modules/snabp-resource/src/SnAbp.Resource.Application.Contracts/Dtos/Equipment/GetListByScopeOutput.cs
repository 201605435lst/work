using SnAbp.Identity;
using SnAbp.Resource.Enums;
using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SnAbp.Basic.Dtos;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.Utils.TreeHelper;

namespace SnAbp.Resource.Dtos
{
    public class GetListByScopeOutput : IGuidKeyTree<GetListByScopeOutput>
    {

        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        public GetListByScopeOutput Parent { get; set; }
        public List<GetListByScopeOutput> Children { get; set; }

        public string EquipmentGroupName { get; set; }

        /// <summary>
        /// 构件分类 Id（针对设备）
        /// </summary>
        public Guid? ComponentCategoryId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public GetListByScopeType Type { get; set; }


        /// <summary>
        /// Gis数据
        /// </summary>
        public string? GisData { get; set; }
    }
}
