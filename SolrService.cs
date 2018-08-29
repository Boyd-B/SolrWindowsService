using System;
using System.Diagnostics;
using SolrWindowsService.Config;

namespace SolrWindowsService
{
    internal class SolrService
    {
        static readonly Process process = new Process();

        public void Start()
        {
            Log("Starting SolrCloud Service");
            try
            {
                var config = SolrServiceConfigurationManager.GetSolrServiceConfiguration();
                process.StartInfo.FileName = config.FileName;
                process.StartInfo.WorkingDirectory = config.WorkingDirectory;
                process.StartInfo.Arguments = config.CommandLineArgs;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                Log(process.ToString());
                var result = process.Start();
                Log("result of batch start: " + result);
                //process.WaitForExit();
            }
            catch (Exception ex)
            {
                Log("An error occurred: " + ex.Message);
                throw;
            }

        }

        public void Stop()
        {
            Log("Stopping service");
            try
            {
                process.Kill();
                process.Dispose();
            }
            catch (Exception ex)
            {
                Log("An error occurred: " + ex.Message);
            }
        }

        protected void Log(string message)
        {
            const string source = "SolrService";
            const string log = "Application";

            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            
            EventLog.WriteEntry(source, message);
        }
    }
}
