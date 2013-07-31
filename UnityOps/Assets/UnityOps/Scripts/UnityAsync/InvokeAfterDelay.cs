﻿using UnityEngine;
using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	public class InvokeAfterDelay : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region properties
		protected Action delayInvokeCallback;
		float delay;
		float startTime;
		#endregion

		#region public methods
		public static IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> Call(Action callback)
		{
			IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> asyncOps = new InvokeAfterDelay(callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public static IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> Call(Action callback, float delay)
		{
			IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> asyncOps = new InvokeAfterDelay(callback, delay);
			asyncOps.Execute();
			return asyncOps;
		}

		public InvokeAfterDelay(Action callback) : this(callback, 0.0f)
		{
		}

		public InvokeAfterDelay(Action callback, float delay)
		{
			delayInvokeCallback = callback;
			this.delay = delay;
			name = "InvokeAfterDelay";
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
			delayInvokeCallback();
		}
		#endregion
	}
}
