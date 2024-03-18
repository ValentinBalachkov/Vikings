using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [SerializeField] private MainPanelManager _mainPanelManager;
    [SerializeField] private ConfigSetting _configSetting;

    public override void InstallBindings()
    {
        AddConfig();
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

    private void AddConfig()
    {
        Container
            .Bind<ConfigSetting>()
            .FromInstance(_configSetting)
            .AsSingle()
            .NonLazy();
    }
}