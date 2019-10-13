using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonRpcNet.Docs
{
    public static class JsonRpcFileReader
    {
		const string staticResourcesPath = "web.dist";
        public static FileContent GetFile(string requestPath, JsonRpcInfoDoc info)
        {
            var basePath = info?.JsonRpcApiEndpoint ?? "/jsonrpc";
            string filePath;

            if (!basePath.StartsWith("/"))
            {
                basePath = "/" + basePath;
            }

            if (!requestPath.StartsWith(basePath))
            {
                return new FileContent();
            }
            
            if (requestPath.EndsWith("/"))
            {
                requestPath = requestPath.Substring(0, requestPath.Length - 1);
            }
            if (requestPath.EndsWith("jsonRpcApi.json"))
            {
                var jsonRpcDoc = DocGenerator.GenerateJsonRpcDoc(info);
                var doc = JsonConvert.SerializeObject(jsonRpcDoc, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                var buffer= Encoding.UTF8.GetBytes(doc);
                
                var fileResult = new FileContent(requestPath, buffer);
                
                return fileResult;
            }
            if (requestPath.Equals(basePath))
            {
                filePath = "index.html";
            }
            else
            {
                filePath = requestPath.Substring(basePath.Length + 1);
                filePath = filePath.Replace("/", ".");
            }
            var embeddedResource = $"{typeof(JsonRpcDoc).Namespace}.{staticResourcesPath}.{filePath}";
            using (var stream = typeof(JsonRpcDoc).Assembly.GetManifestResourceStream(embeddedResource))
            {
                byte[] buffer = null;
                if (stream != null)
                {
                    buffer = ReadToEnd(stream);
                }
                var fileResult = new FileContent(filePath, buffer);
                return fileResult;
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