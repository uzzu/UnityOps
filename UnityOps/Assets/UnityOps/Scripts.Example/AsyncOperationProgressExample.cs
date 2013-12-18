using UnityEngine;
using System.Collections;
using UnityOps;
using UnityOps.UnityAsync;

public class AsyncOperationProgressExample : MonoBehaviour
{
    #region inner classes, enum, and structs
    public class FizzBuzzOutputs : OperationOutputs
    {
        public string ScriptName;
        public int EndCount;
    }

    public class FizzBuzzProgress : OperationProgress
    {
        public string ScriptName;
        public int CurrentCount;
    }

    public class FizzBuzzErrors : AsyncOperationErrors
    {
        public bool InvalidEndCountError;
        public int InvalidEndCount;
        public string ScriptName;
    }

    public class FizzBuzzScript : AsyncOperationCollective<FizzBuzzOutputs, FizzBuzzErrors, FizzBuzzProgress>
    {
        int endCount;
        int count;
        FizzBuzzProgress progress;

        public FizzBuzzScript(string name, int endCount)
        {
            this.name = name;
            this.endCount = endCount;
            count = 1;
            progress = new FizzBuzzProgress() { Value = 0, CurrentCount = count, ScriptName = name };
        }

        protected override void Process()
        {
            WaitForResult();
            InvokeNextFrame.Call(FizzBuzz);
        }

        void FizzBuzz()
        {
            if (endCount < 1)
            {
                result = new FizzBuzzErrors() { ScriptName = this.name, InvalidEndCountError = true, InvalidEndCount = endCount };
                CompleteWaitForResult();
                return;
            }

            string outputs = string.Empty;
            if (count % 3 == 0 && count % 5 == 0)
            {
                outputs = "FizzBuzz";
            }
            else if (count % 3 == 0)
            {
                outputs = "Fizz";
            }
            else if (count % 5 == 0)
            {
                outputs = "Buzz";
            }
            else
            {
                outputs = count.ToString();
            }
            Debug.Log(string.Format("[{0}] {1}\n", name, outputs));

            progress.Value = ((float) count) / ((float) endCount);
            progress.CurrentCount = count;
            NotifyProgress(progress);

            if (count >= endCount)
            {
                result = new FizzBuzzOutputs() { ScriptName = this.name, EndCount = endCount };
                CompleteWaitForResult();
                return;
            }

            count++;
            InvokeNextFrame.Call(FizzBuzz);
        }
    }
    #endregion

    #region override unity methods
    void Start()
    {
        FizzBuzzScript s1 = new FizzBuzzScript("Basic100", 100);
        s1.Success += HandleSuccess;
        s1.Error += HandleError;
        s1.Execute();

        FizzBuzzScript s2 = new FizzBuzzScript("Progress33", 33);
        s2.Success += HandleSuccess;
        s2.Progress += HandleProgress;
        ;
        s2.Error += HandleError;
        s2.Execute();

        FizzBuzzScript s3 = new FizzBuzzScript("Error-1", -1);
        s3.Success += HandleSuccess;
        s3.Progress += HandleProgress;
        ;
        s3.Error += HandleError;
        s3.Execute();
    }
    #endregion

    #region private methods
    void HandleProgress(object sender, ProgressEventArgs<FizzBuzzProgress> e)
    {
        Debug.Log(string.Format("[{0}] progress: {1}, current: {2}\n", e.Progress.ScriptName, e.Progress.Value, e.Progress.CurrentCount));
    }

    void HandleSuccess(object sender, SuccessEventArgs<FizzBuzzOutputs> e)
    {
        Debug.Log(string.Format("[{0}] complete: {1}\n", e.Outputs.ScriptName, e.Outputs.EndCount));
    }

    void HandleError(object sender, ErrorEventArgs<FizzBuzzErrors> e)
    {
        string cause = string.Empty;
        if (e.Errors.InvalidEndCountError)
        {
            cause = string.Format("InvalidEndCount => {0}\n", e.Errors.InvalidEndCount);
        }
        else if (e.Errors.AbortedError)
        {
            cause = "Aborted";
        }
        else if (e.Errors.ExceptionError)
        {
            cause = string.Format("Exception => {0}", e.Errors.ExceptionCause.Message);
        }
        Debug.Log(string.Format("[{0}] error: {1}\n", e.Errors.ScriptName, cause));
    }
    #endregion
}

