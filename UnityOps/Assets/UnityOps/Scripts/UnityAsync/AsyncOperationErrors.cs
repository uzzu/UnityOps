using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
	[Serializable]
	public class AsyncOperationErrors : OperationErrors
	{
		#region properties
		public bool ResultWasNullError;

		public bool AbortedError;
		#endregion
	}
}
