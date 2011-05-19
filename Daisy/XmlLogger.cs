using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MigrationEngine;
using MigrationEngine.Descriptors;

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

            _migrationOutput = new StreamWriter(_baseName + timestamp + "-Migration.xml");
            _migrationOutput.WriteLine("<log>");

            _errorOutput = new StreamWriter(_baseName + timestamp + "-Error.xml");
            _errorOutput.WriteLine("<log>");
        }

        public void EndLog()
        {
            _errorOutput.WriteLine("</log>");
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
            _migrationOutput.WriteLine("<task name=\"{0}\" task_pos=\"{1}\" task_count=\"{2}\">", taskName, taskIndex, taskTotal);
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
                timestamp, itemIndex, itemCount, itemMigrationID, path ?? string.Empty);

            _migrationOutput.WriteLine("<item time=\"{0}\" item_pos=\"{1}\" item_count=\"{2}\">", timestamp, itemIndex, itemCount);
            _migrationOutput.WriteLine("<mig_id>{0}</mig_id>", itemMigrationID);
            _migrationOutput.WriteLine("<path>{0}</path>", path ?? string.Empty);
        }

        public void BeginTaskItem(string taskName, int itemIndex, int itemCount, MigrationData migItem, string path)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Console.WriteLine("{0}\tItem {1} of {2}. Mig item: {3}, path: {4}",
                timestamp, itemIndex, itemCount, migItem, path ?? string.Empty);

            _migrationOutput.WriteLine("<item time=\"{0}\" item_pos=\"{1}\" item_count=\"{2}\">", timestamp, itemIndex, itemCount);
            _migrationOutput.WriteLine("<mig_item>{0}</mig_item>", migItem.ToXmlString());
            _migrationOutput.WriteLine("<path>{0}</path>", path ?? string.Empty);
        }

        public void LogTaskItemInfo(string message)
        {
            Console.WriteLine("INFO: {0}", message);

            _migrationOutput.WriteLine("<information>{0}</information>", message);
        }
        

        public void LogTaskItemWarning(Guid migId, string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("{0}={{<![CDATA[{1}]]>}}\n", kvp.Key, kvp.Value);
                }
                Console.WriteLine("WARNING: {0}, Fields: {1}", message, sb.ToString());
            }

            _migrationOutput.WriteLine("<warning>");
            _migrationOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _migrationOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _migrationOutput.WriteLine("</warning>");
        }

        public void LogTaskItemWarning(MigrationData migItem, string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("{0}={{<![CDATA[{1}]]>}}\n", kvp.Key, kvp.Value);
                }
                Console.WriteLine("WARNING: {0}, Fields: {1}", message, sb.ToString());
            }

            _migrationOutput.WriteLine("<warning>");
            _migrationOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _migrationOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _migrationOutput.WriteLine("</warning>");
        }

        public void LogTaskItemError(Guid migId, string message, Dictionary<string, string> Fields)
        {
            // Write log to console.
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
                }
            }
            Console.WriteLine("ERROR: {0}, Fields: {1}", message, sb.ToString());

            // Write to Log File.
            _migrationOutput.WriteLine("<error>");
            _migrationOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _migrationOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _migrationOutput.WriteLine("</error>");

            // Write to error log
            LogError(_taskStack.Peek(), message, migId, Fields);
        }

        public void LogTaskItemError(MigrationData migItem, string message, Dictionary<string, string> Fields)
        {
            // Write log to console.
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
                }
            }
            Console.WriteLine("ERROR: {0}, Fields: {1}", message, sb.ToString());

            // Write to Log File.
            _migrationOutput.WriteLine("<error>");
            _migrationOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _migrationOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _migrationOutput.WriteLine("</error>");

            // Write to error log
            LogError(_taskStack.Peek(), message, migItem, Fields);
        }

        public void EndTaskItem()
        {
            _migrationOutput.WriteLine("</item>");
        }

        public void LogError(string taskName, string message, Guid itemMigrationID, Dictionary<string, string> Fields)
        {
            // Write to console
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("Field: {0}, Value {1}\n", kvp.Key, kvp.Value);
                }
            }
            Console.WriteLine("ERROR in task {0}, item {1}, \"{2}\", {3}", taskName, itemMigrationID, message, sb.ToString());

            // Write to error log.
            _errorOutput.WriteLine("<error task=\"{0}\" itemid=\"{1}\">", taskName, itemMigrationID);
            _errorOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _errorOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _errorOutput.WriteLine("</error>");
        }

        public void LogError(String taskName, String message, MigrationData migItem, Dictionary<string, string> Fields)
        {
            // Write to console
            StringBuilder sb = new StringBuilder();
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    sb.AppendFormat("Field: {0}, Value {1}\n", kvp.Key, kvp.Value);
                }
            }
            Console.WriteLine("ERROR in task {0}, item {1}, \"{2}\", {3}", taskName, migItem, message, sb.ToString());

            // Write to error log.
            _errorOutput.WriteLine("<error task=\"{0}\">", taskName);
            _errorOutput.WriteLine("<migobject>{0}</migobject>", migItem);
            _errorOutput.WriteLine("<message><![CDATA[{0}]]></message>", message);
            if (Fields != null)
            {
                foreach (KeyValuePair<string, string> kvp in Fields)
                {
                    _errorOutput.WriteLine("<field name=\"{0}\"><![CDATA[{1}]]></field>", kvp.Key, kvp.Value);
                }
            }
            _errorOutput.WriteLine("</error>");
        }


        public void LogUnhandledException(string taskName, Exception ex)
        {
            _errorOutput.WriteLine("<error task=\"{0}\"><![CDATA[{1}]]></error>", taskName, ex.ToString());
        }

        #endregion
    }
}
