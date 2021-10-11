using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Identity
{
    public interface IMemberAppService : IApplicationService
    {
        /// <summary>
        /// 给机构创建角色
        /// </summary>
        /// <param name="input">角色信息</param>
        /// <returns></returns>
        Task<List<MemberDto>> Search(MemberSearchInputDto input);
    }
}
