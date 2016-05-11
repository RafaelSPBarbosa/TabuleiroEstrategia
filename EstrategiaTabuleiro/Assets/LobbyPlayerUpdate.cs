using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LobbyPlayerUpdate : NetworkBehaviour {

    bool isMine = false;
    public bool On = true;

    void Start()
    {
        if (isLocalPlayer)
        {
            isMine = true;
            
        }
            
           
    }

	void Update()
    {
        
        if (On == true)
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (isMine == true)
                {
                    Debug.Log("Local");
                    GameObject[] AllPlayerBases = GameObject.FindGameObjectsWithTag("PlayerBase");
                    for (int i = 0; i < AllPlayerBases.Length; i++)
                    {
                        if (AllPlayerBases[i].GetComponent<PlayerBase>().enabled == true)
                        {
                            /* if (isServer)
                             {
                                 Debug.Log("Server " + Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                                 AllPlayerBases[i].GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                             }
                             else
                             {
                                 Debug.Log("Local " + Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));

                                 AllPlayerBases[i].GetComponent<PlayerBase>().Rpc_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                             }*/

                            //AllPlayerBases[i].GetComponent<PlayerBase>()
                        }
                    }
                    On = false;
                }
            }
            
        }


    }
}
