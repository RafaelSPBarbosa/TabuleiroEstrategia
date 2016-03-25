using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class networkComponentVerification : NetworkBehaviour {

    [SerializeField]
    Behaviour[] ComponentsToDisable;
	

    void Start()
    {

        if(!isLocalPlayer){

            for(int i = 0; i < ComponentsToDisable.Length; i++)
            {
                ComponentsToDisable[i].enabled = false;
            }
        }
    }
}
