using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Base class for all the code doc members
    /// </summary>
    public class CodeDocMember
    {
        /// <summary>
        /// Selects a single node.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string SelectSingleNode(XmlNode xmlNode, string path)
        {
            if (xmlNode == null) throw new ArgumentNullException(nameof(xmlNode));
            return xmlNode.SelectSingleNode(path)?.InnerText ?? throw new Exception($"Failed to find path {path}");
        }

        /// <summary>
        /// Retuirns the xml elements.
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected IEnumerable<XmlElement> SelectXmlElements(XmlNode xmlNode, string path)
        {
            var nodes = xmlNode.SelectNodes(path) ?? throw new Exception("Failed to find path {path}");
            return nodes.Cast<XmlNode>().OfType<XmlElement>();
        }

        /// <summary>
        /// Returns the class name for supplied comment name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetClassName(string nameAttr)
        {
            var name = nameAttr ?? "";
            if (name.StartsWith("T:"))
            {
                return name.Substring(2);
            }
            if (name.StartsWith("M:"))
            {
                name = name.Substring(2);
                var paramIdx = name.IndexOf('(');
                if (paramIdx > -1)
                {
                    name = name.Substring(0, paramIdx);
                }
                var lastDot = name.LastIndexOf('.');
                if (lastDot == -1) throw new Exception("Cannot handle method name:" + name);
                return name.Substring(0, lastDot);
            }
            if (name.StartsWith("P:"))
            {
                name = name.Substring(2);
                var lastDot = name.LastIndexOf('.');
                if (lastDot == -1) throw new Exception("Cannot handle property name:" + name);
                return name.Substring(0, lastDot);
            }
            if (name.StartsWith("E:"))
            {
                name = name.Substring(2);
                var lastDot = name.LastIndexOf('.');
                if (lastDot == -1) throw new Exception("Cannot handle event name:" + name);
                return name.Substring(0, lastDot);
            }
            if (name.StartsWith("F:"))
            {
                name = name.Substring(2);
                var lastDot = name.LastIndexOf('.');
                if (lastDot == -1) throw new Exception("Cannot handle property name:" + name);
                return name.Substring(0, lastDot);
            }
            throw new Exception("Cannot handle name:" + name);
        }

        /// <summary>
        /// Return the method name
        /// </summary>
        /// <param name="nameAttr"></param>
        /// <returns></returns>
        protected string GetMethodName(string nameAttr)
        {
            var name = nameAttr ?? "";
            if(!name.StartsWith("M:"))
            {
                return null;
            }
            name = name.Substring(2);
            var paramIdx = name.IndexOf('(');
            if (paramIdx > -1)
            {
                name = name.Substring(0, paramIdx);
            }
            var lastDot = name.LastIndexOf('.');
            if (lastDot == -1) throw new Exception("Cannot handle method name:" + name);
            return name.Substring(lastDot+1);
        }

        /// <summary>
        /// Returns the property name
        /// </summary>
        /// <param name="nameAttr"></param>
        /// <returns></returns>
        protected string GetPropertyName(string nameAttr)
        {
            var name = nameAttr ?? "";
            if (!name.StartsWith("P:"))
            {
                return null;
            }
            name = name.Substring(2);
            var lastDot = name.LastIndexOf('.');
            if (lastDot == -1) throw new Exception("Cannot handle property name:" + name);
            return name.Substring(lastDot+1);
        }
    }
}