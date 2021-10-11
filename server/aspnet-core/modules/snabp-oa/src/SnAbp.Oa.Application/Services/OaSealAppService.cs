using SnAbp.Identity;
using SnAbp.Oa.Dtos.Seal;
using SnAbp.Oa.Entities;
using SnAbp.Oa.Enums;
using SnAbp.Oa.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Oa.Services
{
    public class OaSealAppService : OaAppService, IOaSealAppService
    {
        private readonly IRepository<Seal, Guid> _repositorySeal;
        private readonly IRepository<SealRltMember, Guid> _repositorySealRltMembers;
        private readonly IRepository<IdentityUser, Guid> _repositoryUsers;
        private readonly IdentityUserManager _identityUserManager;   //获取当前用户仓储
        private readonly IGuidGenerator _guidGenerate;

        public OaSealAppService(
            IRepository<Seal, Guid> repositorySeal,
            IRepository<SealRltMember, Guid> repositorySealRltMembers,
            IRepository<IdentityUser, Guid> repositoryUsers,
            IdentityUserManager identityUserManager,
            IGuidGenerator guidGenerate
            )
        {
            _repositorySeal = repositorySeal;
            _repositorySealRltMembers = repositorySealRltMembers;
            _repositoryUsers = repositoryUsers;
            _identityUserManager = identityUserManager;
            _guidGenerate = guidGenerate;
        }

        public async Task<SealDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var seal = _repositorySeal.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (seal == null) throw new UserFriendlyException("此签章不存在");
            return ObjectMapper.Map<Seal, SealDto>(seal);
        }

        public async Task<PagedResultDto<SealSimpleDto>> GetList(SealSearchDto input)
        {
            var usersList = await _repositoryUsers.GetListAsync();
            //var sealRltMemberList = await _repositorySealRltMembers.GetListAsync();
            var sealsList = _repositorySeal.WithDetails().Where(x => x.IsDeleted == false);
            List<Seal> seals = new List<Seal>();
            Seal seal;
            if (input.Personal)
            {
                var sealRMs = _repositorySealRltMembers.Where(x => x.MemberId == CurrentUser.Id.Value).ToList();
                foreach (var item in sealRMs)
                {
                    seal = sealsList.FirstOrDefault(x => x.Id == item.SealId);
                    if (seal != null)
                    {
                        seals.Add(seal);
                    }
                }
            }
            else
            {
                seals = _repositorySeal.WithDetails()
                .WhereIf(input.Keywords != null, x => x.Name.Contains(input.Keywords)).ToList();
            }
            
            var result = new PagedResultDto<SealSimpleDto>();
            var sealRu = ObjectMapper.Map<List<Seal>, List<SealSimpleDto>>(seals.ToList());

            foreach (var item in sealRu)
            {
                var creates = seals.FirstOrDefault(x => x.Id == item.Id);
                if (creates.CreatorId != null && creates.CreatorId != Guid.Empty)
                {
                    item.CreatorName = usersList.FirstOrDefault(x => x.Id == creates.CreatorId)?.Name;
                }
                if (creates.SealRltMembers.Count > 0)
                {
                    foreach (var members in creates.SealRltMembers)
                    {
                        var sealRltMembers = usersList.FirstOrDefault(x => x.Id == members.MemberId);
                        if (sealRltMembers != null)
                        {
                            item.AuthorizedName = item.AuthorizedName + ","+ sealRltMembers.Name;
                        }
                    }
                    item.AuthorizedName = item.AuthorizedName.Trim(',');
                }
                else
                {
                    item.AuthorizedName = item.CreatorName;
                }
            }

            result.TotalCount = seals.Count();
            result.Items = sealRu.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            return await Task.FromResult(result);
        }

        public async Task<SealDto> Create(SealCreateDto input)
        {
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
            var memberIds = members.Select(x => x.Id).ToList();
            var memberUser = members.Where(x => x.Type == MemberType.User);

            if (input.Name == null) throw new UserFriendlyException("签章名称不能为空");
            if (!input.Type.IsIn(SealType.Company, SealType.Personal)) throw new UserFriendlyException("请选择签章类型");
            if (input.IsPublic != true && input.IsPublic != false) throw new UserFriendlyException("请选择是否为免密签名");
            if (input.IsPublic == false && input.Password == null) throw new UserFriendlyException("密码不能为空");
            if (input.Enabled != true && input.Enabled != false) throw new UserFriendlyException("请选择是否有效");

            await CheckSameName(input.Type,input.Name);

            var seal = new Seal(_guidGenerate.Create());
            seal.Name = input.Name;
            seal.Type = input.Type;
            seal.IsPublic = input.IsPublic;
            seal.Password = input.Password;
            seal.Enabled = input.Enabled;
            seal.ImageId = input.ImageId.GetValueOrDefault();

            //保存关联member表信息
            seal.SealRltMembers = new List<SealRltMember>();
            if (input.SealRltMembers.Count == 0)
            {
                seal.SealRltMembers.Add(new SealRltMember(_guidGenerate.Create())
                {
                    SealId = seal.Id,
                    MemberId = memberUser.Select(x => x.Id).First(),
                    MemberType = memberUser.Select(x => x.Type).First()
                });
            }
            else
            {
                foreach (var srm in input.SealRltMembers)
                {
                    seal.SealRltMembers.Add(new SealRltMember(_guidGenerate.Create())
                    {
                        SealId = seal.Id,
                        MemberId = input.SealRltMembers[input.SealRltMembers.IndexOf(srm)].Id,
                        MemberType = input.SealRltMembers[input.SealRltMembers.IndexOf(srm)].Type
                    });
                }
            }
            await _repositorySeal.InsertAsync(seal);

            return ObjectMapper.Map<Seal, SealDto>(seal);
        }

        public async Task<SealDto> Update(SealUpdateDto input)
        {
            Seal seal = null;
            if (input.Type == UpdateType.DecodeOrEncryption || input.Type == UpdateType.effective)
            {
                seal = await _repositorySeal.GetAsync(x => x.Id == input.Id);
                if (seal == null) throw new UserFriendlyException("该签章不存在，无法修改");
            }

            switch (input.Type)
            {
                case UpdateType.DecodeOrEncryption: //加密OR解密
                    seal.IsPublic = !input.IsPublic;
                    if (!input.IsPublic)
                    {
                        seal.Password = null;
                    }
                    else
                    {
                        seal.Password = input.Password;
                    }
                    await _repositorySeal.UpdateAsync(seal);
                    break;
                case UpdateType.effective: //使其有效
                    seal.Enabled = true;
                    await _repositorySeal.UpdateAsync(seal);
                    break;
                case UpdateType.Noneffective: //使其无效
                    foreach (var item in input.Ids)
                    {
                        seal = await _repositorySeal.GetAsync(x => item == x.Id);
                        seal.Enabled = false;
                        await _repositorySeal.UpdateAsync(seal);
                    }
                    break;
            }
           
            return ObjectMapper.Map<Seal, SealDto>(seal);
        }

        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repositorySeal.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }


        #region 私有方法

        /// <summary>
        /// 同一类型下签章名称不能重复
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        async Task<bool> CheckSameName(SealType type, string name)
        {
            return await Task.Run(() =>
            {
                var sameNames =
                    _repositorySeal.FirstOrDefault(a =>
                        a.Name == name && a.Type == type);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前类型下已存在该名称的签章！");
                }

                return true;
            });
        }

        #endregion

    }
}
