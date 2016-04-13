using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UnitManager : NetworkBehaviour {

    //Definição de Variáveis
    public int MaxActions, curActions;
    public bool Selected;
    public GameObject UnitSelectedIndicator;
    public PlayerManager playerManager;
    public GameManager gameManager;
    public GameObject PlayerOwner;
    [SyncVar]
    public int curHealth = 1;
    public int MaxHealth = 1; 
    [SyncVar]
    public int Damage = 0;
    public int SkillPoints = 20;
    public int UnitType = 0;
    public Animator AnimatedMesh;
    public Vector3 GoToPos;
    public bool Busy = false;
    public bool isAttacking = false;
    public bool isAlive = true;
    public GameObject SteppingTile;
    //Definição de Variáveis

    public void ReloadActions()
    {
        if(isAlive == true)
        curActions = MaxActions;
    }

    void Start()
    {
        GoToPos = this.transform.position;
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
    }

    void OnMouseDown()
    {
        if (isAlive == true)
        {
            if (PlayerOwner.GetComponent<PlayerBase>().PlayerBaseID == playerManager.PlayerID)
            {
                if (Selected == true)
                {
                    DeSelectUnit();
                }
                else
                {
                    //O player só pode selecionar a unidade caso ela tenha mais de uma ação, caso contrário, não poderá nem sequer selecionar, quanto menos mover.
                    if (curActions >= 1 && gameManager.curTurn == playerManager.MyTurn)
                    {
                        // Aqui como uma outra unidade pode estar selecionada, eu procuro todas as unidades da mesma forma que eu fiz ali em cima, e então des-seleciono elas
                        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

                        for (int i = 0; i < AllFriendlyUnits.Length; i++)
                        {
                            AllFriendlyUnits[i].GetComponent<UnitManager>().Selected = false;
                        }

                        //Com nenhuma outra unidade selecionada, seleciono agora esta unidade sem risco de ter 2 unidades selecionadas ao mesmo tempo.
                        Selected = true;
                    }
                }
            }
        }
    }

    void DeSelectUnit()
    {
        if (isAlive == true) {
            //Caso o player seja selecionado e "Selected" seja verdadeiro, tornar falso,
            Selected = false;

            // Então encontro e coloco todas os objetos com tag "Tile" dentro da variavel AllTiles
            GameObject[] AllTiles = GameObject.FindGameObjectsWithTag("Tile");

            // Com um For Loop eu entro no script "TileManager" de cada um dos objetos dentro de "AllTiles" e torno a variável "SelectedUnit" deles igual a nullo ( Nada )
            for (int i = 0; i < AllTiles.Length; i++)
            {
                AllTiles[i].GetComponent<TileManager>().SelectedUnit = null;
            }
        }
    }

    [ClientCallback]
    public void Rpc_MakeUnitRun( bool state)
    {
        if(AnimatedMesh != null)
            AnimatedMesh.SetBool("Running", state);
    }

    [Command]
    public void Cmd_KillUnit()
    {
        isAlive = false;

        if (AnimatedMesh != null)
            AnimatedMesh.SetTrigger("Die");

        if (SteppingTile != null)
            SteppingTile.GetComponent<TileManager>().SteppingObject = null; 

        GetComponent<BoxCollider>().enabled = false;
        Rpc_KillUnit();
    }

    [ClientRpc]
    public void Rpc_KillUnit()
    {
        isAlive = false;
        GetComponent<BoxCollider>().enabled = false;

        if (SteppingTile != null)
            SteppingTile.GetComponent<TileManager>().SteppingObject = null;

        if (AnimatedMesh != null)
            AnimatedMesh.SetTrigger("Die");
    }

    public IEnumerator HideDeadUnit()
    {
        yield return new WaitForSeconds(5);
        for(int i = 0; i < 100; i++)
        {
            yield return 0;
            this.transform.Translate(0,-0.01f, 0);
        }
        if (!isServer)
        {
            Cmd_DestroyUnit(this.gameObject);
        }
        else
        {
            Rpc_DestroyUnit(this.gameObject);
        }
    }

    [Command]
    public void Cmd_DestroyUnit ( GameObject Target)
    {
        Destroy(this.gameObject);
    }

    [ClientRpc]
    public void Rpc_DestroyUnit(GameObject Target)
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (isAlive == true)
        {

            if (curHealth <= 0)
            {
                Cmd_KillUnit();
                DeSelectUnit();
                StartCoroutine(HideDeadUnit());
            }

            if (UnitType != 0)
            {
                if (Selected == true)
                {
                    if (Busy == false && isAttacking == false)
                    {
                        if (curActions >= 1)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;

                                if (Physics.Raycast(ray, out hit, 100))
                                {
                                    if (hit.transform.tag == "Unit")
                                    {
                                        if (UnitType == 1)
                                        {
                                            if (Vector3.Distance(hit.transform.position, this.transform.position) < 1.5f)
                                            {
                                                if (hit.transform.gameObject.GetComponent<UnitManager>().PlayerOwner != PlayerOwner)
                                                {

                                                    StartCoroutine(LocalAttackUnit(hit.transform.gameObject));
                                                    curActions--;
                                                }
                                            }
                                        }
                                        if (UnitType == 2)
                                        {
                                            if (Vector3.Distance(hit.transform.position, this.transform.position) > 1.5f && Vector3.Distance(hit.transform.position, this.transform.position) < 3.0f) 
                                            {
                                                if (hit.transform.gameObject.GetComponent<UnitManager>().PlayerOwner != PlayerOwner)
                                                {

                                                    StartCoroutine(LocalAttackUnit(hit.transform.gameObject));
                                                    curActions--;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //Aqui estou apenas ligando e desligando o efeito de uma unidade selecionada
            if (Selected == true)
            {
                UnitSelectedIndicator.SetActive(true);
            }
            else
            {
                UnitSelectedIndicator.SetActive(false);
            }

            //Caso as ações do jogador acabem, a unidade é desselecionada
            if (curActions <= 0 && Selected == true)
            {
                DeSelectUnit();
            }

            if (gameManager.curTurn != playerManager.MyTurn)
            {
                DeSelectUnit();
            }

            if (isAttacking == false)
            {
                this.transform.LookAt(GoToPos);
                this.transform.position = Vector3.MoveTowards(this.transform.position, GoToPos, Time.deltaTime);
            }
        }
    }

    [ClientRpc]
    public void Rpc_UnitAttack(GameObject Target)
    {
        if (!isServer)
        {
            this.transform.LookAt(Target.transform.position);
            if (AnimatedMesh != null)
                AnimatedMesh.SetTrigger("Attack");
        }
    }

    [Command]
    public void Cmd_UnitAttack(GameObject Target)
    {
        Rpc_UnitAttack(Target);

        this.transform.LookAt(Target.transform.position);
        if (AnimatedMesh != null)
            AnimatedMesh.SetTrigger("Attack");

    }

    [Command]
    public void Cmd_CounterAttack(GameObject Target)
    {

        StartCoroutine(CounterAttack(Target));
        Target.transform.LookAt(this.transform.position);
        Target.GetComponent<UnitManager>().Cmd_UnitAttack(Target);
    }

    [ClientRpc]
    public void Rpc_CounterAttack(GameObject Target)
    {

        StartCoroutine(CounterAttack(Target));
        Target.transform.LookAt(this.transform.position);
        Target.GetComponent<UnitManager>().Cmd_UnitAttack(Target);
    }

    public IEnumerator CounterAttack(GameObject Target)
    {

        yield return new WaitForSeconds(1.5f);

        Cmd_TakeDamage(this.gameObject, Target.GetComponent<UnitManager>().Damage);

        yield return new WaitForSeconds(1);

        isAttacking = false;
    }

    public IEnumerator LocalAttackUnit(GameObject Target)
    {
        if (isAttacking == false)
        {

            isAttacking = true;

            Cmd_UnitAttack(Target);

            yield return new WaitForSeconds(0.9f);
            
            if(isServer)
                Rpc_TakeDamage(Target , Damage);

            if(isClient && !isServer)
                Cmd_TakeDamage(Target, Damage);

            yield return new WaitForSeconds(1);
            //Contra ataques
            if (UnitType == 1)
            {

                // Guerreiro
                if (Target.GetComponent<UnitManager>().UnitType == 1)
                {
                    if (Target.GetComponent<UnitManager>().curHealth > 0)
                    {
                        if (isServer)
                        {
                            Rpc_CounterAttack(Target);
                        }
                        else
                        {

                            Cmd_CounterAttack(Target);
                        }
                    }

                    yield return new WaitForSeconds(1.25f);
                    isAttacking = false;
                }

                //Arqueiro
                if (Target.GetComponent<UnitManager>().UnitType == 2)
                {
                    
                    isAttacking = false;
                }

            }

            if (UnitType == 2)
            {

                // Guerreiro
                if (Target.GetComponent<UnitManager>().UnitType == 1)
                {
                    isAttacking = false;
                }

                //Arqueiro
                if (Target.GetComponent<UnitManager>().UnitType == 2)
                {
                    if (Target.GetComponent<UnitManager>().curHealth > 0)
                    {
                        if (isServer)
                        {
                            Rpc_CounterAttack(Target);
                        }
                        else
                        {

                            Cmd_CounterAttack(Target);
                        }
                    }

                    yield return new WaitForSeconds(1.25f);
                    isAttacking = false;
                }

            }

            //Explorador
            if (Target.GetComponent<UnitManager>().UnitType == 0)
            {

                isAttacking = false;
            }
        }
    }

    [ClientRpc]
    public void Rpc_TakeDamageAnim (GameObject Target)
    {
        if (!isServer)
        {
            if (Target.GetComponent<UnitManager>().AnimatedMesh != null)
                Target.GetComponent<UnitManager>().AnimatedMesh.SetTrigger("GetHit");
        }
    }

    [ClientRpc]
    public void Rpc_TakeDamage(GameObject Target , int Inc_Damage)
    {
        if (Target.GetComponent<UnitManager>().AnimatedMesh != null)
            Target.GetComponent<UnitManager>().AnimatedMesh.SetTrigger("GetHit");

        Target.GetComponent<UnitManager>().curHealth -= Inc_Damage;

    }

    [Command]
    public void Cmd_TakeDamage(GameObject Target , int Inc_Damage)
    {

        if (Target.GetComponent<UnitManager>().AnimatedMesh != null)
            Target.GetComponent<UnitManager>().AnimatedMesh.SetTrigger("GetHit");

        Target.GetComponent<UnitManager>().curHealth -= Inc_Damage;
        
        Rpc_TakeDamageAnim(Target);

    }



    void OnTriggerEnter( Collider other )
    {
        if( other.tag == "Tile")
        {
            SteppingTile = other.gameObject;
            PlayerOwner.GetComponent<PlayerBase>().Cmd_UpdateSteppingOnTile(other.gameObject, this.gameObject);
            StartCoroutine(StopRunning());
        }
    }

    public IEnumerator StopRunning()
    {
        yield return new WaitForSeconds(0.5f);
        Busy = false;
        Rpc_MakeUnitRun(false);
    }

    void OnTriggerExit(Collider other)
    {
        if (isAlive == true)
        {
            if (other.tag == "Tile")
            {
               // SteppingTile = null;
                PlayerOwner.GetComponent<PlayerBase>().Cmd_UpdateSteppingOnTile(other.gameObject, null);
            }
        }
    }

    [ClientCallback]
    public void Rpc_MoveTowardsPoint(Vector3 Pos)
    {
        GoToPos = Pos;
        Rpc_MakeUnitRun(true);
        Busy = true;
    }
}