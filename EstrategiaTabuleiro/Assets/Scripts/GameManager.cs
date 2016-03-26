using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameManager : NetworkBehaviour {

    [SyncVar]
    public int curTurn = 1;
    public int MaxTurns = 4;



    public void PassTurn()
    {
        curTurn++;

        if (curTurn > MaxTurns)
            curTurn = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            curTurn++;
        }

    }
}
