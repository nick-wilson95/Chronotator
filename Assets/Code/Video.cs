using System;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public struct Video
{
    [SerializeField] private string name;
    [SerializeField] private VideoClip clip;

    public string Name => name;
    public VideoClip Clip => clip;
}
