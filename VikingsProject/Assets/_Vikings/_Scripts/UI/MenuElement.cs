using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vikings.Building;

namespace Vikings.UI
{
    public class MenuElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _upgradeBtn;
        [SerializeField] private Image[] _priceForUpgradeImage;
        [SerializeField] private TMP_Text[] _priceForUpgradeCount;

        public void UpdateUI(string itemName, string description, int level, Sprite icon, PriceToUpgrade[] priceToUpgrades)
        {
            _name.text = itemName;
            _description.text = description;
          //  _level.text = $"lvl:{level}";
            _icon.sprite = icon;
            for (int i = 0; i < _priceForUpgradeImage.Length; i++)
            {
                _priceForUpgradeImage[i].sprite = priceToUpgrades[i].itemData.icon;
                _priceForUpgradeCount[i].text = priceToUpgrades[i].count.ToString();
            }
        }

        public void AddOnClickListener(UnityAction action)
        {
            _upgradeBtn.onClick.AddListener(action);
        }
    }
}