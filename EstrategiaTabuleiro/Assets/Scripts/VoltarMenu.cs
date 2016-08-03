using UnityEngine;
using System.Collections;

//Switches from page 3 to page 1
public class VoltarMenu : MonoBehaviour {

    public GameObject Pagina1, Pagina3;

    public void AbrirPagina1()
    {
        Pagina1.SetActive(true);
        Pagina3.SetActive(false);
    }
}
