using SolidRpc.Abstractions.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidRpc.OpenApi.Model.CSharp.Impl
{
    /// <summary>
    /// Implements the logic for a c# type. Interfaces and classes
    /// </summary>
    public abstract class CSharpType : CSharpMember, ICSharpType
    {
        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        /// <param name="runtimeType"></param>
        public CSharpType(ICSharpMember ns, string name, Type runtimeType) : base(ns, name)
        {
            RuntimeType = runtimeType;
        }

        /// <summary>
        /// The runtime type
        /// </summary>
        public Type RuntimeType { get; }

        /// <summary>
        /// Signals if the type has been initialized
        /// </summary>
        public bool Initialized { get; set; }

        /// <summary>
        /// Returns true if this is a file type
        /// </summary>
        public bool IsFileType
        {
            get
            {
                var runtimeProps = Properties.ToDictionary(o => o.Name, o => o.PropertyType.RuntimeType);
                return FileContentTemplate.IsFileType(FullName, runtimeProps);
            }
        }

        /// <summary>
        /// Returns true if this is a generic type
        /// </summary>
        public bool IsGenericType => Name.Contains('<');

        /// <summary>
        /// Returns true if this type is an enum type.
        /// </summary>
        public bool IsEnumType => this is ICSharpEnum;


        /// <summary>
        /// Returns true if this type is an enum type.
        /// </summary>
        public bool IsValueType => this is ICSharpStruct;

        /// <summary>
        /// Returns true if this is a task type
        /// </summary>
        public bool IsTaskType => Name.StartsWith("Task") && Namespace.FullName == "System.Threading.Tasks";

        /// <summary>
        /// Returns true if this is a cancellation token type
        /// </summary>
        public bool IsCancellationTokenType => Name == "CancellationToken" && Namespace.FullName == "System.Threading";

        /// <summary>
        /// Adds an extends clause to this type
        /// </summary>
        /// <param name="extType"></param>
        public void AddExtends(ICSharpType extType)
        {
            if(Members.OfType<ICSharpTypeExtends>().Where(o => o.Name == extType.FullName).Any())
            {
                return;
            }
            var ext = new CSharpTypeExtends(this, extType);
            ProtectedMembers.Add(ext);
        }

        /// <summary>
        /// Adds the supplied member
        /// </summary>
        /// <param name="member"></param>
        public override void AddMember(ICSharpMember member)
        {
            ProtectedMembers.Add(member);
        }

        /// <summary>
        /// Returns the generic arguments
        /// </summary>
        /// <returns></returns>
        public ICollection<ICSharpType> GetGenericArguments()
        {
            var (typeName, genArgs) = CSharpRepository.ReadType(FullName);
            var repo = GetParent<ICSharpRepository>();
            return genArgs?.Select(o => {
                return repo.GetType(o) ?? throw new Exception("Cannot find generic arg:" + o);
            }).ToList();
        }

        /// <summary>
        /// Determines if this is a generic type
        /// </summary>
        /// <returns></returns>
        public ICSharpType GetGenericType()
        {
            var (typeName, genArgs) = CSharpRepository.ReadType(FullName);
            if (genArgs.Any())
            {
                var repo = GetParent<ICSharpRepository>();
                return repo.GetType(typeName);
            }
            return null;
        }

        /// <summary>
        /// Determines if this is a dictionary type
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public bool IsDictionaryType(out ICSharpType keyType, out ICSharpType valueType)
        {
            if (!IsGenericType)
            {
                keyType = null;
                valueType = null;
                return false;
            }
            var (typeName, genArgs) = CSharpRepository.ReadType(FullName);
            if(typeName != "System.Collections.Generic.IDictionary")
            {
                keyType = null;
                valueType = null;
                return false;
            }
            var repo = GetParent<ICSharpRepository>();
            keyType = repo.GetType(genArgs[0]);
            valueType = repo.GetType(genArgs[1]);
            return true;
        }

        /// <summary>
        /// Sets the modifier
        /// </summary>
        /// <param name="modifier"></param>
        public void SetModifier(string modifier)
        {
            if(!Members.OfType<ICSharpModifier>().Any(o => o.Name == modifier))
            {
                AddMember(new CSharpModifier(this, modifier));
            }
        }

        /// <summary>
        /// Emits the type to supplied code writer.
        /// </summary>
        /// <param name="codeWriter"></param>
        public override void WriteCode(ICodeWriter codeWriter)
        {
            var isNsType = Parent is ICSharpNamespace;
            if (isNsType)
            {
                codeWriter.MoveToClassFile(FullName);
            }
            Members.OfType<ICSharpUsing>().ToList().ForEach(o =>
            {
                o.WriteCode(codeWriter);
            });

            if (isNsType)
            {
                codeWriter.Emit($"namespace {Namespace.FullName} {{{codeWriter.NewLine}");
                codeWriter.Indent();
            }
            var structType = (this is CSharpEnum) ? "enum" : (this is CSharpInterface) ? "interface" : "class";

            WriteSummary(codeWriter);
            WriteAttributes(codeWriter);
            Members.OfType<ICSharpModifier>().ToList().ForEach(o => o.WriteCode(codeWriter));
            codeWriter.Emit($"{structType} {Name}");
            if(Members.OfType<ICSharpTypeExtends>().Any())
            {
                codeWriter.Emit($" : {string.Join(",", Members.OfType<ICSharpTypeExtends>().Select(o => o.Name))}");
            }
            codeWriter.Emit($" {{{codeWriter.NewLine}");
            Members.Where(o => o is ICSharpMethod ||
                        o is ICSharpProperty ||
                        o is ICSharpConstructor ||
                        o is ICSharpType || 
                        o is ICSharpField)
                .ToList()
                .ForEach(o =>
                {
                    codeWriter.Indent();
                    o.WriteCode(codeWriter);
                    codeWriter.Unindent();
                    codeWriter.Emit(codeWriter.NewLine);
                });
            codeWriter.Emit($"}}{codeWriter.NewLine}");

            if (isNsType)
            {
                codeWriter.Unindent();
                codeWriter.Emit($"}}{codeWriter.NewLine}");
            }
        }
    }
}