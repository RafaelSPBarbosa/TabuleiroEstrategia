﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Function for switching the owner faction of a farm

public class FarmManager : NetworkBehaviour {

    public Material Vermelho, Amarelo, Verde, Azul; //RED, YELLOW, GREEN, BLUE

    public MeshRenderer Flag;

    public AudioClip[] CapturePointVoices;
    public AudioClip ConstructionSound;

    public GameObject SteppingUnit;

    [SyncVar]
    public GameObject PlayerOwner;

    void Start()
    {
        AudioSource As = GetComponent<AudioSource>();
        As.clip = ConstructionSound;
        As.Play();
    }

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
            if (MyUnits[r].GetComponent<UnitManager>().SteppingTile.GetComponent<TileManager>().PlayerBase != null)
            {
                MyUnits[r].GetComponent<UnitManager>().SteppingTile.GetComponent<TileManager>().PlayerBase.GetComponent<PlayerBase>().Cmd_SwitchOccupied();
                //SteppingTile.GetComponent<TileManager>().PlayerBase.GetComponent<PlayerBase>().Occupied = false;
            }
            //MyUnits[r].GetComponent<UnitManager>().StartCoroutine("HideDeadUnit");
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
            if (MyUnits[r].GetComponent<UnitManager>().SteppingTile.GetComponent<TileManager>().PlayerBase != null)
            {
                MyUnits[r].GetComponent<UnitManager>().SteppingTile.GetComponent<TileManager>().PlayerBase.GetComponent<PlayerBase>().Cmd_SwitchOccupied();
                //SteppingTile.GetComponent<TileManager>().PlayerBase.GetComponent<PlayerBase>().Occupied = false;
            }
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
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 4)
        {
            Flag.material = Verde;
        }
        if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == 3)
        {
            Flag.material = Azul;
        }
    }

    [Command]
    public void Cmd_healUnit()
    {
        if(SteppingUnit != null)
        {
            SteppingUnit.GetComponent<UnitManager>().curHealth += 5;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Unit")
        {

            SteppingUnit = other.transform.gameObject;
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
    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Unit")
        {
            SteppingUnit = null;
        }
    }
}
