using BatchProcessor.Helper;
using System;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class LoopAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.ValueName = node.ReadAttributeString("Value");

            this.From = node.ReadAttributeInteger("From");
            this.To = node.ReadAttributeInteger("To");

            this.Step = node.ReadAttributeInteger("Step", true);
            if (this.Step == 0)
            {
                throw new ParseException("Der Wert 'Step' muss eine positiven oder negativen Ganzzahl beinhalten.");
            }
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            if (context.Values.ContainsKey(this.ValueName))
            {
                throw new ProcessingException("Eine Variable mit dem Name '" + this.ValueName + "' existiert bereits.");
            }

            Console.WriteLine("Starte Iteration von '{0}' nach '{1}' mit einer Erhöhung von '{2}'",
                this.From, this.To, this.Step);

            int currentValue = this.From;
            while (true)
            {
                if ((Step < 0 && currentValue < this.To) || (Step > 0 && currentValue > this.To))
                {
                    break;
                }

                context.SetValue(this.ValueName, currentValue.ToString());
                foreach (ConfigAction action in this.Actions)
                {
                    if (action.Execute)
                    {
                        action.Run(context, action);
                    }
                }

                currentValue += Step;
            }

            context.RemoveValue(this.ValueName);
        }

        #endregion

        #region Properties

        public string ValueName { get; private set; }
        public int From { get; private set; }
        public int To { get; private set; }
        public int Step { get; private set; }

        public override bool ParseChildNodes
        {
            get { return true; }
        }

        #endregion
    }
}