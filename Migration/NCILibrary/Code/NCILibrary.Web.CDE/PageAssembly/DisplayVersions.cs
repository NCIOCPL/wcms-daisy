using System;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Display Version is used to load the correct template based on the request URL i.e print,web etc.
    /// </summary>    
    public enum DisplayVersions
    {
        /// <summary>
        /// This is the standard Display Version which is for the normal web site
        /// and is usually used as a default display verision.
        /// </summary>
        Web = 1,
        /// <summary>
        /// This is for the "print preview" version.
        /// </summary>
        Print = 2,
        /// <summary>
        /// This is for viewing all pages of a IMultiPageAssemblyInstruction.
        /// </summary>
        ViewAll = 3,
        /// <summary>
        /// This is for viewing all pages of a IMultiPageAssemblyInstruction in the "print preview" version.
        /// </summary>
        PrintAll = 4,

        Text=5,

        Image=6
    }
}
