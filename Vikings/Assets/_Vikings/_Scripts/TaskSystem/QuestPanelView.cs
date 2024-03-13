using System.Linq;
using PanelManager.Scripts.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vikings.Building;
using Vikings.UI;

public class QuestPanelView : ViewBase
{
    public override PanelType PanelType => PanelType.Screen;
    public override bool RememberInHistory => false;
    
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _descriptionReward;

    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconReward;
    [SerializeField] private TMP_Text _rewardCount;

    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _acceptBtn;
    [SerializeField] private TMP_Text _acceptBtnText;
    [SerializeField] private Button _closeRewardBtn;

    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private StorageData[] _storagesData;


    public void SetNewQuest(TaskData taskData)
    {
        _closeBtn.onClick.AddListener(CloseWindow);
        _header.text = "New quest";
        _descriptionReward.text = "Your reward";
        _description.text = taskData.descriptionNewTask;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _closeRewardBtn.gameObject.SetActive(false);
        _acceptBtnText.text = "Accept";
        _acceptBtn.onClick.AddListener((() =>
        {
            TaskManager.taskChangeStatusCallback?.Invoke(taskData, TaskStatus.InProcess);
            CloseWindow();
        }));
        gameObject.SetActive(true);
    }

    public void SetCurrentQuest(TaskData taskData)
    {
        _closeBtn.onClick.AddListener(CloseWindow);
        _header.text = "Current quest";
        _descriptionReward.text = "We will get";
        _description.text = taskData.descriptionCurrentTask;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _closeRewardBtn.gameObject.SetActive(false);
        _acceptBtnText.text = "Ok";
        _acceptBtn.onClick.AddListener(CloseWindow);
        gameObject.SetActive(true);
    }

    public void SetReward(TaskData taskData)
    {
        _closeBtn.onClick.AddListener(CloseWindow);
        _header.text = "Quest completed";
        _descriptionReward.text = "Your reward";
        _description.text = taskData.descriptionReward;
        _icon.sprite = taskData.icon;
        _iconReward.sprite = taskData.reward[0].itemData.icon;
        _rewardCount.text = taskData.reward[0].count.ToString();
        _closeRewardBtn.gameObject.SetActive(true);
        _acceptBtnText.text = "Accept";
        _acceptBtn.onClick.AddListener((() =>
        {
            var storage = _storagesData.FirstOrDefault(x => x.ItemType.ID == taskData.reward[0].itemData.ID);
            storage.Count += taskData.reward[0].count;
            _inventoryView.UpdateUI(storage.ItemType);
            TaskManager.taskChangeStatusCallback?.Invoke(taskData, TaskStatus.Done);
            CloseWindow();
        }));

        _closeRewardBtn.onClick.AddListener(CloseWindow);
        gameObject.SetActive(true);
    }

    private void CloseWindow()
    {
        _acceptBtn.onClick.RemoveAllListeners();
        _closeRewardBtn.onClick.RemoveAllListeners();
        _closeBtn.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}