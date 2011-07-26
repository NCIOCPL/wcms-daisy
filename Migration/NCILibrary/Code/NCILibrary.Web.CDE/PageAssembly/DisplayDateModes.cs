using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines the Date Display Modes for a piece of content
    /// </summary>
    [Flags]
    public enum DateDisplayModes
    {
        /// <summary>
        /// Do not display any dates
        /// </summary>
        None = 0,
        /// <summary>
        /// Display the posted date
        /// </summary>
        Posted = 1,
        /// <summary>
        /// Display the updated date
        /// </summary>
        Updated = Posted << 1,
        /// <summary>
        /// Display the reviewed date
        /// </summary>
        Reviewed = Updated << 1,
        /// <summary>
        /// Display the posted and updated dates
        /// </summary>
        PostedUpdated = Posted | Updated,
        /// <summary>
        /// Display the posted and reviewed dates
        /// </summary>
        PostedReviewed = Posted | Reviewed,
        /// <summary>
        /// Display the updated and reviewed dates
        /// </summary>
        UpdatedReviewed = Updated | Reviewed,
        /// <summary>
        /// Display all the dates
        /// </summary>
        All = Posted | Updated | Reviewed
    }

}
