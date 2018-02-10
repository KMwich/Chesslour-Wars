using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class UnitBar : Photon.MonoBehaviour {

    public static UnitBar Instance;

    private int readyState = 0;
    public int ready = 0;

    public PhotonView PhotonView;
    private int PlayersInGame = 0;

    public HexGrid _hexGrid;
    HexFilter hexFilter;

    public Unit unitPrefab;

    public bool isPlay = false;
    public int action = 0;

    public Unit selectUnit;

    private List<Unit> _units;
    private List<Unit> _units2;
    public List<Unit> Units
    {
        get { if (PhotonNetwork.isMasterClient) return _units;
            else return _units2;
        }
        set { if (PhotonNetwork.isMasterClient) _units = value;
            else _units2 = value;
        }
    }
    public List<Unit> enemyUnits {
        get {
            if (PhotonNetwork.isMasterClient) return _units2;
            else return _units;
        }
        set {
            if (PhotonNetwork.isMasterClient) _units2 = value;
            else _units = value;
        }
    }

    private List<Unit> _tower;
    private List<Unit> _tower2;
    public List<Unit> Towers {
        get {
            if (PhotonNetwork.isMasterClient) return _tower;
            else return _tower2;
        }
        set {
            if (PhotonNetwork.isMasterClient) _tower = value;
            else _tower2 = value;
        }
    }
    public List<Unit> enemyTowers {
        get {
            if (PhotonNetwork.isMasterClient) return _tower2;
            else return _tower;
        }
        set {
            if (PhotonNetwork.isMasterClient) _tower2 = value;
            else _tower = value;
        }
    }

    private void Awake() {
        Instance = this;
        print(GameManager.Instance.MainTypeUnit.Count);
        Units = new List<Unit>();
        Towers = new List<Unit>();
        enemyTowers = new List<Unit>();
        enemyUnits = new List<Unit>();
        PhotonView = GetComponent<PhotonView>();

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void Update() {
        if (isPlay) {
            if (Input.GetMouseButtonUp(1)) {
                clearSelectUnit();
            }
        }
    }

    [PunRPC]
    private void UnitInstantiate(int i,int MainTypeUnit, int SubTypeUnit)
    {
        Units.Add(PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), new Vector3(0, 0, 1), Quaternion.identity, 0).GetComponent<Unit>());
        switch (MainTypeUnit)
        {
            case 0:
                Units[i].setUnitSprite(unit_database.units.Attacker[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Attacker[SubTypeUnit].SpritePath_img2;
                Units[i].structure = unit_database.units.Attacker[SubTypeUnit];
                break;
            case 1:
                Units[i].setUnitSprite(unit_database.units.Supporter[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Supporter[SubTypeUnit].SpritePath_img2;
                Units[i].structure = unit_database.units.Supporter[SubTypeUnit];
                break;
            case 2:
                Units[i].setUnitSprite(unit_database.units.Sturture[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Sturture[SubTypeUnit].SpritePath_img2;
                Units[i].structure = unit_database.units.Sturture[SubTypeUnit];
                break;
            case 3:
                Units[i].setUnitSprite(unit_database.units.Trap[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Trap[SubTypeUnit].SpritePath_img2;
                Units[i].structure = unit_database.units.Trap[SubTypeUnit];
                break;
        }


        if ((MainTypeUnit == 0 && SubTypeUnit == 2) || (MainTypeUnit == 1 && SubTypeUnit == 0))
        {
            Units[i].transform.localScale = new Vector3(7, 7, 1);
        }

        Units[i].transform.position = new Vector3(-1920, -1920, -4);
        Units[i].transform.SetParent(transform);
        

        print("create unit " + i);
    }

    [PunRPC]
    public void towerInstantiate(int i, int x, int y, int type) {
        Towers.Add(PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), new Vector3(0, 0, 1), Quaternion.identity, 0).GetComponent<Unit>());
        Towers[i].isTower = true;
        switch (type) {
            case 0:
                Towers[i].setUnitSprite("sprite/unit/blue/CBcastle");
                Towers[i].SpritePath = "sprite/unit/red/CRcastle";
                break;
            case 1:
                Towers[i].setUnitSprite("sprite/unit/blue/CBnexus");
                Towers[i].SpritePath = "sprite/unit/red/CRnexus";
                break;
        }
        print(x + " " + y);
        Vector3 position = HexCoordinates.cubeToOffset(HexCoordinates.FromOffsetCoordinates(x, y));
        print(position.x + " " + position.y);
        Towers[i].setUnitPosition(position);
        Towers[i].transform.SetParent(transform);

        print("create tower");
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "map")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }

    }

    private void MasterLoadedGame()
    {
            PhotonView.RPC("RPC_CreateUnit", PhotonTargets.MasterClient);
    }

    private void NonMasterLoadedGame()
    {
            PhotonView.RPC("RPC_CreateUnit", PhotonTargets.MasterClient);
    }

    [PunRPC]
    private void RPC_CreateUnit()
    {
        print("start build");
        PlayersInGame++;
        if (PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("All players are in the game scene.");
            PhotonView.RPC("RPC_Build", PhotonTargets.AllBuffered);
            //UnitBar.Instance.createUnit(GameManager.Instance.MainTypeUnit.Count);
        }
    }

    [PunRPC]
    private void RPC_Build()
    {
        for (int i = 0; i < GameManager.Instance.MainTypeUnit.Count; i++)
        {
            UnitInstantiate(i, GameManager.Instance.MainTypeUnit[i], GameManager.Instance.SubTypeUnit[i]);
            //PhotonView.RPC("UnitInstantiate", PhotonTargets.Others, i, GameManager.Instance.MainTypeUnit[i], GameManager.Instance.SubTypeUnit[i]);

        }

        for (int i = 0; i < HexGrid.mapDetail.rtower.Length; i += 3) {
            if (PhotonNetwork.isMasterClient) {
                towerInstantiate(i / 3,HexGrid.mapDetail.btower[i], HexGrid.mapDetail.btower[i + 1], HexGrid.mapDetail.btower[i + 2]);
            } 
            else {
                towerInstantiate(i / 3, HexGrid.mapDetail.rtower[i], HexGrid.mapDetail.rtower[i + 1], HexGrid.mapDetail.rtower[i + 2]);
            }
            
        }
    }

    public void setPlayGame()
    {
        _hexGrid.clearHexFilter();

        if (ready == 1) { return; }
        if (readyState != 1)
        {
            ready++;
            PhotonView.RPC("updateRS", PhotonTargets.All);
        }
        else
        {
            PhotonView.RPC("readyPhase", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void readyPhase()
    {
        ready = 0;
        isPlay = true;
        ObjectManager.Instance.hide(1);
        ObjectManager.Instance.show(2);
    }

    public void setMove() {
        if (!StatusControl.Instance.active) return;
        if (selectUnit == null) return;
        if (selectUnit.haveMoved == true || selectUnit.havePlayed == true){ print("move already");  return; }
        action = 1;
        _hexGrid.setHexFilter(selectUnit.coordinate, selectUnit.structure.Movement, action);
        Debug.Log("setMove");
    }

    public void setAttack() {
        if (!StatusControl.Instance.active) return;
        if (selectUnit == null) return;
        if (selectUnit.havePlayed == true || StatusControl.Instance.ActionPoints == 0) { print("attack already"); return; }
        action = 2;
        _hexGrid.setHexFilter(selectUnit.coordinate, selectUnit.structure.Atkrange, action);
        Debug.Log("setAttack");
    }

    [PunRPC]
    private void updateRS()
    {
        readyState++;
    }

    public void setSelectUnit(int i) {
        selectUnit = Units[i];
    }

    public void clearSelectUnit() {
        selectUnit = null;
        action = 0;
        _hexGrid.clearHexFilter();
    }
}
