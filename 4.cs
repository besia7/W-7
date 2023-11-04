using System;
using System.Collections.Generic;
using System.Linq;

public class TaskScheduler<TTask, TPriority>
{
    private SortedDictionary<TPriority, Queue<TTask>> taskQueue = new SortedDictionary<TPriority, Queue<TTask>>();
    private Func<TTask, TPriority> prioritySelector;
    private Dictionary<TTask, TPriority> taskPriorities = new Dictionary<TTask, TPriority>();
    private Queue<TTask> pool = new Queue<TTask>();
    private Func<TTask> initializeTask;
    private Action<TTask> resetTask;

    public TaskScheduler(Func<TTask, TPriority> prioritySelector, Func<TTask> initializeTask, Action<TTask> resetTask)
    {
        this.prioritySelector = prioritySelector;
        this.initializeTask = initializeTask;
        this.resetTask = resetTask;
    }

    public void AddTask(TTask task, TPriority priority)
    {
        if (!taskPriorities.ContainsKey(task))
        {
            taskPriorities[task] = priority;
            if (!taskQueue.ContainsKey(priority))
            {
                taskQueue[priority] = new Queue<TTask>();
            }
            taskQueue[priority].Enqueue(task);
        }
    }

    public TTask ExecuteNext(TaskExecution<TTask> taskExecution)
    {
        if (taskQueue.Any())
        {
            var highestPriority = taskQueue.Keys.First();
            var task = taskQueue[highestPriority].Dequeue();
            if (!taskQueue[highestPriority].Any())
            {
                taskQueue.Remove(highestPriority);
            }

            if (initializeTask != null)
            {
                initializeTask(task);
            }

            taskExecution(task);

            if (resetTask != null)
            {
                resetTask(task);
            }

            return task;
        }
        return default;
    }

    public TTask GetTaskFromPool()
    {
        if (pool.Any())
        {
            var task = pool.Dequeue();
            if (initializeTask != null)
            {
                initializeTask(task);
            }
            return task;
        }
        return default;
    }

    public void ReturnTaskToPool(TTask task)
    {
        if (resetTask != null)
        {
            resetTask(task);
        }
        pool.Enqueue(task);
    }
}

public delegate void TaskExecution<TTask>(TTask task);

class Program
{
    static void Main()
    {
        // Приклад використання дженеричного планувальника завдань

        TaskScheduler<string, int> taskScheduler = new TaskScheduler<string, int>(
            task => int.Parse(task.Split('-')[0]),
            () => "Default-Task",
            task => Console.WriteLine($"Resetting task: {task}"));

        taskScheduler.AddTask("1-High Priority Task", 1);
        taskScheduler.AddTask("2-Medium Priority Task", 2);
        taskScheduler.AddTask("3-Low Priority Task", 3);

        // Виконати завдання з найвищим пріоритетом
        var executedTask = taskScheduler.ExecuteNext(task => Console.WriteLine($"Executing task: {task}"));
        Console.WriteLine($"Executed task: {executedTask}");

        // Повернути завдання в пул
        taskScheduler.ReturnTaskToPool(executedTask);

        // Отримати завдання з пулу та виконати
        var pooledTask = taskScheduler.GetTaskFromPool();
        Console.WriteLine($"Task from pool: {pooledTask}");
        taskScheduler.ExecuteNext(task => Console.WriteLine($"Executing task from pool: {task}"));

        // Додати нове завдання
        taskScheduler.AddTask("4-New High Priority Task", 1);
        taskScheduler.ExecuteNext(task => Console.WriteLine($"Executing new high priority task: {task}"));
    }
}
