using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TrayElement : MonoBehaviour
{
    public int Id => _taskData.id;
    
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;

    private TaskData _taskData;
    
    public void Init(TaskData taskData, Sprite sprite, UnityAction onClick)
    {
        _icon.sprite = sprite;
        _taskData = taskData;
        _button.onClick.AddListener(onClick);
    }
    
    public void Init(Sprite sprite, UnityAction onClick)
    {
        _icon.sprite = sprite;
        _button.onClick.AddListener(onClick);
    }
}
