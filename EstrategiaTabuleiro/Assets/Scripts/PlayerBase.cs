﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerBase : NetworkBehaviour {

    public GameManager gameManager;
    public PlayerManager playerManager;
    NetManager netManager;

    Text TempoTxt;
    public GameObject Aguia_Explorer , Cao_Explorer, Rato_Explorer , Gato_Explorer;
    public GameObject Aguia_Warrior, Cao_Warrior , Rato_Warrior , Gato_Warrior;
    public GameObject Aguia_Archer, Cao_Archer , Rato_Archer , Gato_Archer;

    Button SpawnExplorerBtn, PassTurnButton, SpawnGuerreiroBtn, SpawnArqueiroBtn;
    [SyncVar]
    public string test="test";
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
    [SyncVar]
    public int LastAttackingPlayerId ;
    public Text GoldText, FoodText;

    public Vector3 WinningPlayerPos;

    public int GoldMineAmmount = 0;

    public Text ObjectiveText;
    [SyncVar]
    public int MyObjective;

    bool CanMoveCamera = false;

    string PlayerString;
    //Váriaveis da gambiarra
    [SyncVar]
    float tempoTurno = 45;

    bool ReadyToPlay = false;

    public GameObject Farm;

    public bool GameOver = false;

    bool loadedGameLevel = false;
    bool Initialized = false;

    [Command]
    public void Cmd_UpdatePlayerBaseID(int ID)
    {
        PlayerBaseID = ID - GameObject.Find("NetManager").GetComponent<NetworkManager>().numPlayers;
    }
    [ClientRpc]
    public void Rpc_UpdatePlayerBaseID(int ID)
    {
        //PlayerBaseID = ID;
        Debug.Log("RPC ID Update");
    }

    [Command]
    public void Cmd_UpdateGoldMineAmmount( bool isIncreasing)
    {
        if (isIncreasing)
        {
            GoldMineAmmount++;

        }
        else
        {
            GoldMineAmmount--;
        }
    }

    [ClientRpc]
    public void Rpc_UpdateGoldMineAmmount(bool isIncreasing)
    {
        if (isIncreasing)
        {
            GoldMineAmmount++;

        }
        else
        {
            GoldMineAmmount--;
        }
    }

    IEnumerator DelayedStart()
    {
        /* GameObject[] AllLobbyPlayers = GameObject.FindGameObjectsWithTag("LobbyPlayer");
         for(int i = 0; i < AllLobbyPlayers.Length; i++)
         {
             if(AllLobbyPlayers[i].GetComponent<LobbyPlayerUpdate>().isMine == true)
             {
                 PlayerBaseID = AllLobbyPlayers[i].GetComponent<LobbyPlayerUpdate>().PlayerId;
             }
         }*/
        //print(Convert.ToInt32(playerControllerId));
        
        //Cmd_UpdatePlayerBaseID(Convert.ToInt32(playerControllerId));

        //PlayerBaseID = GetComponent<NetworkIdentity>().NetworkConnection.connectionId();

        yield return new WaitForSeconds(2);

        GoldText = GameObject.Find("_Dinheiro").GetComponent<Text>();
        FoodText = GameObject.Find("_Comida").GetComponent<Text>();
        TempoTxt = GameObject.Find("_Tempo").GetComponent<Text>();
        ObjectiveText = GameObject.Find("_ObjectiveText").GetComponent<Text>();

        gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
        playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();

        gameManager.MyPlayerBase = this.gameObject;
        playerManager.PlayerID = PlayerBaseID;
        playerManager.UpdateVariables();

        if (PlayerBaseID == 1)
        {
            // Mat.material = MatBaseCao;
            GameObject CameraRot = GameObject.Find("CameraRotator");
            CameraRot.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 45, this.transform.eulerAngles.z);
        }
        if (PlayerBaseID == 4)
        {
            // Mat.material = MatBaseGato;
            GameObject CameraRot = GameObject.Find("CameraRotator");
            CameraRot.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 210, this.transform.eulerAngles.z);
        }
        if (PlayerBaseID == 3)
        {
            // Mat.material = MatBaseRato;
            GameObject CameraRot = GameObject.Find("CameraRotator");
            CameraRot.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 330, this.transform.eulerAngles.z);
        }
        if (PlayerBaseID == 2)
        {
            // Mat.material = MatBaseAguia;
            GameObject CameraRot = GameObject.Find("CameraRotator");
            CameraRot.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 135, this.transform.eulerAngles.z);
        }

        SpawnExplorerBtn = GameObject.Find("SpawnExplorer").GetComponent<Button>();
        SpawnExplorerBtn.onClick.AddListener(() => Cmd_SpawnExplorer(PlayerBaseID, false));

        SpawnGuerreiroBtn = GameObject.Find("SpawnWarrior").GetComponent<Button>();
        SpawnGuerreiroBtn.onClick.AddListener(() => Cmd_SpawnGuerreiro(PlayerBaseID));

        SpawnArqueiroBtn = GameObject.Find("SpawnArcher").GetComponent<Button>();
        SpawnArqueiroBtn.onClick.AddListener(() => Cmd_SpawnArqueiro(PlayerBaseID));

        PassTurnButton = GameObject.Find("PassTurnBtn").GetComponent<Button>();
        PassTurnButton.onClick.AddListener(() => Cmd_PassTurn(PlayerBaseID));

        Cmd_SpawnExplorer(PlayerBaseID, true);

        if (isServer)
            DistributeObjectives();

        ReadyToPlay = true;
    }

    void DistributeObjectives()
    {
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("PlayerBase");
        List<int> UsedIds = new List<int>();
        for (int i = 0; i < AllPlayers.Length; i++)
        {
            int id = UnityEngine.Random.Range(1, 6);
            if (UsedIds.Count != 6)
            {
                while (UsedIds.Contains(id))
                {
                    id = UnityEngine.Random.Range(1, 6);
                }
            }
            
            UsedIds.Add(id);
            AllPlayers[i].GetComponent<PlayerBase>().Rpc_RecieveObjective(id);
        }
    }

    [ClientRpc]
    void Rpc_RecieveObjective(int ID)
    {
        MyObjective = ID;
        ObjectiveText = GameObject.Find("_ObjectiveText").GetComponent<Text>();
        if (isLocalPlayer)
        {
            switch (MyObjective)
            {
                case 1:
                    //print("Elimine o Jogador Vermelho");
                    ObjectiveText.text = "Elimine o Jogador Vermelho";
                    break;
                case 2:
                    //print("Elimine o Jogador Amarelo");
                    ObjectiveText.text = "Elimine o Jogador Amarelo";
                    break;
                case 3:
                    //print("Elimine o Jogador Verde");
                    ObjectiveText.text = "Elimine o Jogador Verde";
                    break;
                case 4:
                    //print("Elimine o Jogador Azul");
                    ObjectiveText.text = "Elimine o Jogador Azul";
                    break;
                case 5:
                    //print("Possua 10 Plantações e 2 Minas de Ouro");
                    ObjectiveText.text = "Possua 10 Plantações e 2 Minas de Ouro";
                    break;
            }
        }
    }

    void Start()
    {
        Cmd_UpdatePlayerBaseID(Convert.ToInt32(GetComponent<NetworkIdentity>().netId.Value));
        DontDestroyOnLoad(this.gameObject);
        // PlayerString = Guid.NewGuid().ToString();
        /* netManager = GameObject.Find("NetManager").GetComponent<NetManager>();
         //netManager.storage.Rpc_GetPlayerNumber(netManager.net_Guid);
         for (int x = 0; x < netManager.storage.ids.Count; x++)
         {
             if (netManager.storage.ids[x] == netManager.net_Guid)
             {
                 //Found the client, so X + 1 is equal to their player number.
                 PlayerBaseID = x + 1;
                 break;
             }
         }
         */
        //PlayerBaseID = netManager.storage.my_Player_Number;

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

    [ClientRpc]
    public void Rpc_UpdateWinningPlayer(Vector3 Pos)
    {
        WinningPlayerPos = Pos;
    }

    [Command]
    public void Cmd_UpdateWinningPlayer(Vector3 Pos)
    {
        WinningPlayerPos = Pos;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if(Initialized == false)
            {
                if (isLocalPlayer)
                {
                    StartCoroutine("DelayedStart");
                    Initialized = true;
                }
            }

            // if (gameManager == null)
            //  gameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();
            //if (playerManager == null)
            // playerManager = GameObject.Find("_PlayerManager").GetComponent<PlayerManager>();
            if (GameOver == true)
            {
                Camera.main.transform.parent.transform.position = Vector3.Lerp(Camera.main.transform.parent.transform.position, new Vector3(WinningPlayerPos.x, 7.1f, WinningPlayerPos.z), Time.deltaTime * 2);
            }

            if (CanMoveCamera == true)
            {
                GameObject.Find("CameraRotator").transform.position = Vector3.Lerp(GameObject.Find("CameraRotator").transform.position, new Vector3(WinningPlayerPos.x, 0, WinningPlayerPos.z), Time.deltaTime * 2);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {

                GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("PlayerBase");
                for (int i = 0; i < AllPlayers.Length; i++)
                {
                    if (AllPlayers[i] != this.gameObject)
                    {

                        //if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                        if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                        {
                            AllPlayers[i].GetComponent<PlayerBase>().Rpc_UpdateWinningPlayer(this.transform.position);
                            AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                        }
                        else {
                            AllPlayers[i].GetComponent<PlayerBase>().Cmd_UpdateWinningPlayer(this.transform.position);
                            AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                        }
                    }
                }
                if (isServer)
                {
                    //Rpc_UpdateWinningPlayer(this.transform.position);
                    WinMatch();
                }
                else
                {
                    //Cmd_UpdateWinningPlayer(this.transform.position);
                    WinMatch();
                }
            }
            if (ReadyToPlay == true)
            {


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

            //Condições de vitória
            if (MyObjective == 5)
            {
                if (Food == 11 & GoldMineAmmount == 2)
                {

                    WinMatch();
                    GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("PlayerBase");
                    for (int i = 0; i < AllPlayers.Length; i++)
                    {
                        if (!AllPlayers[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                        {
                            if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                            {
                                AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                                AllPlayers[i].GetComponent<PlayerBase>().Rpc_UpdateWinningPlayer(this.transform.position);
                            }

                            else
                            {
                                AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                                AllPlayers[i].GetComponent<PlayerBase>().Cmd_UpdateWinningPlayer(this.transform.position);
                            }
                        }
                    }
                }
            }
        }
    }

    public void WinMatch()
    {
        ObjectiveText.text = "YOU WON THE GAME!!";
        WinningPlayerPos = this.transform.position;
        StartCoroutine("RepositionCamera");
    }

    [Command]
    public void Cmd_LooseMatch()
    {
        ObjectiveText.text = "YOU LOST THE GAME!!";
        StartCoroutine("RepositionCamera");
    }

    [ClientRpc]
    public void Rpc_LooseMatch()
    {
        ObjectiveText.text = "YOU LOST THE GAME!!";
        StartCoroutine("RepositionCamera");
    }

    public IEnumerator RepositionCamera()
    {
        yield return new WaitForSeconds(2);
        Camera.main.transform.parent.GetComponent<MoveCamera>().enabled = false;
        GameOver = true;
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
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("PlayerBase");
        GameObject TargetPlayer = null;
        for(int i = 0; i < AllPlayers.Length; i++)
        {
            if(AllPlayers[i].GetComponent<PlayerBase>().PlayerBaseID == LastAttackingPlayerId)
            {
                TargetPlayer = AllPlayers[i];
            }
        }

        if(TargetPlayer.GetComponent<PlayerBase>().MyObjective == 1 && PlayerBaseID == 1)
        {
            TargetPlayer.GetComponent<PlayerBase>().WinMatch();
            WinningPlayerPos = TargetPlayer.transform.position;
            for (int i = 0; i < AllPlayers.Length; i++)
            {
                if (!AllPlayers[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                    }
                    else
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                    }
                }
            }
        }
        if (TargetPlayer.GetComponent<PlayerBase>().MyObjective == 2 && PlayerBaseID == 2)
        {
            TargetPlayer.GetComponent<PlayerBase>().WinMatch();
            WinningPlayerPos = TargetPlayer.transform.position;

            for (int i = 0; i < AllPlayers.Length; i++)
            {
                if (!AllPlayers[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                    }
                    else
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                    }
                }
            }
        }
        if (TargetPlayer.GetComponent<PlayerBase>().MyObjective == 3 && PlayerBaseID == 4)
        {
            TargetPlayer.GetComponent<PlayerBase>().WinMatch();
            WinningPlayerPos = TargetPlayer.transform.position;

            for (int i = 0; i < AllPlayers.Length; i++)
            {
                if (!AllPlayers[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                    }
                    else
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                    }
                }
            }
        }
        if (TargetPlayer.GetComponent<PlayerBase>().MyObjective == 4 && PlayerBaseID == 3)
        {
            TargetPlayer.GetComponent<PlayerBase>().WinMatch();
            WinningPlayerPos = TargetPlayer.transform.position;

            for (int i = 0; i < AllPlayers.Length; i++)
            {
                if (!AllPlayers[i].GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    if (AllPlayers[i].GetComponent<NetworkIdentity>().isServer)
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Cmd_LooseMatch();
                    }
                    else
                    {
                        AllPlayers[i].GetComponent<PlayerBase>().Rpc_LooseMatch();
                    }
                }
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
    public void Cmd_PassTurn(int ID)
    {
        GameManager TempGameManager = GameObject.Find("_GameManager").GetComponent<GameManager>();

        if (TempGameManager.curTurn == ID)
        {
            TempGameManager.curTurn++;
            //tempoTurno = 45;

            int GoldToGive = 1;
            GameObject[] AllGoldMines = GameObject.FindGameObjectsWithTag("GoldMine");
            for (int i = 0; i < AllGoldMines.Length; i++)
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

            if (TempGameManager.curTurn > TempGameManager.MaxTurns)
                TempGameManager.curTurn = 1;

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
            Cmd_PassTurn(PlayerBaseID); 
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