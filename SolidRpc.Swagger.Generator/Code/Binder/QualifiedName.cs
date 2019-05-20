using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.Swagger.Generator.Code.Binder
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
        {
            Names = (name1 ?? "").Split('.').Where(o => !string.IsNullOrEmpty(o)).ToArray();
            QName = string.Join(".", Names);
        }

        public QualifiedName(string name1, string name2)
        {
            Names = (name1 ?? "").Split('.').Union((name2 ?? "").Split('.')).Where(o => !string.IsNullOrEmpty(o)).ToArray();
            QName = string.Join(".", Names);
        }

        /// <summary>
        /// The name parts
        /// </summary>
        public IEnumerable<string> Names { get; }

        /// <summary>
        /// The name parts
        /// </summary>
        public string QName { get; }

        public override string ToString()
        {
            return QName;
        }
    }
}