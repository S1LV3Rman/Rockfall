using UnityEngine;

namespace S1LV3Rman.RockFall
{
    public sealed class Cooldown : MonoBehaviour
    {
        [field: SerializeField] public float Duration { get; set; }
        [field: SerializeField] public float Remains { get; private set; }

        private void FixedUpdate() => Advance(Time.deltaTime);

        public void Advance(float time)
        {
            switch (Remains)
            {
                case > 0f:
                    Remains -= time;
                    break;
                case < 0f:
                    Remains = 0f;
                    break;
            }
        }

        public void Begin() => Remains += Duration;

        public void Refresh() => Remains = 0f;
    }
}