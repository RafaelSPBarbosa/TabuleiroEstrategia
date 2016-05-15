using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LobbyPlayerUpdate : NetworkBehaviour {

    public bool isMine = false;
    public bool On = true;
    [SyncVar]
    public int PlayerId = 0;
    public int CristalImg;
    public Sprite TurnedOnCristal, TurnedOffCristal;
    public GameObject[] AllCristals;
    public GameObject[] AllIllustrations;

    void Start()
    {
        if (isServer)
        {
            isMine = true;
            //Cmd_UpdatePlayerId( GameObject.FindGameObjectsWithTag("LobbyPlayer").Length);

        }


        AllCristals = GameObject.FindGameObjectsWithTag("Cristal");
        AllIllustrations = GameObject.FindGameObjectsWithTag("Ilustracao");

        if (isLocalPlayer)
            {
                for (int i = 0; i < AllCristals.Length; i++)
            {
                if (AllCristals[i].transform.name == "Cristal" + GetComponent<NetworkIdentity>().netId)
                {
                    CristalImg = i;
                }
            }

            GameObject[] AllRdyButtons = GameObject.FindGameObjectsWithTag("ReadyBtn");
            for (int i = 0; i < AllRdyButtons.Length; i++)
            {
                if (AllRdyButtons[i].transform.name != "ReadyBtn" + GetComponent<NetworkIdentity>().netId)
                {
                    AllRdyButtons[i].SetActive(false);

                }
                else
                {
                    AllRdyButtons[i].GetComponent<Button>().onClick.AddListener(() => Cmd_ReadyPlayer(CristalImg));
                }
            }

            
            for(int i = 0; i < AllIllustrations.Length; i++)
            {
                if (AllIllustrations[i].transform.name == "Ilustracao" + GetComponent<NetworkIdentity>().netId)
                {
                    if (isServer)
                    {
                        Rpc_UpdateIlustration(i, true);
                    }
                    else
                    {
                        Cmd_UpdateIlustration(i, true);
                    }
                }
            }
        }
    }

    [Command]
    void Cmd_UpdateIlustration (int ID, bool On)
    {
        if (On == true)
        {
            AllIllustrations[ID].GetComponent<Image>().enabled = true;
        }
        else
        {
            AllIllustrations[ID].GetComponent<Image>().enabled = false;
        }
        Rpc_UpdateIlustration(ID, On);
    }

    [ClientRpc]
    void Rpc_UpdateIlustration(int ID, bool On)
    {
        if (On == true)
        {
            AllIllustrations[ID].GetComponent<Image>().enabled = true;
        }
        else
        {
            AllIllustrations[ID].GetComponent<Image>().enabled = false;
        }
    }

    //[Command]
    void Cmd_ReadyPlayer(int TargetImage)
    {
        if (isLocalPlayer)
        {
            //GetComponent<NetworkLobbyPlayer>().readyToBegin = !GetComponent<NetworkLobbyPlayer>().readyToBegin;


            if (GetComponent<NetworkLobbyPlayer>().readyToBegin == true)
            {
                GetComponent<NetworkLobbyPlayer>().SendNotReadyToBeginMessage();
                //AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOnCristal;
                if (isServer)
                {
                    Rpc_UpdateImage(TargetImage, 1);
                }
                else
                {
                    Cmd_UpdateImage(TargetImage, 1);
                }
                
            }
            else
            {
                GetComponent<NetworkLobbyPlayer>().SendReadyToBeginMessage();
                //AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOffCristal;
                if (isServer)
                {
                    Rpc_UpdateImage(TargetImage, 2);
                }
                else
                {
                    Cmd_UpdateImage(TargetImage, 2);
                }
            }
        }
    }

    [Command]
    void Cmd_UpdateImage(int TargetImage, int SpriteID)
    {
        if (SpriteID == 2)
        {
            AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOnCristal;
        }
        else
        {
            AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOffCristal;
        }
        Rpc_UpdateImage(TargetImage, SpriteID);
    }

    [ClientRpc]
    void Rpc_UpdateImage(int TargetImage, int SpriteID)
    {
        if (SpriteID == 2)
        {
            AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOnCristal;
        }
        else
        {
            AllCristals[TargetImage].GetComponent<Image>().sprite = TurnedOffCristal;
        }
    }



    [Command]
    void Cmd_UpdatePlayerId(int id)
    {
        PlayerId = id;
    }
    
    [Command]
    public void Cmd_UpdateJoiningPlayer()
    {
        StartCoroutine("DelayedUpdate");
    }

    IEnumerator DelayedUpdate()
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < AllIllustrations.Length; i++)
        {
            if (AllIllustrations[i].GetComponent<Image>().enabled == true)
            {
                Rpc_UpdateIlustration(i, true);
            }
            else
            {

                Rpc_UpdateIlustration(i, false);
            }
        }
    }
}
