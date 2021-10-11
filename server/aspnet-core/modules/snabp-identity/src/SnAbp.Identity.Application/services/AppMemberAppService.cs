using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    public class AppMemberAppService : IdentityAppServiceBase, IMemberAppService
    {
        private IRepository<IdentityUser> _userRepository;
        private IRepository<IdentityRole> _roleRepository;
        private IRepository<Organization> _organizationRepository;

        public AppMemberAppService(
         IRepository<IdentityUser> userRepository,
         IRepository<IdentityRole> roleRepository,
         IRepository<Organization> organizationRepository
         )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        /// <summary>
        /// 查询成员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<MemberDto>> Search(MemberSearchInputDto input)
        {
            if (input.MemberInfos == null)
            {
                throw new UserFriendlyException("成员信息不能为空");
            }

            List<Guid> organizationIds = new List<Guid>();
            List<Guid> roleIds = new List<Guid>();
            List<Guid> userIds = new List<Guid>();

            foreach (var item in input.MemberInfos)
            {
                if (item.Type == MemberType.Organization)
                {
                    organizationIds.Add(item.Id);
                }
                else if (item.Type == MemberType.Role)
                {
                    roleIds.Add(item.Id);
                }
                else if (item.Type == MemberType.User)
                {
                    userIds.Add(item.Id);
                }
            }

            var organizations = _organizationRepository.Where(x => organizationIds.Contains(x.Id)).ToList();
            var roles = _roleRepository.Where(x => roleIds.Contains(x.Id)).ToList();
            var users = _userRepository.Where(x => userIds.Contains(x.Id)).ToList();

            List<MemberDto> list = new List<MemberDto>();
            list.AddRange(ObjectMapper.Map<List<Organization>, List<MemberDto>>(organizations));
            list.AddRange(ObjectMapper.Map<List<IdentityRole>, List<MemberDto>>(roles));
            list.AddRange(ObjectMapper.Map<List<IdentityUser>, List<MemberDto>>(users));

            return Task.FromResult(list);
        }
    }
}
