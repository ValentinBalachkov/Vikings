using System;
using System.Collections.Generic;
using _Vikings.WeaponObject;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vikings.Building;
using Vikings.Items;
using Vikings.Object;

namespace Vikings.UI
{
    public class MenuElement : MonoBehaviour
    {
        public int priority;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _upgradeBtn;

        [SerializeField] private List<MenuElementItem> _menuElementItems = new();
        
       
        [SerializeField] private TMP_Text _buttonDescription;
        [SerializeField] private TMP_Text _requiredText;

        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _defaultSprite;


        public void UpdateUI(string itemName, string description, int level, Sprite icon,
            ItemCount[] priceToUpgrades, int priority)
        {
            this.priority = priority;
            _name.text = itemName;
            _description.text = description;
            _level.text = level == 0 ? "lvl:1" : $"lvl:{level}";
            _icon.sprite = icon;
            // for (int i = 0; i < _priceForUpgradeImage.Length; i++)
            // {
            //     _priceForUpgradeImage[i].sprite = priceToUpgrades[i].itemData.icon;
            //     _priceForUpgradeCount[i].text = priceToUpgrades[i].count.ToString();
            // }
        }

        public void UpdateUI(AbstractBuilding abstractBuilding)
        {
            var config = abstractBuilding.GetData();
            
            priority = config.priorityInMenu;
            _name.text = config.nameText;
            _description.text = config.description;
            _icon.sprite = config.icon;

            _level.text = $"lvl:{abstractBuilding.CurrentLevel.Value + 1}";

            var priceToUpgrades = abstractBuilding.GetPriceForUpgrade();
            
            foreach (var item in _menuElementItems)
            {
                item.image.sprite = item.itemData.icon;
                item.count.text = priceToUpgrades[item.itemData.ResourceType].ToString();
            }

            SetButtonDescription(abstractBuilding.CurrentLevel.Value == 0);
        }
        
        public void UpdateUI(Weapon weapon)
        {
            var config = weapon.GetWeaponData();
            
            priority = config.priority;
            _name.text = config.nameText;
            _description.text = config.description;
            _icon.sprite = config.icon;

            _level.text = $"lvl:{weapon.Level.Value + 1}";

            var priceToUpgrades = weapon.PriceToBuy;

            foreach (var item in _menuElementItems)
            {
                item.image.sprite = item.itemData.icon;
                item.count.text = priceToUpgrades[item.itemData.ResourceType].ToString();
            }

            SetButtonDescription(weapon.Level.Value == 0);
        }

        public void SetButtonDescription(bool isCreate)
        {
            _buttonDescription.text = isCreate ? "Create" : "Upgrade";
        }


        public void SetEnable(bool isEnable, string requiredText)
        {
            if (isEnable)
            {
                _requiredText.gameObject.SetActive(false);
                _upgradeBtn.interactable = true;
                _upgradeBtn.image.sprite = _activeSprite;
            }
            else
            {
                _requiredText.gameObject.SetActive(true);
                _requiredText.text = requiredText;
                _upgradeBtn.interactable = false;
                _upgradeBtn.image.sprite = _defaultSprite;
            }
        }

        public void AddOnClickListener(UnityAction action)
        {
            _upgradeBtn.onClick.AddListener(action);
        }
    }

    [Serializable]
    public class MenuElementItem
    {
        public Image image;
        public TMP_Text count;
        public ItemData itemData;
    }
}