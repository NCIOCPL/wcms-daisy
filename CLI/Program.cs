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
        void Doit(string scriptName)
        {
            if (File.Exists(scriptName))
            {

                Migrator engine = new Migrator();
                //engine.Save(@"data.xml");
                engine.Load(scriptName);
            }
            else
            {
                Console.WriteLine("File not found: {0}", scriptName);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Program migrate = new Program();
                migrate.Doit(args[0]);
            }
            else
            {
                Console.WriteLine("Syntax: Migrate <scriptname>");
            }
        }
    }
}
