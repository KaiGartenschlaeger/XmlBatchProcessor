using BatchProcessor.Helper;
using BatchProcessor.Processing;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class ActionListAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Name = node.ReadAttributeString("Name");
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public override bool Execute
        {
            get { return false; }
        }

        public override bool ParseChildNodes
        {
            get { return true; }
        }

        #endregion
    }
}