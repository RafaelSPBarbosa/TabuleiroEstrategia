using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class networkComponentVerificationOffline : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ComponentsToDisable;
	

    void Start()
    {

        if(!GetComponent<UnitManager>().PlayerOwner.GetComponent<PlayerBase>().isLocalPlayer){

            for(int i = 0; i < ComponentsToDisable.Length; i++)
            {
                ComponentsToDisable[i].enabled = false;
            }
        }
    }
}