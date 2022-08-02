using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public static class MonoBehaviourExtensions
{
    private static WaitForEndOfFrame FrameEnd { get; } = new WaitForEndOfFrame();

    public static void OnFrameEnd(this MonoBehaviour monoBehaviour, Action action)
    {
        monoBehaviour.StartCoroutine(OnFrameEnd(action));
    }

    public static IEnumerator OnFrameEnd(Action action)
    {
        yield return FrameEnd;

        action();
    }

    public static void OnVideoLoaded(this MonoBehaviour monoBehaviour, VideoPlayer videoPlayer, Action action)
    {
        monoBehaviour.StartCoroutine(OnVideoLoaded(videoPlayer, action));
    }

    private static IEnumerator OnVideoLoaded(VideoPlayer videoPlayer, Action action)
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        action();
    }

    public static Coroutine WaitSecondsAndAct(this MonoBehaviour monoBehaviour, float seconds, Action action)
    {
        return monoBehaviour.StartCoroutine(WaitSecondsAndAct(seconds, action));
    }

    private static IEnumerator WaitSecondsAndAct(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);

        action();
    }

    public static Coroutine WaitFramesAndAct(this MonoBehaviour monoBehaviour, int frames, Action action)
    {
        return monoBehaviour.StartCoroutine(WaitFramesAndAct(frames, action));
    }

    private static IEnumerator WaitFramesAndAct(int Frames, Action action)
    {
        for (var i = 0; i < Frames; i++)
        {
            yield return null;
        }

        action();
    }
}