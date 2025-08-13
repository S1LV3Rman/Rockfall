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

        public Observable<WeaponType> WeaponSelection { get; private set; }

        protected override void Awake()
        {
            WeaponSelection = Observable.Merge(
                _laserWeapon.OnClickAsObservable().Select(_ => WeaponType.LaserBeam),
                _rapidWeapon.OnClickAsObservable().Select(_ => WeaponType.RapidFire));
        }
    }
}