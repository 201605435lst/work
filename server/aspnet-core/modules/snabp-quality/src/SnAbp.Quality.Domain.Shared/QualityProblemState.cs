/**********************************************************************
*******命名空间： SnAbp.Quality
*******类 名 称： QualityProblemState
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/4/29 18:55:51
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System.ComponentModel;

namespace SnAbp.Quality
{
    /// <summary>
    /// $$
    /// </summary>
    public enum QualityProblemState
    {
        [Description("待整改")]
        WaitingImprove = 1,
        [Description("待审查")]
        WaitingVerifie = 2,
        [Description("已整改")]
        Improved = 3
    }
}
