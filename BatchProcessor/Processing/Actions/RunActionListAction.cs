using BatchProcessor.Helper;
using System;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class RunActionListAction : ConfigAction
    {
        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.ActionListName = node.ReadAttributeString("Name");
            this.ActionListName = context.ReplaceWithValues(this.ActionListName);
        }

        public override void Run(ProcessingContext context, ConfigAction parentAction)
        {
            ConfigAction actionList = context.ActionsTree.Find(a =>
            {
                ActionListAction list = a as ActionListAction;
                if (list != null && list.Name == this.ActionListName)
                {
                    return true;
                }

                return false;
            });

            if (actionList == null)
            {
                throw new ProcessingException("Die Aktions-Liste '" + this.ActionListName + "' konnte nicht gefunden werden.");
            }
            else
            {
                Console.WriteLine("Starte Aktions-Liste '{0}'",
                    this.ActionListName);

                foreach (ConfigAction action in actionList.Actions)
                {
                    action.Run(context, this);
                }
            }
        }

        #endregion

        #region Properties

        public string ActionListName { get; set; }

        #endregion
    }
}