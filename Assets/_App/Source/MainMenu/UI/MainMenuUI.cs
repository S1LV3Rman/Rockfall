using System;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall
{
    public class MainMenuUI : UIBehaviour
    {
        [Header("Title")]
        [SerializeField] private GameObject _titleUI;

        [SerializeField] private Button _start;

        [Header("Weapon Selection")]
        [SerializeField] private GameObject _weaponSelectionUI;

        [SerializeField] private Button _laserWeapon;
        [SerializeField] private Button _rapidWeapon;

        public Observable<WeaponType> WeaponSelection { get; private set; }

        protected override void Awake()
        {
            _start.onClick.AddListener(ChooseWeapon);
            WeaponSelection = Observable.Merge(
                _laserWeapon.OnClickAsObservable().Select(_ => WeaponType.LaserBeam),
                _rapidWeapon.OnClickAsObservable().Select(_ => WeaponType.RapidFire));

            _titleUI.SetActive(true);
            _weaponSelectionUI.SetActive(false);
        }

        private void ChooseWeapon()
        {
            _titleUI.SetActive(false);
            _weaponSelectionUI.SetActive(true);
        }
    }
}