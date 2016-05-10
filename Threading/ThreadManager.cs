using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.ComponentModel;

public class ThreadManager : SingletonMono<ThreadManager>,ISynchronizeInvoke
{
	Queue<UnityAsyncResult> fifoToExecute = new Queue<UnityAsyncResult>();
	Thread mainThread;
	static bool initialized;

	public static void Init()
	{
		if (initialized)
			return;

		var go = new GameObject("[Thread Manager]");

		_instance = go.AddComponent < ThreadManager>();
	}

	void Awake()
	{
		mainThread = Thread.CurrentThread;
	}

	void Update()
	{
		ProcessQueue();
	}

	public bool InvokeRequired { get { return mainThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId; } }

	public IAsyncResult BeginInvoke(Delegate method, object[] args)
	{
		var asyncResult = new UnityAsyncResult() {
			method = method,
			args = args,
			IsCompleted = false,
			AsyncWaitHandle = new ManualResetEvent(false),
		};
		lock (fifoToExecute)
		{
			fifoToExecute.Enqueue(asyncResult);
		}
		return asyncResult;
	}

	public IAsyncResult BeginInvoke(Action method, object[] args)
	{
		var asyncResult = new UnityAsyncResult() {
			method = method,
			args = args,
			IsCompleted = false,
			AsyncWaitHandle = new ManualResetEvent(false),
		};
		lock (fifoToExecute)
		{
			fifoToExecute.Enqueue(asyncResult);
		}
		return asyncResult;
	}

	public object EndInvoke(IAsyncResult result)
	{
		if (!result.IsCompleted)
		{
			result.AsyncWaitHandle.WaitOne();
		}
		return result.AsyncState;
	}

	public object Invoke(Delegate method, object[] args)
	{
		if (InvokeRequired)
		{            
			var asyncResult = BeginInvoke(method, args);
			return EndInvoke(asyncResult);
		}
		else
		{
			return method.DynamicInvoke(args);
		}
	}

	public object Invoke(Action method, object[] args)
	{
		if (InvokeRequired)
		{            
			var asyncResult = BeginInvoke(method, args);
			return EndInvoke(asyncResult);
		}
		else
		{
			return method.DynamicInvoke(args);
		}
	}

	void ProcessQueue()
	{
		if (Thread.CurrentThread != mainThread)
		{
			throw new Exception("Must be called from the same thread it was created on");
		}
		bool loop = true;
		UnityAsyncResult data = null;
		while (loop)
		{
			lock (fifoToExecute)
			{
				loop = fifoToExecute.Count > 0;
				if (!loop)
					break;
				data = fifoToExecute.Dequeue();
			}

			data.AsyncState = Invoke(data.method, data.args);
			data.IsCompleted = true;
		}
	}

}

public class UnityAsyncResult : IAsyncResult
{
	public Delegate method;
	public object[] args;

	public bool IsCompleted { get; set; }

	public WaitHandle AsyncWaitHandle { get; internal set; }

	public object AsyncState { get; set; }

	public bool CompletedSynchronously { get { return IsCompleted; } }
}

public static class ISynchronizeInvokeExtension
{
	public static IAsyncResult BeginInvoke(this ISynchronizeInvoke synchronizeInvoke, Delegate method, object[] args)
	{
		return ThreadManager.Instance.BeginInvoke(method, args);
	}

	public static void Invoke(this ISynchronizeInvoke synchronizeInvoke, Action method, object[] args)
	{
		ThreadManager.Instance.Invoke(method, args);
	}
}