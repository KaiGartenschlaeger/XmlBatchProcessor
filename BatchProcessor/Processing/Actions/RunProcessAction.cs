using BatchProcessor.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class RunProcessAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.ProcessPath = node.ReadAttributeString("Path");
            this.ProcessPath = context.ReplaceWithValues(this.ProcessPath);

            this.ProcessArguments = node.ReadAttributeString("Arguments", false);
            this.ProcessArguments = context.ReplaceWithValues(this.ProcessArguments);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            Console.WriteLine("Starte '{0}' {1}",
                this.ProcessPath,
                this.ProcessArguments);

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = this.ProcessPath;
            process.StartInfo.Arguments = this.ProcessArguments;
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            process.Start();
        }

        #endregion

        #region Properties

        public string ProcessPath { get; private set; }
        public string ProcessArguments { get; private set; }

        #endregion
    }
}