using System;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Building;
using Vikings.Object;
using Vikings.SaveSystem;

[CreateAssetMenu(fileName = "TaskData", menuName = "Data/TaskData")]
public class TaskData : ScriptableObject, ISave
{
    public string saveKey;
    public Action<TaskData> taskDoneCallback;
    public int id;

    public TaskStatus TaskStatus
    {
        get => _taskDynamicData.taskStatus;
        set => _taskDynamicData.taskStatus = value;
    }

    public bool AccessDone {
        get => _taskDynamicData.accessDone;
        set => _taskDynamicData.accessDone = value;
    }

    public ItemCount[] reward;

    public Sprite icon;

    private TaskDynamicData _taskDynamicData;
    

    public string descriptionNewTask;
    public string descriptionCurrentTask;
    public string descriptionReward;
    
    public void Save()
    {
        SaveLoadSystem.SaveData(_taskDynamicData, saveKey);
    }

    public void Init()
    {
        _taskDynamicData = new();
        _taskDynamicData = SaveLoadSystem.LoadData(_taskDynamicData, saveKey);
        SaveLoadManager.saves.Add(this);
    }
}
