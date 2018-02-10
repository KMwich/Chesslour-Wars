using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUnitBar_Slot : MonoBehaviour {

    public int UnitIndex;//no change
    public void DeployOnClick()
    {
        SetUnitBar.Instance.deployUnit(UnitIndex);
    }
}
