using UnityEngine;
using TMPro;

public class ProgressUI : MonoBehaviour 
{
    public TextMeshProUGUI progressText;

    private AreaId currentArea = AreaId.Woods;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void SetArea(AreaId newArea)
    {
        currentArea = newArea; 
        UpdateUI();

    }


    private void UpdateUI()
    {
        if (progressText == null)
        {
            Debug.Log("ProgressUI: progressText is null");
            return;
        }
        if (ProgressManager.instance == null)
        {
            progressText.text = "No ProgressManager found";
            return;

        }

        int required = ProgressManager.instance.requiredFindsPerArea;
        int found = ProgressManager.instance.GetFoundCount(currentArea);
        int left = Mathf.Max(0, required - found);

        progressText.text = $"{currentArea}:{found}/{required} found ({left} left)";
    }

}
