using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkInterpolation : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;

    //[SerializeField]
    Transform myTransform;
    [SerializeField]
    float lerpRate = 15;
    [SerializeField]
    float SnapDistance = 2;

    void Start()
    {
        myTransform = this.transform;
    }

    void FixedUpdate()
    {
       

        if (isServer)
        {
            Rpc_TransmitPosition();
        }
        else
        {
            Cmd_TransmitPosition();
        }

        LerpPosition();
    }

    void LerpPosition()
    {
        if (!hasAuthority)
        {
            //if (Vector3.Distance(myTransform.position, syncPos) < SnapDistance)
           // {
                myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
           // }
           // else
           // {
            //    myTransform.position = syncPos;
          // }
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }


    [ClientRpc]
    void Rpc_TransmitPosition()
    {
        CmdProvidePositionToServer(this.transform.position);
    }

    [Command]
    void Cmd_TransmitPosition()
    {
        CmdProvidePositionToServer(this.transform.position);
    }
}
