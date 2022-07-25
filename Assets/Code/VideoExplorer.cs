using System;
using System.Linq;
using static SimpleFileBrowser.FileBrowser;

public static class VideoExplorer
{
    private static readonly string[] fileExtensions = { "mp4", "mov", "webm", "wmv" };

    public static void SelectVideo(Action<string> onSuccess)
    {
        SetFilters(false, new Filter("", fileExtensions));

        ShowLoadDialog(
            result => onSuccess(result.Single()),
            () => { },
            PickMode.Files);
    }
}
