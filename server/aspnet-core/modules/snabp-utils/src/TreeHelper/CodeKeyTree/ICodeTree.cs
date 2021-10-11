using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Utils.TreeHelper
{
    /// <summary>
    /// Code类型树状实体接口
    /// </summary>
    public interface ICodeTree<T>
    {
        string Code { get; set; }
        T Parent { get; set; }
        List<T> Children { get; set; }
    }
}
