using ICSharpCode.SharpZipLib.Zip;
using SolidRpc.OpenApi.Model.CSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace SolidRpc.OpenApi.Generator.Impl
{
    public class CodeWriterZip : ICodeWriter
    {
        public CodeWriterZip(string projectNamespace)
        {
            ProjectNamespace = projectNamespace ?? "";
            IndentationString = "    ";
            IndentationStack = new Stack<string>();
            IndentationStack.Push("");
            MemoryStream = new MemoryStream();
            ZipOutputStream = new ZipOutputStream(MemoryStream);
        }
        public MemoryStream MemoryStream { get; }

        public ZipOutputStream ZipOutputStream { get; }

        public string ProjectNamespace { get; set; }

        public string IndentationString { get; private set; }

        private Stack<string> IndentationStack { get; set; }
        private string CurrentIndentation => IndentationStack.Peek();

        public TextWriter CurrentWriter { get; private set; }

        public string NewLine => Environment.NewLine;

        public bool IndentOnNextEmit { get; private set; }

        public void MoveToClassFile(string fullClassName)
        {
            var prefix = string.IsNullOrEmpty(ProjectNamespace) ? "" : $"{ProjectNamespace}.";
            if (!fullClassName.StartsWith(prefix))
            {
                throw new ArgumentException($"Cannot generate class({fullClassName}) that does not belong to the project namespace({prefix})");
            }
            Close();
            var fileName = fullClassName.Replace('.', Path.DirectorySeparatorChar) + ".cs";
            if(!string.IsNullOrEmpty(prefix))
            {
                fileName = fileName.Substring(prefix.Length);
            }
            var ze = new ZipEntry(fileName);
            ZipOutputStream.PutNextEntry(ze);
            CurrentWriter = new StreamWriter(ZipOutputStream);
        }

        public void Close()
        {
            if (CurrentWriter != null)
            {
                CurrentWriter.Flush();
                CurrentWriter = null;
                ZipOutputStream.CloseEntry();
                while(IndentationStack.Count > 1)
                {
                    IndentationStack.Pop();
                }
                IndentOnNextEmit = false;
            }
        }

        public void Emit(string txt)
        {
            if(txt == null)
            {
                return;
            }
            if (IndentOnNextEmit)
            {
                CurrentWriter.Write(NewLine);
                CurrentWriter.Write(CurrentIndentation);
            }
            if (txt.EndsWith(NewLine))
            {
                txt = txt.Substring(0, txt.Length - NewLine.Length);
                IndentOnNextEmit = true;
            }
            else
            {
                IndentOnNextEmit = false;
            }
            txt = txt.Replace(NewLine, $"{NewLine}{CurrentIndentation}");
            CurrentWriter.Write(txt);
        }

        public void Indent(string indentation = null)
        {
            if (indentation == null) indentation = IndentationString;
            IndentationStack.Push(CurrentIndentation + indentation);
        }

        public void Unindent()
        {
            if(IndentationStack.Count <= 1)
            {
                throw new Exception("Cannot pop more indentations than you push.");
            }
            IndentationStack.Pop();
        }
    }
}
