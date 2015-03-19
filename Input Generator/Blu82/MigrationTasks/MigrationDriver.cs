using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blu82.MigrationTasks
{
    public class MigrationDriver
    {
        private List<MigrationTask> _tasks = new List<MigrationTask>();

        public void Add(MigrationTask task)
        {
            _tasks.Add(task);
        }

        public string ToXML()
        {
            StringBuilder xml = new StringBuilder();

            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            xml.Append("<Migration xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n");
            xml.Append("\t<MigrationTaskList>\n");
            foreach (MigrationTask task in _tasks)
            {
                xml.Append(task.OutputXML());
            }
            xml.Append("\t</MigrationTaskList>\n");
            xml.Append("</Migration>\n");
            return xml.ToString();

        }

    }
}
