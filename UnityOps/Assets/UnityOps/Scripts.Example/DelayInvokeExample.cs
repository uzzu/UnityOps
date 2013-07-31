using UnityEngine;
using System.Collections;
using UnityOps.UnityAsync;

public class DelayInvokeExample : MonoBehaviour
{
	#region properties
	int startFrame;
	float startTime;
	#endregion

	#region override unity methods
	void Start()
	{
		startFrame = Time.frameCount;
		startTime = Time.realtimeSinceStartup;

		// use InvokeAfterDelay
		InvokeAfterDelay.Call(() =>
		{
			Debug.Log(string.Format("InvokeAfterDelay called! delayedTime: {0}", Time.realtimeSinceStartup - startTime));
		}, 1.0f);

		// use InvokeAfterFrame
		InvokeAfterFrame.Call(() =>
		{
			Debug.Log(string.Format("InvokeAfterFrame called! startFrame: {0}, currentFrame: {1}", startFrame, Time.frameCount));
		}, 5);

		// use InvokeNextFrame
		InvokeNextFrame.Call(() =>
		{
			Debug.Log(string.Format("InvokeNextFrame called! startFrame: {0}, currentFrame: {1}", startFrame, Time.frameCount));
		});
	}
	#endregion

	#region private methods
	#endregion
}

