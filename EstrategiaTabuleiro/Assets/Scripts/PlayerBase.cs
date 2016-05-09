using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerBase : NetworkBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    Text TempoTxt;
    public GameObject Aguia_Explorer , Cao_Explorer, Rato_Explorer , Gato_Explorer;
    public GameObject Aguia_Warrior, Cao_Warrior , Rato_Warrior , Gato_Warrior;
    public GameObject Aguia_Archer, Cao_Archer , Rato_Archer , Gato_Archer;

    Button SpawnExplorerBtn, PassTurnButton, SpawnGuerreiroBtn, SpawnArqueiroBtn;
    [SyncVar]
    public int Gold;
    [SyncVar]
    public int Food;
    [SyncVar]
    public int PlayerBaseID;
    [SyncVar]
    public int curHealth;
    [SyncVar]
    public bool Occupied;
    [SyncVar]
    public bool Destroyed;
    public Text GoldText, FoodText;

    //Váriaveis da gambiarra
    [SyncVar]
    float tempoTurno = 45;

    bool ReadyToPlay = false;

    public GameObject Farm;

    [Command]
    public void Cmd_UpdatePlayerBaseID(int ID)
    {
        PlayerBaseID = ID;
    }
    [ClientRpc]
    public void Rpc_UpdatePlayerBaseID(int ID)
    {
        PlayerBaseID = ID;
    }

    IEnumerator DelayedStart()
    {

        yield return new WaitForSeconds(0.5f);

        GoldText = GameObject.Find("_Dinheiro").GetComponent<Text>();
        FoodText = GameObject.Find("_Comida").GetComponent<Text>();
        TempoTxt = GameObject.Find("_Tempo").GetComponent<Text>();

        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();

        if (isLocalPlayer)
        {
            
            gameManager.MyPlayerBase = this.gameObject;
            playerManager.PlayerID = PlayerBaseID;
            playerManager.UpdateVariables();

        }

        if (isLocalPlayer)
        {
            if (PlayerBaseID == 1)
            {
                // Mat.material = MatBaseCao;
                GameObject.Find("CameraRotator").transform.Rotate(0, 45, 0);
            }
            if (PlayerBaseID == 2)
            {
                // Mat.material = MatBaseAguia;
                GameObject.Find("CameraRotator").transform.Rotate(0, 135, 0);
            }
            if (PlayerBaseID == 3)
            {
                // Mat.material = MatBaseGato;
                GameObject.Find("CameraRotator").transform.Rotate(0, 330, 0);
            }
            if (PlayerBaseID == 4)
            {
                // Mat.material = MatBaseRato;
                GameObject.Find("CameraRotator").transform.Rotate(0, 210, 0);
            }
        }

        if (isLocalPlayer)
        {
            SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
            SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer(PlayerBaseID, false));

            SpawnGuerreiroBtn = GameObject.Find("SpawnWarrior").GetComponent<Button>();
            SpawnGuerreiroBtn.onClick.AddListener(() => Cmd_SpawnGuerreiro(PlayerBaseID));

            SpawnArqueiroBtn = GameObject.Find("SpawnArcher").GetComponent<Button>();
            SpawnArqueiroBtn.onClick.AddListener(() => Cmd_SpawnArqueiro(PlayerBaseID));

            PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
            PassTurnButton.onClick.AddListener(() => Cmd_PassTurn());

            Cmd_SpawnExplorer(PlayerBaseID , true);
        }
        ReadyToPlay = true;
    }

    void Start()
    {
        StartCoroutine("DelayedStart");
        
    }

    [Command]
    public void Cmd_PassDestroyedTurn(int TargetTurn)
    {
        gameManager.curTurn = TargetTurn;
    }

    [ClientRpc]
    public void Rpc_PassDestroyedTurn(int TargetTurn)
    {
        gameManager.curTurn = TargetTurn;
    }

    void Update()
    {
        if (ReadyToPlay == true)
        {
            if (gameManager == null)
            {
                gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
            }
            if (playerManager == null)
            {
                playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
            }

            GoldText.text = "Gold : " + Gold;
            FoodText.text = "Food : " + Food;

            if (gameManager.curTurn == playerManager.MyTurn && Destroyed == true)
            {
                if (isServer)
                {
                    Rpc_PassDestroyedTurn(playerManager.MyTurn + 1);
                }
                else
                {
                    Cmd_PassDestroyedTurn(playerManager.MyTurn + 1);
                }

            }

           // if (isServer)
          //  {
               // ControlaTempoTurno();
           // }
            

            if (gameManager.curTurn == playerManager.MyTurn && Destroyed == false)
            {
                PassTurnButton.interactable = true;

                if (Occupied == false)
                {
                    //Explorador
                    if (Gold >= 2)
                    {
                        SpawnExplorerBtn.interactable = true;
                    }
                    else
                    {
                        SpawnExplorerBtn.interactable = false;
                    }

                    //Guerreiro
                    if (Gold >= 5)
                    {
                        SpawnGuerreiroBtn.interactable = true;
                    }
                    else
                    {
                        SpawnGuerreiroBtn.interactable = false;
                    }

                    //Arqueiro
                    if (Gold >= 7)
                    {
                        SpawnArqueiroBtn.interactable = true;
                    }
                    else
                    {
                        SpawnArqueiroBtn.interactable = false;
                    }
                }
                else
                {
                    SpawnExplorerBtn.interactable = false;
                    SpawnGuerreiroBtn.interactable = false;
                    SpawnArqueiroBtn.interactable = false;
                }

            }
            else
            {
                PassTurnButton.interactable = false;
                SpawnExplorerBtn.interactable = false;
                SpawnGuerreiroBtn.interactable = false;
                SpawnArqueiroBtn.interactable = false;
            }

            if (curHealth <= 0 && Destroyed == false)
            {
                Cmd_DestroyKingdom();
            }
        }
    }

    [Command]
    public void Cmd_DestroyKingdom()
    {
        Destroyed = true;
        var AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        for(int i = 0; i < AllUnits.Length; i++)
        {
            if(AllUnits[i].gameObject.GetComponent<UnitManager>().PlayerOwner == this.gameObject)
            {
                AllUnits[i].GetComponent<UnitManager>().curHealth = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Unit")
            Occupied = true;

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Unit")
            Occupied = false;
    }

    [Command]
    public void Cmd_SwitchOccupied()
    {
        Occupied = false;
    }

    [Command]
    public void Cmd_SpawnExplorer( int ExplorerID , bool isFirst )
    {
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        int AmmountOfFriendlyUnits = 0;
        for(int i = 0; i < AllUnits.Length; i++)
        {
            if(AllUnits[i].GetComponent<UnitManager>().PlayerOwner == this.gameObject )
                AmmountOfFriendlyUnits++;
        }
        if (Food > AmmountOfFriendlyUnits && AmmountOfFriendlyUnits <= 5)
        {
            var Explorer = Cao_Explorer;
            if (isFirst == false)
            {
                if (Gold >= 2)
                {
                    Gold -= 2;

                    if (ExplorerID == 1)
                    {
                        Explorer = Cao_Explorer;
                    }
                    if (ExplorerID == 2)
                    {
                        Explorer = Aguia_Explorer;
                    }
                    if (ExplorerID == 3)
                    {
                        Explorer = Rato_Explorer;
                    }
                    if (ExplorerID == 4)
                    {
                        Explorer = Gato_Explorer;
                    }

                    GameObject go = (GameObject)Instantiate(Explorer, this.transform.position, Quaternion.identity);
                    NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
                    go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
                    if (isFirst == true)
                        go.GetComponent<UnitManager>().curActions = 3;

                    Rpc_SetObjectOwner(go);
                }
            }
            else
            {
                if (ExplorerID == 1)
                {
                    Explorer = Cao_Explorer;
                }
                if (ExplorerID == 2)
                {
                    Explorer = Aguia_Explorer;
                }
                if (ExplorerID == 3)
                {
                    Explorer = Rato_Explorer;
                }
                if (ExplorerID == 4)
                {
                    Explorer = Gato_Explorer;
                }

                GameObject go = (GameObject)Instantiate(Explorer, this.transform.position, Quaternion.identity);
                NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
                go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
                if (isFirst == true)
                    go.GetComponent<UnitManager>().curActions = 3;

                Rpc_SetObjectOwner(go);
            }
        }
    }

    [Command]
    public void Cmd_SpawnGuerreiro(int WarriorID)
    {
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        int AmmountOfFriendlyUnits = 0;
        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].GetComponent<UnitManager>().PlayerOwner == this.gameObject)
                AmmountOfFriendlyUnits++;
        }
        if (Food > AmmountOfFriendlyUnits && AmmountOfFriendlyUnits <= 5)
        {
            var Warrior = Cao_Warrior;

            if (Gold >= 5)
            {
                Gold -= 5;

                if (WarriorID == 1)
                {
                    Warrior = Cao_Warrior;
                }
                if (WarriorID == 2)
                {
                    Warrior = Aguia_Warrior;
                }
                if (WarriorID == 3)
                {
                    Warrior = Rato_Warrior;
                }
                if (WarriorID == 4)
                {
                    Warrior = Gato_Warrior;
                }

                GameObject go = (GameObject)Instantiate(Warrior, this.transform.position, Quaternion.identity);
                NetworkServer.SpawnWithClientAuthority(go, this.gameObject);
                go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

                Rpc_SetObjectOwner(go);
            }
        }
    }

    [Command]
    public void Cmd_BuildFarm(Vector3 UnitPos)
    {
        GameObject go = (GameObject)Instantiate(Farm, UnitPos, Quaternion.identity);
        NetworkServer.Spawn(go);
        if (isServer)
        {
            go.GetComponent<FarmManager>().Rpc_SetInitialOwner(this.gameObject);

        }
        else
        {
            go.GetComponent<FarmManager>().Cmd_SetInitialOwner(this.gameObject);
        }
        
    }


    [Command]
    public void Cmd_SpawnArqueiro(int ArcherID)
    {
        GameObject[] AllUnits = GameObject.FindGameObjectsWithTag("Unit");
        int AmmountOfFriendlyUnits = 0;
        for (int i = 0; i < AllUnits.Length; i++)
        {
            if (AllUnits[i].GetComponent<UnitManager>().PlayerOwner == this.gameObject)
                AmmountOfFriendlyUnits++;
        }
        if (Food > AmmountOfFriendlyUnits && AmmountOfFriendlyUnits <= 5)
        {
            var Archer = Cao_Archer;

            if (Gold >= 7)
            {
                Gold -= 7;


                if (ArcherID == 1)
                {
                    Archer = Cao_Archer;
                }
                if (ArcherID == 2)
                {
                    Archer = Aguia_Archer;
                }
                if (ArcherID == 3)
                {
                    Archer = Rato_Archer;
                }
                if (ArcherID == 4)
                {
                    Archer = Gato_Archer;
                }

                GameObject go = (GameObject)Instantiate(Archer, this.transform.position, Quaternion.identity);
                NetworkServer.SpawnWithClientAuthority(go, this.gameObject);
                go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;

                Rpc_SetObjectOwner(go);
            }
        }
    }

    [ClientRpc]
    public void Rpc_SetObjectOwner(GameObject LastCreatedObject)
    {
        LastCreatedObject.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
    }

    [Command]
    public void Cmd_PassTurn()
    {
        gameManager.curTurn++;
        //tempoTurno = 45;

        int GoldToGive = 1;
        GameObject[] AllGoldMines = GameObject.FindGameObjectsWithTag("GoldMine");
        for(int i =0; i < AllGoldMines.Length; i++)
        {
            if (AllGoldMines[i].GetComponent<GoldMineManager>().PlayerOwner != null)
            {
                if (AllGoldMines[i].GetComponent<GoldMineManager>().PlayerOwner == this.gameObject)
                {
                    GoldToGive++;
                }
            }
        }
        Gold += GoldToGive;

        if (gameManager.curTurn > gameManager.MaxTurns)
            gameManager.curTurn = 1;

        //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
        for (int i = 0; i < AllFriendlyUnits.Length; i++)
        {
            AllFriendlyUnits[i].GetComponent<UnitManager>().curActions = AllFriendlyUnits[i].GetComponent<UnitManager>().MaxActions;
            AllFriendlyUnits[i].GetComponent<UnitManager>().HasAttacked = false;
        }

        if (isServer)
        {
            ResetTimer();
        }
        else
        {
            Cmd_ResetTimer();
        }

        Rpc_PassTurn();

    }

    [ClientRpc]
    public void Rpc_PassTurn()
    {
        //tempoTurno = 45;
        //Gold++;
        //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
        for (int i = 0; i < AllFriendlyUnits.Length; i++)
        {
            AllFriendlyUnits[i].GetComponent<UnitManager>().curActions = AllFriendlyUnits[i].GetComponent<UnitManager>().MaxActions;
            AllFriendlyUnits[i].GetComponent<UnitManager>().HasAttacked = false;
        }
        if (isServer)
        {
            ResetTimer();
        }
        else
        {
            Cmd_ResetTimer();
        }
    }

    [Command]
    void Cmd_ResetTimer()
    {
        tempoTurno = 45;
    }

    void ResetTimer()
    {
        tempoTurno = 45;
    }

    [Command]
    public void Cmd_MoveUnit(GameObject Obj, GameObject Tile)
    {

        Obj.GetComponent<UnitManager>().Rpc_MoveTowardsPoint(new Vector3(Tile.transform.position.x , Tile.transform.position.y + 0.15f , Tile.transform.position.z ));
        
        Rpc_MoveUnit(Obj, Tile);

    }

    [ClientRpc]
    public void Rpc_MoveUnit(GameObject Obj, GameObject Tile)
    {

        Obj.GetComponent<UnitManager>().Rpc_MoveTowardsPoint(new Vector3(Tile.transform.position.x, Tile.transform.position.y + 0.15f, Tile.transform.position.z));
        
    }

    [Command]
    public void Cmd_UpdateSteppingOnTile(GameObject tile , GameObject Obj)
    {
        
        tile.GetComponent<TileManager>().SteppingObject = Obj;
        Rpc_UpdateSteppingOnTile(tile, Obj);
    }

    [ClientRpc]
    public void Rpc_UpdateSteppingOnTile(GameObject tile, GameObject Obj)
    {
        
        tile.GetComponent<TileManager>().SteppingObject = Obj;
    }

    public void ControlaTempoTurno() {
        
        //Crio uma variável que guarda o tempo em segundos
        tempoTurno = tempoTurno - Time.deltaTime;
        TempoTxt.text = Convert.ToInt32(tempoTurno).ToString();
        if (tempoTurno <= 0)
        {
            Cmd_PassTurn(); 
            if (isServer)
            {
                ResetTimer(); // CRIARAM UMA FUNÇÃO PRA CONTROLAR O TEMPO USAM A BASE QUE FIZ MAS MUDARAM ;/ DAI NEM ROLA, SE FOR PRA MUDAR O CARA LEVA TEMPO PRA ENTENDER ENTÃO ELE PODE FAZER :S
            }
            else
            {
                Cmd_ResetTimer(); //MEXERAM AQUI!!! EU TO FAZENDO DE FORMA PRIMITIVA, NUNCA TRAMPEI COM SERVER MUITO MENOS NETWORK PRA UM GAME
            }
            tempoTurno = 45;
        }
    }
}