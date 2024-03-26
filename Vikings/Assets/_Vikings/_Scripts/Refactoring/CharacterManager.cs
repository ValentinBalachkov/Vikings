using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;
using Vikings.SaveSystem;
using Vikings.UI;

namespace _Vikings._Scripts.Refactoring
{
    public class CharacterManager : ISave
    {
        public int SpeedMoveLevel => _characterDynamicData.speedMoveLevel;
        public int SpeedWorkLevel => _characterDynamicData.speedWorkLevel;

        public float SpeedMove =>
            (((Mathf.Sqrt(_characterDynamicData.speedMoveLevel) / 2 + 0.5f) * _charactersConfig.speedMove) +
             Random.Range(0, 0.6f)) * _charactersConfig.speed_up;

        public int SpeedMoveCost
        {
            get
            {
                if (_characterDynamicData.speedMoveLevel == 1)
                {
                    return _charactersConfig.speedMoveCost;
                }


                float newPrice = 0;
                var a = _charactersConfig.speedMoveCost - 1;
                for (int i = 2; i <= _characterDynamicData.speedMoveLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }


                return (int)newPrice;
            }
        }

        public int ItemsCount
        {
            get
            {
                if (ItemsCountLevel == 1)
                {
                    return _charactersConfig.itemsCount;
                }

                return ItemsCountLevel + 1;
            }
        }

        public int ItemsCountCost
        {
            get
            {
                if (ItemsCountLevel == 1)
                {
                    return _charactersConfig.itemsCountCost;
                }


                float newPrice = 0;
                var a = _charactersConfig.itemsCountCost - 1;
                for (int i = 2; i <= ItemsCountLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }

                return (int)newPrice;
            }
        }

        public int ItemsCountLevel
        {
            get => _characterDynamicData.itemsCountLevel;
            set
            {
                _characterDynamicData.itemsCountLevel = value;
                if (_characterDynamicData.itemsCountLevel == 2 && _charactersConfig.taskDataBackpack != null)
                {
                    _charactersConfig.taskDataBackpack.AccessDone = true;
                    if (_charactersConfig.taskDataBackpack.TaskStatus == TaskStatus.InProcess)
                    {
                        TaskManager.taskChangeStatusCallback?.Invoke(_charactersConfig.taskDataBackpack,
                            TaskStatus.TakeReward);
                    }
                }
            }
        }

        public float SpeedWork
        {
            get
            {
                if (_characterDynamicData.speedWorkLevel == 1)
                {
                    return _charactersConfig.speedWork / _charactersConfig.speed_up;
                }

                return (4 / (_characterDynamicData.speedWorkLevel + 3)) / _charactersConfig.speed_up;
            }
        }

        public int SpeedWorkCost
        {
            get
            {
                if (_characterDynamicData.speedWorkLevel == 1)
                {
                    return _charactersConfig.speedWorkCost;
                }


                float newPrice = 0;
                var a = _charactersConfig.speedWorkCost - 1;
                for (int i = 2; i <= _characterDynamicData.speedWorkLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }

                if (_characterDynamicData.speedWorkLevel == 3)
                {
                    newPrice -= 5;
                }

                return (int)newPrice;
            }
        }

        private CharactersConfig _charactersConfig;

        private CharacterDynamicData _characterDynamicData;

        public void AddCharacter()
        {
            _characterDynamicData.charactersCount++;
        }

        public CharacterManager(CharactersConfig charactersConfig)
        {
            _charactersConfig = charactersConfig;

            _characterDynamicData = new();
            _characterDynamicData = SaveLoadSystem.LoadData(_characterDynamicData, _charactersConfig.saveKey);
            SaveLoadManager.saves.Add(this);
        }

        public void Upgrade(UpgradeCharacterEnum upgradeCharacterEnum)
        {
            switch (upgradeCharacterEnum)
            {
                case UpgradeCharacterEnum.SpeedMove:
                    _characterDynamicData.speedMoveLevel++;
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                    ItemsCountLevel++;
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    _characterDynamicData.speedWorkLevel++;
                    break;
            }
        }

        public void Save()
        {
            SaveLoadSystem.SaveData(_characterDynamicData, _charactersConfig.saveKey);
        }
    }
}