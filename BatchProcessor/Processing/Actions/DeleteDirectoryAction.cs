using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class DeleteDirectoryAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.DirectoryPath = node.ReadAttributeString("Path");
            this.DirectoryPath = context.ReplaceWithValues(this.DirectoryPath);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            DirectoryInfo dir = new DirectoryInfo(this.DirectoryPath);
            if (dir.Exists)
            {
                Console.WriteLine("Lösche Verzeichnis '{0}'",
                    this.DirectoryPath);

                dir.Delete(true);
            }
            else
            {
                Console.WriteLine("Das Verzeichnis '{0}' wurde nicht gefunden. Löschen wurde übersprungen.",
                    dir.FullName);
            }
        }

        #endregion

        #region Properties

        public string DirectoryPath { get; private set; }

        #endregion
    }
}