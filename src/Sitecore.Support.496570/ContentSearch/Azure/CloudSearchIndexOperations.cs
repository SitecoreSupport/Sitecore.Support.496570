using Sitecore.ContentSearch;

namespace Sitecore.Support.ContentSearch.Azure
{
    public class CloudSearchIndexOperations : Sitecore.ContentSearch.Azure.CloudSearchIndexOperations
    {
        public CloudSearchIndexOperations(ISearchIndex index) : base(index)
        {
        }
    }
}