using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class CreateDirectoryAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Path = node.ReadAttributeString("Path");
            this.Path = context.ReplaceWithValues(this.Path);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            DirectoryInfo dir = new DirectoryInfo(this.Path);
            if (!dir.Exists)
            {
                Console.WriteLine("Erstelle Verzeichnis '{0}'",
                    dir.FullName);

                dir.Create();
            }
        }

        #endregion

        #region Properties

        public string Path { get; set; }

        #endregion
    }
}