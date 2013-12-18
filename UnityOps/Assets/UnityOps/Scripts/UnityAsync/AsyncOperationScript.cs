using System;
using System.Collections;
using UnityEngine;

namespace UnityOps.UnityAsync
{
    [Serializable]
    public abstract partial class AsyncOperationScript<TOutputs, TErrors, TProgress> : IAsyncOperationScript<TOutputs, TErrors, TProgress>
        where TOutputs : OperationOutputs, new()
        where TErrors : AsyncOperationErrors, new()
        where TProgress : OperationProgress, new()
    {
        #region properties
        public event SuccessEventHandler<TOutputs> Success;
        public event ErrorEventHandler<TErrors> Error;
        public event ProgressEventHandler<TProgress> Progress;

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
            executor.SendResult = SendResult;
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

        protected virtual void NotifyProgress(float value)
        {
            NotifyProgress(new TProgress() { Value = value });
        }

        protected virtual void NotifyProgress(TProgress progress)
        {
            if (Progress == null)
            {
                return;
            }
            Progress(this, new ProgressEventArgs<TProgress>(progress));
        }

        protected virtual void WaitForResult()
        {
            if (executor == null)
            {
                HandleAbort();
                return;
            }
            executor.WaitForSendResult();
        }

        protected virtual void CompleteWaitForResult()
        {
            if (executor == null)
            {
                HandleAbort();
                return;
            }
            executor.CompleteWaitForResult();
        }

        protected virtual void SendResult()
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
            CompleteWaitForResult();
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
