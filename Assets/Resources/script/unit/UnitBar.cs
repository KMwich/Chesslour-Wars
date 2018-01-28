using UnityEngine;

public class UnitBar : MonoBehaviour {
    public HexGrid _hexGrid;

    public Unit unitPrefab;
    Unit[] units;

    private void Awake() {
        units = new Unit[GameManager.mainTypeUnit.Count];

        for (int i = 0; i < units.Length; i++) {
            createUnit(i);
        }
    }

    private void createUnit(int i) {
        Unit unit = units[i] = Instantiate<Unit>(unitPrefab);
        switch (GameManager.mainTypeUnit[i]) {
            case 0:
                unit.setUnitSprite(unit_database.units.Attacker[GameManager.subTypeUnit[i]].SpritePath_img);
                break;
            case 1:
                unit.setUnitSprite(unit_database.units.Supporter[GameManager.subTypeUnit[i]].SpritePath_img);
                break;
            case 2:
                unit.setUnitSprite(unit_database.units.Sturture[GameManager.subTypeUnit[i]].SpritePath_img);
                break;
            case 3:
                unit.setUnitSprite(unit_database.units.Trap[GameManager.subTypeUnit[i]].SpritePath_img);
                break;
        }

        if ((GameManager.mainTypeUnit[i] == 0 && GameManager.subTypeUnit[i] == 2) || (GameManager.mainTypeUnit[i] == 1 && GameManager.subTypeUnit[i] == 0)) {
            unit.transform.localScale = new Vector3(7, 7, 1);
        }


        Vector3 position;
        position.x = i*10;
        position.y = 2;
        position.z = 0;

        unit.transform.localPosition = position;
        unit.transform.SetParent(transform);
    }

    public void ready() {
        Debug.Log(true);
    }
}
