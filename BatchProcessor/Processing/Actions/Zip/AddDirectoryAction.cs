using BatchProcessor.Helper;
using System.Xml;

namespace BatchProcessor.Processing.Actions.Zip
{
    public class AddDirectoryAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.SourcePath = node.ReadAttributeString("SourcePath");
            this.SourcePath = context.ReplaceWithValues(this.SourcePath);
        }

        #endregion

        #region Properties

        public string SourcePath { get; private set; }

        public override bool Execute
        {
            get { return false; }
        }

        #endregion
    }
}