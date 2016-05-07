using UnityEngine;
using System.Collections;

public class LookAtCameraSolver : MonoBehaviour {

    Transform TargetObj;

    void Start()
    {
        TargetObj = GameObject.Find("CameraRotator").transform;
    }

	void Update () {

        //this.transform.LookAt(new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z));

        Vector3 newRotation = new Vector3(this.transform.eulerAngles.x, TargetObj.eulerAngles.y + 180, this.transform.eulerAngles.z);
        this.transform.eulerAngles = newRotation;
    }
}
