using System.Collections;
using System.Configuration;

namespace SolrWindowsService.Config
{
    public static class SolrServiceConfigurationManager
    {
        public static SolrServiceConfig GetSolrServiceConfiguration()
        {
            return SolrServiceConfig.FromSectionHashTable((Hashtable)ConfigurationManager.GetSection("solrService"));
        }
    }
}