using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SolrWindowsService.Config
{
    static class HashtableExtension
    {
        public static T GetValueOrDefault<T>(this Hashtable ht, string key, T defaultValue)
        {
            if (!ht.ContainsKey(key)) return defaultValue;
            return (T)Convert.ChangeType(ht[key], typeof(T));
        }
    }
    public class SolrServiceConfig
    {
        public static SolrServiceConfig FromSectionHashTable(Hashtable ht)
        {
            FormatProperties(ht);
            return new SolrServiceConfig
            {
                ServiceName = ht.GetValueOrDefault(nameof(ServiceName), ""),
                DisplayName = ht.GetValueOrDefault(nameof(DisplayName), ""),
                WorkingDirectory = ht.GetValueOrDefault(nameof(WorkingDirectory), ""),
                FileName = ht.GetValueOrDefault(nameof(FileName), ""),

                CommandLineArgs = ht.GetValueOrDefault(nameof(CommandLineArgs), ""),
                Description = ht.GetValueOrDefault(nameof(Description), ""),
            };
        }

        private static void FormatProperties(Hashtable ht)
        {
            // Format command line args first so it can be used in description
            FormatCommandLineArgs(ht);

            // Format description after command line args
            FormatDescription(ht);
        }

        #region Service Properties

        public string ServiceName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region Execution Properties

        public string FileName { get; set; }
        public string WorkingDirectory { get; set; }
        public string CommandLineArgs { get; set; }

        #endregion

        public static string FormatDescription(Hashtable ht)
        {
            string description = ht.GetValueOrDefault(nameof(Description), "");
            
            foreach (var htKey in ht.Keys)
            {
                description = description.Replace("{" + htKey + "}", ht[htKey].ToString());
            }

            ht[nameof(Description)] = description;

            return description;
        }

        public static string FormatCommandLineArgs(Hashtable ht)
        {
            string args = ht.GetValueOrDefault(nameof(CommandLineArgs), "");

            foreach (var htKey in ht.Keys)
            {
                args = args.Replace("{" + htKey + "}", ht[htKey].ToString());
            }

            ht[nameof(CommandLineArgs)] = args;

            return args;
        }
    }
}
