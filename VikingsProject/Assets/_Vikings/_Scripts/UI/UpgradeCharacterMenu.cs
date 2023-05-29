using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;

namespace Vikings.UI
{
    public class UpgradeCharacterMenu : MonoBehaviour
    {
        [SerializeField] private CharacterMenuElement _characterMenuElement;
        [SerializeField] private CharacterUpgradeUIData[] _characterUpgradeUIData;
        [SerializeField] private Transform _content;
        [SerializeField] private CharactersConfig _charactersConfig;
        [SerializeField] private StorageData _storageData;

        private List<CharacterMenuElement> _characterMenuElements = new();

        private void Awake()
        {
            Spawn();
        }

        private void OnEnable()
        {
            _storageData.OnUpdateCount += () =>
            {
                foreach (var item in _characterMenuElements)
                {
                    UpdateUI(item.upgradeCharacterEnum);
                }
            };
        }

        private void OnDisable()
        {
            _storageData.OnUpdateCount = null;
        }

        private void Spawn()
        {
            for (int i = 0; i < 3; i++)
            {
                var element = Instantiate(_characterMenuElement, _content);
                element.Init(_characterUpgradeUIData[i]);
                var index = i;
                element.AddBtnListener(() =>
                {
                    UpdateItemsCount(_characterUpgradeUIData[index].upgradeCharacterEnum);
                    _charactersConfig.Upgrade(_characterUpgradeUIData[index].upgradeCharacterEnum); 
                    UpdateUI(_characterUpgradeUIData[index].upgradeCharacterEnum);
                });
                _characterMenuElements.Add(element);
                switch (_characterUpgradeUIData[i].upgradeCharacterEnum)
                {
                    case UpgradeCharacterEnum.SpeedMove:
                        element.UpdateUI(_charactersConfig.speedMoveLevel, _charactersConfig.SpeedMoveCost, _storageData.Count >= _charactersConfig.SpeedMoveCost);
                        break;
                    case UpgradeCharacterEnum.ItemsCount:
                        element.UpdateUI(_charactersConfig.itemsCountLevel, _charactersConfig.ItemsCountCost, _storageData.Count >= _charactersConfig.ItemsCountCost);
                        break;
                    case UpgradeCharacterEnum.SpeedWork:
                        element.UpdateUI(_charactersConfig.speedWorkLevel, _charactersConfig.SpeedWorkCost, _storageData.Count >= _charactersConfig.SpeedWorkCost);
                        break;
                }
            }
        }

        private void UpdateItemsCount(UpgradeCharacterEnum upgradeCharacterEnum)
        {
            var item = _characterMenuElements.FirstOrDefault(x => x.upgradeCharacterEnum == upgradeCharacterEnum);
            switch (item.upgradeCharacterEnum)
            {
                case UpgradeCharacterEnum.SpeedMove:
                    _storageData.Count -= _charactersConfig.SpeedMoveCost;
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                    _storageData.Count -= _charactersConfig.ItemsCountCost;
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    _storageData.Count -= _charactersConfig.SpeedWorkCost;
                    break;
            }
        }

        private void UpdateUI(UpgradeCharacterEnum upgradeCharacterEnum)
        {
           var item = _characterMenuElements.FirstOrDefault(x => x.upgradeCharacterEnum == upgradeCharacterEnum);
           switch (item.upgradeCharacterEnum)
           {
               case UpgradeCharacterEnum.SpeedMove:
                   item.UpdateUI(_charactersConfig.speedMoveLevel, _charactersConfig.SpeedMoveCost, _storageData.Count >= _charactersConfig.SpeedMoveCost);
                   break;
               case UpgradeCharacterEnum.ItemsCount:
                   item.UpdateUI(_charactersConfig.itemsCountLevel, _charactersConfig.ItemsCountCost, _storageData.Count >= _charactersConfig.ItemsCountCost);
                   break;
               case UpgradeCharacterEnum.SpeedWork:
                   item.UpdateUI(_charactersConfig.speedWorkLevel, _charactersConfig.SpeedWorkCost, _storageData.Count >= _charactersConfig.SpeedWorkCost);
                   break;
           }
        }
    }

}