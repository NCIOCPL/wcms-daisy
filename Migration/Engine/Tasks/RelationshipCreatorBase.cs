﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MigrationEngine.Tasks
{
    public abstract class RelationshipCreatorBase : MigrationTask
    {
        public abstract override void Doit(IMigrationLog logger);
    }
}
