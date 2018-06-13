using System.Collections.Generic;

namespace BatchProcessor.Processing.Actions
{
    public interface IActionIterator
    {
        string IteratorName { get; }

        IEnumerable<string> Iterate();
    }
}