using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.TreeHelper
{
    /// <summary>
    /// 树状实体接口
    /// </summary>
    public interface IGuidKeyTree<T>
    {
        Guid Id { get; }
        Guid? ParentId { get; set; }
        T Parent { get; set; }
        List<T> Children { get; set; }
    }
}
