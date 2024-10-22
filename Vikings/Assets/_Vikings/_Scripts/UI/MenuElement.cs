﻿using System;
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
        [SerializeField] private TMP_Text _requiredLevelText;
        [SerializeField] private Image _requiredImage;
        [SerializeField] private GameObject _requiredPanel;
        

        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _defaultSprite;
        

        public void UpdateUI(AbstractBuilding abstractBuilding)
        {
            var config = abstractBuilding.GetData();
            
            priority = config.priorityInMenu;
            _name.text = config.nameText;
            _description.text = config.description;
            _icon.sprite = config.icon;

            _level.text = $"lvl. {abstractBuilding.CurrentLevel.Value + 1}";

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

            _level.text = $"lvl. {weapon.Level.Value + 1}";

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


        public void SetEnable(bool isEnable, int requiredLevel, Sprite requiredSprite)
        {
            if (isEnable)
            {
                _requiredPanel.gameObject.SetActive(false);
                _upgradeBtn.interactable = true;
                _upgradeBtn.image.sprite = _activeSprite;
            }
            else
            {
                _requiredPanel.gameObject.SetActive(true);
                _requiredLevelText.text = requiredLevel.ToString();
                _requiredImage.sprite = requiredSprite;
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