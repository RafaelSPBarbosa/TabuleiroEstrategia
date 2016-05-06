﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class LobbyPlayerUpdate : NetworkBehaviour {

    public bool isMine = false;
    public bool On = true;

    void Start()
    {
        if (isLocalPlayer)
            isMine = true;
    }

	void Update()
    {
        if (On == true)
        {
            if (Application.loadedLevelName == "Game")
            {
                if (isMine == true)
                {
                    GameObject[] AllPlayerBases = GameObject.FindGameObjectsWithTag("PlayerBase");
                    for (int i = 0; i < AllPlayerBases.Length; i++)
                    {
                        if (AllPlayerBases[i].GetComponent<PlayerBase>().enabled == true)
                        {
                            print("OI!");
                            AllPlayerBases[i].GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));
                            //AllPlayerBases[i].GetComponent<PlayerBase>().Rpc_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));
                            On = false;
                        }
                    }
                }
            }
        }
    }
}
