using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JsonRpcNet
{
    public static class EventProxy
    {
        //void delegate with one parameter
        static public Delegate Create(EventInfo evt, Action<EventArgs> d)
        {
            var handlerType = evt.EventHandlerType;
            var eventParams = handlerType.GetMethod("Invoke").GetParameters();

            //lambda: (object x0, ExampleEventArgs x1) => d(x1)
            var parameters = eventParams.Select(p => Expression.Parameter(p.ParameterType, "x")).ToArray();
            if (!typeof(EventArgs).IsAssignableFrom(parameters[1].Type))
            {
                throw new NotSupportedException(parameters[1] + "->" + typeof(EventArgs));
            }
            var body = Expression.Call(Expression.Constant(d), d.GetType().GetMethod("Invoke"), parameters[1]);
            var lambda = Expression.Lambda(body, parameters);
            return Delegate.CreateDelegate(handlerType, lambda.Compile(), "Invoke", false);
        }
    }
}