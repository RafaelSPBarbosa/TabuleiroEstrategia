using UnityEngine;
using System.Collections;

//Switches from page 2 to page 1?
public class VoltarParaMenu : MonoBehaviour {

    public GameObject Pagina1, Pagina2;

    public void AbrirPagina1()
    {
        Pagina1.SetActive(true);
        Pagina2.SetActive(false);
    }
}

