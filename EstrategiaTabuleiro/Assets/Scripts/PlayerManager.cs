using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public int MyTurn, PlayerID;
    public GameManager gameManager;
    Button PassTurnBtn;

    void Start()
    {

        PlayerID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;
        MyTurn = PlayerID;
        GameObject.Find("PlayerIndicatorText").GetComponent<Text>().text = "Player : " + PlayerID;

        this.transform.name = "_PlayerManager";
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }
}
