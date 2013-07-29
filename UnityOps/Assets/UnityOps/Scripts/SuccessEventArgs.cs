using System;
using System.Collections.Generic;

namespace UnityOps
{
	public class SuccessEventArgs<TOutputs> : EventArgs where TOutputs : OperationOutputs
	{
		public readonly TOutputs Outputs;

		public SuccessEventArgs(TOutputs outputs)
		{
			Outputs = outputs;
		}
	}
}
