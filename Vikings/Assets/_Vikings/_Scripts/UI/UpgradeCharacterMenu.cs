using System.Collections.Generic;
using System.Linq;
using PanelManager.Scripts.Panels;
using UnityEngine;
using Vikings.Building;
using Vikings.Chanacter;

namespace Vikings.UI
{
    public class UpgradeCharacterMenu : ViewBase
    {
        public override PanelType PanelType => PanelType.Screen;
        public override bool RememberInHistory => false;
        
        [SerializeField] private CharacterMenuElement _characterMenuElement;
        [SerializeField] private CharacterUpgradeUIData[] _characterUpgradeUIData;
        [SerializeField] private Transform _content;
        [SerializeField] private CharactersConfig _charactersConfig;
        [SerializeField] private StorageData _storageData;
        [SerializeField] private AudioSource _audioSourceBtnClick;

        //[SerializeField] private CharactersOnMap _charactersOnMap;
      //  [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private InventoryView _inventoryView;

        private List<CharacterMenuElement> _characterMenuElements = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Spawn();
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            _storageData.OnUpdateCount += () =>
            {
                foreach (var item in _characterMenuElements)
                {
                    UpdateUI();
                }
            };
            
            UpdateUI();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
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
                    _audioSourceBtnClick.Play();
                    UpdateItemsCount(_characterUpgradeUIData[index].upgradeCharacterEnum);
                        //_charactersConfig.Upgrade(_characterUpgradeUIData[index].upgradeCharacterEnum); 
                    UpdateUI();
                });
                _characterMenuElements.Add(element);
                switch (_characterUpgradeUIData[i].upgradeCharacterEnum)
                {
                    case UpgradeCharacterEnum.SpeedMove:
                       // element.UpdateUI(_charactersConfig.speedMoveLevel, _charactersConfig.SpeedMoveCost, _storageData.Count >= _charactersConfig.SpeedMoveCost);
                        break;
                    case UpgradeCharacterEnum.ItemsCount:
                       // element.UpdateUI(_charactersConfig.itemsCountLevel, _charactersConfig.ItemsCountCost, _storageData.Count >= _charactersConfig.ItemsCountCost);
                        break;
                    case UpgradeCharacterEnum.SpeedWork:
                       // element.UpdateUI(_charactersConfig.speedWorkLevel, _charactersConfig.SpeedWorkCost, _storageData.Count >= _charactersConfig.SpeedWorkCost);
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
                  //  _storageData.Count -= _charactersConfig.SpeedMoveCost;
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                  //  _storageData.Count -= _charactersConfig.ItemsCountCost;
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    //_storageData.Count -= _charactersConfig.SpeedWorkCost;
                    break;
            }

            // foreach (var character in _charactersOnMap.CharactersList)
            // {
            //     // if (character.CurrentState is IdleState)
            //     // {
            //     //     //_buildingsOnMap.UpdateCurrentBuilding(character);
            //     // }
            // }
            _inventoryView.UpdateUI(null);
        }

        private void UpdateUI()
        {
           // _characterMenuElements[0].UpdateUI(_charactersConfig.speedMoveLevel, _charactersConfig.SpeedMoveCost, _storageData.Count >= _charactersConfig.SpeedMoveCost);
           // _characterMenuElements[1].UpdateUI(_charactersConfig.speedWorkLevel, _charactersConfig.SpeedWorkCost, _storageData.Count >= _charactersConfig.SpeedWorkCost);
           // _characterMenuElements[2].UpdateUI(_charactersConfig.itemsCountLevel, _charactersConfig.ItemsCountCost, _storageData.Count >= _charactersConfig.ItemsCountCost);
        }

        
    }

}