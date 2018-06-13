using BatchProcessor.Processing.Actions;
using System;
using System.IO;
using System.Xml;

namespace BatchProcessor.Processing
{
    public class ProcessingExecutioner
    {
        #region Helper

        /// <summary>
        /// Liest Xml-Elemente ein
        /// </summary>
        private void ParseElements(XmlNode rootNode, ProcessingContext context, ConfigAction parentAction)
        {
            foreach (XmlNode node in rootNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    ConfigAction action = null;
                    try
                    {
                        var prefix = string.Empty;
                        if (parentAction != null)
                        {
                            prefix = parentAction.ChildActionsPrefix;
                        }

                        var typeName = "BatchProcessor.Processing.Actions." + prefix + node.Name + "Action";
                        action = (ConfigAction)Activator.CreateInstance("BatchProcessor", typeName).Unwrap();
                    }
                    catch (Exception)
                    {
                        throw new ParseException("Der Element-Typ '" + node.Name + "' ist nicht bekannt.");
                    }

                    action.Parse(context, node);
                    if (action.ParseChildNodes)
                    {
                        ParseElements(node, context, action);
                    }

                    IActionIterator iterator = action as IActionIterator;
                    if (iterator != null)
                    {
                        context.IteratableActions.Add(iterator.IteratorName, iterator);
                    }

                    if (parentAction == null)
                    {
                        context.ActionsTree.Add(action);
                    }
                    else
                    {
                        parentAction.Actions.Add(action);
                    }
                }
            }
        }

        /// <summary>
        /// Liest die Konfigurations-Datei ein.
        /// </summary>
        private void ParseConfigurationFile()
        {
            //
            // parse xml file
            //

            XmlDocument doc = new XmlDocument();
            doc.Load(_context.ConfigurationFile);

            XmlNode nodeRoot = doc["Batch"];
            if (nodeRoot == null)
            {
                throw new ParseException("Das Root-Element 'Batch' konnte nicht gefunden werden.");
            }

            XmlAttribute projectDirectoryAttribute = nodeRoot.Attributes["Directory"];
            if (projectDirectoryAttribute != null)
            {
                if (Directory.Exists(projectDirectoryAttribute.InnerText))
                {
                    _context.ProjectDirectory = projectDirectoryAttribute.InnerText;
                }
                else
                {
                    throw new ProcessingException("Das Verzeichnis '" + projectDirectoryAttribute.InnerText + "' konnte nicht gefunden werden.");
                }
            }

            ParseElements(nodeRoot, _context, null);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Führt den Prozes aus
        /// </summary>
        public ProcessingResult Run(string configurationFilePath)
        {
            //
            // check paths
            //

            if (string.IsNullOrEmpty(configurationFilePath) || !File.Exists(configurationFilePath))
            {
                return ProcessingResult.ConfigurationFileNotFound;
            }

            _context = new ProcessingContext();
            _context.ConfigurationFile = configurationFilePath;
            _context.ProjectDirectory = Path.GetDirectoryName(configurationFilePath);

            //
            // parse config
            //

            Console.WriteLine("Lese Konfiguration '{0}'",
                _context.ConfigurationFile);

            ParseConfigurationFile();

            //
            // run
            //

            Directory.SetCurrentDirectory(_context.ProjectDirectory);

            foreach (ConfigAction action in _context.ActionsTree)
            {
                if (action.Execute)
                {
                    action.Run(_context, null);
                }
            }

            //
            // done
            //

            return ProcessingResult.OK;
        }

        #endregion

        #region Properties

        private ProcessingContext _context;

        #endregion
    }
}