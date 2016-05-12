using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetManager : NetworkLobbyManager {

    public int player_Count;

    public TempStorage storage;

    public string net_Guid;

    bool loadedGameLevel = false;

    IEnumerator LoadGameLevel()
    {
        yield return new WaitForSeconds(3);
        //SceneManager.LoadScene("Game");
        ServerChangeScene("Game");
    }


        void Start()
    {
        //storage = GameObject.Find("Storage").GetComponent<TempStorage>();
    }

    public void StartServerButton()
    {
        StartHost();
        //SceneManager.LoadScene("Lobby");

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
        SceneManager.LoadScene("Lobby");
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
