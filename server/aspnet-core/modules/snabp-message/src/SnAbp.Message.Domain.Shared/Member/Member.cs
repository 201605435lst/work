/**********************************************************************
*******命名空间： SnAbp.Message.Member
*******类 名 称： Member
*******类 说 明： 成员信息
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/11/16 17:55:21
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;

namespace SnAbp.Message
{
    public class Member
    {
        /// <summary>
        ///     成员 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     成员类型
        /// </summary>
        public MemberType Type { get; set; }

        /// <summary>
        ///     成员名称
        /// </summary>
        public string Name { get; set; }
    }
}