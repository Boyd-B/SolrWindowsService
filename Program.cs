using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using SolrWindowsService.Config;
using Topshelf;
using Topshelf.HostConfigurators;

namespace SolrWindowsService
{
    static class Program
    {
        static void Main()
        {
            InstallService();
        }

        private static void InstallService()
        {
            var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
            HostFactory.Run(x =>
            {
                x.Service<SolrService>(s =>
                {
                    s.ConstructUsing(name => new SolrService());
                    s.WhenStarted(solr => solr.Start());
                    s.WhenStopped(solr => solr.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription(config.Description);
                x.SetDisplayName(config.DisplayName);
                x.SetServiceName(config.ServiceName);
                x.StartManually();
            });
        }
        
    }
}
