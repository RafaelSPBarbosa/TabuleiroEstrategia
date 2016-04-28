using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FarmManager : NetworkBehaviour {

    [SyncVar]
    public GameObject PlayerOwner;

    [Command]
    public void Cmd_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner.GetComponent<PlayerBase>().Food--;
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;
    }

    [ClientRpc]
    public void Rpc_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner.GetComponent<PlayerBase>().Food--;
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;
    }

    [Command]
    public void Cmd_SetInitialOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Unit")
        {
            UnitManager otherScript = other.gameObject.GetComponent<UnitManager>();
            if (otherScript.UnitType == 0)
            {
                if (otherScript.PlayerOwner != PlayerOwner)
                {
                    if (isServer)
                    {
                        Rpc_ChangeOwner(otherScript.PlayerOwner);
                    }
                    else {
                        Cmd_ChangeOwner(otherScript.PlayerOwner);
                    }
                    
                }
            }
        }
    }
}
