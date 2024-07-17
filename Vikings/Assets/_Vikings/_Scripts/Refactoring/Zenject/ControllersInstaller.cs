using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Zenject;

public class ControllersInstaller : MonoInstaller
{
    [SerializeField] private MainPanelManager _mainPanelManager;
    [SerializeField] private ConfigSetting _configSetting;
    [SerializeField] private TextureChanger _textureChanger;

    public override void InstallBindings()
    {
        AddConfig();
        AddPanelManager();
        AddTextureChanger();
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

    private void AddTextureChanger()
    {
        Container
            .Bind<TextureChanger>()
            .FromInstance(_textureChanger)
            .AsSingle()
            .NonLazy();
    }
}