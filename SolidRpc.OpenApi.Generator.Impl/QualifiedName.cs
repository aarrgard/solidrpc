using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Generator
{
    /// <summary>
    /// Represents a qualified name
    /// </summary>
    public class QualifiedName
    {
        public static implicit operator QualifiedName(string s)
        {
            return new QualifiedName("", s);
        }
        public static implicit operator string(QualifiedName qn)
        {
            return qn.ToString();
        }
        public static QualifiedName operator +(QualifiedName a, QualifiedName b)
        {
            return new QualifiedName(a,b);
        }

        public QualifiedName(params string[] namespaces)
            : this(namespaces.Where(o => o!=null).SelectMany(o=>SplitNamespace(o)))
        {
        }
        private static IEnumerable<string> SplitNamespace(string ns)
        {
            var genType = ns.IndexOf('<');
            if (genType > -1)
            {
                var splits = SplitNamespace(ns.Substring(0, genType));
                var count = splits.Count();
                splits = splits.Take(count - 1).Union(new[] { splits.Last() + ns.Substring(genType)});
                return splits;
            }
            return ns.Split('.');
        }

        private QualifiedName(IEnumerable<string> names)
        {
            Names = names.Where(o => !string.IsNullOrEmpty(o)).ToList();
            QName = string.Join(".", Names);
            Namespace = string.Join(".", Names.Reverse().Skip(1).Reverse());
            Name = Names.LastOrDefault();
            if(Namespace.Contains("<"))
            {
                throw new Exception("Illegal characters in namespace");
            }
        }

        /// <summary>
        /// The name parts
        /// </summary>
        public IEnumerable<string> Names { get; }

        /// <summary>
        /// The name parts
        /// </summary>
        public string QName { get; }

        /// <summary>
        /// Returns the namespace.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Returns the name
        /// </summary>
        public string Name { get; }

        public override string ToString()
        {
            return QName;
        }
    }
}