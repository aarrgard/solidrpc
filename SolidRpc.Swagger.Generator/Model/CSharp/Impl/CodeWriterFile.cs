using System;
using System.IO;

namespace SolidRpc.Swagger.Generator.Model.CSharp.Impl
{
    public class CodeWriterFile : ICodeWriter
    {
        private TextWriter _textWriter;

        public CodeWriterFile(string outputPath, string projectNamespace)
        {
            if (!Directory.Exists(outputPath))
            {
                throw new ArgumentException("Directory does not exist:" + outputPath);
            }
            ProjectNamespace = projectNamespace;
            OutputPath = outputPath;
            IndentationString = "    ";
            CurrentIndentation = "";
        }
        public string ProjectNamespace { get; set; }

        public string OutputPath { get; }

        public string IndentationString { get; private set; }

        public string CurrentIndentation { get; private set; }

        public StringWriter CurrentWriter { get; private set; }

        public string NewLine => Environment.NewLine;

        public bool IndentOnNextEmit { get; private set; }

        public FileInfo CurrentFile { get; private set; }

        public void MoveToClassFile(string fullClassName)
        {
            if (!fullClassName.StartsWith($"{ProjectNamespace}."))
            {
                throw new ArgumentException("Cannot generate classes that does not belong to the project namespace");
            }
            Close();
            var fileName = fullClassName.Substring(ProjectNamespace.Length + 1).Replace('.', Path.DirectorySeparatorChar) + ".cs";
            var filePath = Path.Combine(OutputPath, fileName);
            var file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            CurrentFile = file;
            CurrentWriter = new StringWriter();
        }

        public void Close()
        {
            string oldContent = "";
            if(CurrentFile != null && CurrentFile.Exists)
            {
                using (var tr = CurrentFile.OpenText())
                {
                    oldContent = tr.ReadToEnd();
                }
            }
            if (CurrentWriter != null)
            {
                var newContent = CurrentWriter.ToString();
                if(!newContent.Equals(oldContent))
                {
                    using (var sw = CurrentFile.CreateText())
                    {
                        sw.Write(newContent);
                    }
                }
            }
            CurrentWriter = null;
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
