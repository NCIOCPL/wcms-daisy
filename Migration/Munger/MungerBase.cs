using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using Munger.Configuration;

namespace Munger
{
    abstract class MungerBase
    {
        protected ICMSController CMSController;
        protected ILogger MessageLog;

        // List of hosts which are considered aliases for the site being migrated;
        protected HostSet HostAliases { get; private set; }

        protected string CanonicalHostName { get; private set; }

        public MungerBase(ICMSController controller, ILogger messageLog, IMungerConfiguration config)
        {
            CMSController = controller;
            MessageLog = messageLog;

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
