using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine.Descriptors
{
    public class FolderLinkDescription : MigrationData
    {
        public string Path { get; set; }
        public string ObjectMonikerName { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<Path value=""{0}""/><ObjectMonikerName value=""{1}""/>";

                return string.Format(fmt,
                    Path,
                    ObjectMonikerName);
            }
        }
    }
}
