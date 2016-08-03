using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetGameManager : NetworkBehaviour {

    //Server Authoritive Variables.
    List<Player> Players;
    string CurrentTurn = "UUID";

    //Server Settings such as speed and things.
    int speed = 1;

	void Start () {
	    
	}
	
   

	void Update () {
	 
	}

    //Used for keep alives?
    int step = 1;
    void FixedUpdate()
    {
        if (step == 50)
        {
            //1 second passed;
            foreach (Player p in Players)
            {

            }
            step = 1;
        }
        else
        {
            step++;
        }
    }


    //All CMD calls go here.

}

//Contains data relating to the servers clients.
public struct Player
{
    string UID;
    List<GameObject> ActiveUnits;
    int Gold;
    int Farms;
}

