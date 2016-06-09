using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class controlaChat : MonoBehaviour {
   
    public InputField campoChat;
    public Scrollbar Scroll;
    public GameManager gameManager = null;
    public PlayerBase PlayerOwner;

	void Update () {

        if (campoChat.text != "" && Input.GetKey(KeyCode.Return))
        {
            PlayerOwner.Cmd_AdicionaChat(campoChat.GetComponent<InputField>().text);
            campoChat.text = "";
            
        }
        if(campoChat.text != "" && Input.GetKey(KeyCode.Escape))
        {
            campoChat.text = "";
        }
    }
}
