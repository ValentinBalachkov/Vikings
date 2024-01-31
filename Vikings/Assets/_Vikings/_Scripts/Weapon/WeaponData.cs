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
        public Sprite iconOfflineFarm;
        public string nameText;
        public string description;
        public string required;
        public int id;
        public int level
        {
            get => _level;
            set
            {
                _level = value;
                if (_level == 1 && _taskData != null)
                {
                    _taskData.accessDone = true;
                    if (_taskData.taskStatus == TaskStatus.InProcess)
                    {
                        TaskManager.taskChangeStatusCallback?.Invoke(_taskData, TaskStatus.TakeReward);
                    }
                }
            }
        }
        public int priority;
        [SerializeField] private TaskData _taskData;

        private int _level;
        
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

        public List<ItemCount> PriceToBuy
        {
            get
            {
                if (level == 0)
                {
                    return _priceToBuy;
                }

                List<ItemCount> newPrice = new();
                foreach (var price in _priceToBuy)
                {
                    var a = price.count - 1;
                    float p = 0;
                    for (int i = 2; i <= level + 1; i++)
                    {
                        p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3)))/i;
                        a = (int)p;
                    }
                        
                    newPrice.Add(new ItemCount
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
        [SerializeField] private List<ItemCount> _priceToBuy = new();
        [SerializeField] private int _craftingTime;
        
        public void Save()
        {
            //SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            // var data = SaveLoadSystem.LoadData(this) as WeaponData;
            // if (data != null)
            // {
            //     _isOpen = data._isOpen;
            //     level = data.level;
            // }
        }
    }
}