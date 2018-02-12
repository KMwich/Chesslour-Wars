using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour {

    public HexCell cellPrefab;
    public HexCell[] cells;

    public int width;
    public int height;
    
    HexCoordinates hexFilterCoordinate;
    public HexFilter hexFilterPrefab;
    public HexFilter hexFilter;

    HexMesh hexMesh;

    public static JsonMap mapDetail;
    Sprite[] maps;

    void Awake(){

        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);

        hexMesh = GetComponentInChildren<HexMesh>();
        maps = Resources.LoadAll<Sprite>("sprite/map");

        string json = File.ReadAllText("Assets/Resources/database/stage1.json");
        mapDetail = JsonUtility.FromJson<JsonMap>(json);
        width = mapDetail.width;
        height = mapDetail.height;

        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < mapDetail.height; y++)
        {
            for (int x = 0; x < mapDetail.width; x++)
            {
                CreateCell(x, y, i++);
            }
        }


    }

    void Start(){
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int y, int i){

        if (mapDetail.detail[i] == 0) return;

        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = - y - ((float) i % 4 / 4f );
        position.z = (y + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab); //cell is referent to cells[]
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.setType(mapDetail.detail[i] % 4);
        cell.setMap(maps[cell.mapType]);

        if (cell.mapType == 1 || cell.mapType == 2) return;

        if (PhotonNetwork.isMasterClient) {
            if (cell.coordinates.Y < mapDetail.area[0])
                hexFilter.setFilter(cell.coordinates, 1);
        } else {
            if (cell.coordinates.Y > mapDetail.area[1])
                hexFilter.setFilter(cell.coordinates, 1);
        }
    }

    //use send position cross function
    public HexCoordinates getTouchCoordinate() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(inputRay, out hit);
        return HexCoordinates.FromPosition(hit.point);
    }

    public void setSkillFilter(HexCoordinates coordinate, int size, int action) {
        if (hexFilter != null) clearHexFilter();
        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);
        
        for (int x = -size; x <= size; x++) {
            int iX = x + coordinate.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-size, -x - size); y <= Mathf.Min(size, -x + size); y++) {
                int iY = y + coordinate.Y;
                
                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                int index = iX + (iY + iX / 2) * width;

                if (cells[index] != null && (cells[index].mapType != 1 && cells[index].mapType != 2))
                    hexFilter.setFilter(HexCoordinates.FromOffsetCoordinates(iX, iY + iX/ 2), action);
            }
        }
    }

    public void setAttackFilter(HexCoordinates coordinate, int size) {
        if (hexFilter != null) clearHexFilter();
        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);

        for (int x = -size; x <= size; x++) {
            int iX = x + coordinate.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-size, -x - size); y <= Mathf.Min(size, -x + size); y++) {
                int iY = y + coordinate.Y;
                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                Debug.Log(HexCoordinates.cubeDeistance(coordinate, HexCoordinates.FromOffsetCoordinates(iX, iY + iX / 2)));

                if (HexCoordinates.cubeDeistance(coordinate, HexCoordinates.FromOffsetCoordinates(iX, iY + iX / 2)) != size) continue;
                int index = iX + (iY + iX / 2) * width;

                if (cells[index] != null && (cells[index].mapType != 1 && cells[index].mapType != 2))
                    hexFilter.setFilter(HexCoordinates.FromOffsetCoordinates(iX, iY + iX / 2), 2);
            }
        }
    }

    public List<HexCoordinates> setMoveFilter(HexCoordinates coordinate, int size) {
        if (hexFilter != null) clearHexFilter();
        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);

        List<HexCoordinates> coordinates = canMoveCoordinates(coordinate, 0, size);

        for (int i = 0; i < coordinates.Count; i++) {
            hexFilter.setFilter(coordinates[i], 1);
        }

        return coordinates;
    }

    public List<HexCoordinates> canMoveCoordinates(HexCoordinates coordinate, int start , int stop) {
        HexCoordinates[] n = HexCoordinates.neighbor(coordinate);
        List<HexCoordinates> coordinates = new List<HexCoordinates>();
        if (start == stop) {
            coordinates.Add(coordinate);
        } else {
            for (int i = 0; i < n.Length; i++) {
                Vector3Int offset = HexCoordinates.cubeToOffset(n[i]);
                if (offset.x < 0 || offset.x >= width || offset.z < 0 || offset.z >= height) continue;
                if (cells[offset.x + (offset.z * width)] == null) continue;
                int type = cells[(offset.x + (offset.z * width))].mapType;
                if ((type != 2 && type != 1)) {
                    List<HexCoordinates> tmp = canMoveCoordinates(n[i], start + 1, stop);
                    for (int j = 0; j < tmp.Count; j++) {
                        if (!coordinates.Contains(tmp[j])) {
                            offset = HexCoordinates.cubeToOffset(tmp[j]);
                            if (offset.x < 0 || offset.x >= width || offset.z < 0 || offset.z >= height) continue;
                            if (cells[offset.x + (offset.z * width)] == null) continue;
                            offset = HexCoordinates.cubeToOffset(tmp[j]);
                            type = cells[(int)(offset.x + (offset.z * width))].mapType;
                            if (type != 2 && type != 1)
                                coordinates.Add(tmp[j]);
                        }
                    }
                }
            }
        }

        return coordinates;
    }

    public void clearHexFilter() {
        if (hexFilter == null) return;
        Destroy(hexFilter.gameObject);
        hexFilter = null;
    }
}
