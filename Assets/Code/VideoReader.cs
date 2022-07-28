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
    private long previousVideoPlayerFrame;

    private Coroutine skipFrameCoroutine;
    private const float skipFrameDelay = 0.1f;

    private void Start()
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += (_,_) => OnFrameReady();

        settings.OnVideoDropdownSelection.AddListener(x => ReadFromClip(x));
        settings.OnVideoUrlSelection.AddListener(x => ReadFromUrl(x));
    }

    private void Update()
    {
        if (IsReading && skipFrameCoroutine == null)
        {
            var currentFrame = videoPlayer.frame;

            skipFrameCoroutine = this.WaitAndAct(skipFrameDelay, () =>
            {
                if (videoPlayer.frame == currentFrame)
                {
                    videoPlayer.StepForward();
                    Debug.Log("Skipping frame");
                }

                skipFrameCoroutine = null;
            });

            if ((int)videoPlayer.frame > (int)videoPlayer.frameCount - 3)
            {
                OnFinishReading.Invoke(textures);
                videoPreview.Close();
                IsReading = false;
            }
        }
    }

    private void OnFrameReady()
    {
        if (!IsReading) return;

        if (videoPlayer.frame != previousVideoPlayerFrame)
        {
            this.OnFrameEnd(ReadTexture);
        }

        videoPlayer.StepForward();

        previousVideoPlayerFrame = videoPlayer.frame;
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

            previousVideoPlayerFrame = -1;
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
