using TMPro;
using UnityEngine;

public class WarningDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private Coroutine endWarningCoroutine;

    private void Start()
    {
        Warn("Use the arrow keys to move the cube...");
    }

    public void Warn(string warning)
    {
        if (endWarningCoroutine != null)
        {
            StopCoroutine(endWarningCoroutine);
        }

        text.enabled = true;
        text.text = warning;

        endWarningCoroutine = this.WaitSecondsAndAct(10, () => text.enabled = false);
    }
}
