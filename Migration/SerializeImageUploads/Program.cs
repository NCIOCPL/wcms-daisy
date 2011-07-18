using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SerializeImageUploads
{
    class Program
    {
        private void Run(string inputFile, string outputFile)
        {
            ImageInputs inputset = null;

            XmlSerializer serializer = new XmlSerializer(typeof(ImageInputs));
            using (TextReader reader = new StreamReader(inputFile))
            {
                inputset = (ImageInputs)serializer.Deserialize(reader);
            }

            inputset.Process(outputFile);
        }

        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Program prog = new Program();
                prog.Run(args[0],args[1]);
            }
            else
            {
                Console.WriteLine("Syntax: program inputfilename.xml outputfilename.xml");
            }
        }
    }
}
