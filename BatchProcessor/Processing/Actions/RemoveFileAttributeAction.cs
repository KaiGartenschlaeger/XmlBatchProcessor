using BatchProcessor.Helper;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class RemoveFileAttributeAction : ConfigAction
    {
        #region Helper

        private void RemoveAttribute(string filePath, FileAttributes attributeToRemove)
        {
            FileAttributes attributes = File.GetAttributes(filePath);
            if ((attributes & attributeToRemove) == attributeToRemove)
            {
                Console.WriteLine("Entferne Attribute '{0}' von Datei '{1}'",
                    attributeToRemove, filePath);

                File.SetAttributes(filePath, attributes & ~attributeToRemove);
            }
        }

        #endregion

        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Path = node.ReadAttributeString("Path", true);

            string attributeValue = node.ReadAttributeString("Attribute", true);

            FileAttributes attribute;
            if (Enum.TryParse(attributeValue, out attribute))
            {
                this.Attribute = attribute;
            }
            else
            {
                throw new ParseException(string.Format("Das Datei-Attribute '{0}' ist ungültig.",
                    this.Attribute));
            }
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            string fullpath = context.ReplaceWithValues(this.Path);

            FileInfo fi = new FileInfo(fullpath);
            if (fi.Exists)
            {
                RemoveAttribute(fullpath, this.Attribute);
            }
            else
            {
                Console.WriteLine("Die Datei '{0}' wurde nicht gefunden.",
                    fi.FullName);
            }
        }

        #endregion

        #region Properties

        public string Path { get; set; }
        public FileAttributes Attribute { get; set; }

        #endregion
    }
}