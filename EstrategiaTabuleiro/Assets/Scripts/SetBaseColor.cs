using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SetBaseColor : NetworkBehaviour {


    [SerializeField]
    MeshFilter MeshObj;
    [SerializeField]
    MeshRenderer Mat;

    //public Mesh MeshBaseCao, MeshBaseAguia, MeshBaseGato, MeshBaseRato;
    public Material MatBaseCao, MatBaseAguia, MatBaseGato, MatBaseRato;

    public void Start()
    {
        StartCoroutine("FixColor" );
    }

    public IEnumerator FixColor()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
            if (GetComponent<PlayerBase>().PlayerBaseID == 1)
            {
                Mat.material = MatBaseCao;
            }
            if (GetComponent<PlayerBase>().PlayerBaseID == 2)
            {
                Mat.material = MatBaseAguia;
            }
            if (GetComponent<PlayerBase>().PlayerBaseID == 3)
            {
                Mat.material = MatBaseRato;
            }
            if (GetComponent<PlayerBase>().PlayerBaseID == 4)
            {
                Mat.material = MatBaseGato;
            }
        }
    }
}