using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace JsonRpcNet
{
	public class FastMethodInfo
	{
		private delegate object ReturnValueDelegate(object instance, object[] arguments);
		private delegate void VoidDelegate(object instance, object[] arguments);
		private object _syncRoot = new object();
		private Dictionary<Type, ReturnValueDelegate> _taskResultCache = new Dictionary<Type,ReturnValueDelegate>();
		public FastMethodInfo(MethodInfo methodInfo)
		{
			Parameters = methodInfo.GetParameters();
			var instanceExpression = Expression.Parameter(typeof(object), "instance");
			var argumentsExpression = Expression.Parameter(typeof(object[]), "arguments");
			var argumentExpressions = new List<Expression>();
			for (var i = 0; i < Parameters.Length; ++i)
			{
				var parameterInfo = Parameters[i];
				argumentExpressions.Add(Expression.Convert(Expression.ArrayIndex(argumentsExpression, Expression.Constant(i)), parameterInfo.ParameterType));
			}
			var callExpression = Expression.Call(!methodInfo.IsStatic ? Expression.Convert(instanceExpression, methodInfo.ReflectedType) : null, methodInfo, argumentExpressions);
			if (callExpression.Type == typeof(void))
			{
				var voidDelegate = Expression.Lambda<VoidDelegate>(callExpression, instanceExpression, argumentsExpression).Compile();
				Delegate = (instance, arguments) => { voidDelegate(instance, arguments); return null; };
			}
			else
			{
				Delegate = Expression.Lambda<ReturnValueDelegate>(Expression.Convert(callExpression, typeof(object)), instanceExpression, argumentsExpression).Compile();
			}
		}

		private ReturnValueDelegate Delegate { get; }

		public ParameterInfo[] Parameters { get; }
		
		public Task<object> InvokeAsync(object instance, params object[] arguments)
		{
			var result = Delegate(instance, arguments);
			if (!(result is Task task)) return Task.FromResult(result);

			return GetTaskResult(task);
		}

		private async Task<object> GetTaskResult(Task task)
		{
			await task.ConfigureAwait(false);

			var taskType = task.GetType();
			if (!taskType.IsGenericType)
			{
				return null;
			}
			ReturnValueDelegate valueDelegate;
			lock (_syncRoot)
			{
				if (!_taskResultCache.TryGetValue(taskType, out valueDelegate))
				{
					var property = taskType.GetProperty("Result");
					valueDelegate = property == null ? 
						(instance, arguments) => null : // should never happen, but doesnt hurt. ¯\_(ツ)_/¯
						new FastMethodInfo(property.GetGetMethod()).Delegate;
					_taskResultCache.Add(taskType, valueDelegate);
				}
			}

			return valueDelegate.Invoke(task, null);
		}
	}
}