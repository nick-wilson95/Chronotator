using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPreview : MonoBehaviour
{
    [SerializeField] private RawImage videoPreview;
    [SerializeField] private RectTransform videoPreviewRect;
    [SerializeField] private Image videoPreviewBackground;

    public void Close()
    {
        videoPreview.enabled = false;
        videoPreviewBackground.enabled = false;
    }

    public void Prepare(VideoPlayer videoPlayer)
    {
        videoPreview.enabled = true;
        videoPreviewBackground.enabled = true;

        FitVideoPreview(videoPlayer.texture);
    }

    public Rect GetRect(VideoPlayer videoPlayer)
    {
        var previewWidth = (int)videoPreviewRect.rect.width;
        var previewHeight = (int)videoPreviewRect.rect.width * videoPlayer.texture.height / videoPlayer.texture.width;
        previewHeight = Mathf.Clamp(previewHeight, 0, Screen.height);

       return new Rect((Screen.width - previewWidth) / 2, (Screen.height - previewHeight) / 2, previewWidth, previewHeight);
    }

    // Ensures video preview takes up less than half the width of the screen
    private void FitVideoPreview(Texture texture)
    {
        var scaleDown = texture.width * 2 > Screen.width
            ? GetScaleDown(texture)
            : 1;

        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture.width / scaleDown);
        videoPreviewRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture.width / scaleDown);
    }

    private int GetScaleDown(Texture texture)
    {
        var logDiff = Mathf.Log(texture.width, 2) - Mathf.Log(Screen.width, 2);

        return (int)Mathf.Pow(2, Mathf.CeilToInt(logDiff + 1));
    }
}
