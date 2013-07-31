using UnityEngine;
using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	public class InvokeAfterFrame : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region properties
		protected Action delayInvokeCallback;
		int delayFrame;
		int startFrame;
		#endregion

		#region public methods
		public static IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> Call(Action callback)
		{
			IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> asyncOps = new InvokeAfterFrame(callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public static IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> Call(Action callback, int delayFrame)
		{
			IAsyncOperationScript<OperationOutputs, AsyncOperationErrors> asyncOps = new InvokeAfterFrame(callback, delayFrame);
			asyncOps.Execute();
			return asyncOps;
		}

		public InvokeAfterFrame(Action callback) : this(callback, 1)
		{
		}

		public InvokeAfterFrame(Action callback, int delayFrame)
		{
			delayInvokeCallback = callback;
			this.delayFrame = delayFrame;
			name = "InvokeAfterFrame";
		}

		public override void Execute()
		{
			startFrame = Time.frameCount;
			base.Execute();
		}
		#endregion

		#region private methods
		protected override IEnumerator ExecuteCore()
		{
			while (true)
			{
				int elapsed = Time.frameCount - startFrame;
				if (elapsed >= delayFrame)
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
