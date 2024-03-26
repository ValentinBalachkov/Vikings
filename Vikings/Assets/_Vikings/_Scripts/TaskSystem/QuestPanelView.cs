using System.Linq;
using _Vikings._Scripts.Refactoring;
using PanelManager.Scripts.Interfaces;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Map;

public class QuestPanelView : ViewBase, IAcceptArg<MapFactory>
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;

    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _descriptionReward;

    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconReward;
    [SerializeField] private TMP_Text _rewardCount;

    [SerializeField] private Button _acceptBtn;
    [SerializeField] private TMP_Text _acceptBtnText;

    private MapFactory _mapFactory;
    public void SetNewQuest(TaskData taskData)
    {
        _header.text = "New quest";
        _descriptionReward.text = "Your reward";
        _description.text = taskData.descriptionNewTask;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _acceptBtnText.text = "Accept";
        _acceptBtn.onClick.AddListener(() =>
        {
            TaskManager.taskChangeStatusCallback?.Invoke(taskData, TaskStatus.InProcess);
            gameObject.SetActive(false);
        });
        gameObject.SetActive(true);
    }

    public void SetCurrentQuest(TaskData taskData)
    {
        _header.text = "Current quest";
        _descriptionReward.text = "We will get";
        _description.text = taskData.descriptionCurrentTask;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _acceptBtnText.text = "Ok";
        _acceptBtn.onClick.AddListener(() => gameObject.SetActive(false));
        gameObject.SetActive(true);
    }

    public void SetReward(TaskData taskData)
    {
        _header.text = "Quest completed";
        _descriptionReward.text = "Your reward";
        _description.text = taskData.descriptionReward;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _acceptBtnText.text = "Accept";
        _acceptBtn.onClick.AddListener(() =>
        {
            var storage = _mapFactory.GetAllBuildings<Storage>()
                .FirstOrDefault(x => x.ResourceType == taskData.reward[0].itemData.ResourceType);
            storage.SudoChangeCount(taskData.reward[0].count);
            TaskManager.taskChangeStatusCallback?.Invoke(taskData, TaskStatus.Done);
            gameObject.SetActive(false);
        });

        gameObject.SetActive(true);
    }

    public void AcceptArg(MapFactory arg)
    {
        _mapFactory = arg;
    }
}