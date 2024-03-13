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
    
    
    public override void InstallBindings()
    {
        AddPanelManager();
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

    private void InitPanelManager()
    {
        _mainPanelManager.Init();
        _mainPanelManager.ActiveOverlay(true);
        
        _mainPanelManager.SudoGetPanel<CraftAndBuildingMenu>().AcceptArg(_mapFactory, _charactersTaskManager);
    }
}
