using System;
using System.IO;

namespace SolidRpc.Swagger.Generator.Code.CSharp
{
    public class CodeWriterFile : ICodeWriter
    {
        private TextWriter _textWriter;

        public CodeWriterFile(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                throw new ArgumentException("Directory does not exist:" + outputPath);
            }
            OutputPath = outputPath;
            IndentationString = "    ";
            CurrentIndentation = "";
        }

        public string OutputPath { get; }

        public string IndentationString { get; private set; }

        public string CurrentIndentation { get; private set; }

        public TextWriter CurrentWriter
        {
            get
            {
                return _textWriter;
            }
            private set
            {
                if(_textWriter != null)
                {
                    _textWriter.Close();
                }
                _textWriter = value;
            }
        }

        public string NewLine => Environment.NewLine;

        public bool IndentOnNextEmit { get; private set; }

        public void MoveToFile(string fileName)
        {
            CurrentWriter = null;
            var filePath = Path.Combine(OutputPath, fileName);
            var file = new FileInfo(filePath);
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            CurrentWriter = file.CreateText();
        }

        public void Close()
        {
            CurrentWriter = null;
        }

        public void Emit(string txt)
        {
            if(IndentOnNextEmit)
            {
                CurrentWriter.Write(NewLine);
                CurrentWriter.Write(CurrentIndentation);
            }
            if(txt.EndsWith(NewLine))
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
