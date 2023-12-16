using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Networking.Transport.Relay;
using TMPro;
using UnityEngine.UI;

public class Relay : MonoBehaviour
{
    public static Relay Instance { get; private set; }
    
    public TMP_Text joinCodeText;
    public TMP_Text errorText;
    public InputField joinCodeInput;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
        };
        
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
    
    public void CreateRelayFromMethodCall()
    {
        CreateRelay();
    }
    
    public void JoinRelayFromMethodCall()
    {
        JoinRelay(joinCodeInput.text);
    }    

    public async Task<string> CreateRelay()
    {
        try
        {
            // The number at the end of this line signifies the max amount of players allowed in a relay.
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log(joinCode);

            // You can use "dlts" or "udp" or "wss"
            RelayServerData relayServerData = new RelayServerData(allocation, "wss");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();
            
            ConnectingSchoolsTextManager.Instance.Spawn();
            
            joinCodeText.text = joinCode;

            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            errorText.text = e.Message;
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "wss");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
            
            errorText.text = "Connected to relay";
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            errorText.text = e.Message;
        }
    }
}