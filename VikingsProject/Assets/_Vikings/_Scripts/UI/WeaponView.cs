using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Weapon;

namespace Vikings.UI
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _collectTimeText;
        
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private WeaponData _weaponData;

        [SerializeField] private Button _upgradeBtn;
        
        private void Awake()
        {
            _weaponData.OnUpgrade += UpdateUI;
        }

        private void Start()
        {
            _upgradeBtn.onClick.AddListener(_weaponController.UpgradeCurrentWeapon);
        }

        private void UpdateUI(int level, float collectTime)
        {
            _levelText.text = $"Weapon level: {level}";
            _collectTimeText.text = $"Collect time: {collectTime}";
        }
    }
}