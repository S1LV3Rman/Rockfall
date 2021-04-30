using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    // Список пушек для стрельбы
    public Transform[] firePoints;

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
        if (Application.isPlaying && 
            InputManager.instance != null)
        {
            InputManager.instance
                .RemoveWeapons(this);
        }
    }

    // Вызывается, чтобы начать огонь
    public abstract void StartFiring();

    // Вызывается, когда прекращается огонь
    public abstract void StopFiring();
}
