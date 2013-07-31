using UnityEngine;
using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	[Serializable]
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

		public static InvokeNextFrame Call(string name, Action callback)
		{
			InvokeNextFrame asyncOps = new InvokeNextFrame(name, callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public InvokeNextFrame(Action callback) : this("InvokeNextFrame", callback)
		{
		}

		public InvokeNextFrame(string name, Action callback)
		{
			this.name = name;
			this.delayInvokeCallback = callback;
			this.nullResultIsSuccess = true;
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
