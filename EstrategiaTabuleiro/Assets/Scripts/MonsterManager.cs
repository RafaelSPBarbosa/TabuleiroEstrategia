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
	[SyncVar]
    public int LastAttackingPlayerID;

    public Animator AnimMesh;

	[Command]
	public void Cmd_GetLastPlayerID(int ID){

		LastAttackingPlayerID = ID;
	}

    void Update()
    {
        if (isAlive == true)
        {
            if (curHealth <= 0)
            {

                if (LastAttackingPlayerID == 1)
                {
                    Cmd_GetReliquia(LastAttackingPlayerID);
                }
                else
                {
                    Rpc_GetReliquia(LastAttackingPlayerID);
                }

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

   // [Command]
    void Cmd_GetReliquia(int TargetID)
    {
		GameObject[] Bases = GameObject.FindGameObjectsWithTag ("PlayerBase");

		for (int i = 0; i < Bases.Length; i++) {

			if (Bases[i].GetComponent<PlayerBase> ().PlayerBaseID == TargetID) {
				Bases[i].GetComponent<PlayerBase>().GetRelic();
				break;
			}
		}

    }

    [ClientRpc]
    void Rpc_GetReliquia(int TargetID)
    {
		GameObject[] Bases = GameObject.FindGameObjectsWithTag ("PlayerBase");

		for (int i = 0; i < Bases.Length; i++) {

			if (Bases [i].GetComponent<PlayerBase> ().PlayerBaseID == TargetID) {
				Bases[i].GetComponent<PlayerBase>().GetRelic();
				break;
			}
		}
    }

    [ClientRpc]
    public void Rpc_UpdateAttackAnim(GameObject Target)
    {
        AnimMesh.SetTrigger("Attack");

        this.transform.LookAt(Target.transform.position);
    }

    [Command]
    public void Cmd_UpdateGetHit()
    {
        AnimMesh.SetTrigger("GetHit");
    }

    [ClientRpc]
    public void Rpc_UpdateGetHit()
    {
        AnimMesh.SetTrigger("GetHit");
    }

    [Command]
    public void Cmd_UpdateAttackAnim(GameObject Target)
    {
        AnimMesh.SetTrigger("Attack");

        this.transform.LookAt(Target.transform.position);
    }

    [Command]
    public void Cmd_AttackTarget(GameObject Target)
    {
        AnimMesh.SetTrigger("Attack");

        this.transform.LookAt(Target.transform.position);
        Rpc_UpdateAttackAnim(Target);
        Target.GetComponent<UnitManager>().Rpc_TakeDamage(Target, Damage);
    }

    [ClientRpc]
    public void Rpc_LookAtTarget(GameObject Target)
    {
        this.transform.LookAt(Target.transform.position);
    }

    [Command]
    public void Cmd_LookAtTarget(GameObject Target)
    {
        this.transform.LookAt(Target.transform.position);
    }

    [ClientRpc]
    public void Rpc_Die()
    {
        //TileSpawner.CanSpawnMonster = true;

        if(AnimMesh != null)
            AnimMesh.SetTrigger("Die");

        isAlive = false;
        
        NetworkServer.Destroy(this.gameObject);
    }

    [Command]
    public void Cmd_Die()
    {

        if (AnimMesh != null)
            AnimMesh.SetTrigger("Die");

        isAlive = false;

        NetworkServer.Destroy(this.gameObject);
    }
}
