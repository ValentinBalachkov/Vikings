using System;
using SecondChanceSystem.SaveSystem;
using UnityEngine;
using Vikings.Building;

[CreateAssetMenu(fileName = "TaskData", menuName = "Data/TaskData")]
public class TaskData : ScriptableObject, IData
{
    public Action<TaskData> taskDoneCallback;
    public int id;
    public TaskStatus taskStatus;

    public bool accessDone;

    public PriceToUpgrade[] reward;

    public Sprite icon;
    

    public string descriptionNewTask;
    public string descriptionCurrentTask;
    public string descriptionReward;

    private int _taskStatusId;

    public void Save()
    {
        _taskStatusId = (int)taskStatus;
        SaveLoadSystem.SaveData(this);
    }

    public void Load()
    {
        var data = SaveLoadSystem.LoadData(this) as TaskData;
        if (data != null)
        {
            accessDone = data.accessDone;
            taskStatus = (TaskStatus)data._taskStatusId;
        }
    }
}
