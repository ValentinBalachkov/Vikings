using PanelManager.Scripts;

public class MainPanelManager : PanelManagerBase
{
    public void Init()
    {
        CreatePanelsFromSettings();
    }

    private void OnDestroy()
    {
        Disposable.Dispose();
    }
}
