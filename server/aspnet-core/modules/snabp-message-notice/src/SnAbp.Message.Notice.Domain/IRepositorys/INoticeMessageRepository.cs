/**********************************************************************
*******命名空间： SnAbp.Message.Notice.IRepositorys
*******接口名称： INoticeMessageRepository
*******接口说明： 
*******作    者： 东腾 Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/25 10:22:25
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2019-2020. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SnAbp.Message.Notice.IRepositorys
{
    public interface INoticeMessageRepository
    {
        Task<Entities.Notice> InsertAsync(Entities.Notice data);
        Task<List<Entities.Notice>> GetNoProcessMessage(string userId);
    }
}
