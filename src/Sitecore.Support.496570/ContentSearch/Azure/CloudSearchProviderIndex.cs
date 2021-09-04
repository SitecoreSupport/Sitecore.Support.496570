using System;
using System.Reflection;
using System.Threading;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Azure;
using Sitecore.ContentSearch.Azure.Events.RebuildEvents;
using Sitecore.ContentSearch.Azure.Http;
using Sitecore.ContentSearch.Maintenance;

namespace Sitecore.Support.ContentSearch.Azure
{
    [UsedImplicitly]
    public class CloudSearchProviderIndex : Sitecore.ContentSearch.Azure.CloudSearchProviderIndex
    {
        private readonly bool _switchOnRebuild;
        private readonly TimeSpan _oldIndexCleanUpDelay;

        private static readonly PropertyInfo SearchCloudIndexNamePrePropertyInfo =
            typeof(Sitecore.ContentSearch.Azure.CloudSearchProviderIndex).GetProperty("SearchCloudIndexName",
                BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly PropertyInfo RebuildCloudIndexNamePrePropertyInfo =
            typeof(Sitecore.ContentSearch.Azure.CloudSearchProviderIndex).GetProperty("RebuildCloudIndexName",
                BindingFlags.Instance | BindingFlags.NonPublic);

        [Obsolete]
        public CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore) 
            : base(name, connectionStringName, totalParallelServices, propertyStore)
        {
            var settings = this.ServiceCollectionClient.GetInstance<Settings>();
            _switchOnRebuild = settings.SwitchOnRebuild;
            _oldIndexCleanUpDelay = settings.OldIndexCleanUpDelay;
        }

        [Obsolete]
        protected internal CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore, string @group, ServiceCollectionClient serviceCollectionClient)
            : base(name, connectionStringName, totalParallelServices, propertyStore, @group, serviceCollectionClient)
        {
            var settings = this.ServiceCollectionClient.GetInstance<Settings>();
            _switchOnRebuild = settings.SwitchOnRebuild;
            _oldIndexCleanUpDelay = settings.OldIndexCleanUpDelay;
        }

        public CloudSearchProviderIndex(string name, string connectionStringName, string totalParallelServices, IIndexPropertyStore propertyStore, ICloudSearchProviderIndexName cloudSearchProviderIndexName) 
            : base(name, connectionStringName, totalParallelServices, propertyStore, cloudSearchProviderIndexName)
        {
        }

        protected override void PerformRebuild(IndexingOptions indexingOptions, CancellationToken cancellationToken)
        {
            if (!this.ShouldStartIndexing(indexingOptions))
                return;

            if (_switchOnRebuild)
            {
                this.DoRebuild(indexingOptions);

                ISearchService searchServiceToDelete = this.SearchService;

                Sitecore.Support.ContentSearch.Azure.Events.RebuildEvents.EventRaiser.RaiseRebuildEndEvent(new SwitchOnRebuildEventRemote
                {
                    IndexName = this.Name,
                    SearchCloudIndexName = (string)SearchCloudIndexNamePrePropertyInfo.GetValue(this),
                    RebuildCloudIndexName = (string)RebuildCloudIndexNamePrePropertyInfo.GetValue(this)
                });

                Thread.Sleep(_oldIndexCleanUpDelay);

                searchServiceToDelete.Cleanup();
            }
            else
            {
                this.Reset();
                this.DoRebuild(indexingOptions);
            }
        }
    }
}