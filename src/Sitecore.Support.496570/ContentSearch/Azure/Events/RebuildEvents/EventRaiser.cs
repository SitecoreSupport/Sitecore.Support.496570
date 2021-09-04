using Sitecore.Configuration;
﻿using Sitecore.Abstractions;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Events;

namespace Sitecore.Support.ContentSearch.Azure.Events.RebuildEvents
{
    public static class EventRaiser
    {
        private static BaseEventQueueProvider _eventQueueProvider;

        private static BaseEventQueueProvider EventQueueProvider => _eventQueueProvider ?? (_eventQueueProvider =
            ServiceLocator.ServiceProvider.GetService(typeof(BaseEventQueueProvider)) as BaseEventQueueProvider);

        public static void RaiseRebuildEndEvent(SwitchOnRebuildEventRemote @event)
        {
            var parameters = new object[]
            {
                @event.IndexName,
                @event.SearchCloudIndexName,
                @event.RebuildCloudIndexName
            };
            Event.RaiseEvent("index:switchonrebuild", parameters);

            EventQueueProvider.QueueEvent(@event, true, false);
        }
    }
}