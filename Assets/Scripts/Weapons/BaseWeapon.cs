using UnityEngine;

namespace Scripts
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public void Awake()
        {
            // Когда данный объект запускается, сообщить
            // диспетчеру ввода, использовать его
            // как текущий сценарий управления оружием
            InputManager.Instance.AddWeapon(this);
        }

        // Вызывается при удалении объекта
        public void OnDestroy()
        {
            if (InputManager.Instance != null) 
                InputManager.Instance.RemoveWeapon(this);
        }

        // Вызывается, чтобы начать огонь
        public abstract void StartFiring();

        // Вызывается, когда прекращается огонь
        public abstract void StopFiring();
    }
}
