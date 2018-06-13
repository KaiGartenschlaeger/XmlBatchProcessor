using System;

namespace BatchProcessor.Processing
{
    public class ParseException : Exception
    {
        #region Constructor

        public ParseException(string message)
            : base(message)
        {
        }

        #endregion
    }
}