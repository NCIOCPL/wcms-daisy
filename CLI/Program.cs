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
            Migrator migrator = new Migrator();

            XmlSerializer serializer = new XmlSerializer(typeof(Migrator));
            TextWriter writer = new StreamWriter(@"C:\WCMTeam\Tools\MigrationTools\Prototype\data.xml");

            serializer.Serialize(writer, migrator);
        }

        static void Main(string[] args)
        {
            Program migrate = new Program();
            migrate.Doit();
        }
    }
}
