using System.Collections;
using UnityEngine;
using UnityOps.UnityAsync;

public class AsyncOperationDelegateExample : MonoBehaviour
{
    void Start()
    {
        var op1 = new AsyncOperationDelegate(CounterDelegate);
        var op2 = new AsyncOperationDelegate(CounterDelegate);
        var op3 = new AsyncOperationDelegate(CounterDelegate);
        op1.Success += (sender, e) => {
            Debug.Log("finish op1");
        };
        op2.Success += (sender, e) => {
            Debug.Log("finish op2");
        };
        op3.Success += (sender, e) => {
            Debug.Log("finish op3");
        };
        op1.Execute();
        op2.Execute();
        op3.Execute();
    }

    IEnumerator CounterDelegate()
    {
        int count = 0;
        Debug.Log(string.Format("count:{0}", count++));
        yield return null;
        Debug.Log(string.Format("count:{0}", count++));
        yield return null;
        Debug.Log(string.Format("count:{0}", count++));
        yield return null;
    }
}
