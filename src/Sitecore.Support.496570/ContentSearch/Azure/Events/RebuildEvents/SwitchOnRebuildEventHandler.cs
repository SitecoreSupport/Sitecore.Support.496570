using System;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.Pipelines;

namespace Sitecore.Support.ContentSearch.Azure.Events.RebuildEvents
{
    [UsedImplicitly]
    public class SwitchOnRebuildEventHandler : Sitecore.ContentSearch.Azure.Events.RebuildEvents.SwitchOnRebuildEventHandler
    {
        [UsedImplicitly]
        public override void InitializeFromPipeline(PipelineArgs args)
        {
            var action = new Action<SwitchOnRebuildEventRemote>(this.RaiseSwitchOnRebuildRemoteEvent);
            Eventing.EventManager.Subscribe(action);
        }

        private void RaiseSwitchOnRebuildRemoteEvent(SwitchOnRebuildEventRemote @event)
        {
            Sitecore.Events.Event.RaiseEvent("index:switchonrebuild:remote", @event.IndexName, @event.SearchCloudIndexName, @event.RebuildCloudIndexName);
        }
    }
}