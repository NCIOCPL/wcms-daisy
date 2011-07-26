using System;

namespace NCI.Web.CDE
{
    public class FileInstructionException : Exception
    {
        public FileInstructionException()
        {
        }

        public FileInstructionException(string message)
            : base(message)
        {
        }

        public FileInstructionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
