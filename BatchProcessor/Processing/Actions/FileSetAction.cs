using BatchProcessor.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class FileSetAction : ConfigAction, IActionIterator
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Name = node.ReadAttributeString("Name");
            this.SourcePath = node.ReadAttributeString("Path");

            this.IncludeSubDirectories = node.ReadAttributeBool("IncludeSubDirectories", false);

            this.Pattern = node.ReadAttributeString("Pattern", false);
            if (string.IsNullOrEmpty(this.Pattern))
            {
                this.Pattern = "*.*";
            }
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            DirectoryInfo dir = new DirectoryInfo(this.SourcePath);
            if (dir.Exists)
            {
                Console.WriteLine(@"Erstelle FileSet '{0}' für den Pfad '{1}\{2}'..",
                    this.Name,
                    this.SourcePath,
                    this.Pattern);

                this.Files = dir.GetFiles(this.Pattern,
                    this.IncludeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            else
            {
                throw new ProcessingException("Das Verzeichnis '" + dir.FullName + "' konnte nicht gefunden werden.");
            }
        }

        public IEnumerable<string> Iterate()
        {
            foreach (FileInfo file in this.Files)
            {
                yield return file.FullName;
            }
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string SourcePath { get; private set; }
        public string Pattern { get; private set; }
        public bool IncludeSubDirectories { get; private set; }

        public string IteratorName
        {
            get { return this.Name; }
        }

        public FileInfo[] Files { get; private set; }

        #endregion
    }
}