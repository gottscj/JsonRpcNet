using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;

namespace JsonRpcNet.Docs
{
    public static class JsonRpcFileReader
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>{new StringEnumConverter()}
        };
        
		const string staticResourcesPath = "web.dist";
        public static FileContent GetFile(string requestPath, JsonRpcInfo info)
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
                
                var schemaSettings = new JsonSchemaGeneratorSettings
                {
                    SerializerSettings = JsonSerializerSettings,
                    GenerateExamples = true,
                    SchemaType = SchemaType.JsonSchema,
                    GenerateAbstractSchemas = false,
                };
                
                var schema = new JsonSchema();
                
                var resolver = new JsonSchemaResolver(schema, schemaSettings);
                var generator = new JsonSchemaGenerator(schemaSettings);
                
                generator.Generate(schema, typeof(object), resolver);

                var jDoc = JObject.FromObject(jsonRpcDoc);
                // generate schema definitions
                foreach (var rpcService in jsonRpcDoc.Services)
                {
                    foreach (var rpcMethod in rpcService.Methods)
                    {
                        if (rpcMethod.Response.Type != typeof(void) && rpcMethod.Response.Type != typeof(Task))
                        {
                            generator.Generate(rpcMethod.Response.Type, resolver);
                        }
                        
                        foreach (var rpcMethodParameter in rpcMethod.Parameters)
                        {
                            generator.Generate(rpcMethodParameter.Type, resolver);
                        }
                    }
                }

                var schemaJObject = JObject.Parse(schema.ToJson());
                jDoc["definitions"] = schemaJObject["definitions"];
                
                var buffer= Encoding.UTF8.GetBytes(jDoc.ToString(Formatting.None));
                
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
        public static string ToLowerFirstChar(string input)
        {
            string newString = input;
            if (!String.IsNullOrEmpty(newString) && Char.IsUpper(newString[0]))
                newString = Char.ToLower(newString[0]) + newString.Substring(1);
            return newString;
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