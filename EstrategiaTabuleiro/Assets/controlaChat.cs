using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class controlaChat : MonoBehaviour {

    //Serealizei pra pegar o objeto mais fácilmente pelo script
    //Serealiza o rolo de texto do chat
    [SerializeField]
    GameObject textoChat;

    //Serealiza o campo aonde se escreve no chat
    [SerializeField]
    GameObject campoChat;

    [SerializeField]
    GameManager gameManager = null;

    // Use this for initialization
    void Start () {
        textoChat.GetComponent<Text>().text = "teste"; //aleluia!!!!kkk
        
    }
	
	// Update is called once per frame
	void Update () {
        gameManager.TextoChat = textoChat;
        if (campoChat.GetComponent<InputField>().text != "" && Input.GetKey(KeyCode.Return))
        {
            adicionaChat(campoChat.GetComponent<InputField>().text);
            campoChat.GetComponent<InputField>().text = "";
        }

    }

    void adicionaChat(string texto) {
        textoChat.GetComponent<Text>().text += "\nl" + texto;
    }
}
