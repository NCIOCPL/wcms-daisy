using System;

namespace NCI.Web.CDE
{
    public class PageAssemblyException : Exception
    {
        public PageAssemblyException()
        {
        }

        public PageAssemblyException(string message)
            : base(message)
        {
        }

        public PageAssemblyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
