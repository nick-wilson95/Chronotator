using UnityEngine;

public class VideoPreview : MonoBehaviour
{
    [SerializeField] private VideoReader videoReader;

    void Start()
    {
        videoReader.OnFinishReading.AddListener(_ => Destroy(gameObject)); 
    }
}
