﻿using UnityEngine;
using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	[Serializable]
	public class InvokeAfterDelay : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region properties
		protected Action delayInvokeCallback;
		float delay;
		float startTime;
		#endregion

		#region public methods
		public static InvokeAfterDelay Call(Action callback)
		{
			InvokeAfterDelay asyncOps = new InvokeAfterDelay(callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public static InvokeAfterDelay Call(Action callback, float delay)
		{
			InvokeAfterDelay asyncOps = new InvokeAfterDelay(callback, delay);
			asyncOps.Execute();
			return asyncOps;
		}

		public static InvokeAfterDelay Call(string name, Action callback, float delay)
		{
			InvokeAfterDelay asyncOps = new InvokeAfterDelay(name, callback, delay);
			asyncOps.Execute();
			return asyncOps;
		}

		public InvokeAfterDelay(Action callback) : this(callback, 0.0f)
		{
		}

		public InvokeAfterDelay(Action callback, float delay) : this("InvokeAfterDelay", callback, 0.0f)
		{
		}

		public InvokeAfterDelay(string name, Action callback, float delay)
		{
			this.name = name;
			this.delayInvokeCallback = callback;
			this.delay = delay;
			this.nullResultIsSuccess = true;
		}

		public override void Execute()
		{
			startTime = Time.realtimeSinceStartup;
			base.Execute();
		}
		#endregion

		#region private methods
		protected override IEnumerator ExecuteCore()
		{
			while (true)
			{
				float elapsed = Time.realtimeSinceStartup - startTime;
				if (elapsed >= delay)
				{
					break;
				}
				yield return null;
			}
			try
			{
				delayInvokeCallback();
			}
			catch (Exception e)
			{
				HandleException(e);
			}
		}
		#endregion
	}
}

