using System.Collections.Generic;
using _Vikings._Scripts.Refactoring.Objects;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;
using Vikings.Object;
using Vikings.SaveSystem;
using AbstractBuilding = Vikings.Object.AbstractBuilding;

namespace _Vikings._Scripts.Refactoring
{
    public class Storage : AbstractBuilding
    {
        public ResourceType ResourceType => _storageDynamicData.ResourceType;
        
        [SerializeField] private GameObject _buildingSpriteObject;

        private BuildingView _buildingView;

        private BuildingData _storageData;
        private StorageDynamicData _storageDynamicData;

        public override Transform GetPosition()
        {
            return transform;
        }

        public override void CharacterAction(CharacterStateMachine characterStateMachine)
        {
            var item = characterStateMachine.Inventory.GetItemFromInventory();
            ChangeCount?.Invoke(item.count, item.resourceType);
        }
        

        public override void Init()
        {
            for (int i = 0; i < _storageDynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_storageDynamicData.CurrentItemsCount[i].resourceType, _storageDynamicData.CurrentItemsCount[i].count);
            }

            CurrentLevel.Value = _storageDynamicData.CurrentLevel;
            CurrentLevel.Subscribe(OnLevelChange).AddTo(_disposable);
            CreateModel();
            ChangeState(_storageDynamicData.State);
            _buildingView.SetupSprite(_storageDynamicData.CurrentLevel);
        }


        public override void Upgrade()
        {
            _storageDynamicData.CurrentLevel++;
            _storageDynamicData.MaxStorageCount = (int)((Mathf.Pow(_storageDynamicData.CurrentLevel, 3)
                                                         + Mathf.Pow(2, _storageDynamicData.CurrentLevel)
                                                         + (Mathf.Pow(4,
                                                             _storageDynamicData.CurrentLevel - 1)))
                                                        + 15);
            _buildingView.SetupSprite(_storageDynamicData.CurrentLevel);
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
                    ChangeCount = OnCountChangeInProgressState;
                    break;
                case BuildingState.Ready:
                    ChangeSpriteObject(false);
                    ChangeCount = OnCountChangeReadyState;
                    break;
            }
        }

        public override Dictionary<ResourceType, int> GetNeededItemsCount()
        {
            throw new System.NotImplementedException();
        }

        public override void SetData(BuildingData buildingData)
        {
            _storageData = buildingData;

            _storageDynamicData = new();
            _storageDynamicData = SaveLoadSystem.LoadData(_storageDynamicData, buildingData.saveKey);
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


        private void ChangeSpriteObject(bool isBuilding)
        {
            _buildingSpriteObject.SetActive(isBuilding);
            _buildingView.gameObject.SetActive(!isBuilding);
        }

        private void OnCountChangeReadyState(int value, ResourceType itemType)
        {
            _storageDynamicData.Count = value > _storageDynamicData.MaxStorageCount
                ? _storageDynamicData.MaxStorageCount
                : value;
        }

        private void OnCountChangeInProgressState(int value, ResourceType itemType)
        {
            if (currentItems[itemType] + value >= priceToUpgrades[itemType])
            {
                currentItems[itemType] = priceToUpgrades[itemType];
            }
            else
            {
                currentItems[itemType] += value;
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
    }
}