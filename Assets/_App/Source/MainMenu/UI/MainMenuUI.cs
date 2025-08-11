using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace S1LV3Rman.RockFall
{
    public class MainMenuUI : UIBehaviour
    {
        [Header("Title")]
        [SerializeField] private GameObject _title;
        [SerializeField] private Button _start;

        [Header("Weapon Selection")]
        [SerializeField] private GameObject _weaponSelection;
        [SerializeField] private Button _laserWeapon;
        [SerializeField] private Button _rapidWeapon;

        public event Action<WeaponType> OnWeaponSelected;

        protected override void Awake()
        {
            _start.onClick.AddListener(ChooseWeapon);
            _laserWeapon.onClick.AddListener(StartWithLaser);
            _rapidWeapon.onClick.AddListener(StartWithRapid);
            
            _title.SetActive(true);
            _weaponSelection.SetActive(false);
        }

        private void StartWithLaser() => OnWeaponSelected?.Invoke(WeaponType.LaserBeam);
        private void StartWithRapid() => OnWeaponSelected?.Invoke(WeaponType.RapidFire);

        private void ChooseWeapon()
        {
            _title.SetActive(false);
            _weaponSelection.SetActive(true);
        }
    }
}