using System.Collections.Generic;
using PanelManager.Scripts.Panels;
using UnityEngine;
using UnityEngine.UI;

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
    
    [SerializeField] private QuestPanelView _questPanelView;

    [SerializeField] private GameObject _advertisementPanel;
    [SerializeField] private Button _advertisementButton;

    [SerializeField] private IronSourceController _ironSourceController;

    private List<TrayElement> _trayElements = new();

    private TrayElement _advertisementOnTray;

    private void Start()
    {
        // _advertisementButton.onClick.AddListener((() =>
        // {
        //     _ironSourceController.ShowRewardVideo();
        // }));
    }

    public void UpdateTrayPanel(List<TaskData> tasksData)
    {
        ClearElements();
        foreach (var task in tasksData)
        {
            var taskObject = Instantiate(_trayElement, _content);
            _trayElements.Add(taskObject);

            switch (task.taskStatus)
            {
                case TaskStatus.IsSuccess:
                    taskObject.Init(task, _successImage, (() =>
                    {
                        _questPanelView.SetNewQuest(task);
                    }));
                    break;
                case TaskStatus.InProcess:
                    taskObject.Init(task, _processImage, (() =>
                    {
                        _questPanelView.SetCurrentQuest(task);
                    }));
                    break;
                case TaskStatus.TakeReward:
                    taskObject.Init(task, _rewardImage, (() =>
                    {
                        _questPanelView.SetReward(task);
                    }));
                    break;
            }
        }
    }

    public void AddAdvertisementOnPanel()
    {
        _advertisementOnTray = Instantiate(_trayElement, _content);
        _advertisementOnTray.Init(_advertisementImage, (() =>
        {
            _advertisementPanel.SetActive(true);
        }));
    }
    
    public void RemoveAdvertisementOnPanel()
    {
        if (_advertisementOnTray == null)
        {
            return;
        }
        _advertisementPanel.SetActive(false);
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