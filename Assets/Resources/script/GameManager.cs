using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    private static List<int> _mainTypeUnit;
    private static List<int> _mainTypeUnit2;
    public List<int> MainTypeUnit
    {
        get
        {
            if (PhotonNetwork.isMasterClient)
                return _mainTypeUnit;
            else
                return _mainTypeUnit2;
        }
        set
        {
            if (PhotonNetwork.isMasterClient)
                _mainTypeUnit = value;
            else
                _mainTypeUnit2 = value;
        }
    }

    private static List<int> _subTypeUnit;
    private static List<int> _subTypeUnit2;
    public List<int> SubTypeUnit
    {
        get
        {
            if (PhotonNetwork.isMasterClient)
                return _subTypeUnit;
            else
                return _subTypeUnit2;
        }
        set
        {
            if (PhotonNetwork.isMasterClient)
                _subTypeUnit = value;
            else
                _subTypeUnit2 = value;
        }
    }
    //glo
    //public static List<int> mainTypeUnit = new List<int>();//send to map
    //public static List<int> subTypeUnit = new List<int>();//send to map
    public static List<int> coordinate = new List<int>();

    void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        MainTypeUnit = new List<int>();
        SubTypeUnit = new List<int>();
        print("Awake");
    }

}
