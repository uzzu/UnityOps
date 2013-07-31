using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	public abstract class AsyncOperationCollective<TOutputs, TErrors> : AsyncOperationScript<TOutputs, TErrors>
		where TOutputs : OperationOutputs, new()
		where TErrors : AsyncOperationErrors, new()
	{
		#region public methods
		#endregion

		#region private method
		protected abstract void Process();

		protected override IEnumerator ExecuteCore()
		{
			try
			{
				Process();
			}
			catch (Exception e)
			{
				HandleException(e);
			}
			yield break;
		}
		#endregion
	}
}
