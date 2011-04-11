using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine
{
    public interface IMigrationLog
    {
        void BeginTask(string taskName, int taskIndex, int taskTotal);
        void EndTask();

        void BeginTaskItem(string taskName, int itemIndex, int total, Guid itemMigrationID, string path);
        void LogTaskItemWarning(string message, Dictionary<string, string> Fields);
        void LogTaskItemError(string message, Dictionary<string, string> Fields);
        void EndTaskItem();

        void LogError(String taskName, String message, Guid itemMigrationID, Dictionary<string, string> Fields);
        void LogWarning(String taskName, String message, Guid itemMigrationID, Dictionary<string, string> Fields);
        void LogUnhandledException(String taskName, Exception ex);
    }
}
