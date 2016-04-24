using UnityEngine;
using System.Collections;

public class ButtonAnimation : MonoBehaviour {

    public void AbrirBotao()
    {
        GetComponent<Animator>().SetBool("MouseSobre", true);
    }

    public void FecharBotao()
    {
        GetComponent<Animator>().SetBool("MouseSobre", false);


    }
}
