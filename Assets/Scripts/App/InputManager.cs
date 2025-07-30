using System.Collections.Generic;

namespace Scripts
{
    public class InputManager : Singleton<InputManager>
    {
        // Время ожидания в сопрограммах при ошибках
        public float defaultPause = 0.5f;

        // Джойстик, используемый для управления кораблем.
        public VirtualJoystick steering;

        // Текущий сценарий ShipWeapons управления стрельбой.
        private List<BaseWeapon> _currentWeapons = new();

        // Текущий сценарий ShipEngines управления двигателями.
        private ShipEngines _currentEngines;

        // Вызывается сценарием ShipWeapons для обновления
        // переменной currentWeapons.
        public void AddWeapon(BaseWeapon weapon) => _currentWeapons.Add(weapon);

        public void RemoveWeapon(BaseWeapon weapon) => _currentWeapons.Remove(weapon);

        // Вызывается, когда пользователь касается кнопки Fire.
        public void StartFiring()
        {
            foreach (var weapon in _currentWeapons)
                weapon.StartFiring();
        }

        // Вызывается, когда пользователь убирает палец с кнопки Fire
        public void StopFiring()
        {
            foreach (var weapon in _currentWeapons)
                weapon.StopFiring();
        }

        // Вызывается сценарием ShipEngines для обновления
        // переменной currentEngines.
        public void SetEngines(ShipEngines engines)
        {
            _currentEngines = engines;
        }

        // Аналогично; вызывается для сброса
        // переменной currentEngines.
        public void RemoveEngines(ShipEngines engines)
        {
            // Если currentEngines ссылается на данный объект 'engines',
            // присвоить ей null.
            if (_currentEngines == engines)
                _currentEngines = null;
        }

        // Вызывается, когда пользователь нажимает кнопку Engine.
        public void ToggleEngines()
        {
            // Если двигатель подключен
            if (_currentEngines != null)
                // Переключаем режим работы двигателя
                _currentEngines.ToggleEngines();
        }
    }
}