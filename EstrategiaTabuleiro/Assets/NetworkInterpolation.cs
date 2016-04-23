using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkInterpolation : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;

    [SerializeField]
    Transform myTransform;
    [SerializeField]
    float lerpRate = 15;

    void Start()
    {
        myTransform = this.transform;
    }

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }


    [ClientCallback]
    void TransmitPosition()
    {

        CmdProvidePositionToServer(myTransform.position);
    }
}
