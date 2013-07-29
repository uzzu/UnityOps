using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOps.UnityAsync
{
	public class Executor : MonoBehaviour
	{
		#region properties
		public bool IsOrderdExecute;

		public Func<IEnumerator> ExecuteCoroutine;

		public Action CheckForResult;

		public Action<Exception> ExceptionCallback;

		public Action AbortCallback;
		#endregion

		bool isProcessedWaitForExecuteOrder = false;
		bool isProcessedExecuteCoroutine = false;
		bool isProcessedCheckForResult = false;

		#region public methods
		public void Execute()
		{
			if (IsOrderdExecute)
			{
				throw new InvalidOperationException("already executed!");
			}
			IsOrderdExecute = true;
		}

		public void Cancel()
		{
			StopAllCoroutines();
			this.DestroyObject(gameObject);
		}
		#endregion

		#region override unity methods
		IEnumerator Start()
		{
			yield return StartCoroutine(WaitForExecuteOrder());
			isProcessedWaitForExecuteOrder = true;
			if (ExecuteCoroutine != null)
			{
				yield return StartCoroutine(ExecuteCoroutine());
			}
			isProcessedExecuteCoroutine = true;
			if (CheckForResult != null)
			{
				CheckForResult();
			}
			isProcessedCheckForResult = true;
			this.DestroyObject(gameObject);
		}

		void OnDestroy()
		{
			if (IsOrderdExecute
				&& isProcessedWaitForExecuteOrder
				&& isProcessedExecuteCoroutine
				&& isProcessedCheckForResult)
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
			while (!IsOrderdExecute)
			{
				yield return null;
			}
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
