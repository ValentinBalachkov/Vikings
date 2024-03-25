using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;
using Vikings.UI;
using AbstractBuilding = Vikings.Object.AbstractBuilding;
using BuildingView = _Vikings._Scripts.Refactoring.Objects.BuildingView;

namespace _Vikings._Scripts.Refactoring
{
    public class Storage : AbstractBuilding, ISave
    {
        public int Priority => _storageData.priorityToAction;
        
        public int CharactersCount;
        public int MaxStorageCount => _storageDynamicData.MaxStorageCount;
        
        public int Count => _storageDynamicData.Count;
        
        public Action<Storage> StorageNeedItem;
        public ResourceType ResourceType => _storageDynamicData.ResourceType;

        [SerializeField] private GameObject _buildingSpriteObject;

        protected StorageDynamicData _storageDynamicData;
        protected InventoryView _inventoryView;

        private BuildingView _buildingView;

        private BuildingData _storageData;

        private CollectingResourceView _collectingResourceView;


        public override float GetStoppingDistance()
        {
            return _storageData.stoppingDistance;
        }

        public override Transform GetPosition()
        {
            return transform;
        }

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            if (buildingState == BuildingState.Crafting)
            {
                characterStateMachine.SetWorkAnimation(null);
                if (!isCraftActivated)
                {
                    StartCoroutine(UpgradeDelay(characterStateMachine, _storageDynamicData.BuildingTime));
                }
                return;
            }
            
            

            DelayActionCoroutine(characterStateMachine);
        }

        public override void Init()
        {
            for (int i = 0; i < _storageDynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_storageDynamicData.CurrentItemsCount[i].resourceType,
                    _storageDynamicData.CurrentItemsCount[i].count);
            }

