using System.Collections;

namespace UnityOps
{
	public interface IOperationScript<TOutputs, TErrors>
		where TOutputs : OperationOutputs
		where TErrors : OperationErrors
	{
		event SuccessEventHandler<TOutputs> Success;

		event ErrorEventHandler<TErrors> Error;

		void Execute();
	}
}
