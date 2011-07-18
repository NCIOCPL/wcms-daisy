using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines an interface for File Instructions which represent the meta data for files published from percussion.
    /// </summary>
    public interface IFileInstruction
    {
        /// <summary>
        /// The path of all parent folders of the file instruction.        
        /// </summary>
        string SectionPath { get; }

        /// <summary>
        /// Gets or sets the language for the file displayed.
        /// </summary>
        /// <value>The language.</value>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        /// <value>The path of the file.</value>        
        string FilePath { get; set; }
    }
}
