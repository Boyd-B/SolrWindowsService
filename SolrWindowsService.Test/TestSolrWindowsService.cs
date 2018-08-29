using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using NUnit;
using NUnit.Framework;
using SolrWindowsService.Config;

namespace SolrWindowsService.Test
{
    [TestFixture]
    public class TestSolrWindowsService
    {
        [Test]
        public void ReadConfiguration()
        {
            var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
            Assert.IsNotNull(config);
            Assert.AreEqual("SolrCloud",config.ServiceName);
        }

        [Test]
        public void TestHashTableFormat()
        {
            Hashtable ht = new Hashtable();
            ht.Add(nameof(SolrServiceConfig.CommandLineArgs), "start -cloud -s {SolrServerDirectory} -m {MemorySize} -z {zkHost}");
            ht.Add("SolrServerDirectory", "C:\\solr\\solr-6.6.5\\server\\solr");
            ht.Add("MemorySize", "10g");
            ht.Add("zkHost", "10.16.98.45:2181");

            string expected = "start -cloud -s C:\\solr\\solr-6.6.5\\server\\solr -m 10g -z 10.16.98.45:2181";
            string actual = SolrServiceConfig.FormatCommandLineArgs(ht);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetCommandLineArguments()
        {
            string expected = "start -cloud -s C:\\solr\\solr-6.6.5\\server\\solr";
            var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
            Assert.IsNotNull(config);

            Assert.AreEqual(expected, config.CommandLineArgs);
        }

        [Test]
        public void GetDescriptionFormat()
        {
            string expected = "Executes 'C:\\solr\\solr-6.6.5\\bin\\solr' With: 'start -cloud -s C:\\solr\\solr-6.6.5\\server\\solr'";
            var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
            Console.WriteLine(config.Description);

            Assert.AreEqual(expected, config.Description);
        }

        [Test]
        public void TestProcessExecution()
        {
            var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
            using (Process process = new Process())
            {
                process.StartInfo.FileName = config.FileName;
                process.StartInfo.WorkingDirectory = config.WorkingDirectory;
                process.StartInfo.Arguments = config.CommandLineArgs;
                process.StartInfo.UseShellExecute = true;
                //process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

                Console.WriteLine(config.WorkingDirectory + config.FileName);
                var result = process.Start();

                Console.WriteLine(result.ToString());

                Assert.IsTrue(result);
            }

        }
    }
}
