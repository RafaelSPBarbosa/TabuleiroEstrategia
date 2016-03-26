using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnOnOnlyOnOwnersTurn : MonoBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    void Awake()
    {
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }


    void Update()
    {
        if (gameManager.curTurn != playerManager.MyTurn)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }
}
