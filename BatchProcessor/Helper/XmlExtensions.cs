using BatchProcessor.Processing;
using System;
using System.Xml;

namespace BatchProcessor.Helper
{
    public static class XmlExtensions
    {
        #region Methods

        public static string ReadAttributeString(this XmlNode node, string name)
        {
            return ReadAttributeString(node, name, true);
        }

        public static string ReadAttributeString(this XmlNode node, string name, bool isRequired)
        {
            string result = string.Empty;

            XmlAttribute attribute = node.Attributes[name];
            if (attribute == null)
            {
                if (isRequired)
                {
                    throw new ParseException("Das Element '" + node.Name + "' benötigt das Attribute '" + name + "'");
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
            {
                result = attribute.InnerText.Trim();
            }

            return result;
        }

        public static bool ReadAttributeBool(this XmlNode node, string name)
        {
            return ReadAttributeBool(node, name, true);
        }

        public static bool ReadAttributeBool(this XmlNode node, string name, bool isRequired)
        {
            bool result = false;

            string value = ReadAttributeString(node, name, isRequired);
            if (!string.IsNullOrEmpty(value))
            {
                result = Convert.ToBoolean(value);
            }

            return result;
        }

        public static int ReadAttributeInteger(this XmlNode node, string name)
        {
            return ReadAttributeInteger(node, name, true);
        }

        public static int ReadAttributeInteger(this XmlNode node, string name, bool isRequired)
        {
            int result = 0;

            string value = ReadAttributeString(node, name, isRequired);
            if (!string.IsNullOrEmpty(value))
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        #endregion
    }
}