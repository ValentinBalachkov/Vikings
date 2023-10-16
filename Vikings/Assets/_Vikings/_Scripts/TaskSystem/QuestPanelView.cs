using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanelView : MonoBehaviour
{
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
            TaskManager.taskChangeStatusCallback?.Invoke(taskData, TaskStatus.Done);
            CloseWindow();
        }));
        
        _closeRewardBtn.onClick.AddListener(CloseWindow);
    }

    private void CloseWindow()
    {
        _acceptBtn.onClick.RemoveAllListeners();
        _closeRewardBtn.onClick.RemoveAllListeners();
        _closeBtn.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
