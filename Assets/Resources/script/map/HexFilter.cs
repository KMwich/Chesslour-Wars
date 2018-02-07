using System.Collections.Generic;
using UnityEngine;

public class HexFilter : MonoBehaviour {
    List<HexCell> cells = new List<HexCell>();
    public HexCell cellPrefab;
    Sprite filter;

    private void Awake() {
        filter = Resources.Load<Sprite>("sprite/mapfilter/CBzone");
    }

    public void setCoordinate(HexCoordinates coordinate, int index) {
        Vector3 position = HexCoordinates.cubeToOffset(coordinate);
        position.x = position.x * HexMetrics.outerRadius * 1.5f;
        position.y = 0.1f;
        position.z = (position.z + position.x * 0.5f - (int) position.x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.GetComponent<SpriteRenderer>().sprite = filter;
        cell.GetComponent<SpriteRenderer>().color = new Vector4(1,1,1,0.5f);
    }
}
