using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MigrationEngine;

namespace Daisy
{
    class XmlLogger : IMigrationLog
    {
        private Stack<String> _taskStack;
        StreamWriter _migrationOutput = null;
        StreamWriter _errorOutput = null;
        string _baseName;


        public XmlLogger(String baseName)
        {
            _taskStack = new Stack<string>();
            _baseName = baseName;
        }

        public void StartLog()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

            _migrationOutput = new StreamWriter(_baseName + "Migration-" + timestamp + ".xml");
            _migrationOutput.WriteLine("<log>");

            _errorOutput = new StreamWriter(_baseName + "Error-" + timestamp + ".log");
        }

        public void EndLog()
        {
            _errorOutput.Close();
            _errorOutput = null;

            _migrationOutput.WriteLine("</log>");
            _migrationOutput.Close();
            _migrationOutput = null;
        }

        #region IMigrationLog Members

        public void BeginTask(string taskName, int taskIndex, int taskTotal)
        {
            _taskStack.Push(taskName);
            Console.WriteLine("Starting task {0} of {1}: {2}", taskIndex, taskTotal, taskName);
            _migrationOutput.WriteLine("<task name=\"{0}\" pos=\"{1}\" count=\"{2}\">", taskName, taskIndex, taskTotal);
        }

        public void EndTask()
        {
            string taskName = _taskStack.Pop();
            Console.WriteLine("Ending task: {0}", taskName);
            _migrationOutput.WriteLine("</task>");
        }

        public void BeginTaskItem(string taskName, int itemIndex, int itemCount, Guid itemMigrationID, string path)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Console.WriteLine("{0}\tItem {1} of {2}. MigID: {3}, path: {4}",
                timestamp, itemIndex, itemCount, itemMigrationID, path);

            _migrationOutput.WriteLine("<item time=\"{0}\" pos=\"{1}\" count=\"{2}\">", timestamp, itemIndex, itemCount);
            _migrationOutput.WriteLine("<mig_id>{0}</mig_id>", itemMigrationID);
            _migrationOutput.WriteLine("<path>{0}</path>", path);
        }

        public void LogTaskItemWarning(string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
            }
            Console.WriteLine("WARNING: {0}, Fields: {1}", message, sb.ToString());

            _migrationOutput.WriteLine("<warning>");
            _migrationOutput.WriteLine("<message>{0}</message>", message);
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                _migrationOutput.WriteLine("<field name=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            _migrationOutput.WriteLine("</warning>");
        }

        public void LogTaskItemError(string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
            }
            Console.WriteLine("ERROR: {0}, Fields: {1}", message, sb.ToString());

            _migrationOutput.WriteLine("<error>");
            _migrationOutput.WriteLine("<message>{0}</message>", message);
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                _migrationOutput.WriteLine("<field name=\"{0}\" value=\"{1}\" />", kvp.Key, kvp.Value);
            }
            _migrationOutput.WriteLine("</error>");
        }

        public void EndTaskItem()
        {
            _migrationOutput.WriteLine("</item>");
        }

        public void LogError(string taskName, string message, Guid itemMigrationID, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("Field: {0}, Value {1}\n", kvp.Key, kvp.Value);
            }

            Console.WriteLine("ERROR in task {0}, item {1}, \"{2}\", {3}", taskName, itemMigrationID, message, sb.ToString());
        }

        public void LogWarning(String taskName, String message, Guid itemMigrationID, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("Field: {0}, Value {1}\n", kvp.Key, kvp.Value);
            }

            Console.WriteLine("WARNING in task {0}, item {1}, \"{2}\", {3}", taskName, itemMigrationID, message, sb.ToString());
        }


        public void LogUnhandledException(string taskName, Exception ex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
