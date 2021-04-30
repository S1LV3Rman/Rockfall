using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EngineMode { Hold, Acceleration, Deceleration }
public class ShipEngines : MonoBehaviour {
    
    // Стандартная скорость
    public float maxSpeed = 25.0f;
    
    // Скорость разгона
    public float acceleration = 0.5f;
    
    // Скорость торможения
    public float deceleration = 0.5f;
    
    // Текущая скорость
    public float currentSpeed = 0f;

    // Признак работы двигателей
    private bool enginesOn = false;

    // Текущий режим работы двигателя
    private EngineMode currentMode = EngineMode.Hold;

    public void Awake()
    {
        // Когда данный объект запускается, сообщить
        // диспетчеру ввода, использовать его
        // как текущий сценарий управления двигателем
        InputManager.instance.SetEngines(this);
    }

    // Вызывается при удалении объекта
    public void OnDestroy()
    {
        // Ничего не делать, если вызывается не в режиме игры
        if (Application.isPlaying && 
            InputManager.instance != null)
        {
            InputManager.instance
                .RemoveEngines(this);
        }
    }
    
    // Перемещает корабль вперед с постоянной скоростью
    void Update () 
    {
        var offset = Vector3.forward * (Time.deltaTime * currentSpeed);
        this.transform.Translate(offset);
    }
    
    // Вкл/выкл двигателя
    public void ToggleEngines()
    {
        // Переключаем признак работы двигателей
        enginesOn = !enginesOn;

        // Запускаем нужный режим работы
        if (enginesOn)
        {
            // Запустить сопрограмму ускорения
            StartCoroutine(AcceleratingEngines());
        }
        else
        {
            // Запустить сопрограмму торможения
            StartCoroutine(DeceleratingEngines());
        }
    }

    IEnumerator AcceleratingEngines()
    {
        // Меняем режим работы двигателя на ускорение
        currentMode = EngineMode.Acceleration;
        
        // Продолжать итерации, пока двигатели
        // не разовьют максимальную скорость
        while (currentMode == EngineMode.Acceleration &&
               currentSpeed < maxSpeed)
        {
            Accelerate();

            yield return new WaitForEndOfFrame();
        }

        // Если режим двигателя не менялся
        if (currentMode == EngineMode.Acceleration)
        {
            // Меняем режим работы двигателя на удержание
            currentMode = EngineMode.Hold;

            // Корректируем скорость, чтобы не было погрешности
            currentSpeed = maxSpeed;
        }
    }

    IEnumerator DeceleratingEngines()
    {
        // Меняем режим работы двигателя на торможение
        currentMode = EngineMode.Deceleration;
        
        // Продолжать итерации, пока двигатели не остановятся
        while (currentMode == EngineMode.Deceleration &&
               currentSpeed > 0f)
        {
            Decelerate();

            yield return new WaitForEndOfFrame();
        }

        // Если режим двигателя не менялся
        if (currentMode == EngineMode.Deceleration)
        {
            // Меняем режим работы двигателя на удержание
            currentMode = EngineMode.Hold;

            // Корректируем скорость, чтобы не было погрешности
            currentSpeed = 0f;
        }
    }

    public void Accelerate()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed + 1f,
            acceleration * Time.deltaTime);
    }

    public void Decelerate()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, -1f,
            deceleration * Time.deltaTime);
    }
}