            CurrentLevel.Value = _storageDynamicData.CurrentLevel;
            CurrentLevel.Subscribe(OnLevelChange).AddTo(_disposable);
            CreateModel();
            ChangeState(_storageDynamicData.State);
            _buildingView.SetupSprite(_storageDynamicData.CurrentLevel);
            SaveLoadManager.saves.Add(this);
        }


        public override void Upgrade()
        {
            CurrentLevel.Value++;
            if (CurrentLevel.Value > 1)
            {
                _storageDynamicData.MaxStorageCount = (int)((Mathf.Pow(_storageDynamicData.CurrentLevel, 3)
                                                             + Mathf.Pow(2, _storageDynamicData.CurrentLevel)
                                                             + (Mathf.Pow(4,
                                                                 _storageDynamicData.CurrentLevel - 1)))
                                                            + 15);
            }

            _buildingView.SetupSprite(CurrentLevel.Value);
            ChangeState(BuildingState.Ready);
        }

        public override void AcceptArg(MainPanelManager arg)
        {
            base.AcceptArg(arg);
            _collectingResourceView = panelManager.SudoGetPanel<CollectingResourceView>();
            _inventoryView = panelManager.SudoGetPanel<InventoryView>();
            _inventoryView.UpdateUI(_storageDynamicData.Count, _storageDynamicData.MaxStorageCount, ResourceType);
        }

        public override void ChangeState(BuildingState state)
        {
            base.ChangeState(state);
            _storageDynamicData.State = state;
            switch (state)
            {
                case BuildingState.NotSet:
                    _buildingSpriteObject.SetActive(false);
                    _buildingView.gameObject.SetActive(false);
                    break;
                case BuildingState.InProgress:
                    ChangeSpriteObject(true);
                    ChangeCount += OnCountChangeInProgressState;
                    ChangeCount -= OnCountChangeReadyState;
                    _collectingResourceView.Setup(this);
                    break;
                case BuildingState.Ready:
                    ChangeSpriteObject(false);
                    ChangeCount -= OnCountChangeInProgressState;
                    ChangeCount += OnCountChangeReadyState;
                    StorageNeedItem?.Invoke(this);
                    break;
                case BuildingState.Crafting:
                    isCraftActivated = false;
                    BuildingComplete?.Invoke(this);
                    _collectingResourceView.Clear();
                    break;
            }
        }

        public override Dictionary<ResourceType, int> GetNeededItemsCount()
        {
            Dictionary<ResourceType, int> dict = new();

            var maxPrice = GetPriceForUpgrade();

            foreach (var price in maxPrice)
            {
                var currentItem =
                    _storageDynamicData.CurrentItemsCount.FirstOrDefault(x => x.resourceType == price.Key);

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
                foreach (var price in _storageData.priceToUpgrades)
                {
                    dict.Add(price.resourceType, price.count);
                }

                return dict;
            }

            foreach (var price in _storageData.priceToUpgrades)
            {
                var a = price.count - 1;
                float p = 0;
                for (int i = 2; i <= CurrentLevel.Value + 1; i++)
                {
                    p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)p;
                }

                dict.Add(price.resourceType, (int)p);
            }

            return dict;
        }

        public override void SetData(BuildingData buildingData)
        {
            _storageData = buildingData;

            _storageDynamicData = new();
            _storageDynamicData = SaveLoadSystem.LoadData(_storageDynamicData, buildingData.saveKey);
        }

        public override (bool, string) IsEnableToBuild<T>(T arg)
        {
            var craftingTable = arg as AbstractBuilding;

            string requiredText =
                $"REQUIRED:  {_storageData.required} LEVEL{craftingTable.CurrentLevel.Value + 1}";

            bool isEnable = false;

            switch (ResourceType)
            {
                case ResourceType.Wood:
                    isEnable = CurrentLevel.Value == 0 || craftingTable.CurrentLevel.Value - CurrentLevel.Value >= 0;
                    break;
                case ResourceType.Rock:
                    isEnable = craftingTable.CurrentLevel.Value - CurrentLevel.Value >= 1;
                    break;
                case ResourceType.Eat:
                    if (CurrentLevel.Value >= 5)
                    {
                        requiredText = "MAX";
                    }

                    isEnable = craftingTable.CurrentLevel.Value - CurrentLevel.Value >= 1 && CurrentLevel.Value < 5;
                    break;
            }

            return (isEnable, requiredText);
        }

        public override BuildingData GetData()
        {
            return _storageData;
        }

        public bool CheckNeededItem()
        {
            return _storageDynamicData.Count < _storageDynamicData.MaxStorageCount &&
                   buildingState == BuildingState.Ready;
        }

        

        private void CreateModel()
        {
            _buildingView = Instantiate(_storageData._buildingView, transform);
            _buildingView.Init(_storageData);
        }

        private void DelayActionCoroutine(CharacterStateMachine characterStateMachine)
        {
            if (buildingState != BuildingState.Crafting)
            {
                characterStateMachine.SetCollectAnimation(null, () =>
                {
                    if (characterStateMachine.Inventory.CheckItemCount() == 0)
                    {
                        ItemCount item;
                
                        if (Count >= characterStateMachine.BackpackCount)
                        {
                            item = new ItemCount
                            {
                                resourceType = ResourceType,
                                count = characterStateMachine.BackpackCount
                            };
                        }
                        else
                        {
                            item = new ItemCount
                            {
                                resourceType = ResourceType,
                                count = Count
                            };
                        }
                 
                        characterStateMachine.Inventory.SetItemToInventory(item.resourceType, item.count);
                        ChangeCount?.Invoke(-item.count, item.resourceType);
                        CharactersCount--;
                    }
                    else
                    {
                        var item = characterStateMachine.Inventory.GetItemFromInventory();
                        ChangeCount?.Invoke(item.count, item.resourceType);
                    }
                    
                    EndAction?.Invoke();
                });
            }
        }


        private void ChangeSpriteObject(bool isBuilding)
        {
            _buildingSpriteObject.SetActive(isBuilding);
            _buildingView.gameObject.SetActive(!isBuilding);
        }

        private void OnCountChangeReadyState(int value, ResourceType itemType)
        {
            if (_storageDynamicData.Count + value > _storageDynamicData.MaxStorageCount)
            {
                _storageDynamicData.Count = _storageDynamicData.MaxStorageCount;
            }
            else if (_storageDynamicData.Count + value < 0)
            {
                _storageDynamicData.Count = 0;
            }
            else
            {
                _storageDynamicData.Count += value;
            }

            _inventoryView.UpdateUI(_storageDynamicData.Count, _storageDynamicData.MaxStorageCount, ResourceType);

            if (value < 0)
            {
                StorageNeedItem?.Invoke(this);
            }
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

            foreach (var item in _storageDynamicData.CurrentItemsCount)
            {
                item.count = currentItems.FirstOrDefault(x => x.Key == item.resourceType).Value;
            }

            _collectingResourceView.UpdateView(currentItems, priceDict);

            if (priceDict[ResourceType.Wood] == currentItems[ResourceType.Wood] &&
                priceDict[ResourceType.Rock] == currentItems[ResourceType.Rock])
            {
                ChangeState(BuildingState.Crafting);
            }
        }

        private void OnLevelChange(int value)
        {
            _storageDynamicData.CurrentLevel = value;
            if (_storageDynamicData.CurrentLevel == 1 && _storageData.taskData != null)
            {
                _storageData.taskData.accessDone = true;
                if (_storageData.taskData.taskStatus == TaskStatus.InProcess)
                {
                    TaskManager.taskChangeStatusCallback?.Invoke(_storageData.taskData, TaskStatus.TakeReward);
                }
            }
        }

        public void Save()
        {
            SaveLoadSystem.SaveData(_storageDynamicData, _storageData.saveKey);
        }
    }
}