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
        settings.OnVideoSelection.AddListener(x => ReadFromCip(x));
        settings.OnVideoUrlSelection.AddListener(x => ReadFromUrl(x));
    }

    private void Update()
    {
        if (!IsReading) return;

        if ((int)videoPlayer.frame < (int)videoPlayer.frameCount - 2)
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

    private void ReadFromUrl(string url)
    {
        videoPlayer.url = url;
        ReadVideo();
    }

    private void ReadFromCip(Video video)
    {
        videoPlayer.clip = video.Clip;
        ReadVideo();
    }

    private void ReadVideo()
    {
        this.OnVideoLoaded(videoPlayer, () =>
        {
            videoPreview.enabled = true;
            videoPlayer.frame = 0;
            videoPlayer.Pause();

            FitVideoInsideScreen();

            textures.Clear();
            IsReading = true;
        });
    }

    // Ensures video preview takes up less than half the width of the screen
    private void FitVideoInsideScreen()
    {
        var scaleDown = videoPlayer.texture.width * 2 > Screen.width
            ? GetScaleDown()
            : 1;

        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, videoPlayer.texture.width / scaleDown);
        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, videoPlayer.texture.width / scaleDown);
    }

    private int GetScaleDown()
    {
        var logDiff = Mathf.Log(videoPlayer.texture.width, 2) - Mathf.Log(Screen.width, 2);

        return (int)Mathf.Pow(2, Mathf.CeilToInt(logDiff + 1));
    }

    private void ReadTexture()
    {
        if (videoPlayer.frame > 0)
        {
            var previewWidth = (int)videoPreviewRect.rect.width;
            var previewHeight = (int)videoPreviewRect.rect.width * videoPlayer.texture.height / videoPlayer.texture.width;
            previewHeight = Mathf.Clamp(previewHeight, 0, Screen.height);

            var texture = new Texture2D(previewWidth, previewHeight, TextureFormat.RGB24, false);

            var previewRect = new Rect((Screen.width - previewWidth) / 2, (Screen.height - previewHeight) / 2, previewWidth, previewHeight);

            texture.ReadPixels(previewRect, 0, 0);
            texture.Apply();

            textures.Add(texture);
        }
    }
}
