using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public sealed class DamageNumbersFactory : Factory
    {
        private readonly DamageNumbersConfig _config;
        private readonly DamageNumbersPool _pool;
        private readonly RectTransform _uiContainer;
        private readonly GameplayCamera _camera;
        private readonly IRandomSource _random;

        public DamageNumbersFactory(
            DamageNumbersConfig config,
            DamageNumbersPool pool,
            [Key("DamageNumbers")] RectTransform uiContainer,
            GameplayCamera camera,
            RandomService randomService,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _config = config;
            _pool = pool;
            _uiContainer = uiContainer;
            _camera = camera;
            _random = randomService.GetRandomSource();
        }

        protected override void Installation(IContainerBuilder builder)
        {
        }

        public DamageNumber CreateDamageNumber(Vector3 position, int amount)
        {
            var deviation = _random.Float(-_config.HorizontalDeviation, _config.HorizontalDeviation);
            var lifetime = _config.LifetimePerAmount.Evaluate(amount);
            var size = _config.SizePerAmount.Evaluate(amount);
            var color = _config.ColorPerAmount.Evaluate(amount);

            var damageNumber = GetDamageNumber();
            damageNumber.Show(position, amount, deviation, lifetime, size,
                _config.DeathSizeDecrease, color,
                _config.FlyOverLifetime, _camera.Camera);
            return damageNumber;
        }

        private DamageNumber GetDamageNumber()
        {
            if (_pool.TryPull(out var damageNumber))
                return damageNumber;

            var newDamageNumber = Container.Instantiate(_config.Prefab, _uiContainer);
            _pool.Add(newDamageNumber);
            return newDamageNumber;
        }
    }
}