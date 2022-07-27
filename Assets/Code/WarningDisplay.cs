using TMPro;
using UnityEngine;

public class WarningDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void Warn(string warning)
    {
        text.enabled = true;
        text.text = warning;

        this.WaitAndAct(5, () => text.enabled = false);
    }
}
