/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******类 名 称： DataDictionaryAppService
*******类 说 明： 数字字典服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/18 14:28:52
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    [Authorize]
    public class AppDataDictionaryAppService : IdentityAppServiceBase, IAppDataDictionaryAppService
    {

        protected IDataDictionaryRepository DataDictionaryRepository { get; }
        public AppDataDictionaryAppService(IDataDictionaryRepository dataDictionaryRepository)
        {
            DataDictionaryRepository = dataDictionaryRepository;
        }

        public async Task<List<DataDictionaryDto>> GetAllKeys()
        {
            List<DataDictionaryDto> result = null;
            await Task.Run(() =>
            {
                IEnumerable<DataDictionary> ddList = DataDictionaryRepository.WhereIf(true, d => !d.ParentId.HasValue);
                SortDataDictionary(ddList.ToList());
                result = ObjectMapper.Map<IEnumerable<DataDictionary>, List<DataDictionaryDto>>(ddList);
            });
            return result;
        }

        public async Task<List<DataDictionaryDto>> GetTreeListAsync(DataDictionaryGetDto input)
        {
            var name = "";
            if (input != null && !string.IsNullOrEmpty(input.Name))
            {
                name = input.Name;
            }

            var ddList = await DataDictionaryRepository.GetListAsync(true);

            var ddKeyList = ddList.FindAll(m => m.ParentId == null && m.Name != null && m.Name.Contains(name));

            SortDataDictionary(ddKeyList);

            return ObjectMapper.Map<List<DataDictionary>, List<DataDictionaryDto>>(ddKeyList);
        }

        public async Task<List<DataDictionaryDto>> GetValues(string groupCode)
        {
            List<DataDictionaryDto> result = null;
            await Task.Run(() =>
            {
                var ddList = DataDictionaryRepository.WhereIf(true, d => d.Key.StartsWith(groupCode) && d.Key.Length > groupCode.Length && d.ParentId.HasValue).ToList();

                {
                    var rootList = ddList.FindAll(d => !ddList.Exists(pd => pd.Id == d.ParentId));

                    foreach (var item in rootList)
                    {
                        item.Children = GetChildren(item, ddList);
                    }

                    SortDataDictionary(rootList);
                    result = ObjectMapper.Map<IEnumerable<DataDictionary>, List<DataDictionaryDto>>(rootList);
                }
            });

            return result;
        }


        [Authorize(IdentityPermissions.DataDictionary.Create)]
        public async Task<DataDictionaryDto> CreateAsync(DataDictionaryCreateDto input)
        {
            input.Name = input.Name.Trim();
            input.Key = input.Key;
            CheckLength(input.Name, input.Key, input.Remark);
            await CheckSameNameAndKey(input.ParentId, null, input.Name, input.Key);
            var model = new DataDictionary(Guid.NewGuid());
            model = ObjectMapper.Map<DataDictionaryCreateDto, DataDictionary>(input);
            model.SetId(Guid.NewGuid());
            var newDD = await DataDictionaryRepository.InsertAsync(model);
            return ObjectMapper.Map<DataDictionary, DataDictionaryDto>(newDD);
        }

        [Authorize(IdentityPermissions.DataDictionary.Update)]
        public async Task<DataDictionaryDto> UpdateAsync(Guid id, DataDictionaryUpdateDto input)
        {

            var dbData = await DataDictionaryRepository.GetAsync(id);
            if (dbData == null) throw new UserFriendlyException("编辑数据字段实体不存在！");
            dbData.Name = input.Name.Trim();
            dbData.Key = input.Key.Trim();
            dbData.Remark = input.Remark.Trim();
            dbData.Order = input.Order;
            CheckLength(dbData.Name, dbData.Key, dbData.Remark);
            await CheckSameNameAndKey(dbData.ParentId, id, dbData.Name, dbData.Key);
            var dto = await DataDictionaryRepository.UpdateAsync(dbData);
            return ObjectMapper.Map<DataDictionary, DataDictionaryDto>(dto);
        }

        [Authorize(IdentityPermissions.DataDictionary.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var dd = await DataDictionaryRepository.GetAsync(id);
            var childCount = DataDictionaryRepository.Count(m => m.ParentId == dd.Id);

            if (childCount > 0)
            {
                throw new UserFriendlyException("需要先删除子节点！！");
            }
            await DataDictionaryRepository.DeleteAsync(id);
        }

        async Task<bool> CheckSameNameAndKey(Guid? parentId, Guid? id, string name, string key)
        {
            return await Task.Run(() =>
            {
                if (parentId == null) return true;

                var sameNames =
                    DataDictionaryRepository.FirstOrDefault(a =>
                        a.Name == name && a.ParentId == parentId && a.Id != id);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("字典值重复，请修改字典值！");
                }

                var sameKeys = DataDictionaryRepository.Where(d => d.Key == key && d.Id != id).ToList();
                if (sameKeys.Count > 0)
                {
                    throw new UserFriendlyException("标识重复，请修改标识！");
                }

                return true;
            });
        }

        static void CheckLength(string name, string key, string remark)
        {
            if (string.IsNullOrWhiteSpace(name.Trim()))
            {
                throw new UserFriendlyException("请输入名称");
            }

            if (name.Trim().Length > 100)
            {
                throw new UserFriendlyException("名称过长（最大长度100）");
            }

            if (string.IsNullOrWhiteSpace(key.Trim()))
            {
                throw new UserFriendlyException("请输入标识");
            }

            if (key.Trim().Length > 100)
            {
                throw new UserFriendlyException("标识过长（最大长度100）");
            }

            if (remark.Trim().Length > 500)
            {
                throw new UserFriendlyException("备注过长（最大长度500）");
            }
        }

        static List<DataDictionary> GetChildren(DataDictionary parent, List<DataDictionary> ddList)
        {
            var result = new List<DataDictionary>();

            foreach (var item in ddList.Where(item => parent.Id == item.ParentId))
            {
                item.Children = GetChildren(item, ddList);
                result.Add(item);
            }

            return result;
        }

        public async Task<PagedResultDto<DataDictionaryDto>> GetListAsync(DataDictionaryDto input)
        {
            var pageData = new PagedResultDto<DataDictionaryDto>();
            var result = DataDictionaryRepository
                .WithDetails()
                .WhereIf(!string.IsNullOrEmpty(input.Name), a => a.Name.Contains(input.Name))
                .WhereIf(input.ParentId != null, a => a.ParentId == input.ParentId)
                .WhereIf(!string.IsNullOrEmpty(input.Key), a => a.Key == input.Key)
                .WhereIf(!string.IsNullOrEmpty(input.Remark), a => a.Remark.Contains(input.Remark))
                .WhereIf(input.IsStatic, a => a.IsStatic);
            pageData.TotalCount = result.Count();
            pageData.Items = ObjectMapper.Map<List<DataDictionary>, List<DataDictionaryDto>>(result.Skip(0).Take(20).ToList());

            return pageData;
        }

        static void SortDataDictionary(List<DataDictionary> ddList)
        {
            if (ddList == null)
            {
                return;
            }

            ddList.Sort((x, y) => { return x.Order - y.Order; });
            foreach (var item in ddList)
            {
                SortDataDictionary(item.Children);
            }
        }
    }
}