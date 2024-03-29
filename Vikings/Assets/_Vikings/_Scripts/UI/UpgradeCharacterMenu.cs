using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Interfaces;
using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Vikings.UI
{
    public class UpgradeCharacterMenu : ViewBase, IAcceptArgTwo<CharacterManager, EatStorage>
    {
        public override PanelType PanelType => PanelType.Screen;
        public override bool RememberInHistory => false;
        
        [SerializeField] private CharacterMenuElement _characterMenuElement;
        [SerializeField] private CharacterUpgradeUIData[] _characterUpgradeUIData;
        [SerializeField] private Transform _content;
        [SerializeField] private AudioSource _audioSourceBtnClick;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restartBtn;
        
      
        private List<CharacterMenuElement> _characterMenuElements = new();
        private CharacterManager _characterManager;
        private EatStorage _eatStorage;

        public void AcceptArg(CharacterManager arg, EatStorage arg2)
        {
            _characterManager = arg;
            _eatStorage = arg2;
            
            _eatStorage.ChangeCount += OnUpdateCount;
            Spawn();

            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _panelManager.PlaySound(UISoundType.Close);
                _panelManager.OpenPanel<MenuButtonsManager>();
                gameObject.SetActive(false);
            }).AddTo(_panelManager.Disposable);
            
            _restartBtn.OnClickAsObservable().Subscribe(_ =>
            {
               SaveLoadManager.RestartGame();
            }).AddTo(_panelManager.Disposable);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        private void OnUpdateCount(int value, ResourceType type)
        {
            if (type == _eatStorage.ResourceType)
            {
                UpdateUI();
            }
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            UpdateUI();
        }

        protected override void OnClosed()
        {
            base.OnClosed();
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
                    _panelManager.PlaySound(UISoundType.UpgradePeople);
                   // _audioSourceBtnClick.Play();
                    UpdateItemsCount(_characterUpgradeUIData[index].upgradeCharacterEnum);
                    _characterManager.Upgrade(_characterUpgradeUIData[index].upgradeCharacterEnum); 
                    UpdateUI();
                });
                _characterMenuElements.Add(element);
                switch (_characterUpgradeUIData[i].upgradeCharacterEnum)
                {
                    case UpgradeCharacterEnum.SpeedMove:
                        element.UpdateUI(_characterManager.SpeedMoveLevel, _characterManager.SpeedMoveCost, _eatStorage.Count >= _characterManager.SpeedMoveCost);
                        break;
                    case UpgradeCharacterEnum.ItemsCount:
                        element.UpdateUI(_characterManager.ItemsCountLevel, _characterManager.ItemsCountCost, _eatStorage.Count >= _characterManager.ItemsCountCost);
                        break;
                    case UpgradeCharacterEnum.SpeedWork:
                        element.UpdateUI(_characterManager.SpeedWorkLevel, _characterManager.SpeedWorkCost, _eatStorage.Count >= _characterManager.SpeedWorkCost);
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
                    _eatStorage.SudoChangeCount(-_characterManager.SpeedMoveCost);
                    break;
                case UpgradeCharacterEnum.ItemsCount:
                    _eatStorage.SudoChangeCount(-_characterManager.ItemsCountCost);
                    break;
                case UpgradeCharacterEnum.SpeedWork:
                    _eatStorage.SudoChangeCount(-_characterManager.SpeedWorkCost);
                    break;
            }
        }

        private void UpdateUI()
        {
           _characterMenuElements[0].UpdateUI(_characterManager.SpeedMoveLevel, _characterManager.SpeedMoveCost, _eatStorage.Count >= _characterManager.SpeedMoveCost);
           _characterMenuElements[1].UpdateUI(_characterManager.SpeedWorkLevel, _characterManager.SpeedWorkCost, _eatStorage.Count >= _characterManager.SpeedWorkCost);
           _characterMenuElements[2].UpdateUI(_characterManager.ItemsCountLevel, _characterManager.ItemsCountCost, _eatStorage.Count >= _characterManager.ItemsCountCost);
        }
    }

}