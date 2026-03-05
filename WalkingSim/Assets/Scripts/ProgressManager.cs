using UnityEngine;
using System.Collections.Generic;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance {  get; private set; }
    public int requiredFindsPerArea = 3;
    public GameObject GatetoRiver;
    public GameObject GatetoMountains;

    //track unique discoveries per area 
    private readonly HashSet<string> woodsFound = new();
    private readonly HashSet<string> riverFound = new();
    private readonly HashSet <string> mountainsFound = new();

    public bool RiverUnlocked {  get; private set; }
    public bool MountainUnlocked { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;

        //start locked 
        RiverUnlocked = false;
        MountainUnlocked = false;

        if (GatetoRiver != null) GatetoRiver.SetActive(true);
        if (GatetoMountains != null) GatetoMountains .SetActive(true);
    }

    public bool TryAddDiscovery(AreaId area, string discoveryId)
    {
        if(string.IsNullOrWhiteSpace(discoveryId))
        {
            Debug.LogWarning("Discovery ID is empty");
            return false;
        }

        HashSet<string> set = GetSet(area);
        bool added = set.Add(discoveryId);

        if (added)
        {
            Debug.Log($"Discovery added: {area} -> {discoveryId} (count:{set.Count})");
            CheckUnlock(area);
        }

        else
        {
            Debug.Log($"Already found: {area} -> {discoveryId}");
        }

        return added;
    }

    public int GetFoundCount(AreaId area) => GetSet(area).Count;
    private HashSet<string> GetSet(AreaId area)
    {
        return area switch
        {
            AreaId.Woods => woodsFound,
            AreaId.River => riverFound,
            AreaId.Mountains => mountainsFound,
            _ => woodsFound

        };

    }

    private void CheckUnlock (AreaId areaJustUpdated)
    {
        //woods -> unlock River 

        if (areaJustUpdated == AreaId.Woods && !RiverUnlocked && woodsFound.Count >= requiredFindsPerArea )
        {
            RiverUnlocked = true;
            if (GatetoRiver != null) GatetoRiver.SetActive (false);
            Debug.Log("River unclocked");

        }

        if (areaJustUpdated == AreaId.River && !MountainUnlocked && riverFound.Count >= requiredFindsPerArea )

        {
            MountainUnlocked = true;
            if (GatetoMountains != null) GatetoMountains.SetActive (false);
            Debug.Log("Mountains unclocked!");

        }

    }
}
