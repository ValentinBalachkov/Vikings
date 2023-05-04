using System;
using System.Collections.Generic;
using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.Weapon
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData", order = 3)]
    public class WeaponData : ScriptableObject, IData
    {
        public bool isOpenForCrafting;
        public Sprite icon;
        public string nameText;
        public string description;
        
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                _isOpen = value;
                if (_isOpen)
                {
                    OnOpen?.Invoke(this);
                }
            }
        }

        public Action<WeaponData> OnOpen;

        public ItemData ItemData => _itemData;
        public int CraftingTime => _craftingTime;

        public List<PriceToUpgrade> PriceToBuy => _priceToBuy;

        [SerializeField] private ItemData _itemData;
        [SerializeField] private bool _isOpen;
        [SerializeField] private List<PriceToUpgrade> _priceToBuy = new();
        [SerializeField] private int _craftingTime;
        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as WeaponData;
            if (data != null)
            {
                _isOpen = data._isOpen;
            }
        }
    }
}