using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoLookup : MonoBehaviour
{
    [SerializeField] private List<Video> videos;

    public List<Video> Videos => videos;
}

[Serializable]
public struct Video
{
    [SerializeField] private string name;
    [SerializeField] private VideoClip clip;

    public string Name => name;
    public VideoClip Clip => clip;
}
