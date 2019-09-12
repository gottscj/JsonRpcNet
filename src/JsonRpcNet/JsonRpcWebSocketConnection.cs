using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JsonRpcNet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonRpcNet
{
	public abstract class JsonRpcWebSocketConnection : WebSocketConnection
	{
		private static readonly JsonRpcMethodCache MethodCache = new JsonRpcMethodCache();
		
		private const string TokenQueryString = "token";

		protected JsonRpcWebSocketConnection() 
		{
		}

		protected Task SendAsync(JsonRpcContract jsonRpc)
		{
			return SendAsync(jsonRpc.ToJson());
		}

        protected Task BroadcastAsync(JsonRpcContract jsonRpc)
        {
            return BroadcastAsync(jsonRpc.ToJson());
        }

        protected override async Task OnMessage(string msg)
		{
			//TODO: Only accept requests. At the moment, responses are at the moment considered as invalid requests.
			//      Response handling has to be implemented if the server should be allowed to send requests.
			try
			{
				var request = DeserializeRequest(msg);
				var method = GetRequestMethodInfo(request);
				var paramsArray = CreateParamsArray(request, method.MethodInfo);
				var result = InvokeRpcMethod(request, method, paramsArray);
				if (request.Id != null)
				{
					// do not answer notifications
					await SendAsync(result).ConfigureAwait(false);
				}
			}
			catch (JsonRpcErrorException e)
			{
				//JsonRpcWebSocketServiceEventSource.Log.RequestError(ID, GetType().Name, e.Error.Error.Message, msg.Data, e.Error.ToJson(), e.ToString());
				await SendAsync(e.Error);
			}
		}
		
		protected override Task OnConnected()
		{
			var userEndPointIpAddress = GetUserEndpointIpAddress();
//			var identity = GetIdentity();
//			if (identity == null || userEndPointIpAddress == null)
//			{
////				JsonRpcWebSocketServiceEventSource.Log.ConnectionRefused(ID, userEndPointIpAddress?.ToString() ?? "Unknown", GetType().Name);
//				return CloseAsync(CloseStatusCode.PolicyViolation, "Not authorized");
//			}

			// session id and connection id are the same, since there is one connection per session
//			_connection = _connectionManager.Add(ID, userEndPointIpAddress.ToString());
//			_connection.UserId = Guid.Parse(identity.GetUserId());
//			_connection.SessionId = ID;
//
//			_permissions = identity.Permissions;
//
//			JsonRpcWebSocketServiceEventSource.Log.ConnectionOpened(ID, userEndPointIpAddress.ToString(), GetType().Name);

			return Task.CompletedTask;

		}

		protected override Task OnDisconnected(CloseStatusCode code, string reason)
		{
//			_connectionManager.Remove(ID);
//
//			JsonRpcWebSocketServiceEventSource.Log.ConnectionClosed(ID, GetType().Name);
			return Task.CompletedTask;

		}

//		private IIdentity GetIdentity()
//		{
//			return _tokenReader.GetIdentity(Context.QueryString[TokenQueryString]);
//		}

		private static JsonRpcRequest DeserializeRequest(string requestStr)
		{
			try
			{
				return JsonConvert.DeserializeObject<JsonRpcRequest>(requestStr);
			}
			catch (JsonReaderException e)
			{
				var exampleJsonRpc = JsonConvert.SerializeObject(new JsonRpcRequest
				{
					Id = "1",
					Method = "SomeMethod",
					Params = new object[] {"param1", "param2"}
				});
				var errorMessage = $"Invalid input expected jsonrpc 2.0 input '{exampleJsonRpc}'\r\n" +
				                   $"Serializer error message: '{e.Message}'";
				var errorResponse = JsonRpcErrors.ParseError(errorMessage);
				throw new JsonRpcErrorException(errorResponse, e);
			}
			catch (Exception e)
			{
				var errorResponse = JsonRpcErrors.InvalidRequest(e.Message);
				throw new JsonRpcErrorException(errorResponse, e);
			}
		}

		private MethodInfoWithPermissions GetRequestMethodInfo(JsonRpcRequest request)
		{
			var methodCache = MethodCache.Get(this);
			if (methodCache.TryGetValue(request.Method.ToLowerInvariant(), out var method))
			{
				return method;
			}

			var message = $"Unknown method: '{request.Method.ToLowerInvariant()}' valid methods (casing is ignored): " +
			              $"{string.Join(", ", methodCache.Select(m => m.Key))}";
			var errorResponse = JsonRpcErrors.MethodNotFound(request.Id, message);
			throw new JsonRpcErrorException(errorResponse);
		}

		private static object[] CreateParamsArray(JsonRpcRequest request, FastMethodInfo method)
		{
			try
			{
				 return CreateParams(request, method);
			}
			catch (Exception e)
			{
				var errorResponse = JsonRpcErrors.InvalidParams(request.Id, e.Message);
				throw new JsonRpcErrorException(errorResponse, e);
			}
		}

		private JsonRpcResultResponse InvokeRpcMethod(JsonRpcRequest request, MethodInfoWithPermissions method, object[] paramsArray)
		{
			// Execute method and get result
			try
			{
				var result = method.Invoke(this, paramsArray, null);
				return new JsonRpcResultResponse(result) {Id = request.Id};
			}
			catch (Exception e)
			{
				var errorResponse = JsonRpcErrors.InternalError(request.Id, e.Message);
				throw new JsonRpcErrorException(errorResponse, e);
			}
		}

		private static object[] CreateParams(JsonRpcRequest request, FastMethodInfo methodInfo)
		{
			var parameters = methodInfo.Parameters;
			switch (request.Params)
			{
				case JArray jArray:
				{
					if (jArray.Count != parameters.Length)
					{
						throw new InvalidOperationException($"Invalid number of params, " +
						                                    $"expected {parameters.Length}, got {jArray.Count}\r\n" +
						                                    $"Expected parameters: [{string.Join(",", parameters.Select(p => p.Name))}]");
					}
					var result = new object[jArray.Count];
					for (var i = 0; i < result.Length; i++)
					{
						result[i] = jArray[i].ToObject(parameters[i].ParameterType);
					}

					return result;
				}
				case JObject jObject:
				{
					if (TryGetNamedParams(jObject, parameters, out var namedParameters))
					{
						return namedParameters;
					}

					// Handles the case where a single object (with its properties) is received as parameter (neither in an array nor as named args)
					var value = jObject.ToObject(parameters[0].ParameterType);
					return new[] { value };
				}
				default:
				{
					// Handles the case where a single parameter is received (neither in an array nor as named args)
					var jArray = request.Params == null ? new JArray() : new JArray { request.Params };
					return CreateParams(new JsonRpcRequest
					{
						Id = request.Id,
						Method = request.Method,
						Params =  jArray
					}, methodInfo);
				}
			}
		}

		private static bool TryGetNamedParams(JObject jObject, ParameterInfo[] parameters, out object[] namedParameters)
		{
			namedParameters = new object[parameters.Length];
			if (parameters.Length != jObject.Count)
			{
				throw new InvalidOperationException($"Invalid number of params, " +
				                                    $"expected {parameters.Length}, got {jObject.Count}\r\n" +
				                                    $"Expected parameters: [{string.Join(",", parameters.Select(p => p.Name))}]");
			}

			for(var i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				if (!jObject.TryGetValue(parameter.Name, StringComparison.InvariantCultureIgnoreCase, out var value))
				{
					namedParameters = null;
					return false;
				}

				namedParameters[i] = value.ToObject(parameter.ParameterType);
			}

			return true;
		}

		
	}
}
