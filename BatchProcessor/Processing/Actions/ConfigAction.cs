using System.Collections.Generic;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public abstract class ConfigAction
    {
        #region Constructor

        public ConfigAction()
        {
            this.Actions = new List<ConfigAction>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Wird aufgerufen, wenn das Xml-Element dieser Aktion eingelesen wird.
        /// </summary>
        public virtual void Parse(ProcessingContext context, XmlNode node)
        {
        }

        /// <summary>
        /// Wird ausgeführt, wenn die Aktion gestartet wird.
        /// </summary>
        public virtual void Run(ProcessingContext context, ConfigAction parentAction)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Legt fest, ob die Aktion ausgeführt wird oder nur als Container dient.
        /// </summary>
        public virtual bool Execute
        {
            get { return true; }
        }

        /// <summary>
        /// Legt fest, ob Unterelemente als einzelne Aktionen eingelesen werden.
        /// Falls nicht gesetzt, gelten Unterelemente als Eigenschaften.
        /// </summary>
        public virtual bool ParseChildNodes
        {
            get { return false; }
        }

        /// <summary>
        /// Legt den Prefix für Unter-Aktionen fest.
        /// Hat nur eine Wirkung für Unter-Aktionen und wird verwendet wenn ParseChildNodes gesetzt ist.
        /// </summary>
        public virtual string ChildActionsPrefix
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Enthält Unter-Aktionen die innerhalb dieser Aktion liegen.
        /// </summary>
        public List<ConfigAction> Actions { get; private set; }

        #endregion
    }
}