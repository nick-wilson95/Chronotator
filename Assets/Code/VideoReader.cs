using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoReader : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RectTransform videoRect;

    private List<Texture2D> textures = new List<Texture2D>();

    public UnityEvent<List<Texture2D>> OnFinishReading { get; } = new UnityEvent<List<Texture2D>>();

    public void Awake()
    {
        videoPlayer.Pause();
    }

    public void Update()
    {
        if ((int)videoPlayer.frame < (int)videoPlayer.frameCount - 1)
        {
            videoPlayer.StepForward();
            this.OnFrameEnd(ReadTexture);
        }
        else
        {
            OnFinishReading.Invoke(textures);
            Destroy(gameObject);
        }
    }

    public void ReadTexture()
    {
        if ((int)videoPlayer.frame > 0)
        {
            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            var rect = new Rect(0, 0, Screen.width, Screen.height);

            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            textures.Add(texture);
        }
    }
}