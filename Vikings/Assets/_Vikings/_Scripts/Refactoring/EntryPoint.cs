﻿using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Map;
using Vikings.Object;
using Vikings.UI;
using Zenject;

namespace _Vikings._Scripts.Refactoring
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SaveLoadManager _saveLoadManager;
        [SerializeField] private CharactersTaskManager _charactersTaskManager;
        [SerializeField] private TaskManager _taskManager;
        [SerializeField] private IronSourceController _ironSourceController;

        private MainPanelManager _mainPanelManager;
        private MapFactory _mapFactory;
        private CharacterFactory _characterFactory;
        private WeaponFactory _weaponFactory;
        private ConfigSetting _configSetting;
        private EatStorage _eatStorage;

        [Inject]
        public void Init(MainPanelManager mainPanelManager, MapFactory mapFactory, CharacterFactory characterFactory,
            WeaponFactory weaponFactory, ConfigSetting configSetting)
        {
            _mainPanelManager = mainPanelManager;
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            _weaponFactory = weaponFactory;
            _configSetting = configSetting;
        }

        private void Start()
        {
            _mainPanelManager.Init();
            _mainPanelManager.ActiveOverlay(true);

            _saveLoadManager.LoadAll();
            _mapFactory.CreateBoneFire(_mainPanelManager);
            _weaponFactory.CreateWeapons(_configSetting.weaponsData);
            InitBuildings();
            InitPanelManager();
            _taskManager.Init(_mainPanelManager, _configSetting);
        }

        private void InitBuildings()
        {
            _eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();

            var storages = _mapFactory.GetAllBuildings<Storage>();

            var craftingTable = _mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault();
            craftingTable.BuildingComplete += _charactersTaskManager.SetCharactersToCrafting;

            var weapon = _weaponFactory.GetAllWeapons().FirstOrDefault(x => x.IsSet);

            if (weapon != null)
            {
                craftingTable.AcceptArg(weapon);
            }

            foreach (var storage in storages)
            {
                storage.StorageNeedItem += _charactersTaskManager.SetCharacterToStorage;
                storage.BuildingComplete += _charactersTaskManager.SetCharactersToCrafting;
            }

            InitCharacters(_eatStorage.CurrentLevel.Value + 1);
           
        }

        private void InitCharacters(int count)
        {
            _characterFactory.Init(_configSetting, _weaponFactory, count);

            var characters = _characterFactory.GetCharacters();
            
            _saveLoadManager.GetResources();

            var currentBuilding = _mapFactory.GetAllBuildings<AbstractBuilding>().FirstOrDefault(x =>
                x.buildingState == BuildingState.InProgress || x.buildingState == BuildingState.Crafting);

            foreach (var character in characters)
            {
                if (currentBuilding != null)
                {
                    _charactersTaskManager.setBuildingToQueue.Execute(currentBuilding);
                }
                else
                {
                    _charactersTaskManager.SetCharacterToStorage(character);
                }
                
            }

            _mainPanelManager.SudoGetPanel<UpgradeCharacterMenu>()
                .AcceptArg(_characterFactory.CharacterManager, _eatStorage);
        }

        private void InitPanelManager()
        {
            _mainPanelManager.SudoGetPanel<CraftAndBuildingMenu>()
                .AcceptArg(_mapFactory, _charactersTaskManager, _weaponFactory);
            _mainPanelManager.SudoGetPanel<RewardView>().AcceptArg(_ironSourceController);
            _mainPanelManager.SudoGetPanel<QuestPanelView>().AcceptArg(_mapFactory);
        }
    }
}