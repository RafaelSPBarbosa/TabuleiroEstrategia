using UnityEngine;
using System.Collections;

public class Objetivos_Animacao : MonoBehaviour {

    Animator Anim;

    public void Start()
    {
        Anim = GetComponent<Animator>();
    }


	public void MovePanel (bool aberto)
    {
        Anim.SetBool("MouseOver", aberto);
    }
}
