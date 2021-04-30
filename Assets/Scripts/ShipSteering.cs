using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSteering : MonoBehaviour {
    
    // Скорость поворота корабля
    public float turnRate = 90.0f;
    
    // Сила выравнивания корабля
    public float levelDamping = 1.0f;
    
    // Сила поворота корабля
    float rotationDamping = 1.0f;

    private void Start()
    {
        if (turnRate >= 90f)
        {
            rotationDamping = turnRate / 90f;
        }
    }

    void Update () 
    {
        // Создать новый поворот, умножив вектор направления джойстика
        // на turnRate, и ограничить величиной 90 % от половины круга.
        
        // Сначала получить ввод пользователя.
        var steeringInput
            = InputManager.instance.steering.delta;
        
        // Теперь создать вектор для вычисления поворота.
        var rotation = new Vector2();
        rotation.y = steeringInput.x;
        rotation.x = -steeringInput.y;
        
        var maxRotation = rotation.normalized * turnRate / rotationDamping;
        
        // Умножить на turnRate, чтобы получить величину поворота.
        rotation *= turnRate / rotationDamping;

        if (rotation.magnitude > maxRotation.magnitude)
        {
            rotation = maxRotation;
        }

        // И преобразовать радианы в кватернион поворота!
        var newRotation = transform.rotation * Quaternion.Euler(rotation);
        
        // Поворачиваем корабль в нужную сторону
        transform.rotation = Quaternion.Slerp(
            transform.rotation, newRotation,
            rotationDamping * Time.deltaTime);
        
        // Далее попытаться минимизировать поворот!
        
        // Сначала определить, какой была бы ориентация
        // в отсутствие вращения относительно оси Z
        var levelAngles = transform.eulerAngles;
        levelAngles.z = 0.0f;
        var levelOrientation = Quaternion.Euler(levelAngles);
        
        // Объединить текущую ориентацию с небольшой величиной
        // этой ориентации "без вращения"; когда это происходит
        // на протяжении нескольких кадров, объект медленно
        // выравнивается над поверхностью
        transform.rotation = Quaternion.Slerp(
            transform.rotation, levelOrientation,
            levelDamping * Time.deltaTime);
    }
}