using Flappy.Core;
using System.Reflection;

namespace FlappyA.Implementations
{
    internal enum Lifecycle
    {
        Transient, Singleton
    }

    internal record DependencyInfo(Type instanceType, Lifecycle lifecycle, object? instance);

    internal class LocalDependencyResolver : IDependencyResolver
    {
        private Dictionary<Type, DependencyInfo> _depMap = new Dictionary<Type, DependencyInfo>();

        public void AddSingleton<TAbstraction, TImpl>() 
            where TImpl : class, TAbstraction
            where TAbstraction : class
        {
            bool didAdd = _depMap.TryAdd(
                typeof(TAbstraction),
                new DependencyInfo(typeof(TImpl), Lifecycle.Singleton, null)
            );

            if (didAdd == false)
            {
                throw new InvalidOperationException("Cannot register duplicate dependency.");
            }
        }

        public void AddSingleton<T>() where T : class
        {
            AddSingleton<T, T>();
        }

        public void AddSingleton<TAbstraction, TImpl>(TImpl impl) where TImpl : TAbstraction
        {
            if (impl is null) throw new ArgumentNullException(nameof(impl));

            bool didAdd = _depMap.TryAdd(
                typeof(TAbstraction),
                new DependencyInfo(typeof(TImpl), Lifecycle.Singleton, impl)
            );

            if (didAdd == false)
            {
                throw new InvalidOperationException("Cannot register duplicate dependency.");
            }
        }

        public void AddSingleton<T>(T impl) where T : class
        {
            AddSingleton<T, T>(impl);
        }

        public void AddTransient<TAbstraction, TImpl>() where TImpl : TAbstraction
        {
            bool didAdd = _depMap.TryAdd(
               typeof(TAbstraction),
               new DependencyInfo(typeof(TImpl), Lifecycle.Transient, null)
            );

            if (didAdd == false)
            {
                throw new InvalidOperationException("Cannot register duplicate dependency.");
            }
        }

        public void AddTransient<T>() where T : class 
        {
            AddTransient<T, T>();
        }

        public TAbstraction Resolve<TAbstraction>()
        {
            DependencyInfo? depInfo;
            bool isRegistered = _depMap.TryGetValue(typeof(TAbstraction), out depInfo);

            if (isRegistered == false)
            {
                throw new InvalidOperationException("Missing dependency registraion");
            }

            if(depInfo!.lifecycle == Lifecycle.Singleton)
            {
                if (depInfo!.instance != null)
                    return (TAbstraction)depInfo!.instance;

                return (TAbstraction)CreateSingletonInstance<TAbstraction>(depInfo!);
            }

            return (TAbstraction)CreateTransientInstance<TAbstraction>(depInfo);
        }

        private object CreateSingletonInstance<TAbstraction>(DependencyInfo depInfo)
        {
            if(depInfo.instance != null)
            {
                return depInfo.instance;
            }

            object instance = ResolveInternal(depInfo.instanceType);

            _depMap[typeof(TAbstraction)] = new DependencyInfo(depInfo.instanceType, depInfo.lifecycle, instance);

            return instance;
        }

        private object CreateTransientInstance<TAbstraction>(DependencyInfo depInfo)
        {
            object instance = ResolveInternal(depInfo.instanceType);
            return instance;
        }

        private object ResolveInternal(Type registration)
        {
            ParameterInfo[]? paramInfo = registration.GetConstructors().FirstOrDefault()?.GetParameters();
            if(paramInfo == null)
            {
                DependencyInfo depInfo = _depMap[registration];
                object instance = (depInfo.instance ?? Activator.CreateInstance(depInfo.instanceType))!;

                if (depInfo.lifecycle == Lifecycle.Singleton)
                {
                    _depMap[registration] = new DependencyInfo(depInfo.instanceType, Lifecycle.Singleton, instance);
                }

                return instance;
            }

            List<object> paramInstances = new();
            foreach (var parameter in paramInfo)
            {
                Type paramType = parameter.ParameterType;
                object? paramInstance = null;

                if (paramType.IsClass)
                {
                    try
                    {
                        paramInstance = Activator.CreateInstance(paramType);
                    }
                    catch (System.MissingMethodException) { }
                }

                if(paramInstance == null)
                {
                    paramInstance = ResolveInternal(paramType);
                }

                paramInstances.Add(paramInstance);
            }

            return Activator.CreateInstance(registration, args: paramInstances.ToArray())!;
        }
    }
}
