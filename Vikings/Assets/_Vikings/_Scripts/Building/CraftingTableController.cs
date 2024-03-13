﻿using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using _Vikings._Scripts.Refactoring.Objects;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;
using Vikings.Weapon;

namespace Vikings.Building
{
    public class CraftingTableController : AbstractBuilding
    {
        [SerializeField] private AudioSource _audioSourceToStorage;

        [SerializeField] private GameObject _buildingSpriteObject;

        private BuildingData _craftingTableData;
        private CraftingTableDynamicData _craftingTableDynamicData;
        private BuildingView _buildingView;


        private WeaponData _currentWeapon;
        private bool _isUpgradeWeapon;

        public override void Init()
        {
            for (int i = 0; i < _craftingTableDynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_craftingTableDynamicData.CurrentItemsCount[i].resourceType,
                    _craftingTableDynamicData.CurrentItemsCount[i].count);
            }

            CurrentLevel.Value = _craftingTableDynamicData.CurrentLevel;
            CurrentLevel.Subscribe(OnLevelChange).AddTo(_disposable);
            CreateModel();
            ChangeState(_craftingTableDynamicData.State);
            _buildingView.SetupSprite(CurrentLevel.Value);
        }

        private void OnLevelChange(int value)
        {
            _craftingTableDynamicData.CurrentLevel = value;
        }

        private void CreateModel()
        {
            _buildingView = Instantiate(_craftingTableData._buildingView, transform);
            _buildingView.Init(_craftingTableData);
        }
        
        
        public override Transform GetPosition()
        {
            return transform;
        }

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            var item = characterStateMachine.Inventory.GetItemFromInventory();
            ChangeCount?.Invoke(item.count, item.resourceType);
        }

        public override void Upgrade()
        {
            CurrentLevel.Value++;
            _buildingView.SetupSprite(CurrentLevel.Value);
        }

        public override Dictionary<ResourceType, int> GetNeededItemsCount()
        {
            Dictionary<ResourceType, int> dict = new();

            var maxPrice = GetPriceForUpgrade();

            foreach (var price in maxPrice)
            {
                var currentItem =
                    _craftingTableDynamicData.CurrentItemsCount.FirstOrDefault(x => x.resourceType == price.Key);

                if (currentItem == null)
                {
                    Debug.LogError("GetNeededItemsCount is null");
                    continue;
                }
               
                dict.Add(price.Key, price.Value - currentItem.count);
            }

            return dict;
        }

        public override Dictionary<ResourceType, int> GetPriceForUpgrade()
        {
            Dictionary<ResourceType, int> dict = new();

            if (CurrentLevel.Value == 0)
            {
                foreach (var price in _craftingTableData.priceToUpgrades)
                {
                    dict.Add(price.resourceType, price.count);
                }

                return dict;
            }
            
            foreach (var price in _craftingTableData.priceToUpgrades)
            {
                var a = price.count - 1;
                float p = 0;
                for (int i = 2; i <= CurrentLevel.Value + 1; i++)
                {
                    p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3)))/i;
                    a = (int)p;
                }
                
                dict.Add(price.resourceType, (int)p);
            }

            return dict;
        }

        public override void SetData(BuildingData buildingData)
        {
            _craftingTableData = buildingData;

            _craftingTableDynamicData = new();
            _craftingTableDynamicData = SaveLoadSystem.LoadData(_craftingTableDynamicData, buildingData.saveKey);
        }

        public override (bool, string) IsEnableToBuild<T>(T arg)
        {
            var weapon = arg as WeaponData;
            string text = $"REQUIRED:  {_craftingTableData.required} LEVEL{1}";
            bool isEnable = weapon.IsOpen;

            return (isEnable, text);
        }

        public override BuildingData GetData()
        {
            return _craftingTableData;
        }

        public override void ChangeState(BuildingState state)
        {
            base.ChangeState(state);
            _craftingTableDynamicData.State = state;
            
            switch (state)
            {
                case BuildingState.NotSet:
                    _buildingSpriteObject.SetActive(false);
                    _buildingView.gameObject.SetActive(false);
                    break;
                case BuildingState.InProgress:
                    ChangeSpriteObject(true);
                    ChangeCount = OnCountChangeInProgressState;
                    break;
                case BuildingState.Ready:
                    ChangeSpriteObject(false);
                    ChangeCount = OnCountChangeWeaponState;
                    break;
            }
        }
        
        private void ChangeSpriteObject(bool isBuilding)
        {
            _buildingSpriteObject.SetActive(isBuilding);
            _buildingView.gameObject.SetActive(!isBuilding);
        }


        private void OnCountChangeInProgressState(int value, ResourceType itemType)
        {
            var priceDict = GetPriceForUpgrade();

            if (currentItems[itemType] + value >= priceDict[itemType])
            {
                currentItems[itemType] = priceDict[itemType];
            }
            else
            {
                currentItems[itemType] += value;
            }
        }
        
        private void OnCountChangeWeaponState(int value, ResourceType itemType)
        {
            
        }
    }
}