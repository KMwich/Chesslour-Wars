using UnityEngine;

public class HexFilter : MonoBehaviour {
    HexCell[] cells;
    public HexCell cellPrefab;
    Sprite filter;

    public void setHexFilterSize (int size) {
        filter = Resources.Load<Sprite>("sprite/mapfilter/CBzone");
        int index = 1;
        for (int i = 1;i <= size; i++) {
            index += i * 6;
        }
        cells = new HexCell[index];
    }

    public void setCoordinate(HexCoordinates coordinate, int index) {
        Vector3 position = HexCoordinates.cubeToOffset(coordinate);
        position.y = 0.1f;
        
        HexCell cell = cells[index] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.GetComponent<SpriteRenderer>().sprite = filter;
    }
}
