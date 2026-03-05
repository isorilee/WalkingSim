using UnityEngine;
using System;

public class NPCInteractable : Interactable 
{
    public NPCData npcData;

    public override void Interact (CCplayer ccplayer)
    {
        if(npcData == null)
        {
            Debug.Log("npc has no data: ");

        }
        ccplayer.RequestDialogue(npcData);


    }

    //if we are interacting with the npc and it has data then request dialogue
}
