using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class slot2 : MonoBehaviour {
    public int referentIndex;
    public int mainIndex;
    public int UnitCoin;

    public void OnclickDelete()
    {
        GameStatus.deleteUnit(referentIndex);
    }
}
