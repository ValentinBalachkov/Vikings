using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using _Vikings._Scripts.Refactoring.Objects;
using _Vikings.Refactoring.Character;
using PanelManager.Scripts.Interfaces;
using UniRx;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;
using _Vikings.WeaponObject;


namespace Vikings.Building
{
    public class CraftingTable : AbstractBuilding, IAcceptArg<Weapon>, ISave
    {
        [SerializeField] private AudioSource _audioSourceToStorage;

        [SerializeField] private GameObject _buildingSpriteObject;

        private Dictionary<ResourceType, int> _currentItemsWeapon = new();
        
        private BuildingData _craftingTableData;
        private CraftingTableDynamicData _craftingTableDynamicData;
        private BuildingView _buildingView;
        private Weapon _currentWeapon;
        
        public override void Init()
        {
            for (int i = 0; i < _craftingTableDynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_craftingTableDynamicData.CurrentItemsCount[i].resourceType,
                    _craftingTableDynamicData.CurrentItemsCount[i].count);
            }
            
            for (int i = 0; i < _craftingTableDynamicData.CurrentItemsCountWeapon.Length; i++)
            {
                _currentItemsWeapon.Add(_craftingTableDynamicData.CurrentItemsCountWeapon[i].resourceType,
                    _craftingTableDynamicData.CurrentItemsCountWeapon[i].count);
            }

            CurrentLevel.Value = _craftingTableDynamicData.CurrentLevel;
            CurrentLevel.Subscribe(OnLevelChange).AddTo(_disposable);
            CreateModel();
            ChangeState(_craftingTableDynamicData.State);
            _buildingView.SetupSprite(CurrentLevel.Value);
            SaveLoadManager.saves.Add(this);
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
            var weapon = arg as Weapon;
            string text = $"REQUIRED:  {_craftingTableData.required} LEVEL{1}";
            bool isEnable = weapon.Level.Value > 0;

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

        public void AcceptArg(Weapon arg)
        {
            _currentWeapon = arg;
            _currentWeapon.IsSet = true;
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
            
            foreach (var item in  _craftingTableDynamicData.CurrentItemsCount)
            {
                item.count = currentItems.FirstOrDefault(x => x.Key == item.resourceType).Value;
            }
        }

        private void OnCountChangeWeaponState(int value, ResourceType itemType)
        {
            var priceDict = _currentWeapon.PriceToBuy;

            if (_currentItemsWeapon[itemType] + value >= priceDict[itemType])
            {
                _currentItemsWeapon[itemType] = priceDict[itemType];
            }
            else
            {
                _currentItemsWeapon[itemType] += value;
            }

            foreach (var item in  _craftingTableDynamicData.CurrentItemsCountWeapon)
            {
                item.count = _currentItemsWeapon.FirstOrDefault(x => x.Key == item.resourceType).Value;
            }
        }

        public void Save()
        {
            SaveLoadSystem.SaveData(_craftingTableDynamicData, _craftingTableData.saveKey);
        }
    }
}