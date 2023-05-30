using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.UI;

namespace Vikings.Chanacter
{
    [CreateAssetMenu(fileName = "CharactersConfig", menuName = "Data/CharactersConfig", order = 9)]
    public class CharactersConfig : ScriptableObject, IData
    {
        public int charactersCount;
        public int houseLevel;
        
         public float SpeedMove
        {
            get
            {
                if (speedMoveLevel == 1)
                {
                    return speedMove;
                }

                return Mathf.Sqrt(speedMoveLevel) / 2 + 0.5f;
            }
        }

        public int SpeedMoveCost
        {
            get
            {
                if (speedMoveLevel == 1)
                {
                    return speedMoveCost;
                }
                return (int)(Mathf.Pow(speedMoveLevel, 5) + ((speedMoveCost - 1) * speedMoveLevel));
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
                return (int)(Mathf.Pow(itemsCountLevel, 5) + ((itemsCountCost - 1) * itemsCountLevel));
            }
        }

        public float SpeedWork
        {
            get
            {
                if (speedWorkLevel == 1)
                {
                    return speedWork;
                }

                return 4 / (speedWorkLevel + 3);
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
                return (int)(Mathf.Pow(speedWorkLevel, 5) + ((speedWorkCost - 1) * speedWorkLevel));
            }
        }

        public float speedMove = 1;
        public int speedMoveLevel = 1;
        public int speedMoveCost = 14;

        public int speedWork = 1;
        public int speedWorkLevel = 1;
        public int speedWorkCost = 18;

        public int itemsCount = 2;
        public int itemsCountLevel = 1;
        public int itemsCountCost = 10;


        public void Upgrade(UpgradeCharacterEnum upgradeCharacterEnum)
        {
            switch (upgradeCharacterEnum)
            {
                case UpgradeCharacterEnum.SpeedMove:
                    speedMoveLevel++;
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                    speedWorkLevel++;
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    itemsCountLevel++;
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
                itemsCountLevel = data.itemsCountLevel;

            }
        }
    }
}