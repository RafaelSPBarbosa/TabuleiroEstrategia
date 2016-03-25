using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOnOnlyOnOwnersTurn : MonoBehaviour {

    public GameObject GameManager, PlayerManager;

    void Update()
    {
        if (GameManager.GetComponent<GameManager>().curTurn != PlayerManager.GetComponent<PlayerManager>().MyTurn)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
