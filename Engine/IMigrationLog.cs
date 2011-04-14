using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MigrationEngine.Descriptors;

namespace MigrationEngine
{
    public interface IMigrationLog
    {
        /// <summary>
        /// Call before starting a migration task.
        /// </summary>
        /// <param name="taskName">Descriptive name of the task.</param>
        /// <param name="taskIndex">Offset into the list of tasks.</param>
        /// <param name="taskTotal">Total number of tasks to be performed.</param>
        void BeginTask(string taskName, int taskIndex, int taskTotal);

        /// <summary>
        /// Call after completing a migration task.
        /// </summary>
        void EndTask();

        [Obsolete("Use BeginTaskItem(string taskName, int itemIndex, int total, MigrationData migItem, string path) instead.")]
        void BeginTaskItem(string taskName, int itemIndex, int total, Guid itemMigrationID, string path);
        [Obsolete("Use LogTaskItemWarning(MigrationData migItem, string message, Dictionary<string, string> Fields) instead.")]
        void LogTaskItemWarning(Guid migId, string message, Dictionary<string, string> Fields);
        [Obsolete("Use LogTaskItemError(MigrationData migItem, string message, Dictionary<string, string> Fields) instead.")]
        void LogTaskItemError(Guid migId, string message, Dictionary<string, string> Fields);

        /// <summary>
        /// Call before starting work on an individual task item.
        /// </summary>
        /// <param name="taskName">Name of the overall task the item belongs to.</param>
        /// <param name="itemIndex">Offset of the task item into the overall list.</param>
        /// <param name="itemCount">Total number of items in the task.</param>
        /// <param name="migItem">The business object representing the</param>
        /// <param name="path">Path where the item is stored. (May be null).</param>
        void BeginTaskItem(string taskName, int itemIndex, int itemCount, MigrationData migItem, string path);

        /// <summary>
        /// Records informational (non-warning/error) items in the migration log.
        /// </summary>
        /// <param name="message"></param>
        void LogTaskItemInfo(string message);

        /// <summary>
        /// Call to record a warning associated with a task item
        /// </summary>
        /// <param name="migItem">The business object representing the</param>
        /// <param name="message">The warning message</param>
        /// <param name="Fields">Any fields which were to be sent to the CMS.</param>
        void LogTaskItemWarning(MigrationData migItem, string message, Dictionary<string, string> Fields);

        /// <summary>
        /// Call to record a warning associated with a task item
        /// </summary>
        /// <param name="migItem">The business object representing the</param>
        /// <param name="message">The warning message</param>
        /// <param name="Fields">Any fields which were to be sent to the CMS.</param>
        void LogTaskItemError(MigrationData migItem, string message, Dictionary<string, string> Fields);

        /// <summary>
        /// Can after completing an individual task item.
        /// </summary>
        void EndTaskItem();

        void LogError(String taskName, String message, Guid itemMigrationID, Dictionary<string, string> Fields);
        void LogError(String taskName, String message, MigrationData migItem, Dictionary<string, string> Fields);
        void LogUnhandledException(String taskName, Exception ex);
    }
}
