using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillControl : MonoBehaviour {

    public static SkillControl Instance;
    public List<skillStructure> skills;
    private int skillCount = 1;

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
            skillCount = 1;
            skills.Clear();
            this.transform.Find("skill1").localScale = new Vector3(0, 1, 1);
            this.transform.Find("skill2").localScale = new Vector3(0, 1, 1);
        }
    }

    public void showSkillList()
    {
        if (UnitBar.Instance.selectUnit == null) return;

        if (UnitBar.Instance.selectUnit.structure.Skill[0].SkillName != "-")
        {
            skills.Add(UnitBar.Instance.selectUnit.structure.Skill[0]);
            Instance.transform.Find("skill" + skillCount).transform.Find("Text").GetComponent<Text>().text = skills[skillCount - 1].SkillName;
            Instance.transform.Find("skill" + skillCount).localScale = new Vector3(1, 1, 1);
            skillCount++;
        }
        if (UnitBar.Instance.selectUnit.structure.Skill[1].SkillName != "-")
        {
            skills.Add(UnitBar.Instance.selectUnit.structure.Skill[1]);
            Instance.transform.Find("skill" + skillCount).transform.Find("Text").GetComponent<Text>().text = skills[skillCount - 1].SkillName;
            Instance.transform.Find("skill" + skillCount).localScale = new Vector3(1, 1, 1);
            skillCount++;
        }

    }
}
