﻿using UnityEngine;
using UnityEngine.UI;


public class StatusControl : MonoBehaviour {

    public static StatusControl Instance;
    PhotonView PhotonView;
    private int turn;

    private int _actionPoints;
    private int _actionPoints2;
    private int ActionPoints
    {
        get { if (PhotonNetwork.isMasterClient) return _actionPoints; else return _actionPoints2; }
        set { if (PhotonNetwork.isMasterClient) _actionPoints = value; else _actionPoints2 = value; }
    }

    private int _maxActionPoints;
    private int _maxActionPoints2;
    private int MaxActionPoints
    {
        get { if (PhotonNetwork.isMasterClient) return _maxActionPoints; else return _maxActionPoints2; }
        set { if (PhotonNetwork.isMasterClient) _maxActionPoints = value; else _maxActionPoints2 = value; }
    }

    private int _unitRemaining;
    private int _unitRemaining2;
    private int UnitRemaining
    {
        get { if (PhotonNetwork.isMasterClient) return _unitRemaining; else return _unitRemaining2; }
        set { if (PhotonNetwork.isMasterClient) _unitRemaining = value; else _unitRemaining2 = value; }
    }

    private void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        turn = 1;
        if (PhotonNetwork.isMasterClient)
        {
            MaxActionPoints = 3;
            ActionPoints = MaxActionPoints;
        }
        else
        {
            MaxActionPoints = 4;
            ActionPoints = MaxActionPoints;
        }
        UnitRemaining = GameManager.Instance.MainTypeUnit.Count;
    }

    private void Update()
    {
        turn = turn;
        Instance.GetComponent<Transform>().Find("Turn").GetComponent<Transform>().Find("Text").GetComponent<Text>().text =  "Turn "+ turn;
        Instance.GetComponent<Transform>().Find("Unit num").GetComponent<Transform>().Find("Text").GetComponent<Text>().text = UnitRemaining + "/" + UnitRemaining;
        Instance.GetComponent<Transform>().Find("Stamina").GetComponent<Transform>().Find("Text").GetComponent<Text>().text = ActionPoints + "/" + MaxActionPoints;
    }

    public void ReActionPoints()
    {
        ActionPoints = MaxActionPoints;
    }

    public void EndTurn()
    {
        turn++;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(turn);
        }
        else
        {
            turn = (int)stream.ReceiveNext();
        }
    }

}