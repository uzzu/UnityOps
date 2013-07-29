using System.Collections;

namespace UnityOps
{
	public interface IAsyncOperationScript<TOutputs, TErrors> : IOperationScript<TOutputs, TErrors>
		where TOutputs : OperationOutputs
		where TErrors : OperationErrors
	{
		void Cancel();
	}
}
