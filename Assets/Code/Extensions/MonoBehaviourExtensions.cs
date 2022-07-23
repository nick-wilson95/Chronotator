using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    private static WaitForEndOfFrame FrameEnd { get; } = new WaitForEndOfFrame();

    public static void OnFrameEnd(this MonoBehaviour monoBehaviour, Action action)
    {
        monoBehaviour.StartCoroutine(ActOnFrameEnd(action));
    }

    public static IEnumerator ActOnFrameEnd(Action action)
    {
        yield return FrameEnd;

        action();
    }
}