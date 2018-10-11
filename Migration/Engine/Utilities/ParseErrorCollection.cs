using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationEngine.Utilities
{
    /// <summary>
    /// Manages a list of parsing errors.  Lightweight wrapper around List<String>.
    /// </summary>
    public class ParseErrorCollection
    {
        List<String> errors = new List<string>();

        /// <summary>
        /// Boolean value reporting whether the collection contains any error messages.
        /// </summary>
        public bool IsEmpty { get { return errors.Count == 0; } }

        /// <summary>
        /// Get the number of errors which have been reported.
        /// </summary>
        public int Count { get { return errors.Count; } }

        /// <summary>
        /// Remove all messages.
        /// </summary>
        public void Clear()
        {
            errors.Clear();
        }

        /// <summary>
        /// Add a message to the list of errors.
        /// </summary>
        /// <param name="message">The message to add</param>
        public void Add(string message)
        {
            errors.Add(message);
        }

        /// <summary>
        /// Combines the set errors into a single string, separate by newlines.
        /// </summary>
        /// <returns>A newline separated list of errors.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (String item in errors)
            {
                string separator = first ? String.Empty : "\n";
                sb.AppendFormat("{0}{1}", separator, item);
                first = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retrieve the full set of error messages.
        /// </summary>
        /// <returns>Array of strings. If there are no errors, an empty array is returned.</returns>
        public String[] ToArray()
        {
            return errors.ToArray();
        }
    }
}
