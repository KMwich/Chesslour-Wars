using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot : MonoBehaviour {

    public int indexSlot;
    public int MainTyepUnit;
    public int UnitCoin;
    public GameObject box;

    public slot(int index,GameObject slot,int type,int coin)
    {
        indexSlot = index;
        box = slot;
        MainTyepUnit = type;
        UnitCoin = coin;
    }

    public void OnclickDelete()
    {
        Destroy(this.gameObject);
        GameStatus.coin += UnitCoin;
        GameStatus.slotCount--;
    }
}
