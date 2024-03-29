using System.Collections;
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
        public Weapon CurrentWeapon => _currentWeapon;
        [SerializeField] private GameObject _buildingSpriteObject;

        public Dictionary<ResourceType, int> currentItemsWeapon = new();

        private BuildingData _craftingTableData;
        private CraftingTableDynamicData _craftingTableDynamicData;
        private BuildingView _buildingView;
        private Weapon _currentWeapon;
        private CollectingResourceView _collectingResourceView;

        public override void Init(MainPanelManager mainPanelManager)
        {
            _panelManager = mainPanelManager;

            _collectingResourceView = _panelManager.SudoGetPanel<CollectingResourceView>();

            for (int i = 0; i < _craftingTableDynamicData.CurrentItemsCount.Length; i++)
            {
                currentItems.Add(_craftingTableDynamicData.CurrentItemsCount[i].resourceType,
                    _craftingTableDynamicData.CurrentItemsCount[i].count);
            }

            for (int i = 0; i < _craftingTableDynamicData.CurrentItemsCountWeapon.Length; i++)
            {
                currentItemsWeapon.Add(_craftingTableDynamicData.CurrentItemsCountWeapon[i].resourceType,
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

            if (_craftingTableDynamicData.CurrentLevel == 1 && _craftingTableData.taskData != null)
            {
                _craftingTableData.taskData.AccessDone = true;
                if (_craftingTableData.taskData.TaskStatus == TaskStatus.InProcess)
                {
                    TaskManager.taskChangeStatusCallback?.Invoke(_craftingTableData.taskData, TaskStatus.TakeReward);
                }
            }
        }

        private void CreateModel()
        {
            _buildingView = Instantiate(_craftingTableData._buildingView, transform);
            _buildingView.Init(_craftingTableData);
        }


        public override float GetStoppingDistance()
        {
            return _craftingTableData.stoppingDistance;
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
                    if (_currentWeapon == null)
                    {
                        StartCoroutine(UpgradeDelay(characterStateMachine, _craftingTableDynamicData.BuildingTime));
                    }
                    else
                    {
                        StartCoroutine(UpgradeWeaponDelay(characterStateMachine, _currentWeapon.CraftingTime));
                    }
                }

                return;
            }

            var item = characterStateMachine.Inventory.GetItemFromInventory();
            if (item != null)
            {
                ChangeCount?.Invoke(item.count, item.resourceType);
            }
            EndAnimationCharacter(characterStateMachine);
        }

        private void EndAnimationCharacter(CharacterStateMachine characterStateMachine)
        {
            if (buildingState != BuildingState.Crafting)
            {
                characterStateMachine.SetCollectAnimation(null, () =>
                {
                    _changeItemSound.Play();
                    EndAction?.Invoke();
                });
            }
        }

        public override void Upgrade()
        {
            CurrentLevel.Value++;
            _buildingView.SetupSprite(CurrentLevel.Value);

            currentItems[ResourceType.Wood] = 0;
            currentItems[ResourceType.Rock] = 0;
            
            currentItemsWeapon[ResourceType.Wood] = 0;
            currentItemsWeapon[ResourceType.Rock] = 0;

            ChangeState(BuildingState.Ready);
        }

        public override Dictionary<ResourceType, int> GetNeededItemsCount()
        {
            Dictionary<ResourceType, int> dict = new();

            var maxPrice = GetPriceForUpgrade();

            if (_currentWeapon == null)
            {
                foreach (var price in maxPrice)
                {
                    var currentItem =
                        currentItems.FirstOrDefault(x => x.Key == price.Key).Value;

                    dict.Add(price.Key, price.Value - currentItem);
                }
            }
            else
            {
                foreach (var price in maxPrice)
                {
                    var currentItem =
                        currentItemsWeapon.FirstOrDefault(x => x.Key == price.Key).Value;

                    dict.Add(price.Key, price.Value - currentItem);
                }
            }

            return dict;
        }

        public override Dictionary<ResourceType, int> GetPriceForUpgrade()
        {
            if (_currentWeapon != null)
            {
                return _currentWeapon.PriceToBuy;
            }

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
                    p += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
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

        public override (bool, int, Sprite) IsEnableToBuild<T>(T arg)
        {
            var weapon = arg as Weapon;
            bool isEnable = weapon.Level.Value > 0;
            Sprite sprite = _craftingTableData.requiredSprite;
            int level = 1;

            return (isEnable, level, sprite);
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
                    ChangeSpriteObject(false);
                    if (_currentWeapon == null)
                    {
                        ChangeCount = OnCountChangeInProgressState;
                    }
                    else
                    {
                        ChangeCount = OnCountChangeWeaponState;
                    }

                    _collectingResourceView.Setup(this);
                    break;
                case BuildingState.Ready:
                    ChangeCount = OnCountChangeInProgressState;
                    ChangeSpriteObject(true);
                    break;
                case BuildingState.Crafting:
                    isCraftActivated = false;
                    BuildingComplete?.Invoke(this);
                    _collectingResourceView.Clear();
                    break;
            }
        }

        public void AcceptArg(Weapon arg)
        {
            _currentWeapon = arg;
            _currentWeapon.IsSet = true;
            _collectingResourceView.ChangeHeader(_currentWeapon.GetWeaponData().nameText);
            
        }

        private void ChangeSpriteObject(bool isReady)
        {
            if (isReady)
            {
                _buildingSpriteObject.SetActive(false);
                _buildingView.gameObject.SetActive(true);
                return;
            }

            if (CurrentLevel.Value == 0 || _currentWeapon == null)
            {
                _buildingSpriteObject.SetActive(true);
                _buildingView.gameObject.SetActive(false);
            }
            else
            {
                _buildingSpriteObject.SetActive(false);
                _buildingView.gameObject.SetActive(true);
            }
        }

        private IEnumerator UpgradeWeaponDelay(CharacterStateMachine characterStateMachine, float buildingTime)
        {
            _craftingSound.Play();
            isCraftActivated = true;
            particleCraftEffect.gameObject.SetActive(true);
            particleCraftEffect.Play();
            var time = buildingTime * characterStateMachine.SpeedWork;
            _panelManager.SudoGetPanel<CraftingIndicatorView>().Setup((int)time, GetPosition());
            yield return new WaitForSeconds(time);
            particleCraftEffect.Stop();
            particleCraftEffect.gameObject.SetActive(false);
            _craftingSound.Stop();
            UpgradeWeapon();
            EndAction?.Invoke();
        }

        public void UpgradeWeapon()
        {
            _currentWeapon.Level.Value++;
            _currentWeapon.IsSet = false;
            _currentWeapon = null;

            currentItemsWeapon[ResourceType.Wood] = 0;
            currentItemsWeapon[ResourceType.Rock] = 0;
            
            currentItems[ResourceType.Wood] = 0;
            currentItems[ResourceType.Rock] = 0;

            ChangeState(CurrentLevel.Value == 0 ? BuildingState.NotSet : BuildingState.Ready);
            _panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(true);
        }


        private void OnCountChangeInProgressState(int value, ResourceType itemType)
        {
            if (itemType == ResourceType.Eat || value < 0)
            {
                return;
            }
            
            var priceDict = GetPriceForUpgrade();

            if (currentItems[itemType] + value >= priceDict[itemType])
            {
                currentItems[itemType] = priceDict[itemType];
            }
            else
            {
                currentItems[itemType] += value;
            }

            foreach (var item in _craftingTableDynamicData.CurrentItemsCount)
            {
                item.count = currentItems.FirstOrDefault(x => x.Key == item.resourceType).Value;
            }

            _collectingResourceView.UpdateView(currentItems, priceDict);

            if (priceDict[ResourceType.Wood] <= currentItems[ResourceType.Wood] &&
                priceDict[ResourceType.Rock] <= currentItems[ResourceType.Rock])
            {
                ChangeState(BuildingState.Crafting);
            }
        }

        private void OnCountChangeWeaponState(int value, ResourceType itemType)
        {
            if (itemType == ResourceType.Eat || value < 0)
            {
                return;
            }
            var priceDict = _currentWeapon.PriceToBuy;

            if (currentItemsWeapon[itemType] + value >= priceDict[itemType])
            {
                currentItemsWeapon[itemType] = priceDict[itemType];
            }
            else
            {
                currentItemsWeapon[itemType] += value;
            }

            foreach (var item in _craftingTableDynamicData.CurrentItemsCountWeapon)
            {
                item.count = currentItemsWeapon.FirstOrDefault(x => x.Key == item.resourceType).Value;
            }

            _collectingResourceView.UpdateView(currentItemsWeapon, priceDict);

            if (priceDict[ResourceType.Wood] <= currentItemsWeapon[ResourceType.Wood] &&
                priceDict[ResourceType.Rock] <= currentItemsWeapon[ResourceType.Rock])
            {
                ChangeState(BuildingState.Crafting);
            }
        }

        public void Save()
        {
            foreach (var item in currentItems)
            {
                _craftingTableDynamicData.CurrentItemsCount.FirstOrDefault(x => x.resourceType == item.Key).count =
                    currentItems[item.Key];
            }

            
            foreach (var item in currentItemsWeapon)
            {
                _craftingTableDynamicData.CurrentItemsCountWeapon.FirstOrDefault(x => x.resourceType == item.Key)
                        .count =
                    currentItemsWeapon[item.Key];
            }

            SaveLoadSystem.SaveData(_craftingTableDynamicData, _craftingTableData.saveKey);
        }

        public void Reset()
        {
            SaveLoadSystem.Restart(_craftingTableDynamicData, _craftingTableData.saveKey);
        }
    }
}