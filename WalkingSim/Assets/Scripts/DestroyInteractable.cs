using UnityEngine;

public class DestroyInteractable : Interactable 
{
    public override void Interact (CCplayer ccplayer)
    {
        Destroy(gameObject);
        Debug.Log("Destroyed:");
    }


}
