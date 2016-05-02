using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class GameManager : NetworkBehaviour {

    [SyncVar]
    public int curTurn = 1;
    public int MaxTurns = 4;
    public GameObject MyPlayerBase;
    public GameObject NetManager;

    public Text TurnText;

    void Start()
    {
        /* PlayerManager = GameObject.Find("_PlayerManager");
        GameObject[] AllBases = GameObject.FindGameObjectsWithTag("PlayerBase");
        for (int i = 0; i<AllBases.Length; i++)
        {
            print(AllBases[i].transform.name);
            if (AllBases[i].GetComponent<PlayerBase>().PlayerBaseID == PlayerManager.GetComponent<PlayerManager>().PlayerID)
            {

                MyPlayerBase = AllBases[i];
            

            }
        }*/
        NetManager = GameObject.Find("NetManager");
    }
    
    void Update()
    {
        TurnText.text = "Turn : " + curTurn;

        MaxTurns = Convert.ToInt32(NetManager.GetComponent<NetManager>().numPlayers);
    }

    [Command]
    public void Cmd_PassTurn()
    {
        
        curTurn++;
        if (curTurn > MaxTurns)
            curTurn = 1;

        Rpc_UpdateTurn(curTurn);
        

    }

    [ClientRpc]
    public void Rpc_UpdateTurn( int Turn )
    {
        curTurn = Turn;
    }
}