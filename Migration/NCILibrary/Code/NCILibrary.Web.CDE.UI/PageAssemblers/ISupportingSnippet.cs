using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// This interface is used by snippet template control to tell the 
    /// web assembler that addtional snippet controls are needed to support 
    /// the functionality. The snippet controls are created dynamically by the 
    /// snippet control implementing this interface.
    /// </summary>
    public interface ISupportingSnippet
    {
        /// <summary>
        /// Returns the snippet control for the given slot name.
        /// </summary>
        /// <returns>Returns a collection of snippet controls</returns>
        SnippetControl[] GetSupportingSnippets();
    }
}
