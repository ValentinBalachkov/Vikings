using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Vikings.UI;

public class MenuButtonsManager : ViewBase
{
    public override PanelType PanelType => PanelType.Overlay;
    public override bool RememberInHistory => false;
    
    [SerializeField] private Button _craftButton;
    [SerializeField] private Button _upgradeButton;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _craftButton.OnClickAsObservable().Subscribe((_ =>
        {
            _panelManager.OpenPanel<CraftAndBuildingMenu>();
            
        }));
    }


    public void EnableButtons(bool isEnable)
    {
        _craftButton.interactable = isEnable;
    }
    
}
