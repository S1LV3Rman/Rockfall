using System;
using UnityEngine;

namespace S1LV3Rman.RockFall.CoreGameplay
{
    [Serializable]
    public struct IndicatorData
    {
        public string Name;
        public Color Color;
        public Sprite Image;
        public float Size;
        public Color HealthColor;
        public Sprite HealthImage;
    }
}