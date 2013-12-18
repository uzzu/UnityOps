using System.Collections;

namespace UnityOps
{
    public interface IAsyncOperationScript<TOutputs, TErrors, TProgress> : IOperationScript<TOutputs, TErrors>
        where TOutputs : OperationOutputs
        where TErrors : OperationErrors
        where TProgress : OperationProgress
    {
        event ProgressEventHandler<TProgress> Progress;

        void Cancel();
    }
}
