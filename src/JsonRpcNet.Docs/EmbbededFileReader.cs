using System;
using System.IO;

namespace JsonRpcNet.Docs
{
    public class EmbeddedFileReader
    {
        private readonly string _embeddedResource;

        public EmbeddedFileReader(string requestPath, string basePath)
        {
            string filePath = null;
            if (requestPath.EndsWith("/"))
            {
                requestPath = requestPath.Substring(0, requestPath.Length - 1);
            }
            if (requestPath.Equals(basePath))
            {
                filePath = "index.html";
            }
            else if (requestPath.StartsWith(basePath))
            {
                filePath = requestPath.Substring(basePath.Length + 1);
                filePath = filePath.Replace("/", ".");
            }
            _embeddedResource = $"{typeof(JsonRpcDoc).Namespace}.resources.{filePath}";
        }
        public byte[] GetEmbeddedFile()
        {
            using (var stream = typeof(JsonRpcDoc).Assembly.GetManifestResourceStream(_embeddedResource))
            {
                if (stream == null)
                {
                    var split = _embeddedResource.Split('.');
                    var filename = split[split.Length - 2] + "." + split[split.Length - 1];
                    throw new InvalidOperationException($"File '{filename}' not found");
                }
                return ReadToEnd(stream);
            }
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if(stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if(stream.CanSeek)
                {
                    stream.Position = originalPosition; 
                }
            }
        }
    }
}