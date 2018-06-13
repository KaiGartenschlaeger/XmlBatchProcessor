using BatchProcessor.Helper;
using System;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class MessageAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Text = node.ReadAttributeString("Text");
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            string outputText = context.ReplaceWithValues(this.Text);
            Console.WriteLine(outputText);
        }

        #endregion

        #region Properties

        public string Text { get; set; }

        #endregion
    }
}