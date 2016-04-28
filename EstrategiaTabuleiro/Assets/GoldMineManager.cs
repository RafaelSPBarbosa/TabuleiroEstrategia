using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GoldMineManager : NetworkBehaviour {

    [SyncVar]
    public GameObject PlayerOwner;

    [Command]
    public void Cmd_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;
    }

    [ClientRpc]
    public void Rpc_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Unit")
        {
            UnitManager otherScript = other.gameObject.GetComponent<UnitManager>();
            if (otherScript.UnitType == 0)
            {
                if (otherScript.PlayerOwner != PlayerOwner || PlayerOwner == null)
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
