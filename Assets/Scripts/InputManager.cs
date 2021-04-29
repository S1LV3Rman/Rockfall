using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    // Время ожидания в сопрограммах при ошибках
    public float defaultPause = 0.5f;
    
    // Джойстик, используемый для управления кораблем.
    public VirtualJoystick steering;

    // Текущий сценарий ShipWeapons управления стрельбой.
    private ShipWeapons currentWeapons;

    // Текущий сценарий ShipEngines управления двигателями.
    private ShipEngines currentEngines;

    // Вызывается сценарием ShipWeapons для обновления
    // переменной currentWeapons.
    public void SetWeapons(ShipWeapons weapons)
    {
        this.currentWeapons = weapons;
    }

    // Аналогично; вызывается для сброса
    // переменной currentWeapons.
    public void RemoveWeapons(ShipWeapons weapons)
    {
        // Если currentWeapons ссылается на данный объект 'weapons',
        // присвоить ей null.
        if (this.currentWeapons == weapons)
        {
            this.currentWeapons = null;
        }
    }

    // Вызывается, когда пользователь касается кнопки Fire.
    public void StartFiring()
    {
        // Если оружие подключено
        if (currentWeapons != null)
            // Начинаем огонь
            currentWeapons.StartFiring();
    }

    // Вызывается, когда пользователь убирает палец с кнопки Fire
    public void StopFiring()
    {
        // Если оружие подключено
        if (currentWeapons != null)
            // Прекращаем огонь
            currentWeapons.StopFiring();
    }

    // Вызывается сценарием ShipEngines для обновления
    // переменной currentEngines.
    public void SetEngines(ShipEngines engines)
    {
        this.currentEngines = engines;
    }

    // Аналогично; вызывается для сброса
    // переменной currentEngines.
    public void RemoveEngines(ShipEngines engines)
    {
        // Если currentEngines ссылается на данный объект 'engines',
        // присвоить ей null.
        if (this.currentEngines == engines)
        {
            this.currentEngines = null;
        }
    }
    
    // Вызывается, когда пользователь нажимает кнопку Engine.
    public void ToggleEngines()
    {
        // Если двигатель подключен
        if (currentEngines != null)
            // Переключаем режим работы двигателя
            currentEngines.ToggleEngines();
    }
}