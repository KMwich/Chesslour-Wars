using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour {

    public static int damageType=0;
    public static int supportType=0;
    public static int objectType=0;
    public static int trapType=0;
    public static int coin=18;
    public static int countTrap=0;
    public static List<int> mainTypeUnit = new List<int>();
    public static List<int> supTypeUnit = new List<int>();

    private bool sellSucess;

    public void SetCoinOnClick(int sellcoin)
    {
        //use in this screen
        if(coin - sellcoin >= 0)
        {
            coin -= sellcoin;
            sellSucess = true;
        }
        else
        {
            sellSucess = false;
        }
    }

    public void SetTrapOnclick()
    {
        if(countTrap + 1 <= 5)
        {
            countTrap++;
            sellSucess = true;
        }
        else
        {
            sellSucess = false;
        }
    }

    public void SetTypeOnClick(int Type)
    {
        if (sellSucess)
        {
            //use in this screen
            if (Type == 0)
            {
                damageType++;
            }
            else if (Type == 1)
            {
                supportType++;
            }
            else if (Type == 2)
            {
                objectType++;
            }
            else if (Type == 3)
            {
                trapType++;
            }

            //send to map screen as referent index unit_database
            mainTypeUnit.Add(Type);//contain index [0-3] only!!!
        }
    }

    public void SetIndexInType(int supType)
    {
        if (sellSucess)
        {
            //send to map screen as referent index each aerry in unit_database
            supTypeUnit.Add(supType);
        }
    }
}
