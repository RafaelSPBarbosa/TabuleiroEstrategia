using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public int MyTurn;
    public int PlayerID = 1;
    public GameManager gameManager;
    Button PassTurnBtn;

    public void UpdateVariables()
    {

        //PlayerID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;
        MyTurn = PlayerID;
        GameObject.Find("PlayerIndicatorText").GetComponent<Text>().text = "Player : " + PlayerID;

        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }
}
