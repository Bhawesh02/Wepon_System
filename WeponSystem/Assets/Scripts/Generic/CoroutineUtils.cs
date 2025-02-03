using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public static class CoroutineUtils
{
    public static IEnumerator Delay(float delayInSeconds, Action finishedCallback)
    {
        yield return new WaitForSeconds(delayInSeconds);
        finishedCallback?.Invoke();
    }
    
    public static IEnumerator DelayNextFrame(Action finishedAction)
    {
        yield return new WaitForNextFrameUnit();
        finishedAction?.Invoke();
    }
}