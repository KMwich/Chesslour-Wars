using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {

    public static UnitManager Instance;

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
    }
}
