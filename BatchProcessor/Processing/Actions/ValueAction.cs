using BatchProcessor.Helper;
using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace BatchProcessor.Processing.Actions
{
    public class ValueAction : ConfigAction
    {
        #region Helper

        private void ReplaceValueFields()
        {
            Regex regex = new Regex(@"\[([a-zA-Z]+):([^\].]+)\]");

            Match match;
            do
            {
                match = regex.Match(this.Value);
                if (match.Success)
                {
                    if (match.Groups.Count != 3)
                    {
                        throw new ProcessingException("Ungültige Formatierung in der Variable '" + this.Name + "'");
                    }

                    string fieldName = match.Groups[1].Value.Trim();
                    string fieldFormat = match.Groups[2].Value.Trim();

                    string replaceValue;

                    switch (fieldName)
                    {
                        case "DateTime":
                            replaceValue = DateTime.Now.ToString(fieldFormat);
                            break;

                        default:
                            throw new ProcessingException("Die Variable '" + this.Name + "' hat ein unbekanntes CustomField: '" + fieldName + "'");
                    }

                    this.Value = this.Value.Substring(0, match.Index) + replaceValue + this.Value.Substring(match.Index + match.Length);
                }

            } while (match.Success);
        }

        #endregion

        #region Methods

        public override void Parse(ProcessingContext context, XmlNode node)
        {
            this.Name = node.ReadAttributeString("Name");
            this.Value = node.ReadAttributeString("Value");
            this.Value = context.ReplaceWithValues(this.Value);

            ReplaceValueFields();

            if (!context.Values.ContainsKey(this.Name))
            {
                Console.WriteLine("Erstelle Variable '{0}' = '{1}'",
                    this.Name,
                    this.Value);

                context.Values.Add(this.Name, this.Value);
            }
            else
            {
                Console.WriteLine("Setzte Variable '{0}' = '{1}'",
                    this.Name,
                    this.Value);

                context.Values[this.Name] = this.Value;
            }
        }

        #endregion

        #region Properties

        public string Name { get; private set; }
        public string Value { get; private set; }

        public override bool Execute
        {
            get { return false; }
        }

        #endregion
    }
}