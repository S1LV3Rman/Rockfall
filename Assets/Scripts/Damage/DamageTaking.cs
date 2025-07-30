using UnityEngine;

namespace Scripts
{
    public class DamageTaking : MonoBehaviour
    {
        private const float DAMAGE_DELAY = 0.001f;
        
        [SerializeField] private int hitPoints = 10;
        [SerializeField] private GameObject destructionPrefab;
        [SerializeField] private bool gameOverOnDestroyed = false;

        public int CurrentHealth { get; private set; }
        public int MaxHealth => hitPoints;

        private float _lastDamageTime;

        private void Awake()
        {
            CurrentHealth = hitPoints;
        }

        public void TakeDamage(int amount)
        {
            var damageTime = Time.time;
            if (damageTime - _lastDamageTime < DAMAGE_DELAY)
                return;
            
            CurrentHealth -= amount;
            
            DamageNumbersManager.Instance.Create(transform.position, amount);

            if (CurrentHealth > 0)
                return;
            
            Destroy(gameObject);

            if (destructionPrefab != null) 
                Instantiate(destructionPrefab, transform.position, transform.rotation);

            if (gameOverOnDestroyed) 
                GameManager.Instance.GameOver();
        }
    }
}