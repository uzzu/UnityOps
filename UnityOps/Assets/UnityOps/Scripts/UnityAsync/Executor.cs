using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOps.UnityAsync
{
    public class Executor : MonoBehaviour
    {
        #region properties
        public Func<IEnumerator> ExecuteCoroutine;
        public Action SendResult;
        public Action<Exception> ExceptionCallback;
        public Action AbortCallback;
        protected bool isOrderdExecute = false;
        protected bool isOrderdSendResult = true;
        bool isProcessedWaitForExecuteOrder = false;
        bool isProcessedExecuteCoroutine = false;
        bool isProcessedWaitForSendResultOrder = false;
        bool isProcessedSendResult = false;
        #endregion

        #region public methods
        public void Execute()
        {
            if (isOrderdExecute)
            {
                throw new InvalidOperationException("already executed!");
            }
            isOrderdExecute = true;
        }

        public void WaitForSendResult()
        {
            isOrderdSendResult = false;
        }

        public void CompleteWaitForResult()
        {
            isOrderdSendResult = true;
        }

        public void Cancel()
        {
            StopAllCoroutines();
            this.DestroyObject(gameObject);
        }
        #endregion

        #region override unity methods
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator Start()
        {
            if (!isOrderdExecute)
            {
                yield return StartCoroutine(WaitForExecuteOrder());
            }
            isProcessedWaitForExecuteOrder = true;

            if (ExecuteCoroutine != null)
            {
                yield return StartCoroutine(ExecuteCoroutine());
            }
            isProcessedExecuteCoroutine = true;

            if (!isOrderdSendResult)
            {
                yield return StartCoroutine(WaitForSendResultOrder());
            }
            isProcessedWaitForSendResultOrder = true;

            if (SendResult != null)
            {
                SendResult();
            }
            isProcessedSendResult = true;

            this.DestroyObject(gameObject);
        }

        void OnDestroy()
        {
            if (IsCompleted())
            {
                return;
            }
            if (AbortCallback != null)
            {
                AbortCallback();
            }
        }
        #endregion

        #region private methods
        IEnumerator WaitForExecuteOrder()
        {
            while (!isOrderdExecute)
            {
                yield return null;
            }
        }

        IEnumerator WaitForSendResultOrder()
        {
            while (!isOrderdSendResult)
            {
                yield return null;
            }
        }

        bool IsCompleted()
        {
            return isProcessedWaitForExecuteOrder
                && isProcessedExecuteCoroutine
                && isProcessedWaitForSendResultOrder
                && isProcessedSendResult;
        }

        void DestroyObject(GameObject gameobject)
        {
#if UNITY_EDITOR
            UnityEngine.GameObject.DestroyImmediate(gameobject);
#else
            UnityEngine.GameObject.Destroy(gameobject);
#endif
        }
        #endregion
    }
}
