using System;
using System.Collections;
using UnityEngine;

namespace UnityOps.UnityAsync
{
	[Serializable]
	public abstract partial class AsyncOperationScript<TOutputs, TErrors> : IAsyncOperationScript<TOutputs, TErrors>
		where TOutputs : OperationOutputs, new()
		where TErrors : AsyncOperationErrors, new()
	{
		#region properties
		public event SuccessEventHandler<TOutputs> Success;
		public event ErrorEventHandler<TErrors> Error;

		protected string name;
		protected Func<IEnumerator> callback;
		protected Executor executor;
		protected Result result;
		protected bool nullResultIsSuccess;
		#endregion

		#region public methods
		public AsyncOperationScript()
		{
			name = "UnityAsyncOperationScript";
			callback = ExecuteCore;
		}

		public virtual void Execute()
		{
			GameObject gameObject = new GameObject(name);
			executor = gameObject.AddComponent<Executor>();
			executor.ExecuteCoroutine = callback;
			executor.CheckForResult = CheckForResult;
			executor.AbortCallback = HandleAbort;
			executor.Execute();
		}

		public void Cancel()
		{
			if (executor == null)
			{
				return;
			}
			executor.Cancel();
		}
		#endregion

		#region private methods
		/// <summary>
		/// Executes the core.
		/// Set to result if script was finished
		/// </summary>
		/// <returns>
		/// Enumerator
		/// </returns>
		protected abstract IEnumerator ExecuteCore();

		protected virtual void CheckForResult()
		{
			if (result == null)
			{
				if (nullResultIsSuccess)
				{
					if (Success != null)
					{
						Success(this, new SuccessEventArgs<TOutputs>(new TOutputs()));
					}
				}
				else
				{
					if (Error != null)
					{
						Error(this, new ErrorEventArgs<TErrors>(new TErrors(){ ResultWasNullError = true }));
					}
				}
				return;
			}
			if (result.IsSuccess() && (Success != null))
			{
				Success(this, new SuccessEventArgs<TOutputs>(result.Outputs));
			}
			if (result.IsError() && (Error != null))
			{
				Error(this, new ErrorEventArgs<TErrors>(result.Errors));
			}
		}

		protected virtual void HandleException(Exception e)
		{
			result = new TErrors() { ExceptionError = true, ExceptionCause = e };
		}

		protected virtual void HandleAbort()
		{
			if (Error != null)
			{
				Error(this, new ErrorEventArgs<TErrors>(new TErrors() { AbortedError = true }));
			}
		}
		#endregion
	}
}
