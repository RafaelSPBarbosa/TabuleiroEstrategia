using UnityEngine;
using System.Collections;

public class UIMouseTrigger : MonoBehaviour {

    public GameObject TargetObj;

    void Start()
    {
        TargetObj.SetActive(false);
    }

	void OnMouseEnter()
    {
        TargetObj.SetActive(true);
    }

    void OnMouseExit()
    {
        TargetObj.SetActive(false);
    }
}
