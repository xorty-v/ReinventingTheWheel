namespace ThreadPool;

public sealed class SimpleThreadPool : IDisposable
{
    private readonly Queue<Action> _workItems = new();
    private readonly List<Thread> _workers = new();
    private readonly object _lock = new();

    private bool _isStopping;
    private bool _isDisposed;

    public SimpleThreadPool(int workerCount)
    {
        if (workerCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(workerCount), "Worker count must be greater than zero.");

        for (int i = 0; i < workerCount; i++)
        {
            var worker = new Thread(WorkerLoop)
            {
                Name = $"SimpleThreadPool-Worker-{i + 1}",
                IsBackground = true
            };

            _workers.Add(worker);
            worker.Start();
        }
    }

    public void QueueWorkItem(Action workItem)
    {
        if (workItem is null)
            throw new ArgumentNullException(nameof(workItem));

        lock (_lock)
        {
            if (_isStopping || _isDisposed)
                throw new ObjectDisposedException(nameof(SimpleThreadPool));

            _workItems.Enqueue(workItem);

            Monitor.Pulse(_lock);
        }
    }

    private void WorkerLoop()
    {
        while (true)
        {
            Action workItem;

            lock (_lock)
            {
                while (_workItems.Count == 0 && !_isStopping)
                {
                    Monitor.Wait(_lock);
                }

                if (_isStopping && _workItems.Count == 0)
                {
                    return;
                }

                workItem = _workItems.Dequeue();
            }

            try
            {
                workItem();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Worker error: {ex.Message}");
            }
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        lock (_lock)
        {
            if (_isDisposed)
                return;

            _isStopping = true;
            _isDisposed = true;

            Monitor.PulseAll(_lock);
        }

        foreach (var worker in _workers)
        {
            worker.Join();
        }
    }
}