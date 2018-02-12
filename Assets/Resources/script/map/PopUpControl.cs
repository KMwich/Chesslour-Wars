using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUpControl : MonoBehaviour {

    public static PopUpControl Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (UnitBar.Instance.selectUnit != null) stageOne();
        else this.GetComponent<Transform>().Find("pop hub stage1").localScale = new Vector3(0, 1, 1);

    }

    public void stageOne()
    {
        //print("stage1");
        this.GetComponent<Transform>().Find("pop hub stage1").localScale = new Vector3(1, 1, 1);
        this.GetComponent<Transform>().Find("pop hub stage1").GetComponent<Transform>().Find("first blog").GetComponent<Transform>().Find("img").GetComponent<Image>().sprite = Resources.Load <Sprite>(UnitBar.Instance.selectUnit.structure.SpritePath_img) ;
        this.GetComponent<Transform>().Find("pop hub stage1").GetComponent<Transform>().Find("first blog").GetComponent<Transform>().Find("stat").GetComponent<Transform>().Find("Text").GetComponent<Text>().text = string.Format("HP : {0} / {1} {2}ATK : {3} + {4} {5}DEF : {6} + {7} {8}Movement : {9} + {10} {11}ATK Range : {12} + {13}", UnitBar.Instance.selectUnit.structure.Hp -UnitBar.Instance.selectUnit.Damage, UnitBar.Instance.selectUnit.structure.Hp, System.Environment.NewLine, UnitBar.Instance.selectUnit.structure.Atk, UnitBar.Instance.selectUnit.atkbuff+UnitBar.Instance.selectUnit.passiveatk, System.Environment.NewLine, UnitBar.Instance.selectUnit.structure.Def, UnitBar.Instance.selectUnit.defbuff+UnitBar.Instance.selectUnit.passivedef,System.Environment.NewLine, UnitBar.Instance.selectUnit.structure.Movement, UnitBar.Instance.selectUnit.movementbuff,System.Environment.NewLine, UnitBar.Instance.selectUnit.structure.Atkrange, UnitBar.Instance.selectUnit.atkrangebuff);
    }
}
