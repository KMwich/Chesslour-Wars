using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class chess {
    public static int Hp;//zero is destroy
    public static int cooldown;//cooldown skill
    public static int positionX;
    public static int positionY;//in game may be is Z
    public static string type;//Ex can walk pass mountain?
    public static GameObject body; //player chess or enemy chess?

    public static int count;//count number of buff
    public static int[] buffIndex;
}
