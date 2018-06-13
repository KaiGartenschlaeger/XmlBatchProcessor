using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class CopyFilesAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.IncludeSubDirectories = node.ReadAttributeBool("IncludeSubDirectories");

            this.Pattern = node.ReadAttributeString("Pattern");
            this.Pattern = context.ReplaceWithValues(this.Pattern);

            this.SourcePath = node.ReadAttributeString("SourcePath");
            this.SourcePath = context.ReplaceWithValues(this.SourcePath);

            this.DestinationPath = node.ReadAttributeString("DestinationPath");
            this.DestinationPath = context.ReplaceWithValues(this.DestinationPath);
        }

        private void CopyDirectory(DirectoryInfo sourcePath, DirectoryInfo destinationPath)
        {

            if (!sourcePath.Exists)
            {
                throw new ProcessingException("Das Verzeichnis '" + sourcePath.FullName + "' konnte nicht gefunden werden.");
            }

            if (!destinationPath.Exists)
            {
                Console.WriteLine("Erstelle Verzeichnis '{0}'",
                    destinationPath.FullName);

                destinationPath.Create();
            }

            //
            // copy files
            //

            FileInfo[] files = sourcePath.GetFiles(
                string.IsNullOrEmpty(this.Pattern) ? "*.*" : this.Pattern,
                SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationPath.FullName, file.Name);
                file.CopyTo(tempPath, true);

                Console.WriteLine("Kopiere Datei '{0}' -> '{1}'",
                    file.FullName, tempPath);
            }

            //
            // copy sub directories
            //

            if (IncludeSubDirectories)
            {
                DirectoryInfo[] subDirectories = sourcePath.GetDirectories();
                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    string toPath = Path.Combine(destinationPath.FullName, subDirectory.Name);
                    CopyDirectory(subDirectory, new DirectoryInfo(toPath));
                }
            }
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            DirectoryInfo fromDirectory = new DirectoryInfo(this.SourcePath);
            if (!fromDirectory.Exists)
            {
                throw new ProcessingException("Das Quellverzeichnis '" + fromDirectory.FullName + "' konnte nicht gefunden werden.");
            }

            DirectoryInfo toDirectory = new DirectoryInfo(this.DestinationPath);

            CopyDirectory(fromDirectory, toDirectory);
        }

        #endregion

        #region Properties

        public bool IncludeSubDirectories { get; set; }
        public string Pattern { get; set; }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }

        #endregion
    }
}