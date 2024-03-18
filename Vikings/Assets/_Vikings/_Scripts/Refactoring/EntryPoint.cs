using _Vikings.Refactoring.Character;
using UnityEngine;
using Vikings.Map;
using Vikings.UI;
using Zenject;

namespace _Vikings._Scripts.Refactoring
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SaveLoadManager _saveLoadManager;
        [SerializeField] private CharactersTaskManager _charactersTaskManager;
        
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
             InitCharacters();
             InitPanelManager();
        }

        private void InitCharacters()
        {
            _characterFactory.Init(_configSetting, _weaponFactory);

            var characters = _characterFactory.GetCharacters();
            
            foreach (var character in characters)
            {
                var boneFire = _mapFactory.GetBoneFire();
                character.SetState<DoMoveState>(boneFire);
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