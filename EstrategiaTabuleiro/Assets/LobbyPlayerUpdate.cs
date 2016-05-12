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
        if (isServer)
        {
            isMine = true;
            //Cmd_UpdatePlayerId( GameObject.FindGameObjectsWithTag("LobbyPlayer").Length);

        }
    }

    [Command]
    void Cmd_UpdatePlayerId(int id)
    {
        PlayerId = id;
    }

	void Update()
    {

        if (On == true)

        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (isMine == true)
                {
                    if (isServer)
                        //StartCoroutine("AssignIds");
                        On = false;
                }
            }
        }

    }

    IEnumerator AssignIds()
    {
        yield return new WaitForSeconds( 0.5f);
        GameObject[] AllPlayerBases = GameObject.FindGameObjectsWithTag("PlayerBase");
        for (int i = 0; i < AllPlayerBases.Length; i++)
        {

                //AllPlayerBases[i].GetComponent<PlayerBase>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));
                //AllPlayerBases[i].GetComponent<PlayerBase>().Rpc_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.ToString()));

            AllPlayerBases[i].GetComponent<UpdatePlayerBaseId>().Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
        }
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
