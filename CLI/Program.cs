using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MigrationEngine;

namespace Prototype
{
    class Program
    {
        void Doit()
        {
            Migrator engine = new Migrator();
            //engine.Save(@"data.xml");
            engine.Load(@"data.xml");
        }

        static void Main(string[] args)
        {
            Program migrate = new Program();
            migrate.Doit();
        }
    }
}
