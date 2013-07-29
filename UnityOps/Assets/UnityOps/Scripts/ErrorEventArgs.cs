using System;

namespace UnityOps
{
	public class ErrorEventArgs<TErrors> : EventArgs
		where TErrors : OperationErrors
	{
		public readonly TErrors Errors;

		public ErrorEventArgs(TErrors errors)
		{
			Errors = errors;
		}
	}
}
