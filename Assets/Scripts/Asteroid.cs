using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    
    // Скорость перемещения астероида.
    public float speed = 10.0f;

    void Start()
    {

        // установить скорость перемещения твердого тела
        GetComponent<Rigidbody>().velocity
            = transform.forward * speed;

        // Создать красный индикатор для данного астероида
        var indicator = IndicatorManager.instance
            .AddIndicator(gameObject, Color.red);
        
        // Запомнить координаты космической станции,
        // управляемой диспетчером игры,
        // для отображения расстояния от нее до астероида
        indicator.showDistanceTo =
            GameManager.instance.currentSpaceStation.transform;
    }
}