using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class GameManager : Singleton<GameManager>
    {
        // Префаб корабля, позиция его создания
        // и текущий объект корабля
        [Header("Ship")]
        [SerializeField] private ShipWeaponry shipPrefab;
        [SerializeField] private Transform shipStartPosition;

        // Префаб космической станции, позиция ее создания
        // и текущий объект станции
        [Header("Space Station")]
        [SerializeField] private GameObject spaceStationPrefab;
        [SerializeField] private Transform spaceStationStartPosition;

        // Контейнеры для разных групп элементов
        // пользовательского интерфейса
        [Header("UI")]
        [SerializeField] private List<GameStateUI> gameStatesUI;

        // Предупреждающая рамка, которая появляется,
        // когда игрок пересекает границу
        [SerializeField] private GameObject warningUI;

        // Сценарий, управляющий главной камерой
        [Header("Other")]
        [SerializeField] private SmoothFollow cameraFollow;

        // Границы игры
        [SerializeField] private Boundary boundary;

        // Система создания астероидов
        [SerializeField] private AsteroidSpawner asteroidSpawner;

        private ShipWeaponry _currentShip;
        private GameObject _currentSpaceStation;
        private GameState _currentState;

        // Отображает главное меню в момент запуска игры
        void Start()
        {
            ShowMainMenu();
        }

        public void Update()
        {
            if (_currentState is not GameState.InGame)
                return;

            // Если корабль вышел за границу сферы уничтожения,
            // завершить игру. Если он внутри сферы уничтожения, но
            // за границами сферы предупреждения, показать предупреждающую
            // рамку. Если он внутри обеих сфер, скрыть рамку.
            float distance = (_currentShip.transform.position - _currentSpaceStation.transform.position).magnitude;

            if (distance > boundary.destroyRadius)
            {
                // Корабль за пределами сферы уничтожения,
                // завершить игру
                GameOver();
            }
            else
            {
                warningUI.SetActive(distance > boundary.warningRadius);
            }
        }

        // Отображает заданный контейнер с элементами пользовательского
        // интерфейса и скрывает все остальные.
        void SetState(GameState state)
        {
            _currentState = state;
            foreach (var gameStateUI in gameStatesUI)
                gameStateUI.UI.SetActive(gameStateUI.State == state);
        }

        public void ShowMainMenu()
        {
            SetState(GameState.MainMenu);

            // Возобновить ход времени
            Time.timeScale = 1.0f;

            // Если корабль уже есть, удалить его
            if (_currentShip != null)
                Destroy(_currentShip);

            // То же для станции
            if (_currentSpaceStation != null)
                Destroy(_currentSpaceStation);

            // Запретить создавать астероиды
            asteroidSpawner.spawnAsteroids = false;

            // и удалить все уже созданные астероиды
            asteroidSpawner.DestroyAllAsteroids();
        }

        public void ShipChooseMenu()
        {
            SetState(GameState.ShipChoosing);
        }

        public void StartWithLaserBeam() => StartGame(WeaponType.LaserBeam);
        public void StartWithRapidFire() => StartGame(WeaponType.RapidFire);
        
        public void StartGame(WeaponType weaponType)
        {
            // Вывести интерфейс игры
            SetState(GameState.InGame);

            // Создать новый корабль и поместить
            // его в начальную позицию
            _currentShip = Instantiate(shipPrefab,
                shipStartPosition.position,
                shipStartPosition.rotation);
            
            _currentShip.EquipWeapons(weaponType);

            // То же для станции
            _currentSpaceStation = Instantiate(spaceStationPrefab,
                spaceStationStartPosition.position,
                spaceStationStartPosition.rotation);

            // Передать сценарию управления камерой ссылку на
            // новый корабль, за которым она должна следовать
            cameraFollow.target = _currentShip.transform;

            // Начать создавать астероиды
            asteroidSpawner.spawnAsteroids = true;

            // Сообщить системе создания астероидов
            // позицию новой станции
            asteroidSpawner.target = _currentSpaceStation.transform;
        }

        // Вызывается объектами, завершающими игру
        public void GameOver()
        {
            // Показать меню завершения игры
            SetState(GameState.GameOver);

            // Удалить корабль и станцию
            if (_currentShip != null)
                Destroy(_currentShip.gameObject);
            if (_currentSpaceStation != null)
                Destroy(_currentSpaceStation.gameObject);

            // Скрыть предупреждающую рамку, если она видима
            warningUI.SetActive(false);

            // Прекратить создавать астероиды
            asteroidSpawner.spawnAsteroids = false;

            // и удалить все уже созданные астероиды
            asteroidSpawner.DestroyAllAsteroids();
        }

        // Вызывается в ответ на касание кнопки Pause или Unpause
        public void SetPaused(bool paused)
        {
            SetState(paused ? GameState.GamePause : GameState.InGame);
            Time.timeScale = paused ? 0.0f : 1.0f;
        }
    }
}