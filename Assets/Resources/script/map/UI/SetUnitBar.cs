using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUnitBar : MonoBehaviour { 
    public GameObject content;
    public GameObject slotPrefab;

    public static SetUnitBar Instance;

    private List<int> receive_mainTypeUnit;
    private List<int> receive_subTypeUnit;
    private List<GameObject> slot;
    private List<bool> unitIsSet;

    private void Awake()
    {
        Instance = this;
        receive_mainTypeUnit = GameManager.Instance.MainTypeUnit;
        receive_subTypeUnit = GameManager.Instance.SubTypeUnit;
        slot = new List<GameObject>();
        unitIsSet = new List<bool>();
        for (int i = 0; i < receive_mainTypeUnit.Count; i++)
        {
            if (receive_mainTypeUnit[i] == 0)
            {
                slot.Add(Instantiate(slotPrefab));
                Sprite img = Resources.Load<Sprite>(unit_database.units.Attacker[receive_subTypeUnit[i]].SpritePath_img);
                slot[i].GetComponent<Image>().sprite = img;
                slot[i].transform.SetParent(content.transform);
            }
            else if (receive_mainTypeUnit[i] == 1)
            {
                slot.Add(Instantiate(slotPrefab));
                Sprite img = Resources.Load<Sprite>(unit_database.units.Supporter[receive_subTypeUnit[i]].SpritePath_img);
                slot[i].GetComponent<Image>().sprite = img;
                slot[i].transform.SetParent(content.transform);
            }
            else if (receive_mainTypeUnit[i] == 2)
            {
                slot.Add(Instantiate(slotPrefab));
                Sprite img = Resources.Load<Sprite>(unit_database.units.Sturture[receive_subTypeUnit[i]].SpritePath_img);
                slot[i].GetComponent<Image>().sprite = img;
                slot[i].transform.SetParent(content.transform);
            }
            else
            {
                slot.Add(Instantiate(slotPrefab));
                Sprite img = Resources.Load<Sprite>(unit_database.units.Trap[receive_subTypeUnit[i]].SpritePath_img);
                slot[i].GetComponent<Image>().sprite = img;
                slot[i].transform.SetParent(content.transform);
            }
            slot[i].GetComponent<SetUnitBar_Slot>().UnitIndex = i;
            unitIsSet.Add(false); //unit all not set
        }
    }

    public void deployUnit(int index)
    {
        UnitBar.Instance.setSelectUnit(index);
        unitIsSet[index] = true;
        updateList();
    }

    public void cantSetUnit(int i) {
        unitIsSet[i] = false;
        updateList();
    }

    private void updateList()
    { 
        foreach (GameObject obj in slot)
        {
            Destroy(obj);
        }
        slot.Clear();
        for (int i = 0; i < unitIsSet.Count; i++) {
            if (unitIsSet[i]) {
                continue;
            } else {
                GameObject button = Instantiate(slotPrefab);
                if (receive_mainTypeUnit[i] == 0) {
                    Sprite img = Resources.Load<Sprite>(unit_database.units.Attacker[receive_subTypeUnit[i]].SpritePath_img);
                    button.GetComponent<Image>().sprite = img;
                } else if (receive_mainTypeUnit[i] == 1) {
                    Sprite img = Resources.Load<Sprite>(unit_database.units.Supporter[receive_subTypeUnit[i]].SpritePath_img);
                    button.GetComponent<Image>().sprite = img;
                } else if (receive_mainTypeUnit[i] == 2) {
                    Sprite img = Resources.Load<Sprite>(unit_database.units.Sturture[receive_subTypeUnit[i]].SpritePath_img);
                    button.GetComponent<Image>().sprite = img;
                } else {
                    Sprite img = Resources.Load<Sprite>(unit_database.units.Trap[receive_subTypeUnit[i]].SpritePath_img);
                    button.GetComponent<Image>().sprite = img;
                }
                button.transform.SetParent(content.transform);
                button.GetComponent<SetUnitBar_Slot>().UnitIndex = i;
                slot.Add(button);
            }
        }
    }
}
