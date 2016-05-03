using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GoldMineManager : NetworkBehaviour {


    public Material Vermelho, Amarelo, Verde, Azul;

    public MeshRenderer Flag;

    [SyncVar]
    public GameObject PlayerOwner;

    [Command]
    public void Cmd_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;

        if(PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 1)
        {
            Flag.material = Vermelho;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 2)
        {
            Flag.material = Amarelo;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 3)
        {
            Flag.material = Verde;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 4)
        {
            Flag.material = Azul;
        }
    }

    [ClientRpc]
    public void Rpc_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;

        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 1)
        {
            Flag.material = Vermelho;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 2)
        {
            Flag.material = Amarelo;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 3)
        {
            Flag.material = Verde;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 4)
        {
            Flag.material = Azul;
        }
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
