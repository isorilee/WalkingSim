using UnityEngine;

public class AreaZone : MonoBehaviour
{
    public AreaId area;

    private void OnTriggerEnter(Collider other)
    {
      CCplayer player = other.GetComponent<CCplayer>();

        if(player == null )
        {
            player = other.GetComponentInParent<CCplayer>();
        }

        if (player == null) return;
        
        ProgressUI progressUI = FindFirstObjectByType<ProgressUI>();
        if (progressUI != null) 

        {
            progressUI.SetArea(area);
        }

    }

  
}
