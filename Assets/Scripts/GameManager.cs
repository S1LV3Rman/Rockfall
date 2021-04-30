using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    // Шаблон корабля, позиция его создания
    // и текущий объект корабля
    public GameObject[] shipPrefabs;
    public Transform shipStartPosition;
    public GameObject currentShip { get; private set; }

    // Шаблон космической станции, позиция ее создания
    // и текущий объект станции
    public GameObject spaceStationPrefab;
    public Transform spaceStationStartPosition;
    public GameObject currentSpaceStation { get; private set; }

    // Сценарий, управляющий главной камерой
    public SmoothFollow cameraFollow;

    // Границы игры
    public Boundary boundary;

    // Контейнеры для разных групп элементов
    // пользовательского интерфейса
    public GameObject inGameUI;
    public GameObject pausedUI;
    public GameObject gameOverUI;
    public GameObject mainMenuUI;
    public GameObject ShipChooseMenuUI;

    // Предупреждающая рамка, которая появляется,
    // когда игрок пересекает границу
    public GameObject warningUI;

    // Игра находится в состоянии проигрывания?
    public bool gameIsPlaying { get; private set; }

    // Система создания астероидов
    public AsteroidSpawner asteroidSpawner;

    // Отображает главное меню в момент запуска игры
    void Start()
    {
        ShowMainMenu();
    }

    // Отображает заданный контейнер с элементами пользовательского
    // интерфейса и скрывает все остальные.
    void ShowUI(GameObject newUI)
    {

        // Создать список всех контейнеров.
        GameObject[] allUI
            = { inGameUI, pausedUI, gameOverUI, mainMenuUI, ShipChooseMenuUI };

        // Скрыть их все.
        foreach (GameObject UIToHide in allUI)
        {
            UIToHide.SetActive(false);
        }

        // И затем отобразить указанный.
        newUI.SetActive(true);
    }

    public void ShowMainMenu()
    {
        ShowUI(mainMenuUI);

        // Когда игра запускается, она находится не в состоянии проигрывания
        gameIsPlaying = false;
        
        // Возобновить ход времени
        Time.timeScale = 1.0f;

        // Если корабль уже есть, удалить его
        if (currentShip != null)
        {
            Destroy(currentShip);
        }

        // То же для станции
        if (currentSpaceStation != null)
        {
            Destroy(currentSpaceStation);
        }

        // Запретить создавать астероиды
        asteroidSpawner.spawnAsteroids = false;

        // и удалить все уже созданные астероиды
        asteroidSpawner.DestroyAllAsteroids();
    }

    public void ShipChooseMenu()
    {
        ShowUI(ShipChooseMenuUI);
    }

    // Вызывается в ответ на касание кнопки New Game
    public void StartGame(int shipType)
    {
        // Вывести интерфейс игры
        ShowUI(inGameUI);

        // Перейти в режим игры
        gameIsPlaying = true;

        // Если корабль уже есть, удалить его
        if (currentShip != null)
        {
            Destroy(currentShip);
        }

        // То же для станции
        if (currentSpaceStation != null)
        {
            Destroy(currentSpaceStation);
        }

        // Создать новый корабль и поместить
        // его в начальную позицию
        currentShip = Instantiate(shipPrefabs[shipType]);
        currentShip.transform.position
            = shipStartPosition.position;
        currentShip.transform.rotation
            = shipStartPosition.rotation;

        // То же для станции
        currentSpaceStation = Instantiate(spaceStationPrefab);
        currentSpaceStation.transform.position =
            spaceStationStartPosition.position;
        currentSpaceStation.transform.rotation =
            spaceStationStartPosition.rotation;

        // Передать сценарию управления камерой ссылку на
        // новый корабль, за которым она должна следовать
        cameraFollow.target = currentShip.transform;

        // Начать создавать астероиды
        asteroidSpawner.spawnAsteroids = true;

        // Сообщить системе создания астероидов
        // позицию новой станции
        asteroidSpawner.target = currentSpaceStation.transform;
    }

    // Вызывается объектами, завершающими игру
    public void GameOver()
    {
        // Показать меню завершения игры
        ShowUI(gameOverUI);

        // Выйти из режима игры
        gameIsPlaying = false;

        // Удалить корабль и станцию
        if (currentShip != null)
            Destroy(currentShip);
        if (currentSpaceStation != null)
            Destroy(currentSpaceStation);

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

        // Переключиться между интерфейсами паузы и игры
        inGameUI.SetActive(!paused);
        pausedUI.SetActive(paused);

        // Если игра приостановлена...
        if (paused)
        {
            // Остановить время
            Time.timeScale = 0.0f;
        }
        else
        {
            // Возобновить ход времени
            Time.timeScale = 1.0f;
        }
    }

    public void Update()
    {
        // Если корабля нет, выйти
        if (currentShip == null)
            return;

        // Если корабль вышел за границу сферы уничтожения,
        // завершить игру. Если он внутри сферы уничтожения, но
        // за границами сферы предупреждения, показать предупреждающую
        // рамку. Если он внутри обеих сфер, скрыть рамку.
        
        float distance =
            (currentShip.transform.position
             - boundary.transform.position)
            .magnitude;

        if (distance > boundary.destroyRadius)
        {
            // Корабль за пределами сферы уничтожения,
            // завершить игру
            GameOver();
        }
        else if (distance > boundary.warningRadius)
        {
            // Корабль за пределами сферы предупреждения,
            // показать предупреждающую рамку
            warningUI.SetActive(true);
        }
        else
        {
            // Корабль внутри сферы предупреждения,
            // скрыть рамку
            warningUI.SetActive(false);
        }

    }
}
