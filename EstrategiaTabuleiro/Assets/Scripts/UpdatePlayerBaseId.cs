﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//Probably never used.
public class UpdatePlayerBaseId : NetworkBehaviour {

	[Command]
    public void Cmd_UpdatePlayerBaseID(int ID)
    {
        //GetComponent<PlayerBase>().PlayerBaseID = ID;
    }
}
