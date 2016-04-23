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
                {
                    Rpc_Die();
                }
                else
                {
                    Cmd_Die();
                }
            }
        }
    }

    [ClientRpc]
    public void Rpc_AttackTarget(GameObject Target)
    {
        if (AnimMesh != null)
        {
            AnimMesh.SetTrigger("Attack");
        }
        this.transform.LookAt(Target.transform.position);
        Target.GetComponent<UnitManager>().Cmd_TakeDamage(Target, Damage);
        //Cmd_UpdateAnimation(Target);
    }
    [Command]
    public void Cmd_UpdateAnimation(GameObject Target)
    {
        if (AnimMesh != null)
        {
            AnimMesh.SetTrigger("Attack");
        }
        this.transform.LookAt(Target.transform.position);
    }

    [Command]
    public void Cmd_AttackTarget(GameObject Target)
    {
        if (AnimMesh != null)
        {
            AnimMesh.SetTrigger("Attack");
        }
        this.transform.LookAt(Target.transform.position);
        Target.GetComponent<UnitManager>().Rpc_TakeDamage(Target, Damage);
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

        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    public void Cmd_Die()
    {
        TileSpawner.CanSpawnMonster = true;

        if (AnimMesh != null)
            AnimMesh.SetTrigger("Die");

        isAlive = false;

        print("Morreu");
        print("Dar Relíquia ao jogador");

        NetworkServer.Destroy(this.gameObject);
    }
}
