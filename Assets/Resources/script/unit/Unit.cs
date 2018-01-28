using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Photon.MonoBehaviour
{
    HexCoordinates coordinate;
    //   public HexGrid map;
    int attackRange = 1;
    bool move = false;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private PhotonView PhotonView;
    
    private Unit Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        Vector3 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector3((S.x / 2), 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonView.isMine)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(inputRay, out hit);
                Debug.Log(hit.point);
                if (move)
                {
                    int dist = HexCoordinates.cubeDeistance(coordinate, UnitManager.Instance.HexGrid.getTouchCoordinate());
                    if (dist != 0 && dist <= attackRange)
                        setUnitPosition(HexCoordinates.cubeToOffset(UnitManager.Instance.HexGrid.getTouchCoordinate()));
                }
                if (coordinate.Equals(UnitManager.Instance.HexGrid.getTouchCoordinate()))
                {
                    UnitManager.Instance.HexGrid.setHexFilter(coordinate, 1);
                    move = true;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                UnitManager.Instance.HexGrid.clearHexFilter();
                move = false;
            }
        }

    }

    public void setUnitPosition(Vector3 position)
    {
        coordinate = HexCoordinates.FromOffsetCoordinates((int)position.x, (int)position.z);
        position.x = position.x * (HexMetrics.outerRadius * 1.5f);
        position.y = (position.z + position.x * 0.5f - (int)position.x / 2) * (HexMetrics.innerRadius * 2f); 
        position.z = transform.localPosition.z;
        transform.localPosition = position;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void OnMouseDrag() {
        Vector3 dist = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }

    private void OnMouseUp() {
        HexCoordinates hexPosition = GetComponentInParent<UnitBar>()._hexGrid.getTouchCoordinate();
        Vector3 position = HexCoordinates.cubeToOffset(hexPosition);
        setUnitPosition(position);
    }

    public void setUnitSprite(string path) {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }
}
