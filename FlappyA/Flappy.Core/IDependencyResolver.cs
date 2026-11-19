namespace Flappy.Core
{
    public interface IDependencyResolver
    {
        void AddSingleton<TAbstraction, TImpl>() 
            where TImpl : class, TAbstraction
            where TAbstraction : class;

        void AddSingleton<T>() where T : class;

        void AddSingleton<TAbstraction, TImpl>(TImpl impl) where TImpl : TAbstraction;
        void AddSingleton<T>(T impl) where T : class;
        void AddTransient<TAbstraction, TImpl>() where TImpl : TAbstraction;
        void AddTransient<T>() where T : class;
        TAbstraction Resolve<TAbstraction>();
    }
}
