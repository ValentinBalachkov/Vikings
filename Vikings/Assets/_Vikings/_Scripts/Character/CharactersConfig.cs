using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.UI;

namespace Vikings.Chanacter
{
    [CreateAssetMenu(fileName = "CharactersConfig", menuName = "Data/CharactersConfig", order = 9)]
    public class CharactersConfig : ScriptableObject, IData
    {
        public float speed_up;
        public int charactersCount;
        public int houseLevel;

        [SerializeField] private TaskData _taskDataBackpack;

        public float SpeedMove => (((Mathf.Sqrt(speedMoveLevel) / 2 + 0.5f) * speedMove) + Random.Range(0, 0.6f))*speed_up;

        public int SpeedMoveCost
        {
            get
            {
                if (speedMoveLevel == 1)
                {
                    return speedMoveCost;
                }


                float newPrice = 0;
                var a = speedMoveCost - 1;
                for (int i = 2; i <= speedMoveLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }
                


                // return (int)(Mathf.Pow(speedMoveLevel, 5) + ((speedMoveCost - 1) * speedMoveLevel));
                return (int)newPrice;
            }
        }


        public int ItemsCount
        {
            get
            {
                if (itemsCountLevel == 1)
                {
                    return itemsCount;
                }

                return itemsCountLevel + 1;
            }
        }

        public int ItemsCountCost
        {
            get
            {

                if (itemsCountLevel == 1)
                {
                    return itemsCountCost;
                }


                float newPrice = 0;
                var a = itemsCountCost - 1;
                for (int i = 2; i <= itemsCountLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }
                


                //return (int)(Mathf.Pow(itemsCountLevel, 5) + ((itemsCountCost - 1) * itemsCountLevel));
                return (int)newPrice;
            }
        }
        
        public int itemsCountLevel
        {
            get => _itemsCountLevel;
            set
            {
                _itemsCountLevel = value;
                if (_itemsCountLevel == 1 && _taskDataBackpack != null)
                {
                    _taskDataBackpack.accessDone = true;
                    if (_taskDataBackpack.taskStatus == TaskStatus.InProcess)
                    {
                        TaskManager.taskChangeStatusCallback?.Invoke(_taskDataBackpack, TaskStatus.TakeReward);
                    }
                }
            }
        }

        public float SpeedWork
        {
            get
            {
                if (speedWorkLevel == 1)
                {
                    return speedWork/speed_up;
                }

                return (4 / (speedWorkLevel + 3))/speed_up;
            }
        }

        public int SpeedWorkCost
        {
            get
            {
                if (speedWorkLevel == 1)
                {
                    return speedWorkCost;
                }


                float newPrice = 0;
                var a = speedWorkCost - 1;
                for (int i = 2; i <= speedWorkLevel + 1; i++)
                {
                    newPrice += (Mathf.Pow(i, 4) + ((a * i) - Mathf.Pow(i, 3))) / i;
                    a = (int)newPrice;
                }

                if (speedWorkLevel == 3) // ВРЕМЯ КАСТЫЛЯТЬ МЕЖДУ НАМИ КАСТЫЛЬ ТРАТАТАТАТА
                {
                    newPrice -= 5;
                }


                // return (int)(Mathf.Pow(speedWorkLevel, 5) + ((speedWorkCost - 1) * speedWorkLevel));
                return (int)newPrice;
            }
        }

        public float speedMove = 1;
        public int speedMoveLevel = 1;
        public int speedMoveCost = 14;

        public int speedWork = 1;
        public int speedWorkLevel = 1;
        public int speedWorkCost = 17;

        public int itemsCount = 2;
        public int _itemsCountLevel = 1;
        public int itemsCountCost = 10;

        public void ClearData()
        {
            speedMoveLevel = 1;
            speedWorkLevel = 1;
            _itemsCountLevel = 1;
            charactersCount = 0;
            houseLevel = 0;
        }


        public void Upgrade(UpgradeCharacterEnum upgradeCharacterEnum)
        {
            switch (upgradeCharacterEnum)
            {
                case UpgradeCharacterEnum.SpeedMove:
                    speedMoveLevel++;
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                    _itemsCountLevel++;
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    speedWorkLevel++;
                    break;
            }
        }
        


        public void Save()
        {
            SaveLoadSystem.SaveData(this);
        }

        public void Load()
        {
            var data = SaveLoadSystem.LoadData(this) as CharactersConfig;
            if (data != null)
            {
                charactersCount = data.charactersCount;
                houseLevel = data.houseLevel;
                speedMoveLevel = data.speedMoveLevel;
                speedWorkLevel = data.speedWorkLevel;
                _itemsCountLevel = data._itemsCountLevel;
            }
        }
    }
}