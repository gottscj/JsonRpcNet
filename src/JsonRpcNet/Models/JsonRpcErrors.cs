namespace JsonRpcNet.Models
{
	public enum JsonRpcErrorCodes : int
	{
		ParseError = -32700,
		InvalidRequest = -32600,
		MethodNotFound = -32601,
		InvalidParams = -32602,
		InternalError = -32603
	};

	public static class JsonRpcErrors
	{
		public static JsonRpcErrorResponse ParseError(string message = "Parse error")
		{
			return CreateErrorResponse(null, JsonRpcErrorCodes.ParseError, message);
		}

		public static JsonRpcErrorResponse InvalidRequest(string message = "Invalid Request")
		{
			return CreateErrorResponse(null, JsonRpcErrorCodes.InvalidRequest, message);
		}

		public static JsonRpcErrorResponse MethodNotFound(object id, string message = "The method does not exist / is not available.")
		{
			return CreateErrorResponse(id, JsonRpcErrorCodes.MethodNotFound, message);
		}

		public static JsonRpcErrorResponse InvalidParams(object id, string message = "Invalid params")
		{
			return CreateErrorResponse(id, JsonRpcErrorCodes.InvalidParams, message);
		}

		public static JsonRpcErrorResponse InternalError(object id, string message = "Internal error")
		{
			return CreateErrorResponse(id, JsonRpcErrorCodes.InternalError, message);
		}

		private static JsonRpcErrorResponse CreateErrorResponse(object id, JsonRpcErrorCodes code, string message)
		{
			return new JsonRpcErrorResponse
			{
				Id = id,
				Error = new JsonRpcError
				{
					Code = (int)code,
					Message = message
				}
			};
		}
	}
}