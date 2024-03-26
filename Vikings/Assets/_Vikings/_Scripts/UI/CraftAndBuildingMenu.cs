using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Building;
using Vikings.Map;
using Vikings.Object;

namespace Vikings.UI
{
    public class CraftAndBuildingMenu : ViewBase, IAcceptArgs<MapFactory, CharactersTaskManager, WeaponFactory>
    {
        public override PanelType PanelType => PanelType.Screen;
        public override bool RememberInHistory => false;

        [SerializeField] private Transform _content;
        [SerializeField] private MenuElement _menuElement;
        [SerializeField] private AudioSource _audioSourceBtnClick;
        [SerializeField] private ConfigSetting _configSetting;

        [SerializeField] private Button _closeButton;

        private List<MenuElement> _menuElements = new();

        private MapFactory _mapFactory;
        private CharactersTaskManager _charactersTaskManager;
        private WeaponFactory _weaponFactory;


        public void AcceptArg(MapFactory arg, CharactersTaskManager arg2, WeaponFactory arg3)
        {
            _mapFactory = arg;
            _charactersTaskManager = arg2;
            _weaponFactory = arg3;
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            CreatePanels();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _closeButton.OnClickAsObservable().Subscribe(_ =>
            {
                _panelManager.OpenPanel<MenuButtonsManager>();
                _panelManager.ClosePanel<CraftAndBuildingMenu>();
            }).AddTo(_panelManager.Disposable);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            ClearPanel();
        }

        private void CreatePanels()
        {
            var storages = _mapFactory.GetAllBuildings<Storage>();

            foreach (var building in storages)
            {
                var item = Instantiate(_menuElement, _content);
                item.UpdateUI(building);
                item.AddOnClickListener(() =>
                {
                    building.ChangeState(BuildingState.InProgress);
                    _charactersTaskManager.setBuildingToQueue.Execute(building);
                    _panelManager.ClosePanel<CraftAndBuildingMenu>();
                    _panelManager.OpenPanel<MenuButtonsManager>();
                    _panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(false);
                });
                var cortege = building.IsEnableToBuild(_mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault());
                item.SetEnable(cortege.Item1, cortege.Item2, cortege.Item3);
                _menuElements.Add(item);
            }

            CreateCraftingTableElement();

            CreateWeaponElements(storages.FirstOrDefault(x => x.GetData().saveKey == "WoodStorage"));


            SortElements();
        }

        private void CreateCraftingTableElement()
        {
            var craftingTable = _mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault();

            var table = Instantiate(_menuElement, _content);
            table.UpdateUI(craftingTable);
            table.AddOnClickListener(() =>
            {
                craftingTable.ChangeState(BuildingState.InProgress);
                _charactersTaskManager.setBuildingToQueue.Execute(craftingTable);
                _panelManager.ClosePanel<CraftAndBuildingMenu>();
                _panelManager.OpenPanel<MenuButtonsManager>();
                _panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(false);
            });
            var arg = craftingTable.IsEnableToBuild(_weaponFactory.GetWeapon(_configSetting.weaponsData[1]));
            table.SetEnable(arg.Item1, arg.Item2, arg.Item3);
            _menuElements.Add(table);
        }


        private void CreateWeaponElements(Storage storage)
        {
            for (int i = 1; i < _configSetting.weaponsData.Count; i++)
            {
                var weapon = _weaponFactory.GetWeapon(_configSetting.weaponsData[i]);

                var item = Instantiate(_menuElement, _content);
                item.UpdateUI(weapon);
                item.AddOnClickListener(() =>
                {
                    _mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault().AcceptArg(weapon);
                    _mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault().ChangeState(BuildingState.Ready);
                    _charactersTaskManager.setBuildingToQueue.Execute(_mapFactory.GetAllBuildings<CraftingTable>()
                        .FirstOrDefault());
                    _panelManager.ClosePanel<CraftAndBuildingMenu>();
                    _panelManager.OpenPanel<MenuButtonsManager>();
                    _panelManager.SudoGetPanel<MenuButtonsManager>().EnableButtons(false);
                });
                if (i == 1)
                {
                    var cortege = weapon.IsEnableToBuild(_mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault(),
                        storage);
                    item.SetEnable(cortege.Item1, cortege.Item2, cortege.Item3);
                    _menuElements.Add(item);
                }
                else if (i == 2)
                {
                    var cortege = weapon.IsEnableToBuild(_mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault());
                    item.SetEnable(cortege.Item1, cortege.Item2, cortege.Item3);
                    _menuElements.Add(item);
                }
            }
        }

        private void ClearPanel()
        {
            foreach (var element in _menuElements)
            {
                Destroy(element.gameObject);
            }

            _menuElements.Clear();
        }


        private void SortElements()
        {
            foreach (var element in _menuElements.OrderByDescending(x => x.priority))
            {
                element.transform.SetAsFirstSibling();
            }
        }
    }
}