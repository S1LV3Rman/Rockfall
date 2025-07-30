using UnityEngine;

namespace Scripts
{
    public class AsteroidSpawner : MonoBehaviour
    {
        // Радиус сферы, на поверхности которой создаются астероиды
        public float radius = 250.0f;

        // Шаблон для создания астероидов
        public Asteroid asteroidPrefab;

        // Ждать spawnRate ± variance секунд перед созданием нового астероида
        public float spawnRate = 5.0f;
        public float variance = 1.0f;

        // Объект, служащий целью для астероидов
        public Transform target;

        // Значение false запрещает создавать астероиды
        public bool spawnAsteroids = false;

        private float _nextSpawnTime;

        private void Start()
        {
            _nextSpawnTime = Time.time;
        }

        private void Update()
        {
            if (!spawnAsteroids)
                return;
            
            if (_nextSpawnTime <= Time.time)
            {
                CreateNewAsteroid();
                _nextSpawnTime += spawnRate + Random.Range(-variance, variance);
            }
        }

        void CreateNewAsteroid()
        {
            // Если создавать астероиды запрещено, выйти
            if (spawnAsteroids == false)
                return;

            // Выбрать случайную точку на поверхности сферы
            var asteroidPosition = Random.onUnitSphere * radius;

            // Масштабировать в соответствии с объектом
            asteroidPosition.Scale(transform.lossyScale);

            // И добавить смещение объекта, порождающего астероиды
            asteroidPosition += transform.position;

            // Создать новый астероид
            var newAsteroid = Instantiate(asteroidPrefab);

            // Поместить его в только что вычисленную точку
            newAsteroid.transform.position = asteroidPosition;
            
            newAsteroid.SetTarget(target);
        }

        // Вызывается редактором, когда выбирается объект,
        // порождающий астероиды.
        void OnDrawGizmosSelected()
        {
            // Установить желтый цвет
            Gizmos.color = Color.yellow;

            // Сообщить визуализатору Gizmos, что тот должен использовать
            // текущие позицию и масштаб
            Gizmos.matrix = transform.localToWorldMatrix;

            // Нарисовать сферу, представляющую собой область создания астероидов
            Gizmos.DrawWireSphere(Vector3.zero, radius);
        }

        public void DestroyAllAsteroids()
        {
            // Удалить все имеющиеся в игре астероиды
            foreach (var asteroid in FindObjectsByType<Asteroid>(FindObjectsInactive.Include, FindObjectsSortMode.None)) 
                Destroy(asteroid.gameObject);
        }
    }
}