using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigrationEngine
{
    interface IMigrationLog
    {
        void BeginTask(string taskName);
        void EndTask();
        void IncrementProgress(string taskName, int itemIndex, int total, Guid itemMigrationID);

        void LogError(String taskName, String message, Guid itemMigrationID, Dictionary<string, string> Fields);
        void LogUnhandledException(String taskName, Exception ex);
    }
}
