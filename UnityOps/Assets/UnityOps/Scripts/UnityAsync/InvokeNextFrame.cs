using UnityEngine;
using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	public class InvokeNextFrame : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region properties
		protected Action delayInvokeCallback;
		int startFrame;
		#endregion

		#region public methods
		public static InvokeNextFrame Call(Action callback)
		{
			InvokeNextFrame asyncOps = new InvokeNextFrame(callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public InvokeNextFrame(Action callback)
		{
			delayInvokeCallback = callback;
			name = "InvokeNextFrame";
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
				if (elapsed > 0)
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
