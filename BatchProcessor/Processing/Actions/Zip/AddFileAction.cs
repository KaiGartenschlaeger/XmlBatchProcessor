using BatchProcessor.Helper;
using System.Xml;

namespace BatchProcessor.Processing.Actions.Zip
{
    public class AddFileAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.FilePath = node.ReadAttributeString("Path");
            this.FilePath = context.ReplaceWithValues(this.FilePath);
        }

        #endregion

        #region Properties

        public string FilePath { get; private set; }

        public override bool Execute
        {
            get { return false; }
        }

        #endregion
    }
}