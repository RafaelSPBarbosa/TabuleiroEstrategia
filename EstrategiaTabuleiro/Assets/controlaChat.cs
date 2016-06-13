using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class controlaChat : MonoBehaviour {
   
    public InputField campoChat;
    public Text ChatText;
    public Scrollbar Scroll;
    public GameManager gameManager = null;
    public PlayerBase PlayerOwner;

	void Update () {

        if (campoChat.text != "" && Input.GetKey(KeyCode.Return))
        {
            if (campoChat.text != "/h" && campoChat.text != "/help")
            {
                GameObject[] Bases = GameObject.FindGameObjectsWithTag("PlayerBase");
                for (int i = 0; i < Bases.Length; i++)
                {
                    PlayerBase Script = Bases[i].GetComponent<PlayerBase>();
                    if (Script.enabled == true)
                    {
                        PlayerOwner.Cmd_AdicionaChat(campoChat.GetComponent<InputField>().text, Script.PlayerBaseID);
                        campoChat.text = "";
                        break;
                    }
                }
            }
            else if(campoChat.text == "/h" || campoChat.text == "/help")
            {
                ChatText.text += "<color=black><i>\n Welcome to the help menu, the avaiable commands are : \n /h - Displays the help message \n /w RaceName Msg - RaceName being Birds, Dogs, Cats or Rats. Sends a private message to the desired user</i></color>";
                campoChat.text = "";
            }
        }
        if(campoChat.text != "" && Input.GetKey(KeyCode.Escape))
        {
            campoChat.text = "";
        }
    }
}
