using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetManager : NetworkLobbyManager {


    public void StartServerButton()
    {
        StartHost();
        SceneManager.LoadScene("Lobby");
        TryToAddPlayer();
    }

    public void StartClientButton()
    {
       StartClient();
       SceneManager.LoadScene("Lobby");
    }

    public void SetIP(string ip)
    {
       networkAddress = ip;
    }

    public void SetPort(string port)
    {
        networkPort = Convert.ToInt32(port);
    }

    public override void OnStartHost()
    {
        
    }
}
