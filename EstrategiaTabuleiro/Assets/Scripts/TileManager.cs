using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TileManager : NetworkBehaviour {

    //Definição de variáveis
    public Color Idle, Hovering, Moveable;
    public Vector3 TargetScale;
    public Vector3 NormalScale, NormalPosition;
    public float TargetYPos;
    public GameObject SelectedUnit;
    public GameManager gameManager;
    public PlayerManager playerManager;
    public GameObject SteppingObject;
    public GameObject PlayerBase;
    public bool hasMonster , isMonsterTrigger , CanSpawnMonster = true;
    public GameObject CurrentMonster;
    public TileManager MonsterSpawner;
    public GameObject[] Monsters;

    //Definição de variáveis

    void Awake()
    {
        //Aqui estou pegando os valores iniciais de posição e escala do tile para poder resetar depois quando estiver alterado.
        NormalScale = this.transform.localScale;
        NormalPosition = this.transform.position;

        //Como o código é baseado na cor atual do material, estou colocando todas as tiles no inicio do jogo na cor Idle para depois poder verificar se esta é a cor atual sem ter conflitos
        GetComponent<MeshRenderer>().material.color = Idle;

        
    }

    void OnMouseEnter()
    {
        //Quando o mouse passa sobre este objeto, o objeto muda de cor , também fica maior e um pouco acima dos outros tiles

        //Neste pedaço estou Mudando a cor para Hovering
        GetComponent<MeshRenderer>().material.color = Hovering;

        // Aqui aumento e coloco este tile acima do resto para que se sobresaia
        this.transform.localScale = TargetScale;
        this.transform.position = new Vector3( NormalPosition.x , TargetYPos , NormalPosition.z ); 
    }

    void OnMouseExit()
    {
        //Quando o mouse sai de cima deste objeto a cor, o tamanho e a posição voltam ao normal.
        //Coloco a cor inicial de volta ao tile
        GetComponent<MeshRenderer>().material.color = Idle;

        //Reseto também o tamanho e a posição do tile para voltar ao normal
        this.transform.localScale = NormalScale;
        this.transform.position = NormalPosition;
    }

    public void SpawnMonster(GameObject Target)
    {
        if (hasMonster == true && CanSpawnMonster == true)
        {
            if (isServer)
            {
                GameObject go = (GameObject)Instantiate(Monsters[Random.Range(0, Monsters.Length)], new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z), this.transform.rotation);
                NetworkServer.Spawn(go);
                if (isServer)
                {
                    Rpc_SpawnMonster(go, Target);
                }
                else
                {
                    Cmd_SpawnMonster(go, Target);
                }
            }
        }
    }

    [Command]
    public void Cmd_SpawnMonster(GameObject go , GameObject Target)
    {
        if (isServer)
        {
            go.GetComponent<MonsterManager>().Rpc_AttackTarget(Target);
        }
        else
        {
            go.GetComponent<MonsterManager>().Cmd_AttackTarget(Target);
        }
        go.GetComponent<MonsterManager>().TileSpawner = this;
        CanSpawnMonster = false;
    }

    [ClientRpc]
    public void Rpc_SpawnMonster(GameObject go, GameObject Target)
    {
        if (isServer)
        {
            go.GetComponent<MonsterManager>().Rpc_AttackTarget(Target);
        }
        else
        {
            go.GetComponent<MonsterManager>().Cmd_AttackTarget(Target);
        }
        go.GetComponent<MonsterManager>().TileSpawner = this;
        CanSpawnMonster = false;
    }

    void Update()
    {


        if (gameManager != null && playerManager != null)
        {
            //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
            GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

            //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
            for (int i = 0; i < AllFriendlyUnits.Length; i++)
            {
                if (gameManager.curTurn == playerManager.MyTurn && AllFriendlyUnits[i].GetComponent<UnitManager>().enabled == true && AllFriendlyUnits[i].GetComponent<UnitManager>().Selected == true)
                {
                    SelectedUnit = AllFriendlyUnits[i].gameObject;
                }
            }

            if (SelectedUnit != null && SteppingObject == null)
            {
                //Esta parte serve para mostrar melhor os tiles para os quais o player pode se movimentar

                if (Vector3.Distance(this.transform.position, SelectedUnit.transform.position) <= 1.5f && SelectedUnit.GetComponent<UnitManager>().Busy == false)
                {
                    if (GetComponent<MeshRenderer>().material.color == Idle)
                    {
                        if (hasMonster == false)
                        {
                            GetComponent<MeshRenderer>().material.color = Moveable;
                            this.transform.position = new Vector3(NormalPosition.x, TargetYPos, NormalPosition.z);
                        }
                    }
                }
                else //Este Else des-seleciona as tiles que estão longe demais da unidade selecionada.

                {
                    if (GetComponent<MeshRenderer>().material.color == Moveable)
                    {
                        GetComponent<MeshRenderer>().material.color = Idle;
                        this.transform.position = NormalPosition;
                    }
                }
            }
            else //Este Else serve para des-selecionar as tiles quando o player for des-selecionado.
            {
                if (GetComponent<MeshRenderer>().material.color == Moveable)
                {
                    GetComponent<MeshRenderer>().material.color = Idle;
                    this.transform.position = NormalPosition;
                }
            }
        }
        else
        {
            try{
                gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
                playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
            }catch
            {
                print("Retrying to find managers...");
            }
        }
    }

    void OnMouseDown()
    {
        if (SelectedUnit != null && SteppingObject == null) //Ainda é preciso pedir a quantidade de ações que uma unidade pode executar
        {
            if (hasMonster == false) { 
                if (SelectedUnit.GetComponent<UnitManager>().PlayerOwner == PlayerBase || PlayerBase == null)
                {
                    if (SelectedUnit.GetComponent<UnitManager>().Busy == false)
                    {
                        if (Vector3.Distance(this.transform.position, SelectedUnit.transform.position) <= 1.5f)
                        {
                            SelectedUnit.GetComponent<UnitManager>().curActions--;
                            SelectedUnit.GetComponent<UnitManager>().PlayerOwner.GetComponent<PlayerBase>().Cmd_MoveUnit(SelectedUnit, this.gameObject);

                        }
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "PlayerBase")
        {
            PlayerBase = other.gameObject;
        }

        if( other.transform.tag == "Farm")
        {
            SteppingObject = other.transform.gameObject;
        }

        if (isMonsterTrigger)
        {
            if (other.transform.tag == "Unit")
            {
                MonsterSpawner.SpawnMonster(other.gameObject);
            }
        }
    }
}
