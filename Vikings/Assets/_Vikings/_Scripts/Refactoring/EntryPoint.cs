using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Map;
using Vikings.UI;
using Zenject;

namespace _Vikings._Scripts.Refactoring
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SaveLoadManager _saveLoadManager;
        [SerializeField] private CharactersTaskManager _charactersTaskManager;
        [SerializeField] private BuildingHomeActionController _buildingHomeAction;
        
        
        private MainPanelManager _mainPanelManager;
        private MapFactory _mapFactory;
        private CharacterFactory _characterFactory;
        private WeaponFactory _weaponFactory;
        private ConfigSetting _configSetting;
        
        [Inject]
        public void Init(MainPanelManager mainPanelManager, MapFactory mapFactory, CharacterFactory characterFactory, WeaponFactory weaponFactory, ConfigSetting configSetting)
        {
            _mainPanelManager = mainPanelManager;
            _mapFactory = mapFactory;
            _characterFactory = characterFactory;
            _weaponFactory = weaponFactory;
            _configSetting = configSetting;
        }

        private void Start()
        {
             _saveLoadManager.LoadAll();
             _mapFactory.CreateBoneFire();
             _weaponFactory.CreateWeapons(_configSetting.weaponsData);
             InitPanelManager();
             InitBuildings();
        }

        private void InitBuildings()
        {
            var eatStorage = _mapFactory.GetAllBuildings<EatStorage>().FirstOrDefault();
            _buildingHomeAction.Init(eatStorage);
             
            var storages = _mapFactory.GetAllBuildings<Storage>();
             
            var craftingTable = _mapFactory.GetAllBuildings<CraftingTable>().FirstOrDefault();
            craftingTable.BuildingComplete += _charactersTaskManager.SetCharactersToCrafting;
            craftingTable.AcceptArg(_mainPanelManager);
            
            foreach (var storage in storages)
            {
                storage.StorageNeedItem += _charactersTaskManager.SetCharacterToStorage;
                storage.BuildingComplete += _charactersTaskManager.SetCharactersToCrafting;
                storage.AcceptArg(_mainPanelManager);
            } 
            
            InitCharacters(eatStorage.CurrentLevel.Value + 1);
        }

        private void InitCharacters(int count)
        {
            _characterFactory.Init(_configSetting, _weaponFactory, count);

            var characters = _characterFactory.GetCharacters();
            
            foreach (var character in characters)
            {
                _charactersTaskManager.SetCharacterToStorage(character);
            }
        }

        private void InitPanelManager()
        {
            _mainPanelManager.Init();
            
            _mainPanelManager.ActiveOverlay(true);
        
            _mainPanelManager.SudoGetPanel<CraftAndBuildingMenu>().AcceptArg(_mapFactory, _charactersTaskManager, _weaponFactory);
        }
    }
}