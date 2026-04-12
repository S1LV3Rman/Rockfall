using R3;
using S1LV3Rman.RockFall.CoreGameplay;
using UnityEngine;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall.MainMenu
{
    public class WeaponSelectionUI : DefaultUIPanel
    {
        [SerializeField] private Button _laserWeapon;
        [SerializeField] private Button _rapidWeapon;

        public Observable<string> WeaponSelection { get; private set; }

        protected override void Awake()
        {
            WeaponSelection = Observable.Merge(
                _laserWeapon.OnClickAsObservable().Select(_ => WeaponTypes.LaserWeapon),
                _rapidWeapon.OnClickAsObservable().Select(_ => WeaponTypes.ProjectileWeapon));
        }
    }
}