using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace SnAbp.Bpm.Events
{
    class WorkflowEventHandler : ILocalEventHandler<EntityChangedEventData<Workflow>>
    {
        public int EntityChangedEventCount { get; set; }

        public Task HandleEventAsync(EntityChangedEventData<Workflow> eventData)
        {
            EntityChangedEventCount++;
            return Task.CompletedTask;
        }

    }
}
