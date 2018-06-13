using BatchProcessor.Helper;
using System;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class DecrementAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Name = node.ReadAttributeString("Name");
            this.Number = node.ReadAttributeInteger("Number");
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            if (context.Values.ContainsKey(this.Name))
            {
                int value = Convert.ToInt32(context.Values[this.Name]);

                Console.WriteLine("Veringere die Variable '{0}' um '{1}'",
                    this.Name,
                    value);

                context.Values[this.Name] = (value - Number).ToString();
            }
            else
            {
                throw new ProcessingException("Die Variable '" + this.Name + "' konnte nicht gefunden werden.");
            }
        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public int Number { get; set; }

        #endregion
    }
}