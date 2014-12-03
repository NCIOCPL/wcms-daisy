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
        protected HostSet HostAliases { get; private set; }

        protected string CanonicalHostName { get; private set; }

        public MungerBase(CMSController controller, Logger messageLog)
        {
            CMSController = controller;
            MessageLog = messageLog;

            MungerConfiguration config = (MungerConfiguration)ConfigurationManager.GetSection("MungerConfig");

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
