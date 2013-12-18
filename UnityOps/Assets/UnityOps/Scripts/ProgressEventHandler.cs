using UnityEngine;
using System.Collections;

namespace UnityOps
{
    public delegate void ProgressEventHandler<TProgress>(object sender, ProgressEventArgs<TProgress> e) where TProgress : OperationProgress;
}
