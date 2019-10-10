using System;
using System.IO;

namespace JsonRpcNet.Docs
{
    public class FileContent
    {
        private readonly string _filePath;

        public FileContent() : this("", null)
        {
            
        }
        public FileContent(string filePath, byte[] buffer)
        {
            _filePath = filePath;
            Buffer = buffer;
        }

        public string Extension => Path.GetExtension(_filePath);

        public byte[] Buffer { get; }

        public bool Exist => Buffer != null;
    }
}