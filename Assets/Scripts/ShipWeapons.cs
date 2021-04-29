using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapons : MonoBehaviour
{
    // Задержка между выстрелами в секундах.
    public float fireDelay = 0.2f;

    // Шаблон для создания снарядов
    public GameObject shotPrefab;

    // Список пушек для стрельбы
    public Transform[] firePoints;

    // Индекс в firePoints, указывающий на следующую пушку
    private int firePointIndex;
    
    // Содержит true, если в данный момент ведется огонь.
    private bool isFiring = false;

    public void Awake()
    {
        // Когда данный объект запускается, сообщить
        // диспетчеру ввода, использовать его
        // как текущий сценарий управления оружием
        InputManager.instance.SetWeapons(this);
    }

    // Вызывается при удалении объекта
    public void OnDestroy()
    {
        // Ничего не делать, если вызывается не в режиме игры
        if (Application.isPlaying == true)
        {
            InputManager.instance
                .RemoveWeapons(this);
        }
    }

    // Вызывается, когда чтобы начать огонь
    public void StartFiring()
    {
        // Запустить сопрограмму ведения огня
        StartCoroutine(Firing());
    }

    // Вызывается, когда прекращается огонь
    public void StopFiring()
    {
        // Присвоить false, чтобы завершить цикл в Firing
        isFiring = false;
    }

    IEnumerator Firing()
    {
        // Установить признак ведения огня
        isFiring = true;

        // Продолжать итерации, пока isFiring равна true
        while (isFiring)
        {
            Fire();

            // Ждать fireDelay секунд перед
            // следующим выстрелом
            yield return new WaitForSeconds(fireDelay);
        }
    }

    // Вызывается при каждом выстреле
    void Fire()
    {
        // Если пушки отсутствуют, выйти
        if (firePoints.Length == 0)
            return;

        // Определить следующую пушку для выстрела
        var firePointToUse = firePoints[firePointIndex];

        // Создать новый снаряд с ориентацией,
        // соответствующей пушке
        Instantiate(shotPrefab,
            firePointToUse.position,
            firePointToUse.rotation);

        // Если пушка имеет компонент источника звука,
        // воспроизвести звуковой эффект
        var audio = firePointToUse.GetComponent<AudioSource>();
        if (audio)
        {
            audio.Play();
        }

        // Перейти к следующей пушке
        firePointIndex++;

        // Если произошел выход за границы массива,
        // вернуться к его началу
        if (firePointIndex >= firePoints.Length)
            firePointIndex = 0;
    }
}