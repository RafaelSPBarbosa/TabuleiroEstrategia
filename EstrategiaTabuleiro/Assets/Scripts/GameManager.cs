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

    //So gambiarra
    [SerializeField]
    public GameObject TextoChat;

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

        //Cmd_atualizaChat();
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

    //SÓ EXISTE 1 CONTROLADOR DE COMUNICAÇÃO ENTRE O SERVER E O USER QUE MANDA O TURNO, TERÍA QUE SER FEITO ALGO QUE MANDE OS CHAT, OS TEXTO PARA QUE TODOS VEJAM
    //Não sei mandar isso pra outra máquina :/  Tá além do meu conhecimento teria que começar com algo basico entre interação do unity entre máquinas, não sei como se comportam, nem como funcionam
    /*
    [Command]
    public void Cmd_SendChat()
    {
        //Não sei aonde que a parada é enviada para os demais user conectados, tá além do meu conhecimento. 
        TextoChat.GetComponent<Text>().text = NetManager.GetComponent<Text>().text;
        //Rpc_SendChat();

    }
    [ClientRpc]
    public void Rpc_SendChat() {
        //Não sei aonde que a parada é enviada para os demais user conectados, tá além do meu conhecimento. 
        TextoChat.GetComponent<Text>().text = TextoChat.GetComponent<Text>().text;

    }
    */
    /*
    [Command]
    public void Cmd_atualizaChat()
    {
        //Não sei aonde que a parada é enviada para os demais user conectados, tá além do meu conhecimento. 
        TextoChat.GetComponent<Text>().text = NetManager.GetComponent<Text>().text;

    }
    */



}