using Sitecore.Configuration;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.Data;
using Sitecore.Events;

namespace Sitecore.Support.ContentSearch.Azure.Events.RebuildEvents
{
    public static class EventRaiser
    {
        public static void RaiseRebuildEndEvent(SwitchOnRebuildEventRemote @event)
        {
            var parameters = new object[]
            {
                @event.IndexName,
                @event.SearchCloudIndexName,
                @event.RebuildCloudIndexName
            };
            Event.RaiseEvent("index:switchonrebuild", parameters);

            Database web = Factory.GetDatabase("web");
            web?.RemoteEvents.EventQueue.QueueEvent(@event, true, false);
        }
    }
}