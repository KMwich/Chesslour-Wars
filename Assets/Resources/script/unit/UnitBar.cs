using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public class UnitBar : Photon.MonoBehaviour {
    public HexGrid _hexGrid;

    public static UnitBar Instance;

    public PhotonView PhotonView;
    private int PlayersInGame = 0;

    public Unit unitPrefab;

    private int state;

    private List<int> UnitList;

    public static Unit selectUnit;
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
    public List<Vector3> defaultPosition;

    private void Awake() {
        Instance = this;
        print(GameManager.Instance.MainTypeUnit.Count);
        Units = new List<Unit>();
        defaultPosition = new List<Vector3>();
        PhotonView = GetComponent<PhotonView>();

        SceneManager.sceneLoaded += OnSceneFinishedLoading;

    }


    [PunRPC]
    private void UnitInstantiate(int i,int MainTypeUnit, int SubTypeUnit)
    {
        print("debug1");
        Units.Add(PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), new Vector3(0, 0, 1), Quaternion.identity, 0).GetComponent<Unit>());
        print("debug2");
        switch (MainTypeUnit)
        {
            case 0:
                Units[i].setUnitSprite(unit_database.units.Attacker[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Attacker[SubTypeUnit].SpritePath_img2;
                break;
            case 1:
                Units[i].setUnitSprite(unit_database.units.Supporter[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Supporter[SubTypeUnit].SpritePath_img2;
                break;
            case 2:
                Units[i].setUnitSprite(unit_database.units.Sturture[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Sturture[SubTypeUnit].SpritePath_img2;
                break;
            case 3:
                Units[i].setUnitSprite(unit_database.units.Trap[SubTypeUnit].SpritePath_img);
                Units[i].SpritePath = unit_database.units.Trap[SubTypeUnit].SpritePath_img2;
                break;
        }


        if ((MainTypeUnit == 0 && SubTypeUnit == 2) || (MainTypeUnit == 1 && SubTypeUnit == 0))
        {
            Units[i].transform.localScale = new Vector3(7, 7, 1);
        }


        Vector3 position;
        //if (PhotonNetwork.isMasterClient) position.x = i * 10;
        //else position.x = i * 10 + 50;
        position.x = i * 10;
        position.y = 2;
        position.z = 0;

        defaultPosition.Add(position);
        Units[i].transform.localPosition = position;
        Units[i].transform.SetParent(transform);

        print("create unit");
    }

    public void ready() {
        Debug.Log(true);
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
    }
}
