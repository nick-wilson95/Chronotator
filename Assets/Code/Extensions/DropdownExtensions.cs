using System.Collections.Generic;
using System.Linq;
using TMPro;

public static class DropdownExtensions
{
    public static string GetCurrentOption(this TMP_Dropdown dropdown)
    {
        return dropdown.options[dropdown.value].text;
    }

    public static void SetOptions(this TMP_Dropdown dropdown, IEnumerable<string> options)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(options.ToList());
    }
}
