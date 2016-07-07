using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerListEntry : MonoBehaviour {

    Text Name;
    Text Players;
    Text Passworded;
    ServerData local_data;

	// Use this for initialization
    public void SetData(ServerData input)
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.name == "S_Name")
            {
                Name = child.GetComponent<Text>();
            }
            if (child.name == "S_Players")
            {
                Players = child.GetComponent<Text>();
            }
            if (child.name == "S_Passworded")
            {
                Passworded = child.GetComponent<Text>();
            }
        }

        local_data = input;
        Name.text = input.name;
        Players.text = "Players " + input.current_players + "/" + input.max_players;
        if (input.is_password == true)
        {
            Passworded.text = "Passworded : Yes";
        }
        else
        {
            Passworded.text = "Passworded : No";
        }
    }
}
