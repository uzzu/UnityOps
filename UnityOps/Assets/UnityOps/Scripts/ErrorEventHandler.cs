using System.Collections;

namespace UnityOps
{
	public delegate void ErrorEventHandler<TErrors>(object sender, ErrorEventArgs<TErrors> e)
		where TErrors : OperationErrors;
}

