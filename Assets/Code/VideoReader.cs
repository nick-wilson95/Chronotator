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

    private const int MinCutFrames = 100;
    private const int WebUrlSkipFrameDelay = 30;
    private const int DefaultSkipFrameDelay = 3;

    private Coroutine skipFrameCoroutine;
    private long previousVideoPlayerFrame;
    private int skipFrameDelay;

    private readonly List<Texture2D> textures = new();

    public bool IsReading { get; private set; } = false;
    public UnityEvent<List<Texture2D>> OnFinishReading { get; } = new UnityEvent<List<Texture2D>>();

    private void Start()
    {
        videoPlayer.sendFrameReadyEvents = true;
        videoPlayer.frameReady += (_,_) => OnFrameReady();

        settings.OnVideoDropdownSelection.AddListener(x => ReadFromClip(x));
        settings.OnVideoLocalUrlSelection.AddListener(x => ReadFromUrl(x, false));
        settings.OnVideoWebUrlSelection.AddListener(x => ReadFromUrl(x, true));
    }

    private void Update()
    {
        if (!IsReading) return;

        HandleStuckFrameSkipping();

        var shouldCut = Input.GetKeyDown(KeyCode.Space) && textures.Count >= MinCutFrames;
        var finishedVideo = (int)videoPlayer.frame > (int)videoPlayer.frameCount - 3;

        if (shouldCut || finishedVideo)
        {
            OnFinishReading.Invoke(textures);
            videoPreview.Close();
            IsReading = false;
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

    private void ReadFromUrl(string url, bool isWebUrl)
    {
        var skipFrameDelay = isWebUrl
            ? WebUrlSkipFrameDelay
            : DefaultSkipFrameDelay;

        videoPlayer.url = url;
        ReadVideo(url, skipFrameDelay);
    }

    private void ReadFromClip(Video video)
    {
        if (videoPlayer.clip != video.Clip)
        {
            videoPlayer.clip = video.Clip;
            ReadVideo("", DefaultSkipFrameDelay);
        }
        else if (videoPlayer.source != VideoSource.VideoClip)
        {
            videoPlayer.source = VideoSource.VideoClip;
            ReadVideo("", DefaultSkipFrameDelay);
        }
    }

    private void ReadVideo(string url, int skipFrameDelay)
    {
        this.skipFrameDelay = skipFrameDelay;

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
        if (DepthSurpassingWidth())
        {
            return;
        }

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

        if (textures.Count > MinCutFrames)
        {
            videoPreview.ShowCanCut();
        }
    }

    private bool DepthSurpassingWidth()
    {
        var depthAsProportionOfWidth = textures.Count / videoPreview.Rect.width;
        var frameProportion = videoPlayer.frame / (float)videoPlayer.frameCount;
        return depthAsProportionOfWidth > frameProportion;
    }

    private void HandleStuckFrameSkipping()
    {
        if (skipFrameCoroutine == null)
        {
            var currentFrame = videoPlayer.frame;

            skipFrameCoroutine = this.WaitFramesAndAct(skipFrameDelay, () =>
            {
                if (videoPlayer.frame == currentFrame)
                {
                    videoPlayer.StepForward();
                }

                skipFrameCoroutine = null;
            });
        }
    }
}
