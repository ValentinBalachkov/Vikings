using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static Action<TaskData, TaskStatus> taskChangeStatusCallback;

    private TrayView _trayView;
    private List<TaskData> _taskQueue = new();
    private ConfigSetting _configSetting;


    public void Init(MainPanelManager panelManager, ConfigSetting configSetting)
    {
        _configSetting = configSetting;

        foreach (var task in _configSetting.tasksData)
        {
            task.Init();
        }

        _trayView = panelManager.SudoGetPanel<TrayView>();
        panelManager.OpenPanel<TrayView>();

        SubscribeChangeStatusEvent();

        var tasks = _configSetting.tasksData
            .Where(x => x.TaskStatus != TaskStatus.Done && x.TaskStatus != TaskStatus.NotAdded)
            .OrderBy(x => x.TaskStatus).ToList();


        foreach (var task in _configSetting.tasksData)
        {
            task.taskDoneCallback += SetSuccessStatus;
        }

        foreach (var data in tasks)
        {
            AddTaskToQueue(data);
        }

        if (!PlayerPrefs.HasKey("FirstLoading"))
        {
            var task = _configSetting.tasksData.FirstOrDefault(x => x.id == 2);
            taskChangeStatusCallback?.Invoke(task, TaskStatus.IsSuccess);
            PlayerPrefs.SetInt("FirstLoading", 1);
        }

        if (tasks.Count > 0)
        {
            _trayView.UpdateTrayPanel(_taskQueue);
        }
    }

    public void SubscribeChangeStatusEvent()
    {
        taskChangeStatusCallback += OnTaskChangeStatus;
    }

    private void SetSuccessStatus(TaskData taskData)
    {
        var data = _configSetting.tasksData.FirstOrDefault(x => x.id == taskData.id + 1);
        if (data == null)
        {
            return;
        }

        taskChangeStatusCallback?.Invoke(data, TaskStatus.IsSuccess);
    }

    private void OnDestroy()
    {
        taskChangeStatusCallback -= OnTaskChangeStatus;

        foreach (var task in _configSetting.tasksData)
        {
            task.taskDoneCallback = null;
        }
    }

    private void AddTaskToQueue(TaskData taskData)
    {
        if (_taskQueue.Contains(taskData))
        {
            return;
        }
        _taskQueue.Add(taskData);
    }

    private void RemoveTaskFromQueue(TaskData taskData)
    {
        _taskQueue.Remove(taskData);
    }

    private void OnTaskChangeStatus(TaskData taskData, TaskStatus status)
    {
        taskData.TaskStatus = status;

        switch (taskData.TaskStatus)
        {
            case TaskStatus.IsSuccess:
                AddTaskToQueue(taskData);
                break;
            case TaskStatus.InProcess:
                if (taskData.AccessDone)
                {
                    taskChangeStatusCallback?.Invoke(taskData, TaskStatus.TakeReward);
                }

                break;
            case TaskStatus.Done:
                taskData.taskDoneCallback?.Invoke(taskData);
                RemoveTaskFromQueue(taskData);
                break;
        }

        _trayView.UpdateTrayPanel(_taskQueue);
    }
}