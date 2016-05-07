using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FarmManager : NetworkBehaviour {

    public Material Vermelho, Amarelo, Verde, Azul;

    public MeshRenderer Flag;

    [SyncVar]
    public GameObject PlayerOwner;

    [Command]
    public void Cmd_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner.GetComponent<PlayerBase>().Food--;
        //Kill Hungry Unit
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        int UnitAmmount = 0;
        List<GameObject> MyUnits = new List<GameObject>();
        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].GetComponent<UnitManager>().PlayerOwner == PlayerOwner)
            {
                UnitAmmount++;
                MyUnits.Add(AllUnits[i]);
            }
        }

        if (UnitAmmount > PlayerOwner.GetComponent<PlayerBase>().Food)
        {
            int r = Random.Range(0, MyUnits.Count);
            MyUnits[r].GetComponent<UnitManager>().Cmd_KillUnit();
            MyUnits[r].GetComponent<UnitManager>().StartCoroutine("HideDeadUnit");
        }
        
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;

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

    [ClientRpc]
    public void Rpc_ChangeOwner(GameObject newOwner)
    {
        PlayerOwner.GetComponent<PlayerBase>().Food--;
        //Kill Hungry Unit
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        int UnitAmmount = 0;
        List<GameObject> MyUnits = new List<GameObject>();

        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].GetComponent<UnitManager>().PlayerOwner == PlayerOwner)
            {
                UnitAmmount++;
                MyUnits.Add(AllUnits[i]);
            }
        }

        if (UnitAmmount > PlayerOwner.GetComponent<PlayerBase>().Food)
        {
            int r = Random.Range(0, MyUnits.Count);
            MyUnits[r].GetComponent<UnitManager>().Cmd_KillUnit();
            MyUnits[r].GetComponent<UnitManager>().StartCoroutine("HideDeadUnit");
        }
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;

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

    [Command]
    public void Cmd_SetInitialOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;

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

    [ClientRpc]
    public void Rpc_SetInitialOwner(GameObject newOwner)
    {
        PlayerOwner = newOwner;
        PlayerOwner.GetComponent<PlayerBase>().Food++;

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
