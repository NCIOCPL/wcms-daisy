using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine;

namespace Prototype
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

        public void IncrementTaskProgress(string taskName, int itemIndex, int itemCount, Guid itemMigrationID, string path)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            Console.WriteLine("{0}\tItem {1} of {2}. MigID: {3}, path: {4}",
                timestamp, itemIndex, itemCount, itemMigrationID, path);
        }

        public void LogError(string taskName, string message, Guid itemMigrationID, Dictionary<string, string> Fields)
        {
            throw new NotImplementedException();
        }

        public void LogUnhandledException(string taskName, Exception ex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
