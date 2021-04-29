using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Blinking : MonoBehaviour
{
    // Скорость изменения прозрачности
    public float blinkDumping = 1f;
    
    // Изображение объекта
    private Image image;
    
    // Текущий цвет изображения
    private Color currentColor;

    // true: изображение затухает
    // false: изображение проявляется
    private bool isFading;
        
    // Start is called before the first frame update
    private void Start()
    {
        // Получаем компонент изображения
        image = GetComponent<Image>();
        
        // Получаем текущий цвет изображения
        currentColor = image.color;
    }
    
    // Делаем изображение полностью
    // непрозрачным при каждой активации
    void OnEnable()
    {
        // Изображение начитает затухать
        isFading = true;
        
        // Делаем цвет полность непрозрачным
        currentColor.a = 1f;
        
        // Устанавливаем получившийся цвет
        image.color = currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        // Проверяем, изображение затухает или проявляется
        if (isFading)
        {
            // Делаем изображение более прозрачнм
            currentColor.a -= blinkDumping * Time.deltaTime;
            
            // Запускаем проявление изображение,
            // если оно полность прозрачно
            if (currentColor.a <= 0f)
                isFading = false;
        }
        else
        {
            // Делаем изображение менее прозрачным
            currentColor.a += blinkDumping * Time.deltaTime;
            
            // Запускаем затухание изображение,
            // если оно полность непрозрачно
            if (currentColor.a >= 1f)
                isFading = true;
        }
        
        // Устанавливаем получившийся цвет
        image.color = currentColor;
    }
}
