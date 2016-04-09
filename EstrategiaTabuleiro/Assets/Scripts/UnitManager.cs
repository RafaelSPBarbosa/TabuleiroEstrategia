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
    public int curHealth = 1;
    public int MaxHealth = 1; 
    public int Damage = 0;
    public int SkillPoints = 20;
    public int UnitType = 0;
    public Animator AnimatedMesh;
    public Vector3 GoToPos;
    public bool Busy = false;
    //Definição de Variáveis

    public void ReloadActions()
    {
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

    void DeSelectUnit()
    {
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

    [ClientCallback]
    public void Rpc_MakeUnitRun( bool state)
    {
        if(AnimatedMesh != null)
            AnimatedMesh.SetBool("Running", state);
    }

    void Update()
    {

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

        if(gameManager.curTurn != playerManager.MyTurn)
        {
            DeSelectUnit();
        }

        this.transform.LookAt(GoToPos);
        this.transform.position = Vector3.MoveTowards(this.transform.position, GoToPos, Time.deltaTime );

    }

    void OnTriggerEnter( Collider other )
    {
        if( other.tag == "Tile")
        {
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
        if (other.tag == "Tile")
        {
            PlayerOwner.GetComponent<PlayerBase>().Cmd_UpdateSteppingOnTile(other.gameObject, null );
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
