using System;
using System.Threading;
using ThreadPool;

namespace ReinventingTheWheel.Tests.ThreadPool;

public class SimpleThreadPoolTests
{
    /// <summary>
    /// Проверяет, что пул может выполнить одну добавленную задачу.
    /// Задача вызывает ManualResetEventSlim.Set(), а тест ждёт этот сигнал.
    /// Если сигнал пришёл, значит worker-поток забрал задачу из очереди и выполнил её.
    /// </summary>
    [Fact]
    public void QueueWorkItem_ExecutesWorkItem()
    {
        using var pool = new SimpleThreadPool(workerCount: 2);

        using var completed = new ManualResetEventSlim(false);

        pool.QueueWorkItem(completed.Set);

        Assert.True(completed.Wait(TimeSpan.FromSeconds(2)));
    }

    /// <summary>
    /// Проверяет, что пул выполняет все добавленные задачи и не теряет их.
    /// CountdownEvent создаётся с количеством ожидаемых задач.
    /// Каждая выполненная задача вызывает Signal(), уменьшая счётчик на один.
    /// </summary>
    [Fact]
    public void QueueWorkItem_ExecutesAllWorkItems()
    {
        using var pool = new SimpleThreadPool(workerCount: 4);

        const int taskCount = 20;
        using var completed = new CountdownEvent(taskCount);

        for (int i = 0; i < taskCount; i++)
        {
            pool.QueueWorkItem(() => { completed.Signal(); });
        }

        Assert.True(completed.Wait(TimeSpan.FromSeconds(2)));
    }

    /// <summary>
    /// Проверяет, что после Dispose() пул больше не принимает новые задачи.
    /// Сначала пул закрывается, затем тест пытается добавить новую задачу.
    /// Ожидается ObjectDisposedException, потому что объект уже завершил работу.
    /// </summary>
    [Fact]
    public void QueueWorkItem_AfterDispose_ThrowsObjectDisposedException()
    {
        var pool = new SimpleThreadPool(workerCount: 2);

        pool.Dispose();

        Assert.Throws<ObjectDisposedException>(() => { pool.QueueWorkItem(() => { }); });
    }
}