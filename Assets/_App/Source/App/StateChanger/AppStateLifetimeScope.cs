using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall
{
    public abstract class AppStateLifetimeScope<TStateData> : LifetimeScope where TStateData : IStateData
    {
        private TStateData _data;

        public void SetData(TStateData data) => _data = data;

        protected sealed override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_data);
            ConfigureState(builder);
        }

        protected abstract void ConfigureState(IContainerBuilder builder);
    }
}