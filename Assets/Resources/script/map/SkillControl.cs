using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillControl : MonoBehaviour {

    public static SkillControl Instance;
    public List<skillStructure> skills;

    private void Awake()
    {
        Instance = this;

        this.transform.Find("skill1").localScale = new Vector3(0, 1, 1);
        this.transform.Find("skill2").localScale = new Vector3(0, 1, 1);
    }

    private void Update()
    {
        if(UnitBar.Instance.selectUnit == null)
        {
            skills.Clear();
            this.transform.Find("skill1").localScale = new Vector3(0, 1, 1);
            this.transform.Find("skill2").localScale = new Vector3(0, 1, 1);
        }
    }

    public void showSkillList()
    {
        if (UnitBar.Instance.selectUnit == null) return;

        for (int i = 1; i <= 2; i++)
        {
            if (UnitBar.Instance.selectUnit.structure.Skill[i - 1].SkillName != "-")
            {
                skills.Add(UnitBar.Instance.selectUnit.structure.Skill[i-1]);
                Instance.transform.Find("skill" + i).transform.Find("Text").GetComponent<Text>().text = skills[i - 1].SkillName + System.Environment.NewLine + "Can use in turn " + UnitBar.Instance.selectUnit.cooldownSkill[i - 1];
                Instance.transform.Find("skill" + i).localScale = new Vector3(1, 1, 1);
            }
        }

    }

    public void useSkill(int num)
    {
        if (UnitBar.Instance.selectUnit.cooldownSkill[num] != 0) return;
        UnitBar.Instance.selectSkill = skills[num];
        UnitBar.Instance.setSkill();
    }
}
