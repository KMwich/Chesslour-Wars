using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public static UnitManager Instance;
    private PhotonView PhotonView;

    [SerializeField]
    private HexGrid _hexGrid;
    public HexGrid HexGrid
    {
        get { return _hexGrid; }
    }

    [SerializeField]
    private Unit _unit;
    public Unit Unit
    {
        get { return _unit; }
    }

    private void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
    }

    public void ClickEnable()
    {
        PhotonView.RPC("OnEnableUnit", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void OnEnableUnit()
    {
        print("OK");
        for(int i = 0; i < UnitBar.Instance.Units.Count; i++ )
             UnitBar.Instance.Units[i].GetComponent<Unit>().gameObject.GetComponent<RectTransform>().localScale = new Vector3(8.0f, 8.0f, 1.0f);
    }
}
