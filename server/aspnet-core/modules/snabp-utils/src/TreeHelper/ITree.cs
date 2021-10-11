using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.TreeHelper
{
    /// <summary>
    /// 树状实体接口
    /// </summary>
    public interface ITree<KeyT, T>
    {
        KeyT Id { get; }
        KeyT ParentId { get; set; }
        T Parent { get; set; }
        List<T> Children { get; set; }
    }

  
}
