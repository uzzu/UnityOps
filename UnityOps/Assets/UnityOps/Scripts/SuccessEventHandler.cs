using UnityEngine;
using System.Collections;

namespace UnityOps
{
    public delegate void SuccessEventHandler<TOutputs>(object sender, SuccessEventArgs<TOutputs> e) where TOutputs : OperationOutputs;
}

