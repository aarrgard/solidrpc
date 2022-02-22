using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SolidRpc.OpenApi.Model.CodeDoc.Impl
{
    /// <summary>
    /// Creates a wrapper for supplied documentation
    /// </summary>
    public class CodeDocAssembly : CodeDocMember, ICodeDocAssembly
    {
        private static readonly ConcurrentDictionary<Type, Action<object, string>> s_SummarySetters = new ConcurrentDictionary<Type, Action<object, string>>();
        private static readonly ConcurrentDictionary<Type, Action<object, ICodeDocParameter>> s_ParamDocSetters = new ConcurrentDictionary<Type, Action<object, ICodeDocParameter>>();
        private static readonly ConcurrentDictionary<Type, Action<object, ICodeDocException>> s_ExceptionDocSetters = new ConcurrentDictionary<Type, Action<object, ICodeDocException>>();


        /// <summary>
        /// Constrcuts a new instance
        /// </summary>
        /// <param name="xmlDocument"></param>
        public CodeDocAssembly(XmlDocument xmlDocument)
        {
            ClassDocumentation = new Dictionary<string, CodeDocClass>();
            ProcessNodes(xmlDocument.ChildNodes);
        }

        private void ProcessNodes(XmlNodeList childNodes)
        {
            foreach(var childNode in childNodes)
            {
                if(childNode is XmlElement childElement)
                {
                    switch(childElement.Name)
                    {
                        case "linker":
                            continue;
                        case "name":
                            Name = childElement.InnerText;
                            break;
                        case "member":
                            ProcessNodeMember(childElement);
                            break;
                        case "members":
                        case "doc":
                        case "assembly":
                            ProcessNodes(childElement.ChildNodes);
                            break;
                        default:
                            throw new Exception("Cannot handle node:"+childElement.Name);
                    }
                }
            }
        }

        private void ProcessNodeMember(XmlElement childElement)
        {
            var name = childElement.GetAttribute("name");
            var type = name.Substring(0,2);
            ICodeDocMemberWithSummary member;
            switch(type)
            {
                case "T:":
                    var tClassName = GetClassName(name);
                    member = ClassDocumentation[tClassName] = new CodeDocClass(this, tClassName);
                    break;
                case "M:":
                    var mClassName = GetClassName(name);
                    var methodName = GetMethodName(name);
                    if(!ClassDocumentation.TryGetValue(mClassName, out CodeDocClass methodClass))
                    {
                        ClassDocumentation[mClassName] = methodClass = new CodeDocClass(this, mClassName);
                    }
                    member = methodClass.MethodDocumentation[methodName] = new CodeDocMethod(methodClass, methodName);
                    break;
                case "P:":
                    var pClassName = GetClassName(name);
                    var propertyName = GetPropertyName(name);
                    if (!ClassDocumentation.TryGetValue(pClassName, out CodeDocClass propertyClass))
                    {
                        ClassDocumentation[pClassName] = propertyClass = new CodeDocClass(this, pClassName);
                    }
                    member = propertyClass.PropertyDocumentation[propertyName] = new CodeDocProperty(propertyClass, propertyName);
                    break;
                case "E:":
                case "F:":
                    return;
                default:
                throw new Exception("Cannot handle member type:" + type);
            }
            ProcessNodeMemberNodes(member, childElement.ChildNodes);
        }

        private void ProcessNodeMemberNodes(ICodeDocMemberWithSummary member, XmlNodeList childNodes)
        {
            foreach (var childNode in childNodes)
            {
                if (childNode is XmlElement childElement)
                {
                    switch (childElement.Name)
                    {
                        case "summary":
                            s_SummarySetters.GetOrAdd(member.GetType(), CreateSummarySetter).Invoke(member, (childElement.InnerText ?? "").Trim());
                            break;
                        case "param":
                            if(member is CodeDocMethod method)
                            {
                                var codeDocParam = new CodeDocParameter(method, childElement.GetAttribute("name"), childElement.InnerText);
                                s_ParamDocSetters.GetOrAdd(method.GetType(), CreateParamDocSetter).Invoke(member, codeDocParam);
                            }
                            break;
                        case "exception":
                            var codeDocException = new CodeDocException((CodeDocMethod)member, childElement.GetAttribute("cref"), childElement.InnerText);
                            s_ExceptionDocSetters.GetOrAdd(member.GetType(), CreateExceptionDocSetter).Invoke(member, codeDocException);
                            break;
                        case "typeparam":
                        case "returns":
                        case "a":
                            continue;
                        default:
                            //throw new Exception("Cannot handle node:" + childElement.Name);
                            continue;
                    }
                }
            }
        }

        private Action<object, ICodeDocException> CreateExceptionDocSetter(Type t)
        {
            var m = t.GetProperty(nameof(ICodeDocMethod.ExceptionDocumentation)).GetGetMethod();
            return (o, param) =>
            {
                var lst = (IList<ICodeDocException>)m.Invoke(o, null);
                lst.Add(param);
            };
        }

        private Action<object, ICodeDocParameter> CreateParamDocSetter(Type t)
        {
            var m = t.GetProperty(nameof(ICodeDocMethod.ParameterDocumentation)).GetGetMethod();
            return (o, param) =>
            {
                var lst = (IList<ICodeDocParameter>)m.Invoke(o, null);
                lst.Add(param);
            };
        }

        private Action<object, string> CreateSummarySetter(Type t)
        {
            var m = t.GetProperty(nameof(ICodeDocMemberWithSummary.Summary)).GetSetMethod(true);
            return (o, summary) => m.Invoke(o, new object[] { summary });
        }

        IEnumerable<ICodeDocClass> ICodeDocAssembly.ClassDoumentation => ClassDocumentation.Values;

        ICodeDocClass ICodeDocAssembly.GetClassDocumentation(Type type)
        {
            var className = type.FullName.Replace('+', '.');
            CodeDocClass doc;
            if (!ClassDocumentation.TryGetValue(className, out doc))
            {
                doc = new CodeDocClass(this, className) ;
            }
            return doc;
        }

        /// <summary>
        /// The name of the assembly.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The underlying xml document
        /// </summary>
        public XmlDocument XmlDocument { get; }

        /// <summary>
        /// The class documentation
        /// </summary>
        IDictionary<string, CodeDocClass> ClassDocumentation { get; }
    }
}