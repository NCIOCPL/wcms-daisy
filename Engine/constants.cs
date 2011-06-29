using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine
{
    struct Constants
    {
        // Field names.
        public struct Fields
        {
            public const string MIGRATION_ID = "mig_id";
            public const string CONTENT_TYPE = "contenttype";
            public const string COMMUNITY_NAME = "community";
            public const string PATH = "folder";
            public const string PRETTY_URL = "pretty_url_name";
        }

        // Community Aliases
        public struct Aliases
        {
            public const string SITE = "site";
        }
    }
}
