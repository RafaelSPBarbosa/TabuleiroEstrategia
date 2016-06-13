using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class GameManager : NetworkBehaviour {

    public Sprite VezCachorro, VezGato, VezRato, VezPassaro;
    Image PlayerIndicator;

    [SyncVar]
    public int ActualCurTurn = 1;
    [SyncVar]
    public int curTurn = 1;
    public int MaxTurns = 4;
    public GameObject MyPlayerBase;
    public GameObject NetManager;

    //public Text TurnText;

    void Start()
    {
        PlayerIndicator = GameObject.Find("PlayerIndicator").GetComponent<Image>();
        NetManager = GameObject.Find("NetManager");
    }
    
    void Update()
    {
        if (curTurn == 4)

        {
            PlayerIndicator.sprite = VezGato;
           
        }
        if (curTurn == 3)
        {
            PlayerIndicator.sprite = VezRato;
          
        }
        if (curTurn == 2)
        {
            PlayerIndicator.sprite = VezPassaro;
          
        }
        if (curTurn == 1)
        {
            PlayerIndicator.sprite = VezCachorro;
          
        }

        MaxTurns = Convert.ToInt32(NetManager.GetComponent<NetManager>().numPlayers);
    }

    [Command]
    public void Cmd_PassTurn()
    {

        ActualCurTurn++;
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