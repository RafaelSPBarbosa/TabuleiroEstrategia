using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MonsterManager : NetworkBehaviour {

    [SyncVar]
    public int curHealth;
    [SyncVar]
    public int Damage;
    public bool isAlive = true;
    public TileManager TileSpawner;

    public Animator AnimMesh;

    void Update()
    {
        if (isAlive == true)
        {
            if (curHealth <= 0)
            {
                if (isServer)
                    Rpc_Die();
            }
        }
    }

    [ClientRpc]
    public void Rpc_AttackTarget(GameObject Target)
    {
        if (isServer)
        {
            if (AnimMesh != null)
            {
                AnimMesh.SetTrigger("Attack");
            }
            this.transform.LookAt(Target.transform.position);
            Target.GetComponent<UnitManager>().Rpc_TakeDamage(Target, Damage);
        }
    }

    [ClientRpc]
    public void Rpc_Die()
    {
        TileSpawner.CanSpawnMonster = true;

        if(AnimMesh != null)
            AnimMesh.SetTrigger("Die");

        isAlive = false;

        print("Morreu");
        print("Dar Relíquia ao jogador");

        GetComponent<MeshRenderer>().enabled = false;
    }
}
