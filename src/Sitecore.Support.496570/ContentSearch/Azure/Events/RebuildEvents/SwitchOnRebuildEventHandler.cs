using System;
using Sitecore.Abstractions;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.DependencyInjection;
using Sitecore.Pipelines;

namespace Sitecore.Support.ContentSearch.Azure.Events.RebuildEvents
{
    [UsedImplicitly]
    public class SwitchOnRebuildEventHandler : Sitecore.ContentSearch.Azure.Events.RebuildEvents.SwitchOnRebuildEventHandler
    {
        private BaseEventManager _eventManager;

        [UsedImplicitly]
        public override void InitializeFromPipeline(PipelineArgs args)
        {
            _eventManager = ServiceLocator.ServiceProvider.GetService(typeof(BaseEventManager)) as BaseEventManager;

            var action = new Action<SwitchOnRebuildEventRemote>(this.RaiseSwitchOnRebuildRemoteEvent);
            _eventManager?.Subscribe(action);
        }

        private void RaiseSwitchOnRebuildRemoteEvent(SwitchOnRebuildEventRemote @event)
        {
            Sitecore.Events.Event.RaiseEvent("index:switchonrebuild:remote", @event.IndexName, @event.SearchCloudIndexName, @event.RebuildCloudIndexName);
        }
    }
}