using System;
using System.IO;

namespace JsonRpcNet.Docs
{
    public static class EmbeddedFileReader
    {
        public static string GetFilePath(string requestPath, string basePath)
        {
            string filePath = requestPath;
            if (filePath.EndsWith("/"))
            {
				filePath = requestPath.Substring(0, requestPath.Length - 1);
            }

            if (filePath.Equals(basePath))
            {
                filePath = "index.html";
            }
            else if (filePath.StartsWith(basePath))
            {
                filePath = requestPath.Substring(basePath.Length + 1);
            }

			if (filePath.StartsWith("/"))
			{
				filePath = filePath.Substring(1);
			}

            return filePath;
        }

        public static byte[] GetEmbeddedFile(string filePath)
        {
            var resourcePath = filePath.Replace("/", ".");
            var embeddedResource = $"{typeof(JsonRpcDoc).Namespace}.resources.{resourcePath}";
            using (var stream = typeof(JsonRpcDoc).Assembly.GetManifestResourceStream(embeddedResource))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"No content was found for file '{filePath}'");
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