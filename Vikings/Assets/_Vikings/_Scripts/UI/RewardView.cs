using PanelManager.Scripts.Interfaces;
using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : ViewBase, IAcceptArg<IronSourceController>
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;

    [SerializeField] private Button _watchButton;
    [SerializeField] private Button _closeButton;
    
    private IronSourceController _ironSourceController;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _closeButton.OnClickAsObservable().Subscribe(_ =>
        {
            _panelManager.PlaySound(UISoundType.Close);
            gameObject.SetActive(false);
        }).AddTo(_panelManager.Disposable);
    }

    public void AcceptArg(IronSourceController arg)
    {
        _ironSourceController = arg;
        
        _watchButton.OnClickAsObservable().Subscribe(_ =>
        {
            _panelManager.PlaySound(UISoundType.Open);
            _ironSourceController.ShowRewardVideo();
        }).AddTo(_panelManager.Disposable);
    }
}
