using UnityEngine;

public class Discoverable : Interactable
{
    public AreaId area;
    public string discoveryId;

    private bool collected = false;

    public override void Interact(CCplayer ccplayer)
    {
        if (collected) return;
        bool added = ProgressManager.instance.TryAddDiscovery(area, discoveryId);

        if (added)
        {
            collected = true;
            Debug.Log("Discovery found: " + discoveryId);

        }

    }
}
