using System.Collections.Generic;
using System.Linq;
using S1LV3Rman.RockFall.App;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    public class GameManager : Singleton<GameManager>
    {

        // Система создания астероидов
        [SerializeField] private AsteroidSpawner asteroidSpawner;

        private ShipWeaponry _currentShip;
        private GameObject _currentSpaceStation;

        // Отображает главное меню в момент запуска игры
        void Start()
        {
            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            // Если корабль уже есть, удалить его
            if (_currentShip != null)
                Destroy(_currentShip);

            // То же для станции
            if (_currentSpaceStation != null)
                Destroy(_currentSpaceStation);

            // Запретить создавать астероиды
            asteroidSpawner.StopLaunching();

            // и удалить все уже созданные астероиды
            asteroidSpawner.DestroyAllAsteroids();
        }


        // public void StartWithLaserBeam() => StartGame(WeaponType.LaserBeam);
        // public void StartWithRapidFire() => StartGame(WeaponType.RapidFire);
        //
        // public void StartGame(WeaponType weaponType)
        // {
        //     // Создать новый корабль и поместить
        //     // его в начальную позицию
        //     _currentShip = Instantiate(shipPrefab,
        //         shipStartPosition.position,
        //         shipStartPosition.rotation);
        //     
        //     _currentShip.EquipWeapons(weaponType);
        //     _currentShip.GetAimTargets = GetAimTargetsForShip;
        //
        //     // То же для станции
        //     _currentSpaceStation = Instantiate(spaceStationPrefab,
        //         spaceStationStartPosition.position,
        //         spaceStationStartPosition.rotation);
        //
        //     // Начать создавать астероиды
        //     asteroidSpawner.StartLaunchingAt(_currentSpaceStation.transform);
        // }

        private List<Transform> GetAimTargetsForShip()
        {
            var targets = asteroidSpawner.ExistingAsteroids.Select(asteroid => asteroid.transform).ToList();
            targets.Add(_currentSpaceStation.transform);
            return targets;
        }

        // Вызывается объектами, завершающими игру
        public void GameOver()
        {
            // Удалить корабль и станцию
            if (_currentShip != null)
                Destroy(_currentShip.gameObject);
            if (_currentSpaceStation != null)
                Destroy(_currentSpaceStation.gameObject);

            // Прекратить создавать астероиды
            asteroidSpawner.StopLaunching();

            // и удалить все уже созданные астероиды
            asteroidSpawner.DestroyAllAsteroids();
        }
    }
}