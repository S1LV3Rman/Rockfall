using System;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public abstract class Factory : IDisposable
    {
        private readonly LifetimeScope _factoryLifetime;

        protected Factory(
            LifetimeScope lifetimeScope
        )
        {
            _factoryLifetime = lifetimeScope.CreateChild(Installation);
        }

        protected IObjectResolver Container => _factoryLifetime.Container;

        protected abstract void Installation(IContainerBuilder builder);

        public virtual void Dispose()
        {
            _factoryLifetime.Dispose();
        }
    }
}