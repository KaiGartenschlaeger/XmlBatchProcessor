using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class CopyFileAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.SourcePath = node.ReadAttributeString("SourcePath");
            this.SourcePath = context.ReplaceWithValues(this.SourcePath);

            this.DestinationPath = node.ReadAttributeString("DestinationPath");
            this.DestinationPath = context.ReplaceWithValues(this.DestinationPath);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            FileInfo file = new FileInfo(this.SourcePath);
            if (file.Exists)
            {
                Console.WriteLine("Kopiere Datei '{0}' -> '{1}'",
                    file.FullName,
                    this.DestinationPath);

                file.CopyTo(this.DestinationPath, true);
            }
            else
            {
                throw new ProcessingException("Die Datei '" + this.SourcePath + "' konnte nicht gefunden werden.");
            }
        }

        #endregion

        #region Properties

        public string SourcePath { get; private set; }
        public string DestinationPath { get; private set; }

        #endregion
    }
}