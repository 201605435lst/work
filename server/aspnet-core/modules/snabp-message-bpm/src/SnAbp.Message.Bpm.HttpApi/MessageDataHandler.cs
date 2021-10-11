/**********************************************************************
*******命名空间： SnAbp.Message.Bpm
*******类 名 称： MessageDataHandler
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/11 18:00:04
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using SnAbp.Message.Bpm.Dtos;
using SnAbp.Message.Bpm.Entities;

namespace SnAbp.Message.Bpm
{
    public static class MessageDataHandler
    {
        public static BpmMessageDto TransData(BpmRltMessage message) => new BpmMessageDto()
        {
            MessageId = message.Id,
            UserName = message.User.Name,
            FlowId = message.WorkflowId,
            FlowName = message.Workflow.FlowTemplate.FormTemplate.WorkflowTemplate.Name,
            Process = message.Process,
            Sponsor = message.Sponsor.Name,
            Processor = message.Processor.Name,
            CreateTime = message.CreationTime,
            Type = message.Type,
            State = (int)message.State
        };
    }
}