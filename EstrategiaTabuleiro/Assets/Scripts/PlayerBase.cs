using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PlayerBase : NetworkBehaviour {

    GameManager gameManager;
    PlayerManager playerManager;
    public GameObject Aguia_Explorer , Cao_Explorer, Rato_Explorer , Gato_Explorer;
    public GameObject Aguia_Warrior, Cao_Warrior , Rato_Warrior , Gato_Warrior;
    public GameObject Aguia_Archer, Cao_Archer , Rato_Archer , Gato_Archer;

    Button SpawnExplorerBtn, PassTurnButton, SpawnGuerreiroBtn, SpawnArqueiroBtn;
    public int Gold;
    public int Food;
    [SyncVar]
    public int PlayerBaseID;
    [SyncVar]
    public int curHealth;
    [SyncVar]
    public bool Occupied;
    [SyncVar]
    public bool Destroyed;

    public GameObject Farm;

    void Start()
    { 

        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();

        if (isLocalPlayer)
        {
            PlayerBaseID = GameObject.FindGameObjectsWithTag("PlayerBase").Length;
            gameManager.MyPlayerBase = this.gameObject;
        }

        if (PlayerBaseID == playerManager.PlayerID)
        {
            if (PlayerBaseID == 1)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 45, 0);
            }
            if (PlayerBaseID == 2)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 135, 0);
            }
            if (PlayerBaseID == 3)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 330, 0);
            }
            if (PlayerBaseID == 4)
            {
                GameObject.Find("CameraRotator").transform.Rotate(0, 210, 0);
            }
        }

        if (PlayerBaseID == playerManager.PlayerID)
        {
            SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
            SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer(PlayerBaseID,false));

            SpawnGuerreiroBtn = GameObject.Find("SpawnWarrior").GetComponent<Button>();
            SpawnGuerreiroBtn.onClick.AddListener(() => Cmd_SpawnGuerreiro(PlayerBaseID));

            SpawnArqueiroBtn = GameObject.Find("SpawnArcher").GetComponent<Button>();
            SpawnArqueiroBtn.onClick.AddListener(() => Cmd_SpawnArqueiro(PlayerBaseID));

            PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
            PassTurnButton.onClick.AddListener(() => Cmd_PassTurn());

            //Cmd_SpawnExplorer(true);
        }
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

        if(gameManager.curTurn == playerManager.MyTurn && Destroyed == true)
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

        if (gameManager.curTurn == playerManager.MyTurn && Destroyed == false)
        {
            PassTurnButton.interactable = true;

            if(Occupied == false)
            {
                SpawnExplorerBtn.interactable = true;
                SpawnGuerreiroBtn.interactable = true;
                SpawnArqueiroBtn.interactable = true;
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

        if( curHealth <= 0 && Destroyed == false)
        {
            Cmd_DestroyKingdom();
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
        var Explorer = Cao_Explorer;

        if(ExplorerID == 1)
        {
            Explorer = Cao_Explorer;
        }
        if(ExplorerID == 2)
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

        GameObject go = (GameObject)Instantiate( Explorer , this.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
        if (isFirst == true)
            go.GetComponent<UnitManager>().curActions = 3;
        
        Rpc_SetObjectOwner(go);
    }

    [Command]
    public void Cmd_SpawnGuerreiro(int WarriorID)
    {
        var Warrior = Cao_Warrior;

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

    [Command]
    public void Cmd_BuildFarm(Vector3 UnitPos)
    {
        GameObject go = (GameObject)Instantiate(Farm, UnitPos, Quaternion.identity);
        NetworkServer.Spawn(go);
        go.GetComponent<FarmManager>().Cmd_SetInitialOwner(this.gameObject);
    }


    [Command]
    public void Cmd_SpawnArqueiro(int ArcherID)
    {
        var Archer = Cao_Archer;

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

    [ClientRpc]
    public void Rpc_SetObjectOwner(GameObject LastCreatedObject)
    {
        LastCreatedObject.GetComponent<UnitManager>().PlayerOwner = this.gameObject;
    }

    [Command]
    public void Cmd_PassTurn()
    {
        gameManager.curTurn++;

        Gold++;

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
        Rpc_PassTurn();
    }

    [ClientRpc]
    public void Rpc_PassTurn()
    {
        Gold++;
        //Aqui carrego a variável com todos os objetos da cena que possuem o Tag "Unit"
        GameObject[] AllFriendlyUnits = GameObject.FindGameObjectsWithTag("Unit");

        //Este For loop, é para identificar qual das unidades do jogador que está atualmente selecionado e salvá-lo na variável "SelectedUnit"
        for (int i = 0; i < AllFriendlyUnits.Length; i++)
        {
            AllFriendlyUnits[i].GetComponent<UnitManager>().curActions = AllFriendlyUnits[i].GetComponent<UnitManager>().MaxActions;
            AllFriendlyUnits[i].GetComponent<UnitManager>().HasAttacked = false;
        }

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
}