using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	[Serializable]
		/// <summary>
		/// Async operation delegate.
		/// [Warning] An exception cannot be handled in this object.
		/// </summary>
	public class AsyncOperationDelegate : AsyncOperationScript<OperationOutputs, AsyncOperationErrors>
	{
		#region public methods
		public static AsyncOperationDelegate Call(Func<IEnumerator> callback)
		{
			AsyncOperationDelegate asyncOps = new AsyncOperationDelegate(callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public static AsyncOperationDelegate Call(string name, Func<IEnumerator> callback)
		{
			AsyncOperationDelegate asyncOps = new AsyncOperationDelegate(name, callback);
			asyncOps.Execute();
			return asyncOps;
		}

		public AsyncOperationDelegate(Func<IEnumerator> callback) : this("AsyncOperationDelegate", callback)
		{
		}

		public AsyncOperationDelegate(string name, Func<IEnumerator> callback) : base()
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
