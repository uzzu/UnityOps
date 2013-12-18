using System;
using System.Collections;

namespace UnityOps
{
    public class ProgressEventArgs<TProgress> : EventArgs where TProgress : OperationProgress
    {
        public readonly TProgress Progress;

        public ProgressEventArgs(TProgress progress)
        {
            Progress = progress;
        }
    }
}
