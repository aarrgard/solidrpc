using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code
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

        public QualifiedName(string name1)
            : this((name1 ?? "").Split('.'))
        {
        }

        public QualifiedName(string name1, string name2)
            : this((name1 ?? "").Split('.').Union((name2 ?? "").Split('.')))
        {
        }

        public QualifiedName(IEnumerable<string> names)
        {
            Names = names.Where(o => !string.IsNullOrEmpty(o)).ToList();
            QName = string.Join(".", Names);
            Namespace = string.Join(".", Names.Reverse().Skip(1).Reverse());
            Name = Names.LastOrDefault();
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