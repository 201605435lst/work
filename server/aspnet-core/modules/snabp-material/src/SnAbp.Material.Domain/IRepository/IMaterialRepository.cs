/**********************************************************************
*******命名空间： SnAbp.Material.Repository
*******接口名称： IMaterialRepository
*******接口说明： 
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 18:06:02
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnAbp.Material
{
    public interface IMaterialRepository
    {
        Task InsertRange<T>(List<T> range) where T : class;
    }
}
