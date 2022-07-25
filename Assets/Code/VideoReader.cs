using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoReader : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Settings settings;
    [SerializeField] private RawImage videoPreview;
    [SerializeField] private RectTransform videoPreviewRect;

    private readonly List<Texture2D> textures = new();
    public UnityEvent<List<Texture2D>> OnFinishReading { get; } = new UnityEvent<List<Texture2D>>();
    public bool IsReading { get; private set; } = false;

    private void Start()
    {
        settings.OnVideoChange.AddListener(ReadVideo);
    }

    private void Update()
    {
        if (!IsReading) return;

        if ((int)videoPlayer.frame < (int)videoPlayer.frameCount - 1)
        {
            videoPlayer.StepForward();
            this.OnFrameEnd(ReadTexture);
        }
        else
        {
            OnFinishReading.Invoke(textures);
            videoPreview.enabled = false;
            IsReading = false;
        }
    }

    private void ReadVideo(Video video)
    {
        videoPreview.enabled = true;
        videoPlayer.clip = video.Clip;
        videoPlayer.frame = 0;
        videoPlayer.Pause();

        FitVideoOutsideScreen();

        textures.Clear();
        IsReading = true;
    }

    private void FitVideoOutsideScreen()
    {
        var widthRatio = (float)videoPlayer.clip.width / Screen.width;
        var heightRatio = (float)videoPlayer.clip.height / Screen.height;

        var scale = heightRatio > widthRatio
            ? Screen.width
            : Screen.width * (widthRatio / heightRatio);

        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scale);
        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scale);
    }

    public void ReadTexture()
    {
        if (videoPlayer.frame > 0)
        {
            var texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            var rect = new Rect(0, 0, Screen.width, Screen.height);

            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            textures.Add(texture);
        }
    }
}
