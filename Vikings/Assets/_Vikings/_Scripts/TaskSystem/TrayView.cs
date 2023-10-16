using System.Collections.Generic;
using UnityEngine;

public class TrayView : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private TrayElement _trayElement;


    [SerializeField] private Sprite _successImage;
    [SerializeField] private Sprite _processImage;
    [SerializeField] private Sprite _rewardImage;
    
    [SerializeField] private QuestPanelView _questPanelView;
    

    private List<TrayElement> _trayElements = new();

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
    }


}