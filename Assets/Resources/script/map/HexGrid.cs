using UnityEngine;
using UnityEngine.UI;

public class HexGrid : Photon.MonoBehaviour {

    private PhotonView PhotonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    int width = 6;
    int height = 6;

    public HexCell cellPrefab;
    HexCell[] cells;
    
    int hexFilterSize = 0;
    HexCoordinates hexFilterCoordinate;
    public HexFilter hexFilterPrefab;
    HexFilter hexFilter;

    HexMesh hexMesh;

    Sprite[] maps;

    void Awake(){

        hexFilter = null;
        hexMesh = GetComponentInChildren<HexMesh>();
        maps = Resources.LoadAll<Sprite>("sprite/map");
        PhotonView = GetComponent<PhotonView>();

        Debug.Log(hexMesh.name);

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

    }

    void Start(){
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i){

        if (x == z) return;

        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = - z - ((float) i % 4 / 4f );
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab); //cell is referent to cells[]
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.setType(i % 4);
        cell.setMap(maps[cell.mapType]);
    }

    void Update () {
        
        //if (Input.GetMouseButtonUp(0)) {
        //    PhotonView.RPC("HandleInput", PhotonTargets.All);
        //    //HandleInput();
        //    //Debug.Log("x is" + getTouchCoordinate().x + "y is" + getTouchCoordinate().y + "z is" + getTouchCoordinate().z);
        //}
        if (hexFilterSize != 0){
            setHexFilter(hexFilterCoordinate,hexFilterSize);
        }
    }
    [PunRPC]
    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            TouchCell(hit.point);
        }
    }

    //use send position cross function
    public HexCoordinates getTouchCoordinate() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(inputRay, out hit);
        return HexCoordinates.FromPosition(hit.point);
    }

    void TouchCell (Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinate = HexCoordinates.FromPosition(position);
        hexFilterSize = 1;
        hexFilterCoordinate = coordinate;
    }

    public void setHexFilter(HexCoordinates coordinate, int size) {
        if (hexFilter != null) clearHexFilter();
        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);
        hexFilter.setHexFilterSize(size);
        

        for (int i = 0, x = -size; x <= size; x++) {
            int iX = x + coordinate.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-size, -x - size); y <= Mathf.Min(size, -x + size); y++) {
                int iY = y + coordinate.Y;
                
                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                int index = iX + (iY + iX / 2) * width;

                if (cells[index] != null)
                    hexFilter.setCoordinate(HexCoordinates.FromOffsetCoordinates(iX, iY + iX/ 2),i);
                
                i++;
            }
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting)
        {
            if (hexFilter != null)
                stream.SendNext(hexFilter.transform.position);
        }
        else
        {
            if (hexFilter != null)
                TargetPosition = (Vector3)stream.ReceiveNext();
        }
    }

    public void clearHexFilter() {
        Destroy(hexFilter.gameObject);
        hexFilterSize = 0;
        
    }

}
