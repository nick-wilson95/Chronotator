using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoReader : MonoBehaviour
{
    [SerializeField] private WarningDisplay warningDisplay;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoPreview videoPreview;
    [SerializeField] private Settings settings;

    private readonly List<Texture2D> textures = new();
    public UnityEvent<List<Texture2D>> OnFinishReading { get; } = new UnityEvent<List<Texture2D>>();
    public bool IsReading { get; private set; } = false;

    private void Start()
    {
        settings.OnVideoDropdownSelection.AddListener(x => ReadFromClip(x));
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
            videoPreview.Close();
            IsReading = false;
        }
    }

    private void ReadFromUrl(string url)
    {
        videoPlayer.url = url;
        ReadVideo(url);
    }

    private void ReadFromClip(Video video)
    {
        if (videoPlayer.clip != video.Clip)
        {
            videoPlayer.clip = video.Clip;
            ReadVideo("");
        }
        else if (videoPlayer.source != VideoSource.VideoClip)
        {
            videoPlayer.source = VideoSource.VideoClip;
            ReadVideo("");
        }
    }

    private void ReadVideo(string url)
    {
        this.OnVideoLoaded(videoPlayer, () =>
        {
            if (videoPlayer.url != url)
            {
                warningDisplay.Warn($"Can't find video at URL '{url}'");
                return;
            }

            textures.Clear();
            
            videoPlayer.Pause();

            videoPreview.Prepare(videoPlayer);

            IsReading = true;
        });
    }

    private void ReadTexture()
    {
        if (videoPlayer.frame > 0)
        {
            var texture = new Texture2D(
                (int)videoPreview.Rect.width,
                (int)videoPreview.Rect.height,
                TextureFormat.RGB24,
                false
            );

            texture.ReadPixels(videoPreview.Rect, 0, 0);
            texture.Apply();

            textures.Add(texture);
        }
    }
}
