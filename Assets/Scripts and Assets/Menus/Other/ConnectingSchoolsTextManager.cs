using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ConnectingSchoolsTextManager : NetworkBehaviour
{
    public static ConnectingSchoolsTextManager Instance { get; private set; }
    
    public GameObject connectingText;
    public GameObject schoolsText;
    
    private bool canShowText = true;
    
    public void Awake()
    {
        connectingText.SetActive(false);
        schoolsText.SetActive(false);
        
        canShowText = true;
        
        Instance = this;
    }

    public void Spawn()
    {
        if (IsServer)
        {
            Spawn();
        }
    }

    public void ShowTextHost()
    {
        if (IsServer)
        {
            connectingText.SetActive(true);
            schoolsText.SetActive(false);
            canShowText = false;
            ShowTextClientRpc();
        }
    }

    [ClientRpc]
    public void ShowTextClientRpc()
    {
        if (IsClient && canShowText)
        {
            schoolsText.SetActive(true);
            connectingText.SetActive(false);
        }
    }
}
