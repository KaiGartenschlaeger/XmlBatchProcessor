using BatchProcessor.Helper;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class ForEachAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.VariableName = node.ReadAttributeString("Value");
            this.In = node.ReadAttributeString("In");
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            if (!context.IteratableActions.ContainsKey(this.In))
            {
                throw new ProcessingException("Die Aktion '" + this.In + "' konnte nicht gefunden werden.");
            }

            if (context.Values.ContainsKey(this.VariableName))
            {
                throw new ProcessingException("Eine Variable mit dem Name '" + this.VariableName + "' ist bereits vorhanden.");
            }

            foreach (string value in context.IteratableActions[this.In].Iterate())
            {
                context.SetValue(this.VariableName, value);

                foreach (ConfigAction action in this.Actions)
                {
                    if (action.Execute)
                    {
                        action.Run(context, action);
                    }
                }
            }

            context.RemoveValue(this.VariableName);
        }

        #endregion

        #region Properties

        public string VariableName { get; private set; }
        public string In { get; private set; }


        public override bool ParseChildNodes
        {
            get { return true; }
        }

        #endregion
    }
}