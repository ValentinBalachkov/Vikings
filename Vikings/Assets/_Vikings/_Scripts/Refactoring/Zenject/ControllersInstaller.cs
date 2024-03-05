using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [SerializeField] private MainPanelManager _mainPanelManager;
    
    
    public override void InstallBindings()
    {
        AddPanelManager();
    }
    
    
    
    private void AddPanelManager()
    {
        Container
            .Bind<MainPanelManager>()
            .FromInstance(_mainPanelManager)
            .AsSingle()
            .NonLazy();
    }
}
