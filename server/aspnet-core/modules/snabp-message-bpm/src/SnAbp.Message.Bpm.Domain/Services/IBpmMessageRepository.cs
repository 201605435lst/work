/**********************************************************************
*******命名空间： SnAbp.Message.Bpm.Services
*******接口名称： IBpmMessageRepository
*******接口说明： 
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/11 8:46:02
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Message.Bpm.Dtos;
using SnAbp.Message.Bpm.Entities;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Message.Bpm.Services
{
    public interface IBpmMessageRepository: IRepository<BpmRltMessage, Guid>
    {
        Task<BpmRltMessage> Insert(BpmRltMessage model);
        Task<List<BpmRltMessage>> GetNoProcessMessage(string userId);
        Task Update(string messageId);
        Task<BpmRltMessage> UpdateRange(List<Guid> messageIds);
        Task<List<BpmRltMessage>> GetList(string keyword,bool? isProcess);
        Task<bool> Delete(string messageId);
    }
}
