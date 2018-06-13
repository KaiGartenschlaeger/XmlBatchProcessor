using System;

namespace BatchProcessor.Processing
{
    public class ProcessingException : Exception
    {
        #region Constructor

        public ProcessingException(string message)
            : base(message)
        {
        }

        #endregion
    }
}