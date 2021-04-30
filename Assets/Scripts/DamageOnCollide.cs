using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnCollide : MonoBehaviour {
    
    // Объем повреждений, наносимых объекту.
    public float damage = 1;
    
    // Объем повреждений, наносимых себе при попадании
    // в какой-то другой объект.
    public float damageToSelf = 5;
    
    // Уничтожать себя при столкновении?
    public bool selfDestroy = false;

    void HitObject(GameObject theObject)
    {
        // Нанести повреждение объекту, в который попал данный объект,
        // если возможно.
        var theirDamage =
            theObject.GetComponentInParent<DamageTaking>();
        if (theirDamage)
        {
            theirDamage.TakeDamage(damage);
        }

        if (selfDestroy)
        {
            // Удалить себя из игры
            Destroy(gameObject);
        }
        else
        {
            // Нанести повреждение себе, если возможно
            var ourDamage =
                this.GetComponentInParent<DamageTaking>();
            if (ourDamage)
            {
                ourDamage.TakeDamage(damageToSelf);
            }
        }
    }

    // Объект вошел в область действия данного триггера?
    void OnTriggerEnter(Collider collider) {
        HitObject(collider.gameObject);
    }
    
    // Другой объект столкнулся с текущим объектом?
    void OnCollisionEnter(Collision collision) {
        HitObject(collision.gameObject);
    }
}