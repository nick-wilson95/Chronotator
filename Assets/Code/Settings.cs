using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private VideoLookup videoLookup;
    [SerializeField] private TMP_Dropdown videoDropdown;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider rotationSlider;

    private Video CurrentVideo;

    [HideInInspector] public UnityEvent<Video> OnVideoChange = new();
    [HideInInspector] public UnityEvent<float> OnSpeedChange = new();
    [HideInInspector] public UnityEvent<float> OnRotationSpeedChange = new();

    private void Start()
    {
        var options = videoLookup.Videos.Select(x => x.Name);
        videoDropdown.SetOptions(options);
        this.OnFrameEnd(BroadcastSettings);
    }

    private void BroadcastSettings()
    {
        OnVideoDropdownChange();
        OnSpeedSliderChange();
        OnRotationSliderChange();
    }

    public void OnVideoDropdownChange()
    {
        var videoName = videoDropdown.GetCurrentOption();

        CurrentVideo = videoLookup.Videos.Single(x => x.Name == videoName);

        OnVideoChange.Invoke(CurrentVideo);
    }

    public void OnSpeedSliderChange()
    {
        var speed = speedSlider.value / speedSlider.maxValue;
        OnSpeedChange.Invoke(speed);
    }

    public void OnRotationSliderChange()
    {
        var rotationSpeed = rotationSlider.value / rotationSlider.maxValue;
        OnRotationSpeedChange.Invoke(rotationSpeed);
    }
}
