using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace SolidRpc.OpenApi.Generator.Model.CSharp.Impl
{
    public class CodeWriterZip : ICodeWriter
    {
        public CodeWriterZip(string projectNamespace)
        {
            ProjectNamespace = projectNamespace;
            IndentationString = "    ";
            CurrentIndentation = "";
            MemoryStream = new MemoryStream();
            ZipOutputStream = new ZipOutputStream(MemoryStream);
        }
        public MemoryStream MemoryStream { get; }

        public ZipOutputStream ZipOutputStream { get; }

        public string ProjectNamespace { get; set; }

        public string IndentationString { get; private set; }

        public string CurrentIndentation { get; private set; }

        public TextWriter CurrentWriter { get; private set; }

        public string NewLine => Environment.NewLine;

        public bool IndentOnNextEmit { get; private set; }

        public void MoveToClassFile(string fullClassName)
        {
            if (!fullClassName.StartsWith($"{ProjectNamespace}."))
            {
                throw new ArgumentException("Cannot generate classes that does not belong to the project namespace");
            }
            Close();
            var fileName = fullClassName.Substring(ProjectNamespace.Length + 1).Replace('.', Path.DirectorySeparatorChar) + ".cs";
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
                CurrentIndentation = "";
                IndentOnNextEmit = false;
            }
        }

        public void Emit(string txt)
        {
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

        public void Indent()
        {
            CurrentIndentation = CurrentIndentation + IndentationString;
        }

        public void Unindent()
        {
            CurrentIndentation = CurrentIndentation.Substring(0, CurrentIndentation.Length - IndentationString.Length);
        }
    }
}
