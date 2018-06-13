using BatchProcessor.Processing.Actions;
using System;
using System.Collections.Generic;
using System.IO;

namespace BatchProcessor.Processing
{
    public class ProcessingContext
    {
        #region Constructor

        public ProcessingContext()
        {
            this.ActionsTree = new List<ConfigAction>();
            this.Values = new Dictionary<string, string>();
            this.IteratableActions = new Dictionary<string, IActionIterator>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ändert den Wert einer Variable. 
        /// Falls die Variable nicht existiert, wird die Variable erstellt.
        /// </summary>
        public void SetValue(string name, string value)
        {
            if (this.Values.ContainsKey(name))
            {
                this.Values[name] = value;
            }
            else
            {
                this.Values.Add(name, value);
            }
        }

        /// <summary>
        /// Entfernt die Variable.
        /// </summary>
        public bool RemoveValue(string name)
        {
            return this.Values.Remove(name);
        }

        /// <summary>
        /// Ersetzt den Wert mit Variablen und System-Werten
        /// </summary>
        public string ReplaceWithValues(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            //
            // replace custom values
            //

            string result = value;
            foreach (var item in this.Values)
            {
                result = result.Replace("{" + item.Key + "}", item.Value);
            }

            //
            // replace system values
            //

            result = result.Replace("{ProjectPath}", Directory.GetCurrentDirectory());
            result = result.Replace("{SystemPath}", Environment.GetFolderPath(Environment.SpecialFolder.System));
            result = result.Replace("{WindowPath}", Environment.GetFolderPath(Environment.SpecialFolder.Windows));

            result = result.Replace("{UserName}", Environment.UserName);
            result = result.Replace("{MachineName}", Environment.MachineName);

            return result;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Enthält das Projekt-Verzeichnis
        /// </summary>
        public string ProjectDirectory { get; internal set; }

        /// <summary>
        /// Enthält den Pfad zur Konfigurations-Datei
        /// </summary>
        public string ConfigurationFile { get; internal set; }

        /// <summary>
        /// Enthält die Aktionen als Baum
        /// </summary>
        public List<ConfigAction> ActionsTree { get; private set; }

        /// <summary>
        /// Enthält iterierbare Aktionen
        /// </summary>
        public Dictionary<string, IActionIterator> IteratableActions { get; private set; }

        /// <summary>
        /// Enthält Platzhalter
        /// </summary>
        public Dictionary<string, string> Values { get; private set; }

        #endregion
    }
}