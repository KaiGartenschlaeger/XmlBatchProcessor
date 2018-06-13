using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class DeleteFileAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.FilePath = node.ReadAttributeString("Path");
            this.FilePath = context.ReplaceWithValues(this.FilePath);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            FileInfo file = new FileInfo(this.FilePath);
            if (file.Exists)
            {
                Console.WriteLine("Lösche Datei '{0}'",
                    file.FullName);

                file.Delete();
            }
            else
            {
                Console.WriteLine("Die Datei '{0}' wurde nicht gefunden. Löschvorgang wurde übersprungen.",
                    file.FullName);
            }
        }

        #endregion

        #region Properties

        public string FilePath { get; private set; }

        #endregion
    }
}