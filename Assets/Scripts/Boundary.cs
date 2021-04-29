using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {
    
    // Показывает предупреждающую рамку, когда игрок
    // улетает слишком далеко от центра
    public float warningRadius = 400.0f;
    
    // Расстояние от центра, удаление на которое вызывает завершение игры
    public float destroyRadius = 450.0f;
    
    public void OnDrawGizmosSelected() {
        
        // Желтым цветом показать сферу предупреждения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,
            warningRadius);
        
        // ...а красным — сферу уничтожения
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,
            destroyRadius);
    }
}
