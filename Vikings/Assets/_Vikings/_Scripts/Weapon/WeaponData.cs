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
        public string required;
        public int id;
        public int level;
        public int priority;
        
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
      
        public float CraftingTime
        {
            get
            {
                if (level == 0)
                {
                    return _craftingTime;
                }
                return (0.5f * Mathf.Pow(level + 1, 2)) + _craftingTime;
            }
        }

        public List<PriceToUpgrade> PriceToBuy
        {
            get
            {
                if (level == 0)
                {
                    return _priceToBuy;
                }

                List<PriceToUpgrade> newPrice = new();
                foreach (var price in _priceToBuy)
                {
                    var a = price.count - 1;
                    float p = 0;
                    for (int i = 2; i <= level + 1; i++)
                    {
                        p += Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3));
                        a = (int)p;
                    }
                        
                    newPrice.Add(new PriceToUpgrade
                    {
                        count = (int)p,
                        itemData = price.itemData
                    });
                }

                return newPrice;
            }
        }

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
                level = data.level;
            }
        }
    }
}