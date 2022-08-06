using TMPro;
using UnityEngine;

public class WarningDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private VideoReader videoReader;

    private Coroutine endWarningCoroutine;

    private void Start()
    {
        videoReader.OnFinishReading.AddListener(_ => Warn("Use the arrow keys to move the cube..."));
    }

    public void Warn(string warning)
    {
        if (endWarningCoroutine != null)
        {
            StopCoroutine(endWarningCoroutine);
        }

        text.enabled = true;
        text.text = warning;

        endWarningCoroutine = this.WaitSecondsAndAct(5, () => text.enabled = false);
    }
}
