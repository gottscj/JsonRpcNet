using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JsonRpcNet.Docs
{
    public static class JsonTypeHelper
    {
        public static string GetSchemaTypeString(Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Boolean: return "boolean";
                case TypeCode.Byte: return "integer";
                case TypeCode.Char: return "string";
                case TypeCode.DateTime: return "string";
                case TypeCode.DBNull: return "null";
                case TypeCode.Decimal: return "number";
                case TypeCode.Double: return "number";
                case TypeCode.Empty: return "null";
                case TypeCode.Int16: return "integer";
                case TypeCode.Int32: return "integer";
                case TypeCode.Int64: return "integer";
                case TypeCode.Object: 
                {
                    if (type == typeof(void) || type == typeof(Task))
                    {
                        return "void";
                    }
                    return typeof(IEnumerable<object>).IsAssignableFrom(type) ? 
                    "array" : 
                    "object";
                    
                }
                case TypeCode.SByte: return "integer";
                case TypeCode.Single: return "number";
                case TypeCode.String: return "string";
                case TypeCode.UInt16: return "integer";
                case TypeCode.UInt32: return "integer";
                case TypeCode.UInt64: return "integer";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}