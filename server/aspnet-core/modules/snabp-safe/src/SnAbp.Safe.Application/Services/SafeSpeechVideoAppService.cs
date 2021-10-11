/**********************************************************************
*******命名空间： SnAbp.Safe.Services
*******类 名 称： SafeSpeechVideoAppService
*******类 说 明： 班前讲话视频接口服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/5/7 15:01:14
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Safe.Dtos;
using SnAbp.Safe.Entities;
using SnAbp.Safe.IServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Safe.Services
{
    /// <summary>
    /// 班前讲话视频接口服务
    /// </summary>
    public class SafeSpeechVideoAppService : SafeAppService, ISafeSpeechVideoAppService
    {
        IRepository<SafeSpeechVideo, Guid> _speechVideoRepository;

        public SafeSpeechVideoAppService(
            IRepository<SafeSpeechVideo, Guid> speechVideoRepository
            )
        {
            _speechVideoRepository = speechVideoRepository;
        }

        /// <summary>
        /// 创建班前讲话
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        //public async Task<bool> Create(SpeechVideoCreateDto input)
        //{
        //    var model = ObjectMapper.Map<SpeechVideoCreateDto, SafeSpeechVideo>(input);
        //    model.SetId(GuidGenerator.Create());
        //    await _speechVideoRepository.InsertAsync(model);
        //    return true;
        //}

        /// <summary>
        /// 获取班前讲话视频
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SafeSpeechVideoDto>> GetList(SafeSpeechVideoSimpleDto input)
        {
            //var result = new PagedResultDto<SafeSpeechVideoSimpleDto>();
            //var query = _speechVideoRepository.WithDetails(a => a.Video);
            //result.TotalCount = query.Count();
            //var list = query.WhereIf(!string.IsNullOrEmpty(input.Keywords), a => a.Site.Contains(input.Keywords) || a.Content.Contains(input.Keywords))
            //    .WhereIf(input.t != null && input.ETime != null, a => a.Date > input.STime && a.Date <= input.ETime)
            //    .PageBy(input.SkipCount, input.MaxResultCount).ToList();
            //result.Items = ObjectMapper.Map<List<SafeSpeechVideo>, List<SpeechVideoQueryDto>>(list);
            //return result;
            throw new Exception();
        }
    }
}
