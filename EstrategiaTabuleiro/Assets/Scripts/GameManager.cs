using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;


public class GameManager : NetworkBehaviour {

    [SyncVar]
    public int curTurn = 1;
    public int MaxTurns = 4;

    public Text TurnText;

    void Update()
    {
        TurnText.text = "Turn : " + curTurn;
    }

    [Command]
    public void Cmd_PassTurn()
    {
        curTurn++;
        if (curTurn > MaxTurns)
            curTurn = 1;
    }

    [Command]
    public void Cmd_SpawnObject(GameObject ObjToSpawn, Vector3 Pos)
    {
        GameObject go = (GameObject)Instantiate(ObjToSpawn, Pos, Quaternion.identity);
        NetworkServer.Spawn(go);
    }
}