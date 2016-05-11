using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LobbyPlayerUpdate : NetworkBehaviour {

    public bool isMine = false;
    public bool On = true;
    [SyncVar]
    public int PlayerId = 0;

    void Start()
    {
        if (isLocalPlayer)
        {
            isMine = true;
            Cmd_UpdatePlayerId( GameObject.FindGameObjectsWithTag("LobbyPlayer").Length);

        }
    }

    [Command]
    void Cmd_UpdatePlayerId(int id)
    {
        PlayerId = id;
    }

	void Update()
    {
        /*if (On == true)
        
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

                            //AllPlayerBases[i].GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));
                            //AllPlayerBases[i].GetComponent<PlayerBase>().Rpc_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));

                            StartCoroutine("WhileRetry",AllPlayerBases[i]);
                           On = false;

                            /* if (isServer)
                             {
                                 Debug.Log("Server " + Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                                 AllPlayerBases[i].GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                             }
                             else
                             {
                                 Debug.Log("Local " + Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));

                                 AllPlayerBases[i].GetComponent<PlayerBase>().Rpc_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
                             }

                            //AllPlayerBases[i].GetComponent<PlayerBase>()
                        }
                    }
                    On = false;
                }
            }
        }*/

    }
    /*IEnumerator WhileRetry(GameObject Base)
    {
        while (Base.GetComponent<PlayerBase>().PlayerBaseID == 0)
        {
            print("Retring - LobbyPlayer");
            Base.GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(PlayerId);

            yield return Base.GetComponent<PlayerBase>().PlayerBaseID;
        }
    }*/
}
