using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Authorization;
using SnAbp.StdBasic.Dto;
using SnAbp.StdBasic.Entities;
//using SnAbp.StdBasic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp; 
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    //[Authorize(StdBasicPermissions.Orgs_Organization)]
    [Authorize]
    public class OrganizationAppService : StdBasicAppService, IOrganizationAppService
    {
        private readonly IRepository<Organization, Guid> orgRepository;
        //private readonly IOrganizationRespository orgRepository;

        public OrganizationAppService(IRepository<Organization, Guid> organization)
        {
            this.orgRepository = organization;
        }

        [Authorize(StdBasicPermissions.Orgs_Organization)]
        public virtual async Task<OrganizationDto> CreateAsync(OrganizationInputDto input)
        {
            string code = await CreateNewOrgCode(input.ParentId);

            Organization org = new Organization(Guid.NewGuid());
            org = ObjectMapper.Map(input, org);
            org.Code = code;

            Organization newOrg = await orgRepository.InsertAsync(org);

            return ObjectMapper.Map<Organization, OrganizationDto>(newOrg);
        }
        /// <summary>
        /// 生成新的机构树形码
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private async Task<string> CreateNewOrgCode(Guid? parentId)
        {
            Organization parent = null;
            string parentCode = "";
            if (parentId.HasValue)
            {
                parent = await orgRepository.GetAsync(parentId.Value);
                parentCode = parent != null ? parent.Code : "";
            }

            List<Organization> orgList = await orgRepository.GetListAsync();

            List<Organization> orgChildren = orgList.FindAll(o =>  o.Code.Length == parentCode.Length + 4 && o.Code.Left(parentCode.Length) == parentCode);

            if (orgChildren != null && orgChildren.Count > 0)
            {
                Organization lastOrg = orgChildren.OrderBy<Organization, string>(o => o.Code).Last();
                return parentCode + (Convert.ToInt32(lastOrg.Code.Right(4)) + 1).ToString().PadLeft(4, '0');
            }
            else
            {
                return parentCode + "0001";
            }
        }

        [Authorize(StdBasicPermissions.Orgs_Organization)]
        public async Task<bool> DeleteAsync(Guid id)
        {
            Organization org = await orgRepository.GetAsync(id);

            if (org == null)
            {
                throw new UserFriendlyException("未找到该机构，或已被删除！！！");
                //throw new BusinessException("未找到该机构，或已被删除！！！");
            }
            List<Organization> orgAllList = await orgRepository.GetListAsync();

            List<Organization> children = orgAllList.FindAll(o => o.Code.Length > org.Code.Length && o.Code.Left(org.Code.Length) == org.Code && o.Code.Length == org.Code.Length + 4);

            if (children != null && children.Count() > 0)
            {
                throw new UserFriendlyException("需要先删除下级机构！！");
            }

            await orgRepository.DeleteAsync(id);

            return true;
        }

        public async Task<List<OrganizationDto>> GetListAsync(OrganizationGetDto input)
        {
            List<OrganizationDto> orgDtoList = new List<OrganizationDto>();

            if (input.Name == null)
            {
                input.Name = "";
            }

            await Task.Run(() =>
            {
                List<Organization> orgList = orgRepository.WhereIf(!string.IsNullOrEmpty(input.Name), o => o.Name.Contains(input.Name)).ToList();

                //if(string.IsNullOrEmpty(input.Sort))
                //{
                //    switch (input.Sort)
                //    {
                //        case "Name": orgList.Sort((x, y) => { return string.Compare(x.Name, y.Name); }); break;
                //        case "Order":
                //            orgList.Sort((x, y) => { return x.Order - y.Order; });break;
                //        case "ShortName":
                //            orgList.Sort((x, y) => { return string.Compare(x.ShortName, y.ShortName); }); break;
                //    }
                //}

                orgList.Sort((x, y) => { return x.Order - y.Order; });

                if (!string.IsNullOrEmpty(input.Name))
                {
                    orgDtoList = ObjectMapper.Map<List<Organization>, List<OrganizationDto>>(orgList);
                }
                else
                {
                    foreach (var item in orgList)
                    {
                        if (item.Code.Length == 4)
                        {
                            string parentCode = item.Code;
                            OrganizationDto dto = ObjectMapper.Map<Organization, OrganizationDto>(item);
                            dto.Children = GetChildren(item, orgList);
                            dto.ParentId = null;
                            orgDtoList.Add(dto);
                        }
                    }
                }
            });
            return orgDtoList;
        }

        private List<OrganizationDto> GetChildren(Organization parent, List<Organization> orgList)
        {
            List<OrganizationDto> children = new List<OrganizationDto>();
            Regex regChildPre = new Regex(@"^" + parent.Code);
            foreach (var item in orgList)
            {
                if (regChildPre.IsMatch(item.Code) && item.Code.Length == parent.Code.Length + 4)
                {
                    OrganizationDto dto = ObjectMapper.Map<Organization, OrganizationDto>(item);
                    dto.Children = GetChildren(item, orgList);
                    dto.ParentId = parent.Id;
                    children.Add(dto);
                }
            }
            return children;
        }

        [Authorize(StdBasicPermissions.Orgs_Organization)]
        public async Task<bool> UpdateAsync(OrganizationUpdateDto input)
        {
            Organization org = await orgRepository.GetAsync(input.Id);
            Guid? parentIdDB = null;
            if (org.Code.Length <= 4)
            {
                parentIdDB = null;
            }
            else
            {
                IEnumerable<Organization> parentOrgDb = orgRepository.Where(o => o.Code == org.Code.Left(org.Code.Length - 4));
                if (parentOrgDb.Count() > 0)
                {
                    parentIdDB = parentOrgDb.First().Id;
                }
                else
                {
                    parentIdDB = null;
                }
            }

            org = ObjectMapper.Map(input, org);
            string oldCode = org.Code;

            if (input.ParentId != parentIdDB)
            {
                oldCode = org.Code;
                org.Code = await CreateNewOrgCode(parentIdDB);
            }

            Regex regChildPre = new Regex(@"^" + oldCode);

            IQueryable<Organization> children = orgRepository.Where(o => o.Code.Left(oldCode.Length) == oldCode && org.Code.Length == oldCode.Length + 4);

            if (children != null && children.Count() > 0)
            {
                foreach (var item in children)
                {
                    regChildPre.Replace(item.Code, org.Code);
                }
            }

            await orgRepository.UpdateAsync(org);

            return true;

        }

        public async Task<string> Test(string txt)
        {
            Random rand = new Random();
            string result = "";
            await Task.Run(() =>
            {
                result = txt + rand.Next();
            });
            return result;
        }

        public async Task<OrganizationDto> GetAsync(Guid id)
        {
            try
            {
                Organization org = await orgRepository.GetAsync(id);

                return ObjectMapper.Map<Organization, OrganizationDto>(org);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
