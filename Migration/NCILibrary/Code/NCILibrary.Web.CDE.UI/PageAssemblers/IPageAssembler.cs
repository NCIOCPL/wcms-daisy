using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI
{
    /// <summary>
    /// Interface to define core funtionality for the assembler.
    /// </summary>
    public interface IPageAssembler
    {
        /// <summary>
        /// Gets the collection of Template Slots used by this page assembler.
        /// </summary>
        TemplateSlotCollection TemplateSlots { get; }
    }
}
