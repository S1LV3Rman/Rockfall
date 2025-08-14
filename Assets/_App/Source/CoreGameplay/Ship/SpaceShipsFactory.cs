using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class SpaceShipsFactory : BaseFactory
    {
        private readonly SpaceShipsConfig _shipsConfig;
        private readonly SpaceShipsPool _pool;
        private readonly Transform _world;
        private readonly Transform _startingPoint;
        private readonly WeaponsConfig _weaponsConfig;

        public SpaceShipsFactory(
            SpaceShipsConfig shipsConfig,
            WeaponsConfig weaponsConfig,
            SpaceShipsPool pool,
            [Key("World")] Transform world,
            [Key("SpaceShip")] Transform startingPoint,
            LifetimeScope lifetimeScope
        ) : base(lifetimeScope)
        {
            _shipsConfig = shipsConfig;
            _pool = pool;
            _world = world;
            _startingPoint = startingPoint;
            _weaponsConfig = weaponsConfig;
        }

        protected override void Installation(IContainerBuilder builder)
        {
        }

        public SpaceShip CreateBasicShip(WeaponType weaponType)
        {
            var ship = Container.Instantiate(
                _shipsConfig.BasicShip, _startingPoint.position, _startingPoint.rotation, _world);

            if (!_weaponsConfig.Weapons.TryGetValue(weaponType, out var weaponPrefab))
                throw new KeyNotFoundException(
                    $"Weapon type '{weaponType}' not found in weapons config.");

            foreach (var weaponSlot in ship.Weaponry.WeaponSlots)
            {
                var weapon = Container.Instantiate(weaponPrefab, weaponSlot);
                ship.Weaponry.EquipWeapon(weapon);
            }

            _pool.Add(ship);
            return ship;
        }
    }
}