using UnityEngine;

public class UnitBar : MonoBehaviour {
    public HexGrid _hexGrid;

    public Unit unitPrefab;
    Unit[] units;
    Unit selectUnit;

    private void Awake() {
        units = new Unit[5];

        for (int i = 0; i < units.Length; i++) {
            createUnit(i);
        }
    }

    private void createUnit(int i) {
        Unit unit = units[i] = Instantiate<Unit>(unitPrefab);

        Vector3 position;
        position.x = i;
        position.y = 2;
        position.z = -1;

        unit.setUnitPosition(position);
        unit.transform.SetParent(transform);
    }

    public void setSelectUnit() {
        selectUnit = this.gameObject.GetComponent<Unit>();
    }

    private void Update() {
        if (selectUnit != null)
            selectUnit.transform.localPosition = Input.mousePosition;
    }
}
