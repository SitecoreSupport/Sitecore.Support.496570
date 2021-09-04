using Sitecore.Configuration;
﻿using System;
﻿using Sitecore.Abstractions;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
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
            
            try
            {
                EventQueueProvider.GetEventQueue("master")?.QueueEvent(@event, true, false);
            }
            catch (Exception e)
            {
                Log.Error("Could not locate the 'master' EventQueue", e, typeof(EventRaiser));
            }
        }
    }
}