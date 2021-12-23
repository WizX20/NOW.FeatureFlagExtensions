using System.Reflection;

namespace NOW.FeatureFlagExtensions.DependencyInjection.Helpers
{
    ////Initial code:
    //IInterface obj = new Implementation();

    ////Interceptor added:
    //IInterface obj = new Implementation();
    //var interceptor = new MyInterceptor<IInterface>();
    //obj = interceptor.Decorate(obj);

    //Interceptor class (every method there is optional):
    public class MyInterceptor<T> : ClassInterceptor<T>
    {
        protected override void OnInvoked(MethodInfo methodInfo, object[] args, object result)
        {
            // do something when the method or property call ended successfully
            var test = 1;
        }

        protected override void OnInvoking(MethodInfo methodInfo, object[] args)
        {
            // do something before the method or property call is invoked
            var test = 2;
        }

        protected override void OnException(MethodInfo methodInfo, object[] args, Exception exception)
        {
            // do something when a method or property call throws an exception
            var test = 3;
        }
    }

    public abstract class ClassInterceptor<TInterface> : DispatchProxy
    {
        private object _decorated;

        public ClassInterceptor() : base()
        {
        }

        public TInterface Decorate<TImplementation>(TImplementation decorated)
            where TImplementation : TInterface
        {
            var proxy = typeof(DispatchProxy)
                .GetMethod("Create")
                .MakeGenericMethod(typeof(TInterface), GetType())
                .Invoke(null, Array.Empty<object>())
                as ClassInterceptor<TInterface>;

            proxy._decorated = decorated;

            return (TInterface)(object)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            OnInvoking(targetMethod, args);

            try
            {
                var result = targetMethod.Invoke(_decorated, args);
                OnInvoked(targetMethod, args, result);
                return result;
            }
            catch (TargetInvocationException exc)
            {
                OnException(targetMethod, args, exc);
                throw exc.InnerException;
            }
        }

        protected virtual void OnException(MethodInfo methodInfo, object[] args, Exception exception)
        { }

        protected virtual void OnInvoked(MethodInfo methodInfo, object[] args, object result)
        { }

        protected virtual void OnInvoking(MethodInfo methodInfo, object[] args)
        { }
    }
}