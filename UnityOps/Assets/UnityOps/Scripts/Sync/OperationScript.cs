﻿using System;
using System.Collections;

namespace UnityOps
{
    [Serializable]
    public abstract partial class OperationScript<TOutputs, TErrors> : IOperationScript<TOutputs, TErrors>
        where TOutputs : OperationOutputs
        where TErrors : OperationErrors, new()
    {
        #region inner classes, enum, and structs
        // protected sealed class Result in OperationScript.Result.cs
        #endregion

        #region constants
        #endregion

        #region properties
        public event SuccessEventHandler<TOutputs> Success;
        public event ErrorEventHandler<TErrors> Error;
        #endregion

        #region public methods
        public void Execute()
        {
            try
            {
                Result result = ExecuteCore();
                if (result.IsSuccess() && (Success != null))
                {
                    Success(this, new SuccessEventArgs<TOutputs>(result.Outputs));
                }
                if (result.IsError() && (Error != null))
                {
                    Error(this, new ErrorEventArgs<TErrors>(result.Errors));
                }
            }
            catch (Exception e)
            {
                if (Error != null)
                {
                    Error(this, new ErrorEventArgs<TErrors>(new TErrors() { ExceptionError = true, ExceptionCause = e }));
                }
            }
        }
        #endregion

        #region private methods
        protected abstract Result ExecuteCore();
        #endregion
    }
}
