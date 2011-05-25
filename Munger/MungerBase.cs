using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using Munger.Configuration;

namespace Munger
{
    abstract class MungerBase
    {
        protected CMSController CMSController;
        protected Logger MessageLog;

        // List of hosts which are considered aliases for the site being migrated;
        // Deliberately static so it won't go away between calls.
        // This object is not thread-safe.
        protected static HostSet HostAliases { get; private set; }

        protected string CanonicalHostName { get; private set; }

        public MungerBase(CMSController controller, Logger messageLog)
        {
            CMSController = controller;
            MessageLog = messageLog;

            // Load alias list only once.
            if (HostAliases == null)
            {
                MungerConfigurationSection config = (MungerConfigurationSection)ConfigurationManager.GetSection("MungerConfig");

                HostAliases = new HostSet();
                foreach (HostElement item in config.HostList)
                {
                    HostAliases.Add(item.Name);
                    if (item.IsCanonical || string.IsNullOrEmpty(CanonicalHostName))
                    {
                        CanonicalHostName = item.Name;
                    }
                }
            }
        }
    }
}
