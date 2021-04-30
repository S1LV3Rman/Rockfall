using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaking : MonoBehaviour
{

    // Число очков прочности данного объекта
    public float hitPoints = 10;

    // При разрушении создать этот объект
    // в текущей позиции
    public GameObject destructionPrefab;

    // Завершить игру при разрушении данного объекта?
    public bool gameOverOnDestroyed = false;

    // Вызывается другими объектами (например, астероидами и шарами плазмы)
    // для нанесения повреждений
    public void TakeDamage(float amount)
    {

        // Сообщить о попадании в текущий объект
        //Debug.Log(gameObject.name + " damaged!");

        // Вычесть amount из числа очков прочности
        hitPoints -= amount;

        // Очки исчерпаны?
        if (hitPoints <= 0f)
        {

            // Зафиксировать этот факт
            //Debug.Log(gameObject.name + " destroyed!");

            // Удалить себя из игры
            Destroy(gameObject);

            // Задан шаблон для создания объекта в точке разрушения?
            if (destructionPrefab != null)
            {
                // Создать объект в текущей позиции
                // с текущей ориентацией.
                Instantiate(destructionPrefab,
                    transform.position, transform.rotation);
            }

            // Если требуется завершить игру, вызвать
            // метод GameOver класса GameManager.
            if (gameOverOnDestroyed == true)
            {
                GameManager.instance.GameOver();
            }
        }
    }
}