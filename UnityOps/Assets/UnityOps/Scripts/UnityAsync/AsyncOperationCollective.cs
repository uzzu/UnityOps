using System;
using System.Collections;

namespace UnityOps.UnityAsync
{
    /// <summary>
    /// Async operation collective.
    /// </summary>
    public abstract class AsyncOperationCollective<TOutputs, TErrors, TProgress> : AsyncOperationScript<TOutputs, TErrors, TProgress>
        where TOutputs : OperationOutputs, new()
        where TErrors : AsyncOperationErrors, new()
        where TProgress : OperationProgress, new()
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
