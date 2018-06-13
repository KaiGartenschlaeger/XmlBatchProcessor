using BatchProcessor.Helper;
using BatchProcessor.Processing.Actions.Zip;
using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class CreateZIPAction : ConfigAction
    {
        #region Helper

        public void AddFile(string sourcePath, string name, string subFolder)
        {
            Console.WriteLine("Füge Datei '{0}' hinzu..",
                sourcePath);

            m_zip.CreateEntryFromFile(sourcePath, subFolder + name, CompressionLevel.Optimal);
        }

        private void AddDirectory(string path, string subFolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                if (!string.IsNullOrEmpty(subFolderName))
                {
                    subFolderName += "/";
                    m_zip.CreateEntry(subFolderName);
                }

                // add files

                foreach (FileInfo file in dir.GetFiles("*.*", SearchOption.TopDirectoryOnly))
                {
                    AddFile(file.FullName, file.Name, subFolderName);
                }

                // add sub directories

                foreach (DirectoryInfo subDirectory in dir.GetDirectories())
                {
                    AddDirectory(Path.Combine(path, subDirectory.Name), subDirectory.Name);
                }
            }
            else
            {
                Console.WriteLine("Der Ordner '{0}' konnte nicht gefunden werden.",
                    dir.FullName);
            }
        }

        #endregion

        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.ArchivePath = node.ReadAttributeString("Path");
            this.ArchivePath = context.ReplaceWithValues(this.ArchivePath);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            Console.WriteLine("Erstelle Zip-Archiv '{0}'",
                this.ArchivePath);

            using (Stream stream = File.Create(this.ArchivePath))
            {
                using (m_zip = new ZipArchive(stream, ZipArchiveMode.Create))
                {
                    foreach (ConfigAction action in this.Actions)
                    {
                        string actionName = action.GetType().Name;
                        switch (actionName)
                        {
                            case "AddDirectoryAction":
                                AddDirectoryAction addDirectoryAction = (AddDirectoryAction)action;
                                AddDirectory(addDirectoryAction.SourcePath, null);
                                break;
                            case "AddFileAction":
                                AddFileAction addFileAction = (AddFileAction)action;
                                AddFile(addFileAction.FilePath, Path.GetFileName(addFileAction.FilePath), null);
                                break;

                            default:
                                throw new ProcessingException("Die Unteraktion '" + actionName + "' ist für CreateZip ungültig.");
                        }
                    }
                }
            }
        }

        #endregion

        #region Properties

        private ZipArchive m_zip = null;

        public string ArchivePath { get; private set; }



        public override bool ParseChildNodes
        {
            get { return true; }
        }

        public override string ChildActionsPrefix
        {
            get { return "Zip."; }
        }

        #endregion
    }
}