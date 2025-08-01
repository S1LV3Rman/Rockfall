﻿using UnityEngine;

namespace Scripts
{
    public class SmoothFollow : MonoBehaviour
    {
        // Целевой объект для следования
        public Transform target;
    
        // Высота камеры над целевым объектом
        public float height = 5.0f;
    
        // Расстояние до целевого объекта без учета высоты
        public float distance = 10.0f;
    
        // Скорость изменения в повороте и движении
        public float rotationDamping = 5.0f;
        public float movementDamping = 5.0f;
    
        // Вызывается для каждого кадра
        void LateUpdate()
        {
            // Выйти, если цель не определена
            if (!target)
                return;
        
            // Ориентируем объектив камеры в сторону,
            // куда направляется целевой объект
            transform.rotation = Quaternion.Lerp(transform.rotation,
                target.rotation,
                rotationDamping * Time.deltaTime);
        
            // Вычислить желаемое положение камеры
            var wantedPosition = target.position - 
                                 transform.rotation * (Vector3.forward * distance + Vector3.down * height);
        
            // Передвигаем камеру к желаемой позиции
            transform.position = Vector3.Lerp(
                transform.position, wantedPosition,
                movementDamping * Time.deltaTime);
        
        }
    }
}