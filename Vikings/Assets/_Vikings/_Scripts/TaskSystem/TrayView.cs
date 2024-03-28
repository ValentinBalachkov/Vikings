using System.Collections.Generic;
using PanelManager.Scripts.Panels;
using UnityEngine;

public class TrayView : ViewBase
{
    public override PanelType PanelType => PanelType.Overlay;
    public override bool RememberInHistory => false;
    
    [SerializeField] private Transform _content;
    [SerializeField] private TrayElement _trayElement;


    [SerializeField] private Sprite _successImage;
    [SerializeField] private Sprite _processImage;
    [SerializeField] private Sprite _rewardImage;
    
    [SerializeField] private Sprite _advertisementImage;

    private List<TrayElement> _trayElements = new();

    private TrayElement _advertisementOnTray;
    
    private QuestPanelView _questPanelView;

    private void Start()
    {
        _questPanelView = _panelManager.SudoGetPanel<QuestPanelView>();
    }

    public void UpdateTrayPanel(List<TaskData> tasksData)
    {
        ClearElements();
        foreach (var task in tasksData)
        {
            DebugLogger.SendMessage(tasksData.Count.ToString(), Color.red);
            var taskObject = Instantiate(_trayElement, _content);
            _trayElements.Add(taskObject);

            switch (task.TaskStatus)
            {
                case TaskStatus.IsSuccess:
                    taskObject.Init(task, _successImage, (() =>
                    {
                        _panelManager.PlaySound(UISoundType.Open);
                        _questPanelView.SetNewQuest(task);
                    }));
                    break;
                case TaskStatus.InProcess:
                    taskObject.Init(task, _processImage, (() =>
                    {
                        _panelManager.PlaySound(UISoundType.Open);
                        _questPanelView.SetCurrentQuest(task);
                    }));
                    break;
                case TaskStatus.TakeReward:
                    taskObject.Init(task, _rewardImage, (() =>
                    {
                        _panelManager.PlaySound(UISoundType.Open);
                        _questPanelView.SetReward(task);
                    }));
                    break;
            }
        }
    }

    public void AddAdvertisementOnPanel()
    {
        _advertisementOnTray = Instantiate(_trayElement, _content);
        _advertisementOnTray.Init(_advertisementImage, () =>
        {
            _panelManager.PlaySound(UISoundType.Open);
            _panelManager.OpenPanel<RewardView>();
        });
    }
    
    public void RemoveAdvertisementOnPanel()
    {
        if (_advertisementOnTray == null)
        {
            return;
        }
        _panelManager.ClosePanel<RewardView>();
       Destroy(_advertisementOnTray.gameObject);
       _advertisementOnTray = null;
    }

    private void ClearElements()
    {
        if (_trayElements.Count == 0)
        {
            return;
        }

        foreach (var element in _trayElements)
        {
            Destroy(element.gameObject);
        }
        _trayElements.Clear();
    }


}