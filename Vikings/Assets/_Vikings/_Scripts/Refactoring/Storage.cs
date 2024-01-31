using _Vikings._Scripts.Refactoring.Objects;
using SecondChanceSystem.SaveSystem;
using UniRx;
using UnityEngine;
using Vikings.Building;
using Vikings.Items;
using Vikings.Object;
using AbstractBuilding = Vikings.Object.AbstractBuilding;

namespace _Vikings._Scripts.Refactoring
{
    public class Storage : AbstractBuilding
    {
        [SerializeField] private ItemData[] _itemsData;
        [SerializeField] private GameObject _buildingSpriteObject;
        private GameObject _storageSpriteObject;

        private StorageVisual _storageVisual;

        private StorageData _storageData;

        public override Transform GetPosition()
        {
            return transform;
        }

        public override void CharacterAction()
        {
        }

        public override void Init()
        {
            for (int i = 0; i < _storageData.DynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_itemsData[i], _storageData.DynamicData.CurrentItemsCount[i]);
            }
            
            CurrentLevel.Value = _storageData.DynamicData.CurrentLevel;
            CurrentLevel.Subscribe(OnLevelChange).AddTo(_disposable);
            CreateModel();
            _storageVisual.SetupSprite(_storageData.DynamicData.CurrentLevel);
        }


        public override void Upgrade()
        {
            _storageData.DynamicData.CurrentLevel++;
            _storageData.DynamicData.MaxStorageCount = (int)((Mathf.Pow(_storageData.DynamicData.CurrentLevel, 3)
                                                              + Mathf.Pow(2, _storageData.DynamicData.CurrentLevel)
                                                              + (Mathf.Pow(4,
                                                                  _storageData.DynamicData.CurrentLevel - 1)))
                                                             + 15);
            _storageVisual.SetupSprite(_storageData.DynamicData.CurrentLevel);
        }

        public override void ChangeState(BuildingState state)
        {
            base.ChangeState(state);
            switch (state)
            {
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

        public override void FindData(BuildingType buildingType)
        {
            _storageData = Resources.Load<StorageData>($"Building/{buildingType}");
        }

        private void CreateModel()
        {
            _storageVisual = Instantiate(_storageData.storageVisual, transform);
            _storageVisual.Init(_storageData);
        }

        

        private void ChangeSpriteObject(bool isBuilding)
        {
            _buildingSpriteObject.SetActive(isBuilding);
            _storageSpriteObject.SetActive(!isBuilding);
        }

        private void OnCountChangeReadyState(int value, ItemData itemData)
        {
            _storageData.DynamicData.Count = value > _storageData.DynamicData.MaxStorageCount
                ? _storageData.DynamicData.MaxStorageCount
                : value;
        }
        
        private void OnCountChangeInProgressState(int value, ItemData itemData)
        {
            if (currentItems[itemData] + value >= priceToUpgrades[itemData])
            {
                currentItems[itemData] = priceToUpgrades[itemData];
            }
            else
            {
                currentItems[itemData] += value;
            }
        }

        private void OnLevelChange(int value)
        {
            _storageData.DynamicData.CurrentLevel = value;
            if (_storageData.DynamicData.CurrentLevel == 1 && _storageData.TaskData != null)
            {
                _storageData.TaskData.accessDone = true;
                if (_storageData.TaskData.taskStatus == TaskStatus.InProcess)
                {
                    TaskManager.taskChangeStatusCallback?.Invoke(_storageData.TaskData, TaskStatus.TakeReward);
                }
            }
        }
    }
}