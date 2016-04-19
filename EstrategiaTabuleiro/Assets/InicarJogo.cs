using UnityEngine;
using System.Collections;

public class InicarJogo : MonoBehaviour {

    public GameObject Pagina1, Pagina2;

    public void AbrirPagina2()
    {
        Pagina1.SetActive(false);
        Pagina2.SetActive(true);
    }
}
