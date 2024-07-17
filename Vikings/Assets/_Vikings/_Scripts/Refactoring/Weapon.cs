using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UniRx;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;
using Vikings.SaveSystem;

namespace _Vikings.WeaponObject
{
    public class Weapon : ISave
    {
        public bool IsSet
        {
            get => _weaponDynamicData.IsSetOnCraftingTable;
            set => _weaponDynamicData.IsSetOnCraftingTable = value;
        }

        public ReactiveProperty<int> Level = new();

        private WeaponDynamicData _weaponDynamicData;
        private WeaponData _weaponData;

        public float CraftingTime
        {
            get
            {
                if (Level.Value == 0)
                {
                    return _weaponData.craftingTime;
                }

                return (0.5f * Mathf.Pow(Level.Value + 1, 2)) + _weaponData.craftingTime;
            }
        }

        public Dictionary<ResourceType, int> PriceToBuy
        {
            get
            {
                Dictionary<ResourceType, int> newPrice = new();

                if (Level.Value == 0)
                {
                    foreach (var price in _weaponData.priceToBuy)
                    {
                        newPrice.Add(price.resourceType, price.count);
                    }

                    return newPrice;
                }

                foreach (var price in _weaponData.priceToBuy)
                {
                    var a = price.count - 1;
                    float p = 0;
                    for (int i = 2; i <= Level.Value + 1; i++)
                    {
                        p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                        a = (int)p;
                    }

                    newPrice.Add(price.resourceType, (int)p);
                }

                return newPrice;
            }
        }

        public Weapon(WeaponData weaponData)
        {
            _weaponData = weaponData;
            _weaponDynamicData = new();
            _weaponDynamicData = SaveLoadSystem.LoadData(_weaponDynamicData, _weaponData.saveKey);

            Level.Value = _weaponDynamicData.Level;
            Level.Subscribe(OnLevelChange);
            SaveLoadManager.saves.Add(this);
        }

        public WeaponData GetWeaponData()
        {
            return _weaponData;
        }

        private void OnLevelChange(int value)
        {
            _weaponDynamicData.Level = value;
            if (_weaponDynamicData.Level == 1 && _weaponData.taskData != null)
            {
                _weaponData.taskData.AccessDone = true;
                if (_weaponData.taskData.TaskStatus == TaskStatus.InProcess)
                {
                    TaskManager.taskChangeStatusCallback?.Invoke(_weaponData.taskData, TaskStatus.TakeReward);
                }
            }
        }
        
        public (bool, int, Sprite) IsEnableToBuild(CraftingTable craftingTable, Storage storage)
        {
            bool isEnable;
            int level;
            Sprite sprite = _weaponData.requiredSprite;
            
            if (Level.Value > 0)
            {
                isEnable = craftingTable.CurrentLevel.Value > Level.Value;
                level = craftingTable.CurrentLevel.Value + 1;
            }
            else
            {
                isEnable = true;
                level = storage.CurrentLevel.Value + 1;
            }

            return (isEnable, level, sprite);
        }
        
        public (bool, int, Sprite) IsEnableToBuild(CraftingTable craftingTable)
        {
            bool isEnable;
            int level;
            Sprite sprite = _weaponData.requiredSprite;
            
            if (Level.Value > 0)
            {
                isEnable = craftingTable.CurrentLevel.Value >= 2 && craftingTable.CurrentLevel.Value > Level.Value;
                level = craftingTable.CurrentLevel.Value + 1;
            }
            else
            {
                isEnable = craftingTable.CurrentLevel.Value >= 2;
                level = 2;
            }

            return (isEnable, level, sprite);
        }

        public void Save()
        {
            SaveLoadSystem.SaveData(_weaponDynamicData, _weaponData.saveKey);
        }

        public void Reset()
        {
            SaveLoadSystem.Restart(_weaponDynamicData, _weaponData.saveKey);
        }
    }
}