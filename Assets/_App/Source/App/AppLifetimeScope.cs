using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public sealed class AppLifetimeScope : LifetimeScope
    {
        // [Header("Configs")]

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<RandomService>(Lifetime.Singleton);
            builder.Register<TimeService>(Lifetime.Singleton);
        }
    }
}