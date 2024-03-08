using UnityEngine;
using Vikings.UI;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [SerializeField] private MainPanelManager _mainPanelManager;
    
    
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
        _mainPanelManager.OpenPanel<MenuButtonsManager>();
        _mainPanelManager.OpenPanel<InventoryView>();
        _mainPanelManager.OpenPanel<TrayView>();
    }
}
