using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private VideoLookup videoLookup;
    [SerializeField] private TMP_Dropdown videoDropdown;
    [SerializeField] private Button videoBrowser;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider rotationSlider;

    private List<Selectable> Selectables => new() { videoDropdown, videoBrowser, speedSlider, rotationSlider };

    private Video CurrentVideo;

    [HideInInspector] public UnityEvent<Video> OnVideoSelection = new();
    [HideInInspector] public UnityEvent<string> OnVideoUrlSelection = new();
    [HideInInspector] public UnityEvent<float> OnSpeedChange = new();
    [HideInInspector] public UnityEvent<float> OnRotationSpeedChange = new();

    private void Start()
    {
        var options = videoLookup.Videos.Select(x => x.Name);
        videoDropdown.SetOptions(options);
        this.OnFrameEnd(BroadcastInitialSettings);
    }

    private void BroadcastInitialSettings()
    {
        OnVideoDropdownChange();
        OnSpeedSliderChange();
        OnRotationSliderChange();
    }

    public void OnVideoDropdownChange()
    {
        var videoName = videoDropdown.GetCurrentOption();

        CurrentVideo = videoLookup.Videos.Single(x => x.Name == videoName);

        OnVideoSelection.Invoke(CurrentVideo);
    }

    public void OnVideoBrowserClick()
    {
        DisableSettings();

        VideoExplorer.SelectVideo(
            x => { OnVideoUrlSelection.Invoke(x); EnableSettings(); },
            EnableSettings
        );
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

    private void EnableSettings()
    {
        Selectables.ForEach(x => x.interactable = true);
    }

    private void DisableSettings()
    {
        Selectables.ForEach(x => x.interactable = false);
    }
}
