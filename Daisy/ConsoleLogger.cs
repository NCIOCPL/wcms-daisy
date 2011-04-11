using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine;

namespace Daisy
{
    class ConsoleLogger : IMigrationLog
    {
        private Stack<String> _taskStack;

        public ConsoleLogger()
        {
            _taskStack = new Stack<string>();
        }

        #region IMigrationLog Members

        public void BeginTask(string taskName, int taskIndex, int taskTotal)
        {
            _taskStack.Push(taskName);
            Console.WriteLine("Starting task {0} of {1}: {2}", taskIndex, taskTotal, taskName);
        }

        public void EndTask()
        {
            string taskName = _taskStack.Pop();
            Console.WriteLine("Ending task: {0}", taskName);
        }

        public void BeginTaskItem(string taskName, int itemIndex, int itemCount, Guid itemMigrationID, string path)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Console.WriteLine("{0}\tItem {1} of {2}. MigID: {3}, path: {4}",
                timestamp, itemIndex, itemCount, itemMigrationID, path);
        }

        public void LogTaskItemWarning(string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
            }

            Console.WriteLine("WARNING: {0}, Fields: {1}", message, sb.ToString());
        }

        public void LogTaskItemError(string message, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in Fields)
            {
                sb.AppendFormat("{0}={{{1}}}\n", kvp.Key, kvp.Value);
            }

            Console.WriteLine("ERROR: {0}, Fields: {1}", message, sb.ToString());
        }

        public void EndTaskItem()
        {
        }

        public void LogError(string taskName, string message, Guid itemMigrationID, Dictionary<string, string> Fields)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string,string> kvp in Fields)
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
