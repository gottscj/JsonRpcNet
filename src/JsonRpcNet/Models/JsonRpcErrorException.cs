using System;

namespace JsonRpcNet.Models
{
	public sealed class JsonRpcErrorException : Exception
	{
		public JsonRpcErrorResponse Error { get; }

		public JsonRpcErrorException(JsonRpcErrorResponse errorResponse)
			: base(errorResponse.Error.Message)
		{
			Error = errorResponse;
		}

		public JsonRpcErrorException(JsonRpcErrorResponse errorResponse, Exception inner)
			: base(errorResponse.Error.Message, inner)
		{
			Error = errorResponse;
		}
	}
}