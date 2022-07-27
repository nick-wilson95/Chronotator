using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

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

    public static void OnVideoLoaded(this MonoBehaviour monoBehaviour, VideoPlayer videoPlayer, Action action)
    {
        monoBehaviour.StartCoroutine(ActOnVideoLoaded(videoPlayer, action));
    }

    public static IEnumerator ActOnVideoLoaded(VideoPlayer videoPlayer, Action action)
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        action();
    }

    public static void WaitAndAct(this MonoBehaviour monoBehaviour, int seconds, Action action)
    {
        monoBehaviour.StartCoroutine(ActAfterTimeElapsed(seconds, action));
    }

    public static IEnumerator ActAfterTimeElapsed(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);

        action();
    }
}