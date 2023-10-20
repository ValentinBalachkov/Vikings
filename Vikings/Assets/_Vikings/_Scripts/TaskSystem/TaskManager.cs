using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static Action<TaskData, TaskStatus> taskChangeStatusCallback;

    [SerializeField] private TaskData[] _tasksData;
    [SerializeField] private TrayView _trayView;

    private List<TaskData> _taskQueue = new();

    private void Start()
    {
        var tasks = _tasksData.Where(x => x.taskStatus != TaskStatus.Done && x.taskStatus != TaskStatus.NotAdded).OrderBy(x => x.taskStatus).ToList();

        DebugLogger.SendMessage(tasks.Count.ToString(), Color.cyan);
        
        foreach (var task in _tasksData)
        {
            task.taskDoneCallback += SetSuccessStatus;
        }

        for (int i = 0; i < tasks.Count; i++)
        {
            AddTaskToQueue(tasks[i]);
        }

        taskChangeStatusCallback += OnTaskChangeStatus;

        if (!PlayerPrefs.HasKey("FirstLoading"))
        {
            DebugLogger.SendMessage("Task 1", Color.cyan);
            var task = _tasksData.FirstOrDefault(x => x.id == 2);
            taskChangeStatusCallback?.Invoke(task, TaskStatus.IsSuccess);
            PlayerPrefs.SetInt( "FirstLoading", 1);
        }
        if(tasks.Count > 0)
        {
            _trayView.UpdateTrayPanel(_taskQueue);
        }
    }

    private void SetSuccessStatus(TaskData taskData)
    {
        var data = _tasksData.FirstOrDefault(x => x.id == taskData.id + 1);
        if (data == null)
        {
            return;
        }
        taskChangeStatusCallback?.Invoke(data, TaskStatus.IsSuccess);
    }

    private void OnDestroy()
    {
        taskChangeStatusCallback -= OnTaskChangeStatus;

        foreach (var task in _tasksData)
        {
            task.taskDoneCallback = null;
        }
    }

    private void AddTaskToQueue(TaskData taskData)
    {
        DebugLogger.SendMessage("Task 3", Color.cyan);
        _taskQueue.Add(taskData);
    }
    private void RemoveTaskFromQueue(TaskData taskData)
    {
        _taskQueue.Remove(taskData);
    }

    private void OnTaskChangeStatus(TaskData taskData, TaskStatus status)
    {
        taskData.taskStatus = status;
        DebugLogger.SendMessage("Task 2", Color.cyan);
        switch (taskData.taskStatus)
        {
            case TaskStatus.IsSuccess:
                AddTaskToQueue(taskData);
                break;
            case TaskStatus.InProcess:
                if (taskData.accessDone)
                {
                    taskChangeStatusCallback?.Invoke(taskData, TaskStatus.TakeReward);
                }
                break;
            case TaskStatus.Done:
                taskData.taskDoneCallback?.Invoke(taskData);
                RemoveTaskFromQueue(taskData);
                break;
        }
        DebugLogger.SendMessage("Task 2 1", Color.cyan);
        _trayView.UpdateTrayPanel(_taskQueue);
    }


}