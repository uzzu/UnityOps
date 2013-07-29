using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	[Serializable]
	public class AsyncOperationDelegate : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region public methods
		public AsyncOperationDelegate(Func<IEnumerator> callback) : this(callback, "AsyncOperationDelegate")
		{
		}

		public AsyncOperationDelegate(Func<IEnumerator> callback, string name) : base()
		{
			this.name = name;
			this.callback = callback;
			nullResultIsSuccess = true;
		}
		#endregion

		#region private methods
		/// <summary>
		/// Nop (because async-operation delegates callback function)
		/// </summary>
		/// <returns>
		/// The core.
		/// </returns>
		protected override IEnumerator ExecuteCore()
		{
			yield return null;
		}
		#endregion
	}
}
