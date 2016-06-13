using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GoldMineManager : NetworkBehaviour {


    public Material Vermelho, Amarelo, Verde, Azul;

    public MeshRenderer Flag;

    public AudioClip[] CapturePointVoices;

    [SyncVar]
    public GameObject PlayerOwner;

    [Command]
    public void Cmd_ChangeOwner(GameObject newOwner)
    {
        if (PlayerOwner != null)
        {
            PlayerOwner.GetComponent<PlayerBase>().Cmd_UpdateGoldMineAmmount(false);
        }
        PlayerOwner = newOwner;

        PlayerOwner.GetComponent<PlayerBase>().Cmd_UpdateGoldMineAmmount(true);


        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 1)
        {
            Flag.material = Vermelho;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 2)
        {
            Flag.material = Amarelo;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 4)
        {
            Flag.material = Verde;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 3)
        {
            Flag.material = Azul;
        }
    }

    [ClientRpc]
    public void Rpc_ChangeOwner(GameObject newOwner)
    {
        if (PlayerOwner != null)
        {
            if (isServer)
            {
                PlayerOwner.GetComponent<PlayerBase>().Rpc_UpdateGoldMineAmmount(false);
            }
        }

        PlayerOwner = newOwner;
        if (isServer)
        {
            PlayerOwner.GetComponent<PlayerBase>().Rpc_UpdateGoldMineAmmount(true);
        }

        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 1)
        {
            Flag.material = Vermelho;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 2)
        {
            Flag.material = Amarelo;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 4)
        {
            Flag.material = Verde;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 3)
        {
            Flag.material = Azul;
        }

        AudioSource As = GetComponent<AudioSource>();
        As.clip = CapturePointVoices[PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID - 1];
        As.Play();
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
