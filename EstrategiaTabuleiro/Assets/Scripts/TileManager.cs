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
    //Definição de variáveis

    void Awake()
    {
        //Aqui estou pegando os valores iniciais de posição e escala do tile para poder resetar depois quando estiver alterado.
        NormalScale = this.transform.localScale;

        NormalPosition = this.transform.position;

        //Como o código é baseado na cor atual do material, estou colocando todas as tiles no inicio do jogo na cor Idle para depois poder verificar se esta é a cor atual sem ter conflitos
        GetComponent<MeshRenderer>().material.color = Idle;

        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        
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

            if (SelectedUnit != null) //TODO: Ainda é preciso pedir a quantidade de ações que uma unidade pode executar
            {
                //Esta parte serve para mostrar melhor os tiles para os quais o player pode se movimentar
                if (Vector3.Distance(this.transform.position, SelectedUnit.transform.position) <= 1.5f)
                {
                    if (GetComponent<MeshRenderer>().material.color == Idle)
                    {
                        GetComponent<MeshRenderer>().material.color = Moveable;
                        this.transform.position = new Vector3(NormalPosition.x, TargetYPos, NormalPosition.z);
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
                playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
                gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        }

      
    }

    void OnMouseDown()
    {
        if( SelectedUnit != null ) //Ainda é preciso pedir a quantidade de ações que uma unidade pode executar
        {
            if( Vector3.Distance( this.transform.position , SelectedUnit.transform.position ) <= 1.5f)
            {
                SelectedUnit.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z ) ;
                SelectedUnit.GetComponent<UnitManager>().curActions--;
            }
        }
    }
}
