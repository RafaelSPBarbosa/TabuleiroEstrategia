using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class NetManager : NetworkManager {


    public void StartServerButton()
    {
       StartHost();
    }

    public void StartClientButton()
    {
       StartClient();
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

    public void SetMaxPlayers(string Temp_NumberOfPlayers)
    {
        int NumberOfPlayers = Convert.ToInt32(Temp_NumberOfPlayers); 

        if (NumberOfPlayers > 4)
            NumberOfPlayers = 4;

        maxConnections = NumberOfPlayers;
    }
}
