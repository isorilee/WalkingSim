using UnityEngine;
using TMPro;

public class ProgressUI : MonoBehaviour 
{
    public TextMeshProUGUI progressText;

    public AreaId areaToTrack = AreaId.Woods;

    private void Update()
    {
        if (ProgressManager.instance == null || progressText == null) return;

        int required = ProgressManager.instance.requiredFindsPerArea;
        int found = ProgressManager.instance.GetFoundCount(areaToTrack);

        int left = required - found;

        progressText.text = $"{areaToTrack}:{found}/{required} found ({left} left)";
    }

}
