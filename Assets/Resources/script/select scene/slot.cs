using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slot : MonoBehaviour {

    public int indexSlot;
    public int MainTyepUnit;
    public int UnitCoin;
    public GameObject box;

    public slot(int index,GameObject slot)
    {
        indexSlot = index;
        box = slot;
    }
}
