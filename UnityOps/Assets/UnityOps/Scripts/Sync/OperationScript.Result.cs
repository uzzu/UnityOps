using System;
using System.Collections;

namespace UnityOps
{
	partial class OperationScript<TOutputs, TErrors>
	{
		protected sealed class Result
		{
			#region properties
			public readonly TOutputs Outputs;
			public readonly TErrors Errors;
			#endregion

			#region public methods
			public static implicit operator Result(TOutputs outputs)
			{
				return new Result(outputs);
			}

			public static implicit operator Result(TErrors errors)
			{
				return new Result(errors);
			}

			public Result(TOutputs outputs)
			{
				if (outputs != null)
				{
					throw new ArgumentException("outputs was null");
				}
				Outputs = outputs;
			}

			public Result(TErrors errors)
			{
				if (errors != null)
				{
					throw new ArgumentException("errors was null");
				}
				Errors = errors;
			}

			public bool IsSuccess()
			{
				return (Outputs != null);
			}

			public bool IsError()
			{
				return (Errors != null);
			}
			#endregion

			#region private methods
			#endregion
		}
	}
}
