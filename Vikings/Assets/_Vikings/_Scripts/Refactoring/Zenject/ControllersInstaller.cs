using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Map;
using Vikings.UI;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [SerializeField] private MainPanelManager _mainPanelManager;
    [SerializeField] private MapFactory _mapFactory;
    [SerializeField] private CharactersTaskManager _charactersTaskManager;
    [SerializeField] private WeaponFactory _weaponFactory;
    [SerializeField] private ConfigSetting _configSetting;
    
    
    public override void InstallBindings()
    {
        AddPanelManager();
        AddConfig();
    }

    public override void Start()
    {
        base.Start();
        InitPanelManager();
    }

    private void AddPanelManager()
    {
        Container
            .Bind<MainPanelManager>()
            .FromInstance(_mainPanelManager)
            .AsSingle()
            .NonLazy();
    }
    
    private void AddConfig()
    {
        Container
            .Bind<ConfigSetting>()
            .FromInstance(_configSetting)
            .AsSingle()
            .NonLazy();
    }

    private void InitPanelManager()
    {
        _mainPanelManager.Init();
        _mainPanelManager.ActiveOverlay(true);
        
        _mainPanelManager.SudoGetPanel<CraftAndBuildingMenu>().AcceptArg(_mapFactory, _charactersTaskManager, _weaponFactory);
    }
}
