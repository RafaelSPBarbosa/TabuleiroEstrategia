using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetManager : NetworkLobbyManager {

    public int player_Count;
    //AllLobbyPlayers will be used by the server to allow reconnects.
    GameObject[] AllLobbyPlayers;
    public TempStorage storage;

    public string net_Guid;
    public string unique_id = "";

    bool loadedGameLevel = false;

    //Called when changing to the game scene. - At the possibility of adding different levels to the game, this may require changed.
    IEnumerator LoadGameLevel()
    {
        yield return new WaitForSeconds(3);
        ServerChangeScene("Game");
    }

    //Called on the server everytime a client connects
    public override void OnLobbyServerConnect(NetworkConnection conn)
    {
        AllLobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
        GameObject TargetPlayer = null;
        for(int a = 0; a < AllLobbyPlayers.Length; a++)
        {
            if (AllLobbyPlayers[a].GetComponent<NetworkIdentity>().isServer)
            {
                TargetPlayer = AllLobbyPlayers[a];
            }
        }

        
        TargetPlayer.GetComponent<LobbyPlayerUpdate>().Cmd_UpdateJoiningPlayer();
    }


    void Start()
    {
        //storage = GameObject.Find("Storage").GetComponent<TempStorage>();
    }

    public void StartServerButton()
    {
        StartHost();
        //SceneManager.LoadScene("Lobby");
        StartCoroutine("AddServerAsPlayer");
    }

    IEnumerator AddServerAsPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        this.TryToAddPlayer();
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

    public void MatchMakingConnect(string ip)
    {
        networkAddress = ip;
        StartClient();
        SceneManager.LoadScene("Lobby");

    }

    public void SetPort(string port)
    {
        networkPort = Convert.ToInt32(port);
    }

    public override void OnStartHost()
    {
        //Add server to the matchmaking system.
        StartCoroutine("SendMatchmaking");
        SceneManager.LoadScene("Lobby");
    }

    IEnumerator SendMatchmaking()
    {
        string url = "http://www.jaytechmedia.com/autem/create.php";
        WWW www = new WWW(url);
        yield return www;
        unique_id = www.text;
        //Start Keepalive function
        StartCoroutine("MatchmakingKeepalive");
        yield return null;
    }

    IEnumerator MatchmakingKeepalive()
    {
        string url = "http://www.jaytechmedia.com/autem/keepalive.php?unique_id=" + unique_id;
        while (true)
        {
            //execute code here.
            WWW www = new WWW(url);
            yield return www;
            if (www.text == "0")
            {
                Debug.Log("A matchmaking error occured.");
            }
            yield return new WaitForSeconds(5);
        }
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        conn.isReady = true;
        this.TryToAddPlayer();

        //net_Guid = Guid.NewGuid().ToString();
       // storage.Cmd_AddGuid(net_Guid);
       // storage.Cmd_UpdatePlayerNumber();
       //player_Count++;

    }

    public void ShowServers()
    {
        SceneManager.LoadScene("MatchMaking");
    }

    //public override void OnServerConnect(NetworkConnection conn)
   // {
        //if (conn.address == "localServer" && conn.connectionId == 0 )
        //{
            //storage.PlayerNumber = 1;
            //net_Guid = Guid.NewGuid().ToString();
            //storage.ids.Add(net_Guid);
            //storage.my_Player_Number = 1;
            //this.TryToAddPlayer();
        //}
    //}

    void Update()
    {
        //if (GameObject.Find("Storage") != null)
       // {
           // GameObject.Find("Storage").SetActive(true);
       // }

        minPlayers = numPlayers;
        //Debug.Log(NetManager.singleton.numPlayers);


        if (loadedGameLevel == false)
        {
            if (SceneManager.GetActiveScene().name == "LoadingScreen")
            {
                loadedGameLevel = true;
                StartCoroutine("LoadGameLevel");
            }
        }

    }
}
