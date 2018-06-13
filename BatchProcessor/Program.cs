using BatchProcessor.Processing;
using System;

namespace BatchProcessor
{
    class Program
    {
        static int Main(string[] args)
        {
            ProcessingResult result;

            if (args.Length != 1)
            {
                Console.WriteLine("Syntax : BatchProcessor.exe <path to xml configuration>");

                result = ProcessingResult.InvalidArguments;
            }
            else
            {
                try
                {
                    var executioner = new ProcessingExecutioner();
                    result = executioner.Run(args[0]);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.ToString());
                    Console.ForegroundColor = ConsoleColor.Gray;

                    result = ProcessingResult.Exception;
                }
            }

#if DEBUG
            Console.WriteLine("done, press any key to exit..");
            Console.ReadKey(true);
#endif

            return (int)result;
        }
    }
}