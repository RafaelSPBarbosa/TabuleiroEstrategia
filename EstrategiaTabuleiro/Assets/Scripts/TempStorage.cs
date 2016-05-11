using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TempStorage : NetworkBehaviour {

    [SyncVar]
    public int PlayerNumber = 0;

    public int my_Player_Number;
    //string[] ids = new string[3];

   public SyncList<string> ids;

    [ClientRpc]
    public void Rpc_GetPlayerNumber(string clientUid)
    {
        for (int x = 0; x < ids.Count; x++)
        {
            if (ids[x] == clientUid)
            {
                //Found the client, so X + 1 is equal to their player number.
                my_Player_Number = x + 1; 
                break;
            }
        }
    }

    [Command]
    public void Cmd_AddGuid(string guid)
    {
        ids.Add(guid);
        PlayerNumber++;
    }

    [Command]
    public void Cmd_AddPlayer()
    {
        PlayerNumber++;
    }


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
