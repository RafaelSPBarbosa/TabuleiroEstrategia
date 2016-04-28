using UnityEngine;
using System.Collections;

public class TelaOpcoes : MonoBehaviour {

    public GameObject Pagina1, Pagina3;

    public void AbrirPagina3()
    {
        Pagina1.SetActive(false);
        Pagina3.SetActive(true);
    }
}
